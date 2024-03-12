import { Link } from 'react-router-dom';
import './UserNavbar.css'; 


const UserNavbar = () => {
   
    return (
        <nav  >
            <div className="logo">LatissimusDorsi.NET</div>
            <ul className="menu">
                <li><Link className="menu-link" to="/Home">Home</Link></li>
                <li><Link className="menu-link" to="/UserProfile">Profile</Link></li>
                <li><Link className="menu-link" to="/WorkoutPlan">Workout Plan</Link></li>
                <li><Link className="menu-link" to="/TrainingSessions">Training Sessions</Link></li>
            </ul>
        </nav>
    );
};

export default UserNavbar;
