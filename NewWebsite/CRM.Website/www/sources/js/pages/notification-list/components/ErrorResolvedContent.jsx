import React, { Component } from 'react';
import { Button, Modal, InputGroup } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';

export default class ErrorResolvedContent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            remark: ''
        }
    }

    buildHeader = () => {
        return (
            <HeaderRow>
                <HeaderCell colWidth={25}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
                <HeaderCell colWidth={25}>{LangHelper.getResource('Recipient')}</HeaderCell>
                <HeaderCell colWidth={50}>{LangHelper.getResource('Reason')}</HeaderCell>
            </HeaderRow>
        );
    }

    buildBody = data => {
        return <Row key={data.notificationId}>
            <Cell>{this.getHeadlines(data.headlines)}</Cell>
            <Cell>{this.getRecipients(data.recipients)}</Cell>
            <Cell>{data.reason}</Cell>
        </Row>
    }

    getHeadlines = headlines => {
        if (headlines && headlines.length > 0) {
            return headlines.map(headline => headline.headlineName).join(', ')
        }
        
        return '';
    }

    getRecipients = recipients => {
        if (recipients && recipients.length > 0) {
            return recipients.map((recipient, index) => <div key={index}>{`${recipient.recipientName} (${recipient.roleName})`}</div>)
        }
        
        return '';
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
                    <div className='error-occured-message'>{LangHelper.getResource('ErrorOccuredWhileSendingTheNotificationToTheFollowingRecipient')}</div>
                    <Table
                        minWidth={500}
                        maxWidth={772}
                        header={this.buildHeader()}
                        body={this.buildBody(data)}
                        containerPadding={0}
                        adjustedHeight={50}
                    />
                    <div>{LangHelper.getResource('Remark') + ':'}</div>
                    <textarea>{this.state.remark}</textarea>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={onClose} variant='primary' size='sm'>{LangHelper.getResource('Resolved')}</Button>
                    <Button onClick={onClose} variant='primary' size='sm'>{LangHelper.getResource('Close')}</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}