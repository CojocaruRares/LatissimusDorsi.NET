import WorkoutDay from "./WorkoutDay";
import { useState, useEffect } from 'react';
import './Workout.css';
import axios from 'axios';
import { API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import { useNavigate } from 'react-router-dom';
import Alert from '@mui/material/Alert';

const CreateWorkout = () => {
    const navigate = useNavigate();
    const [workoutTitle, setWorkoutTitle] = useState("");
    const [workoutData, setWorkoutData] = useState({
        Monday: { name: '', sets: null, reps: null, rpe: null, description: '' },
        Tuesday: { name: '', sets: null, reps: null, rpe: null, description: '' },
        Wednesday: { name: '', sets: null, reps: null, rpe: null, description: '' },
        Thursday: { name: '', sets: null, reps: null, rpe: null, description: '' },
        Friday: { name: '', sets: null, reps: null, rpe: null, description: '' },
        Saturday: { name: '', sets: null, reps: null, rpe: null, description: '' },
        Sunday: { name: '', sets: null, reps: null, rpe: null, description: '' },
    });
    const [isFail, setIsFail] = useState(false);
    const [token, setToken] = useState(null);
    const [uid, setUid] = useState(''); 
    const user = auth.currentUser;
    useEffect(() => {
        if (user) {
            user.getIdToken().then((token) => {
                setToken(token);
                setUid(user.uid);
            }).catch((error) => {
                console.error('Error fetching token:', error);
            });
        }
    }, [user]);

    const handleTitleChange = (e) => {
        setWorkoutTitle(e.target.value);
    };

    const updateWorkoutData = (day, newData) => {
        setWorkoutData(prevData => ({
            ...prevData,
            [day]: newData,
        }));
        console.log(workoutData);
    };

    const saveWorkout = async () => {
        try {
            const updatedWorkoutData = {};
            let calculated_intensity = 'low';
            let intensity_value = 0;
           
            Object.keys(workoutData).forEach(day => {
                const dayExercises = workoutData[day];              
                const hasExercises = Object.values(dayExercises).some(value => value !== '' && value !== null);
                updatedWorkoutData[day] = hasExercises ? dayExercises : [{ name: 'RestDay', sets: null, reps: null, rpe: null, description: '' }];
                updatedWorkoutData[day].forEach(exercise => {
                    if (exercise.rpe !== null && exercise.sets !== null) {
                        intensity_value += exercise.rpe * exercise.sets;
                    }
                });
            });

            if (intensity_value >= 100 && intensity_value <= 250)
                calculated_intensity = 'moderate';
            else if (intensity_value > 250)
                calculated_intensity = 'high';


            const workoutToSend = {
                title: workoutTitle,
                intensity: calculated_intensity,
                exercises: updatedWorkoutData
            };

            await axios.post(`${API_URL_TRAINER}/${uid}/workout`, workoutToSend, {
                headers: {
                    Authorization: 'Bearer ' + token,
                }
 
            });
            console.log("Workout saved successfully!");
            navigate('/WorkoutList');
        } catch (error) {
            console.error("Error saving workout:", error);
            setIsFail(true);
        }
    };
   
    return (
        <div >
            <div className="work-title">
                <div className='d-flex flex-column'>
                <label htmlFor="workoutTitle">Workout Title</label>
                <input
                    type="text"
                    className="form-control"
                    id="workoutTitle"
                    placeholder="Enter workout title"
                    value={workoutTitle}
                    onChange={handleTitleChange}
                    />
                </div>
            </div>

            {Object.keys(workoutData).map(day => (
                <WorkoutDay
                    key={day}
                    currentDay={day}
                    setData={updateWorkoutData}               
                />
            ))}
            <div className="btn-save">
                <button className="btn btn-primary" onClick={saveWorkout} >Save Workout</button>
            </div>
            {
                isFail && <Alert variant="outlined" severity="error" onClose={() => {setIsFail(false)}}
                    sx={{
                        color: 'red', width: '40vw', margin: 'auto', 
                        marginBottom: '30px',
                        zIndex: '999'
                    }}
                >Please enter valid data !</Alert>
            }
        </div>
    );
}
export default CreateWorkout;