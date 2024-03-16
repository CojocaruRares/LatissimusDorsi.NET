import { useState } from "react"
import './CustomLogin.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { API_URL_USER } from "../../utils/api_url";

const UserAccountForm = () => {
    const navigate = useNavigate();
    const [image, setImage] = useState(null);
    const [next, setNext] = useState(false);
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        name: '',
        address: '',
        age: 0,
        height: 0,
        weight: 0,
        objective: 'Bodybuilding',
        gender: 0,
        bodyFatPercentage: 0
    });

    
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const saveImage = (e) => {
        console.log(e.target.files[0]);
        setImage(e.target.files[0]);
    };

    const handleLogin = (e) => {
        e.preventDefault();
        setNext(true);
    };


    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const sendForm = new FormData();
            for (let key in formData) {
                sendForm.append(key, formData[key]);
            }
            sendForm.append("profileImage", image);
            const response = await axios.post(API_URL_USER, sendForm);
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
                        <input type="password" className="form-control" name="password" value={formData.password} onChange={handleInputChange} minLength="8" />
                    </div>
                    <button type="submit" className="btn btn-primary">Next</button>
                </form>
                :
                <form onSubmit={handleSubmit} className="user-account-form " encType="multipart/form-data">
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
                        <select className="form-select" name="objective" value={formData.objective} onChange={handleInputChange}>
                            <option value="Bodybuilding">Bodybuilding</option>
                            <option value="Powerlifting">Powerlifting</option>
                            <option value="Weightloss">Weightloss</option>
                        </select>
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Gender:</label>
                        <select className="form-select" name="gender" value={formData.gender} onChange={handleInputChange}>
                            <option value={0}>Male</option>
                            <option value={1}>Female</option>
                        </select>
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Body Fat Percentage:</label>
                        <input type="number" className="form-control" name="bodyFatPercentage" value={formData.bodyFatPercentage} onChange={handleInputChange} />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Profile Image:</label>
                        <input type="file" className="form-control" name="profileImage" onChange={saveImage} />        
                    </div>
                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
            }
        </div>
    );
};

export default UserAccountForm;