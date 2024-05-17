import {useState, useEffect} from "react";
import adminService from "../services/AdminService";
import Button from "../common/Button";
import Pagination from "../components/Paging";
import UserTable from "../components/Tables/UserTable";
import ApplicationsTable from "../components/Tables/ApplicationsTable";
import AttendsTable from "../components/Tables/AttendsTable";
import "../styles/tournament.css";

export default function AdminDashboard() {
    const [userList, setUserList] = useState([]);
    const [sorting, setSorting] = useState({sortBy: "RoleName", sortOrder: "ASC"});
    const [paging, setPaging] = useState({pageNumber: 1, pageSize: 5, totalPages: 0});
    const [applications, setApplications] = useState([]);
    const [attendsList, setAttendsList] = useState([]);

    useEffect(() => {
        fetchPlayers();
    }, [sorting, paging]);

    async function fetchPlayers() {
        try {
            await adminService.getUsers(sorting, paging)
            .then(response => {
                setUserList(response.data.list);
                if(response.data.pageCount !== paging.totalPages || response.data.pageSize !== paging.pageSize)
                    setPaging({...paging, totalPages: response.data.pageCount});
            });
        } catch (error) {
            setUserList([]);
        }
        try {
            await adminService.getPendingApplications()
            .then(response => {
                setApplications(response.data);
            });
        } catch (error) {
            setApplications([]);
        }
        try {
            await adminService.getAttends()
            .then(response => {
                setAttendsList(response.data.list);
            });
        } catch (error) {
            setAttendsList([]);
        }

            
       
    }
    async function deleteUser(id) {
        await adminService.deleteUser(id);
        fetchPlayers();
    }

    function handleChange (e) {
        setSorting({...sorting, [e.target.name]: e.target.value});
    }

    const nextPage = () => {
        if(paging.pageNumber < paging.totalPages){
            setPaging({...paging, pageNumber: paging.pageNumber + 1});
        }
    }

    const previousPage = () => {
        if(paging.pageNumber > 1){
            setPaging({...paging, pageNumber: paging.pageNumber - 1});
        }
    }

    const handlePageChange = (event) => {
        setPaging({...paging, pageSize: event.target.value, pageNumber: 1});
    }

    const handleApproveApplication = async (id) => {
        await adminService.approveApplication(id).then(() => {
            fetchPlayers();
        });
    }

    const handleDisapproveApplication = async (id) => {
        await adminService.disapproveApplication(id).then(() => {
            fetchPlayers();
        });
    }

    const handleApproveAttend = async (id) => {
        await adminService.approveAttend(id).then(() => {
            fetchPlayers();
        });
    }

    const handleDisapproveAttend = async (id) => {
        await adminService.disapproveAttend(id).then(() => {
            fetchPlayers();
        });
    }

    return (
        <div className="adminDashboard">
            <h1>Admin Dashboard</h1>
            <div className="adminBtn">
                <label>OrderBy:</label>
                <select name="sortBy" onChange={handleChange}>
                    <option value="RoleName">Role</option>
                    <option value="Username">Username</option>
                </select>
                <label>Order:</label>
                <select name="sortOrder" onChange={handleChange}>
                    <option value="ASC">Ascending</option>
                    <option value="DESC">Descending</option>
                </select>
            </div>
            <hr/>
            <UserTable userList={userList} deleteUser={deleteUser}/>
            <Pagination paging={paging} previousPage={previousPage} nextPage={nextPage} handlePageChange={handlePageChange}/>
            <ApplicationsTable applications={applications} approveApplication={handleApproveApplication} disapproveApplication={handleDisapproveApplication}/>
            <AttendsTable approveAttend={handleApproveAttend} disapproveAttend={handleDisapproveAttend} attendsList={attendsList} />
        </div>
    );
}