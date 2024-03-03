interface TodoProps {
  todo: Todo;
}

export const Todo: React.FunctionComponent<TodoProps> = ({todo}) => {
  return <div>
    <span>{todo.title}</span>
    <span>{todo.completed ? "✅" : "❌"}</span>
  </div>;
};