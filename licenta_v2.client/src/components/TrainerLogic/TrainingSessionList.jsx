import { useState, useEffect } from 'react';
import '../UserLogic/TrainingSession.css'
import axios from 'axios';
import { API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import { useNavigate } from 'react-router-dom';
import SessionView from './SessionView';
 

const TrainingSessionList = () => {
    const navigate = useNavigate();
    const user = auth.currentUser;
    const [sessions, setSessions] = useState([]);
    const [selectedSession, setSelectedSession] = useState(null)
    const [disableBack, setDisableBack] = useState(true);

    useEffect(() => {
        const fetchSessions = async () => {
            try {
                const response = await axios.get(`${API_URL_TRAINER}/TrainingSession`, {
                    params: { id: user.uid },
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setSessions(response.data);
            }
            catch (error) {
                console.log("Exception: ", error);
            }
        };
        fetchSessions();
    }, [user]);


    const handleBack = () => {
        setSelectedSession(null);
        setDisableBack(true);
    }

    const navigateToCreateTrainingSession = () => {
        navigate('/CreateTrainingSession');
    }

    const viewSession = (session) => {
        setSelectedSession(session);
        setDisableBack(false);
    }


    return (
        <div>
            <div className='sessions-title'>
                <button className="btn btn-primary back-button" onClick={handleBack} disabled={disableBack}>Back</button>
                <h2>My Training Sessions</h2>
            </div>
            {selectedSession ? (
                <SessionView session={selectedSession} />

            ) : (
                <div>
                    {Object.keys(sessions).length > 0 ? (
                        <ul className='list-group workout-group mb-5'>
                            {Object.values(sessions).map((session, index) => (
                                <li key={index} className='list-group-item'>
                                    <div className='row align-items-center'>
                                        <div className='col-md-4'>
                                            <h3>{session.title}</h3>
                                        </div>
                                        <div className='col-md-4 text-center'>
                                            <p className="mb-0">Location: {session.gym}</p>
                                        </div>
                                        <div className='col-md-4'>
                                            <button className='btn btn-primary float-end' onClick={() => viewSession(session)}>View</button>
                                        </div>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No workouts available. Create one!</p>
                    )}
                    <div className="d-flex justify-content-center">
                        <button className="btn btn-lg btn-primary  m-4 " onClick={navigateToCreateTrainingSession}>Create Training Program</button>
                    </div>
                </div>
            )}
        </div>
    );

}

export default TrainingSessionList;