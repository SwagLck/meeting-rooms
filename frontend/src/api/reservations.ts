import { request } from "./client";
import type { Reservation, CreateReservationRequest } from "../types";

export interface ReservationFilters {
    roomId?: number;
    organizerId?: number;
    status?: string;
    from?: string;
    to?: string;
}

export function getReservations(filters: ReservationFilters = {}): Promise<Reservation[]>{
    const params = new URLSearchParams();

    Object.entries(filters).forEach(([key, value]) => {
        if(value !== undefined && value !== "") params.append(key, String(value));
    });

    const query = params.toString();
    return request<Reservation[]>(`/reservations${query ? `?${query}` : ""}`);
}

export function createReservation(data: CreateReservationRequest): Promise<Reservation> {
    return request<Reservation>("/reservations", {
        method: "POST",
        body: JSON.stringify(data)
    });
}

export function cancelReservation(id: number): Promise<Reservation>{
    return request<Reservation>(`/reservations/${id}/cancel`, { method: "POST" });
}

export function endReservation(id: number): Promise<Reservation>{
    return request<Reservation>(`/reservations/${id}/end`, { method: "POST" });
}