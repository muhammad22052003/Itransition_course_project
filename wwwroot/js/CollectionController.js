document.addEventListener('DOMContentLoaded', function () {
    console.log("listController");
});

async function exportToCsv() {

    const collectionId = document.getElementById('collectionId');

    const url = serverUrl + '/collection/exporttocsv?id=' + collectionId.value;

    try {
        const response = await fetch(url, {
            method: "GET",
            credentials: "include"
        })

        if (response.ok) {
            const fileBlob = await response.blob();
            const fileName = 'exported_file.csv'; // Замените на имя файла, которое вы хотите сохранить

            // Создайте ссылку для скачивания файла
            const downloadLink = document.createElement('a');
            downloadLink.href = URL.createObjectURL(fileBlob);
            downloadLink.download = fileName;
            downloadLink.click();
        }
    } catch (e) {
        console.log(e);
    }
}
