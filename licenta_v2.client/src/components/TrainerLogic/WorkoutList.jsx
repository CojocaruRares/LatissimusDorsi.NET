import { useState, useEffect } from 'react';
import './Workout.css';
import WorkoutView from './WorkoutView';
import axios from 'axios';
import { API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import { useNavigate } from 'react-router-dom';

const WorkoutList = () => {
    const navigate = useNavigate();
    const [selectedWorkout, setSelectedWorkout] = useState(null)
    const [workouts, setWorkouts] = useState({});
    const [disableBack, setDisableBack] = useState(true);
    const user = auth.currentUser;

    useEffect(() => {
        const fetchWorkout = async () => {
            try {
                const response = await axios.get(`${API_URL_TRAINER}/${user.uid}/workouts`, {
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setWorkouts(response.data.resource);
            }
            catch (error) {
                console.log("Exception: ", error);
            }
        };
        fetchWorkout();
    }, [user]);

    const deleteWorkout = async (workoutIndex) => {
        try {
            await axios.delete(`${API_URL_TRAINER}/${user.uid}/workout/${workoutIndex}`, {
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            
            const response = await axios.get(`${API_URL_TRAINER}/${user.uid}/workouts`, {
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            setWorkouts(response.data.resource);
        } catch (error) {
            console.log("Exception: ", error);
        }
    }

    const navigateToCreateWorkout = () => {
        navigate('/CreateWorkout');
    }

    const viewWorkout = (workout) => {
        setSelectedWorkout(workout);
        setDisableBack(false);
    }

    const handleBack = () => {
        setSelectedWorkout(null);
        setDisableBack(true);
    }


    return (
        <div>
            <div className='work-title'>
                <button className="btn btn-primary back-button" onClick={handleBack} disabled={disableBack}>Back</button>
                <h2>My Training Programs</h2>
            </div>
            {selectedWorkout ? (
                <WorkoutView workout={selectedWorkout} /> 
            ) : (
                <div>
                    {Object.keys(workouts).length > 0 ? (
                        <ul className='list-group workout-group mb-5'>
                            {Object.values(workouts).map((workout, index) => (
                                <li key={index} className='list-group-item'>
                                    <div className='row align-items-center'>
                                        <div className='col-md-4'>
                                            <h3>{workout.title}</h3>
                                        </div>
                                        <div className='col-md-4 text-center'>
                                            <p className="mb-0">Intensity: {workout.intensity}</p>
                                        </div>
                                        <div className='col-md-4'>
                                            <button className='btn btn-danger float-end btn-delete-workout' onClick={() => deleteWorkout(index)}>Delete</button>
                                            <button className='btn btn-primary float-end' onClick={() => viewWorkout(workout)}>View</button> 
                                        </div>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No workouts available. Create one!</p>
                        )}
                    <div className="d-flex justify-content-center">
                            <button className="btn btn-lg btn-primary  m-4 " onClick={navigateToCreateWorkout}>Create Training Program</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default WorkoutList;