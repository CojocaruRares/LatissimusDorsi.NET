import {useState } from "react"
import './CustomLogin.css'; 
import axios from 'axios';
import { useNavigate } from 'react-router-dom';


const UserAccountForm = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        name: '',
        address: '',
        age: 0,
        height: 0,
        weight: 0,
        objective: '',
        gender: 0,
        bodyFatPercentage: 0
    });

    const [next, setNext] = useState(false);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };


    const handleLogin = (e) => {
        e.preventDefault();
        setNext(true);
    };
   

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('https://localhost:7281/api/User/PostUser', formData);
            console.log('server:', response.data);
            navigate('/')
           
        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <div className='flex-login'>
            {next == false ? 
                 <form onSubmit={handleLogin} className="login-form">
            <div className="mb-3">
                        <label className="form-label">Email:</label>
                        <input type="email" className="form-control" name="email" value={formData.email} onChange={handleInputChange} />
            </div>
            <div className="mb-3">
                        <label className="form-label">Password:</label>
                        <input type="password" className="form-control" name="password" value={formData.password} onChange={handleInputChange} minLength = "8"/>
            </div>
            <button type="submit" className="btn btn-primary">Login</button>
                </form>
                :
                <form onSubmit={handleSubmit} className="user-account-form ">
                    <div className="mb-3">
                        <label className="form-label">Name:</label>
                        <input type="text" className="form-control" name="name" value={formData.name} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Address:</label>
                        <input type="text" className="form-control" name="address" value={formData.address} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Age:</label>
                        <input type="number" className="form-control" name="age" value={formData.age} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Height:</label>
                        <input type="number" className="form-control" name="height" value={formData.height} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Weight:</label>
                        <input type="number" className="form-control" name="weight" value={formData.weight} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Objective:</label>
                        <input type="text" className="form-control" name="objective" value={formData.objective} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Gender:</label>
                        <input type="number" className="form-control" name="gender" value={formData.gender} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Body Fat Percentage:</label>
                        <input type="number" className="form-control" name="bodyFatPercentage" value={formData.bodyFatPercentage} onChange={handleInputChange} />
                    </div>
                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
            }
        </div>
    );
};

export default UserAccountForm;
