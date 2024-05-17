import axios from 'axios';

const applyFilters = (filtering, paging, sorting) => {
    let url = `https://localhost:44323/api/Tournament?pageNumber=${paging.pageNumber}&pageSize=${paging.pageSize}&orderBy=${sorting.orderBy}&sortOrder=${sorting.sortOrder}`;
    if (filtering.name !== '') {
        url += `&name=${filtering.name}`;
    }
    if (filtering.description !== '') {
        url += `&description=${filtering.description}`;
    }
    if(filtering.description !== ''){
        url += `&description=${filtering.description}`;
    }
    if(filtering.location !== ''){
        url += `&location=${filtering.location}`;
    }
    if(filtering.date !== ''){
        url += `&date=${filtering.date}`;
    }
    if(filtering.status !== ''){
        url += `&status=${filtering.status}`;
    }
    return url;
}

const getAllTournaments = (filtering, paging, sorting) => { 
    return axios.get(applyFilters(filtering, paging, sorting),
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const getTournament = (id) => {
    return axios.get(`https://localhost:44323/api/Tournament/${id}`,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}


const createTournament = (tournament) => {
    return axios.post('https://localhost:44323/api/Tournament', tournament,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const deleteTournament = (id) => {
    return axios.delete(`https://localhost:44323/api/Tournament/${id}`,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const updateTournament = (id, tournament) => {
    return axios.put(`https://localhost:44323/api/Tournament/${id}`, tournament,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const attendTournament = (data) => {
    return axios.post('https://localhost:44323/api/Attend', data,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const TournamentService = {
    getAllTournaments,
    createTournament,
    deleteTournament,
    updateTournament,
    getTournament,
    attendTournament
};

export default TournamentService;
