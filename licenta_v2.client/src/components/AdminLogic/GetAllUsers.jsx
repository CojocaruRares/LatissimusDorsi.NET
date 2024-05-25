import { useState, useEffect } from 'react';
import axios from 'axios';
import { API_URL_ADMIN } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';
import './ViewAllUsers.css';

const GetAllUsers = () => {
    const [users, setUsers] = useState([]);
    const [filteredUsers, setFilteredUsers] = useState([]);
    const [showFilters, setShowFilters] = useState(false);
    const [filterName, setFilterName] = useState('');
    const [filterAddress, setFilterAddress] = useState('');
    const user = auth.currentUser;

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get(`${API_URL_ADMIN}/users`, {
                    headers: {
                        Authorization: 'Bearer ' + user.accessToken,
                    }
                });
                setUsers(response.data);
                setFilteredUsers(response.data);
                console.log(response.data);
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        };

        fetchUsers();
    }, [user]);

    const DeleteUser = async (myId) => {
        try {
                await axios.delete(`${API_URL_ADMIN}/user/${myId}`, {
                params: { id: myId },
                headers: {
                    Authorization: 'Bearer ' + user.accessToken,
                }
                });
            const updatedUsers = users.filter(user => user.id !== myId);
            setUsers(updatedUsers);
            setFilteredUsers(updatedUsers);
        } catch (error) {
            console.error('Error deleting user:', error);
        }
    }


    useEffect(() => {
        const filteredByName = users.filter(user => {
            return user.name.toLowerCase().includes(filterName.toLowerCase());
        });
        setFilteredUsers(filteredByName);
    }, [filterName, users]);


    useEffect(() => {
        const filteredByAddress = users.filter(user => {
            return user.address.toLowerCase().includes(filterAddress.toLowerCase());
        });
        setFilteredUsers(filteredByAddress);
    }, [filterAddress, users]);

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
                    {filteredUsers.map((user, index) => (
                        <li key={index}>
                            <div className="user-details">
                                <img
                                    src={`https://localhost:7281/Public/${user.profileImage}`}
                                    alt='Profile'
                                    className='user-image-admin'
                                />
                                <p className="user-info"><strong>Name:</strong> {user.name}</p>
                                <p className="user-info"><strong>Address:</strong> {user.address}</p>
                                <p className="user-info"><strong>Fitness Objective:</strong> {user.objective}</p>
                                <button type="button" className="btn btn-danger delete-btn" onClick={() => DeleteUser(user.id)}>Delete</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default GetAllUsers;
