import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Dialog, Button, TextField, RadioGroup, FormControl, DialogTitle, DialogActions, DialogContent, FormControlLabel, FormLabel, Radio } from '@mui/material';
import "../UserLogic/UserProfile.css";
import axios from 'axios';
import { API_URL_TRAINER } from '../../utils/api_url';
import { auth } from '../../utils/firebase-config';


const EditTrainer = ({ open, handleClose, userData }) => {

    const [editedData, setEditedData] = useState(userData);
    const [token, setToken] = useState(null);
    const user = auth.currentUser;
    useEffect(() => {
        if (user) {
            user.getIdToken().then((token) => {
                setToken(token);
            }).catch((error) => {
                console.error('Error fetching token:', error);
            });
        }
    }, [user]);



    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditedData({ ...editedData, [name]: value });
    };

    const handleSubmit = async () => {
        try {
            if (!token) {
                console.error('Token is null or unavailable');
                return;
            }
            const response = await axios.put(API_URL_TRAINER, editedData, {
                params: { id: editedData.id },
                headers: {
                    Authorization: 'Bearer ' + token,
                }
            });
            console.log(response)
            window.location.reload();
        } catch (error) {
            console.error('Error:', error);
        }
        handleClose();

    };

    useEffect(() => {
        setEditedData(userData);
    }, [userData]);

    return (
        <Dialog open={open} onClose={handleClose} className="dialog" >
            <DialogTitle sx={{ color: '#ccc' }}>Edit UserProfile.</DialogTitle>
            <DialogContent className="dialog-content" sx={{ input: { color: '#ccc' }, label: { color: '#ccc' } }}>
                <TextField

                    autoFocus
                    margin="dense"
                    label="Name"
                    className="form-control"
                    name="name"
                    value={editedData.name}
                    onChange={handleInputChange}

                />
                <TextField
                    autoFocus
                    margin="dense"
                    label="Address"
                    className="form-control"
                    name="address"
                    value={editedData.address}
                    onChange={handleInputChange}
                />
                <TextField
                    autoFocus
                    margin="dense"
                    label="Age"
                    className="form-control"
                    name="age"
                    type="number"
                    value={editedData.age}
                    onChange={handleInputChange}
                />
                <TextField
                    autoFocus
                    margin="dense"
                    label="Address"
                    className="form-control"
                    name="address"
                    value={editedData.address}
                    onChange={handleInputChange}
                />
                <TextField
                    autoFocus
                    margin="dense"
                    label="Motto"
                    className="form-control"
                    name="motto"
                    value={editedData.motto}
                    onChange={handleInputChange}
                />
                <TextField
                    autoFocus
                    margin="dense"
                    label="Description"
                    className="form-control"
                    name="description"
                    value={editedData.description}
                    multiline
                    maxRows={6}
                    inputProps={{ style: { color: '#ccc' } }}
                    onChange={handleInputChange}
                />
                <FormControl>
                    <FormLabel id="objective-label">Specialization</FormLabel>
                    <RadioGroup
                        row
                        aria-labelledby="demo-radio-buttons-group-label"
                        value={editedData.specialization}
                        name="specialization"
                        onChange={handleInputChange}
                    >
                        <FormControlLabel value="Bodybuilding" control={<Radio />} label="Bodybuilding" />
                        <FormControlLabel value="Powerlifting" control={<Radio />} label="Powerlifting" />
                        <FormControlLabel value="Weightloss" control={<Radio />} label="Weightloss" />
                    </RadioGroup>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button variant="contained" color="primary" onClick={handleSubmit}>
                    Submit
                </Button>
                <Button variant="contained" color="secondary" onClick={handleClose}>
                    Close
                </Button>
            </DialogActions>
        </Dialog>
    );
};

EditTrainer.propTypes = {
    open: PropTypes.bool.isRequired,
    handleClose: PropTypes.func.isRequired,
    userData: PropTypes.object.isRequired,
};

export default EditTrainer;
