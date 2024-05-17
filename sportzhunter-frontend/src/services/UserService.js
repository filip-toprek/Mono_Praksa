import axios from "axios";


async function login(username, password) {
    return await axios.post("https://localhost:44323/Api/User/login", {
      username: username,
      password: password,
      grant_type: "password"
    },  {headers: {'content-type': 'application/x-www-form-urlencoded'}})
  }

async function register(data) {
    return await axios.post("https://localhost:44323/Api/User/Register", data);
}

async function logout() {
    return await axios.get("https://localhost:44323/Api/User", 
    {headers: {'Authorization': `Bearer ${localStorage.getItem("AuthToken")}`}});
}

async function getSportCategories() {
    return await axios.get("https://localhost:44323/Api/User/SportCategories");
}

async function getPrefPositions() {
    return await axios.get("https://localhost:44323/Api/User/PrefPositions");
}

async function getCounties() {
    return await axios.get("https://localhost:44323/Api/User/Counties");
}

const userService = {
    login,
    register,
    logout,
    getSportCategories,
    getPrefPositions,
    getCounties
};

export default userService;