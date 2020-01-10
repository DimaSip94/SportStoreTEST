class UserForm extends React.Component {
    constructor(props) {
        super(props);
        var name = props.name;
        var nameIsValid = this.validateName(name);
        var age = props.age;
        var ageIsValid = this.validateAge(age);
        this.state = { name: name, age: age, nameValid: nameIsValid, ageValid: ageIsValid }; 

        this.onNameChange = this.onNameChange.bind(this);
        this.onAgeChange = this.onAgeChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    validateName(name) {
        return name.length > 2;
    }
    validateAge(age) {
        return age > 0;
    }

    onAgeChange(e) {
        var val = e.target.value;
        var valid = this.validateAge(val);
        this.setState({ age: val, ageValid: valid });
    }
    onNameChange(e) {
        var val = e.target.value;
        var valid = this.validateName(val);
        this.setState({ name: val, nameValid: valid });
    }

    handleSubmit(e) {
        e.preventDefault();
        if (this.state.nameValid === true && this.state.ageValid === true) {
            const data = new FormData();
            data.append("name", this.state.name);
            data.append("age", this.state.age);
            var xhr = new XMLHttpRequest();
            xhr.open("post", "/React_Product/TestPost", true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    console.log(JSON.parse(xhr.response).message);
                }
            }.bind(this);
            xhr.send(data);
        }
    }

    render() {
        var nameColor = this.state.nameValid === true ? "green" : "red";
        var ageColor = this.state.ageValid === true ? "green" : "red";
        return (
            <form onSubmit={this.handleSubmit}>
                <p>
                    <label>Имя:</label><br />
                    <input type="text" value={this.state.name}
                        onChange={this.onNameChange} style={{ borderColor: nameColor }} />
                </p>
                <p>
                    <label>Возраст:</label><br />
                    <input type="number" value={this.state.age}
                        onChange={this.onAgeChange} style={{ borderColor: ageColor }} />
                </p>
                <input type="submit" value="Отправить" />
            </form>  
        );
    }
}

ReactDOM.render(
    <UserForm name="" age={0} apiUrl=""/>,
    document.getElementById("app")
);