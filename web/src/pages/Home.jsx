import {useAuthContext} from "../contexts/Auth.jsx";

export const Home = () => {

    const {logout} = useAuthContext();
    return (
        <div>
            <h1 className="text-2xl">Home</h1>
            <button className="btn btn-primary"
                    onClick={() => logout()}
            >Logout</button>
        </div>
    )
}