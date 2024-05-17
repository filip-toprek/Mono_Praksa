import React, { useEffect, useState } from "react";
import Invite from "./Invite";
import { InviteService } from "../services/InviteServices";
import { useParams } from "react-router-dom";
import "../styles/tournament.css";

export default function InviteList() {
    const inviteService = new InviteService();
    const [invites, setInvites] = useState([]);
    const [status, setStatus] = useState(false);
    const { id: userId } = useParams();

    useEffect(() => {
        async function fetchInvites() {
            try {
                const invitesData = await inviteService.getInvites(userId);
                setInvites(invitesData || []); 
            } catch (error) {
                console.error("Error fetching invites:", error);
            }
        }
    
        fetchInvites();
    }, [userId, status]); 

    if (!invites) {
        return <p>Loading...</p>; 
    }

    return (
        <center>
        <div className="inviteCard">
            {invites.length === 0 ? (
                <p>No invites</p>
            ) : (
                invites.map(invite => (
                    <React.Fragment key={invite.id}>
                        <Invite
                            playerUsername={invite.player.user.username}
                            inviteId={invite.id} 
                            playerId={invite.playerId} 
                            teamId={invite.team.id} 
                            teamName={invite.team.name}
                            status={status}
                            setStatus={setStatus} />
                        <br />
                    </React.Fragment>
                ))
            )}
        </div>
        </center>
    );
}
