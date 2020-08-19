import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Col, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import MultipleSelectField from './MultipleSelectField';
import InsertPersonalMappingPopup from './InsertPersonalMappingPopup';
import Site from '@jsroot/common/site';

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
                chatType: null,
                name: '',
                groupHeadline: '',
                recipient: '',
                letstalkID: '',
                mappingStatuses: {
                    value: ''
                },
                languages: {
                    value: ''
                },
                botIds: {
                    value: ''
                },
                active: ''
            },
            settings: {
                chatTypes: [],
                mappingStatuses: [],
                languages: [],
                bots: []
            },
            validation: {
                isValid: true,
                errMsg: ''
            }
        };
        this.props.getSettings();
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (prevState.settings.bots.length === 0 && nextProps.settings.bots) {
            const bots = nextProps.settings.bots;
            const selectedbotIds = [];
            bots.map(v => {
                v['checked'] = true;
                selectedbotIds.push(v['id']);
            });

            const chatTypes = nextProps.settings.chatTypes;
            const mappingStatuses = nextProps.settings.mappingStatuses;
            const selectedMappingStatuses = [];
            mappingStatuses.map(v => {
                v['checked'] = true;
                selectedMappingStatuses.push(v['id']);
            });

            const languages = nextProps.settings.languages;
            const selectedLanguages = [];
            languages.map(v => {
                v['checked'] = true;
                selectedLanguages.push(v['id']);
            });

            const state = {
                filter: {
                    chatType: null,
                    name: '',
                    groupHeadline: '',
                    recipient: '',
                    letstalkID: '',
                    mappingStatuses: {
                        value: selectedMappingStatuses.join(','),
                        isValid: true
                    },
                    languages: {
                        value: selectedLanguages.join(','),
                        isValid: true
                    },
                    botIds: {
                        value: selectedbotIds.join(','),
                        isValid: true
                    },
                    active: ''
                },
                settings: {
                    chatTypes: chatTypes,
                    mappingStatuses: mappingStatuses,
                    languages: languages,
                    bots: bots
                }
            };
            return state;
        }
        return null;
    }

    onChange = (fieldName, fieldValue) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [fieldName]: fieldValue } }
        }));
    }

    getParams = () => {
        return {
            chatType: this.state.filter.chatType,
            botIds: this.state.filter.botIds.value,
            name: this.state.filter.name,
            groupHeadline: this.state.filter.groupHeadline,
            recipientIds: this.state.filter.recipient,
            letstalkId: this.state.filter.letstalkID,
            mappingStatuses: this.state.filter.mappingStatuses.value,
            languageIds: this.state.filter.languages.value,
            isActive: this.state.filter.active,
            pageIndex: 1,
            pageSize: 100
        };
    }

    isValidParams = () => {
        let isValid = true;
        let errMsg;
        const filter = this.state.filter;
        if (filter.botIds.value === '') {
            isValid = false;
            errMsg = 'Please choose Bot Name';
        }

        if (filter.languages.value === '') {
            isValid = false;
            errMsg = 'Please choose Language';
        }

        if (filter.mappingStatuses.value === '') {
            isValid = false;
            errMsg = 'Please choose Status';
        }

        this.setState({
            validation: {
                isValid: isValid,
                errMsg: errMsg
            }
        });

        return isValid;
    }

    submitHandler = e => {
        e.preventDefault();
        if (this.isValidParams()) {
            const params = this.getParams();
            this.props.setGridParams(params);
            this.props.getGridData(params);
        }
    }

    renderChatTypes = (item, key) => {
        return (
            <option key={key} value={item.id}>
                {item.name}
            </option>
        );
    }
    render() {
        return (
            <Fragment>
                {
                    !this.state.validation.isValid &&
                    <ResponseMessage isSuccessful={false}
                        message={this.state.validation.errMsg}
                    />
                }
                <Form className='form-filter' tag='div'>
                    <Form.Row bsPrefix='form-row'>
                        <label className='col-auto col-form-label'>{LangHelper.getResource('ChatType')}</label>
                        <div className="col-auto">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.chatType}
                                onChange={(e) => this.onChange('chatType', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                {this.state.settings.chatTypes.map(this.renderChatTypes)}
                            </Form.Control>
                        </div>
                        <label className="col-auto col-form-label">{LangHelper.getResource('Name')}</label>
                        <div className="col-auto">
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.name}
                                onChange={(e) => this.onChange('name', e.target.value)}
                            />
                        </div>
                        <label className="col-auto col-form-label">{LangHelper.getResource('GroupHeadline')}</label>
                        <div className="col-auto">
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.groupHeadline}
                                onChange={(e) => this.onChange('groupHeadline', e.target.value)}
                            />
                        </div>
                        <label className="col-auto col-form-label">{LangHelper.getResource('Recipient')}</label>
                        <div className="col-auto">
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.recipient}
                                onChange={(e) => this.onChange('recipient', e.target.value)}
                            />
                        </div>
                        <label className="col-auto col-form-label">{LangHelper.getResource('LetstalkID')}</label>
                        <div className="col-auto">
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.letstalkID}
                                onChange={(e) => this.onChange('letstalkID', e.target.value)}
                            />
                        </div>
                        <label className='col-auto col-form-label'>{LangHelper.getResource('Status')}</label>
                        <div className='col-auto position-relative'>
                            <MultipleSelectField
                                width={150}
                                initialValue={this.state.filter.mappingStatuses}
                                data={this.state.settings.mappingStatuses}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={value => this.onChange('mappingStatuses', value)}
                            />
                        </div>
                    </Form.Row>
                    <Form.Row bsPrefix='form-row'>
                        <label className='col-auto col-form-label'>{LangHelper.getResource('Language')}</label>
                        <div className='col-auto position-relative'>
                            <MultipleSelectField
                                width={150}
                                initialValue={this.state.filter.languages}
                                data={this.state.settings.languages}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={value => this.onChange('languages', value)}
                            />
                        </div>
                        <label className='col-auto col-form-label'>{LangHelper.getResource('Active')}</label>
                        <div className="col-auto">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.active}
                                onChange={e => this.onChange('active', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                <option value="1">{LangHelper.getResource('Yes')}</option>
                                <option value="0">{LangHelper.getResource('No')}</option>
                            </Form.Control>
                        </div>
                        <label className='col-auto col-form-label'>{LangHelper.getResource('BotName')}</label>
                        <div className='col-auto position-relative'>
                            <MultipleSelectField
                                width={150}
                                initialValue={this.state.filter.botIds}
                                data={this.state.settings.bots}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={value => this.onChange('botIds', value)}
                            />
                        </div>
                        <div className='col-auto'>
                            <Button className='mr-2' variant='primary' size='sm' type='button' onClick={this.submitHandler}>{LangHelper.getResource('Submit')}</Button>
                        </div>
                        <div className='col-auto'>
                            <Button className='mr-2' variant='primary' size='sm' type='button' onClick={this.props.openInsertPersonalMappingPopup}>{LangHelper.getResource('InsertPersonalMapping')}</Button>
                        </div>
                    </Form.Row>
                </Form>
                {
                    this.props.insertPersonalMappingData.isShow &&
                    <InsertPersonalMappingPopup onClose={this.props.closePersonalMappingPopup} />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { settings, insertPersonalMappingData, insertPersonalMappingResult } = state;
    return {
        settings,
        insertPersonalMappingData,
        insertPersonalMappingResult
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);