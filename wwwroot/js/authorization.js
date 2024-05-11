let name_field = document.getElementById("name_field");
let lastname_field = document.getElementById("lastname_field");
let email_field = document.getElementById("email_field");
let password_field = document.getElementById("password_field");
let confirmPassword_field = document.getElementById("confirmPassword_field");
let serverUrl = window.location.origin;

password_field.addEventListener("input", async function () {
    if (password_field.value == ' ') {
        password_field.value = '';
        return;
    }
});

confirmPassword_field.addEventListener("input", async function () {
    if (password_field.value == ' ') {
        password_field.value = '';
        return;
    }
});

async function login() {
    const postUrl = serverUrl + '/apiUser' + '/login'

    /*if (!isValidEmail(email_field.value)) {
        let errorMessage_field = document.getElementById('email_errorMessage');
        errorMessage_field.innerText = '* Uncorrect email format';
    }
    else {
        let errorMessage_field = document.getElementById('email_errorMessage');
        errorMessage_field.innerText = null;
    }

    if (password_field.value.length < 4) {
        let errorMessage_field = document.getElementById('password_errorMessage');
        errorMessage_field.innerText = '* Uncorect count password symbols (4-40)';
    }
    else {
        let errorMessage_field = document.getElementById('password_errorMessage');
        errorMessage_field.innerText = null;
    }*/

    const response = await fetch(postUrl, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            'Email': email_field.value,
            'Password': password_field.value
        })
    });

    if (response.ok) {
        document.cookie = response.headers.get('userData');
        window.location.replace(serverUrl + '/home' + '/index');
    }
}

async function registration() {
    const postUrl = serverUrl + '/apiUser' + '/registration'

    const response = await fetch(postUrl, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            'Name': name_field.value,
            'Lastname':lastname_field.value,
            'Email': email_field.value,
            'Password': password_field.value,
            'ConfirmPassword': confirmPassword_field.value,
        })
    });

    if (response.ok) {
        window.location.replace(serverUrl + '/login' + '/index');
    }
    console.log("error");
}

function isValidEmail(email) {
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailRegex.test(email);
}
