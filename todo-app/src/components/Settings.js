import Button from './Button';
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";

const Settings = () => {

    const navigate = useNavigate();

    const onLogout = (e) => {
        e.preventDefault();
        localStorage.removeItem("token");
        localStorage.removeItem("userId");
        
        Swal.fire({
            icon: 'error',
            title: 'Ok...',
            text: 'You have been logged out!'
        })

        navigate("/login");
    }

    return (
        <div>
            <div className="form-control">
                <label>Username</label>                
            </div>
                
            <Button onClick={onLogout} color={'red'} text={'Logout'} />
            
        </div>
    )
}

export default Settings
