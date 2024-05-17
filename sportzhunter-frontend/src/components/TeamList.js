import React, { useState, useEffect } from "react";
import { fetchTeams, fetchTeamLeader } from "../services/TeamService";
import FilterTeam from "./FilterTeam";
import Team from "./Team";
import '../styles/tournament.css';
import Button from "../common/Button";

function TeamList({
  filters,
  setFilters,
  teamList,
  fetchTeam,
  status,
  setStatus,
}) {
  const [teams, setTeams] = useState([]);
  const [filteredTeams, setFilteredTeams] = useState([]);
  const [teamLeaders, setTeamLeaders] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const teamLeadersData = await fetchTeamLeader();
        setTeamLeaders(teamLeadersData);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }
    fetchData();
  }, [status]);

  async function applyFilters(filters) {
    fetchTeam();
  }

  return (
    <div className="tournaments">
      <center>
        <h1>Teams</h1>
      </center>
      <div className="filteringNear">
          <div className="filtering">
            <FilterTeam
              onSubmit={() => applyFilters}
              filters={filters}
              setFilters={setFilters}
            />
          </div>
            <table className="teamTable">
              <thead></thead>
              <tbody>
                {teamList.map((team) => (
                  <Team team={team} status={status} setStatus={setStatus}></Team>
                ))}
              </tbody>
            </table>
        </div>
    </div>
  );
}

export default TeamList;
