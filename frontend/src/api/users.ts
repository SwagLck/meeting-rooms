import { request } from "./client";
import type { User } from "../types";

export function getUsers(): Promise<User[]> {
    return request<User[]>("/users");
}