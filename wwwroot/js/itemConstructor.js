function createItem() {
    let form = document.getElementById('form-data');
    let textAreasArray = document.getElementsByTagName('textarea');
    let inputsArray = document.getElementsByTagName('input');

    for (var i = 0; i < textAreasArray.length; i++) {
        if (thisTextEmpty(textAreasArray[i].value)) {
            alert('ERROR! Fill in all empty fields');
            return;
        }

        if (thisTextEmpty(inputsArray[i].value)) {
            alert('ERROR! Fill in all empty fields');
            return;
        }

        form.submit();
    }
}

let customBool1 = document.getElementById('customBool1');
let customBool2 = document.getElementById('customBool2');
let customBool3 = document.getElementById('customBool3');

function thisTextEmpty(str) {
    const trimmedStr = str.replace(/\s/g, '');

    return (trimmedStr === '');
}

function btn_customBool1_yes() {
    let btn_yes = document.getElementById('btn1-yes');
    let btn_no = document.getElementById('btn1-no');

    console.log(btn_yes.value);
    console.log('btn-yes-1');

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

    console.log(btn_no.value);
    console.log('btn-no-1');

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
