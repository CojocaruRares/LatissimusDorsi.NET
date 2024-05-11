import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PropTypes from 'prop-types';
import CreateAccount from './components/AuthLogic/CreateAccount';
import UserAccount from './components/AuthLogic/UserAccount';
import TrainerAccount from './components/AuthLogic/TrainerAccount';
import TrainerProfile from './components/TrainerLogic/TrainerProfile';
import WorkoutList from './components/TrainerLogic/WorkoutList';
import CreateWorkout from './components/TrainerLogic/CreateWorkout';
import CreateTrainingSession from './components/TrainerLogic/CreateTrainingSession';
import LoginForm from './components/AuthLogic/Login';
import Home from './Home';
import UserProfile from './components/UserLogic/UserProfile';
import UserNavbar from './components/UserLogic/UserNavbar';
import WorkoutPlan from './components/UserLogic/WorkoutPlan';
import TrainingSessions from './components/UserLogic/TrainingSessions';
import TrainerNavbar from './components/TrainerLogic/TrainerNavbar';
import { useState, useEffect } from 'react';
import { auth } from './utils/firebase-config';
import { onAuthStateChanged } from 'firebase/auth';
import TrainingSessionList from './components/TrainerLogic/TrainingSessionList';
import AdminNavbar from './components/AdminLogic/AdminNavbar';
import GetAllUsers from './components/AdminLogic/GetAllUsers';
import GetAllTrainers from './components/AdminLogic/GetAllTrainers';

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
        if (isLoggedIn && role === "user") {
            return <UserNavbar />;
        }
        else if (isLoggedIn && role === "trainer") {
            return <TrainerNavbar />;
        }
        else if (isLoggedIn && role === "admin") {
            return <AdminNavbar />;
        }
        else return;

    }
    Navbar.propTypes = {
        isLoggedIn: PropTypes.bool.isRequired,
        role: PropTypes.string.isRequired
    };
    

    return (
        <Router>
            <Navbar isLoggedIn={isLogged} role={userRole} />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/Home" element={<Home />} />
                <Route path="/Login" element={<LoginForm />} />
                <Route path="UserProfile" element={<UserProfile/> }/>
                <Route path="/CreateAccount" element={<CreateAccount/>}/>
                <Route path="/UserAccount" element={<UserAccount />} />
                <Route path="/TrainerAccount" element={<TrainerAccount />} />
                <Route path="/TrainerProfile" element={<TrainerProfile />} />
                <Route path="/WorkoutList" element={<WorkoutList />} />
                <Route path="/CreateWorkout" element={<CreateWorkout />} />
                <Route path="/WorkoutPlan" element={<WorkoutPlan />} />
                <Route path="/CreateTrainingSession" element={<CreateTrainingSession />} />
                <Route path="/TrainingSessionList" element={<TrainingSessionList /> } />
                <Route path="/TrainingSessions" element={<TrainingSessions />} />
                <Route path="/GetAllUsers" element={<GetAllUsers />} />
                <Route path="/GetAllTrainers" element={<GetAllTrainers />} />
            </Routes>
        </Router>
    );
}

export default App;
