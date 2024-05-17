import React from 'react';
import '../styles/tournament.css';

function Sorting({ sorting, handleSortChange }) {
    return (
        <div className='sorting'>
            <p style={{fontSize:'20px'}}>Sort by:</p>
            <select name="orderBy" value={sorting.orderBy} onChange={handleSortChange}>
                <option value="name">Name</option>
                <option value="description">Description</option>
                <option value="location">Location</option>
                <option value="date">Date</option>
            </select>
            <select name="sortOrder" value={sorting.sortOrder} onChange={handleSortChange}>
                <option value="asc">Ascending</option>
                <option value="desc">Descending</option>
            </select>
        </div>
    );
}

export default Sorting;