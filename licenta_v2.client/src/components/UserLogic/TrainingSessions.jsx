import { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL_USER, API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import ViewTrainer from './ViewTrainer';
import Alert from '@mui/material/Alert';
import './TrainingSession.css';
const TrainingSessionsList = () => {
    const [sessions, setSessions] = useState([]);
    const [filteredSessions, setFilteredSessions] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const user = auth.currentUser;
    const [trainer, setTrainer] = useState(null); 
    const [dialogOpen, setDialogOpen] = useState(false);
    const [isFail, setIsFail] = useState(false);

    useEffect(() => {
        const fetchSessions = async () => {
            try {
                const response = await axios.get(`${API_URL_USER}/training-sessions`, {
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setSessions(response.data.resource);
                setFilteredSessions(response.data);
                console.log(response.data);
            }
            catch (error) {
                console.log("Exception: ", error);
            }
        };
        fetchSessions();
    }, [user]);

    const handleSearch = (event) => {
        const term = event.target.value;
        setSearchTerm(term);
        const filtered = sessions.filter(session =>
            session.city.toLowerCase().includes(term.toLowerCase())
        );
        setFilteredSessions(filtered);
    };

    const handleOpenDialog = async (trainerId) => {
        try {
            const response = await axios.get(`${API_URL_TRAINER}/${trainerId}`, {
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            setTrainer(response.data);
            setDialogOpen(true);
        } catch (error) {
            console.error('Error fetching trainer data:', error);
        }
    };

    const joinTrainingSession = async (id) => {
        try {
            const response = await axios.patch(`${API_URL_USER}/training-sessions/${id}`, { UserId:user.uid } , {
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            console.log(response);
        } catch (error) {
            console.log("Error joining session: ", error);
            setIsFail(true);
        }
    }

    const handleCloseDialog = () => {
        setDialogOpen(false);
    };

    const formatDateTime = (dateTimeString) => {
        const dateTime = new Date(dateTimeString);
        const formattedDate = dateTime.toLocaleDateString();
        const formattedTime = dateTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        return `${formattedDate} ${formattedTime}`;
    };

    return (
        <div className="container">
            <div className="mb-4 mt-5">
                <input
                    type="text"
                    className="form-control"
                    placeholder="Filter by city"
                    value={searchTerm}
                    onChange={handleSearch}
                />
            </div>
            <div className="row row-cols-1 row-cols-md-3 g-4">
                {filteredSessions.map(session => (
                    <div className="col" key={session.id}>
                        <div className="card">
                            <div className="card-body session-body">
                                <h5 className="card-title">{session.title}</h5>
                                <p className="card-text">Start Date: {formatDateTime(session.startDate)}</p>
                                <p className="card-text">City: {session.city}</p>
                                <p className="card-text">Gym Location: {session.gym}</p>
                                <p className="card-text">Available Slots: {session.slots}</p>
                                <button className="btn btn-primary" onClick={() => joinTrainingSession(session.id)} >Join</button>
                                <button className="btn btn-info trainer-info-btn" onClick={() => handleOpenDialog(session.trainerId)}>Info</button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
            {trainer && <ViewTrainer open={dialogOpen} handleClose={handleCloseDialog} trainerData={trainer} />}
            {
                isFail && <Alert variant="outlined" severity="error" onClose={() => {setIsFail(false)}}
                    sx={{
                        color: 'red', width: '40vw', margin: 'auto', 
                        marginBottom: '30px',
                        zIndex: '999'
                    }}
                >You cannot enroll into this session !</Alert>
            }
        </div>
    );
};

export default TrainingSessionsList;
