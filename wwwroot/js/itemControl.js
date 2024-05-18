const serverUrl = window.location.origin;
let lastLoadTime = new Date();

document.addEventListener('DOMContentLoaded', function () {

    let new_commentElement = document.getElementById('new-comment');

    new Promise((async function (resolve) {

        while (true) {

            try {
                await defineComentariesBlock();
            } catch (e) {
                console.log(e);
            }

            await sleep(10000);
        }
    }));
});


async function sendComment() {

    const commentContent = document.getElementById('comment-area').value;
    const itemId = document.getElementById('item-id').value;

    const url = serverUrl + '/api/addcomment';

    let bodyData = {
        ItemId: itemId,
        CommentText: commentContent
    }

    console.log(bodyData);

    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify(bodyData),
        });

        if (response.ok) {
            console.log('Comment added');
            document.getElementById('comment-area').value = '';
        }
    } catch (e) {
        console.log(e)
    }
}

async function defineComentariesBlock() {
    let commentariesBlock = document.getElementById('comentaries-block');
    let commentTemplate = document.getElementById('comment-template');

    let comentaries = await getNewComentaries();

    if (comentaries != null) {
        for (var i = 0; i < comentaries.length; i++) {
            let newCommentary = commentTemplate.cloneNode(true);
            newCommentary.hidden = false;

            newCommentary.querySelector('#author-field').textContent = comentaries[i].User;
            newCommentary.querySelector('#comment-text-field').textContent = comentaries[i].Text;
            newCommentary.querySelector('#date-field').textContent = comentaries[i].Date;

            commentariesBlock.appendChild(newCommentary);
        }
    }
    
}

async function getNewComentaries() {
    const itemId = document.getElementById('item-id').value;

    const url = serverUrl + '/api/getitemcomments';

    try {

        let bodyData = {
            ItemId: itemId,
            FromTime: lastLoadTime
        }

        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify(bodyData),
        });

        lastLoadTime = new Date();

        if (response.ok) {
            return response.json()
        }
        else {
            return null;
        }
    } catch (e) {
        console.log(e);
        return null;
    }
    
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}