document.addEventListener('DOMContentLoaded', function () {
    let btn_yes = document.getElementById('btn1-yes');
    let btn_no = document.getElementById('btn1-no');

    if (customBool1 != null && customBool1.value === 'false') {

        btn_no.style.backgroundColor = 'rgb(0,0,0,255)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,0)';
    }

    btn_yes = document.getElementById('btn2-yes');
    btn_no = document.getElementById('btn2-no');

    if (customBool2 != null && customBool2.value === 'false') {

        btn_no.style.backgroundColor = 'rgb(0,0,0,255)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,0)';
    }

    btn_yes = document.getElementById('btn3-yes');
    btn_no = document.getElementById('btn3-no');

    if (customBool3 != null && customBool3.value === 'false') {

        btn_no.style.backgroundColor = 'rgb(0,0,0,255)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,0)';
    }

    ///////

    btn_yes = document.getElementById('btn1-yes');
    btn_no = document.getElementById('btn1-no');

    console.log(customBool1.value);

    if (customBool1 != null && customBool1.value === 'true') {

        console.log('Worked');

        btn_no.style.backgroundColor = 'rgb(0,0,0,0)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,255)';
    }

    btn_yes = document.getElementById('btn2-yes');
    btn_no = document.getElementById('btn2-no');

    if (customBool2 != null && customBool2.value != null && customBool2.value === 'true') {

        btn_no.style.backgroundColor = 'rgb(0,0,0,0)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,255)';
    }

    btn_yes = document.getElementById('btn3-yes');
    btn_no = document.getElementById('btn3-no');

    if (customBool3 != null && customBool3.value === 'true') {

        btn_no.style.backgroundColor = 'rgb(0,0,0,0)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,255)';
    }
});
function createItem() {
    let form = document.getElementById('form-data');

    let textAreasArray = form.getElementsByTagName('textarea');
    let inputsArray = form.getElementsByTagName('input');

    console.log(textAreasArray.length);

    for (var i = 0; i < textAreasArray.length; i++) {

        if (thisTextEmpty(textAreasArray[i].value)) {
            alert('ERROR! Fill in all empty fields');
            return false;
        }
    }

    console.log(inputsArray.length);

    for (var i = 0; i < inputsArray.length; i++) {

        const text = inputsArray[i].value.toString();

        if (thisTextEmpty(text)) {

            console.log(text);
            console.log(inputsArray[i]);
            alert('ERROR! Fill in all empty fields');
            return false;
        }

    }

    form.submit();

    return true;
}

let customBool1 = document.getElementById('customBool1');
let customBool2 = document.getElementById('customBool2');
let customBool3 = document.getElementById('customBool3');

function thisTextEmpty(str) {
    const trimmedStr = str.replace(' ', '');

    return (trimmedStr === '');
}

function btn_customBool1_yes() {
    let btn_yes = document.getElementById('btn1-yes');
    let btn_no = document.getElementById('btn1-no');

    if (customBool1.value === 'false') {
        customBool1.value = 'true'

        btn_no.style.backgroundColor = 'rgb(0,0,0,0)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,255)';
    }
}

function btn_customBool2_yes() {
    let btn_yes = document.getElementById('btn2-yes');
    let btn_no = document.getElementById('btn2-no');

    if (customBool2.value === 'false') {
        customBool2.value = 'true'

        btn_no.style.backgroundColor = 'rgb(0,0,0,0)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,255)';
    }
}

function btn_customBool3_yes() {
    let btn_yes = document.getElementById('btn3-yes');
    let btn_no = document.getElementById('btn3-no');

    if (customBool3.value === 'false') {
        customBool3.value = 'true'

        btn_no.style.backgroundColor = 'rgb(0,0,0,0)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,255)';
    }
}

function btn_customBool1_no() {
    let btn_yes = document.getElementById('btn1-yes');
    let btn_no = document.getElementById('btn1-no');

    if (customBool1.value === 'true') {
        customBool1.value = 'false'

        btn_no.style.backgroundColor = 'rgb(0,0,0,255)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,0)';
    }
}

function btn_customBool2_no() {
    let btn_yes = document.getElementById('btn2-yes');
    let btn_no = document.getElementById('btn2-no');

    if (customBool2.value === 'true') {
        customBool2.value = 'false'

        btn_no.style.backgroundColor = 'rgb(0,0,0,255)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,0)';
    }
}

function btn_customBool3_no() {
    let btn_yes = document.getElementById('btn3-yes');
    let btn_no = document.getElementById('btn3-no');

    if (customBool3.value === 'true') {
        customBool3.value = 'false'

        btn_no.style.backgroundColor = 'rgb(0,0,0,255)';
        btn_yes.style.backgroundColor = 'rgb(0,0,0,0)';
    }
}
