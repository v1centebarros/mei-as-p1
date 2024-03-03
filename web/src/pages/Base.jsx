import { Outlet } from "react-router-dom"

export const Base = () => {
    return (
        <div>
            <h1 className="text-2xl">Base</h1>
            <Outlet />
        </div>
    )
}