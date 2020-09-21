import React, { Component } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

export default class Content extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        const { data, onClose } = this.props;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered
                onHide={onClose}
                dialogAs={DraggableModalDialog}
            >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>
                        {`${LangHelper.getResource('NotificationID')}: ${data.notificationId} (${LangHelper.getResource('MessageID')}: ${data.messageId})`}
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>{`${LangHelper.getResource('LetstalkName')}: ${data.letstalkName}`}</div>
                    <div className='border p-2 mt-2 mb-3 notification-content'>{data.content}</div>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={onClose} variant='primary' size='sm'>{LangHelper.getResource('Close')}</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}