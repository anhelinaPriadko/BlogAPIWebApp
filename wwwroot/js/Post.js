// Post.js
const postsUri = '/api/Posts';
const usersUri = '/api/BlogUsers';
const tagsUri = '/api/Tags';
let posts = [];
let users = [];
let tags = [];

// Завантажити авторів для селектів
async function loadUsers() {
    const res = await fetch(usersUri);
    users = await res.json();
    const addSelect = document.getElementById('add-author');
    const editSelect = document.getElementById('edit-author');
    [addSelect, editSelect].forEach(sel => {
        sel.innerHTML = '';
        users.forEach(u => {
            const opt = document.createElement('option');
            opt.value = u.id;
            opt.textContent = u.name;
            sel.appendChild(opt);
        });
    });
}

// Завантажити теги
async function loadTags() {
    const res = await fetch(tagsUri);
    tags = await res.json();
    const addSelect = document.getElementById('add-tags');
    const editSelect = document.getElementById('edit-tags');
    [addSelect, editSelect].forEach(sel => {
        sel.innerHTML = '';
        tags.forEach(t => {
            const opt = document.createElement('option');
            opt.value = t.id;
            opt.textContent = t.name;
            sel.appendChild(opt);
        });
    });
}

// Ініціалізація: спочатку завантажуємо користувачів, теги, потім пости
document.addEventListener('DOMContentLoaded', async () => {
    await loadUsers();
    await loadTags();
    await getPosts();
});

// CRUD Posts
async function getPosts() {
    const res = await fetch(postsUri);
    posts = await res.json();
    _displayPosts(posts);
}

function getSelectedValues(selectElement) {
    return Array.from(selectElement.selectedOptions).map(opt => parseInt(opt.value, 10));
}

async function addPost() {
    const rawDate = document.getElementById('add-datetime').value;
    const postDateTime = rawDate.includes(':') ? `${rawDate}:00` : rawDate;

    const post = {
        authorId: parseInt(document.getElementById('add-author').value, 10),
        postText: document.getElementById('add-text').value.trim(),
        postDateTime: postDateTime,
        tagIds: getSelectedValues(document.getElementById('add-tags'))
    };
    await fetch(postsUri, {
        method: 'POST',
        headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
        body: JSON.stringify(post)
    });
    document.getElementById('add-post-form').reset();
    await getPosts();
}

async function deletePost(id) {
    await fetch(`${postsUri}/${id}`, { method: 'DELETE' });
    await getPosts();
}

function displayEditForm(id) {
    const post = posts.find(p => p.id === id);
    document.getElementById('edit-id').value = post.id;
    document.getElementById('edit-author').value = post.authorId;
    document.getElementById('edit-text').value = post.postText;
    document.getElementById('edit-datetime').value = post.postDateTime.substring(0, 16);
    const tagSelect = document.getElementById('edit-tags');
    Array.from(tagSelect.options).forEach(opt => {
        opt.selected = post.tagIds && post.tagIds.includes(parseInt(opt.value, 10));
    });
    document.getElementById('editForm').style.display = 'block';
}

async function updatePost() {
    const id = document.getElementById('edit-id').value;
    const rawDate = document.getElementById('edit-datetime').value;
    const postDateTime = rawDate.includes(':') ? `${rawDate}:00` : rawDate;

    const updated = {
        id: parseInt(id, 10),
        authorId: parseInt(document.getElementById('edit-author').value, 10),
        postText: document.getElementById('edit-text').value.trim(),
        postDateTime: postDateTime,
        tagIds: getSelectedValues(document.getElementById('edit-tags'))
    };
    await fetch(`${postsUri}/${id}`, {
        method: 'PUT',
        headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
        body: JSON.stringify(updated)
    });
    closeInput();
    await getPosts();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayPosts(data) {
    const container = document.getElementById('posts');
    container.innerHTML = '';
    console.log('Loaded posts:', data);
    console.log('Loaded users:', users);
    data.forEach(post => {
        const authorDisplay = post.authorName || (post.author && post.author.name) || getUserName(post.authorId);
        const col = document.createElement('div');
        col.className = 'col-md-6 mb-4';
        col.innerHTML = `
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">${authorDisplay}</h5>
          <h6 class="card-subtitle mb-2 text-muted">${new Date(post.postDateTime).toLocaleString()}</h6>
          <p class="card-text">${post.postText}</p>
          ${post.tags ? `<div class="mb-2">${post.tags.map(t => `<span class='badge bg-secondary me-1'>${t.name}</span>`).join(' ')}</div>` : ''}
          <div class="d-flex justify-content-end">
            <button class="btn btn-sm btn-primary mr-2" onclick="displayEditForm(${post.id})">Edit</button>
            <button class="btn btn-sm btn-danger" onclick="deletePost(${post.id})">Delete</button>
          </div>
        </div>
      </div>`;
        container.appendChild(col);
    });
}

function getUserName(id) {
    const u = users.find(x => x.id === id);
    return u ? u.name : 'Unknown';
}
