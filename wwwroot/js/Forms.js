document.addEventListener('DOMContentLoaded', function () {
    const btnAdd = document.getElementById('btnShowAdd');
    if (btnAdd) {
        btnAdd.addEventListener('click', showAddForm);
    }
});

function showAddForm() {
    const form = document.getElementById('addForm');
    if (form) {
        form.style.display = 'block';
    }
}

function hideAddForm() {
    const form = document.getElementById('addForm');
    const formElement = document.getElementById('add-form');
    if (form && formElement) {
        form.style.display = 'none';
        formElement.reset();
    }
}

function closeInput() {
    const form = document.getElementById('editForm');
    if (form) {
        form.style.display = 'none';
    }
}
