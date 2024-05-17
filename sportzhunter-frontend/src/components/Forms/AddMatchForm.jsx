import { useEffect, useState } from "react";
import { fetchTeams } from "../../services/TeamService";
import { useNavigate, useParams } from "react-router-dom";
import Button from "../../common/Button";
import adminService from "../../services/AdminService";

export default function AddMatchForm() {
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

      const AddMatch = async (e) => {
        e.preventDefault();
        await adminService.createMatch(match).then(() => {
            navigate(`/tournament/${params.id}`);
        }).catch((err) => {
            setError(err);
        });
        }

    useEffect(() => {
        async function fetchData() {
            try{
                const response = await fetchTeams(filters);
                setTeamList(response.data.list);
                setMatch((prevData) => ({ ...prevData, teamHomeId: response.data.list[0].id}));
                setMatch((prevData) => ({ ...prevData, teamAwayId: response.data.list[0].id}));
            }catch(error){
                console.error(`Error: ${error}`);
            }
        }
        fetchData();
      }, []);
    return (
        <div className="formDiv"> 
            <h1>Add Match</h1>
            <form onSubmit={AddMatch} className="form">
                {error && <p>{error.message}</p>}
                <label htmlFor="result">Result</label>
                <input type="text" id="result" name="result" onInput={(e) => setMatch({...match, result: e.target.value})} required/>
                <label htmlFor="teamHome">Team Home</label>
                <select id="teamHome" name="teamHome" onInput={(e) => setMatch({...match, teamHomeId: e.target.value})} required>
                    {teamList.map((team) => <option key={team.id} value={team.id}>{team.name}</option>)}
                </select>
                <label htmlFor="teamAway">Team Away</label>
                <select id="teamAway" name="teamAway" onInput={(e) => setMatch({...match, teamAwayId: e.target.value})} required>
                    {teamList.map((team) => <option key={team.id} value={team.id}>{team.name}</option>)}
                </select>
                <Button>Add Match</Button>
            </form>
        </div>
    );
    }