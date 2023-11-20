//get all labs, for lab manager for managment
function GetLabs(){
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
              ButtonDelete.style.backgroundImage = 'url(./img/delete.jpg)';
              ButtonDelete.style.backgroundSize = 'cover';
              ButtonDelete.style.width = '25px';
              ButtonDelete.style.height = '25px';

              ButtonDelete.addEventListener("click", function () {
                DeletePc(computer.id);
              });

              const ButtonModPc = document.createElement("button");
              ButtonModPc.style.backgroundImage = 'url(./img/pencil.png)';
              ButtonModPc.style.backgroundSize = 'cover';
              ButtonModPc.style.width = '25px';
              ButtonModPc.style.height = '25px';
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
            ButtonAdd.style.backgroundImage = 'url(./img/add.jpg)';
            ButtonAdd.style.backgroundSize = 'cover';
            ButtonAdd.style.width = '25px';
            ButtonAdd.style.height = '25px';
            ButtonAdd.addEventListener("click", function () {
              AddComputer(lab.name);
            });

            divLab.appendChild(ButtonAdd);
            content.appendChild(divLab);
          }
          GetReservation();
        },
        error: function(xhr) {
          handleError(xhr.status);
        }
      });
}

//modify status, name or programs of pc (only for lab manager)
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
        handleError(xhr.status);
      }
    });
  }

  //add computer in specific lab (only for lab manager)
  function AddComputer(labName){
    $.ajax({
        method: "POST",
        contentType: "application/json",
        url: "http://localhost:5033/Labapi/Lab/Computer",
        beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
        data: JSON.stringify(labName),
        success: function () {
          document.getElementById("labNavItem").click();
        },
        error: function(xhr) {
            handleError(xhr.status);
        }
      });
  }

  //delete one computer (only for lab manager)
  function DeletePc(id){
    $.ajax({
        method: "DELETE",
        contentType: "application/json",
        url: "http://localhost:5033/Labapi/Lab/Computer",
        beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
        data: JSON.stringify(id),
        success: function () {
          document.getElementById("labNavItem").click();
        },
        error: function(xhr) {
          handleError(xhr.status);
        }
      });
  }