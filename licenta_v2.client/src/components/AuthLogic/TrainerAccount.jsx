import { useState } from "react";
import './CustomLogin.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { API_URL_TRAINER } from "../../utils/api_url";
import Alert from '@mui/material/Alert';

const TrainerAccountForm = () => {
    const navigate = useNavigate();
    const [image, setImage] = useState(null);
    const [next, setNext] = useState(false);
    const [isFail, setIsFail] = useState(false);
    const [formData, setFormData] = useState({
        name: '',
        address: '',
        description: '',
        age: 0,
        motto: '',
        gym: '',
        specialization: 'Bodybuilding'
    });

    const saveImage = (e) => {
        console.log(e.target.files[0]);
        setImage(e.target.files[0]);
    };

    const handleLogin = (e) => {
        e.preventDefault();
        setNext(true);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const sendForm = new FormData();
            for (let key in formData) {
                sendForm.append(key, formData[key]);
            }
            sendForm.append("profileImage", image);
            const response = await axios.post(API_URL_TRAINER, sendForm);
            console.log('server:', response.data);
            navigate('/')

        } catch (error) {
            console.error('Error:', error);
            setIsFail(true);
        }
    };

    return (
        <div>
            <div className='flex-login'>
                {next == false ?
                    <div>
                        <h2>Enter user credentials.</h2>
                        <br></br>
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
                    </div>
                    :
                    <form onSubmit={handleSubmit} className="trainer-account-form">
                        <div className="mb-3">
                            <label className="form-label">Name:</label>
                            <input type="text" className="form-control" name="name" value={formData.name} onChange={handleInputChange} />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Address:</label>
                            <input type="text" className="form-control" name="address" value={formData.address} onChange={handleInputChange} />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Description:</label>
                            <textarea className="form-control" name="description" maxLength={340} value={formData.description} onChange={handleInputChange} />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Age:</label>
                            <input type="number" className="form-control" name="age" value={formData.age} onChange={handleInputChange} />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Motto:</label>
                            <input type="text" className="form-control" name="motto" value={formData.motto} onChange={handleInputChange} />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Gym:</label>
                            <input type="text" className="form-control" name="gym" value={formData.gym} onChange={handleInputChange} />
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
                            <label className="form-label">Profile Image:</label>
                            <input type="file" className="form-control" name="profileImage" onChange={saveImage} />
                        </div>
                        <button type="submit" className="btn btn-primary">Submit</button>
                    </form>
                }
            </div>
            {
                isFail && <Alert variant="outlined" severity="error" onClose={() => setIsFail(false)}
                    sx={{
                        color: 'red', width: '40vw', margin: 'auto', position: 'absolute',
                        top: '20px',
                        left: '50%',
                        transform: 'translateX(-50%)',
                        zIndex: '999'
                    }}
                >Please enter valid data !</Alert>
            }
        </div>
    );
};

export default TrainerAccountForm;
