import { useEffect, useState } from "react";
import {
    Alert, AlertTitle, Button, Dialog, DialogActions, DialogContent,
    DialogTitle, MenuItem, Stack, TextField,
} from "@mui/material";

import { createReservation } from "../api/reservations";
import type { ApiError, Room, User } from "../types";

interface Props {
    open: boolean;
    rooms: Room[];
    users: User[];
    onClose: () => void;
    onCreated: () => void;
}


const TIME_SLOTS: string[] = [];
for (let h = 0; h < 24; h++) {
    TIME_SLOTS.push(`${String(h).padStart(2, "0")}:00`);
    TIME_SLOTS.push(`${String(h).padStart(2, "0")}:30`);
}

function todayString() {
    const d = new Date();
    const month = String(d.getMonth() + 1).padStart(2, "0");
    const day = String(d.getDate()).padStart(2, "0");
    return `${d.getFullYear()}-${month}-${day}`;
}

const emptyForm = {
    roomId: "",
    organizerId: "",
    title: "",
    startDate: "",
    startTime: "",
    endDate: "",
    endTime: "",
    numberOfParticipants: "",
};

export default function ReservationForm({ open, rooms, users, onClose, onCreated }: Props) {
    const [form, setForm] = useState(emptyForm);
    const [error, setError] = useState<ApiError | null>(null);
    const [submitting, setSubmitting] = useState(false);

    useEffect(() => {
        if (open) {
            setForm(emptyForm);
            setError(null);
        }
    }, [open]);

    function setField(field: keyof typeof emptyForm, value: string) {
        setForm((prev) => ({ ...prev, [field]: value }));
    }

    function handleStartDateChange(value: string) {
    setForm((prev) => {
        const newEndDate = !prev.endDate || prev.endDate < value ? value : prev.endDate;
        return {
            ...prev,
            startDate: value,
            endDate: newEndDate,
            // jei dabar ta pati diena ir pabaiga nebevėlesnė už pradžią, išvalome
            endTime:
                newEndDate === value && prev.endTime <= prev.startTime ? "" : prev.endTime,
            };
        });
    }

    const isFilled =
        form.roomId && form.organizerId && form.title.trim() &&
        form.startDate && form.startTime && form.endDate && form.endTime &&
        form.numberOfParticipants;


    
    async function handleSubmit() {
        setSubmitting(true);
        setError(null);
        try {
            await createReservation({
                roomId: Number(form.roomId),
                organizerId: Number(form.organizerId),
                title: form.title.trim(),
                start: `${form.startDate}T${form.startTime}:00`,
                end: `${form.endDate}T${form.endTime}:00`,
                numberOfParticipants: Number(form.numberOfParticipants),
            });
            onCreated();
        } catch (e) {
            const apiError = e as Partial<ApiError>;
            setError({
                code: apiError.code ?? "UNKNOWN",
                message: apiError.message ?? "Could not create the reservation",
            });
        } finally {
            setSubmitting(false);
        }
    }

    const endTimesSlots =
        form.startDate && form.startDate === form.endDate && form.startDate
        ? TIME_SLOTS.filter((t) => t > form.startTime)
        : TIME_SLOTS;

    return (
        <Dialog open={open} onClose={submitting ? undefined : onClose} fullWidth maxWidth="sm">
            <DialogTitle>New reservation</DialogTitle>
            <DialogContent>
                <Stack spacing={2} sx={{ mt: 1 }}>
                    {error && (
                        <Alert severity="error">
                            <AlertTitle>{error.code}</AlertTitle>
                            {error.message}
                        </Alert>
                    )}

                    <TextField select label="Room" value={form.roomId}
                        onChange={(e) => setField("roomId", e.target.value)} required>
                        {rooms.map((r) => (
                            <MenuItem key={r.id} value={String(r.id)} disabled={!r.isAvailable}>
                                {r.name} (capacity {r.capacity}){!r.isAvailable && " – inactive"}
                            </MenuItem>
                        ))}
                    </TextField>

                    <TextField select label="Organizer" value={form.organizerId}
                        onChange={(e) => setField("organizerId", e.target.value)} required>
                        {users.map((u) => (
                            <MenuItem key={u.id} value={String(u.id)}>
                                {u.firstName} {u.lastName}
                            </MenuItem>
                        ))}
                    </TextField>

                    <TextField label="Title" value={form.title}
                        onChange={(e) => setField("title", e.target.value)} required />

                    <Stack direction="row" spacing={2}>
                        <TextField fullWidth label="Start date" type="date" value={form.startDate}
                            onChange={(e) => handleStartDateChange(e.target.value)}
                            slotProps={{
                                inputLabel: { shrink: true },
                                htmlInput: { min: todayString() },
                            }} required />

                        <TextField select fullWidth label="Start time" value={form.startTime}
                            onChange={(e) => {
                                const newStart = e.target.value;
                                setForm((prev) => ({
                                    ...prev,
                                    startTime: newStart,
                                    endTime: prev.startDate === prev.endDate && prev.endTime <= newStart ? "" : prev.endTime,
                                }));
                            }}
                            slotProps={{ select: { MenuProps: { slotProps: { paper: { sx: { maxHeight: 300 } } } } } }}
                            required>
                            {TIME_SLOTS.map((t) => (
                                <MenuItem key={t} value={t}>{t}</MenuItem>
                            ))}
                        </TextField>
                    </Stack>

                    <Stack direction="row" spacing={2}>
                        <TextField fullWidth label="End date" type="date" value={form.endDate}
                            onChange={(e) => setField("endDate", e.target.value)}
                            slotProps={{
                                inputLabel: { shrink: true },
                                htmlInput: { min: form.startDate || todayString() },
                            }} required />

                        <TextField select fullWidth label="End time" value={form.endTime}
                            onChange={(e) => setField("endTime", e.target.value)}
                            slotProps={{ select: { MenuProps: { slotProps: { paper: { sx: { maxHeight: 300 } } } } } }}
                            required>
                            {endTimesSlots.map((t) => (
                                <MenuItem key={t} value={t}>{t}</MenuItem>
                            ))}
                        </TextField>
                    </Stack>

                    <TextField label="Number of participants" type="number" value={form.numberOfParticipants}
                        onChange={(e) => setField("numberOfParticipants", e.target.value)}
                        slotProps={{ htmlInput: { min: 1 } }} required />
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={submitting}>Cancel</Button>
                <Button variant="contained" onClick={handleSubmit} disabled={!isFilled || submitting}>
                    Create
                </Button>
            </DialogActions>
        </Dialog>
    );
}