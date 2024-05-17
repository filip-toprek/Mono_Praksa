import React from "react";
import '../styles/tournament.css';
import Button from "../common/Button";

function AddTournamentForm({ formData, handleInputChange, handleSubmit}) {
    return (
        <>
            <center>
                <h1>Add Tournament</h1>
            </center>
            <div className="formDiv">
                <form onSubmit={handleSubmit} className="form">
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

                    <label htmlFor="numberOfTeams">Number of Teams:</label>
                    <input
                        type="number"
                        id="numberOfTeams"
                        name="numberOfTeams"
                        value={formData.numberOfTeams}
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
                        value={formData.date}
                        onChange={handleInputChange}
                    />
                    <div style={{margin:"20px"}}>
                        <Button type="submit">Add Tournament</Button>
                    </div>
                </form>
        </div>
        </>
    );
}

export default AddTournamentForm;
    