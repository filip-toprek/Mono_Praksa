import React from "react";
import { useAuth } from "../services/AuthContext";
import Button from "../common/Button";
import { useNavigate, useParams } from "react-router-dom";
import adminService from "../services/AdminService";
import '../styles/tournament.css';

function Matches({ matches,status, setStatus }) {
  const user = useAuth();
  const naviage = useNavigate();
  const params = useParams();

  const handleUpdate = (id) => {
    naviage(`/tournament/${params.id}/match/${id}`)
  };

  const handleDelete = async (id) => {
    await adminService.deleteMatch(id).then(() => {
      setStatus(!status)
    });
  };

  return (
    <center>
      <h2>All Matches</h2>
      <div className="table">
        <table>
          <thead>
            <tr className="thead">
              <th>Home Team</th>
              <th>Away Team</th>
              <th>Result</th>
              {user.role === "Admin" && <th>Action</th>}
            </tr>
          </thead>
          <tbody className="tableBody">
            {matches.map((match) => (
              <tr key={match.id}>
                <td>{match.teamHomeName}</td>
                <td>{match.teamAwayName}</td>
                <td>{match.result}</td>
                {user.role === "Admin" && (
                  <td className="action-buttons">
                    <Button onClick={() => handleUpdate(match.id)}>Update</Button>
                    <Button onClick={() => handleDelete(match.id)}>Delete</Button>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </center>
  );
}

export default Matches;
