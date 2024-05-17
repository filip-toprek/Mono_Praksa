import Button from "../../common/Button";

export default function AttendsTable({ attendsList, approveAttend, disapproveAttend }) {
    return (
        
            <>
            <h1>Tournament attends</h1>
           {attendsList.length > 0 ? (
                <div className="tournamentsList">
                    {attendsList.map(attends => (
                        <div key={attends.id} className="tournamentCard">
                            <div className="details">
                                <h2>Tournament: {attends.tournament.name}</h2>
                                <h3>Team: {attends.team.name}</h3>
                            </div>
                            <div className="actions">
                                <Button onClick={() => approveAttend(attends.id)}>Approve</Button>
                                <Button onClick={() => disapproveAttend(attends.id)}>Disapprove</Button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (<p>No attends found</p>)}
        </>
    );
}