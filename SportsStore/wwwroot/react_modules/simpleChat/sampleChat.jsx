class MessageChat extends React.Component {
    render() {
        return <li>{this.props.author}: {this.props.text}</li>;
    }
}

class SampleChat extends React.Component {
    constructor(props) {
        super(props);
        this.state = { messages: [], currentMessage: '', hubConnection: null, userName: "", showLogin: "block", showChat:"hide" };
        this.onHandleSendMessage = this.onHandleSendMessage.bind(this);
        this.onChange = this.onChange.bind(this);
        this.loginEvent = this.loginEvent.bind(this);
        this.onChangeLogin = this.onChangeLogin.bind(this);
    }
    componentDidMount = () => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

        this.setState({ hubConnection }, () => {
            this.state.hubConnection.start();

            this.state.hubConnection.on('Send', (data, userName) => {
                let messages = this.state.messages;
                messages.push({ id: messages.length+1, text: data, userName: userName });
                this.setState({ messages: messages });
            });
        });
    }

    onChangeLogin(e) {
        let userName = e.target.value;
        this.setState({ userName: userName });
    }

    loginEvent(e) {
        this.setState({ showLogin: "hide", showChat: "block" })
    }

    onChange(e) {
        var val = e.target.value;
        this.setState({ currentMessage: val });
    }

    onHandleSendMessage(e) {
        e.preventDefault();
        let message = this.state.currentMessage;
        let userName = this.state.userName;
        this.setState({ currentMessage:""});
        this.state.hubConnection.invoke("Send", message, userName);
    }

    render() {
        return (
            <div>
                <div>
                    <div id="loginBlock" className={this.state.showLogin}>
                        Введите логин:<br />
                        <input id="userName" type="text" value={this.state.userName} onChange={this.onChangeLogin} />
                        <input id="loginBtn" type="button" value="Войти" onClick={this.loginEvent} />
                    </div> <br />
                    <div id="header">{this.state.userName}</div> <br />
                </div>
                <div id="inputForm" className={this.state.showChat}>
                    <input type="text" id="message" value={this.state.currentMessage} onChange={this.onChange} />
                    <input type="button" id="sendBtn" value="Отправить" onClick={this.onHandleSendMessage} />
                </div>
                <div id="chatroom">
                    {   
                        this.state.messages.map(function (item) {
                            return <MessageChat key={item.id} text={item.text} author={item.userName} />
                        })
                    }
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <SampleChat/>,
    document.getElementById("chatreact")
);