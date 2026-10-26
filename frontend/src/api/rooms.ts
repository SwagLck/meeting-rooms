import { request } from "./client";
import type { Room, Equipment } from "../types";

export interface RoomFilters{
    minCapacity?: number;
    floor?: number;
    equipmentIds?: number[];
}

export function getRooms(filters: RoomFilters = {}): Promise<Room[]>{
    const params = new URLSearchParams();
    if(filters.minCapacity) params.append("minCapacity", String(filters.minCapacity));
    if(filters.floor) params.append("floor", String(filters.floor));
    filters.equipmentIds?.forEach((id) => params.append("EquipmentIds", String(id)));

    const query = params.toString();
    return request<Room[]>(`/rooms${query ? `?${query}` : ""}`);
}

export function getEquipment(): Promise<Equipment[]>{
    return request<Equipment[]>("/equipment");
}