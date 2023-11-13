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
  Add(email, name, password);
  switchToLogin();
}

async function switchToRegistration() {
    document.getElementById("login-page").style.display = "none";
    document.getElementById("registration-page").style.display = "block";
  }

  async function switchToLogin() {
    document.getElementById("login-page").style.display = "block";
    document.getElementById("registration-page").style.display = "none";
    document.getElementById("application-menu").style.display = "none";
  }

  async function showApplication() {
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

    document.getElementById('profileNavItem').click();
  }

  async function gestisciClic(event) {
    const targetId = event.target.id;

    switch (targetId) {
      case "userNavItem":
        //User Interface
        document.getElementById("content").innerHTML = "User";
        break;
      case "adminNavItem":
        //Admin Interface
        $.ajax({
          method:'GET',
          url:'http://localhost:5117/Userapi/User',
          success:function(response){
            let content = document.getElementById("content");
            content.innerHTML = '';

            for (const user of response) {
              const divUser = document.createElement('div');
              divUser.textContent = user.name+" | "+user.email+" | "+user.role;
        
              const buttonUser = document.createElement('button');
              
              buttonUser.textContent ="Modify";
              buttonUser.classList.add("Lessrigth");
              buttonUser.addEventListener('click', function() {
                document.getElementById("myModal").style.display = "block";

                  document.getElementById("name").value = user.name;
                  document.getElementById("email").value = user.email;
                  document.getElementById("role").value = user.role;
              });

              const ButtonDelete = document.createElement('button');
              ButtonDelete.classList.add("rigth");
              ButtonDelete.textContent = "Delete";
              ButtonDelete.addEventListener('click', function() {
                  $.ajax({
                    method:'DELETE',
                    contentType: "application/json",
                    url:'http://localhost:5117/Userapi/User',
                    data:JSON.stringify(user.email),
                    success:function(){
                      document.getElementById('adminNavItem').click();
                    },
                    error:{}
                  })
              });

              divUser.appendChild(buttonUser);
              divUser.appendChild(ButtonDelete);
              content.appendChild(divUser);
            } 
            const ButtonAdd = document.createElement('button');
            ButtonAdd.textContent = "Add User";
            ButtonAdd.addEventListener('click', function() {
              document.getElementById("myModalAdd").style.display = "block";
              
              const email = document.getElementById("emailAdd").value;
              const name = document.getElementById("nameAdd").value;
              const password = document.getElementById("passwordAdd").value;
              Add(email,name,password);
            });
            content.appendChild(ButtonAdd);
          },
          error:{}
        })
        
        break;
      case "labNavItem":
        //Lab Manager Interface
        document.getElementById("content").innerHTML = "Lab Manager";
        break;
      case "profileNavItem":
        //Profile
        const TokenString =sessionStorage.getItem('token');
        const GetToken = JSON.parse(TokenString);
        document.getElementById("content").innerHTML = "Profile:<br>Name: "+GetToken.name+"<br>Email: "+GetToken.email+"<br>Role: "+GetToken.role;        
        break;
      default:
        break;
    }
  }
  async function Modify() {
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    let role = document.getElementById("role").value;
    role = role.split(",");

    $.ajax({
      method:'PUT',
      url:'http://localhost:5117/Userapi/User',
      contentType: "application/json",
      data: JSON.stringify({
        Email: email,
        Name: name,
        Password:null,
        Nonce:null,
        role: role
      }),
    })
}

async function Add(email, name, password) {
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
    error: function (xhr, textStatus, errorThrown) {
      document.getElementById("errorRegistration").textContent = "Error Creation: " + xhr.status +" "+ textStatus +" "+ errorThrown;

    },
  });
}

async function closeModal() {
  document.getElementById("myModal").style.display = "none";
  document.getElementById("myModalAdd").style.display = "none";
}
async function submitForm(event) {
  event.preventDefault();
  closeModal();
  document.getElementById('adminNavItem').click();
}