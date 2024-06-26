import { useState } from 'react';
import { auth } from '../../utils/firebase-config';
import { signInWithEmailAndPassword } from 'firebase/auth';
import { useNavigate } from 'react-router-dom';
import Alert from '@mui/material/Alert';



const LoginForm = () => {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isFail, setIsFail] = useState(false);

    const handleEmailChange = (e) => {
        setEmail(e.target.value);
    };

    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
    };

    const handleLogin = (e) => {
        e.preventDefault();
        signInWithEmailAndPassword(auth, email, password)
            .then((userCredential) => {
                const user = userCredential.user;
                console.log(user.accessToken);
                navigate('/Home');
            })
            .catch((error) => {
                const errorCode = error.code;
                const errorMessage = error.message;
                console.log(errorCode, errorMessage);
                setIsFail(true);
            });
    };

    return (
        <div>
            <div className="flex-login">
                <div>
                    <h2>Sign in</h2>
                    <form>
                        <div className="mb-3">
                            <label htmlFor="email" className="form-label">Email:</label>
                            <input
                                type="email"
                                className="form-control"
                                id="email"
                                value={email}
                                onChange={handleEmailChange}
                            />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="password" className="form-label">Password:</label>
                            <input
                                type="password"
                                className="form-control"
                                id="password"
                                value={password}
                                onChange={handlePasswordChange}
                            />
                        </div>
                        <button className="btn btn-primary" onClick={handleLogin}>Login</button>
                        <p className="mt-3"><a href='/CreateAccount'>Don&apos;t have an account ?</a></p>
                    </form>
                </div>
            </div>
            {
                isFail && <Alert variant="outlined" severity="error" onClose={() => setIsFail(false)}
                    sx={{
                        color: 'red', width: '40vw', margin: 'auto', position: 'absolute',
                        top: '20px',
                        left: '50%',
                        transform: 'translateX(-50%)',
                        zIndex: '999'
                    }}
                >Please enter valid data !</Alert>
            }
        </div>
    );
};

export default LoginForm;
