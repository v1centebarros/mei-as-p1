import { useAuthContext } from "../contexts/Auth";
import { Navigate } from "react-router-dom";

export const PrivateRoute = ({ component: Component, roles, ...rest }) => {
    const { isLogged, role } = useAuthContext();

    if (!isLogged()) {
        return <Navigate to="/login" />;
    }

    if (roles.includes(role)) {
        return <Component {...rest} />;
    }

    return <Navigate to={"/404"} replace />
};
