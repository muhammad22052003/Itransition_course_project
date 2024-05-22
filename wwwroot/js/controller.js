let serverUrl = window.location.origin;

document.addEventListener('DOMContentLoaded', async function () {
    console.log("Controller");


    await defineAllCategories();
    await defineAllLanguages();
})

async function defineAllCategories() {
    const response = await fetch(serverUrl + '/apicategory' + '/getall', {
        method: "GET"
    });

    let categories = await response.json();
    const categories_select = document.getElementById('select-categories');

    categories_select.innerHTML = '';

    for (var i = 0; i < categories.length; i++) {
        let option = document.createElement('option');
        option.value = i;
        option.textContent = categories[i];

        categories_select.appendChild(option);
    }
}
async function defineAllLanguages() {

    try {
        const response = await fetch(serverUrl + '/apilang' + '/getall', {
            method: "GET"
        });

        let regions = await response.json();
        const regions_select = document.getElementById('select-regions');

        regions_select.innerHTML = '';

        for (var i = 0; i < regions.length; i++) {
            let option = document.createElement('option');
            option.value = i;
            option.textContent = regions[i];

            regions_select.appendChild(option);
        }
    } catch (e) {
        console.log(e);
    }
}

function changeLang() {
    let langSelect = document.getElementById('select-regions');

    let pageLang = document.getElementsByTagName('html')[0].getAttribute('lang');

    pageLang = document.getElementById('select-regions').value;

    window.location = serverUrl + '/home/index/' + langSelect.value;

    console.log(window.location)
}

function selectAll() {
    let chechboxes = document.querySelectorAll('#select-field');

    for (var i = 0; i < chechboxes.length; i++) {
        chechboxes[i].checked = true;
    }
}

function cancelAll() {
    let chechboxes = document.querySelectorAll('#select-field');

    for (var i = 0; i < chechboxes.length; i++) {
        chechboxes[i].checked = false;
    }
}

let newPassword = document.getElementById('newPass_field');
let curPassword = document.getElementById('curPass_field');
let confPassword = document.getElementById('confPass_field');

newPassword.addEventListener("input", async function () {
    if (newPassword.value == ' ') {
        newPassword.value = '';
        return;
    }
});
curPassword.addEventListener("input", async function () {
    if (curPassword.value == ' ') {
        curPassword.value = '';
        return;
    }
});
confPassword.addEventListener("input", async function () {
    if (confPassword.value == ' ') {
        confPassword.value = '';
        return;
    }
});

function myProfile() {
    let editBlock = document.getElementById('edit-block');
    let profileBlock = document.getElementById('profile-block');

    editBlock.hidden = true;
    profileBlock.hidden = false;
}

function editProfile() {
    let editBlock = document.getElementById('edit-block');
    let profileBlock = document.getElementById('profile-block');

    editBlock.hidden = false;
    profileBlock.hidden = true;
}

async function sendUserEditForm() {
    let errorMessage = document.getElementById('errorInputDataMessage');
    let formElement = document.getElementById('edit-profile-form');
    let inputs = formElement.getElementsByTagName('input');

    errorMessage.hidden = true;

    if (newPassword.value == curPassword.value) {
        alert('The new and old password are the same');
        return;
    }

    if (newPassword.value != confPassword.value) {
        alert('Password mismatch');
        return;
    }

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].value.replace(' ', '') == '') {
            alert('ERROR! Fill in all empty fields');
            return;
        }
    }

    let form = new FormData(formElement);
    const url = serverUrl + '/myprofile/editProfile';

    try {
        const response = await fetch(url, {
            method: 'POST',
            credentials: 'include',
            body: form
        });

        if (response.ok) {
            console.log('User profile edited');
            window.location.reload();
        }
        else {
            errorMessage.hidden = false;
        }

    } catch (e) {
        console.log(e);
        errorMessage.hidden = false;
        clearPasswordFields();
    }

    clearPasswordFields();
}

function clearPasswordFields() {
    newPassword.value = '';
    curPassword.value = '';
    confPassword.value = '';
}

