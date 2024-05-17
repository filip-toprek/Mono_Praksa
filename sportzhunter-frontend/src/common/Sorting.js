export default function Sorting({ sorting, setSorting }) {

    const handleSortChange = (event) => {
        setSorting({...sorting, [event.target.name]: event.target.value});
    }
    return (
        <div className='sorting'>
            <h2>Sort players</h2>
            <select name="sortBy" onChange={handleSortChange}>
                <option value="Height">Height</option>
                <option value="Weight">Weight</option>
                <option value="DOB">Age</option>
            </select>
            <select name="sortOrder" onChange={handleSortChange}>
                <option value="ASC">Ascending</option>
                <option value="DESC">Descending</option>
            </select>
        </div>
    );
}