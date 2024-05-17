import { InviteService } from "../services/InviteServices";
import Button from "../common/Button";
import { useAuth } from "../services/AuthContext";

export default function Invite({inviteId, playerId, teamId, teamName, status, setStatus, playerUsername}){
    const inviteService = new InviteService();
    const user = useAuth();

    const handleDeclineClick = async () => {
        await inviteService.acceptDeclineInvite({id: inviteId, playerId: playerId, teamId: teamId, responseSent: false});
        setStatus(!status);
    };

    const handleAcceptClick = async () => {
        await inviteService.acceptDeclineInvite({id: inviteId, playerId: playerId, teamId: teamId, responseSent: true});
        setStatus(!status);
    };

    return (
      
        <div className="invitationCard">
            {user.role === "Teamleader" ? <p>{playerUsername} requested to join your team.</p> : <p>You have been invited to join {teamName}</p>}
            <div className="actions">
                <Button onClick={handleAcceptClick}>Accept</Button>
                <Button onClick={handleDeclineClick}>Decline</Button>
            </div>
        </div>
        
    );
}