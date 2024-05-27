import { Outlet } from "react-router-dom";
import NavBar from "./NavBar";

const Layout = () => {
    return (
        <div className="h-full w-full flex flex-col">
            <NavBar />
            <div className="grow">
                <Outlet />
            </div>
        </div>
    )
};

export default Layout;