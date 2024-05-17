import React, { useState, useEffect } from "react";
import { useAuth } from "../services/AuthContext";
import { deleteTeam, updateTeam } from "../services/TeamService";
import { Link } from "react-router-dom";
import TeamDetails from "../pages/TeamDetails";
import Button from "../common/Button";
import { useNavigate } from "react-router-dom";
import { InviteService } from "../services/InviteServices";

function Team({ team, setStatus, status }) {
  const inviteService = new InviteService();
  const user = useAuth();
  const [isEditFormOpen, setIsEditFormOpen] = useState(false);
  const [newTeamName, setNewTeamName] = useState("");
  const [newTeamDescription, setNewTeamDescription] = useState("");
  const [checkInviteSent, setCheckInviteSent] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (isEditFormOpen) {
      setNewTeamName(team.name);
      setNewTeamDescription(team.description);
    }
  }, [isEditFormOpen, team.name, team.description]);

  const handleUpdate = async () => {
    try {
      const trimmedName = newTeamName.trim();
      const trimmedDescription = newTeamDescription.trim();

      if (trimmedName === "" || trimmedDescription === "") {
        alert("Fields cannot be empty!");
        return;
      }

      const updatedTeam = {};
      if (
        trimmedName !== team.name &&
        trimmedDescription === team.description
      ) {
        updatedTeam.name = trimmedName;
        updatedTeam.description = trimmedDescription;
      }
      if (
        trimmedDescription !== team.description &&
        trimmedName === team.name
      ) {
        updatedTeam.description = trimmedDescription;
        updatedTeam.name = trimmedName;
      }

      if (Object.keys(updatedTeam).length === 0) {
        setIsEditFormOpen(false);
        return;
      }

      await updateTeam(team.id, updatedTeam);
      setIsEditFormOpen(false);
      setStatus(!status);
    } catch (error) {
      console.error("Greška prilikom ažuriranja tima:", error);
    }
  };

  const handleRequestSend = async () => {
    await inviteService.sendInvite(localStorage.getItem("UserId"), team.id);

    setCheckInviteSent(true); 
  };

  const handleDelete = async () => {
    try {
      await deleteTeam(team.id);
      setStatus(team.id);
    } catch (error) {
      console.error("Greška prilikom brisanja tima:", error);
    }
  };

  return (
    <div className="tournamentsList">
      <div className="tournamentCard">
        <div className="details">
          <h1>{team.name}</h1>
          <p>Description: {team.description}</p>
          <p>Date created: {new Date(team.dateCreated).toLocaleDateString()}</p>
          <p>Team Leader: {team.teamLeaderUsername}</p>
        </div>
        <div className="actions">
          <Button onClick={() => navigate(`/teams/${team.id}`)}>
            See details
          </Button>
          {user.role === "Admin" && (
            <div>
              <Button onClick={handleDelete}>Delete</Button>
            </div>
          )}
          {user.role === "Admin" && (
            <Button
              onClick={() => {
                setIsEditFormOpen(true);
                setNewTeamName(team.name);
                setNewTeamDescription(team.description);
              }}
            >
              Edit
            </Button>
          )}

          {isEditFormOpen && (
            <div className="editTeam">
              <label htmlFor="name">Name:</label>
              <input
                type="text"
                value={newTeamName}
                onChange={(e) => setNewTeamName(e.target.value)}
              />
              <label htmlFor="description">Description:</label>
              <input
                type="text"
                value={newTeamDescription}
                onChange={(e) => setNewTeamDescription(e.target.value)}
              />
              <Button onClick={handleUpdate}>Save</Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default Team;
