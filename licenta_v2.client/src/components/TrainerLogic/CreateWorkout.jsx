import WorkoutDay from "./WorkoutDay";


const CreateWorkout = () => {
    return (
        <div>
            <WorkoutDay currentDay="Monday" />
            <WorkoutDay currentDay="Tuesday" />
            <WorkoutDay currentDay="Wednesday" />
            <WorkoutDay currentDay="Thursday" />
            <WorkoutDay currentDay="Friday" />
            <WorkoutDay currentDay="Saturday" />
            <WorkoutDay currentDay="Sunday" />
        </div>
    );
}
export default CreateWorkout;