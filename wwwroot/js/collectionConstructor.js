let collectionName = document.getElementById('collectionName');
let collectionDescription = document.getElementById('collectionDescription');
let categoryName = document.getElementById('categoryName');

collectionName.addEventListener('input', function(){
    collectionData.Name = collectionName.value;
});

collectionDescription.addEventListener('input', function(){
    collectionData.Description = collectionDescription.value;
});

categoryName.addEventListener('change', function(){
    collectionData.CategoryName = categoryName.value;
});

let textColumnCount = 3;
let stringColumnCount = 3;
let boolColumnCount = 3;
let intColumnCount = 3;
let dateColumnCount = 3;

let collectionData = { Name: "", CategoryName: categoryName.value, Description: ""};

async function addConstructor(){
    createNewAddConstructor();
}

function createNewAddConstructor(){
    let currentConstructor = getAddConstructor();
    let newConstuctor = currentConstructor.cloneNode(true);
    let sectionsBlock = getSectionBlock();

    if(!checkConstructorData(currentConstructor)){
        return;
    }
    addCollectionData(currentConstructor);

    createColumnElement(currentConstructor);

    currentConstructor.outerHTML = '';

    let typeSelect = getTypesSelect(newConstuctor);
    defineTypeSelectElement(typeSelect);

    const sum = textColumnCount + stringColumnCount
    + boolColumnCount + intColumnCount + dateColumnCount;

    if(sum === 0){
        return;
    }

    sectionsBlock.appendChild(newConstuctor);
}

function checkConstructorData(constructor){
    let columnInfoSection = document.getElementById('columnInfo').cloneNode(true);
    let columnName = getAddConstructor()
    .querySelector('#columnName').value;       
    
    let typeName = getTypesSelect(constructor).value;

    if(thisTextEmpty(columnName) || thisTextEmpty(typeName)){
        return false;
    }

    return true;
}

function createColumnElement(constructor){
    let columnInfoSection = document.getElementById('columnInfo').cloneNode(true);
    let columnName = getAddConstructor()
    .querySelector('#columnName').value;       
    
    let typeName = getTypesSelect(constructor).value;

    defineAddedType(typeName);

    columnInfoSection.querySelector('#columnName').textContent =  columnName;
    columnInfoSection.querySelector('#columnTypeName').textContent = "Type : " + typeName;

    columnInfoSection.hidden = false;

    let sectionsBlock = getSectionBlock();

    sectionsBlock.appendChild(columnInfoSection);
}

function addCollectionData(constructor){
    let data = getAddConstructorData(constructor);

    collectionData = {...collectionData, ...data};
}

async function sendCollectionData(){
    if(!collectionDataIsValid()){
        postMessage("ERROR! Fill in all empty fields");

        return;
    }

    let url = serverUrl +  '/collectionconstructor/CreateCollection'

    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify(collectionData),
        });

        if (response.ok) {
            window.location.replace(serverUrl + '/home/index');
        }
    } catch (ex) {
        console.log(ex);
    }

    
}

function collectionDataIsValid(){
    console.log(collectionData.Name);
    console.log(collectionData.CategoryName);
    console.log(collectionData.Description);

    if(thisTextEmpty(collectionData.Name) ||
       thisTextEmpty(collectionData.CategoryName) ||
       thisTextEmpty(collectionData.Description)){
        return false;
     }

     return true;
}

function defineTypeSelectElement(selectNode){

    selectNode.innerHTML = '';

    if(textColumnCount > 0){
        let option = document.createElement('option');
        option.value = 'Text';
        option.textContent = 'Text';
        selectNode.appendChild(option);
    }
    if(stringColumnCount > 0){
        let option = document.createElement('option');
        option.value = 'String';
        option.textContent = 'String';
        selectNode.appendChild(option);
    }
    if(intColumnCount > 0){
        let option = document.createElement('option');
        option.value = 'Number';
        option.textContent = 'Number';
        selectNode.appendChild(option);
    }
    if(dateColumnCount > 0){
        let option = document.createElement('option');
        option.value = 'Date';
        option.textContent = 'Date';
        selectNode.appendChild(option);
    }
    if(boolColumnCount > 0){
        let option = document.createElement('option');
        option.value = 'Yes/No';
        option.textContent = 'Yes/No';
        selectNode.appendChild(option);
    }

    return selectNode;
}

function getTypesSelect(constructorNode){
    return constructorNode.querySelector('#column-types');
}

function getAddConstructor(){
    return getSectionBlock()
    .querySelector('#addCollumnConstructor');
}

function getSectionBlock(){
    return document.getElementById('sections-block');
}

function getAddConstructorData(constructor){
    let select = getTypesSelect(constructor);

    let columnName = constructor
    .querySelector('#columnName');
    let typeName = defineTypeName(select.value)

    let data = {};
    data[typeName] = columnName.value;

    return data;
}

function defineAddedType(text){

    if(text === "Text"){
        textColumnCount--;
    }
    if(text === "String"){
        stringColumnCount--;
    }
    if(text === "Number"){
        intColumnCount--;
    }
    if(text === "Yes/No"){
        boolColumnCount--;
    }
    if(text === "Date"){
        dateColumnCount--;
    }
}

function defineTypeName(text){

    if(text === "Text"){
        const number = 4 - textColumnCount;

        return 'CustomText' + number +'_name'
    }
    if(text === "String"){
        const number = 4 - stringColumnCount;

        return 'CustomString' + number +'_name'
    }
    if(text === "Number"){
        const number = 4 - intColumnCount;

        return 'CustomInt' + number +'_name'
    }
    if(text === "Yes/No"){
        const number = 4 - boolColumnCount;

        return 'CustomBool' + number +'_name'
    }
    if(text === "Date"){
        const number = 4 - dateColumnCount;

        return 'CustomDate' + number +'_name'
    }
}

function thisTextEmpty(str){
    const trimmedStr = str.replace(/\s/g, '');

    return (trimmedStr === '');
}

function createCollection(){
    sendCollectionData();
}
