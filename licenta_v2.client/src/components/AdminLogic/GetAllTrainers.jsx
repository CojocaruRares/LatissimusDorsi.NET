import { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL_ADMIN } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';

const GetAllTrainers = () => {
    const [trainers, setTrainers] = useState([]);
    const user = auth.currentUser;

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get(`${API_URL_ADMIN}/Trainers`, {

                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setTrainers(response.data);
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        };

        fetchUsers();
    }, [user]);

    return (
        <div>
            <h1>Trainers</h1>
            <ul>
                {trainers.map((trainer, index) => (
                    <li key={index}>
                        <div>
                            <strong>Name:</strong> {trainer.name}
                        </div>
                        <div>
                            <strong>Address:</strong> {trainer.address}
                        </div>
                        <div>
                            <strong>Gym:</strong> {trainer.gym}
                        </div>
                        <div>
                            <strong>Spec:</strong> {trainer.specialization}
                        </div>
                        <div>
                            <strong>Profile Image:</strong> {trainer.profileImage}
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default GetAllTrainers;
