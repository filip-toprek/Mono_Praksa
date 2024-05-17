import React from "react";
import TournamentService from "../services/TournamentService";
import { useState } from "react";
import AddTournamentForm from "../components/AddTournamentForm";
import { useNavigate } from "react-router-dom";
function CreateTournament() {
    const [formData, setFormData] = useState({
        name: "",
        description: "",
        numberOfTeams: 0,
        location: "",
        date: ""
    });
    let navigate = useNavigate();
    const handleInputChange = (event) => {
        setFormData({ ...formData, [event.target.name]: event.target.value });
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        TournamentService.createTournament(formData)
            .then((response) => {
                alert("Tournament created successfully");
                setFormData({
                    name: "",
                    description: "",
                    numberOfTeams: 0,
                    location: "",
                    date: ""
                });
                navigate("/tournaments");
            })
            .catch((error) => {
                console.error(`Error: ${error}`);
                alert("Error creating tournament");
            });
    };

    return (
        <div>
            <AddTournamentForm
                formData={formData}
                handleInputChange={handleInputChange}
                handleSubmit={handleSubmit}
            />
        </div>
    );
}

export default CreateTournament;