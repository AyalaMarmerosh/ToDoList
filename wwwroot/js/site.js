const url = '/User';
let token = localStorage.getItem("token");
const btnAdmin = document.querySelector('#admin');

// function getUsers() {
//     fetch(`${url}/Get`)
//         .then(response => response.json())
//         .then(data => _displayUsers(data))
//         .catch(_error => console.log(alert('you dont have authorize')));
// }

function getUsers() {
    console.log("in getalluser");
    fetch(url, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            }
        })
        .then(response => response.json())
        .then(data => {
            _displayUsers(data);
        })
        .catch(_error => alert('you dont have authorize'));
}


function _displayCountUser(userCount) {
    const name = (userCount === 1) ? 'user' : (userCount === 0) ? 'no users' : 'users';
    document.getElementById('countUser').innerText = `${userCount} ${name}`;
}

function _displayUsers(data) {
    document.getElementById('manager').style.display = 'block';
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';
    _displayCountUser(data.length);
    data.forEach(user => {
        const button = document.createElement('button');

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = '❌';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(user.id);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(user.name);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(user.password);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });
}


function deleteUser(id) {
    fetch(`${url}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
    })
        .then(() => getUsers())
        .catch(error => console.log('Unable to delete user.', error));
}

btnAdmin.onclick = () => {
    btnAdmin.style.display = 'none';
    getUsers();
}


function addUser() {
    const addNameTextbox = document.getElementById('add-user-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const user = {
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim(),
    };
    fetch(`${url}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            addNameTextbox.value = '';
            addPasswordTextbox.value = '';
        })
        .catch(error => console.log('Unable to add item.', error));
}
//-------------------Task--------------------

const uri = '/Task';
let tasks = [];


// function getItems() {
//     getUserName();
    
//     fetch(uri, {
//         method: 'GET',
//         headers: {
//             'Accept': 'application/json',
//             'Content-Type': 'application/json',
//             'Authorization': 'Bearer ' + token
//         }
//     })
//         .then(response => response.json())
//         .then(data => _displayItems(data))
//         .catch(error => alert('Unable to get items.', error));
// }
function getItems() {
    // var myHeaders = new Headers();
    // myHeaders.append("Authorization", `Bearer ${token}`);
    // myHeaders.append("Content-Type", "application/json");
  
    // var requestOptions = {
    //   method: "GET",
    //   headers: {
    //     'Accept': 'application/json',
    //     'Content-Type': 'application/json',
    //     'Authorization': 'Bearer'+ token
    //   }
    // };

    console.log('the token is'+ token);
    fetch(uri, {
        method: "GET",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    })
      .then((response) => response.json())
      .then((data) => _displayItems(data))
      .catch((error) => alert("Unable to get items... \n" + error));
  }


function addItem() {

    const addNameTextbox = document.getElementById('add-name');
    const item = {
        perform: false,
        Name: addNameTextbox.value.trim(),
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
    })
        .then(() => getItems())
        .catch(error => console.log('Unable to delete item.', error));
}

// function displayEditForm(id) {

//     const item = tasks.find(item => item.id === id);

//     document.getElementById('edit-name').value = item.name;
//     document.getElementById('edit-id').value = item.id;
//     document.getElementById('edit-perform').checked = item.perform;
//     document.getElementById('save').style.display = 'block';
// }

function displayEditForm(id) {
    const item =tasks.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-perform').checked = item.IsDoing;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        perform: document.getElementById('edit-perform').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount <= 1) ? 'task' : 'task kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById("tasks");
    tBody.innerHTML = "";

    _displayCount(data.length);

    const button = document.createElement("button");

    data.forEach(item => {
        let isDoneCheckbox = document.createElement("input");
        isDoneCheckbox.type = "checkbox";
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.perform;

        let editButton = button.cloneNode(false);
        editButton.innerText = "Edit";
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}

// const deleteTaskIsDone = document.getElementById('deleteTaskIsDone');
// deleteTaskIsDone.onclick = () => {
//     fetch(uri, {
//         method: 'DELETE',
//         headers: {
//             'Accept': 'application/json',
//             'Content-Type': 'application/json',
//             'Authorization': 'Bearer ' + token
//         },
//     })
//         .then(() => {
//             getItems();
//             alert("the task is Doen deleted");
//         })
//         .catch(error => console.log('Unable to delete item.', error));
// }

function getUserName() {
    const name = document.getElementById('userName');
    fetch(`${uri}/action`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify()
    })
        .then(response => response.json())
        .then((data) => {
            name.innerHTML = "Hi, " + data + " wellcome!!";
        })
        .catch(error => console.error('not found name.', error));
}


logout.onclick = () => {
    location.href = "/index.html";
}