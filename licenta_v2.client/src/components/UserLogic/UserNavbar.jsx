import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import './UserNavbar.css'; 


const UserNavbar = () => {
    const [menuOpen, setMenuOpen] = useState(false);
    return (
        <nav>
            <div className="title-logo">LatissimusDorsi.NET</div>
            <div className="menu" onClick={() => setMenuOpen(!menuOpen)}>
                <span></span>
                <span></span>
                <span></span>
            </div>
            <ul className={menuOpen ? "open nav-items" : "nav-items"}>
                <li><NavLink  to="/Home">Home</NavLink></li>
                <li><NavLink  to="/UserProfile">Profile</NavLink></li>
                <li><NavLink  to="/WorkoutPlan">Workout Plan</NavLink></li>
                <li><NavLink to="/TrainingSessions">Training Sessions</NavLink></li>
            </ul>
        </nav>
    );
};

export default UserNavbar;
