window.onload=checkToken;

function checkToken(){
  if(sessionStorage.getItem("token"))
    showApplication();
}

async function login() {
  if(!sessionStorage.getItem("token")){
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
          error: function(xhr) {
            if (xhr.status === 401) {
                alert('Token non valido. Effettua il login.');
                sessionStorage.removeItem("token");
                location.reload();
            } else {
                alert('Errore nella richiesta: ' + xhr.statusText);
            }
          }
        });
      },
      error: function(xhr) {
        if (xhr.status === 401) {
            alert('Token non valido. Effettua il login.');
            sessionStorage.removeItem("token");
            location.reload();
        } else {
            alert('Errore nella richiesta: ' + xhr.statusText);
        }
      }
    });
  } else showApplication();
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
  const TokenString = sessionStorage.getItem("token");
  const GetToken = JSON.parse(TokenString);

  document.getElementById("login-page").style.display = "none";
  document.getElementById("registration-page").style.display = "none";
  document.getElementById("application-page").style.display = "block";
  document.getElementById("userNavItem").style.display = "block";

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

async function gestisciClic(event) {
  const targetId = event.target.id;

  switch (targetId) {
    case "userNavItem":
      //User Interface
      const content = document.getElementById("content");
      content.innerHTML = "";

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
      inputDate.textContent = "Select Date";
      inputDate.addEventListener("click", function () {
        toggleCalendario();
      });

      const pDate = document.createElement("p");
      pDate.textContent = "Date: ";
      const pHour = document.createElement("p");
      pHour.textContent = "Hour: ";

      const search = document.createElement("button");
      search.textContent = "Search";
      search.id = "search";
      search.addEventListener("click", function () {
        $.ajax({
          method: "POST",
          contentType: "application/json",
          beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
          url: "http://localhost:5033/Labapi/Lab/Available",
          data: JSON.stringify({
            Date: dateInput.value,
            Hour: inputHour.value,
          }),
          success: function (response) {
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
                ButtonAdd.textContent = "Reserve";
                ButtonAdd.addEventListener("click", function () {
                  const TokenString = sessionStorage.getItem("token");
                  const GetToken = JSON.parse(TokenString);
                  $.ajax({
                    method: "POST",
                    url: "http://localhost:5033/Reservation/Reservation",
                    contentType: "application/json", 
                    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
                    data: JSON.stringify({
                      NameUser: GetToken.email,
                      NamePc: computer.name,
                      Hour: inputHour.value,
                      Date: dateInput.value,
                    }),
                    success: function () {
                      document.getElementById("userNavItem").click();
                    },
                    error: function(xhr) {
                      if (xhr.status === 401) {
                          alert('Token non valido. Effettua il login.');
                          sessionStorage.removeItem("token");
                          location.reload();
                      } else {
                          alert('Errore nella richiesta: ' + xhr.statusText);
                      }
                    }
                  });
                });

                divPc.appendChild(ButtonAdd);

                divLab.appendChild(divPc);
              }

              content.appendChild(divLab);
            }
          },
          error: function(xhr) {
            if (xhr.status === 401) {
                alert('Token non valido. Effettua il login.');
                sessionStorage.removeItem("token");
                location.reload();
            } else {
                alert('Errore nella richiesta: ' + xhr.statusText);
            }
          }
        });
      });

      div.appendChild(pDate);
      div.appendChild(inputDate);
      div.appendChild(Date);
      div.appendChild(pHour);
      div.appendChild(inputHour);
      div.appendChild(search);
      content.appendChild(div);
      break;
    case "adminNavItem":
      //Admin Interface
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

            buttonUser.textContent = "Modify";
            buttonUser.classList.add("Lessrigth");
            buttonUser.addEventListener("click", function () {
              document.getElementById("myModal").style.display = "block";

              document.getElementById("name").value = user.name;
              document.getElementById("email").value = user.email;
              document.getElementById("role").value = user.role;
            });

            const ButtonDelete = document.createElement("button");
            ButtonDelete.classList.add("rigth");
            ButtonDelete.textContent = "Delete";
            ButtonDelete.addEventListener("click", function () {
              $.ajax({
                method: "DELETE",
                contentType: "application/json",
                url: "http://localhost:5117/Userapi/User",
                beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
                data: JSON.stringify(user.email),
                success: function () {
                  document.getElementById("adminNavItem").click();
                },
                error: function(xhr) {
                  if (xhr.status === 401) {
                      alert('Token non valido. Effettua il login.');
                      sessionStorage.removeItem("token");
                      location.reload();
                  } else {
                      alert('Errore nella richiesta: ' + xhr.statusText);
                  }
                }
              });
            });

            divUser.appendChild(buttonUser);
            divUser.appendChild(ButtonDelete);
            content.appendChild(divUser);
          }
          const ButtonAdd = document.createElement("button");
          ButtonAdd.textContent = "Add User";
          ButtonAdd.addEventListener("click", function () {
            document.getElementById("myModalAdd").style.display = "block";
            document.getElementById("adminNavItem").click();
          });
          content.appendChild(ButtonAdd);
        },
        error: function(xhr) {
          if (xhr.status === 401) {
              alert('Token non valido. Effettua il login.');
              sessionStorage.removeItem("token");
              location.reload();
          } else {
              alert('Errore nella richiesta: ' + xhr.statusText);
          }
        }
      });

      break;
    case "labNavItem":
      //Lab Manager Interface
      $.ajax({
        method: "GET",
        beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
        url: "http://localhost:5033/Labapi/Lab",
        success: function (response) {
          let content = document.getElementById("content");
          content.innerHTML = "";
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

              const ButtonDelete = document.createElement("button");
              ButtonDelete.classList.add("rigth");
              ButtonDelete.textContent = "Delete";
              ButtonDelete.addEventListener("click", function () {
                $.ajax({
                  method: "DELETE",
                  contentType: "application/json",
                  url: "http://localhost:5033/Labapi/Lab/Computer",
                  beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
                  data: JSON.stringify(computer.id),
                  success: function () {
                    document.getElementById("labNavItem").click();
                  },
                  error: function(xhr) {
                    if (xhr.status === 401) {
                        alert('Token non valido. Effettua il login.');
                        sessionStorage.removeItem("token");
                        location.reload();
                    } else {
                        alert('Errore nella richiesta: ' + xhr.statusText);
                    }
                  }
                });
              });

              const ButtonModPc = document.createElement("button");
              ButtonModPc.textContent = "Modify";
              ButtonModPc.classList.add("Lessrigth");
              ButtonModPc.addEventListener("click", function () {
                document.getElementById("myModalAddProgram").style.display =
                  "block";
                document.getElementById("namePc").value = computer.name;
                document.getElementById("IdPc").value = computer.id;
                document.getElementById("status").value = computer.status;
                document.getElementById("programs").value = computer.program;
              });

              divPc.appendChild(ButtonModPc);
              divPc.appendChild(ButtonDelete);
              divLab.appendChild(divPc);
            }
            const ButtonAdd = document.createElement("button");
            ButtonAdd.textContent = "Add";
            ButtonAdd.addEventListener("click", function () {
              $.ajax({
                method: "POST",
                contentType: "application/json",
                url: "http://localhost:5033/Labapi/Lab/Computer",
                beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
                data: JSON.stringify(lab.name),
                success: function () {
                  document.getElementById("labNavItem").click();
                },
                error: function(xhr) {
                  if (xhr.status === 401) {
                      alert('Token non valido. Effettua il login.');
                      sessionStorage.removeItem("token");
                      location.reload();
                  } else {
                      alert('Errore nella richiesta: ' + xhr.statusText);
                  }
                }
              });
            });
            divLab.appendChild(ButtonAdd);
            content.appendChild(divLab);
          }
          $.ajax({
            method: "GET",
            beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
            url: "http://localhost:5033/Reservation/Reservation",
            success: function (response) {
              const div = document.createElement("div");
              div.textContent = "Reservations";
              for (const reservation of response) {
                const single = document.createElement("div");
                single.textContent =
                  reservation.nameUser +
                  " | " +
                  reservation.namePc +
                  " | " +
                  reservation.date +
                  " | at:" +
                  reservation.hour;

                const buttonDel = document.createElement("button");
                buttonDel.textContent = "Delete";
                buttonDel.classList.add("rigth");
                buttonDel.addEventListener("click", function () {
                  const TokenString = sessionStorage.getItem("token");
                  const GetToken = JSON.parse(TokenString);
                  $.ajax({
                    method: "Delete",
                    url: "http://localhost:5033/Reservation/Reservation",
                    contentType: "application/json",
                    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
                    data: JSON.stringify({
                      NameUser: GetToken.email,
                      NamePc: reservation.namePc,
                      Hour: reservation.hour,
                      Date: reservation.date,
                    }),
                    success: function () {
                      document.getElementById("labNavItem").click();
                    },
                    error: function(xhr) {
                      if (xhr.status === 401) {
                          alert('Token non valido. Effettua il login.');
                          sessionStorage.removeItem("token");
                          location.reload();
                      } else {
                          alert('Errore nella richiesta: ' + xhr.statusText);
                      }
                    }
                  });
                });

                single.appendChild(buttonDel);
                div.appendChild(single);
              }
              content.appendChild(div);
            },
            error: function(xhr) {
              if (xhr.status === 401) {
                  alert('Token non valido. Effettua il login.');
                  sessionStorage.removeItem("token");
                  location.reload();
              } else {
                  alert('Errore nella richiesta: ' + xhr.statusText);
              }
            }
          });
        },
        error: function(xhr) {
          if (xhr.status === 401) {
              alert('Token non valido. Effettua il login.');
              sessionStorage.removeItem("token");
              location.reload();
          } else {
              alert('Errore nella richiesta: ' + xhr.statusText);
          }
        }
      });
      break;
    case "profileNavItem":
      //Profile
      const TokenString = sessionStorage.getItem("token");
      const GetToken = JSON.parse(TokenString);

      $.ajax({
        method: "POST",
        contentType: "application/json",
        url: "http://localhost:5033/Reservation/Reservation/Specific",
        beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
        data: JSON.stringify(GetToken.email),
        success: function (response) {
          const Profile = document.getElementById("content");
          Profile.innerHTML =
            "Profile:<br>Name: " +
            GetToken.name +
            "<br>Email: " +
            GetToken.email +
            "<br>Role: " +
            GetToken.role;

          const reservations = document.createElement("div");

          reservations.textContent = "Your Reservation:";

          for (const reservation of response) {
            const res = document.createElement("div");
            res.textContent =
              reservation.namePc +
              " | " +
              reservation.date +
              " | at: " +
              reservation.hour;

            const buttonDel = document.createElement("button");
            buttonDel.textContent = "Delete";
            buttonDel.classList.add("rigth");
            buttonDel.addEventListener("click", function () {
              $.ajax({
                method: "Delete",
                url: "http://localhost:5033/Reservation/Reservation",
                contentType: "application/json",
                beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
                data: JSON.stringify({
                  NameUser: GetToken.email,
                  NamePc: reservation.namePc,
                  Hour: reservation.hour,
                  Date: reservation.date,
                }),
                success: function () {
                  document.getElementById("profileNavItem").click();
                },
                error: function(xhr) {
                  if (xhr.status === 401) {
                      alert('Token non valido. Effettua il login.');
                      sessionStorage.removeItem("token");
                      location.reload();
                  } else {
                      alert('Errore nella richiesta: ' + xhr.statusText);
                  }
                }
              });
            });

            res.appendChild(buttonDel);
            reservations.appendChild(res);
          }

          Profile.appendChild(reservations);
        },
        error: function(xhr) {
          if (xhr.status === 401) {
              alert('Token non valido. Effettua il login.');
              sessionStorage.removeItem("token");
              location.reload();
          } else {
              alert('Errore nella richiesta: ' + xhr.statusText);
          }
        }
      });
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
      if (xhr.status === 401) {
          alert('Token non valido. Effettua il login.');
          sessionStorage.removeItem("token");
          location.reload();
      } else {
          alert('Errore nella richiesta: ' + xhr.statusText);
      }
    }
  });
}

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
      if (xhr.status === 401) {
          alert('Token non valido. Effettua il login.');
          sessionStorage.removeItem("token");
          location.reload();
      } else {
          alert('Errore nella richiesta: ' + xhr.statusText);
      }
    }
  });
}

function ModifyPc() {
  const namePc = document.getElementById("namePc").value;
  const IdPc = document.getElementById("IdPc").value;
  let status = document.getElementById("status").value;

  if (status == "true") {
    status = true;
  } else {
    status = false;
  }
  let programs = document.getElementById("programs").value;
  programs = programs.split(",");
  console.log(namePc, IdPc, status, programs);

  $.ajax({
    method: "PUT",
    url: "http://localhost:5033/Labapi/Lab/Computer",
    contentType: "application/json",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    data: JSON.stringify({
      Name: namePc,
      Id: IdPc,
      Status: status,
      program: programs,
    }),
    success: function () {
      document.getElementById("labNavItem").click();
    },
    error: function(xhr) {
      if (xhr.status === 401) {
          alert('Token non valido. Effettua il login.');
          sessionStorage.removeItem("token");
          location.reload();
      } else {
          alert('Errore nella richiesta: ' + xhr.statusText);
      }
    }
  });
}

async function closeModal() {
  document.getElementById("myModal").style.display = "none";
  document.getElementById("myModalAdd").style.display = "none";
  document.getElementById("myModalAddProgram").style.display = "none";
  document.getElementById("calendar").style.display = "none";
}
async function submitForm(event) {
  event.preventDefault();
  closeModal();
}

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

function selezionaData() {
  let giorno = document.getElementById("giorno").value;
  let mese = document.getElementById("mese").value;
  let anno = document.getElementById("anno").value;
  let dateInput = document.getElementById("dateInput");
  let hour = document.getElementById("pHour");

  dateInput.value = giorno + "/" + mese + "/" + anno;
  document.getElementById("calendar").style.display = "none";
}
