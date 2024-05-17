import React, { useState, useEffect } from "react";
import { fetchTeamLeader } from "../services/TeamService";
import Button from "../common/Button";

function FilterTeam({ onSubmit, filters, setFilters }) {
  const [showFilters, setShowFilters] = useState(false);
  const [teamLeaders, setTeamLeaders] = useState([]);
  useEffect(() => {
    async function fetchTeamLeadersData() {
      try {
        const teamLeadersData = await fetchTeamLeader();
        setTeamLeaders(teamLeadersData);
      } catch (error) {
        console.error("Error fetching team leaders:", error);
      }
    }

    fetchTeamLeadersData();
  }, []);

  function handleInputChange(e) {
    const { name, value } = e.target;
    setFilters((prevFilters) => ({
      ...prevFilters,
      [name]: value,
    }));
  }

  function handleSubmit(e) {
    e.preventDefault();
    onSubmit(filters);
  }

  function toggleFilters() {
    setShowFilters(!showFilters);
  }

  return (
    <div className="form-container">
      <Button onClick={toggleFilters} className="filterbutton">
        Filters
      </Button>

      {showFilters && (
        <form onSubmit={handleSubmit}><br/>
          <label>
            Sort by:
            <select
              name="sortby"
              value={filters.sortby}
              onChange={handleInputChange}
            >
              <option value="DateCreated">Date Created</option>
              <option value="Name">Team name</option>
              <option value="Description">Description</option>
              <option value="TeamLeaderId">TeamLeader name</option>
            </select>
          </label>
          <label>
            Sort order:
            <select
              name="sortorder"
              value={filters.sortorder}
              onChange={handleInputChange}
            >
              <option value="desc">DESC</option>
              <option value="asc">ASC</option>
            </select>
          </label>
          <label>
            Team name:
            <input
              type="text"
              name="name"
              value={filters.name}
              onChange={handleInputChange}
            />
          </label>
          <label>
            Description:
            <input
              type="text"
              name="description"
              value={filters.description}
              onChange={handleInputChange}
            />
          </label>
          <label>
            Team leader:
            <select
              name="teamleaderid"
              value={filters.teamleaderid}
              onChange={handleInputChange}
            >
              <option value="">All</option>
              {teamLeaders.map((teamLeader) => (
                <option key={teamLeader.teamLeaderId} value={teamLeader.teamLeaderId}>
                  {teamLeader.teamLeaderName}
                </option>
              ))}
            </select>
          </label>
        </form>
      )}
    </div>
  );
}

export default FilterTeam;
