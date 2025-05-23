const uri = '/api/Tags';
let tags = [];

document.addEventListener('DOMContentLoaded', () => {
    getTags();
});

document.getElementById('btnShowAdd').addEventListener('click', showAddForm);

function getTags() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayTags(data))
        .catch(error => console.error('Не вдалося отримати теги.', error));
}

function addTag() {
    const tagText = document.getElementById('add-tagtext').value.trim();
    if (!tagText) return;

    fetch(uri, {
        method: 'POST',
        headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
        body: JSON.stringify({ tagText })
    })
        .then(response => response.json())
        .then(() => {
            getTags();
            document.getElementById('add-tag-form').reset();
            hideAddForm();
        })
        .catch(error => console.error('Не вдалося додати тег.', error));
}

function deleteTag(id) {
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(() => getTags())
        .catch(error => console.error('Не вдалося видалити тег.', error));
}

function _displayTags(data) {
    const tableBody = document.getElementById('tagsTableBody');
    tableBody.innerHTML = '';

    if (data.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="2" style="text-align:center;">Немає тегів</td></tr>`;
        return;
    }

    data.forEach(tag => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${tag.tagText}</td>
            <td>
                <button onclick="deleteTag(${tag.id})">Видалити</button>
            </td>
        `;
        tableBody.appendChild(row);
    });

    tags = data;
}

function hideAddForm() {
    document.getElementById('addForm').style.display = 'none';
    document.getElementById('add-user-form').reset();
}

function showAddForm() {
    document.getElementById('addForm').style.display = 'block';
}
