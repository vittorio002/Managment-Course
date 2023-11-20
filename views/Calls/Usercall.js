//get the nonce
async function Nonce(email){
    return fetch("http://localhost:5117/Userapi/Auth/GetNonce", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(email),
      });
}

//send the password+token and get the token
async function LoginAndToken(email, password){
    return fetch("http://localhost:5117/Userapi/Auth/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ Email: email, Password: password }),
      });
}

//get the list of users (only for admin)
async function GetUsers(){
  $.ajax({
    method: "GET",
    url: "http://localhost:5117/Userapi/User",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    success: function (response) {
      let content = document.getElementById("content");
      content.innerHTML = "";

      for (const user of response) {
        const divUser = document.createElement("div");
        divUser.textContent =
          user.name + " | " + user.email + " | " + user.role;

        const buttonUser = document.createElement("button");

        buttonUser.style.backgroundImage = 'url(./img/pencil.png)';
          buttonUser.style.backgroundSize = 'cover';
          buttonUser.style.width = '25px';
          buttonUser.style.height = '25px';
        buttonUser.classList.add("Lessrigth");
        buttonUser.addEventListener("click", function () {
          document.getElementById("myModal").style.display = "block";

          document.getElementById("name").value = user.name;
          document.getElementById("email").value = user.email;
          document.getElementById("role").value = user.role;
        });

        const ButtonDelete = document.createElement("button");
        ButtonDelete.classList.add("rigth");
        ButtonDelete.style.backgroundImage = 'url(./img/delete.jpg)';
          ButtonDelete.style.backgroundSize = 'cover';
          ButtonDelete.style.width = '25px';
          ButtonDelete.style.height = '25px';

        ButtonDelete.addEventListener("click", function () {
          DeleteUser(user.email);
        });

        divUser.appendChild(buttonUser);
        divUser.appendChild(ButtonDelete);
        content.appendChild(divUser);
      }
      const ButtonAdd = document.createElement("button");
          ButtonAdd.style.backgroundImage = 'url(./img/add.jpg)';
          ButtonAdd.style.backgroundSize = 'cover';
          ButtonAdd.style.width = '25px';
          ButtonAdd.style.height = '25px';

      ButtonAdd.addEventListener("click", function () {
        document.getElementById("myModalAdd").style.display = "block";
        document.getElementById("adminNavItem").click();
      });
      content.appendChild(ButtonAdd);
    },
    error: function(xhr) {
      handleError(xhr.status);
    }
  });
}

//delete one user account (only for admin)
async function DeleteUser(email){
  $.ajax({
    method: "DELETE",
    contentType: "application/json",
    url: "http://localhost:5117/Userapi/User",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    data: JSON.stringify(email),
    success: function () {
      document.getElementById("adminNavItem").click();
    },
    error: function(xhr) {
      handleError(xhr.status);
    }
  });
}

//modify one user (only for admin)
async function Modify() {
  const name = document.getElementById("name").value;
  const email = document.getElementById("email").value;
  let role = document.getElementById("role").value;
  role = role.split(",");

  $.ajax({
    method: "PUT",
    url: "http://localhost:5117/Userapi/User",
    contentType: "application/json",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    data: JSON.stringify({
      Email: email,
      Name: name,
      Password: null,
      Nonce: null,
      role: role,
    }),
    success: function (response) {
      document.getElementById("adminNavItem").click();
    },
    error: function(xhr) {
      handleError(xhr.status);
    }
  });
}

//add one user (only for admin)
function Add() {
  const emailAdd = document.getElementById("emailAdd").value;
  const nameAdd = document.getElementById("nameAdd").value;
  const passwordAdd = document.getElementById("passwordAdd").value;

  $.ajax({
    method: "POST",
    url: "http://localhost:5117/Userapi/User",
    contentType: "application/json",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    data: JSON.stringify({
      Email: emailAdd,
      Name: nameAdd,
      Password: passwordAdd,
      Nonce: null,
      role: ["user"],
    }),
    error: function(xhr) {
      handleError(xhr.status);
    }
  });
}