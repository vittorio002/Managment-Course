async function Nonce(email){
    return await fetch("http://localhost:5117/Userapi/Auth/GetNonce", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(email),
      });
}

async function LoginAndToken(email, password){
    return await fetch("http://localhost:5117/Userapi/Auth/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ Email: email, Password: password }),
      });
}