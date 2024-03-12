import PropTypes from 'prop-types';
import { Dialog, Button } from '@mui/material';
import './TrainingSession.css'

const ViewTrainer = ({ open, handleClose, trainerData }) => {
    return (
        <Dialog open={open} onClose={handleClose}>
            <div className="dialogContainer">
                <h2 className="dialogTitle">View Trainer Profile</h2>
                <div className="trainerHeader">
                    {trainerData.profileImage && <img src={`https://localhost:7281/Public/${trainerData.profileImage}`} alt="Trainer" className="trainerImage" />}
                    <h3 className="trainerName">{trainerData.name}</h3>
                </div>
                <div className="trainerInfo">
                    <p><strong>Address:</strong> {trainerData.address}</p>
                    <p><strong>Age:</strong> {trainerData.age}</p>
                    <p><strong>Motto:</strong> {trainerData.motto}</p>
                    <p className="descriptionParagraph"><strong>Description:</strong></p>
                    <p className="description">{trainerData.description}</p>
                    <p><strong>Specialization:</strong> {trainerData.specialization}</p>
                </div>
                <Button onClick={handleClose} variant="contained" color="error" className="closeButton">Close</Button>
            </div>
        </Dialog>
    );
};

ViewTrainer.propTypes = {
    open: PropTypes.bool.isRequired,
    handleClose: PropTypes.func.isRequired,
    trainerData: PropTypes.object.isRequired,
};

export default ViewTrainer;
