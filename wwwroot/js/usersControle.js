async function demote() {
    const url = serverUrl + '/users/demote';

    await sendForm(url);
}

async function promote() {
    const url = serverUrl + '/users/promote';

    await sendForm(url);
}

async function deleteUser() {
    if (!confirm("Will you confirm the deletion of the selected users?")) {
        return;
    }

    const url = serverUrl + '/users/delete';

    await sendForm(url);
}

async function sendForm(url) {
    let form = document.getElementById('users-action-form');
    let formData = new FormData(form);

    try {
        let response = await fetch(url, {
            credentials: "include",
            method: "POST",
            body: formData
        });

        if (response.ok) {
            console.log('Response ok!');
            location.reload();
        }

    } catch (e) {
        console.log(e)
    }
}