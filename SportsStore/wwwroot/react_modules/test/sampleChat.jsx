class MessageChat extends React.Component {
    render() {
        return <li>{this.props.text}</li>;
    }
}

class SampleChat extends React.Component {
    constructor(props) {
        super(props);
        this.state = { messages: [], currentMessage: '', hubConnection: null };
        this.onHandleSendMessage = this.onHandleSendMessage.bind(this);
        this.onChange = this.onChange.bind(this);
    }
    componentDidMount = () => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

        this.setState({ hubConnection }, () => {
            this.state.hubConnection.start();

            this.state.hubConnection.on('Send', (data) => {
                let messages = this.state.messages;
                messages.push(data);
                this.setState({ messages: messages });
            });
        });
    }

    onChange(e) {
        var val = e.target.value;
        this.setState({ currentMessage: val });
    }

    onHandleSendMessage(e) {
        e.preventDefault();
        let message = this.state.currentMessage;
        this.setState({ currentMessage:""});
        this.state.hubConnection.invoke("Send", message);
    }

    render() {
        return (
            <div>
                <div id="inputForm">
                    <input type="text" id="message" value={this.state.currentMessage} onChange={this.onChange} />
                    <input type="button" id="sendBtn" value="Отправить" onClick={this.onHandleSendMessage} />
                </div>
                <div id="chatroom">
                    {
                        this.state.messages.map(function (item) { return <MessageChat text={item}/> })
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