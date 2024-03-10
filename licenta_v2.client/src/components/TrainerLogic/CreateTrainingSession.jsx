import { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';

const CreateTrainingSession = () => {
    const [trainingSession, setTrainingSession] = useState({
        trainerId: '',
        users: [],
        startDate: '',
        city: '',
        gym: '',
        slots: 0
    });

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
        } catch (error) {
            console.error('Error creating training session:', error);
        }
    };

    return (
        <div className="container ">
            <div className="row justify-content-center mt-5">
                <div className="col-md-6">
                    <h2 className="text-center mb-4">Create Training Session</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label htmlFor="startDate" className="form-label">Start Date</label>
                            <input type="date" className="form-control" id="startDate" name="startDate" value={trainingSession.startDate} onChange={handleChange} />
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
        </div>
    );
};

export default CreateTrainingSession;
