import { useEffect, useState } from "react";
import { Alert, Box, Button, Chip,
    MenuItem, Paper, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, TextField, Typography,
 } from "@mui/material";

import { getReservations, cancelReservation, endReservation } from "../api/reservations";
import { getRooms } from "../api/rooms";
import { getUsers } from "../api/users";
import ReservationForm from "../components/ReservationForm";
import type { Reservation, Room, User, ApiError } from "../types";

const statusColor: Record<string, "primary" | "success" | "default"> = {
    Booked: "primary",
    Completed: "success",
    Cancelled: "default",
};

function formatDateTime(iso: string) {
    return new Date(iso).toLocaleString("lt-LT", {
        year: "numeric",
        month:"2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
    });
}

export default function ReservationsPage() {
    const [reservations, setReservations] = useState<Reservation[]>([]);
    const [rooms, setRooms] = useState<Room[]>([]);
    const [users, setUsers] = useState<User[]>([]);
    const [error, setError] = useState("");

    const[roomId, setRoomId] = useState("");
    const[organizerId, setOrganizerId] = useState("");
    const[status, setStatus] = useState("");
    const[from, setFrom] = useState("");
    const[to, setTo] = useState("");

    const[refresh, setRefresh] = useState(0);
    const[dialogOpen, setDialogOpen] = useState(false);

    useEffect(() => {
        getReservations({
            roomId: roomId ? Number(roomId) : undefined,
            organizerId: organizerId ? Number(organizerId) : undefined,
            status: status || undefined,
            from: from || undefined,
            to: to || undefined,
        })
        .then((data) => {
            setReservations(data);
            setError("");
        })
        .catch(() => setError("Could not load the reservations"));
    }, [roomId, organizerId, status, from, to, refresh]);

    useEffect(() => {
        getRooms().then(setRooms).catch(() => setError("Could not load the rooms"));
        getUsers().then(setUsers).catch(() => setError("Could not load the users"));
    })

    async function runAction(action: () => Promise<unknown>) {
    try {
        await action();
        setRefresh((n) => n + 1);
    } catch (e) {
        const apiError = e as ApiError;
        setError(apiError.message ?? "Action failed.");
        }
    }

    function isInProgress(r: Reservation){
        const now = new Date();
        return r.status === "Booked" && new Date(r.start) <= now && now < new Date(r.end);
    }

    return (
        <>
        <Box sx={{ display: "flex", justifyContent: "space-between", alignItems:"center", mb: 2 }}>
            <Typography variant="h4">Reservations</Typography>
            <Button variant="contained" onClick={() => setDialogOpen(true)}>
                New Reservation
            </Button>
        </Box>

        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

        <Box sx={{ display: "flex", gap: 2, mb: 3, flexWrap: "wrap"}}>
            <TextField select label="Room" value={roomId}
            onChange={(e) => setRoomId(e.target.value)} sx={{ width: 180 }}>
                <MenuItem value="">All</MenuItem>
                {rooms.map((r) => <MenuItem key={r.id} value={String(r.id)}>{r.name}</MenuItem>)}
            </TextField>

            <TextField select label="Organizer" value={organizerId}
            onChange={(e) => setOrganizerId(e.target.value)} sx={{ width: 200 }}>
                <MenuItem value="">All</MenuItem>
                {users.map((u) => (
                    <MenuItem key={u.id} value={String(u.id)}>{u.firstName} {u.lastName}</MenuItem>
                ))}
            </TextField>

            <TextField select label="Status" value={status}
                onChange={(e) => setStatus(e.target.value)} sx={{ width: 160 }}>
                <MenuItem value="">All</MenuItem>
                <MenuItem value="Booked">Booked</MenuItem>
                <MenuItem value="Completed">Completed</MenuItem>
                <MenuItem value="Cancelled">Cancelled</MenuItem>
            </TextField>

            <TextField label="From" type="date" value={from}
                onChange={(e) => setFrom(e.target.value)}
                slotProps={{ inputLabel: { shrink: true} }}/>
            <TextField label="to" type="date" value={to}
                onChange={(e) => setTo(e.target.value)}
                slotProps={{ inputLabel: {shrink: true }}}/>
        </Box>


        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Room</TableCell>
                        <TableCell>Organizer</TableCell>
                        <TableCell>Title</TableCell>
                        <TableCell>Start</TableCell>
                        <TableCell>End</TableCell>
                        <TableCell>Participants</TableCell>
                        <TableCell>Status</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {reservations.map((r) => (
                        <TableRow key={r.id}>
                            <TableCell>{r.roomName}</TableCell>
                            <TableCell>{r.organizerName}</TableCell>
                            <TableCell>{r.title}</TableCell>

                            <TableCell>{formatDateTime(r.start)}</TableCell>
                            <TableCell>{formatDateTime(r.end)}</TableCell>
                            <TableCell>{r.numberOfParticipants}</TableCell>
                            <TableCell>
                                <Chip label={r.status} color={statusColor[r.status] ?? "default"} size="small" />
                            </TableCell>
                            <TableCell>
                                {r.status === "Booked" && (
                                    <Button size="small" color="error"
                                    onClick={() => runAction(() => cancelReservation(r.id))}>
                                        Cancel
                                    </Button>
                                )}
                                {isInProgress(r) && (
                                    <Button size="small"
                                    onClick={() => runAction(() => endReservation(r.id))}>
                                        End early
                                    </Button>
                                )}
                            </TableCell>
                        </TableRow>
                    ))}
                    {reservations.length === 0 && (
                        <TableRow>
                            <TableCell colSpan={8}>No reservations found</TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </TableContainer>
        <ReservationForm
            open={dialogOpen}
            rooms={rooms}
            users={users}
            onClose={() => setDialogOpen(false)}
            onCreated={() =>{
                setDialogOpen(false);
                setRefresh((n) => n +1 );
            }}
        />
        </>
    );
}