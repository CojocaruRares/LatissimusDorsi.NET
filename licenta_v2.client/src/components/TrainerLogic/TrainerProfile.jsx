import { useState, useEffect } from 'react';
import axios from 'axios';
import { auth } from '../../utils/firebase-config';
import { signOut } from 'firebase/auth';
import { useNavigate } from 'react-router-dom';
import './TrainerProfile.css';
import TrainingCalendar from '../../TrainingCalendar';
import { API_URL_TRAINER } from '../../utils/api_url';
import EditTrainer from './EditTrainer';

const TrainerProfile = () => {
    const [trainerData, setTrainerData] = useState({});
    const [openModal, setOpenModal] = useState(false);
    const navigate = useNavigate();
    const user = auth.currentUser;

    const handleSignOut = async () => {
        signOut(auth).then(() => {
            navigate('/');
        }).catch((error) => {
            console.log(error);
        });
    }

    const handleOpenModal = () => {
        setOpenModal(true);
    };

    const handleCloseModal = () => {
        setOpenModal(false);
    };

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const response = await axios.get(API_URL_TRAINER, {
                    params: { id: user.uid },
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setTrainerData(response.data);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchUserData();
    }, [user]);


    return (
        <div>
            <div className="d-flex justify-content-center">
                <div className='profile-container'>
                    <div className='profile-info'>
                        <div className='profile-image'>
                            <img
                                src={`https://localhost:7281/Public/${trainerData.profileImage}`}
                                alt='Profile'
                                className='rounded-image-trainer'
                            />
                            <p className='name'>{trainerData.name}</p>
                            <div className="buttons">
                                <button className='btn btn-danger mt-3' onClick={handleSignOut}>
                                    Log Out
                                </button>
                                <button className='btn btn-primary mt-3 edit' onClick={handleOpenModal}>
                                    Edit Profile
                                </button>
                            </div>
                        </div>
                        <div className='profile-details'>
                            <p><strong>Address:</strong> {trainerData.address}</p>
                            <p><strong>Age:</strong> {trainerData.age}</p>
                            <p><strong>Description:</strong> {trainerData.description}</p>
                            <p><strong>Motto:</strong> {trainerData.motto}</p>
                            <p><strong>Specialization:</strong> {trainerData.specialization}</p>
                            <p><strong>Gym:</strong> {trainerData.gym}</p>
                        </div>
                    </div>
                    <EditTrainer open={openModal} userData={trainerData} handleClose={handleCloseModal} />
                </div>
            </div>
            <div className="calendar-container">
                <TrainingCalendar apiUrl={API_URL_TRAINER}></TrainingCalendar>
            </div>
        </div>
    );
};

export default TrainerProfile;
