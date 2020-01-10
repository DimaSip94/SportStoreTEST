class Hello extends React.Component {
    constructor(props) {
        super(props);
        this.state = { class: "off", label: "Нажми" };
        this.press = this.press.bind(this);
    }
    press() {
        let className = (this.state.class === "off") ? "on" : "off";
        let btnText = (this.state.class === "off") ? "Отожми" : "Нажми";
        this.setState({ class: className, label: btnText });
    }
    render() {
        return <button onClick={this.press} className={this.state.class}>{this.state.label}</button>;
    }
    //render() {
    //    return <h1>Привет, React.JS, {this.props.myname} {this.state.welcome}</h1>
    //}
}
Hello.defaultProps = { myname: "Иван"};
ReactDOM.render(
    <Hello />,
    document.getElementById("app")
);