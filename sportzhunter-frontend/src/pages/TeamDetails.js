import React, { useEffect, useState } from "react";
import { fetchTeamById } from "../services/TeamService";
import { useAuth } from "../services/AuthContext";
import "../styles/tournament.css";
import Button from "../common/Button";
import { InviteService } from "../services/InviteServices";

function TeamDetails() {
  const inviteService = new InviteService();
  const [team, setTeam] = useState(null);
  const [checkInviteSent, setCheckInviteSent] = useState(false);
  const user = useAuth();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const id = window.location.pathname.split("/").pop();
        const teamData = await fetchTeamById(id);
        setTeam(teamData);

        const inviteSent = await inviteService.checkIfInviteWasSent(teamData.id);
        setCheckInviteSent(inviteSent.data);
      } catch (error) {
        console.error("Greška prilikom dohvata tima:", error);
      }
    };

    fetchData();
  }, []);

  const handleRequestSend = async () => {
    await inviteService.sendInvite(localStorage.getItem("UserId"), team.id);
    setCheckInviteSent(true); 
  };

  if (!team) {
    return <div>No players</div>;
  }

  return (
    <center>
    <div className="detailsCard">
      <h1>{team.name}</h1>
      <p>Description: {team.description}</p>
      {(user.role === "Player" && !checkInviteSent) && <Button onClick={handleRequestSend}>Send a request</Button>}
      <p>Date created: {team.dateCreated}</p>
      <p>Team Leader: {team.teamLeaderUsername}</p>
      <h2>Current players:</h2>
      <ul>
        {team.playerList ? (
          team.playerList.map((player) => (
            <li key={player.id}>{player.username}</li>
          ))
        ) : (
          <li>No players found</li>
        )}
      </ul>
    </div>
    </center>
  );
}

export default TeamDetails;
