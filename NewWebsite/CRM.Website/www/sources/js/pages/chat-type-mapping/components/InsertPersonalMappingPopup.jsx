import React, { Component, useRef } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row, Alert } from 'react-bootstrap';
import MultipleSelectField from './MultipleSelectField';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';
import { ChatTypeMapping } from '../../../common/constants';

export class InsertPersonalMappingPopup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {
                botId: '',
                recipientType: '',
                groupHeadlines: {
                    value: '',
                    isValid: false
                },
                recipients: {
                    value: '',
                    isValid: false
                },
                letstalkIds: [],
                languageId: '',
                remark: ''
            },
            isSubmitting: false,
            validate: {
                isShow: false,
                errMsgs: []
            }
        };
    }

    handleChange = (fieldName, fieldValue) => {
        this.setState(prevState => ({
            form: { ...prevState.form, ...{ [fieldName]: fieldValue } }
        }));
    };

    handleGroupHeadlineChange = (data) => {
        this.handleChange('groupHeadlines', data);
        this.props.getRecipientByGroupHeadline(data.value);
    };

    renderOptions = (item, key) => {
        return (
            <option key={item.id} value={item.id}>
                {item.name}
            </option>
        );
    }

    isValidParams = () => {
        const data = this.state.form;
        let isValid = true;
        let errMsg = [];

        if (!data.botId) {
            isValid = false;
            errMsg.push(LangHelper.getResource('BotRequired'));
        }

        if (!data.recipientType) {
            isValid = false;
            errMsg.push(LangHelper.getResource('RecipientTypeRequired'));
        }

        if (!data.groupHeadlines.isValid) {
            isValid = false;
            errMsg.push(LangHelper.getResource('GroupHeadlineRequired'));
        }

        if (!data.recipients.isValid) {
            isValid = false;
            errMsg.push(LangHelper.getResource('RecipientRequired'));
        }

        if (!data.languageId) {
            isValid = false;
            errMsg.push(LangHelper.getResource('LanguageRequired'));
        }

        this.setState({
            isSubmitting: false,
            validate: {
                isShow: !isValid,
                errMsgs: errMsg
            }
        });
        return isValid;
    }

    getParams = () => {
        return {
            botId: this.state.form.botId,
            chatType: ChatTypeMapping.Personal,
            recipientTypeId: this.state.form.recipientType,
            groupHeadlineIds: this.state.form.groupHeadlines.value,
            recipientIds: this.state.form.recipients.value,
            languageId: this.state.form.languageId,
            remark: this.state.form.remark
        };
    }

    handleSubmit = (e) => {
        this.setState({ isSubmitting: true });
        e.preventDefault();
        if (this.isValidParams()) {
            const params = this.getParams();
            this.props.insertPersonalMapping(params).then(() => {
                if (this.props.insertPersonalMappingResult.isSuccessful) {
                    setTimeout(() => {
                        this.props.closeInsertPersonalMappingPopup();
                        this.props.getGridData(this.props.gridParams);
                    }, 3000);
                }
                this.setState({ isSubmitting: false });
            });
        }
    }

    renderForm = () => {
        return (
            <Form onSubmit={this.handleSubmit}>
                <Modal.Body>
                    {
                        this.state.validate.isShow &&
                        <Alert variant="danger">
                            <ul className="list-message-error">
                                {
                                    this.state.validate.errMsgs.map((msg, key) => {
                                        return <li key={key}>{msg}</li>;
                                    })
                                }
                            </ul>
                        </Alert>
                    }
                    {
                        this.props.personalMappingResult &&
                        <ResponseMessage isSuccessful={this.props.insertPersonalMappingResult.isSuccessful}
                            message={this.props.insertPersonalMappingResult.message || LangHelper.getResource('InsertNotificationTemplateSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('BotName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                name='botId'
                                onChange={e => this.handleChange('botId', e.target.value)}
                                value={this.state.form.botId}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.settings.bots.map(this.renderOptions)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3">
                            {LangHelper.getResource('Name')}
                        </Form.Label>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Form.Label column sm="8" md="9">
                            {LangHelper.getResource('Personal')}
                        </Form.Label>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('RecipientType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                name="recipientTypeId"
                                onChange={e => this.handleChange('recipientType', e.target.value)}
                                value={this.state.form.recipientType}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.settings.recipientTypes.map(this.renderOptions)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('GroupHeadline')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelectField
                                width={'100%'}
                                name="headlineIds"
                                initialValue={this.state.form.groupHeadlines.value}
                                data={this.props.settings.groupHeadline}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                onChange={this.handleGroupHeadlineChange}
                                isSearchable

                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('Recipient')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelectField
                                width={'100%'}
                                initialValue={this.state.form.recipients.value}
                                data={this.props.recipients || []}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                onChange={value => this.handleChange('recipients', value)}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('LetstalkID')}
                        </Form.Label>
                        <Col sm="8" md="9" />
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('Language')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                name="languageId"
                                onChange={e => this.handleChange('languageId', e.target.value)}
                                value={this.state.form.languageId}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.settings.languages.map(this.renderOptions)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" >
                            {LangHelper.getResource('Remark')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="textarea" size="sm" className="shadow-none"
                                rows={5}
                                name="remark"
                                onChange={e => this.handleChange('remark', e.target.value)}
                                value={this.state.form.remark}
                            />
                        </Col>
                    </Form.Group>
                </Modal.Body>

                <Modal.Footer>
                    <Button type="submit" variant="primary" size="sm" disabled={this.state.isSubmitting}>
                        {LangHelper.getResource('Submit')}
                    </Button>
                    <Button type="button" variant="primary" size="sm" onClick={this.props.closeInsertPersonalMappingPopup}>
                        {LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Form>
        );
    }

    render() {
        return (
            <Modal show className="new-notification-template"
                backdrop="static" keyboard={false} size="md" centered
                onHide={this.props.closeInsertPersonalMappingPopup}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('InsertPersonalMapping')}</Modal.Title>
                </Modal.Header>
                {
                    this.renderForm()
                }
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { settings, recipients, gridParams, insertPersonalMappingResult } = state;
    return {
        settings,
        recipients,
        gridParams,
        insertPersonalMappingResult
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(InsertPersonalMappingPopup);