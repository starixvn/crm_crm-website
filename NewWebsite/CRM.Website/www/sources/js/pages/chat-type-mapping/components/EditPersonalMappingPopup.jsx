import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row, Alert } from 'react-bootstrap';
import MultipleSelect from '../../../components/multiple-select/index';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';
import LangHelper from '@jsroot/common/langHelper';
import SelectWithSearch from 'react-select';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';

export class EditPersonalMappingPopup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {
                botId: this.props.updatePersonalMappingData.data.botId,
                name: this.props.updatePersonalMappingData.data.conversationGroupName,
                conversationId: this.props.updatePersonalMappingData.data.conversationId,
                recipientType: this.props.updatePersonalMappingData.data.recipients[0].recipientTypeId,
                groupHeadlines: this.props.updatePersonalMappingData.data.headlines.map(v => v.headlineId).toString(),
                recipient: {
                    value: this.props.updatePersonalMappingData.data.recipients[0].recipientId
                },
                letstalkId: this.props.updatePersonalMappingData.data.conversationId,
                languageId: this.props.updatePersonalMappingData.data.languageId,
                remark: this.props.updatePersonalMappingData.data.remark
            },
            isSubmitting: false,
            settings: {
                letstalkIds: '',
                recipientOptions: [],
                headlineOptions: []
            },
            validate: {
                isShow: false,
                errMsgs: []
            },
            isShowConfirmDirtyForm: false
        };

        this.conversationGroupId = this.props.updatePersonalMappingData.data.conversationGroupId;
        this.initState = null;
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (nextProps.updatePersonalMappingData.data !== null) {
            const recipientId = prevState.form.recipient.value;
            const headlineIds = prevState.form.groupHeadlines.split(',');
            let letstalkIds = '';
            if (nextProps.recipients.length > 0) {
                const selectRecipient = nextProps.recipients.filter(r => r.id === recipientId);
                letstalkIds = selectRecipient.length > 0 ? selectRecipient[0].letstalkIds : '';
            }
            const recipientOptions = nextProps.recipients.map(item => { return { value: item.id, label: item.name, letstalkIds: item.letstalkIds }; });
            const headlineOptions = nextProps.groupHeadline;
            headlineOptions.map(headline => { headline['checked'] = headlineIds.some(v => v === headline.id); });
            const state = {
                settings: {
                    letstalkIds: letstalkIds,
                    recipientOptions: recipientOptions,
                    headlineOptions: headlineOptions
                }
            };
            return state;
        }

        return null;
    }

    componentDidMount() {
        this.initSate = Object.assign({}, this.state.form);
        const initData = this.props.updatePersonalMappingData.data;
        const recipientTypeId = initData.recipients[0].recipientTypeId;
        const selectedRecipientId = initData.recipients[0].recipientId;

        this.props.getRecipientByRecipientType(recipientTypeId);
        this.props.getGroupHeadlineByRecipient(selectedRecipientId);
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
        this.handleChange('recipient', { value: '' });
        this.handleChange('groupHeadlines', '');
        this.props.getRecipientByRecipientType(value);
    }

    handleRecipientChange = option => {
        this.handleChange('recipient', option);
        this.handleChange('groupHeadlines', '');
        this.setState({
            settings: {
                letstalkIds: option.letstalkIds
            }
        });
        this.props.getGroupHeadlineByRecipient(option.value);
    };

    handleLetstalkSelect = (e) => {
        this.handleChange('conversationId', e.target.value);
        this.props.getLetstalkDisplayName(this.state.form.botId, e.target.value).then(() => {
            this.handleChange('name', this.props.letstalkName);
        });
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
        if (this.state.settings.letstalkIds) {
            const letstalkList = this.state.settings.letstalkIds.split(',');
            result = letstalkList.map((letstalkId, key) => {
                return (
                    <Form.Check key={key} inline label={letstalkId}
                        name='conversationId' type="radio" id={`letstalkId-${key}`}
                        value={letstalkId}
                        onChange={this.handleLetstalkSelect}
                        checked={this.state.form.conversationId === letstalkId} />
                );
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

        if (!data.conversationId) {
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
            isSubmitting: isValid,
            validate: {
                isShow: !isValid,
                errMsgs: errMsg
            }
        });

        return isValid;
    }

    isDirtyForm = () => {
        return JSON.stringify(this.initSate) !== JSON.stringify(this.state.form);
    }

    onClosePopup = () => {
        if (this.isDirtyForm()) {
            this.setState({ isShowConfirmDirtyForm: true });
        } else {
            this.props.closeUpdatePersonalMappingPopup();
        }
    }

    closeConfirmDirtyForm = () => {
        this.setState({
            isShowConfirmDirtyForm: false
        });
    }

    getParams = () => {
        return {
            conversationGroupId: this.conversationGroupId,
            botId: this.state.form.botId,
            recipientTypeId: this.state.form.recipientType,
            letstalkId: this.state.form.conversationId,
            groupHeadlineIds: this.state.form.groupHeadlines,
            recipientIds: this.state.form.recipient.value,
            languageId: this.state.form.languageId,
            remark: this.state.form.remark
        };
    }

    handleSubmit = (e) => {
        this.props.setUpdatePersonalMappingResult(null);
        this.setState({ isSubmitting: true });
        e.preventDefault();

        if (this.isValidParams()) {
            const params = this.getParams();
            this.props.updatePersonalMapping(params).then(() => {
                if (this.props.updatePersonalMappingResult.isSuccessful) {
                    setTimeout(() => {
                        this.props.closeUpdatePersonalMappingPopup();
                        this.props.getGridData(this.props.gridParams);
                    }, 3000);
                } else {
                    this.setState({ isSubmitting: false });
                }
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
                        this.props.updatePersonalMappingResult &&
                        <ResponseMessage isSuccessful={this.props.updatePersonalMappingResult.isSuccessful}
                            message={this.props.updatePersonalMappingResult.message || LangHelper.getResource('UpdatePersonalMappingSuccess')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('BotName')}
                        </Form.Label>
                        <Col sm="9" md="10">
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
                        <Form.Label column sm="3" md="2">
                            {LangHelper.getResource('LetstalkName')}
                        </Form.Label>
                        <Form.Label column sm="9" md="10">
                            {this.state.form.name}
                        </Form.Label>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="3" md="2">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Form.Label column sm="9" md="10">
                            {LangHelper.getResource('Personal')}
                        </Form.Label>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('RecipientType')}
                        </Form.Label>
                        <Col sm="9" md="10">
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
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('Recipient')}
                        </Form.Label>
                        <Col sm="9" md="10">
                            <SelectWithSearch
                                className='basic-single'
                                isSearchable
                                isClearable
                                name='recipient'
                                onChange={this.handleRecipientChange}
                                options={this.state.settings.recipientOptions}
                                value={this.state.form.recipient.value}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('LetstalkID')}
                        </Form.Label>
                        <Col sm="9" md="10" >
                            {
                                this.renderLetstalkIds(this.selectedLetstalkIds)
                            }
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('GroupHeadline')}
                        </Form.Label>
                        <Col sm="9" md="10">
                            <MultipleSelect
                                width={'100%'}
                                name="headlineIds"
                                dataSource={this.state.settings.headlineOptions}
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
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('Language')}
                        </Form.Label>
                        <Col sm="9" md="10">
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
                        <Form.Label column sm="3" md="2" >
                            {LangHelper.getResource('Remark')}
                        </Form.Label>
                        <Col sm="9" md="10">
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
                    <Button type="button" variant="primary" size="sm" onClick={this.onClosePopup}>
                        {LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Form>
        );
    }

    render() {
        return (
            <Fragment>
                <Modal show className="chat-type-mapping-popup" dialogAs={DraggableModalDialog}
                    backdrop="static" keyboard={false} size="lg" centered
                    onHide={this.props.closeUpdatePersonalMappingPopup}
                >
                    <Modal.Header closeButton>
                        <Modal.Title as="div">{LangHelper.getResource('EditPersonalMapping')}</Modal.Title>
                    </Modal.Header>
                    {
                        this.renderForm()
                    }

                </Modal>
                {
                    this.state.isShowConfirmDirtyForm &&
                    <ConfirmPopup
                        size='lg'
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>{LangHelper.getResource('ConfirmDirtyMessage')}</div>
                        }
                        onSubmit={this.props.closeUpdatePersonalMappingPopup}
                        onClose={this.closeConfirmDirtyForm}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { settings, recipients, groupHeadline, gridParams, updatePersonalMappingData, updatePersonalMappingResult, letstalkName } = state;
    return {
        settings,
        recipients,
        groupHeadline,
        gridParams,
        letstalkName,
        updatePersonalMappingData,
        updatePersonalMappingResult
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditPersonalMappingPopup);