import React from 'react';
import '../styles/tournament.css';

function Filter({ filtering, handleFilterChange }) {
    return (
        <div className='filtering'>
            <p style={{fontSize:'20px'}}>Apply filters:</p>
            <input type="text" name="name" value={filtering.name} onChange={handleFilterChange} placeholder="Name"/>
            <input type="text" name="description" value={filtering.description} onChange={handleFilterChange} placeholder="Description"/>
            <input type="text" name="location" value={filtering.location} onChange={handleFilterChange} placeholder="Location"/>
            <input type="date" name="date" value={filtering.date} onChange={handleFilterChange} placeholder="Date"/>
            <select name="status" value={filtering.status} onChange={handleFilterChange}>
                <option value="">All</option>
                <option value="Upcoming">Upcoming</option>
                <option value="Past">Past</option>
            </select>
        </div>
    );
}

export default Filter;