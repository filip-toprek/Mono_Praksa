import Button from "../../common/Button"
import adminService from "../../services/AdminService"

export default function ApplicationsTable({ applications, approveApplication, disapproveApplication }) {
    return (
        <>
        <center>
        <h1>Applications</h1>
        {applications.length > 0 ?(
            <div className="table">
            <table>
                <thead>
                    <tr className="thead">
                        <th>Team name</th>
                        <th>Team description</th>
                        <th>Teamleader username</th>
                        <th>Date created</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody className="tableBody">
                    {applications.map(application => (
                        <tr key={application.id}>
                            <td>{application.team.name}</td>
                            <td>{application.team.description}</td>
                            <td>{application.team.teamLeaderName}</td>
                            <td>{new Date(application.dateCreated).toLocaleDateString()}</td>
                            <td className="action-buttons">
                                <Button onClick={() => approveApplication(application.team.teamLeaderId)}>Approve</Button>
                                <Button onClick={() => disapproveApplication(application.team.teamLeaderId)}>Disapprove</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
            )
        : (<p>No applications found</p>)}
        </center>
        </>
    )
}