import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Col, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import MultipleSelect from '../../../components/multiple-select/index';
import InsertPersonalMappingPopup from './InsertPersonalMappingPopup';

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
                chatType: '',
                name: '',
                groupHeadline: '',
                recipient: '',
                letstalkID: '',
                mappingStatuses: {
                    value: ''
                },
                languageId: '',
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
            const state = {
                filter: {
                    chatType: '',
                    name: '',
                    groupHeadline: '',
                    recipient: '',
                    letstalkID: '',
                    mappingStatuses: selectedMappingStatuses.toString(),
                    languages: '',
                    botIds: selectedbotIds.toString(),
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
            botIds: this.state.filter.botIds,
            name: this.state.filter.name,
            groupHeadline: this.state.filter.groupHeadline,
            recipientIds: this.state.filter.recipient,
            letstalkId: this.state.filter.letstalkID,
            mappingStatuses: this.state.filter.mappingStatuses,
            languageId: this.state.filter.languageId,
            isActive: this.state.filter.active,
            pageIndex: 1,
            pageSize: 100
        };
    }

    isValidParams = () => {
        let isValid = true;
        let errMsg;
        const filter = this.state.filter;
        if (filter.botIds === '') {
            isValid = false;
            errMsg = 'Please choose Bot Name';
        }

        if (filter.mappingStatuses === '') {
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
            console.log(params);
            this.props.setGridParams(params);
            this.props.getGridData(params);
        }
    }

    renderOptions = (item, key) => {
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
                    <Form.Row className='form-row'>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('ChatType')}</Form.Label>
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.chatType}
                                onChange={(e) => this.onChange('chatType', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                {this.state.settings.chatTypes.map(this.renderOptions)}
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('Name')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.name}
                                onChange={(e) => this.onChange('name', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('GroupHeadline')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.groupHeadline}
                                onChange={(e) => this.onChange('groupHeadline', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('Recipient')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.recipient}
                                onChange={(e) => this.onChange('recipient', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('LetstalkID')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.letstalkID}
                                onChange={(e) => this.onChange('letstalkID', e.target.value)}
                            />
                        </Form.Group>

                    </Form.Row>
                    <Form.Row className='form-row'>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('Status')}</Form.Label>
                            <MultipleSelect
                                width={154}
                                initialValue={this.state.filter.mappingStatuses}
                                dataSource={this.state.settings.mappingStatuses}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={(currentItem, selectedItem) => this.onChange('mappingStatuses', selectedItem)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('Language')}</Form.Label>
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.languageId}
                                onChange={(e) => this.onChange('languageId', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                {this.state.settings.languages.map(this.renderOptions)}
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('Active')}</Form.Label>
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.active}
                                onChange={e => this.onChange('active', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                <option value="1">{LangHelper.getResource('Yes')}</option>
                                <option value="0">{LangHelper.getResource('No')}</option>
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('BotName')}</Form.Label>
                            <MultipleSelect
                                width={154}
                                initialValue={this.state.filter.botIds}
                                dataSource={this.state.settings.bots}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={(currentItem, selectedItem) => this.onChange('botIds', selectedItem)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <div className='col-auto pd-0'>
                                <Button className='mr-2' variant='primary' size='sm' type='button' onClick={this.submitHandler}>{LangHelper.getResource('Submit')}</Button>
                            </div>

                            <div className='col-auto'>
                                <Button className='mr-2' variant='primary' size='sm' type='button' onClick={this.props.openInsertPersonalMappingPopup}>{LangHelper.getResource('InsertPersonalMappingBtn')}</Button>
                            </div>
                        </Form.Group>
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