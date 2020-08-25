import React, { Component, useRef } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row, Alert } from 'react-bootstrap';
import MultipleSelect from '../../../components/multiple-select/index';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';
import SelectWithSearch from 'react-select';

export class EditPersonalMappingPopup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {
                botId: '',
                name: '',
                recipientType: '',
                groupHeadlines: '',
                recipient: null,
                letstalkIds: '',
                languageId: '',
                remark: ''
            },
            isSubmitting: false,
            letstalkIds: '',
            validate: {
                isShow: false,
                errMsgs: []
            }
        };
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (nextProps.updatePersonalMappingData.data !== null) {
            const initData = nextProps.updatePersonalMappingData.data;
            const recipientTypeId = initData.recipients[0].recipientTypeId;
            const recipientId = initData.recipients[0].recipientId;
            const headlineIds = initData.headlines.map(headline => headline.headlineId).toString();
            const state = {
                form: {
                    botId: initData.botId,
                    name: initData.conversationGroupName,
                    recipientType: recipientTypeId,
                    groupHeadlines: headlineIds,
                    recipient: recipientId,
                    letstalkId: initData.conversationId,
                    languageId: initData.languageId,
                    remark: initData.remark
                }
            };
            return state;
        }

        return null;
    }

    componentDidMount() {
        this.props.getRecipientByRecipientType(this.props.updatePersonalMappingData.data.recipients[0].recipientTypeId);
        this.props.getGroupHeadlineByRecipient(this.props.updatePersonalMappingData.data.recipients[0].recipientId);
    }

    componentDidUpdate() {
    }

    handleChange = (fieldName, fieldValue) => {
        this.setState(prevState => ({
            form: { ...prevState.form, ...{ [fieldName]: fieldValue } }
        }));
    };

    handleGroupHeadlineChange = (currentItem, selectedItemsKey) => {
        this.handleChange('groupHeadlines', selectedItemsKey);
    }

    handleRecipientTypeChange = value => {
        this.handleChange('recipientType', value);
        this.props.getRecipientByRecipientType(value);
    }

    handleRecipientChange = option => {
        this.handleChange('recipient', option);
        this.setState({
            letstalkIds: option.letstalkIds
        });
        this.props.getGroupHeadlineByRecipient(option.value);
    };

    handleLetstalkSelect = (e) => {
        this.handleChange('letstalkId', e.target.value);
    }

    renderOptions = (item, key) => {
        return (
            <option key={item.id} value={item.id}>
                {item.name}
            </option>
        );
    }

    renderLetstalkIds = () => {
        let result = null;
        if (this.state.letstalkIds) {
            const letstalkList = this.state.letstalkIds.split(',');
            result = letstalkList.map((letstalkId, key) => {
                return <Form.Check key={key} inline label={letstalkId} name='letstalkId' type="radio" id={`letstalkId-${key}`} value={letstalkId} onChange={this.handleLetstalkSelect} />;
            });
        }

        return result;
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

        if (!data.recipient) {
            isValid = false;
            errMsg.push(LangHelper.getResource('RecipientRequired'));
        }

        if (!data.letstalkId) {
            isValid = false;
            errMsg.push(LangHelper.getResource('LetstalkIdRequired'));
        }

        if (!data.groupHeadlines) {
            isValid = false;
            errMsg.push(LangHelper.getResource('GroupHeadlineRequired'));
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
            recipientTypeId: this.state.form.recipientType,
            letstalkId: this.state.form.letstalkId,
            groupHeadlineIds: this.state.form.groupHeadlines,
            recipientId: this.state.form.recipient.value,
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
        const recipientOptions = this.props.recipients.map(item => { return { value: item.id, label: item.name, letstalkIds: item.letstalkIds }; });
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
                        this.props.updatePersonalMappingResult &&
                        <ResponseMessage isSuccessful={this.props.updatePersonalMappingResult.isSuccessful}
                            message={this.props.updatePersonalMappingResult.message || LangHelper.getResource('InsertPersonalMappingSuccessfully')}
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
                        <Form.Label column sm="8" md="9">
                            {this.state.form.name}
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
                                onChange={e => this.handleRecipientTypeChange(e.target.value)}
                                value={this.state.form.recipientType}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.settings.recipientTypes.map(this.renderOptions)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('Recipient')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <SelectWithSearch
                                className='basic-single'
                                isSearchable
                                isClearable
                                name='recipient'
                                onChange={this.handleRecipientChange}
                                options={recipientOptions}
                                value={this.state.form.recipient}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('LetstalkID')}
                        </Form.Label>
                        <Col sm="8" md="9" >
                            {
                                this.renderLetstalkIds(this.selectedLetstalkIds)
                            }
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('GroupHeadline')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelect
                                width={'100%'}
                                name="headlineIds"
                                dataSource={this.props.groupHeadline}
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
                    <Button type="button" variant="primary" size="sm" onClick={this.props.closeUpdatePersonalMappingPopup}>
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
                onHide={this.props.closeUpdatePersonalMappingPopup}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditPersonalMapping')}</Modal.Title>
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
    const { settings, recipients, groupHeadline, gridParams, updatePersonalMappingData, updatePersonalMappingResult } = state;
    return {
        settings,
        recipients,
        groupHeadline,
        gridParams,
        updatePersonalMappingData,
        updatePersonalMappingResult
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditPersonalMappingPopup);