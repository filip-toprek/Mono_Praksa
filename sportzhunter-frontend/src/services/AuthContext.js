import { createContext, useContext, useState } from 'react';
import userService from './UserService';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);


const AuthProvider = ({ children }) => {
    const emptyGuid = '00000000-0000-0000-0000-000000000000';
    const [token, setToken] = useState(localStorage.getItem("AuthToken") || "");
    const [role, setRole] = useState(localStorage.getItem("Role") || "");
    const [userId, setUserId] = useState(localStorage.getItem("UserId") || "");
    const [isInTeam, setisInTeam] = useState(localStorage.getItem("TeamId") !== emptyGuid || false);

    const login = async (username, password) => {
        try {
            const response = await userService.login(username, password);
            if(response.status == 200) {
                setToken(response.data.access_token);
                setUserId(response.data.userId);
                setRole(response.data.role);
                setisInTeam(response.data.teamId);
                localStorage.setItem("AuthToken", response.data.access_token);
                localStorage.setItem("UserId", response.data.userId);
                localStorage.setItem("Role", response.data.role);
                localStorage.setItem("SportId", response.data.sportId);
                localStorage.setItem("TeamId", response.data.teamId);

            }
            return response;
        }
        catch (error) {
            return error;
        }
    }

    const logout = async () => {
        try {
            const response = await userService.logout();
            if(response.status == 200) {
                setToken("");
                localStorage.removeItem("AuthToken");
                localStorage.removeItem("UserId");
                localStorage.removeItem("Role");
                localStorage.removeItem("SportId");
                localStorage.removeItem("TeamId");
                window.location.href = "/";
            }
            return response;
        }
        catch (error) {
            return error;
        }
    }

  return (
    <AuthContext.Provider value={{ token, role, userId, isInTeam, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;
