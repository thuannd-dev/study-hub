import React, { useEffect, useState } from "react";
import "./GoogleLoginButton.css";

export default function GoogleLoginButton() {
  const [error, setError] = useState(null);

  useEffect(() => {
    if (window.google) {
      window.google.accounts.id.initialize({
        client_id:
          "983687873578-rs5o6560m6n73uviuv1vug8sns5imh7v.apps.googleusercontent.com",
        callback: handleCredentialResponse,
      });

      window.google.accounts.id.renderButton(
        document.getElementById("google-signin-button"),
        { theme: "filled", size: "large", shape: "rectangular" }
      );
    }
  }, []);

  function handleCredentialResponse(response) {
    setError(null); // Clear any previous errors
    console.log(response);
    fetch("https://localhost:7016/User/login-google", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
      body: JSON.stringify({ credential: response.credential }),
    })
      .then(async (res) => {
        if (!res.ok) {
          const errorData = await res.json().catch(() => ({}));
          throw new Error(errorData.message || "Login failed");
        }
        return res.json();
      })
      .then((data) => {
        console.log("Backend response:", data);
        if (data.token) {
          localStorage.setItem("token", data.token);
          // Optionally redirect or update UI state
        }
      })
      .catch((err) => {
        console.error("Login error:", err);
        setError(err.message || "Failed to complete login. Please try again.");
      });
  }

  if (error) {
    return <div className="google-error">{error}</div>;
  }

  return <div id="google-signin-button"></div>;
}
//CORS
