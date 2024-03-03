
import { useState, useEffect } from 'react';
import { API_URL_USER } from '../../utils/api_url';
import axios from 'axios';
import { auth } from '../../utils/firebase-config';


const WorkoutPlan = () => {
    const [workout, setWorkout] = useState({});
    const user = auth.currentUser;

    useEffect(() => {
        const fetchWorkout = async () => {
            try {
                const response = await axios.get(`${API_URL_USER}/Workout`, {
                    params: { id: user.uid },
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setWorkout(response.data);             
            }
            catch (error) {
                console.log("Exception: ", error);
            }
        };
        fetchWorkout();
    }, [user]);

    const sendWorkout = async () => {
        try {
            const response = await axios.post(`${API_URL_USER}/Workout`, workout, {
                params: { email: user.email },
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            console.log(response);
        }
        catch (error) {
            console.log("Exception: ", error);
        }
    }

    return (
        <div className="container mt-4">
            {workout && Object.keys(workout).length > 0 ? (
                <div>
                    <h2>{workout.title}</h2>
                    {Object.keys(workout.exercises).map((day, index) => (
                        <div key={index}>
                            {workout.exercises[day][0].name !== "RestDay" && (
                                <div className='workout'>
                                    <h4>{day}</h4>
                                    <table className="table">
                                        <thead>
                                            <tr>
                                                <th scope="col">Exercise</th>
                                                <th scope="col">Sets</th>
                                                <th scope="col">Reps</th>
                                                <th scope="col">RPE</th>
                                                <th scope="col">Description</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {workout.exercises[day].map((exercise, exerciseIndex) => (
                                                <tr key={exerciseIndex}>
                                                    <td>{exercise.name}</td>
                                                    <td>{exercise.sets}</td>
                                                    <td>{exercise.reps}</td>
                                                    <td>{exercise.rpe}</td>
                                                    <td>{exercise.description}</td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            )}
                        </div>
                    ))}
                    <button className="btn btn-lg btn-primary position-fixed bottom-0 end-0 m-4" onClick={sendWorkout}>Send workout to email</button>
                </div>
            ) : (
                <p>Loading...</p>
            )}
        </div>
    );
};

export default WorkoutPlan;