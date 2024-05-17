import { useEffect, useState } from "react";
import Button from "../common/Button";
import { useAuth } from "../services/AuthContext";
import playerService from "../services/PlayerService";

export default function TeamInfo({ teamInfo, leaveTeam, kickPlayer }) {
    const [teamleaderTeam, setTeamleaderTeam] = useState([{}]);
    const user = useAuth();

    const fetchTeamInfo = async () => {
        try {
            const response = await playerService.getTeamLeaderProfile();
            setTeamleaderTeam(response.data.team);
        } catch (error) {
            setTeamleaderTeam(null);
        }
    }

    useEffect(() => {
        fetchTeamInfo();
    },[]);

    async function kickPlayerAsync(data) {
        await kickPlayer(data);
        fetchTeamInfo();
    }

    return (
        <>
        {teamInfo && (
            <div>
              <h1>Team info</h1>
              <p><strong>Name: </strong>{teamInfo.name}</p>
              <p><strong>Teamleader: </strong>{teamInfo.teamLeaderName}</p>
              <p><strong>Description: </strong>{teamInfo.description}</p>
              <Button onClick={leaveTeam}>Leave team</Button>
            </div>
          )}
          
          {(teamleaderTeam && !teamInfo) && (
            <>
              <h1>Team info</h1>
              <p><strong>Name: </strong> {teamleaderTeam.name}</p>
              <p><strong>Teamleader: </strong> {teamleaderTeam.teamLeaderName}</p>
              <p><strong>Description: </strong> {teamleaderTeam.description}</p>
          
              {user.role === "Teamleader" && (
                <>
                  <p><strong>Player list: </strong></p>
                  {(teamleaderTeam.playerList && teamleaderTeam.playerList.length > 0) ? (teamleaderTeam.playerList.map((player) => (
                    <div key={player.id}>
                      <p>{player.username}</p>
                      <Button onClick={() => kickPlayerAsync({ playerId: player.id, teamId: teamleaderTeam.id })}>
                        Kick player
                      </Button>
                    </div>
                  ))) : <p><strong>No players. </strong></p>}
                </>
              )}
            </>
          )}
          </>
    );
}