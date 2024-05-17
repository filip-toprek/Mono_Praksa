import React from "react";
import { useState, useEffect } from "react";
import ProfileService from "../services/ProfileService";
import { useAuth } from "../services/AuthContext";
import UserInfo from "../components/UserInfo";
import playerService from '../services/PlayerService';

function Profile() {
    const [userInfo, setUserInfo] = useState();
    const user = useAuth();

    const fetchProfileInfo = async () => {
        try {
            await ProfileService.getProfileInfo(user.userId)
                .then((response) => {
                    setUserInfo(response.data);
                });
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }

    async function leaveTeam() {
        try {
            await playerService.leaveTeam().then(() => {
                localStorage.setItem("TeamId", "00000000-0000-0000-0000-000000000000");
                });
            fetchProfileInfo();

        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }

    async function kickPlayer(data) {
        try {
            await playerService.kickPlayer(data);
            fetchProfileInfo();
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }

    useEffect(() => {
        fetchProfileInfo();
    }, []);

    return (
        <div>
            {userInfo ? <UserInfo leaveTeam={leaveTeam} kickPlayer={kickPlayer} userInfo={userInfo} /> : <p>Loading...</p>}
        </div>
  );
}

export default Profile;