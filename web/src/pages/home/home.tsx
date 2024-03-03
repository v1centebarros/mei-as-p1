import {TodoList} from "@/components/todoList";

interface HomeProps {}

export const Home: React.FunctionComponent<HomeProps> = () => {
  return <>
    <div className={"text-3xl font-bold"}>Todos Page</div>
    <TodoList/>

  </>;
};