import { useState, useEffect } from 'react';
import axios from 'axios';

const UserProfile = () => {
    const [userData, setUserData] = useState({});

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const response = await axios.get('https://localhost:7281/api/User/GetUser');
                setUserData(response.data);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchUserData();
    }, []);

    return (
        <div className='container mt-4'>
            <h2>User Profile</h2>
            <div className='card'>
                <div className='card-body'>
                    <p className='card-text'><strong>Name:</strong> {userData.name}</p>
                    <p className='card-text'><strong>Address:</strong> {userData.address}</p>
                    <p className='card-text'><strong>Age:</strong> {userData.age}</p>
                    <p className='card-text'><strong>Height:</strong> {userData.height}</p>
                    <p className='card-text'><strong>Weight:</strong> {userData.weight}</p>
                    <p className='card-text'><strong>Objective:</strong> {userData.objective}</p>
                    <p className='card-text'><strong>Gender:</strong> {userData.gender}</p>
                    <p className='card-text'><strong>Body Fat Percentage:</strong> {userData.bodyFatPercentage}</p>
                </div>
            </div>
        </div>
    );
};

export default UserProfile;
