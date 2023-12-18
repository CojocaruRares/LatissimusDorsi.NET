import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import CreateAccount from './components/AuthLogic/CreateAccount';
import UserAccount from './components/AuthLogic/UserAccount';
import LoginForm from './components/AuthLogic/Login';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginForm />} />
                <Route path="/CreateAccount" element={<CreateAccount/>}/>
                <Route path="/UserAccount" element={<UserAccount />} />
            </Routes>
        </Router>
    );
}

export default App;
