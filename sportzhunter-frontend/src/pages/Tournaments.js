import React from 'react';
import { useState, useEffect } from 'react';
import TournamentService from '../services/TournamentService.js';
import Filter from '../components/TournamentsFiltering.js';
import Sort from '../components/TournamentsSorting.js';
import Pagination from '../components/Paging.js';
import TournamentTable from '../components/TournamentsTable.js';
import Button from '../common/Button.jsx';
import { useNavigate } from 'react-router-dom';
import '../styles/tournament.css';
import { useAuth } from '../services/AuthContext.js';

function Tournaments() {
    const [tournaments, setTournaments] = useState([]);
    const [filtering, setFiltering] = useState({name: '', description: '', location: '', date: '', status: ''});
    const [paging, setPaging] = useState({pageNumber: 1, pageSize: 5, totalPages: 0});
    const [sorting, setSorting] = useState({orderBy: 'date', sortOrder: 'asc'});
    const [loading, setLoading] = useState(true);
    const user = useAuth();

    const nextPage = () => {
        if(paging.pageNumber < paging.totalPages){
            setPaging({...paging, pageNumber: paging.pageNumber + 1});
        }
    }

    const previousPage = () => {
        if(paging.pageNumber > 1){
            setPaging({...paging, pageNumber: paging.pageNumber - 1});
        }
    }

    const handlePageChange = (event) => {
        setPaging({...paging, pageSize: event.target.value, pageNumber: 1});
    }

    const handleFilterChange = (event) => {
        setFiltering({...filtering, [event.target.name]: event.target.value});
    }

    const handleSortChange = (event) => {
        setSorting({...sorting, [event.target.name]: event.target.value});
    }

    const fetchTournaments = async () => {
        try {
            setLoading(true);
            await TournamentService.getAllTournaments(filtering, paging, sorting)
            .then(response => {
                setTournaments(response.data.list);
                setPaging({...paging, totalPages: response.data.pageCount});
            });
        } catch (error) {
            console.error(`Error: ${error}`);
        }
        finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        fetchTournaments();
        const delaySearch = setTimeout(() => {
            fetchTournaments();
        }, 500);
        return () => clearTimeout(delaySearch);
    }, [paging.pageNumber, paging.pageSize, filtering, sorting]);

    let navigate = useNavigate();
    const routeChange = () => {
        let path = '/tournament/create';
        navigate(path);
    }

    return (
        <div>
            <div className='adminTournament'>
                <h1 style={{padding:'10px'}}>Tournaments</h1>
                {user.role === "Admin" && (
                        <div className='createBtn'>
                            <Button onClick={routeChange} text='Create Tournament'>Create Tournament</Button>
                        </div>
                    )}
            </div>
            <div className='tournaments'>
                <div className='filteringNear'>
                    <div className='filtersort'>
                        <Sort sorting={sorting} handleSortChange={handleSortChange}/>
                        <Filter filtering={filtering} handleFilterChange={handleFilterChange}/>
                    </div>
                    <TournamentTable fetchTournaments={fetchTournaments} tournaments={tournaments} loading={loading}/>
                </div>
                <Pagination paging={paging} previousPage={previousPage} nextPage={nextPage} handlePageChange={handlePageChange}/>
                
            </div>
        </div>
    );
}

export default Tournaments;