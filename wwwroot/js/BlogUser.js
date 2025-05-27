const uri = 'api/BlogUsers';
let users = [];

function getUsers() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

function addUser() {
    const user = {
        userLogin: document.getElementById('add-login').value.trim(),
        name: document.getElementById('add-name').value.trim(),
        birthDate: document.getElementById('add-birthdate').value,
        avatarPath: document.getElementById('add-avatar').value.trim(),
        aboutYourself: document.getElementById('add-about').value.trim(),
        isActive: document.getElementById('add-isactive').checked,
        isOnline: document.getElementById('add-isonline').checked
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(() => {
            getUsers();
            hideAddForm();
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm(id) {
    const user = users.find(u => u.id === id);
    document.getElementById('edit-id').value = user.id;
    document.getElementById('edit-login').value = user.userLogin;
    document.getElementById('edit-name').value = user.name;
    document.getElementById('edit-birthdate').value = user.birthDate.split('T')[0];
    document.getElementById('edit-avatar').value = user.avatarPath;
    document.getElementById('edit-about').value = user.aboutYourself;
    document.getElementById('edit-isactive').checked = user.isActive;
    document.getElementById('edit-isonline').checked = user.isOnline;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const id = document.getElementById('edit-id').value;
    const user = {
        id: parseInt(id, 10),
        userLogin: document.getElementById('edit-login').value.trim(),
        name: document.getElementById('edit-name').value.trim(),
        birthDate: document.getElementById('edit-birthdate').value,
        avatarPath: document.getElementById('edit-avatar').value.trim(),
        aboutYourself: document.getElementById('edit-about').value.trim(),
        isActive: document.getElementById('edit-isactive').checked,
        isOnline: document.getElementById('edit-isonline').checked
    };

    fetch(`${uri}/${id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
        })
        .then(() => {
            getUsers();
            closeInput();
        })
        .catch(error => console.error('Unable to update user.', error));

    return false;
}

function _displayUsers(data) {
    const tbody = document.getElementById('usersTableBody');
    tbody.innerHTML = '';

    data.forEach(user => {
        const tr = document.createElement('tr');

        let td = document.createElement('td');
        td.textContent = user.userLogin;
        tr.appendChild(td);

        td = document.createElement('td');
        td.textContent = user.name;
        tr.appendChild(td);

        td = document.createElement('td');
        td.textContent = user.birthDate.split('T')[0];
        tr.appendChild(td);

        td = document.createElement('td');
        td.textContent = user.avatarPath;
        tr.appendChild(td);

        td = document.createElement('td');
        const chkActive = document.createElement('input');
        chkActive.type = 'checkbox';
        chkActive.disabled = true;
        chkActive.checked = user.isActive;
        td.appendChild(chkActive);
        tr.appendChild(td);

        td = document.createElement('td');
        const chkOnline = document.createElement('input');
        chkOnline.type = 'checkbox';
        chkOnline.disabled = true;
        chkOnline.checked = user.isOnline;
        td.appendChild(chkOnline);
        tr.appendChild(td);

        td = document.createElement('td');
        const btnEdit = document.createElement('button');
        btnEdit.textContent = 'Редагувати';
        btnEdit.onclick = () => displayEditForm(user.id);
        const btnDel = document.createElement('button');
        btnDel.textContent = 'Видалити';
        btnDel.onclick = () => deleteUser(user.id);
        td.appendChild(btnEdit);
        td.appendChild(btnDel);
        tr.appendChild(td);

        tbody.appendChild(tr);
    });

    users = data;
}


document.addEventListener('DOMContentLoaded', getUsers);