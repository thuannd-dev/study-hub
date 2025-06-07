import React, { useEffect } from "react";
export default function GoogleLoginButton() {
  useEffect(() => {
    if (window.google) {
      window.google.accounts.id.initialize({
        client_id:
          "983687873578-rs5o6560m6n73uviuv1vug8sns5imh7v.apps.googleusercontent.com",
        callback: handleCredentialResponse,
      });

      window.google.accounts.id.renderButton(
        document.getElementById("google-signin-button"),

        { theme: "outline", size: "large" }
      );
    }
  }, []);

  function handleCredentialResponse(response) {
    console.log(response);
    fetch("https://localhost:7016/User/login-google", {
      method: "POST",

      headers: { "Content-Type": "application/json" },

      body: JSON.stringify({ credential: response.credential }),
    })
      .then((res) => res.json())

      .then((data) => {
        console.log("Backend response:", data);
      })
      .catch((err) => console.error("Login error:", err));
  }

  return <div id="google-signin-button"></div>;
}
//CORS
