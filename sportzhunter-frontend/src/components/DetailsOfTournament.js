import React, { useEffect } from "react";
import CommentAdd from "./CommentAdd";
import CommentList from "./CommentList";
import TournamentService from "../services/TournamentService";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../services/AuthContext";
import Matches from "./Matches";
import Button from "../common/Button";
function DetailsOfTournament({ tournament, matches, status, setStatus }) {
  const [attend, setAttend] = useState(false);
  const params = useParams();
  const user = useAuth();
  const navigate = useNavigate();
  
  const handleAddMatches = () => {
    navigate(`/tournament/${params.id}/match`);
  }

  const handleAttend = async () => {
    const data = {
      tournamentId: params.id,
    };

    try {
      await TournamentService.attendTournament(data);
      setAttend(true);
      alert("You have successfully attended the tournament!");
    } catch (error) {
      alert("You have already attended the tournament!");
    }
  };

  if (!tournament) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <center>
            <div className="detailsCard">
                <h1>Tournament Details</h1>
                <p>Name: {tournament.name}</p>
                <p>Description: {tournament.description}</p>
                <p>Location: {tournament.location}</p>
                <p>Date: {new Date(tournament.date).toLocaleDateString()}</p>
                <p>Teams attended: {tournament.attendees}/{tournament.numberOfTeams}</p>
                {user.role === "Teamleader" && (<Button onClick={handleAttend} disabled={attend}>Attend</Button>)}
                {user.role === "Admin" && (<Button onClick={handleAddMatches}>Add matches</Button>)}
            </div>
            {(matches.length > 0) &&
                <Matches status={status} setStatus={setStatus} matches={matches} />}
            <CommentList status={status} setStatus={setStatus}/>
            {user.role === "Player" && (
                <>
                    <CommentAdd status={status} setStatus={setStatus}/>
                </>
            )}
        </center>
        </>
  );
}

export default DetailsOfTournament;
