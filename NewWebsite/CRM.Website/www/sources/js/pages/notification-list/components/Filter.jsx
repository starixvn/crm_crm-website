import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Col, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import DatePicker from '../../../components/date-picker/index'
import Axios from 'axios';
import Site from '@jsroot/common/site';

class Filter extends Component {
    constructor(props) {
        super(props);
        const now = new Date();
        const minDate = new Date();
        minDate.setMonth(minDate.getMonth() - 1);

        this.state = {
            filter: {
                notificationTypes: {
                    value: ''
                },
                groupHeadlines: {
                    value: ''
                },
                recipients: {
                    value: ''
                },
                statuses: {
                    value: ''
                },
                fromDate: now,
                toDate: now,
                notificationIDFrom: '',
                notificationIDTo: '',
                messageID: '',
                content: ''
            },
            minDate: minDate,
            maxDate: now,
            notificationTypeOptions: [],
            groupHeadlineOptions: [],
            recipientOptions: [],
            statusOptions: []
        };
    }

    componentDidMount() {
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params, this.setResendOptions);
        const promises = [
            Axios(Site.resolveClientUrl('NotificationList/GetNotificationTypes')),
            Axios(Site.resolveClientUrl('NotificationList/GetGroupHeadlines')),
            Axios(Site.resolveClientUrl('NotificationList/GetRecipients')),
            Axios(Site.resolveClientUrl('NotificationList/GetStatuses'))
        ]

        Promise.all(promises)
            .then(([notificationTypeResponse, groupHeadlinesResponse, recipientsResponse, statusesResponse]) => {
                // notification type
                const notificationTypeIds = [];
                const notificationTypeOptions = notificationTypeResponse.data.map(x => {
                    notificationTypeIds.push(x.id);
                    return {
                        id: x.id,
                        name: x.name,
                        checked: true
                    };
                });

                // group headline
                const groupHeadlineIds = [];
                const groupHeadlineOptions = groupHeadlinesResponse.data.map(x => {
                    groupHeadlineIds.push(x.id);
                    return {
                        id: x.id,
                        name: x.name,
                        checked: true
                    };
                });

                // recipient
                const recipientIds = [];
                const recipientOptions = recipientsResponse.data.map(x => {
                    recipientIds.push(x.id);
                    return {
                        id: x.id,
                        name: x.name,
                        checked: true
                    };
                });

                // status
                const statusIds = [];
                const statusOptions = statusesResponse.data.map(x => {
                    statusIds.push(x.id);
                    return {
                        id: x.id,
                        name: x.name,
                        checked: true
                    };
                });
        
                this.setState(prevState => ({
                    notificationTypeOptions,
                    groupHeadlineOptions,
                    recipientOptions,
                    statusOptions,
                    filter: {
                        ...prevState.filter,
                        ...{
                            notificationTypes: {
                                value: notificationTypeIds.toString()
                            },
                            groupHeadlines: {
                                value: groupHeadlineIds.toString()
                            },
                            recipients: {
                                value: recipientIds.toString()
                            },
                            statuses: {
                                value: recipientIds.toString()
                            }
                        }
                    }
                }), () => {
                        const params = this.getParams();
                        this.props.setGridParams(params);
                        this.props.getGridData(params);
                });
            }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    }

    handleFilterChange = (name, value) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [name]: value } }
        }));
    }

    handleNotificationTypeChange = value => this.handleFilterChange('notificationTypes', value)

    handleFromDateChange = value => this.handleFilterChange('fromDate', value)

    handleToDateChange = value => this.handleFilterChange('toDate', value)

    handleGroupHeadlineChange = value => this.handleFilterChange('groupHeadlines', value)

    handleRecipientChange = value => this.handleFilterChange('recipients', value)

    handleStatusChange = value => this.handleFilterChange('statuses', value)

    handleChangeFilterControl = e => {
        if (e.target.validity.valid) {
            this.handleFilterChange(e.target.name, e.target.value);
        }
    }

    handleChange = e => this.setState({ [e.target.name]: e.target.value })

    getParams = () => {
        return {
            notificationTypeIds: this.state.filter.notificationTypes.value,
            fromDate: this.state.filter.fromDate,
            toDate: this.state.filter.toDate,
            groupHeadlineIds: this.state.filter.groupHeadlines.value,
            recipientIds: this.state.filter.recipients.value,
            messageId: this.state.filter.messageID,
            notificationIdFrom: this.state.filter.notificationIDFrom,
            notificationIdTo: this.state.filter.notificationIDTo,
            content: this.state.filter.content,
            statuses: this.state.filter.statuses.value,
            pageIndex: this.props.gridParams.pageIndex,
            pageSize: this.props.gridParams.pageSize
        };
    }

    submitHandler = e => {
        e.preventDefault();
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    setResendOptions = data => {
        let dataList = data.map(item => {
            return {
                notificationId: item.notificationId,
                messageId: item.messageId,
                status: item.statusString,
                isResent: false
            }
        });

        this.props.setResendOptions({
            isCheckedAll: false,
            dataList
        });
    }

    isDisabledDateParam = () => {
        return this.state.filter.messageID !== '' || this.state.filter.notificationIDFrom !== '' || this.state.filter.notificationIDTo !== '';
    }

    render() {
        return (
            <Form className='form-filter'>
                <Form.Row bsPrefix='form-row'>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('NotificationType')}</label>
                    <div className='col-auto position-relative'>
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.notificationTypes.value}
                            data={this.state.notificationTypeOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.handleNotificationTypeChange}
                        />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('From')}</label>
                    <div className='col-auto'>
                        <DatePicker disabled={this.isDisabledDateParam()} minDate={this.state.minDate} maxDate={this.state.maxDate} selected={this.state.filter.fromDate} onChange={this.handleFromDateChange} />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('To')}</label>
                    <div className='col-auto'>
                        <DatePicker disabled={this.isDisabledDateParam()} minDate={this.state.minDate} maxDate={this.state.maxDate} selected={this.state.filter.toDate} onChange={this.handleToDateChange} />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('GroupHeadline')}</label>
                    <div className='col-auto position-relative'>
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.groupHeadlines.value}
                            data={this.state.groupHeadlineOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isSearchable={true}
                            isAllTextShown
                            onChange={this.handleGroupHeadlineChange}
                        />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('Recipient')}</label>
                    <div className='col-auto position-relative'>
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.recipients.value}
                            data={this.state.recipientOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isSearchable={true}
                            isAllTextShown
                            onChange={this.handleRecipientChange}
                        />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('MessageID')}</label>
                    <div className='col-auto'>
                        <Form.Control as='input' type='text' pattern='[0-9]*' size='sm' className='shadow-none' name='messageID'
                            value={this.state.filter.messageID}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                </Form.Row>
                <Form.Row bsPrefix='form-row'>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('NotificationID')}</label>
                    <div className='col-auto'>
                        <Form.Control as='input' type='text' pattern='[0-9]*' size='sm' className='shadow-none' name='notificationIDFrom'
                            value={this.state.filter.notificationIDFrom}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <div className='col-auto'>-</div>
                    <div className='col-auto'>
                        <Form.Control as='input' type='text' pattern='[0-9]*' size='sm' className='shadow-none' name='notificationIDTo'
                            value={this.state.filter.notificationIDTo}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('Content')}</label>
                    <div className='col-auto'>
                        <Form.Control as='input' size='sm' className='shadow-none' name='content'
                            value={this.state.filter.content}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('Status')}</label>
                    <div className='col-auto position-relative'>
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.statuses.value}
                            data={this.state.statusOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.handleStatusChange}
                        />
                    </div>
                    <div className='col-auto'>
                        <Button className='mr-2' variant='primary' size='sm' onClick={this.submitHandler}>{LangHelper.getResource('Submit')}</Button>
                    </div>
                </Form.Row>
            </Form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        gridParams,
        setResendOptions
    } = state;

    return {
        gridParams,
        setResendOptions
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);