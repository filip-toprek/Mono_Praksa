import { useState, useEffect } from 'react';
import PlayerTable from '../components/Tables/PlayerTable';
import PlayerFiltering from '../components/PlayerFiltering';
import Pagination from '../components/Paging';
import PlayerService from '../services/PlayerService';
import Sorting from '../common/Sorting';
import { useAuth } from '../services/AuthContext';
import '../styles/tournament.css';

export default function PlayersPage() {
    const [playerList, setPlayerList] = useState([{}]);
    const [paging, setPaging] = useState({pageNumber: 1, pageSize: 5, totalPages: 0});
    const [filters, setFilters] = useState({sportCategoryId: localStorage.getItem("SportId"), positionId: '', countyId: '', ageCategory: ''});
    const [sorting, setSorting] = useState({sortBy: 'Height', sortOrder: 'ASC'});
    const user = useAuth();

    useEffect(() => {
        const fetchPlayers = async () => {
            try {
                await PlayerService.getPlayers(filters, paging, sorting)
                .then(response => {
                    setPlayerList(response.data.list);
                    if(response.data.pageCount !== paging.totalPages || response.data.pageSize !== paging.pageSize)
                        setPaging((prevData) => ({...prevData, totalPages: response.data.pageCount}));
                });
            } catch (error) {
                setPlayerList([]);
            }
        }
        fetchPlayers();
    }, [paging, filters, sorting]);

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

    return (
        <>
        <div className='tournaments'>
            <div className='filteringNear'>
                <div className='filtersort'>
                    <PlayerFiltering filters={filters} setFilters={setFilters}/>
                    <Sorting sorting={sorting} setSorting={setSorting}/>
                </div>
                <PlayerTable playerList={playerList} /> 
            </div>
            <Pagination paging={paging} previousPage={previousPage} nextPage={nextPage} handlePageChange={handlePageChange}/>
        </div>
        </>
    );
}