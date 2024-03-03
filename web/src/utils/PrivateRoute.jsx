import { useAuthContext } from "../contexts/Auth";
import { Navigate } from "react-router-dom";


export const PrivateRoute = ({component: Component, ...rest}) => {
    const {isLogged} = useAuthContext();

    return isLogged() ? <Component {...rest}/> : <Navigate to={"/login"}/>
}