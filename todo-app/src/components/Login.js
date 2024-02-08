import React, { useState, useContext } from "react";
import {Link} from "react-router-dom";
import axios from "axios";
import { AuthContext } from "./AuthContext";
import { useNavigate } from "react-router-dom";

const Login = () => {

  const loginUrl = process.env.REACT_APP_TODOAUTHURL;

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState(null); // New state for handling error messages
  const { setToken } = useContext(AuthContext);
  
  const navigate = useNavigate();
  
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`${loginUrl}api/auth/login`, {
        username,
        password,
      });
      setToken(response.data.token);
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("userId", response.data.userId);
      navigate("/todo");
    } catch (error) {
      console.error("Authentication failed:", error);
      setToken(null);
      localStorage.removeItem("token");
      localStorage.removeItem("userId");
      if (error.response && error.response.data) {
        setErrorMessage(error.response.data);
      } else {
        setErrorMessage("An unexpected error occurred. Please try again.");
      }
    }
  };

  return (
    <div className="container">
      <div className="header">
        <h3>Login</h3>
        <Link to="/register">Register</Link>
      </div>
      
      <form className="add-form" onSubmit={handleSubmit}>
        <div className="form-control">        
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Username"
          />
        </div>
        <div className="form-control">
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Password"
          />
        </div>

        {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}{" "}

        <button className="btn btn-block" type="submit">Login</button>
        
      </form>
    </div>
  );
};

export default Login;
