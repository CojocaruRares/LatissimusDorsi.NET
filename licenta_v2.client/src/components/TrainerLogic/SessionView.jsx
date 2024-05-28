import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import '../UserLogic/TrainingSession.css'
import axios from 'axios';
import { API_URL_USER } from '../../utils/api_url';

const SessionView = ({ session }) => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get(`${API_URL_USER}/training-sessions/${session.id}/enrolled-users`);
                setUsers(response.data.resource);
                console.log(response.data);
            }
            catch (exception) {
                console.log("Exceptiion: ", exception);
            }
        }
        if (session.id) {
            fetchUsers();
        }
    }, [session.id]);

    const formatDateTime = (dateTimeString) => {
        const dateTime = new Date(dateTimeString);
        const formattedDate = dateTime.toLocaleDateString();
        const formattedTime = dateTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        return `${formattedDate} ${formattedTime}`;
    };

    return (
        <div>
            <div className="d-flex justify-content-center align-self-center">
                <div className="session-container">
                    <div className="text-container">
                        <h2 className='mb-4'>{session.title}</h2>
                        <p><strong>Start Date:</strong> {formatDateTime(session.startDate)}</p>
                        <p><strong>City:</strong> {session.city}</p>
                        <p><strong>Gym:</strong> {session.gym}</p>
                        <p><strong>Available Slots:</strong> {session.slots}</p>
                    </div>
                </div>
            </div>
            <div className="list-users">
                <ul className="user-session-list">
                    <h3>Enrolled Users: </h3>
                    {users.map((user, index) => (
                        <li key={index}>
                            <div className="user-session-data">
                                <div>
                                    <img src={`https://localhost:7281/Public/${user.profileImage}`} alt='user' className='user-session-image'></img>
                                    <p>{user.name}</p>
                                </div>                              
                                <p className="p-user-data">Age: {user.age}</p>
                                <p className="p-user-data">Gender: {user.gender}</p>
                                <p className="p-user-data">Objective: {user.objective}</p>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

SessionView.propTypes = {
    session: PropTypes.shape({
        id: PropTypes.string.isRequired,
        title: PropTypes.string.isRequired,
        startDate: PropTypes.string.isRequired,
        city: PropTypes.string.isRequired,
        gym: PropTypes.string.isRequired,
        slots: PropTypes.number.isRequired
    }).isRequired
};

export default SessionView;