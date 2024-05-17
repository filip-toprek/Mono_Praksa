import React, { useEffect } from "react";
import { useState } from "react";
import ProfileService from "../services/ProfileService";
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../services/AuthContext";
import userService from "../services/UserService";
import Button from "../common/Button";

function EditProfile() {
    const [userInfo, setUserInfo] = useState();
    const [formData, setFormData] = useState({});
    let navigate = useNavigate();
    const user = useAuth();
    const params = useParams();
    const [positionList, setPositionList] = useState([{}]);

    const fetchProfileInfo = async () => {
        try {
            const position_response = await userService.getPrefPositions();
            setPositionList(position_response.data);
            await ProfileService.getProfileInfo(user.userId)
                .then((response) => {
                    setUserInfo(response.data);
                    setFormData({
                        firstName: response.data.user.firstName,
                        lastName: response.data.user.lastName,
                        username: response.data.user.username,
                        description: response.data.description,
                        height: response.data.height,
                        weight: response.data.weight,
                        preferredPositionId: response.data.preferredPositionId,
                        positionName: response.data.positionName
                    });
                });
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }

    useEffect(() => {
        fetchProfileInfo();
    }, []);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData({...formData, [name]: value});
    }
    
    const handleSubmit = async (event) => {
        event.preventDefault();
        try{
            delete formData.firstName;
            delete formData.lastName;
            delete formData.username;

            await ProfileService.updateProfileInfo(params.id, formData);
            alert("Profile updated successfully");
            navigate('/profile');
        }catch(error){
            console.error(`Error: ${error}`);
            alert("Error updating profile");
        }
    }

    return (
        <div className="formDiv">
            <h2>Edit Profile</h2>
            <form onSubmit={handleSubmit} className="form">
                <label htmlFor="firstName">First Name:</label>
                <input
                    type="text"
                    id="firstName"
                    name="firstName"
                    value={formData.firstName}
                    onChange={handleInputChange}
                    disabled
                /><br/>

                <label htmlFor="lastName">Last Name:</label>
                <input
                    type="text"
                    id="lastName"
                    name="lastName"
                    value={formData.lastName}
                    onChange={handleInputChange}
                    disabled
                /><br/>

                <label htmlFor="username">Username:</label>
                <input
                    type="text"
                    id="username"
                    name="username"
                    value={formData.username}
                    disabled
                /><br/>

                <label htmlFor="height">Height:</label>
                <input
                    type="number"
                    id="height"
                    name="height"
                    value={formData.height}
                    onChange={handleInputChange}
                    required
                /><br/>

                <label htmlFor="weight">Weight:</label>
                <input
                    type="number"
                    id="weight"
                    name="weight"
                    value={formData.weight}
                    onChange={handleInputChange}
                    required
                /><br/>

                <label htmlFor="dateOfBirth">Date of Birth:</label>
                <input
                    type="date"
                    id="dateOfBirth"
                    name="dateOfBirth"
                    value={userInfo? userInfo.dateOfBirth.toString().split('T')[0] : ""}
                    disabled
                /><br/>
                {user.role === "Player" && <>
                <label htmlFor="description">Preferred position:</label>

                <select onChange={(e) => setFormData({...formData, preferredPositionId: e.target.value})}>
                {positionList.map(position => (
                    <option key={position.id} value={position.id} selected={formData.preferredPositionId === position.id}>{position.positionName}</option>
                    ))}
                </select><br/></>}

                <label htmlFor="description">Description:</label>
                <input
                    type="text"
                    id="description"
                    name="description"
                    value={formData.description}
                    onChange={handleInputChange}
                /><br/>

                <Button type="submit">Save</Button>
            </form>
        </div>
   );
}

export default EditProfile;