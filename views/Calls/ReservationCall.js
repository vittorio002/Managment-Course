//serch all computer available in specific day and hour
function SerachFreePc(date, hour){
      return new Promise((resolve,reject)=>{
        $.ajax({
          method: "POST",
          contentType: "application/json",
          beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
          url: "http://localhost:5033/Labapi/Lab/Available",
          data: JSON.stringify({
            Date: date,
            Hour: hour,
          }),
          success: function (response) {
            resolve(response);
          },
          error: function(xhr) {
            handleError(xhr.status);
          }
        });
      })
}

//send reservation at one hour in one day at specific computer
function SendReservation(email, pc, hour, date){
      $.ajax({
        method: "POST",
        url: "http://localhost:5033/Reservation/Reservation",
        contentType: "application/json", 
        beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
        data: JSON.stringify({
          NameUser: email,
          NamePc: pc,
          Hour: hour,
          Date: date,
        }),
        success: function (response) {
          document.getElementById("userNavItem").click(); 
        },
        error: function(xhr) {
          handleError(xhr.status);
        }
      });
}

//get all reservation (only for lab manager)
function GetReservation(){
  $.ajax({
    method: "GET",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    url: "http://localhost:5033/Reservation/Reservation",
    success: function (response) {
      const div = document.createElement("div");
      div.textContent = "Reservations:";
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
        buttonDel.style.backgroundImage = 'url(./img/delete.jpg)';
        buttonDel.style.backgroundSize = 'cover';
        buttonDel.style.width = '25px';
        buttonDel.style.height = '25px';
        buttonDel.classList.add("rigth");

        buttonDel.addEventListener("click", function () {
          const TokenString = sessionStorage.getItem("token");
          const GetToken = JSON.parse(TokenString);
          DeleteReservation(GetToken.email, reservation.namePc, reservation.hour, reservation.date);
        });

        single.appendChild(buttonDel);
        div.appendChild(single);
      }
      const br = document.createElement('br');
      let content = document.getElementById("content");
      content.appendChild(br);
      content.appendChild(div);
    },
    error: function(xhr) {
      handleError(xhr.status);
    }
  });
}

//get specific user reservation
function GetSpecificUserReservation(UserEmail){
  $.ajax({
    method: "POST",
    contentType: "application/json",
    url: "http://localhost:5033/Reservation/Reservation/Specific",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    data: JSON.stringify(UserEmail),
    success: function (response) {
      const Profile = document.getElementById("content");
     
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
        buttonDel.style.backgroundImage = 'url(./img/delete.jpg)';
        buttonDel.style.backgroundSize = 'cover';
        buttonDel.style.width = '25px';
        buttonDel.style.height = '25px';
        buttonDel.classList.add("rigth");

        buttonDel.addEventListener("click", function () {
          DeleteReservation(UserEmail, reservation.namePc, reservation.hour, reservation.date);
          location.reload();
        });
        
        res.appendChild(buttonDel);
        reservations.appendChild(res);
      }
      const br = document.createElement('br');
      const br2 = document.createElement('br');

      Profile.appendChild(br);
      Profile.appendChild(br2);
      Profile.appendChild(reservations);
    },
    error: function(xhr) {
      handleError(xhr.status);
    }
  });
}

//delete one reservation
function DeleteReservation(email, pc, hour, date){
  $.ajax({
    method: "Delete",
    url: "http://localhost:5033/Reservation/Reservation",
    contentType: "application/json",
    beforeSend: function(xhr){xhr.setRequestHeader('Authentication', sessionStorage.getItem("token"))},
    data: JSON.stringify({
      NameUser: email,
      NamePc: pc,
      Hour: hour,
      Date: date,
    }),
    success: function () {
      document.getElementById("labNavItem").click();
    },
    error: function(xhr) {
        handleError(xhr.status);
    }
  });
}