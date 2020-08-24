import React, { Component } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';

export default class Confirm extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        const { onOk, onClose } = this.props;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered onHide={onClose} >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{LangHelper.getResource('Confirmation')}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>{LangHelper.getResource('ThereAreSomeChangesConfirmMessage')}</div>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={onOk} variant='primary' size='sm'>{LangHelper.getResource('OK')}</Button>
                    <Button onClick={onClose} variant='primary' size='sm'>{LangHelper.getResource('Cancel')}</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}