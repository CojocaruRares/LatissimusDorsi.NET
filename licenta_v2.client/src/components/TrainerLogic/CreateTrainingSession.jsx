import { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import { useNavigate } from 'react-router-dom';
import Alert from '@mui/material/Alert';

const CreateTrainingSession = () => {
    const navigate = useNavigate();
    const [trainingSession, setTrainingSession] = useState({
        trainerId: '',
        users: [],
        title: '',
        startDate: '',
        city: '',
        gym: '',
        slots: 0
    });
    const [isFail, setIsFail] = useState(false);
    const [token, setToken] = useState(null);
    const [uid, setUid] = useState('');
    const user = auth.currentUser;
    useEffect(() => {
        if (user) {
            user.getIdToken().then((token) => {
                setToken(token);
                setUid(user.uid);
            }).catch((error) => {
                console.error('Error fetching token:', error);
            });
        }
    }, [user]);

    const handleChange = (e) => {
        setTrainingSession({ ...trainingSession, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            trainingSession.trainerId = uid;
            const response = await axios.post(`${API_URL_TRAINER}/TrainingSession`, trainingSession, {
                params: { id: uid },
                headers: {
                    Authorization: 'Bearer ' + token,
                }
            });
            console.log('Training session created:', response.data);
            navigate('/TrainingSessionList');
        } catch (error) {
            console.error('Error creating training session:', error);
            setIsFail(true);
        }
    };

    return (
        <div className="container ">
            <div className="row justify-content-center mt-5">
                <div className="col-md-6">
                    <h2 className="text-center mb-4">Create Training Session</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label htmlFor="title" className="form-label">Title:</label>
                            <input type="text" className="form-control" id="title" name="title" value={trainingSession.title} onChange={handleChange} />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="startDate" className="form-label">Start Date and Time</label>
                            <input type="datetime-local" className="form-control" id="startDate" name="startDate" value={trainingSession.startDate} onChange={handleChange} />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="city" className="form-label">City</label>
                            <input type="text" className="form-control" id="city" name="city" value={trainingSession.city} onChange={handleChange} />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="gym" className="form-label">Gym</label>
                            <input type="text" className="form-control" id="gym" name="gym" value={trainingSession.gym} onChange={handleChange} />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="slots" className="form-label">Available Slots</label>
                            <input type="number" className="form-control" id="slots" name="slots" value={trainingSession.slots} onChange={handleChange} />
                        </div>
                        <button type="submit" className="btn btn-primary">Create Session</button>
                    </form>
                </div>
            </div>
            {
                isFail && <Alert variant="outlined" severity="error" onClose={() => setIsFail(false)}
                    sx={{
                        color: 'red', width: '40vw', margin: 'auto', position: 'absolute',
                        bottom: '60px',
                        left: '50%',
                        transform: 'translateX(-50%)',
                        zIndex: '999'
                    }}
                >Please enter valid data !</Alert>
            }
        </div>
    );
};

export default CreateTrainingSession;
