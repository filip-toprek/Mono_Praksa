import React from 'react';
import '../styles/tournament.css';  

function Pagination({ paging, previousPage, nextPage, handlePageChange }) {
    return (
        <div className='pagination'>
            <button onClick={previousPage}>{'<<'}</button>
            <span>{paging.pageNumber}</span>
            <button onClick={nextPage}>{'>>'}</button>
            <select onChange={handlePageChange}>
                <option value="5">5</option>
                <option value="10">10</option>
                <option value="15">15</option>
            </select>
        </div>
    );
}

export default Pagination;