import React from 'react';
import { Form } from 'react-bootstrap';

export default class CustomInput extends React.PureComponent {
    constructor(props) {
        super(props);
    }

    render() {
        const { disabled, onClick, value } = this.props;

        return (
            <Form.Control as='input' readOnly disabled={disabled} onClick={onClick} value={value} />
        );
    }
}