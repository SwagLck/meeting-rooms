import { useEffect, useState } from "react";
import { Alert, Box, Checkbox, Chip, FormControl, InputLabel, ListItemText,
    MenuItem, OutlinedInput, Paper, Select, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, TextField, Typography,
 } from "@mui/material";

import { getUsers } from "../api/users";
import type { User } from "../types";

export default function UsersPage(){
    const[users, setUsers] = useState<User[]>([]);
    const[error, setError] = useState<string>("");

    useEffect(() => {
    getUsers()
        .then(setUsers)
        .catch(() => setError("Could not load users"));    
    }, []);


    return(
        <>
        <Typography variant="h4" sx={{ mb: 2 }}>Users</Typography>
        
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>First name</TableCell>
                        <TableCell>Last name</TableCell>
                        <TableCell>Active reservations</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {users.map((user) =>
                    <TableRow key={user.id}>
                        <TableCell>{user.id}</TableCell>
                        <TableCell>{user.firstName}</TableCell>
                        <TableCell>{user.lastName}</TableCell>
                        <TableCell>{user.activeReservationsCount}</TableCell>
                    </TableRow>
                    )}
                    {users.length === 0 && (
                        <TableRow>
                            <TableCell colSpan={4}>No users found</TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </TableContainer>
        </>
    )

}