import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

import CreateAccount from './components/AuthLogic/CreateAccount';
import UserAccount from './components/AuthLogic/UserAccount';
import TrainerAccount from './components/AuthLogic/TrainerAccount';
import LoginForm from './components/AuthLogic/Login';
import Home from './Home';
import UserProfile from './components/UserLogic/UserProfile';
import UserNavbar from './components/UserLogic/UserNavbar';
import { useState } from 'react';
import { auth } from './utils/firebase-config';
import { onAuthStateChanged } from 'firebase/auth';

function App() {
    const [isLogged, SetLog] = useState(false);
    onAuthStateChanged(auth, (user) => {
        if (user) {
            SetLog(true);
        } else {
            SetLog(false);
        }
    });
   
    return (
        <Router>
            {isLogged && < UserNavbar />}
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
            </Routes>
        </Router>
    );
}

export default App;
