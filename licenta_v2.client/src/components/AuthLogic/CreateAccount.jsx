import { useNavigate } from 'react-router-dom';
import './CustomLogin.css';

const CreateAccount = () => {
    const navigate = useNavigate();

    const handleRoleSelection = (role) => {
        console.log('Selected Role:', role);
        if (role === 'Trainer') {
            navigate('/TrainerAccount');
        } else if (role === 'User') {
            navigate('/UserAccount');
        }
    };

    return (
        <div className="flex-login">
            <div className="role-selection">
                <h2>Which is your role:</h2>
                <div className="button-group">
                    <button onClick={() => handleRoleSelection('Trainer')} className="btn btn-primary">Trainer</button>
                    <button onClick={() => handleRoleSelection('User')} className="btn btn-primary">User</button>
                </div>
            </div>
        </div>
    );
};

export default CreateAccount;
