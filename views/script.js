let currentUser = null;

async function login() {
  let email = document.getElementById("login-email").value;
  $.ajax({
    method: "POST",
    url: "http://localhost:5117/Userapi/Auth/GetNonce",
    contentType: "application/json",
    data: JSON.stringify(email),
    success: function (response) {
      let nonce = response;
      let password = document.getElementById("login-password").value;
      password += nonce;
      $.ajax({
        method: "POST",
        url: "http://localhost:5117/Userapi/Auth/Login",
        contentType: "application/json",
        data: JSON.stringify({ Email: email, Password: password }),
        success: function (token) {
          sessionStorage.setItem("token", token);
          showApplication();
        },
        error: function (xhr, textStatus, errorThrown) {
          document.getElementById("errorLogin").textContent = "Error Login: " +" "+ xhr.status +" "+ textStatus +" "+ errorThrown;
        },
      });
    },
    error: function (xhr, textStatus, errorThrown) {
      document.getElementById("errorLogin").textContent = "Error Login: " +" "+ xhr.status +" "+ textStatus +" "+ errorThrown;
    },
  });
}

async function register() {
  const email = document.getElementById("register-email").value;
  const name = document.getElementById("register-name").value;
  const password = document.getElementById("register-password").value;
  $.ajax({
    method: "POST",
    url: "http://localhost:5117/Userapi/User",
    contentType: "application/json",
    data: JSON.stringify({
      Email: email,
      Name: name,
      Password: password,
      Nonce: null,
      role: ["user"],
    }),
    success: function (response) {
      switchToLogin();
    },
    error: function (xhr, textStatus, errorThrown) {
      document.getElementById("errorRegistration").textContent = "Error Creation: " + xhr.status +" "+ textStatus +" "+ errorThrown;

    },
  });
}

function switchToRegistration() {
    document.getElementById("login-page").style.display = "none";
    document.getElementById("registration-page").style.display = "block";
  }

  function switchToLogin() {
    document.getElementById("login-page").style.display = "block";
    document.getElementById("registration-page").style.display = "none";
    document.getElementById("application-menu").style.display = "none";
  }

  function showApplication() {
    const TokenString =sessionStorage.getItem('token');
    const GetToken = JSON.parse(TokenString);

    document.getElementById("login-page").style.display = "none";
    document.getElementById("registration-page").style.display = "none";
    document.getElementById("application-page").style.display = "block";
    document.getElementById("userNavItem").style.display = "block";

    if(GetToken.role.includes("admin")){
        document.getElementById("adminNavItem").style.display = "block";
    }else{
        document.getElementById("adminNavItem").style.display = "none";
    } 
    
    if(GetToken.role.includes("labManager")){
        document.getElementById("labNavItem").style.display = "block";
    }else{
        document.getElementById("labNavItem").style.display = "none";
    }
    document.getElementById("profileNavItem").textContent += GetToken.name;
  }

  function gestisciClic(event) {
    const targetId = event.target.id;

    switch (targetId) {
      case "userNavItem":
        document.getElementById("content").innerHTML = "User";
        break;
      case "adminNavItem":
        $.ajax({
          method:'GET',
          url:'http://localhost:5117/Userapi/User',
          success:function(response){
            const content = document.getElementById("content");

            for (const user of response) {
              const divUser = document.createElement('div');
              divUser.textContent = user.name+" | "+user.email+" | "+user.role;
        
              const buttonUser = document.createElement('button');
              //buttonUser.classList.add("rigth");
              buttonUser.textContent ="Modify";
              buttonUser.addEventListener('click', function() {
                
              });

              const ButtonDelete = document.createElement('button');
              ButtonDelete.classList.add("rigth");
              ButtonDelete.textContent = "Delete";
              ButtonDelete.addEventListener('click', function() {
                  
              });

              divUser.appendChild(buttonUser);
              divUser.appendChild(ButtonDelete);
              content.appendChild(divUser);
            } 
            const ButtonAdd = document.createElement('button');
            ButtonAdd.textContent = "Add User";
            ButtonAdd.addEventListener('click', function() {
                
            });
            content.appendChild(ButtonAdd);
          }
        })
        
        break;
      case "labNavItem":
        document.getElementById("content").innerHTML = "Lab Manager";
        break;
      case "profileNavItem":
        const TokenString =sessionStorage.getItem('token');
        const GetToken = JSON.parse(TokenString);
        document.getElementById("content").innerHTML = "Profile:<br>Name: "+GetToken.name+"<br>Email: "+GetToken.email+"<br>Role: "+GetToken.role;
        break;
      default:
        break;
    }
  }