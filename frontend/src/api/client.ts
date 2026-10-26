const BackEnd = "http://localhost:5248/api";

export async function request<T>(path: string, options?: RequestInit): Promise<T>{
    const response = await fetch(`${BackEnd}${path}`, {
        headers: { "Content-Type": "application/json"},
        ...options,
    });

    if(!response.ok){
        const error = await response.json().catch(() => ({
            code: "UNKNOWN",
            message: "Something went wrong",
        }));
        throw error;
    }
    return response.json();
}