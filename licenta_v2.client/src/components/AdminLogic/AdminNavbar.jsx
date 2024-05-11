import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import '../UserLogic/UserNavbar.css';
import { auth } from '../../utils/firebase-config';
import { signOut } from 'firebase/auth';
import { useNavigate } from 'react-router-dom';

const AdminNavbar = () => {
    const [menuOpen, setMenuOpen] = useState(false);
    const navigate = useNavigate();
    const handleLogoutClick = async () => {
        signOut(auth).then(() => {
            if (window.location.pathname === '/' || window.location.pathname === '/Home') {
                window.location.reload();
            } else {
                navigate('/');
            }
        }).catch((error) => {
            console.log(error);
        });
    };

    return (
        <nav>
            <div className="title-logo">LatissimusDorsi.NET</div>
            <div className="menu" onClick={() => setMenuOpen(!menuOpen)}>
                <span></span>
                <span></span>
                <span></span>
            </div>
            <ul className={menuOpen ? "open nav-items" : "nav-items"}>
                <li><NavLink to="/Home">Home</NavLink></li>
                <li><NavLink to="/GetAllUsers">Users</NavLink></li>
                <li><NavLink to="/GetAllTrainers">Trainers</NavLink></li>
                <li><a  onClick={handleLogoutClick}>Log Out</a></li>
            </ul>
        </nav>
    );
};

export default AdminNavbar;
