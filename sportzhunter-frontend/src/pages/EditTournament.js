import React, { useEffect, useState } from 'react';
import TournamentService from '../services/TournamentService';
import { useParams, useNavigate } from 'react-router-dom';
import Button from '../common/Button';

function EditTournament() {
    const params = useParams();
    const tournamentId = params.id;
    let navigate = useNavigate();
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        location: '',
        date: '',
        status: ''
    });

    useEffect(() => {
        fetchTournament();
    }, []);

    const fetchTournament = async () => {
        try {
            const response = await TournamentService.getTournament(tournamentId);
            setFormData(response.data);
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }

    const handleInputChange = (event) => {
        setFormData({...formData, [event.target.name]: event.target.value});
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            await TournamentService.updateTournament(tournamentId, formData);
            alert("Tournament updated successfully");
            navigate('/tournaments');

        } catch (error) {
            console.error(`Error: ${error}`);
            alert("Error updating tournament");
        }
    }

    return (
        <div className='formDiv'>
        <h1>Edit Tournament</h1>
        <form onSubmit={handleSubmit} className='form'>
        <label htmlFor="name">Name:</label>
        <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            required
        />

        <label htmlFor="description">Description:</label>
        <input
            type="text"
            id="description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
        />

        <label htmlFor="location">Location:</label>
        <input
            type="text"
            id="location"
            name="location"
            value={formData.location}
            onChange={handleInputChange}
        />

        <label htmlFor="date">Date:</label>
        <input
            type="date"
            id="date"
            name="date"
            value={formData.date.toString().split('T')[0]}
            onChange={handleInputChange}
        />

        <Button type="submit">Update Tournament</Button>
    </form>
    </div>
    );
}

export default EditTournament;