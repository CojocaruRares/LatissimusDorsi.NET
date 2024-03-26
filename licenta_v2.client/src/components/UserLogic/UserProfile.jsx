import { useState, useEffect } from 'react';
import axios from 'axios';
import { auth } from '../../utils/firebase-config';
import { signOut } from 'firebase/auth';
import { useNavigate } from 'react-router-dom';
import './UserProfile.css'
import EditUser from './EditUser';
import { API_URL_USER } from '../../utils/api_url';
import TrainingCalendar from '../../TrainingCalendar';

const UserProfile = () => {
    const [userData, setUserData] = useState({});
    const [openModal, setOpenModal] = useState(false);
    const [gender, setGender] = useState("");
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
                const response = await axios.get(API_URL_USER, {
                    params: { id: user.uid },
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setUserData(response.data);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchUserData();
    }, [user]);

    useEffect(() => {
        if (userData.gender === 0) {
            setGender("Male");
        } else if (userData.gender === 1) {
            setGender("Female");
        }
    }, [userData.gender]);

    return (
        <div>
            <div className="d-flex justify-content-center flex-column">
                <div className='profile-container'>
                    <div className='profile-info'>
                        <div className='profile-image'>
                            <img
                                src={`https://localhost:7281/Public/${userData.profileImage}`}
                                alt='Profile'
                                className='rounded-image'
                            />
                            <p className='name'>{userData.name}</p>
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
                            <p><strong>Address:</strong> {userData.address}</p>
                            <p><strong>Age:</strong> {userData.age}</p>
                            <p><strong>Height:</strong> {userData.height}</p>
                            <p><strong>Weight:</strong> {userData.weight}</p>
                            <p><strong>Objective:</strong> {userData.objective}</p>
                            <p><strong>Gender:</strong> {gender}</p>
                            <p><strong>Body Fat Percentage:</strong> {userData.bodyFatPercentage}</p>
                        </div>
                    </div>
                    <EditUser open={openModal} userData={userData} handleClose={handleCloseModal} />
                </div>
            </div>
            <div className="calendar-container">
                <TrainingCalendar></TrainingCalendar>
            </div>
        </div>
    );
};

export default UserProfile;
