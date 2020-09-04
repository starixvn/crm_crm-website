import React, { Component } from 'react';
import { Button, Form } from 'react-bootstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelect from '../../../components/multiple-select/index';
import { toast } from 'react-toastify';

export class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
                notificationTypeName: '',
                recipientTypes: '',
                chatTypes: '',
                systems: ''
            },
            recipientTypesOptions: [],
            chatTypesOptions: [],
            systemsOptions: []
        };
    }

    componentDidMount = () => {
        this.props.getSettings().then(() => {
            // recipient type
            const recipientTypeIds = [];
            const recipientTypesOptions = this.props.settings.recipientTypes.map(x => {
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

            this.setState({
                recipientTypesOptions,
                chatTypesOptions,
                systemsOptions,
                filter: {
                    recipientTypes: recipientTypeIds.toString(),
                    chatTypes: chatTypeIds.toString(),
                    systems: systemIds.toString()
                }
            });
        });
    }

    recipientNotificationTypeChange = (currentItem, selectedItemsKey) => this.handleFilterChange('recipientTypes', selectedItemsKey)
    chatNotificationTypeChange = (currentItem, selectedItemsKey) => this.handleFilterChange('chatTypes', selectedItemsKey)
    systemChange = (currentItem, selectedItemsKey) => this.handleFilterChange('systems', selectedItemsKey)

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
            chatTypeIds: this.state.filter.chatTypes,
            recipientTypeIds: this.state.filter.recipientTypes,
            systemId: this.state.filter.systems,
            pageIndex: 1,
            pageSize: 100
        };
    }

    submitHandler = e => {
        e.preventDefault();
        const params = this.getParams();

        const error = [];
        if (params.recipientTypeIds === '') {
            error.push(`${LangHelper.getResource('RecipientType')}`);
        }

        if (params.chatTypeIds === '') {
            error.push(`${LangHelper.getResource('ChatType')}`);
        }

        if (params.systemId === '') {
            error.push(`${LangHelper.getResource('System')}`);
        }

        if (error.length > 0) {
            toast.error(`${LangHelper.getResource('PleaseChoose')} ${error.join(', ').toString()}`);
        }
        else {
            this.props.setGridParams(params);
            this.props.getGridData(params);
        }
    }

    handleOpenCreatePopup = () => this.props.setCreateNotificationTypePopup({ show: true })

    render() {
        const { isHost } = this.props;

        return (
            <Form className="form-filter" onSubmit={this.submitHandler}>
                <Form.Row bsPrefix='form-row'>
                    <Form.Group>
                        <Form.Label htmlFor="notificationTypeName">{LangHelper.getResource('NotificationType')}</Form.Label>
                        <Form.Control type="text" size="sm" className="shadow-none" autoComplete="off" id="notificationTypeName"
                            value={this.state.notificationTypeName}
                            onChange={this.handleNotificationTypeChange}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('RecipientType')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                dataSource={this.state.recipientTypesOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={this.recipientNotificationTypeChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('ChatType')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                dataSource={this.state.chatTypesOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={this.chatNotificationTypeChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('System')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                dataSource={this.state.systemsOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={this.systemChange}
                            />
                        </div>
                    </Form.Group>

                    <Form.Group>
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
                    </Form.Group>
                </Form.Row>
            </Form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        gridParams,
        settings,
        getSettings,
    } = state;
    return {
        gridParams,
        settings,
        getSettings,
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);