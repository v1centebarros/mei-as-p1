import {useQuery} from "@tanstack/react-query";
import {getTodos} from "@/api/todos.ts";

export const useGetTodos = () => {
    return useQuery<Todo[], Error>({queryKey: ["todos"], queryFn: () => getTodos()});
};