import React from "react";
import { Link } from "react-router-dom";
import "../styles/home.css";
import { useAuth } from "../services/AuthContext";

import Button from "../common/Button";
function Navbar() {
  const user = useAuth();
  return (
    <header>
      <nav>
        <Link to="/">HOME</Link>
        {(user.token) && (
        <Link to="/tournaments">TOURNAMENTS</Link>
        )}
        {(user.role === "Admin" || user.role === "Teamleader") && (
        <Link to="/players">PLAYERS</Link>
        )}
        {(user.role === "Admin" || (user.role === "Player" && !user.isInTeam)) && (
          <Link to="/teams">TEAMS</Link>
        )}
        {(user.role === "Player" && !user.isInTeam)&& (
          <Link to="/teamleader">BECOME TEAMLEADER</Link>
        )}
        {user.token !== "" ? (
          <>
          {user.role === "Admin" && (
            <Link to="/admin">ADMIN DASHBOARD</Link>
          )}
            <Link to="/profile">PROFILE</Link>
            <Link to="/logout" onClick={() => user.logout()}>
              LOGOUT
            </Link>
          </>
        ) : (
          <>
            <Link to="/register">REGISTER</Link>
            <Link to="/login">LOGIN</Link>
          </>
        )}
      </nav>
    </header>
  );
}
export default Navbar;