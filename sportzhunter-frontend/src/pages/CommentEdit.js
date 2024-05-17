import { useEffect, useState } from "react";
import { CommentService } from "../services/CommentService";
import { useParams } from "react-router-dom";

export default function CommentEdit(){
    const commentService = new CommentService(); 
    const [comment, setComment] = useState({});
    const params  = useParams();

    useEffect(() => {
        commentGet();
    }, []);

    async function commentGet(){
        const commentData = await commentService.getComment(params.id);
        setComment(commentData.data);
    }

    function handleTextChange(event) {
        setComment({ ...comment, text: event.target.value });
    }

    async function handleSave() {
        await commentService.updateComment(params.id, {text: comment.text, isActive: comment.isActive} );
    }

    return (
        <center>
            <div className="container">
                <textarea id="textbox" rows="4" cols="50" value={comment.text} onChange={handleTextChange}></textarea>
                <br></br>
                <button id="saveButton" onClick={handleSave}>Save</button>
            </div>
        </center>
    );
}