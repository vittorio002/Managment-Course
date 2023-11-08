let currentUser = null;

async function login() {
    const email = document.getElementById('login-username').value;
    const password = document.getElementById('login-password').value;
    
    if (email === u.email && password === u.password) {
        currentUser = { email };

    } else {
        alert('Login fallito. Nome utente o password errati.');
    }
}

async function register(){
    const email = document.getElementById('register-email').value;
    const name = document.getElementById('register-name').value;
    const password = document.getElementById('register-password').value;


}