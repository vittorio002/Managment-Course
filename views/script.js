let currentUser = null;

async function login() {

    let email = document.getElementById('login-email').value;
    $.ajax({
        method:'POST',
        url:'http://localhost:5117/Userapi/Auth/GetNonce',
        contentType: "application/json",
        data: JSON.stringify(email), 
        success:function (response){
            let nonce = response;
            let password = document.getElementById('login-password').value;
            password+=nonce;
            $.ajax({
                method:'POST',
                url:'http://localhost:5117/Userapi/Auth/Login',
                contentType: 'application/json',
                data: JSON.stringify({Email:email, Password:password}),
                success:function(token){
                    console.log(token);
                    sessionStorage.setItem('token', token);                    
                },
                error: function (xhr, textStatus, errorThrown) {
                    console.error('Error Login: ', xhr.status, textStatus, errorThrown);
                }
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error Nonce: ', xhr.status, textStatus, errorThrown);
        }
    });
}

async function register(){
    const email = document.getElementById('register-email').value;
    const name = document.getElementById('register-name').value;
    const password = document.getElementById('register-password').value;
    $.ajax({
        method:'POST',
        url:'http://localhost:5117/Userapi/User',
        contentType:"application/json",
        data: JSON.stringify({Email:email,Name:name,Password:password,Nonce:null,role:["user"]}),
        success:function(response){
            console.log("creato", response)
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error Creation: ', xhr.status, textStatus, errorThrown);
        }
    });
}