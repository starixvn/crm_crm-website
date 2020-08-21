import React, { Component } from 'react';
import { Button,Form } from 'react-bootstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
                notificationTypeName: '',
                recipientTypes: {
                    value: ''
                },
                chatTypes: {
                    value: ''
                },
                systems: {
                    value: ''
                }
            },
            recipientTypesOptions: [],
            chatTypesOptions: [],
            systemsOptions: []
        };
    }

    componentDidUpdate = (nextProps, prevState) => {
        if(nextProps.settings !== this.props.settings) {
            // recipient type
            const recipientTypeIds = [];
            const recipientTypesOptions = this.props.settings.recipients.map(x => {
                recipientTypeIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });

            // chat type
            const chatTypeIds = [];
            const chatTypesOptions = this.props.settings.chatTypes.map(x => {
                chatTypeIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });

            // system
            const systemIds = [];
            const systemsOptions = this.props.settings.systems.map(x => {
                systemIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });

            this.setState(prevState => ({
                    recipientTypesOptions,
                    chatTypesOptions,
                    systemsOptions,
                    filter: {
                            recipientTypes: {
                                value: recipientTypeIds.toString()
                            },
                            chatTypes: {
                                value: chatTypeIds.toString()
                            },
                            systems: {
                                value: systemIds.toString()
                            }
                    }
                }));
        }
    }

    componentDidMount() {
        this.props.getSettings();
    }

    recipientTypeChange = value => this.handleFilterChange('recipientTypes', value)
    chatTypeChange = value => this.handleFilterChange('chatTypes', value)
    systemChange = value => this.handleFilterChange('systems', value)

    handleNotificationTypeChange = event => {
        this.setState({
            notificationTypeName: event.target.value
        });
    }

    handleFilterChange = (name, value) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [name]: value } }
        }));
    }

    getParams = () => {
        return {
            notificationTypeName: this.state.notificationTypeName,
            chatTypeId: this.state.filter.chatTypes.value,
            recipientTypeId: this.state.filter.recipientTypes.value,
            systemId: this.state.filter.systems.value,
            pageIndex: 1,
            pageSize: 50
        };
    }

    submitHandler = e => {
        e.preventDefault();
        const params = this.getParams();
        this.props.setInvalidParams({
            show: false
        });

        const error = [];
        if (params.recipientTypeId == '') {
            error.push('Recipient Type');
        }

        if (params.chatTypeId == '') {
            error.push(' Chat Type');
        }

        if (params.systemId == '') {
            error.push(' System');
        }

        if(error.length > 0)
        {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} ${error.toString()}` 
            });
        }
        else
        {
            this.props.setGridParams(params);
            this.props.getGridData(params);
        }
    }

    handleOpenCreatePopup = () => this.props.setCreateTypePopup({ show: true })

    render() {
        const { isHost } = this.props;
        return (
            <Form className="form-filter" onSubmit={this.submitHandler}>
                <div className="form-row"> 
                    <Form.Group>
                        <Form.Label htmlFor="NotificationType">{LangHelper.getResource('NotificationType')}</Form.Label>
                        <Form.Control type="text" size="sm" className="shadow-none" autoComplete="off"
                            value={this.state.notificationTypeName}
                            onChange={this.handleNotificationTypeChange}
                        />
                    </Form.Group>

                    <label className="col-auto col-form-label">{LangHelper.getResource('RecipientType')}</label>
                    <div className="col-auto">
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.recipientTypes.value}
                            data={this.state.recipientTypesOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.recipientTypeChange}
                        />
                    </div>

                    <label className="col-auto col-form-label">{LangHelper.getResource('ChatType')}</label>
                    <div className="col-auto">
                    <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.chatTypes.value}
                            data={this.state.chatTypesOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.chatTypeChange}
                        />
                    </div>

                    <label className="col-auto col-form-label">{LangHelper.getResource('System')}</label>
                    <div className="col-auto">
                    <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.systems.value}
                            data={this.state.systemsOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.systemChange}
                        />
                    </div>

                    <div className="col-auto">
                        <Button className="mr-2" variant="primary" size="sm"
                            onClick={this.submitHandler}
                        >
                            {LangHelper.getResource('Submit')}
                        </Button>
                        {
                             isHost &&
                            <Button variant="primary" size="sm"
                                onClick={this.handleOpenCreatePopup}
                            >
                                {LangHelper.getResource('Insert')}
                            </Button>
                        }
                    </div>
                </div>
            </Form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { gridParams, settings, getSettings, invalidParams, setInvalidParams } = state;
    return {
        gridParams,
        settings,
        getSettings,
        invalidParams,
        setInvalidParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);