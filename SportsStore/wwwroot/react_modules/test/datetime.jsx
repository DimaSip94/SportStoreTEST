class DateTimeEl extends React.Component {
    constructor(props) {
        super(props);
        this.state = { date: new Date(), name: "Tom" };
    }

    componentDidMount() {
        this.timerId = setInterval(
            () => this.tick(),
            1000
        );
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }

    tick() {
        this.setState({
            date: new Date()
        });
    }

    render() {
        return <div>
            <h2>Текущее время {this.state.date.toLocaleTimeString()}.</h2>
        </div>;
    }
}

function tick() {
    ReactDOM.render(
        <DateTimeEl />,
        document.getElementById("dateTimeEl")
    );
}
setInterval(tick, 1000);