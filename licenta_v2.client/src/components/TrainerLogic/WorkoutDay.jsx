import { useState } from 'react';
import './Workout.css'
import PropTypes from 'prop-types';
const WorkoutDay = ({currentDay, setData}) => {
    const [exercises, setExercises] = useState([]);


    const handleInputChange = (index, event) => {
        const { name, value } = event.target;
        const newExercises = [...exercises];
        newExercises[index][name] = value;
        setExercises(newExercises);
        setData(currentDay, newExercises);  
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

    const deleteExercise = (index) => {
        const newExercises = exercises.filter((_, i) => i !== index);
        setExercises(newExercises);
        setData(currentDay, newExercises);
    };

    return (
        <div>
            <div className="d-flex align-items-center mb-3">
                <h3 className="mr-3 title">{currentDay}</h3>              
                {exercises.length === 0 && <h4>&nbsp;- Rest Day</h4>}
                <button className="btn btn-primary btn-add-work" onClick={addExercise}>
                    Add Exercise
                </button>            
            </div>
            <table className="table">
                <thead>
                    {exercises.length !== 0 &&
                        <tr>
                            <th>Name</th>
                            <th>Sets</th>
                            <th>Reps</th>
                            <th>RPE</th>
                            <th>Description</th>
                            <th></th>
                        </tr>
                    }
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
                                    required
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
                            <td>
                                <button
                                    className="btn btn-danger"
                                    onClick={() => deleteExercise(index)}
                                >
                                    Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

WorkoutDay.propTypes = {
    currentDay: PropTypes.string.isRequired,
    setData: PropTypes.func.isRequired,
};

export default WorkoutDay;
