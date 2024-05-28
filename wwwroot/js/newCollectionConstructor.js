let stringColumnsLimit = 3;
let textColumnsLimit = 3;
let intColumnsLimit = 3;
let boolColumnsLimit = 3;
let dateColumnsLimit = 3;

let columnsCount = 0;
let maxColumnsLimit = 15;

function addColumn() {
    if (columnsCount > maxColumnsLimit) {
        return;
    }

    const columnTemplate = document.getElementById('columnTemplate');
    let columnsBlock = document.getElementById('columns-block');
    let newColumn = columnTemplate.cloneNode(true);
    newColumn.id = 'column-data';

    let typeText = document.getElementById('new-column-type-select').value
    defineColumnLimitAdding(typeText);
    newColumn.querySelector('#type-text').innerText = typeText;

    let columnEraserButton = newColumn.querySelector('#erase-column-button');
    columnEraserButton.onclick = deleteHandler;

    // adding new column to columns block
    columnsBlock.appendChild(newColumn);
    if (columnsCount == maxColumnsLimit) {
        let addColumnSelect = document.getElementById('add-new-column-block');
        addColumnSelect.hidden = true;
    }

    defineAvailableTypesOption();
}

function defineAvailableTypesOption() {
    const typesSum = stringColumnsLimit + textColumnsLimit +
        intColumnsLimit + boolColumnsLimit +
        dateColumnsLimit
    if (typesSum == 0)
        return null;

    let typesBlock = document.createElement('div');

    if (stringColumnsLimit > 0)
        typesBlock.appendChild(getTypeOptionElement('String', 'String'));

    if (textColumnsLimit > 0)
        typesBlock.appendChild(getTypeOptionElement('Text', 'Text'));

    if (intColumnsLimit > 0)
        typesBlock.appendChild(getTypeOptionElement('Number', 'Number'));

    if (boolColumnsLimit > 0)
        typesBlock.appendChild(getTypeOptionElement('Yes/No', 'Yes/No'));

    if (dateColumnsLimit > 0)
        typesBlock.appendChild(getTypeOptionElement('Date', 'Date'));

    let newColumnTypesSelect = document.getElementById('new-column-type-select');

    newColumnTypesSelect.innerHTML = '';
    newColumnTypesSelect.innerHTML = typesBlock.innerHTML;
}

function getTypeOptionElement(value, text) {
    let element = document.createElement('option');

    element.value = value;
    element.innerText = text;

    return element;
}

let newColumnSelectElement = document.getElementById('new-column-type-select');

newColumnSelectElement.addEventListener('change', function (event) {
});

function defineColumnLimitAdding(typeText) {
    typeText = typeText.toLocaleLowerCase()

    if (typeText === 'string') {
        stringColumnsLimit--;
        columnsCount++;
    }
    if (typeText === 'text') {
        textColumnsLimit--;
        columnsCount++;
    }
    if (typeText === 'number') {
        intColumnsLimit--;
        columnsCount++;
    }
    if (typeText === 'yes/no') {
        boolColumnsLimit--;
        columnsCount++;
    }
    if (typeText === 'date') {
        dateColumnsLimit--;
        columnsCount++;
    }
}

function defineColumnLimitDeleting(typeText) {
    typeText = typeText.toLocaleLowerCase()

    if (typeText === 'string') {
        stringColumnsLimit++;
        columnsCount--;
    }
    if (typeText === 'text') {
        textColumnsLimit++;
        columnsCount--;
    }
    if (typeText === 'number') {
        intColumnsLimit++;
        columnsCount--;
    }
    if (typeText === 'yes/no') {
        boolColumnsLimit++;
        columnsCount--;
    }
    if (typeText === 'date') {
        dateColumnsLimit++;
        columnsCount--;
    }
}

document.addEventListener('DOMContentLoaded', async function () {
    let allColumns = await document.querySelectorAll('#column-data');
    let allEraseButtons = await document.querySelectorAll('#erase-column-button');

    for (let i = 0; i < allEraseButtons.length; i++) {
        allEraseButtons[i].onclick = deleteHandler;
    }

    for (let i = 0; i < allColumns.length; i++) {
        let typeText = allColumns[i].querySelector('#type-text')
            .innerText.toLocaleLowerCase();

        defineColumnLimitAdding(typeText);
    }

    if (columnsCount >= maxColumnsLimit) {
        let addColumnSelect = document.getElementById('add-new-column-block');
        addColumnSelect.hidden = true;
    }

    defineAvailableTypesOption();
});

let submitButton = document.getElementById('submit-button');
submitButton.addEventListener("click", async function (event) {
    let allColumns = await document.querySelectorAll('#column-data');

    let stringCount = 1;
    let textCount = 1;
    let intCount = 1;
    let boolCount = 1;
    let dateCount = 1;

    let collectionName = document.getElementById('collectionName');

    let collectionCategory = document.getElementById('collectionCategory');

    if (isEmpty(collectionName.value)) {
        alert('You need to assign collection name');
        return;
    }

    if (collectionCategory != null) {
        if (isEmpty(collectionCategory.value)) {
            alert('You need to assign collection category');
            return;
        }
    }

    for (let i = 0; i < allColumns.length; i++) {

        let typeText = allColumns[i].querySelector('#type-text')
            .innerText.toLocaleLowerCase();

        let columnNameElement = allColumns[i].querySelector('#column-name');

        if (isEmpty(columnNameElement.value)) {
            alert('You need to assign a name to the column');
            return;
        }

        if (typeText === 'string') {
            columnNameElement.name = 'CustomString' + stringCount + '_name';
            stringCount++;
        }

        if (typeText === 'text') {
            columnNameElement.name = 'CustomText' + textCount + '_name';
            textCount++;
        }

        if (typeText === 'number') {
            columnNameElement.name = 'CustomInt' + intCount + '_name';
            intCount++;
        }

        if (typeText === 'yes/no') {
            columnNameElement.name = 'CustomBool' + boolCount + '_name';
            boolCount++;
        }

        if (typeText === 'date') {
            columnNameElement.name = 'CustomDate' + dateCount + '_name';
            dateCount++;
        }
    }

    let formData = document.getElementById('data-form');

    formData.submit();
});

function isEmpty(text) {
    return text.trim() === '';
}

async function deleteHandler(event) {

    let columnElement = event.currentTarget.parentElement
        .parentElement.parentElement
        .parentElement;

    let typeText = columnElement.querySelector('#type-text').innerText;

    defineColumnLimitDeleting(typeText);

    columnElement.outerHTML = '';

    defineAvailableTypesOption();

    if (columnsCount < maxColumnsLimit) {
        let addColumnSelect = document.getElementById('add-new-column-block');
        addColumnSelect.hidden = false;
    }
}
