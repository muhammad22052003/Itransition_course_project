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
    let pageLang = document.getElementsByTagName('html')[0].getAttribute('lang');

    pageLang = document.getElementById('select-regions').value;

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