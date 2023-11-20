//on reload verify if the token is in session
function checkToken() {
  if (sessionStorage.getItem("token")) showApplication();
}

//login
async function login() {
  if (!sessionStorage.getItem("token")) {
    let email = document.getElementById("login-email").value;

    const response = await Nonce(email);

    if (!response.ok) {
      handleError(response.status);
    } else {
      const nonce = await response.json();
      let password = document.getElementById("login-password").value;
      const newPassword = password + nonce;

      const response2 = await LoginAndToken(email, newPassword);

      if (!response2.ok) {
        handleError(response2.status);
      } else {
        await response2
          .json()
          .then((value) =>
            sessionStorage.setItem("token", JSON.stringify(value))
          );
        showApplication();
      }
    }
  } else showApplication();
}

//gestion error
function handleError(status) {
  if (status == 401) {
    alert("Token Not Valid. Go To Login.");
    sessionStorage.removeItem("token");
    location.reload();
  } else if (status == 500) {
    alert("Connection Error");
  } else {
    alert("Request Error");
  }
}

//registration
function register() {
  const email = document.getElementById("register-email").value;
  const name = document.getElementById("register-name").value;
  const password = document.getElementById("register-password").value;
  Add(email, name, password);
  switchToLogin();
}

//logout with remove token from session
function logout() {
  sessionStorage.removeItem("token");
  location.reload();
}

//close all modals
function closeModal() {
  document.getElementById("myModal").style.display = "none";
  document.getElementById("myModalAdd").style.display = "none";
  document.getElementById("myModalAddProgram").style.display = "none";
  document.getElementById("calendar").style.display = "none";
}

//prevent default on submit
function submitForm(event) {
  event.preventDefault();
  closeModal();
}

//calendar
function toggleCalendario() {
  let calendario = document.getElementById("calendar");
  calendario.style.display =
    calendario.style.display === "block" ? "none" : "block";
  if (calendario.style.display === "block") {
    popolaGiorni();
    popolaMesi();
    popolaAnni();
  }
}

//hour
function popolaOre() {
  let selectOra = document.getElementById("hour");
  selectOra.innerHTML = "";
  for (let i = 9; i <= 18; i++) {
    let option = document.createElement("option");
    option.value = i;
    option.text = i;
    selectOra.appendChild(option);
  }
}

//day of calendar
function popolaGiorni() {
  let selectGiorno = document.getElementById("giorno");
  selectGiorno.innerHTML = "";
  for (let i = 1; i <= 31; i++) {
    let option = document.createElement("option");
    option.value = i;
    option.text = i;
    selectGiorno.appendChild(option);
  }
}

//mouth of calendar
function popolaMesi() {
  let selectMese = document.getElementById("mese");
  let mesi = [
    "Gennaio",
    "Febbraio",
    "Marzo",
    "Aprile",
    "Maggio",
    "Giugno",
    "Luglio",
    "Agosto",
    "Settembre",
    "Ottobre",
    "Novembre",
    "Dicembre",
  ];
  selectMese.innerHTML = "";
  for (let i = 0; i < mesi.length; i++) {
    let option = document.createElement("option");
    option.value = i + 1;
    option.text = mesi[i];
    selectMese.appendChild(option);
  }
}

//year of calendar
function popolaAnni() {
  let selectAnno = document.getElementById("anno");
  let annoCorrente = new Date().getFullYear();
  selectAnno.innerHTML = "";
  for (let i = annoCorrente; i <= annoCorrente + 10; i++) {
    let option = document.createElement("option");
    option.value = i;
    option.text = i;
    selectAnno.appendChild(option);
  }
}

//formatting string of data
function selezionaData() {
  let giorno = document.getElementById("giorno").value;
  let mese = document.getElementById("mese").value;
  let anno = document.getElementById("anno").value;
  let dateInput = document.getElementById("dateInput");
  let hour = document.getElementById("pHour");

  dateInput.value = giorno + "/" + mese + "/" + anno;
  document.getElementById("calendar").style.display = "none";
}
