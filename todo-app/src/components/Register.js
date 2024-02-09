import React, { useState } from "react";
import {Link} from "react-router-dom";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Register = () => {
  const registrationUrl = process.env.REACT_APP_TODOAUTHURL;

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      
      localStorage.removeItem("token");
      localStorage.removeItem("userId");
      
      const response = await axios.post(`${registrationUrl}auth/register`, {      
        username,
        password,
      });
      setMessage(response.data.message);
      
      navigate("/login");
    } catch (error) {
      console.error("Registration failed:", error.response.data.error);
      setMessage(error.response.data.error);
    }
  };

  return (
    <div className="container">
      <div className="header">
        <h3>Register</h3>
        <Link to="/login">Login</Link>
      </div>
      
      <form className="add-form" onSubmit={handleSubmit}>
        <div className="form-control">
          <input
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Username"
            required
          />
        </div>
        
        <div className="form-control">
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Password"
            required
          />
        </div>

        <button className="btn btn-block" type="submit">Register</button>
        
      </form>
      {message && <p>{message}</p>}
    </div>
  );
};

export default Register;
