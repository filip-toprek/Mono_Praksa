import axios from "axios";

async function getProfileInfo (id) {
    return await axios.get(`https://localhost:44323/api/Player/profile/${id}`,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const updateProfileInfo = (id, profileInfo) => {
    return axios.put(`https://localhost:44323/api/Player/${id}`, profileInfo,
    {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('AuthToken')}`,
        }
    });
}

const ProfileService = {
    getProfileInfo,
    updateProfileInfo
};

export default ProfileService;