import React, { useState } from "react";
import { CommentService } from "../services/CommentService";
import { useEffect } from "react";
import Comment from "./Comment";
import { useParams } from "react-router-dom";
import { useAuth } from "../services/AuthContext";
import Button from "../common/Button";

export default function CommentList({status, setStatus}){
    const commentService = new CommentService(); 
    const [comments, setComments] = useState([]);
    const [pageSize, setPageSize] = useState(3);
    const params = useParams();
    const user = useAuth();

    useEffect(() => {
        commentsGet();
    }, [pageSize, status])

    async function commentsGet(){
        const commentsData = await commentService.getComments(params.id, pageSize); 
        setComments(commentsData.list);
    }

    function handleLoadMore() {
        setComments([]);
        setPageSize(prevPageSize => prevPageSize + 3);
    }

    return(
        <div>
            <hr/>
            {comments.length === 0 ? <p>Be first to comment.</p> : null}
            {comments.map(comment => (
                <React.Fragment key={comment.id}>
                    <Comment 
                        username={comment.username} 
                        text={comment.text} 
                        dateCreated={comment.dateCreated} 
                        dateUpdated={comment.dateUpdated} 
                        commentId={comment.id}/>
                    <br />
                </React.Fragment>
            ))}
            {pageSize === comments.length ? (
                <Button onClick={handleLoadMore}>Load more...</Button>
            ) : null}
        </div>
    );
}