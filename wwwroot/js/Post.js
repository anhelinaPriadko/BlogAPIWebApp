const postsUri = '/api/Posts';
const usersUri = '/api/BlogUsers';
const tagsUri = '/api/Tags';

let posts = [];
let users = [];
let tags = [];


async function loadUsers() {
    const res = await fetch(usersUri);
    users = await res.json();
    ['add-author', 'edit-author'].forEach(id => {
        const sel = document.getElementById(id);
        sel.innerHTML = '';
        users.forEach(u => {
            const opt = document.createElement('option');
            opt.value = u.id;
            opt.textContent = u.name;
            sel.appendChild(opt);
        });
    });
}

async function loadTags() {
    const res = await fetch(tagsUri);
    tags = await res.json();

    const addContainer = document.getElementById('add-tags-container');
    const editContainer = document.getElementById('edit-tags-container');
    [addContainer, editContainer].forEach(container => {
        container.innerHTML = '';
        tags.forEach(t => {
            const lbl = document.createElement('label');
            const cb = document.createElement('input');
            cb.type = 'checkbox';
            cb.value = t.id;
            cb.name = container.id; 
            lbl.appendChild(cb);
            lbl.appendChild(document.createTextNode(t.tagText));
            container.appendChild(lbl);
        });
    });
}

function getSelectedValuesFrom(containerId) {
    const container = document.getElementById(containerId);

    return Array.from(container.querySelectorAll('input[type=checkbox]'))
        .filter(cb => cb.checked)
        .map(cb => parseInt(cb.value, 10));
}


function getSelectedValues(selectElement) {
    return Array.from(selectElement.selectedOptions)
        .map(opt => parseInt(opt.value, 10));
}


async function getPosts() {
    const res = await fetch(postsUri);
    posts = await res.json();
    displayPosts(posts);
}


async function addPost() {
    const post = {
        authorId: parseInt(document.getElementById('add-author').value, 10),
        postText: document.getElementById('add-text').value.trim(),
        postDateTime: document.getElementById('add-datetime').value,
        tagIds: getSelectedValuesFrom('add-tags-container')
    };

    await fetch(postsUri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(post)
    });

    document.getElementById('add-form').reset();
    hideAddForm();
    await getPosts();
}


async function deletePost(id) {
    if (!confirm('Ви справді хочете видалити цей пост?')) return;
    await fetch(`${postsUri}/${id}`, { method: 'DELETE' });
    await getPosts();
}
function displayEditForm(id) {
    const post = posts.find(p => p.id === id);


    document.getElementById('edit-id').value = post.id;


    document.getElementById('edit-author').value = post.authorId;


    document.getElementById('edit-text').value = post.postText;

    document.getElementById('edit-datetime').value =
        toLocalDatetimeInputValue(post.postDateTime);


    document
        .getElementById('edit-tags-container')
        .querySelectorAll('input[type=checkbox]')
        .forEach(cb => cb.checked = false);

    if (post.tags) {
        post.tags.forEach(t => {
            const cb = document
                .getElementById('edit-tags-container')
                .querySelector(`input[value="${t.id}"]`);
            if (cb) cb.checked = true;
        });
    }

    document.getElementById('editForm').style.display = 'block';
}


function toLocalDatetimeInputValue(isoString) {
    const d = new Date(isoString);        // парсимо ISO з сервера
    const pad = n => String(n).padStart(2, '0');

    const year = d.getFullYear();
    const month = pad(d.getMonth() + 1); // місяці від 0 до 11
    const day = pad(d.getDate());
    const hours = pad(d.getHours());
    const mins = pad(d.getMinutes());

    return `${year}-${month}-${day}T${hours}:${mins}`;
}


async function updatePost() {
    const id = parseInt(document.getElementById('edit-id').value, 10);
    const updated = {
        id: id,
        authorId: parseInt(document.getElementById('edit-author').value, 10),
        postText: document.getElementById('edit-text').value.trim(),
        postDateTime: document.getElementById('edit-datetime').value,
        tagIds: getSelectedValuesFrom('edit-tags-container')
    };

    await fetch(`${postsUri}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updated)
    });

    closeInput();
    await getPosts();
}

function displayPosts(data) {
    const tbody = document.getElementById('postsTableBody');
    tbody.innerHTML = '';

    data.forEach(post => {
        const authorName = users.find(u => u.id === post.authorId)?.name ?? 'Unknown';
        const dateStr = new Date(post.postDateTime).toLocaleString();
        const tagsHtml = post.tags && post.tags.length
            ? post.tags.map(t => `<span class="badge">${t.tagText}</span>`).join(' ')
            : '';

        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${authorName}</td>
            <td>${post.postText}</td>
            <td>${dateStr}</td>
            <td>${tagsHtml}</td>
            <td>
                <button onclick="displayEditForm(${post.id})">Edit</button>
                <button onclick="deletePost(${post.id})">Delete</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}
 

document.addEventListener('DOMContentLoaded', async () => {
    await loadUsers();
    await loadTags();
    await getPosts();

    document.getElementById('btnShowAdd').onclick = showAddForm;
});
