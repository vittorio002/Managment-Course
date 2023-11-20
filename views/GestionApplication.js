//verify if the token is in storege on reload
window.onload=checkToken;

//switch to registration page
function switchToRegistration() {
  document.getElementById("login-page").style.display = "none";
  document.getElementById("registration-page").style.display = "block";
}

//switch on login page
function switchToLogin() {
  document.getElementById("login-page").style.display = "block";
  document.getElementById("registration-page").style.display = "none";
  document.getElementById("application-page").style.display = "none";
}

//switch to application page
function showApplication() {
  const TokenString = sessionStorage.getItem("token");
  const GetToken = JSON.parse(TokenString);

  document.getElementById("login-page").style.display = "none";
  document.getElementById("registration-page").style.display = "none";
  document.getElementById("application-page").style.display = "block";
  document.getElementById("userNavItem").style.display = "block";


  //see different tab verifying the roles of user, in the token
  if (GetToken.role.includes("admin")) {
    document.getElementById("adminNavItem").style.display = "block";
  } else {
    document.getElementById("adminNavItem").style.display = "none";
  }

  if (GetToken.role.includes("labManager")) {
    document.getElementById("labNavItem").style.display = "block";
  } else {
    document.getElementById("labNavItem").style.display = "none";
  }
  document.getElementById("profileNavItem").textContent += GetToken.name;

  document.getElementById("profileNavItem").click();
}

//gestion click on tabs
async function gestisciClic(event) {
  const targetId = event.target.id;

  //gestion switch on tabs
  switch (targetId) {
    case "userNavItem":
      //User Interface
      const content = document.getElementById("content");
      content.innerHTML = "";

      const divLabs = document.createElement("div");
      divLabs.id = 'divLabs';
      const div = document.createElement("div");
      const inputHour = document.createElement("select");
      inputHour.textContent = "Select Hour";
      for (let i = 9; i <= 18; i++) {
        let option = document.createElement("option");
        option.value = i;
        option.text = i;
        inputHour.appendChild(option);
      }
      inputHour.addEventListener("click", function () {});

      const Date = document.createElement("input");
      Date.type = "text";
      Date.setAttribute("readonly",true);
      Date.id = "dateInput";

      const inputDate = document.createElement("button");
      inputDate.style.backgroundImage = 'url(./img/calendar.png)';
      inputDate.style.backgroundSize = 'cover';
      inputDate.style.width = '25px';
      inputDate.style.height = '25px';

      inputDate.addEventListener("click", function () {
        toggleCalendario();
      });

      const pDate = document.createElement("p");
      pDate.textContent = "Date: ";
      const pHour = document.createElement("p");
      pHour.textContent = "Hour: ";

      const search = document.createElement("button");
      search.style.backgroundImage = 'url(./img/search.jpg)';
      search.style.backgroundSize = 'cover';
      search.style.width = '25px';
      search.style.height = '25px';

      search.id = "search";
      search.addEventListener("click", async function () {
        if(dateInput.value == ""){
          alert("fill in all fields");
        }
        else
        {
          divLabs.innerHTML = "";
          await SerachFreePc(dateInput.value, inputHour.value)
            .then((response =>{
              for (const lab of response) {
                const divLab = document.createElement("div");
                divLab.textContent = lab.name;
                for (const computer of lab.computers) {
                  const divPc = document.createElement("div");
                  divPc.textContent =
                    computer.name +
                    " | Id: " +
                    computer.id +
                    " | Status: " +
                    computer.status +
                    " | Program: " +
                    computer.program;

                  const ButtonAdd = document.createElement("button");
                  ButtonAdd.style.backgroundImage = 'url(./img/reserve.png)';
                  ButtonAdd.style.backgroundSize = 'cover';
                  ButtonAdd.style.width = '25px';
                  ButtonAdd.style.height = '25px';

                  ButtonAdd.addEventListener("click", async function () {
                    const TokenString = sessionStorage.getItem("token");
                    const GetToken = JSON.parse(TokenString);
                    await SendReservation(GetToken.email, computer.name, inputHour.value, dateInput.value);
                  });

                  divPc.appendChild(ButtonAdd);
                  divLab.appendChild(divPc);
                }

                divLabs.appendChild(divLab);
                content.appendChild(divLabs);
              }
          }));
         }       
      });
      const br = document.createElement('br');
      const br2 = document.createElement('br');

      div.appendChild(pDate);
      div.appendChild(Date);
      div.appendChild(inputDate);
      div.appendChild(pHour);
      div.appendChild(inputHour);
      div.appendChild(br);
      div.appendChild(br2);
      div.appendChild(search);
      content.appendChild(div);
      break;
    case "adminNavItem":
      //Admin Interface

      GetUsers();

      break;
    case "labNavItem":
      //Lab Manager Interface
      GetLabs();
      break;
    case "profileNavItem":
      //Profile
      const TokenString = sessionStorage.getItem("token");
      const GetToken = JSON.parse(TokenString);

      const Profile = document.getElementById("content");
      Profile.innerHTML =
        "Profile:<br>Name: " +
        GetToken.name +
        "<br>Email: " +
        GetToken.email +
        "<br>Role: " +
        GetToken.role;

        GetSpecificUserReservation(GetToken.email);

      break;
      case "logout":
        logout();
        break;
    default:
      break;
  }
}