import React, { Component } from 'react';
import { Button, Form } from 'react-bootstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelect from '../../../components/multiple-select/index';

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
        if (nextProps.settings !== this.props.settings) {
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

    recipientTypeChange = (currentItem, selectedItemsKey) => this.handleFilterChange('recipientTypes', selectedItemsKey)
    chatTypeChange = (currentItem, selectedItemsKey) => this.handleFilterChange('chatTypes', selectedItemsKey)
    systemChange = (currentItem, selectedItemsKey) => this.handleFilterChange('systems', selectedItemsKey)

    handleNotificationTypeChange = event => {
        this.setState({
            notificationTypeName: event.target.value
        });
    }

    handleFilterChange = (name, value) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [name]: { value: value } } }
        }));
    }

    getParams = () => {
        return {
            notificationTypeName: this.state.notificationTypeName,
            chatTypeIds: this.state.filter.chatTypes.value,
            recipientTypeIds: this.state.filter.recipientTypes.value,
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
        if (params.recipientTypeIds == '') {
            error.push('Recipient Type');
        }

        if (params.chatTypeIds == '') {
            error.push(' Chat Type');
        }

        if (params.systemId == '') {
            error.push(' System');
        }

        if (error.length > 0) {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} ${error.toString()}`
            });
        }
        else {
            this.props.setGridParams(params);
            this.props.getGridData(params);
        }
    }

    handleOpenCreatePopup = () => this.props.setCreateTypePopup({ show: true })

    render() {
        const { isHost } = this.props;

        return (
            <Form className="form-filter" onSubmit={this.submitHandler}>
                <Form.Row bsPrefix='form-row'>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('NotificationType')}</Form.Label>
                        <Form.Control type="text" size="sm" className="shadow-none" autoComplete="off"
                            value={this.state.notificationTypeName}
                            onChange={this.handleNotificationTypeChange}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('RecipientType')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                initialValue={this.state.filter.recipientTypes.value}
                                dataSource={this.state.recipientTypesOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={this.recipientTypeChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('ChatType')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                initialValue={this.state.filter.chatTypes.value}
                                dataSource={this.state.chatTypesOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={this.chatTypeChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('System')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                initialValue={this.state.filter.systems.value}
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