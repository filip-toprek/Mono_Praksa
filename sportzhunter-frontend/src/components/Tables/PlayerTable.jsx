import Button from "../../common/Button";
import { useNavigate } from "react-router-dom";
import '../../styles/tournament.css';

export default function PlayerTable({ playerList }) {
    const navigate = useNavigate();
    return (
        <>
        {playerList.length === 0 ? ( 
            <p>No players found.</p> 
          ) : (
            <div className='tournamentsList'>
              {playerList && playerList.map(player => (
                <div key={player.id} className='playerCard'>
                  <div className='details'>
                    <div className="center">
                      <h2>{player.firstName}</h2>
                    </div>
                    <p><strong>Lastname: </strong>{player.lastName}</p>
                    <p><strong>Username: </strong>{player.username}</p>
                    <p><strong>Height: </strong>{player.height}</p>
                    <p><strong>Weight: </strong>{player.weight}</p>
                    <p><strong>Date of birth: </strong>{new Date(player.dateOfBirth).toLocaleDateString()}</p>
                    <p><strong>County: </strong>{player.county ? player.county.countyName : 'n/a'}</p>
                    <p><strong>Description: </strong>{player.description}</p>
                    <div className="playerBtn">
                      <Button onClick={() => navigate(`/player/${player.id}`)}>See details</Button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
    </>
    );
}