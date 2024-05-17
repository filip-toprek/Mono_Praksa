import React from 'react';
import TournamentService from '../services/TournamentService';
import Button from '../common/Button.jsx';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../services/AuthContext.js';
import '../styles/tournament.css';

function TournamentTable({ tournaments, fetchTournaments }) {
    const user = useAuth();

    async function deleteTournament(id) {
        let confirmDelete = window.confirm("Are you sure you want to delete this tournament?");
        if (confirmDelete) {
            try {
                await TournamentService.deleteTournament(id);
                alert("Tournament deleted successfully");
                fetchTournaments();
            } catch (error) {
                console.error(`Error: ${error}`);
                alert("Error deleting tournament");
            }
        }else{
            alert("Tournament not deleted");
        }
    }

    let navigate = useNavigate();

    const handleRouteChange = (id) => {
        navigate(`/tournament/edit/${id}`);
    }

    return (
        <div className='tournamentsList'>
            {tournaments.map((tournament) => (
                <div key={tournament.id} className='tournamentCard'>
                    <div className='details'>
                        <h2>{tournament.name}</h2>
                        <p>{tournament.description}</p>
                        <p>Location: {tournament.location}</p>
                        <p>Date: {new Date(tournament.date).toLocaleDateString()}</p>
                    </div>
                    <div className='actions'>
                        {user && user.role === "Admin" && (
                            <>
                                <Button onClick={() => handleRouteChange(tournament.id)}>Edit</Button>
                                <Button onClick={() => deleteTournament(tournament.id)}>Delete</Button>
                            </>
                        )}
                        <Button onClick={() => navigate(`/tournament/${tournament.id}`)}>See details</Button>
                    </div>
                </div>
            ))}
        </div>
    );
}

export default TournamentTable;