import React, { Component, Fragment } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';

class ErrorResolvedContent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            remark: props.data.resolvedRemark
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
        return [<Row key={data.notificationId}>
            <Cell>{this.getHeadlines(data.headlines)}</Cell>
            <Cell>{this.getRecipients(data.recipients)}</Cell>
            <Cell>{data.reason}</Cell>
        </Row>]
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

    resendNotification = notificationId => {
        this.props.resendNotification({
            notificationId,
            remark: this.state.remark
        }, true)
    }

    handleChange = e => {
        this.setState({ [e.target.name]: e.target.value});
    }

    onClose = close => {
        this.props.setErrorResolvedContent({
            resolvedBy: '',
            result: {
                code: 1,
                message: '',
                isSuccessful: false
            }
        })
        
        close();
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
                    <div className='error-occured-message error-resolved-row'>{LangHelper.getResource('ErrorOccuredWhileSendingTheNotificationToTheFollowingRecipient')}</div>
                    <Table
                        minWidth={500}
                        maxWidth={772}
                        header={this.buildHeader()}
                        body={this.buildBody(data)}
                        containerPadding={0}
                        adjustedHeight={50}
                    />
                    {
                        this.props.errorResolvedContent.result.isSuccessful ? <Fragment>
                            <div className='error-resolved-row'>{LangHelper.getResource('Remark') + ': ' + LangHelper.getResource('ResendDone')}</div>
                            <div>{LangHelper.getResource('ResolvedBy') + ': ' + this.props.errorResolvedContent.resolvedBy}</div>
                        </Fragment> : <Fragment>
                            <div className='error-resolved-row'>{LangHelper.getResource('Remark') + ':'}</div>
                            <textarea className='full-width' rows='3' onChange={this.handleChange} name='remark' value={this.state.remark}></textarea>
                        </Fragment>
                    }
                </Modal.Body>
                <Modal.Footer>
                    {
                        !this.props.errorResolvedContent.result.isSuccessful && <Button onClick={() => this.resendNotification(data.notificationId)} variant='primary' size='sm'>{LangHelper.getResource('Resolved')}</Button>
                    }
                    <Button onClick={() => this.onClose(onClose)} variant='primary' size='sm'>{LangHelper.getResource('Close')}</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        errorResolvedContent,
        resendNotification,
        setErrorResolvedContent
    } = state;

    return {
        errorResolvedContent,
        resendNotification,
        setErrorResolvedContent
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ErrorResolvedContent);