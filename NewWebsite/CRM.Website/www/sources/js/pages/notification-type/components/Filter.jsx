import React, { Component } from 'react';
import { Button,Form } from 'react-bootstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import Axios from 'axios';
import Site from '@jsroot/common/site';


class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
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

    componentDidMount() {
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
        const promises = [
            Axios(Site.resolveClientUrl('NotificationType/GetRecipientTypes')),
            Axios(Site.resolveClientUrl('NotificationType/GetChatTypes')),
            Axios(Site.resolveClientUrl('NotificationType/GetSystems'))
        ]

        Promise.all(promises)
            .then(([recipientTypesResponse, chatTypesResponse, systemsResponse]) => {
                // recipient type
                const recipientTypeIds = [];
                const recipientTypesOptions = recipientTypesResponse.data.map(x => {
                    recipientTypeIds.push(x.recipientTypeID);
                    return {
                        id: x.id,
                        name: x.name,
                        checked: true
                    };
                });

                // chat type
                const chatTypeIds = [];
                const chatTypesOptions = chatTypesResponse.data.map(x => {
                    chatTypeIds.push(x.id);
                    return {
                        id: x.id,
                        name: x.name,
                        checked: true
                    };
                });

                // system
                const systemIds = [];
                const systemsOptions = systemsResponse.data.map(x => {
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
                        ...prevState.filter,
                        ...{
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
                    }
                }), () => {
                    const pageIndex = 1;
                });
            }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    }

    handleChatTypeChange = event => {
        this.setState({
            chatTypeId: event.target.value
        });
    }

    handleRecipientTypeChange = event => {
        this.setState({
            recipientTypeId: event.target.value
        });
    }

    recipientTypeChange = value => this.handleFilterChange('recipientTypes', value)
    chatTypeChange = value => this.handleFilterChange('chatTypes', value)
    systemChange = value => this.handleFilterChange('systems', value)

    handleSystemChange = event => {
        this.setState({
            systemId: event.target.value
        });
    }

    handleNotificationTypeChange = event => {
        this.setState({
            notificationType: event.target.value
        });
    }

    handleFilterChange = (name, value) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [name]: value } }
        }));
    }

    getParams = () => {
        return {
            notificationType: this.state.notificationType,
            chatTypeId: parseInt(this.state.chatTypeId, 10),
            recipientTypeId: parseInt(this.state.recipientTypeId, 10),
            systemId: parseInt(this.state.systemTypeId, 10),
            pageIndex: 1,
            pageSize: 50
        };
    }

    submitHandler = e => {
        e.preventDefault();
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    handleOpenCreatePopup = () => this.props.setCreateTypePopup({ show: true })

    render() {
        const { isHost } = this.props;
        return (
            <form className="form-filter">
                <div className="form-row"> 
                    <label className="col-auto col-form-label">{LangHelper.getResource('NotificationType')}</label>
                    <div className="col-auto">
                        <input type="text" className="form-control form-control-sm shadow-none"
                            value={this.state.notificationType}
                            onChange={this.handleNotificationTypeChange}
                        />
                    </div>

                    <label className="col-auto col-form-label">{LangHelper.getResource('RecipientType')}</label>
                    <div className="col-auto">
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.recipientTypes.value}
                            data={this.state.recipientTypeOptions}
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
                            data={this.state.chatTypeOptions}
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
                            data={this.state.systemOptions}
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
            </form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { notificationTypes, gridParams } = state;
    return {
        notificationTypes,
        gridParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);