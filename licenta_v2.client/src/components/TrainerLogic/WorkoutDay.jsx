import { useState } from 'react';
import './Workout.css'
const WorkoutDay = ({currentDay }) => {
    const [exercises, setExercises] = useState([
        {
            name: '',
            sets: null,
            reps: null,
            rpe: null,
            description: '',
        },
    ]);

    const handleInputChange = (index, event) => {
        const { name, value } = event.target;
        const newExercises = [...exercises];
        newExercises[index][name] = value;
        setExercises(newExercises);
    };

    const addExercise = () => {
        setExercises([
            ...exercises,
            {
                name: '',
                sets: null,
                reps: null,
                rpe: null,
                description: '',
            },
        ]);
    };

    return (
        <div>
            <div className="d-flex align-items-center mb-3">
                <h3 className="mr-3 title">{currentDay}</h3>               
                <button className="btn btn-primary" onClick={addExercise}>
                    Add Exercise
                </button>
            </div>
            <table className="table">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Sets</th>
                        <th>Reps</th>
                        <th>RPE</th>
                        <th>Description</th>
                    </tr>
                </thead>
                <tbody>
                    {exercises.map((exercise, index) => (
                        <tr key={index}>
                            <td>
                                <input
                                    type="text"
                                    name="name"
                                    value={exercise.name}
                                    onChange={(event) => handleInputChange(index, event)}
                                    className="form-control"
                                />
                            </td>
                            <td>
                                <input
                                    type="number"
                                    name="sets"
                                    value={exercise.sets || ''}
                                    onChange={(event) => handleInputChange(index, event)}
                                    className="form-control"
                                />
                            </td>
                            <td>
                                <input
                                    type="number"
                                    name="reps"
                                    value={exercise.reps || ''}
                                    onChange={(event) => handleInputChange(index, event)}
                                    className="form-control"
                                />
                            </td>
                            <td>
                                <input
                                    type="number"
                                    name="rpe"
                                    value={exercise.rpe || ''}
                                    onChange={(event) => handleInputChange(index, event)}
                                    className="form-control"
                                />
                            </td>
                            <td>
                                <textarea
                                    name="description"
                                    maxLength={60 }
                                    value={exercise.description}
                                    onChange={(event) => handleInputChange(index, event)}
                                    className="form-control"                                   
                                />
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};


export default WorkoutDay;
