import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import CreateAccount from './components/AuthLogic/CreateAccount';
import UserAccount from './components/AuthLogic/UserAccount';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<CreateAccount />} />
                <Route path="/UserAccount" element={<UserAccount />} />
            </Routes>
        </Router>
    );
}

export default App;
