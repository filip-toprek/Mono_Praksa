import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";
import Home from "./pages/Home";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import Teams from "./pages/Teams";
import Tournaments from "./pages/Tournaments";
import CreateTournament from "./pages/CreateTournament";
import EditTournament from "./pages/EditTournament";
import TournamentDetails from "./pages/TournamentDetails";
import PlayersPage from "./pages/PlayersPage";
import PlayerDetails from "./pages/PlayerDetails";
import CommentEdit from "./pages/CommentEdit";
import TeamDetails from "./pages/TeamDetails";
import Profile from "./pages/Profile";
import EditProfile from "./pages/EditProfile";
import AdminDashboard from "./pages/AdminDashboard";
import TeamleaderPage from "./pages/TeamleaderPage";
import AddMatchForm from "./components/Forms/AddMatchForm";
import EditMatchForm from "./components/Forms/EditMatchForm";
import InviteList from "./components/InviteList";

function App() {
  return (
    <>
      <BrowserRouter>
      <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/tournaments" element={<Tournaments />} />
          <Route path="/tournament/:id" element={<TournamentDetails />} />
          <Route path="/tournament/create" element={<CreateTournament />} />
          <Route path="/tournament/edit/:id" element={<EditTournament />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/players" element={<PlayersPage />} />
          <Route path="/player/:id" element={<PlayerDetails />} />
          <Route path="/teams" element={<Teams></Teams>}/>
          <Route path="/editComment/:id" element={ <CommentEdit /> }/>
          <Route path="/teams/:id" element={<TeamDetails/>}/>
          <Route path="/profile" element={<Profile />}/>
          <Route path="/profile/edit/:id" element={<EditProfile />}/>
          <Route path="/admin" element={<AdminDashboard />}/>
          <Route path="/teamleader" element={<TeamleaderPage />}/>
          <Route path="/tournament/:id/match" element={<AddMatchForm />} />
          <Route path="/tournament/:id/match/:matchId" element={<EditMatchForm />} />
          <Route path="/invites/:id" element={<InviteList />} />
        </Routes>
        <Footer />
      </BrowserRouter>
    </>
  );
}

export default App;
