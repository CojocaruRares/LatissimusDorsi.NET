import { Link } from 'react-router-dom';
import '../UserLogic/UserNavbar.css';


const UserNavbar = () => {

    return (
        <nav >
            <div className="logo">LatissimusDorsi.NET</div>
            <ul className="menu">
                <li><Link className="menu-link" to="/Home">Home</Link></li>
                <li><Link className="menu-link" to="/TrainerProfile">Profile</Link></li>
                <li><Link className="menu-link" to="/WorkoutList">Workout Plan</Link></li>
                <li><Link className="menu-link" to="/CreateTrainingSession">Training Sessions</Link></li>
            </ul>
        </nav>
    );
};

export default UserNavbar;
