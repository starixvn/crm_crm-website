import React, { Component } from 'react'
import ReactDOM from 'react-dom';

export default class App extends Component {
    render() {
        return (
            <h1>notification list</h1>
        );
    }
}

ReactDOM.render(<App />, document.getElementById('app-container'));