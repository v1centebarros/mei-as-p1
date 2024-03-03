import {useGetTodos} from "@/hooks/useGetTodos.ts";
import {Todo} from "@/components/todo";

interface TodoListProps {}

export const TodoList: React.FunctionComponent<TodoListProps> = () => {

  const {data:todos, isLoading, isError,isSuccess} = useGetTodos();
  return <div>

    {isLoading && <div>Loading...</div>}
    {isError && <div>Error fetching todos</div>}
    {isSuccess && todos?.map((todo: Todo) => <Todo todo={todo} key={todo.id}/>)}
  </div>;
};