let currentUser = null;

async function login() {

    let email = document.getElementById('login-email').value;
    $.ajax({
        url:'http://localhost:5117/Userapi/Auth/GetNonce',
        method:'POST',
        data: {email},
        success:function (response){
            let nonce = response.nonce;
            let password = document.getElementById('login-password').value;
            password+=nonce;
            $.ajax({
                url:'http://localhost:5117/Userapi/Auth/Login',
                method:'POST',
                data:{Email:email, Password:password},
                success:function(token){
                    console.log(fatto);
                    sessionStorage.setItem('token',token);                    
                },
                error: function(error){
                    console.error('Error during login');
                }
            });
        },
        error: function (error){
            console.error('Error during get nonce');
        }
    });
}

async function register(){
    const email = document.getElementById('register-email').value;
    const name = document.getElementById('register-name').value;
    const password = document.getElementById('register-password').value;


}