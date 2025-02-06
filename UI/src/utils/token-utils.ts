import { jwtDecode } from "jwt-decode";
import { userToken } from "../definitions"

export const UserAuthenticated = () :boolean => {
    try {
        const token = localStorage.getItem(userToken);
        if (!token) return false;
        const decodedToken = jwtDecode(token);
        if(!decodedToken || !decodedToken.exp || decodedToken.exp < Date.now() / 1000) return false;
        return true;
    }
    catch {
        return false;
    }
}