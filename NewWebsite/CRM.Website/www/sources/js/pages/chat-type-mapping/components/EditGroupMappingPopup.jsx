import React, { Component, useRef } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row, Alert } from 'react-bootstrap';
import MultipleSelect from '../../../components/multiple-select/index';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';
import LangHelper from '@jsroot/common/langHelper';
import Site from '@jsroot/common/site';

export class EditGroupMappingPopup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {
                bot: {
                    id: this.props.updateGroupMappingData.data.botId,
                    name: this.props.updateGroupMappingData.data.botName
                },
                name: this.props.updateGroupMappingData.data.conversationGroupName,
                conversationId: this.props.updateGroupMappingData.data.conversationId,
                recipientTypeId: this.props.updateGroupMappingData.data.recipients.length > 0 ? this.props.updateGroupMappingData.data.recipients[0].recipientTypeId : '',
                recipientIds: this.props.updateGroupMappingData.data.recipients.map(r => r.recipientId).toString(),
                groupHeadlines: this.props.updateGroupMappingData.data.headlines.map(v => v.headlineId).toString(),
                mappingStatus: {
                    id: this.props.updateGroupMappingData.data.mappingStatusId,
                    name: this.props.updateGroupMappingData.data.mappingStatus
                },
                languageId: this.props.updateGroupMappingData.data.languageId,
                remark: this.props.updateGroupMappingData.data.remark || ''
            },
            isSubmitting: false,
            settings: {
                displayLetstalkIds: [],
                recipientOptions: [],
                headlineOptions: []
            },
            validate: {
                isShow: false,
                errMsgs: []
            }
        };

        this.conversationGroupId = this.props.updateGroupMappingData.data.conversationGroupId;
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (nextProps.updateGroupMappingData.data !== null) {
            const selectedRecipientIds = prevState.form.recipientIds.split(',');
            const recipientOptions = nextProps.recipients;
            nextProps.recipients.map(item => {
                item['checked'] = selectedRecipientIds.some(v => parseInt(v, 10) === item.id);
            });

            const headlineIds = prevState.form.groupHeadlines.split(',');
            const headlineOptions = nextProps.groupHeadline;
            headlineOptions.map(headline => { headline['checked'] = headlineIds.some(v => parseInt(v, 10) === headline.id); });

            const state = {
                settings: {
                    recipientOptions: recipientOptions,
                    headlineOptions: headlineOptions
                }
            };

            return state;
        }

        return null;
    }

    componentDidMount() {
        const initData = this.props.updateGroupMappingData.data;
        const recipientTypeId = initData.recipients.length > 0 ? initData.recipients[0].recipientTypeId : '';
        const selectedRecipientIds = initData.recipients.map(r => r.recipientId).toString();

        if (recipientTypeId) {
            this.props.getRecipientByRecipientType(recipientTypeId);
            this.props.getGroupHeadlineByRecipient(selectedRecipientIds);
            this.verifyLetstalk(selectedRecipientIds);
        }
    }

    handleChange = (fieldName, fieldValue) => {
        this.setState(prevState => ({
            form: { ...prevState.form, ...{ [fieldName]: fieldValue } }
        }));
    };

    handleRecipientTypeChange = value => {
        this.handleChange('recipientTypeId', value);
        this.handleChange('recipientIds', '');
        this.handleChange('groupHeadlines', '');
        this.props.getRecipientByRecipientType(value);
    }

    handleRecipientChange = (currentItem, selectedItemsKey) => {
        this.handleChange('recipientIds', selectedItemsKey);
        this.handleChange('groupHeadlines', '');
        this.verifyLetstalk(selectedItemsKey.split(','));
        this.props.getGroupHeadlineByRecipient(selectedItemsKey);
    };

    verifyLetstalk = (recipientIds) => {
        const selectedOptions = this.state.settings.recipientOptions.filter(r => recipientIds.some(id => parseInt(id, 10) === r.id));
        const params = {
            botId: this.state.form.bot.id,
            groupConversationId: this.state.form.conversationId,
            recipients: selectedOptions.map((item, key) => {
                return {
                    recipientId: item.id,
                    recipientName: item.name,
                    letstalks: item.letstalkIds.split(',').map((l, k) => {
                        return {
                            letstalkId: l
                        };
                    })
                };
            })
        };

        this.props.verifyLetstalkInGroup(params);
    }

    handleGroupHeadlineChange = (currentItem, selectedItemsKey) => {
        this.handleChange('groupHeadlines', selectedItemsKey);
    }

    renderOptions = (item, key) => {
        return (
            <option key={item.id} value={item.id}>
                {item.name}
            </option>
        );
    }

    renderLetstalkIdVerification = (item, key) => {
        return (
            <div className='letstalk-group-mapping'>
                <div className='recipient-name common-letstalk-display'>{item.recipientName}</div>
                <div className='letstalk-wrapper'>
                    {
                        item.letstalks.length > 0 &&
                        item.letstalks.map((v, k) => {
                            return <div className='letstalk-name common-letstalk-display'>{v.letstalkId}<span className={v.isGroupMember ? 'fa fa-check icon' : ''} /></div>;
                        })
                    }
                </div>

            </div>
        );
    }

    isValidParams = () => {
        const data = this.state.form;
        let isValid = true;
        let errMsg = [];

        if (!data.recipientTypeId) {
            isValid = false;
            errMsg.push(LangHelper.getResource('RecipientTypeRequired'));
        }

        if (!data.recipientIds) {
            isValid = false;
            errMsg.push(LangHelper.getResource('RecipientRequired'));
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
            conversationGroupId: this.conversationGroupId,
            botId: this.state.form.bot.id,
            name: this.state.form.name,
            recipientTypeId: this.state.form.recipientTypeId,
            letstalkId: this.state.form.conversationId,
            groupHeadlineIds: this.state.form.groupHeadlines,
            recipientIds: this.state.form.recipientIds,
            languageId: this.state.form.languageId,
            remark: this.state.form.remark
        };
    }

    handleSubmit = (e) => {
        this.setState({ isSubmitting: true });
        this.props.setGroupMappingResult(null);
        e.preventDefault();
        if (this.isValidParams()) {
            const params = this.getParams();
            this.props.updateGroupMapping(params).then(() => {
                if (this.props.updateGroupMappingResult.isSuccessful) {
                    setTimeout(() => {
                        this.props.closeUpdateGroupMappingPopup();
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
                        this.props.updateGroupMappingResult &&
                        <ResponseMessage isSuccessful={this.props.updateGroupMappingResult.isSuccessful}
                            message={this.props.updateGroupMappingResult.message || LangHelper.getResource('UpdateGroupMappingSuccess')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3">
                            {LangHelper.getResource('BotName')}
                        </Form.Label>
                        <Form.Label column sm="8" md="9">
                            {this.state.form.bot.name}
                        </Form.Label>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3">
                            {LangHelper.getResource('LetstalkName')}
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
                            {LangHelper.getResource('Group')}
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
                                value={this.state.form.recipientTypeId}
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
                            <MultipleSelect
                                width={'100%'}
                                name="headlineIds"
                                dataSource={this.state.settings.recipientOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                onChange={this.handleRecipientChange}
                                isSearchable

                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3">
                            {LangHelper.getResource('LetstalkID')}
                        </Form.Label>
                        <Col sm="8" md="9" className="mrg-top-3">
                            {
                                this.props.letstalkVerification.length > 0 &&
                                this.props.letstalkVerification.map(this.renderLetstalkIdVerification)
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
                        <Form.Label column sm="4" md="3">
                            {LangHelper.getResource('MappingStatus')}
                        </Form.Label>
                        <Form.Label column sm="8" md="9" className={Site.mappingStatusClass(this.state.form.mappingStatus.id)}>
                            {this.state.form.mappingStatus.name}
                        </Form.Label>
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
                    <Button type="button" variant="primary" size="sm" onClick={this.props.closeUpdateGroupMappingPopup}>
                        {LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Form>
        );
    }

    render() {
        return (
            <Modal show className="chat-type-mapping-popup" dialogAs={DraggableModalDialog}
                backdrop="static" keyboard={false} size="md" centered
                onHide={this.props.closeUpdateGroupMappingPopup}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditGroupMapping')}</Modal.Title>
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
    const { settings, recipients, gridParams, groupHeadline, updateGroupMappingData, updateGroupMappingResult, letstalkVerification } = state;
    return {
        settings,
        recipients,
        groupHeadline,
        gridParams,
        updateGroupMappingData,
        updateGroupMappingResult,
        letstalkVerification
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditGroupMappingPopup);