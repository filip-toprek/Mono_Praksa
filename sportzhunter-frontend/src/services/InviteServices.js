import axios from "axios";

export class InviteService{

    async getInvites(playerId){
        try{
            const response = await axios.get(`https://localhost:44323/api/invite/${playerId}`,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
            return response.data;
        }
        catch (error){
            console.error('Error fetching invites: ', error);
        }
    }

    async sendInvite(UserId, TeamId){
        try{
            const response = await axios.post(`https://localhost:44323/api/invite`, {UserId, TeamId},
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});

            return response.data;
        }
        catch (error){
            console.error('Error sending an invite: ', error);
        }
    }

    async acceptDeclineInvite(data){
        try{
            await axios.put(`https://localhost:44323/api/invite`, data,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
        } catch (error){
            console.error('Error occured: ', error);
        }
    }

    async deleteInvite(invitationId){
        try{
            await axios.delete(`https://localhost:44323/api/invite/${invitationId}`,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
        }
        catch(error){
            console.error('Error deleting an invitation: ', error);
        }
    }

    async checkIfInviteWasSent(TeamId){
        try{
            const response = await axios.get(`https://localhost:44323/api/invite/check/${TeamId}`,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
            return response;
        }
        catch(error){
            console.error(error);
        }
    }
}