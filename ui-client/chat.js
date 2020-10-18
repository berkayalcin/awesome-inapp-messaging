// WebSocket bağlantısı kur



$(document).ready(function () {

    if (!localStorage.getItem('userName')) {
        var userName = null;
        while (userName === null) {
            userName = prompt('Kullanıcı Adınızı Girin', null);

        }
        localStorage.setItem('userName', userName);
    }
    $('#greetings').text('Selam '+localStorage.getItem('userName'))

    const socket = new WebSocket('wss://localhost:5001/in-app-chat?username=' + localStorage.getItem('userName'));

    // Bağlantı kurulduğunda

    socket.addEventListener('open', function (event) {

    });

    // Mesaj alındığında
    socket.addEventListener('message', function (event) {
        var message = JSON.parse(event.data);
        console.log(message);
        addMessage(message.Body, message.Sender);
    });

    // Bağlantı Kapatıldığında
    socket.onclose = function (event) {
        console.log("WebSocket is closed now.");
    };

    window.socket = socket;
});

var addMessage = function addMessage(message, sender) {
    var template = `
    <div class="col-12 mb-2">
    <div class="card" style="width: 100%;border-radius: 15px;">
      <div class="card-body">
        <h6 class="card-subtitle mb-2 text-muted">${sender} - ${new Date().toLocaleDateString()}</h6>
        <p class="card-text">${message}</p>
        </div>
        </div>
    </div>
    `;
    $('#messages').append(template);
}



var send = function sendMessage() {
    var msg = $('[name=message]').val();
    if (!msg) {
        alert('Lütfen bir mesaj girin');
        return;
    }
    window.socket.send(msg);
    addMessage(msg,localStorage.getItem('userName'))
}




