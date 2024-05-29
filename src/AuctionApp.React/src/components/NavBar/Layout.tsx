import { Outlet } from "react-router-dom";
import NavBar from "./NavBar";

import './Layout.css';

const Layout = () => {
    return (
        <div className="full-page">
            <NavBar />
            <div className="outlet">
                <Outlet />
            </div>
        </div>
    )
};

export default Layout;