import React from 'react';
import Button from '../common/Button';
import { useNavigate } from 'react-router-dom';
import userService from '../services/UserService';
import { useState, useEffect } from 'react';
import TeamInfo from './TeamInfo';
import { useAuth } from '../services/AuthContext';
function UserInfo({ userInfo, leaveTeam, kickPlayer }) {
    const [positionLabel, setPositionLabel] = useState();
    const user = useAuth();

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const formattedDate = `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
        return formattedDate;
    }

    let navigate=useNavigate();

    const handleRouteChange = (id) => {
        navigate(`/profile/edit/${id}`);
    }

    const handleRouteChangeInvites = (id) => {
        navigate(`/invites/${id}`);
    }

    useEffect(() => {
        const fetchData = async () => {
            const positionName = await getPrefferedPosition(userInfo.preferredPositionId);
            setPositionLabel(positionName);
        };
        fetchData();
    }, [userInfo.preferredPositionId]);

    const getPrefferedPosition = async (id) => {
        const position = userInfo.preferredPositionId;
        const response = await userService.getPrefPositions();
        const positionData = response.data.find(element => element.id === position);
        return positionData ? positionData.positionName : null;
    }

    return (
        <center>
            <h2>User Profile</h2>
            <div className='detailsCardProfile'>
                <p><strong>First Name: </strong> {userInfo.user.firstName}</p>
                <p><strong>Last Name: </strong>{userInfo.user.lastName}</p>
                <p><strong>Username: </strong> {userInfo.user.username}</p>
                <p><strong>Height: </strong> {userInfo.height}</p>
                <p><strong>Weight: </strong> {userInfo.weight}</p>
                <p><strong>Date of Birth: </strong> {formatDate(userInfo.dateOfBirth)}</p>
                {user.role === "Player" &&<p><strong>Preffered position: </strong> {positionLabel !== null ? positionLabel : 'Loading...'}</p>}
                <p><strong>Description: </strong> {userInfo.description}</p>
                {(user.role === "Player" || user.role === "Teamleader") && <TeamInfo kickPlayer={kickPlayer} teamInfo={userInfo.team} leaveTeam={leaveTeam}/>}
            </div>
            <div className='profileBtn'>
                    <Button onClick={() => handleRouteChange(userInfo.id)}>Edit</Button>
            </div>
            <div className='profileBtn'>
                    <Button onClick={() => handleRouteChangeInvites(userInfo.id)}>Invites</Button>
            </div>
        </center>
    );
}

export default UserInfo;