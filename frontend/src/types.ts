export interface Equipment {
    id: number;
    name: string;
}

export interface Room{
    id: number;
    name: string;
    capacity: number;
    floor: number;
    isAvailable: boolean;
    equipment: Equipment[];
}

export interface User{
    id: number;
    firstName: string;
    lastName: string;
    activeReservationsCount: number;
}

export interface Reservation{
    id: number;
    roomId: number;
    roomName: string;
    organizerId: number;
    organizerName: string;
    title: string;
    start: string;
    end: string;
    numberOfParticipants: number;
    status: string;
}

export interface ApiError{
    code: string;
    message: string;
}

export interface CreateReservationRequest {
    roomId: number;
    organizerId: number;
    title: string;
    start: string;
    end: string;
    numberOfParticipants: number;
}