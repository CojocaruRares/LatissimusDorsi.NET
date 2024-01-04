import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PropTypes from 'prop-types';
import CreateAccount from './components/AuthLogic/CreateAccount';
import UserAccount from './components/AuthLogic/UserAccount';
import TrainerAccount from './components/AuthLogic/TrainerAccount';
import TrainerProfile from './components/TrainerLogic/TrainerProfile';
import CreateWorkout from './components/TrainerLogic/CreateWorkout';
import LoginForm from './components/AuthLogic/Login';
import Home from './Home';
import UserProfile from './components/UserLogic/UserProfile';
import UserNavbar from './components/UserLogic/UserNavbar';
import TrainerNavbar from './components/TrainerLogic/TrainerNavbar';
import { useState, useEffect } from 'react';
import { auth } from './utils/firebase-config';
import { onAuthStateChanged } from 'firebase/auth';

function App() {
    const [isLogged, SetLog] = useState(false);
    const [userRole, setUserRole] = useState('');
    onAuthStateChanged(auth, (user) => {
        if (user) {
            SetLog(true);
        } else {
            SetLog(false);
        }
    });
    useEffect(() => {
        const getUserRole = async () => {
            try {
                const currentUser = auth.currentUser;
                if (currentUser) {
                    const idTokenResult = await currentUser.getIdTokenResult();
                    setUserRole(idTokenResult.claims.role);
                }
            } catch (error) {
                console.log(error);
            }
        };
        getUserRole();
    }, [isLogged]); 

    function Navbar(props) {
        const isLoggedIn = props.isLoggedIn;
        const role = props.role;
        console.log(role);
        console.log(role);
        if (isLoggedIn && role === "user") {
            return <UserNavbar />;
        }
        else if (isLoggedIn && role === "trainer") {
            return <TrainerNavbar />;
        }
        else return;

    }
    Navbar.propTypes = {
        isLoggedIn: PropTypes.bool.isRequired,
        role: PropTypes.oneOf(['user', 'trainer']).isRequired
    };
    

    return (
        <Router>
            <Navbar isLoggedIn={isLogged} role={userRole} />
            <Routes>
                {isLogged ? (
                    <Route path="/" element={<Home />} />
                ) : (
                    <Route path="/" element={<LoginForm />} />
                )}
                <Route path="/Home" element={<Home />} />
                <Route path="UserProfile" element={<UserProfile/> }/>
                <Route path="/CreateAccount" element={<CreateAccount/>}/>
                <Route path="/UserAccount" element={<UserAccount />} />
                <Route path="/TrainerAccount" element={<TrainerAccount />} />
                <Route path="/TrainerProfile" element={<TrainerProfile />} />
                <Route path="/CreateWorkout" element={<CreateWorkout/> } />
            </Routes>
        </Router>
    );
}

export default App;
