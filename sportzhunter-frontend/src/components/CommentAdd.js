import { useEffect, useState } from "react";
import { CommentService } from "../services/CommentService";
import { useParams } from "react-router-dom";
import Button from "../common/Button";
import "../styles/tournament.css";

export default function CommentAdd({status, setStatus}){
    const commentService = new CommentService();
    const [text, setText] = useState("");
    const params = useParams();

    // Function to handle changes in textarea
    function handleTextChange(event) {
        setText(event.target.value);
    }

    // TournamentId from url and Text
    async function handleAdd() {
        try{
            await commentService.postComment({tournamentId: params.id, text: text}).then(() => {
                setText("");
                setStatus(!status);
            });
        }catch(error){
        }
    }

    return (
        <center>
            <div className="container">
                <textarea id="textbox" rows="4" cols="50" value={text} onChange={handleTextChange}></textarea>
                <br></br>
                <Button id="saveButton" onClick={handleAdd}>Add</Button>
            </div>
        </center>
    );
}