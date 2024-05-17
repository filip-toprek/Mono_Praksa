import React, { useState, useEffect } from "react";
import TeamList from "../components/TeamList";
import TeamService from "../services/TeamService";
import teamService, { fetchTeams } from "../services/TeamService";
import Pagination from "../components/Paging";
function Teams() {
  const [teamList, setTeamList] = useState([{}]);
  const [status, setStatus] = useState(false);
  const [filters, setFilters] = useState({
    name: "",
    description: "",
    teamleaderid: "",
    datecreated: "",
    sortby: "",
    sortorder: "",
    pageNumber: 1,
    pageSize: 5,
    totalPages: 0,
  });
  const fetchTeam = async () => {
    try {
      await teamService.fetchTeams(filters).then((response) => {
        setTeamList(response.data.list);
        if (
          response.data.pageCount !== filters.totalPages ||
          response.data.pageSize !== filters.pageSize
        ) {
          setFilters({ ...filters, totalPages: response.data.pageCount });
        }
      });
    } catch (error) {
      setTeamList([]);
    }
  };
  useEffect(() => {
    fetchTeam();
  }, [filters, status]);

  const nextPage = () => {
    if (filters.pageNumber < filters.totalPages) {
      setFilters({ ...filters, pageNumber: filters.pageNumber + 1 });
    }
  };

  const previousPage = () => {
    if (filters.pageNumber > 1) {
      setFilters({ ...filters, pageNumber: filters.pageNumber - 1 });
    }
  };

  const handlePageChange = (event) => {
    setFilters({ ...filters, pageSize: event.target.value, pageNumber: 1 });
  };
  return (
    <div>
      <TeamList
        fetchTeam={fetchTeam}
        teamList={teamList}
        filters={filters}
        setFilters={setFilters}
        status={status}
        setStatus={setStatus}
      />
      <Pagination
        paging={filters}
        previousPage={previousPage}
        nextPage={nextPage}
        handlePageChange={handlePageChange}
      />
    </div>
  );
}

export default Teams;
