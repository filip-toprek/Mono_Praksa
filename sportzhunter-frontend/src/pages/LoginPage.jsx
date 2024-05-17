import Button from "../common/Button";
import { useState } from "react";
import { useAuth } from "../services/AuthContext";
import '../styles/Common.css'
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
    const [user, setUser] = useState({username: "", password: ""});
    const [error, setError] = useState(null);
    const auth = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setUser({...user, [e.target.name]: e.target.value});
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        const response = await auth.login(user.username, user.password);
        if(response.status === 200) {
            setError(null);
            navigate("/");
        }
        else {
            setError(response.data);
        }
    }

    return (
        <div className="formDiv">
        <h1>Login</h1>
        <form onSubmit={handleSubmit} className="form" style={{margin:'50px 0px 110px 0px'}}>
            {error && <p>{error.message}</p>}
            <label htmlFor="username">Username</label>
            <input type="username" id="username" name="username" onInput={handleChange} required/>
            <label htmlFor="password">Password</label>
            <input type="password" id="password" name="password" onInput={handleChange} required/>
            <Button>Login</Button>
        </form>
        </div>
    );
    }