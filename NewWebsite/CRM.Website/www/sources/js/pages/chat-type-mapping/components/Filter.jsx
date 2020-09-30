import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelect from '../../../components/multiple-select/index';
import InsertPersonalMappingPopup from './InsertPersonalMappingPopup';
import { toast } from 'react-toastify';

export class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
                chatType: '',
                name: '',
                groupHeadline: '',
                recipient: '',
                letstalkID: '',
                mappingStatuses: '',
                languageId: '',
                botId: '',
                active: ''
            },
            settings: {
                chatTypes: [],
                mappingStatuses: [],
                languages: [],
                bots: []
            }
        };
    }

    componentDidMount() {
        this.props.getSettings().then(() => {
            const mappingStatuses = this.props.settings.mappingStatuses;
            const selectedMappingStatuses = [];
            mappingStatuses.map(v => {
                v['checked'] = true;
                selectedMappingStatuses.push(v['id']);
            });

            const settings = {
                chatTypes: this.props.settings.chatTypes,
                mappingStatuses: mappingStatuses,
                languages: this.props.settings.languages,
                bots: this.props.settings.bots
            };

            this.setState(prevState => ({
                filter: { ...prevState.filter, ...{ mappingStatuses: selectedMappingStatuses.toString() } },
                settings: settings
            }), () => this.props.setGridParams(this.getParams()));
        });
    }

    onChange = (fieldName, fieldValue) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [fieldName]: fieldValue } }
        }));
    }

    getParams = () => {
        return {
            chatType: this.state.filter.chatType,
            botId: this.state.filter.botId,
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

    isValidParams = () => this.state.filter.mappingStatuses !== '';

    submitHandler = e => {
        e.preventDefault();
        if (this.isValidParams()) {
            const params = this.getParams();
            this.props.setGridParams(params);
            this.props.getGridData(params);
        } else {
            toast.error(LangHelper.getResource('PleaseChooseMappingStatus'));
        }
    }

    renderOptions = (item, key) => <option key={key} value={item.id}>{item.name}</option>;

    render() {
        return (
            <Fragment>
                <Form className='form-filter' onSubmit={this.submitHandler}>
                    <Form.Row className='form-row'>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('ChatType')}</Form.Label>
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.chatType}
                                onChange={e => this.onChange('chatType', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                {this.state.settings.chatTypes.map(this.renderOptions)}
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('LetstalkName')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.name}
                                onChange={e => this.onChange('name', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('GroupHeadline')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.groupHeadline}
                                onChange={e => this.onChange('groupHeadline', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('Recipient')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.recipient}
                                onChange={e => this.onChange('recipient', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className="col-auto col-form-label">{LangHelper.getResource('LetstalkID')}</Form.Label>
                            <Form.Control type="text" size="sm" className="shadow-none"
                                value={this.state.filter.letstalkID}
                                onChange={e => this.onChange('letstalkID', e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('MappingStatus')}</Form.Label>
                            <div className='form-control-wrapper'>
                                <MultipleSelect
                                    initialValue={this.state.filter.mappingStatuses}
                                    dataSource={this.state.settings.mappingStatuses}
                                    keyField='id'
                                    valueField='name'
                                    statusField='checked'
                                    isSummited={false}
                                    isAllTextShown
                                    onChange={(currentItem, selectedItem) => this.onChange('mappingStatuses', selectedItem)}
                                />
                            </div>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('Language')}</Form.Label>
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.languageId}
                                onChange={e => this.onChange('languageId', e.target.value)}
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
                                <option value="true">{LangHelper.getResource('Yes')}</option>
                                <option value="false">{LangHelper.getResource('No')}</option>
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label className='col-auto col-form-label'>{LangHelper.getResource('BotName')}</Form.Label>
                            <Form.Control as="select" size="sm" className="shadow-none"
                                value={this.state.filter.botId}
                                onChange={e => this.onChange('botId', e.target.value)}
                            >
                                <option value="">{LangHelper.getResource('All')}</option>
                                {this.state.settings.bots.map(this.renderOptions)}
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <div className='col-auto pd-0'>
                                <Button className='mr-2' variant='primary' size='sm' type='submit'>
                                    {LangHelper.getResource('Submit')}
                                </Button>
                            </div>
                            {
                                this.props.isHost &&
                                <div className='col-auto'>
                                    <Button className='mr-2' variant='primary' size='sm' type='button' onClick={this.props.openInsertPersonalMappingPopup}>
                                        {LangHelper.getResource('InsertPersonalMappingBtn')}
                                    </Button>
                                </div>
                            }
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