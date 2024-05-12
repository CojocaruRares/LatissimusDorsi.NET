import { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL_ADMIN } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import './ViewAllUsers.css';

const GetAllTrainers = () => {
    const [trainers, setTrainers] = useState([]);
    const [filteredTrainers, setFilteredTrainers] = useState([]);
    const [showFilters, setShowFilters] = useState(false);
    const [filterName, setFilterName] = useState('');
    const [filterAddress, setFilterAddress] = useState('');
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
                setFilteredTrainers(response.data);
                console.log(response.data);
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        };

        fetchUsers();
    }, [user]);

    const DeleteTrainer = async (myId) => {
        try {
            await axios.delete(`${API_URL_ADMIN}/Trainers`, {
                params: { id: myId },
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
            });
            const updatedTrainers = trainers.filter(user => user.id !== myId);
            setTrainers(updatedTrainers);
            setFilteredTrainers(updatedTrainers);
        } catch (error) {
            console.error('Error deleting user:', error);
        }
    }


    useEffect(() => {
        const filteredByName = trainers.filter(user => {
            return user.name.toLowerCase().includes(filterName.toLowerCase());
        });
        setFilteredTrainers(filteredByName);
    }, [filterName, trainers]);


    useEffect(() => {
        const filteredByAddress = trainers.filter(user => {
            return user.address.toLowerCase().includes(filterAddress.toLowerCase());
        });
        setFilteredTrainers(filteredByAddress);
    }, [filterAddress, trainers]);

    const handleNameFilterChange = event => {
        setFilterName(event.target.value);
    };

    const handleAddressFilterChange = event => {
        setFilterAddress(event.target.value);
    };

    const toggleFilters = () => {
        setShowFilters(!showFilters);
    };

    return (
        <div>
            <div className="d-flex justify-content-center">
                <div className="dropdown">
                    <button className="dropbtn" onClick={toggleFilters}>Filters</button>
                    {showFilters && (
                        <div className="dropdown-content">
                            <div className="input-container">
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="Filter by name"
                                    value={filterName}
                                    onChange={handleNameFilterChange}
                                />
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="Filter by address"
                                    value={filterAddress}
                                    onChange={handleAddressFilterChange}
                                />
                            </div>
                        </div>
                    )}
                </div>
            </div>
            <div className="list-container">
                <ul className="all-users-list">
                    {filteredTrainers.map((trainer, index) => (
                        <li key={index}>
                            <div className="user-details">
                                <img
                                    src={`https://localhost:7281/Public/${trainer.profileImage}`}
                                    alt='Profile'
                                    className='user-image-admin'
                                />
                                <p className="user-info"><strong>Name:</strong> {trainer.name}</p>
                                <p className="user-info"><strong>Address:</strong> {trainer.address}</p>
                                <p className="user-info"><strong>Fitness Objective:</strong> {trainer.gym}</p>
                                <p className="user-info"><strong>Fitness Objective:</strong> {trainer.specialization}</p>
                                <button type="button" className="btn btn-danger delete-btn" onClick={() => DeleteTrainer(trainer.id)}>Delete</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default GetAllTrainers;
