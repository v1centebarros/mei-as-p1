import {api} from "@/api/axios.ts";

export const getTodos = async (): Promise<Todo[]> => {
    const response = await api.get<Todo[]>("/todos/");
    return response.data;
}