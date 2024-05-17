import axios from "axios";

export class CommentService{

    // tournamentsId, pageSize, pageNumber
    async getComments(tournamentId, pageSize){
        try{
            const response = await axios.get(`https://localhost:44323/api/comment/${tournamentId}?pageSize=${pageSize}`,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
            return response.data;
        }
        catch (error){
            console.error('Error fetching comments: ', error);
        }
    }

    async getComment(commentId){
        try{
            const response = await axios.get(`https://localhost:44323/api/comment/GetOneComment/${commentId}`,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
            return response;
        }
        catch (error){
            console.error('Error fetching a comment: ', error);
        }
    }

    async postComment(data){
        try{
            const response = await axios.post(`https://localhost:44323/api/comment`, data,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
            return response.data;
        }
        catch (error){
            console.error('Error adding a comment: ', error);
        }
    }

    // Passes guid commentId and Comment class to update updatedBy, dateUpdated
    // Passing commentId
    async updateComment(id, data){
        try{
            await axios.put(`https://localhost:44323/api/comment/${id}`, data,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
        } catch (error){
            console.error('Error updating a comment: ', error);
        }
    }

    async deleteComment(commentId){
        try{
            await axios.delete(`https://localhost:44323/api/comment/${commentId}`,
            {headers: {"Authorization": `Bearer ${localStorage.getItem("AuthToken")}`}});
        }
        catch(error){
            console.error('Error deleting a comment: ', error);
        }
    }
}