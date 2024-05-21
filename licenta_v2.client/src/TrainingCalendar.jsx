import { useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import './TrainingCalendar.css';
import { auth } from './utils/firebase-config';
import axios from 'axios';
import PropTypes from 'prop-types';


const MyCalendar = ({ apiUrl }) => {
    const [selectedDate, setSelectedDate] = useState(new Date());
    const user = auth.currentUser;
    const [sessions, setSessions] = useState([]);

    const handleDateClick = async (date) => {
        try {
            setSelectedDate(date);
            const response = await axios.get(`${apiUrl}/${user.uid}/my-sessions`, {
                params: { datetime: date },
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            setSessions(response.data);
        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <div className="calendar-container-flex">
            <div className="calendar-data">
                <h3>Select a Date</h3>
                <Calendar
                    onChange={handleDateClick}
                    value={selectedDate}
                />
            </div>
            <div className="sessions-data">
                <h3>Available Training Sessions:</h3>
                <ul className="session-list">
                    {sessions.map(session => (
                        <li key={session.id} className="session-list-item">
                            <p>Title: {session.title}</p>
                            <p>Start Date: {new Date(session.startDate).toLocaleString()}</p>
                            <p>Location: {session.gym}</p>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};
MyCalendar.propTypes = {
    apiUrl: PropTypes.string.isRequired,
};
export default MyCalendar;
