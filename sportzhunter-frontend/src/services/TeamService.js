import axios from "axios";

const API_BASE_URL = "https://localhost:44323/api";

const api = axios.create({
  baseURL: API_BASE_URL,
});

export async function fetchTeams(filter) {
  try {
    let url = "/Team";
    let params = "";

    if (filter) {
      params = Object.entries(filter)
        .filter(
          ([key, value]) =>
            value !== null && value !== undefined && value !== ""
        )
        .map(([key, value]) => `${key}=${encodeURIComponent(value)}`)
        .join("&");
    }

    if (params) {
      url += `?${params}`;
    }
    url += `&pageNumber=${filter.pageNumber}&pageSize=${filter.pageSize}`;

    const response = await api.get(url, {
      headers: { Authorization: `Bearer ${localStorage.getItem("AuthToken")}` },
    });
    return response;
  } catch (error) {
    console.error("Error fetching teams:", error);
    throw error;
  }
}
export async function fetchTeamById(id) {
  try {
    const response = await api.get(`/Team/${id}`, {
      headers: { Authorization: `Bearer ${localStorage.getItem("AuthToken")}` },
    });
    return response.data;
  } catch (error) {
    console.error("Greška prilikom dohvata tima:", error);
    throw error;
  }
}

export async function addTeam(newTeam) {
  try {
    const token = localStorage.getItem("AuthToken");
    const response = await api.post("/Team", newTeam, {
      headers: { Authorization: `Bearer ${token}` },
    });
    const addedTeam = response.data;
    return addedTeam;
  } catch (error) {
    console.error("Error adding team:", error);
    throw error;
  }
}

export async function deleteTeam(id) {
  try {
    const token = localStorage.getItem("AuthToken");
    const response = await api.delete(`/Team/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return id;
  } catch (error) {
    console.error("Error deleting team:", error);
    throw error;
  }
}

export async function updateTeam(id, updatedTeam) {
  try {
    const token = localStorage.getItem("AuthToken");
    const response = await api.put(`/Team/${id}`, updatedTeam, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    console.error("Error updating team:", error);
    throw error;
  }
}

export async function fetchTeamLeader() {
  try {
    const response = await api.get(`/TeamLeader`);

    return response.data;
  } catch (error) {
    throw error;
  }
}

const teamService = {
  fetchTeams,
};

export default teamService;
