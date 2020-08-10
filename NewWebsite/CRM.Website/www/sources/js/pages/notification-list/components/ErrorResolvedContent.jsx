import React, { Component } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';

export default class ErrorResolvedContent extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
    }

    render() {
        const { data, onClose } = this.props;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered onHide={onClose} >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{LangHelper.getResource('ErrorNotification')}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>{`${LangHelper.getResource('NotificationID')}: ${data.notificationId}`}</div>
                    <div>{`${LangHelper.getResource('NotificationTemplate')}: ${data.title}`}</div>
                    <div>{`${LangHelper.getResource('SentAt')}: ${data.sendDateString} ${data.sendTimeString}`}</div>
                    <div className='notification-content'>{data.content}</div>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={onClose} variant='primary' size='sm'>{LangHelper.getResource('Close')}</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}