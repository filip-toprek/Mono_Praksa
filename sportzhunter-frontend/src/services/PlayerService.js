import axios from "axios";
import { applyFilter } from "../common/Filtering";

async function getPlayers(filters, paging, sorting) {
    let url = applyFilter(filters, `https://localhost:44323/api/teamLeader`);
    url += `&pageNumber=${paging.pageNumber}&pageSize=${paging.pageSize}`;
    url += `&sortBy=${sorting.sortBy}&sortOrder=${sorting.sortOrder}`;

    return await axios.get(url,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function getPlayer(id) {
    return await axios.get(`https://localhost:44323/api/player/${id}`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}})
}

async function postTeamleader(data) {
    return await axios.post(`https://localhost:44323/api/player/`, data,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function leaveTeam() {
    return await axios.put(`https://localhost:44323/api/player`, null,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function kickPlayer(data) {
    return await axios.post(`https://localhost:44323/api/teamleader/`, data,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function getTeamLeader() {
    return await axios.get(`https://localhost:44323/api/teamleader`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function getTeamLeaderProfile() {
    return await axios.get(`https://localhost:44323/api/teamleader/profile`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

const playerService = {
    getPlayers,
    getPlayer,
    postTeamleader,
    leaveTeam,
    kickPlayer,
    getTeamLeader,
    getTeamLeaderProfile
};

export default playerService;