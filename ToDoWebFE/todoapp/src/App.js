import React from "react";
import GoogleLoginButton from "./GoogleLoginButton";
import FacebookLoginButton from "./FacebookLoginButton";
import "./App.css";

function App() {
  return (
    <div className="App">
      <div className="login-container">
        <h1 className="login-title">Welcome</h1>
        <p className="login-subtitle">Sign in to continue</p>
        <GoogleLoginButton />
        <FacebookLoginButton />
      </div>
    </div>
  );
}

export default App;
