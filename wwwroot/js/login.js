const url = '/User';

const login = () => {
    const Name = document.getElementById('loginName');
    const Password = document.getElementById('loginPassword');
    const user ={
        Name: Name.value.trim(),
        Password: Password.value.trim()
    }
    debugger
    fetch(`${url}/Login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then((data) => {
            alert(data);
            localStorage.setItem("token", data);
            location.href = "/indexTask.html";
            Name.value = '';
            Password.value = '';
        })
        .catch((error) => {
            Name.value = '';
            Password.value = '';
            console.error(alert("user is not found"));
        });
}