import React from "react";
import "../styles/comment.css";
import { CommentService } from "../services/CommentService";
import { Link } from "react-router-dom";
import Button from "../common/Button";
//import CgProfile from "react-icons/cg";

export default function Comment(params)
{
    const commentService = new CommentService();

    const formatDate = (date) => {
        const options = { year: "numeric", month: "long", day: "numeric" };
        return new Date(date).toLocaleDateString(undefined, options);
    };

    const isSameDate = (date1, date2) => {
        return new Date(date1).toDateString() === new Date(date2).toDateString();
    };

    const deleteComment = async () =>{
        await commentService.deleteComment(params.commentId).then(() => {
            window.location.reload();
        });
    }

    return (
        <>
            <div className="comment">
                <div className="commentInfo">
                    <div className="profileIcon">
                        { /*<CgProfile /> */}
                        <h2>{params.username}</h2>
                    </div>
                    <p className="speed-text">{isSameDate(params.dateCreated, params.dateUpdated)
                        ? formatDate(params.dateCreated)
                        : `${formatDate(params.dateUpdated)} - Comment edited`}
                    </p>
                    <div className="options">
                    <div className="dropdown" onclick="toggleDropdown()">
                        <Button className="dropbtn" style={{width: "20px", paddingLeft: "10px"}}>&#8942;&#8942;&#8942;</Button>
                        <div className="dropdown-content" id="dropdownMenu">
                            <Link to={{
                                        pathname: `/editComment/${params.commentId}`,
                                        state: {id: params.commentId}
                                }}>Edit</Link>
                            <a href="#" onClick={deleteComment}>Remove</a>
                        </div>
                    </div>
                </div>
                </div>
                <p className="commentText">{params.text}</p>
            </div>
        </>
    );
}