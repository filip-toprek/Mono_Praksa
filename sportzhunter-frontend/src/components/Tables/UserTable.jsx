import Button from "../../common/Button"
import '../../styles/tournament.css';

export default function UserTable({ userList, deleteUser }) {
    return (
       <>
        <h1>Users</h1>
            <div className="users">
                {userList.map(user => (
                    <div key={user.id} className="user">
                        <h3>{user.firstName} {user.lastName}</h3>
                        <p><strong>Username:</strong> {user.username}</p>
                        <p><strong>Role: </strong> {user.userRole.roleName}</p>
                        <Button onClick={() => deleteUser(user.id)}>Delete</Button>
                    </div>
                ))}
            </div>
        </>
    )
}