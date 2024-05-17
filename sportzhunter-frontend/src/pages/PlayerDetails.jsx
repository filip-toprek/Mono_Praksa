import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import playerService from "../services/PlayerService";
import Button from "../common/Button";
import { useAuth } from "../services/AuthContext";
import { InviteService } from "../services/InviteServices";
export default function PlayerDetails() {
    const { id } = useParams();
    const [player, setPlayer] = useState(null);
    const [loading, setLoading] = useState(true);
    const inviteService = new InviteService();
    const [checkInviteSent, setCheckInviteSent] = useState(false);

    const user = useAuth();

    function getAge(dateString) {
        const today = new Date();
        const birthDate = new Date(dateString);
        let age = today.getFullYear() - birthDate.getFullYear();
        const month = today.getMonth() - birthDate.getMonth();
        if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
        age--;
        }
        return age;
    }
    
  const handleRequestSend = async () => {
    const team = await playerService.getTeamLeaderProfile();
    await inviteService.sendInvite(id, team.data.team.id);
    setCheckInviteSent(true); 
  };

    useEffect(() => {
        async function fetchPlayer() {
        try {
            const response = await playerService.getPlayer(id);
            setPlayer(response.data);    
        } catch (error) {
            setPlayer(null);
        } finally {
            setLoading(false);
        }
        }
        fetchPlayer();
    }, [id]);
    
    return (
        <center>
            <div className="detailsCard">
                {loading && <p>Loading...</p>}
                {player && (
                    <div>
                    <h1>{player.user.firstName} {player.user.lastName}</h1>
                    <h2>{player.user.username}</h2>
                    <p><strong>Age: </strong>{getAge(player.dateOfBirth)}</p>
                    <p><strong>Position: </strong>{player.position ? player.position.positionName : "None"}</p>
                    <p><strong>County: </strong>{player.county.countyName}</p>
                    <p><strong>Height: </strong>{player.height} cm</p>
                    <p><strong>Weight: </strong>{player.weight} kg</p>
                    <p><strong>Description: </strong>{player.description ? player.description : "None"}</p>
                    {(user.role === "Teamleader" && !checkInviteSent)  && <Button onClick={handleRequestSend}>Send a request</Button>}
                    </div>
                )}
            </div>
        </center>
    );
}