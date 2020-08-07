import React, { Component } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';

export default class Content extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
    }

    render() {
        const { data, onClose } = this.props;

        return (
            <Modal show className='new-notification-template'
                backdrop='static' keyboard={false} size='lg' centered
                onHide={onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{`${LangHelper.getResource('NotificationID')}: ${data.notificationId} (${LangHelper.getResource('MesageID')}: ${data.messageId})`}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>{`Name: Group ${data.groupHeadlines.map(gh => gh.name).join(', ')}`}</div>
                    <div className='notification-content'>{data.content}</div>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={onClose} variant='primary' size='sm'>{LangHelper.getResource('Close')}</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}