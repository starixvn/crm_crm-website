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
import Loading from '@jsroot/components/Loading';

export class InsertPersonalMappingPopup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {
                botId: this.props.settings.bots[0].id,
                displayName: '',
                recipientType: '',
                groupHeadlines: '',
                recipient: '',
                letstalkId: '',
                languageId: '',
                remark: ''
            },
            isSubmitting: false,
            letstalkIds: '',
            validate: {
                isShow: false,
                errMsgs: []
            },
            isLoading: false,
            dirtyFormConfirmation: {
                isShow: false
            }
        };

        this.initState = null;
    }

    componentDidMount() {
        this.initState = Object.assign({}, this.state.form);
    }

    handleChange = (fieldName, fieldValue) => {
        this.setState(prevState => ({
            form: { ...prevState.form, ...{ [fieldName]: fieldValue } }
        }));
    };

    handleBotChange = value => {
        this.setState({ isLoading: true });
        this.handleChange('botId', value);
        this.props.getLetstalkDisplayName(value, this.state.form.letstalkId).then(() => {
            this.handleChange('displayName', this.props.letstalkName);
            this.setState({ isLoading: false });
        });
    }

    handleGroupHeadlineChange = (currentItem, selectedItemsKey) => {
        this.handleChange('groupHeadlines', selectedItemsKey);
    }

    handleRecipientTypeChange = value => {
        this.handleChange('recipientType', value);
        this.props.getRecipientByRecipientType(value);
    }

    handleRecipientChange = option => {
        this.handleChange('recipient', option);
        this.handleChange('letstalkId', '');
        this.handleChange('displayName', '');
        this.setState({
            letstalkIds: option.letstalkIds
        });
        this.props.getGroupHeadlineByRecipient(option.value);
    };

    handleLetstalkSelect = e => {
        this.setState({ isLoading: true });
        this.handleChange('letstalkId', e.target.value);
        this.props.getLetstalkDisplayName(this.state.form.botId, e.target.value).then(() => {
            this.handleChange('displayName', this.props.letstalkName);
            this.setState({ isLoading: false });
        });
    }

    renderOptions = (item, key) => {
        return (
            <option key={`${key}-${item.id}`} value={item.id}>
                {item.name}
            </option>
        );
    }

    renderLetstalkIds = () => {
        let result = null;
        if (this.state.letstalkIds) {
            const letstalkList = this.state.letstalkIds.split(',');
            result = letstalkList.map((letstalkId, key) => (
                <Form.Check key={key}
                    inline label={letstalkId}
                    name='letstalkId'
                    type="radio"
                    id={`letstalkId-${key}`}
                    value={letstalkId}
                    checked={this.state.form.letstalkId === letstalkId}
                    onChange={this.handleLetstalkSelect} />
            ));
        }

        return result;
    }

    isValidParams = () => {
        const data = this.state.form;
        const errMsg = [];
        let isValid = true;

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
            isSubmitting: isValid,
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
            recipientIds: this.state.form.recipient.value,
            languageId: this.state.form.languageId,
            remark: this.state.form.remark
        };
    }

    handleSubmit = e => {
        this.props.setInsertPersonalMappingResult(null);
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
                } else {
                    this.setState({ isSubmitting: false });
                }
            });
        }
    }

    isDirtyForm = () => JSON.stringify(this.initState) !== JSON.stringify(this.state.form);

    onCloseInsertPopup = () => {
        if (this.isDirtyForm()) {
            this.setState({
                dirtyFormConfirmation: { isShow: true }
            });
        } else {
            this.props.closeInsertPersonalMappingPopup();
        }
    }

    closeConfirmDirtyForm = () => {
        this.setState({
            dirtyFormConfirmation: { isShow: false }
        });
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
                                    this.state.validate.errMsgs.map((msg, key) => <li key={key}>{msg}</li>)
                                }
                            </ul>
                        </Alert>
                    }
                    {
                        this.props.insertPersonalMappingResult &&
                        <ResponseMessage isSuccessful={this.props.insertPersonalMappingResult.isSuccessful}
                            message={this.props.insertPersonalMappingResult.message || LangHelper.getResource('InsertPersonalMappingSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="3" md="2" className="required">
                            {LangHelper.getResource('BotName')}
                        </Form.Label>
                        <Col sm="9" md="10">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                name='botId'
                                onChange={e => this.handleBotChange(e.target.value)}
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
                            {
                                this.state.isLoading &&
                                <Loading />
                            }
                            {
                                !this.state.isLoading &&
                                this.state.form.displayName
                            }
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
                                options={recipientOptions}
                                value={this.state.form.recipient}
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
                                dataSource={this.props.groupHeadline}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                onChange={this.handleGroupHeadlineChange}
                                hasSearchBox

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
                    <Button type="button" variant="primary" size="sm" onClick={this.onCloseInsertPopup}>
                        {LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Form>
        );
    }

    render() {
        return (
            <Fragment>
                <Modal show className="chat-type-mapping-modal" dialogAs={DraggableModalDialog}
                    backdrop="static" keyboard={false} size="lg" centered
                    onHide={this.onCloseInsertPopup}
                >
                    <Modal.Header closeButton>
                        <Modal.Title as="div">{LangHelper.getResource('InsertPersonalMapping')}</Modal.Title>
                    </Modal.Header>
                    {
                        this.renderForm()
                    }

                </Modal>
                {
                    this.state.dirtyFormConfirmation.isShow &&
                    <ConfirmPopup
                        size='lg'
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>{LangHelper.getResource('ConfirmDirtyMessage')}</div>
                        }
                        onSubmit={this.props.closeInsertPersonalMappingPopup}
                        onClose={this.closeConfirmDirtyForm}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { settings, recipients, groupHeadline, gridParams, insertPersonalMappingResult, letstalkName } = state;
    return {
        settings,
        recipients,
        groupHeadline,
        gridParams,
        insertPersonalMappingResult,
        letstalkName
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(InsertPersonalMappingPopup);