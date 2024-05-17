import { useEffect, useState } from "react";
import { fetchTeams } from "../../services/TeamService";
import { useNavigate, useParams } from "react-router-dom";
import Button from "../../common/Button";
import adminService from "../../services/AdminService";
import { fetchMatchById } from "../../services/MatchService";

export default function EditMatchForm() {
    const params = useParams();
    const [match, setMatch] = useState({result: "", teamHomeId: "", teamAwayId: "", tournamentId: params.id});
    const [teamList, setTeamList] = useState([]);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const [filters, setFilters] = useState({
        name: "",
        description: "",
        teamleaderid: "",
        datecreated: "",
        sortby: "",
        sortorder: "",
        pageNumber: 1,
        pageSize: 100,
        totalPages: 0,
      });

      const editMatch = async (e) => {
        e.preventDefault();
        await adminService.editMatch(params.matchId, match).then((result) => {
            navigate(`/tournament/${params.id}`);
        }).catch((err) => {
            setError(err);
        });
        }

    useEffect(() => {
        async function fetchData() {
            try{
                await fetchMatchById(params.matchId).then((response) => {
                    setMatch((prevData) => ({ ...prevData, teamHomeId: response.teamHomeId}));
                    setMatch((prevData) => ({ ...prevData, teamAwayId: response.teamAwayId}));
                    setMatch((prevData) => ({ ...prevData, tournamentId: params.id}));
                    setMatch((prevData) => ({ ...prevData, result: response.result}));

                });
                const response = await fetchTeams(filters);
                setTeamList(response.data.list);
            }catch(error){
                console.error(`Error: ${error}`);
            }
        }
        fetchData();
      }, []);
    return (
        <div className="formDiv">
            <h1>Edit Match</h1>
            <form onSubmit={editMatch} className="form">
                {error && <p>{error.message}</p>}
                <label htmlFor="result">Result</label>
                <input type="text" id="result" name="result" value={match.result} onInput={(e) => setMatch({...match, result: e.target.value})} required/>
                <label htmlFor="teamHome">Team Home</label>
                <select id="teamHome" name="teamHome" value={match.teamHomeId} onChange={(e) => setMatch({...match, teamHomeId: e.target.value})} required>
                    {teamList.map((team) => <option key={team.id} value={team.id}>{team.name}</option>)}
                </select>
                <label htmlFor="teamAway">Team Away</label>
                <select id="teamAway" name="teamAway" value={match.teamAwayId} onChange={(e) => setMatch({...match, teamAwayId: e.target.value})} required>
                    {teamList.map((team) => <option key={team.id} value={team.id}>{team.name}</option>)}
                </select>
                <Button>Edit Match</Button>
            </form>
        </div>
    );
    }