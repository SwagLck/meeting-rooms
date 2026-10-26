import { useEffect, useState } from "react";
import { Alert, Box, Checkbox, Chip, FormControl, InputLabel, ListItemText,
    MenuItem, OutlinedInput, Paper, Select, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, TextField, Typography,
 } from "@mui/material";

import { getRooms, getEquipment } from "../api/rooms";
import type { Room, Equipment } from "../types";

export default function RoomPage(){
    const[rooms, setRooms] = useState<Room[]>([]);
    const [equipment, setEquipment] = useState<Equipment[]>([]);
    const [error, setError] = useState<string>("");

    const [minCapacity, setMinCapacity] = useState("");
    const [floor, setFloor] = useState("");
    const [selectedEquipmentIds, setSelectedEquipmentIds] = useState<number[]>([]);

    useEffect(() => {
        getEquipment()
            .then(setEquipment)
            .catch(() => setError("Could not load the equipment"));
    }, []);

    useEffect(() => {
        getRooms({
            minCapacity: minCapacity ? Number(minCapacity) : undefined,
            floor: floor ? Number(floor) : undefined,
            equipmentIds: selectedEquipmentIds,
        })
        .then((data) => {
            setRooms(data);
            setError("");
        })
        .catch(() => setError("Could not load the rooms"));
    }, [minCapacity, floor, selectedEquipmentIds]);

    return (
        <>
        <Typography variant="h4" sx={{ mb: 2 }}>Rooms</Typography>

        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

        <Box sx={{ display: "flex", gap: 2, mb:3, flexWrap: "wrap" }}>
            <TextField
            label="Minimum capacity"
            type="number"
            value={minCapacity}
            onChange={(e) => setMinCapacity(e.target.value)}
            sx={{ width: 160 }}
            />
            <TextField
            label="Floor"
            type="number"
            value={floor}
            onChange={(e) => setFloor(e.target.value)}
            sx={{ width: 120 }}
            />

            
            <FormControl sx={{ width: 260 }}>
                <InputLabel id="equipment-label">Equipment</InputLabel>
                <Select
                labelId="equipment-label"
                multiple
                value={selectedEquipmentIds}
                onChange={(e) => setSelectedEquipmentIds(e.target.value as number[])}
                input={<OutlinedInput label="Equipment" />}
                renderValue={(ids) =>
                    equipment.filter((eq) => ids.includes(eq.id)).map((eq) => eq.name).join(", ")

                }
                >
                    {equipment.map((eq) => (
                        <MenuItem key={eq.id} value={eq.id}>
                            <Checkbox checked={selectedEquipmentIds.includes(eq.id)} />
                            <ListItemText primary={eq.name} />
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </Box>

        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>ID</TableCell>
                        <TableCell>Capacity</TableCell>
                        <TableCell>Floor</TableCell>
                        <TableCell>Equipment</TableCell>
                        <TableCell>Status</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {rooms.map((room)=>(
                        <TableRow key={room.id}>
                            <TableCell>{room.name}</TableCell>
                            <TableCell>{room.id}</TableCell>
                            <TableCell>{room.capacity}</TableCell>
                            <TableCell>{room.floor}</TableCell>
                            <TableCell>
                                {room.equipment.map((eq) => (
                                    <Chip key={eq.id} label={eq.name} size="small" sx={{ mr: 0.5 }} />
                                ))}
                            </TableCell>
                            <TableCell>
                                <Chip
                                    label={room.isAvailable ? "Available" : "Unavailable"}
                                    color={room.isAvailable ? "success" : "default"}
                                    size="small"
                                />
                            </TableCell>
                        </TableRow>
                    ))}
                    {rooms.length === 0 && (
                        <TableRow>
                            <TableCell colSpan={6}>No rooms found</TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </TableContainer>
        </>
    )
}
