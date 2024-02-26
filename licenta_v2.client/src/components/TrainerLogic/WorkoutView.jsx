import PropTypes from 'prop-types';

const WorkoutView = ({ workout }) => {
    return (
        <div className="container mt-4">
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
        </div>
    );
};

WorkoutView.propTypes = {
    workout: PropTypes.shape({
        title: PropTypes.string.isRequired,
        exercises: PropTypes.objectOf(
            PropTypes.arrayOf(
                PropTypes.shape({
                    name: PropTypes.string.isRequired,
                    sets: PropTypes.number.isRequired,
                    reps: PropTypes.number.isRequired,
                    rpe: PropTypes.number.isRequired,
                    description: PropTypes.string.isRequired
                })
            )
        ).isRequired
    }).isRequired
};

export default WorkoutView;
