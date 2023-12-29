import { useState, useEffect } from 'react';
import axios from 'axios';
import { auth } from '../../utils/firebase-config';
import { signOut } from 'firebase/auth';
import { useNavigate } from 'react-router-dom';
import './UserProfile.css'

const UserProfile = () => {
    const [userData, setUserData] = useState({});
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

    useEffect(() => {
        const fetchUserData = async () => {
            try {      
                const response = await axios.get('https://localhost:7281/api/User/GetUser', {
                    params: { id: user.uid } 
                });            
                setUserData(response.data);
                if (userData.gender == 0) setGender("Male");
                else setGender("Female");
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchUserData();
    }, [user]);

    return (
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
                        <button className='btn btn-primary mt-3 edit' onClick={handleSignOut}>
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
        </div>
    );
};

export default UserProfile;
