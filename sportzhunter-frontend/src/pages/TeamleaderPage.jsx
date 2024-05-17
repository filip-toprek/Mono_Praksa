import { useEffect, useState } from "react";
import playerService from "../services/PlayerService";
import userService from "../services/UserService";
import { useNavigate } from "react-router-dom";
import Button from "../common/Button";

export default function TeamleaderPage() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        leaderDescription: '',
        teamName: '',
        teamDescription: '',
        sportCategoryId: localStorage.getItem('SportCategoryId'),
    });
    const [sportCategoryList, setSportCategoryList] = useState([]);

    const getSportCategories = async () => {
        const response = await userService.getSportCategories();
        setSportCategoryList(response.data);
        setFormData({...formData, sportCategoryId: response.data[0].id});
    }
    useEffect(() => {
        getSportCategories();
    }
    , []);

    const handleChange = (e) => {
        setFormData({...formData, [e.target.name]: e.target.value});
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        const response = await playerService.postTeamleader(formData);
        navigate('/');
    }

    return (
        <div className="formDiv">
            <h1>Become a teamleader</h1>
            <p>Fill out the form to become a teamleader</p>
            <form onSubmit={handleSubmit} className="form">
                <label htmlFor="teamName">Team name</label>
                <input type="text" id="teamName" name="teamName" required onInput={handleChange}></input><br/>
                <label htmlFor="teamDescription">Team description</label><br/>
                <textarea id="teamDescription" name="teamDescription" required onInput={handleChange}></textarea><br/>
                <label htmlFor="leaderDescription">Teamleader description</label><br/>
                <textarea id="leaderDescription" name="leaderDescription" required onInput={handleChange}></textarea><br/>
                     <br/>              
                <Button type="submit">Submit</Button>
            </form>
        </div>
    );
}