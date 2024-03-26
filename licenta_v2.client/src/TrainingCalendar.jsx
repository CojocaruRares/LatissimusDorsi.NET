import { useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import './TrainingCalendar.css';

const MyCalendar = () => {
    const [selectedDate, setSelectedDate] = useState(new Date()); 

    const handleDateClick = (date) => {
        setSelectedDate(date);
    };

    return (
        <div>
            <h2>Calendar</h2>
            <p>Current day: {selectedDate.toDateString()}</p>
            <Calendar
                onChange={handleDateClick} 
                value={selectedDate} 
            />
        </div>
    );
};

export default MyCalendar;