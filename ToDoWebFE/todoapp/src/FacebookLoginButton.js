import React, { useEffect, useState } from "react";
import "./FacebookLoginButton.css";

/**
 * Facebook Login Button Component
 * @requires Facebook SDK to be loaded
 */
export default function FacebookLoginButton() {
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Initialize Facebook SDK
    window.fbAsyncInit = function () {
      try {
        window.FB.init({
          appId: "1024568333148565", // Replace with your actual Facebook App ID
          cookie: true,
          status: true,
          xfbml: true,
          version: "v23.0", // Updated to latest stable version
        });

        window.FB.AppEvents.logPageView();
        setIsLoading(false);
      } catch (err) {
        setError("Failed to initialize Facebook SDK");
        setIsLoading(false);
      }
    };

    // Load Facebook SDK
    (function (d, s, id) {
      var js,
        fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) return;
      js = d.createElement(s);
      js.id = id;
      js.src = "https://connect.facebook.net/en_US/sdk.js";
      fjs.parentNode.insertBefore(js, fjs);
    })(document, "script", "facebook-jssdk");

    return () => {
      // Cleanup if needed
      delete window.fbAsyncInit;
    };
  }, []);

  const handleLogin = () => {
    setError(null); // Clear any previous errors

    window.FB.login(
      (response) => {
        if (response.authResponse) {
          // User successfully logged in
          console.log("Facebook login successful");
          console.log(response);
          // Send token to backend
          fetch("https://localhost:7016/User/login-facebook", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
              Accept: "application/json",
            },
            credentials: "include",
            body: JSON.stringify({
              accessToken: response.authResponse.accessToken,
            }),
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
              // Here you would typically:
              // 1. Store the token/user data
              // 2. Update app state
              // 3. Redirect or update UI
            })
            .catch((err) => {
              console.error("Login error:", err);
              setError(
                err.message || "Failed to complete login. Please try again."
              );
            });
        } else {
          // Handle login failure
          if (response.status === "not_authorized") {
            setError("Please authorize the required permissions to continue");
          } else {
            setError("Login was cancelled or failed. Please try again.");
          }
        }
      },
      {
        scope: "public_profile,email,user_link", // Removed space after comma
      }
    );
  };

  if (error) {
    return <div className="facebook-error">{error}</div>;
  }

  if (isLoading) {
    return <div className="facebook-loading">Loading Facebook SDK...</div>;
  }

  return (
    <button
      className="facebook-login-button"
      onClick={handleLogin}
      disabled={isLoading}
    >
      <i className="fab fa-facebook"></i>
      Continue with Facebook
    </button>
  );
}
