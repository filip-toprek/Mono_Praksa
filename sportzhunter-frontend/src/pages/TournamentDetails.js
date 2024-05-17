import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import TournamentService from "../services/TournamentService.js";
import DetailsOfTournament from "../components/DetailsOfTournament.js";
import Matches from "../components/Matches.js";
import matchService from "../services/MatchService.js";

function TournamentDetails() {
  const [tournament, setTournament] = useState(null);
  const [matches, setMatches] = useState([]);
  const params = useParams();
  const tournamentId = params.id;
  const [status, setStatus] = useState(false);

  const fetchTournament = async () => {
    try {
      const response = await TournamentService.getTournament(tournamentId);
      setTournament(response.data);
    } catch (error) {
      console.error(`Error: ${error}`);
    }
  };

  const fetchMatchesForTournament = async () => {
    try {
      const response = await matchService.fetchMatchByTournamentId(
        tournamentId
      );
      setMatches(response);
    } catch (error) {
      console.error(`Error fetching matches for tournament: ${error}`);
    }
  };
  useEffect(() => {
    fetchTournament();
    fetchMatchesForTournament();
  }, [status]);

  if (!tournament) {
    return <div>Loading...</div>;
  }


    return (
        <div>
            <DetailsOfTournament status={status} setStatus={setStatus} matches={matches} tournament={tournament} />
        </div>
    );
 
}

export default TournamentDetails;
