import { useState, useEffect } from 'react';
import userService from "../services/UserService";
import { useAuth } from '../services/AuthContext';

export default function PlayerFiltering({filters, setFilters}) {
    const [sportCategory, setSportCategory] = useState([{}]);
    const [positionList, setPositionList] = useState([{}]);
    const [countyList, setCountyList] = useState([{}]);
    const user = useAuth();

    useEffect(() => {
        async function fetchData() {
            try{
                const position_response = await userService.getPrefPositions();
                setPositionList(position_response.data);
                const county_response = await userService.getCounties();
                setCountyList(county_response.data);
                const sportCategory_response = await userService.getSportCategories();
                setSportCategory(sportCategory_response.data);                
            }catch(error){
                console.error(`Error: ${error}`);
            }
        }
        fetchData();
      }, []);

    return (
        <>
            <div className='filtering'>
            <h2>Filter players</h2>
            <select onChange={(e) => setFilters({...filters, ageCategory: e.target.value})}>
                <option value="">Select an age category</option>
                <option value="1">18-25</option>
                <option value="2">Over 25</option>
            </select>
            <select onChange={(e) => setFilters({...filters, countyId: e.target.value})}>
                <option value="">Select a county</option>
                {countyList.map(county => (
                    <option key={county.id} value={county.id}>{county.countyName}</option>
                ))}
            </select>
            <select onChange={(e) => setFilters({...filters, positionId: e.target.value})}>
                <option value="">Select a position</option>
                {positionList.map(position => (
                    <option key={position.id} value={position.id}>{position.positionName}</option>
                ))}
            </select>
                {user.role == "Admin" ? (
                <select onChange={(e) => setFilters({...filters, sportCategoryId: e.target.value})}>
                    <option value="">Select a sport</option>
                    {sportCategory.map(sport => (
                        <option key={sport.id} value={sport.id}>{sport.categoryName}</option>
                    ))}
                </select>
            ) : null}
        </div>
    </>
    );
    }