import axios from 'axios';

async function getUsers(sorting, paging) {
    let url = "https://localhost:44323/api/admin/users";
    if (sorting.sortBy) {
        url += `?orderBy=${sorting.sortBy}`;
    }
    if (sorting.sortOrder) {
        url += `&sortOrder=${sorting.sortOrder}`;
    }
    if (paging.pageNumber) {
        url += `&pageNumber=${paging.pageNumber}`;
    }
    if (paging.pageSize) {
        url += `&pageSize=${paging.pageSize}`;
    }

    return await axios.get(url,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function deleteUser(id) {
    return await axios.delete(`https://localhost:44323/api/admin/user/${id}`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function getPendingApplications() {
    return await axios.get("https://localhost:44323/api/admin/",
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function approveApplication(id) {
    return await axios.put(`https://localhost:44323/api/admin?id=${id}`, null,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function disapproveApplication(id) {
    return await axios.delete(`https://localhost:44323/api/admin?id=${id}`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function getAttends() {
    return await axios.get("https://localhost:44323/api/attend",
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function approveAttend(id) {
    return await axios.put(`https://localhost:44323/api/admin/attend/${id}`, null,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function disapproveAttend(id) {
    return await axios.delete(`https://localhost:44323/api/admin/attend/${id}`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});

}

async function createMatch(match) {
    return await axios.post("https://localhost:44323/api/match", match,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function editMatch(id, match) {
    return await axios.put(`https://localhost:44323/api/match?id=${id}`, match,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}

async function deleteMatch(id) {
    return await axios.delete(`https://localhost:44323/api/match/${id}`,
    {headers: {'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`}});
}



const adminService = {
    getUsers,
    deleteUser,
    getPendingApplications,
    approveApplication,
    disapproveApplication,
    getAttends,
    approveAttend,
    disapproveAttend,
    createMatch,
    editMatch,
    deleteMatch
};

export default adminService;