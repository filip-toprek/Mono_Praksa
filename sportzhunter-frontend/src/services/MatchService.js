import axios from "axios";

const API_BASE_URL = "https://localhost:44323/api";

const api = axios.create({
  baseURL: API_BASE_URL,
});

export async function fetchMatches() {
  try {
    const response = await api.get("/Match", {
      headers: { Authorization: `Bearer ${localStorage.getItem("AuthToken")}` },
    });
    return response.data.list;
  } catch (error) {
    console.error("Error fetching matches:", error);
    throw error;
  }
}

export async function fetchMatchById(id) {
  try {
    const response = await api.get(`/Match/${id}`, {
      headers: { Authorization: `Bearer ${localStorage.getItem("AuthToken")}` },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching match by ID:", error);
    throw error;
  }
}

export async function addMatch(newMatch) {
  try {
    const token = localStorage.getItem("AuthToken");
    const response = await api.post("/Match", newMatch, {
      headers: { Authorization: `Bearer ${token}` },
    });
    const addedMatch = response.data;
    return addedMatch;
  } catch (error) {
    console.error("Error adding match:", error);
    throw error;
  }
}

export async function deleteMatch(id) {
  try {
    const token = localStorage.getItem("AuthToken");
    const response = await api.delete(`/Match/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return id;
  } catch (error) {
    console.error("Error deleting match:", error);
    throw error;
  }
}

export async function updateMatch(id, updatedMatch) {
  try {
    const token = localStorage.getItem("AuthToken");
    const response = await api.put(`/Team/${id}`, updatedMatch, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    console.error("Error updating match:", error);
    throw error;
  }
}

export async function fetchMatchByTournamentId(tournamentId) {
  try {
    const response = await api.get(`/Match?tournamentId=${tournamentId}`, {
      headers: { Authorization: `Bearer ${localStorage.getItem("AuthToken")}` },
    });
    return response.data.list;
  } catch (error) {
    console.error("Error fetching matches by tournament ID:", error);
    throw error;
  }
}

const matchService = {
  fetchMatches,
  fetchMatchByTournamentId,
};

export default matchService;
