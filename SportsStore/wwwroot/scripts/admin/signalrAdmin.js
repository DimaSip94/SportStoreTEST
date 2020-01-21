var signalrAdmin = {
    init: function () {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

        let connectionId = "";

        hubConnection.on("CreateProductNotify", function (data) {
            console.log(data);
        });

        hubConnection.start().then(() => {
            // после соединения получаем id подключения
            console.log(hubConnection.connectionId);
            connectionId = hubConnection.connectionId;
            document.cookie = "hubConnectionId=" + connectionId;
        });
    }
};