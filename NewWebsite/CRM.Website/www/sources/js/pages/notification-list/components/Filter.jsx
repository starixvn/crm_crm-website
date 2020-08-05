import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Col, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import DatePicker from '../../../components/date-picker/index'

const fakeNotificationType = [
    { notificationTypeID: 1, notificationTypeName: 'Group'},
    { notificationTypeID: 2, notificationTypeName: 'Personal'}
];

const fakeSender = [
    { id: 1, name: 'Crm User1'},
    { id: 2, name: 'Crm User2'},
    { id: 3, name: 'Crm User3'},
    { id: 4, name: 'Crm User4'},
];

const fakeGroupHeadline = [
    { id: 1, name: 'Group Headline1'},
    { id: 2, name: 'Group Headline2'},
    { id: 3, name: 'Group Headline3'},
    { id: 4, name: 'Group Headline4'},
];

const fakeRecipient = [
    { id: 1, name: 'Recipient1'},
    { id: 2, name: 'Recipient2'},
    { id: 3, name: 'Recipient3'},
    { id: 4, name: 'Recipient4'},
];

const notificationIDRangeType = [
    { id: 1, name: 'Between'},
    { id: 2, name: 'Greater than'},
    { id: 3, name: 'Less than'},
    { id: 4, name: 'Equal'}
];

const fakeStatus = [
    { id: 1, name: 'Send'},
    { id: 2, name: 'Error'},
    { id: 3, name: 'Resolved'},
    { id: 4, name: 'Sending'},
    { id: 5, name: 'Draft'}
];

class Filter extends Component {
    constructor(props) {
        super(props);

        const toDate = new Date();
        const fromDate = new Date();
        fromDate.setDate(fromDate.getDate() - 7);

        this.state = {
            filter: {
                notificationTypes: {
                    value: ''
                },
                senders: {
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
                fromDate: fromDate,
                toDate: toDate,
                notificationIDFrom: '',
                notificationIDTo: '',
                messageID: ''
            },
            notificationIDRangeType: 1,
            notificationTypeOptions: [],
            senderOptions: [],
            groupHeadlineOptions: [],
            recipientOptions: [],
            statusOptions: []
        };
    }

    componentDidMount() {
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);

        // notification type
        const notificationTypeIds = [];
        const notificationTypeOptions = fakeNotificationType.map(x => {
            notificationTypeIds.push(x.notificationTypeID);
            return {
                id: x.notificationTypeID,
                name: x.notificationTypeName,
                checked: true
            };
        });

        // sender
        const senderIds = [];
        const senderOptions = fakeSender.map(x => {
            senderIds.push(x.id);
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });

        // sender
        const groupHeadlineIds = [];
        const groupHeadlineOptions = fakeGroupHeadline.map(x => {
            groupHeadlineIds.push(x.id);
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });

        // recipient
        const recipientIds = [];
        const recipientOptions = fakeRecipient.map(x => {
            recipientIds.push(x.id);
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });

        // status
        const statusIds = [];
        const statusOptions = fakeStatus.map(x => {
            statusIds.push(x.id);
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });

        this.setState(prevState => ({
            notificationTypeOptions,
            senderOptions,
            groupHeadlineOptions,
            recipientOptions,
            statusOptions,
            filter: {
                ...prevState.filter,
                ...{
                    notificationTypes: {
                        value: notificationTypeIds.toString()
                    },
                    senders: {
                        value: senderIds.toString()
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
                const pageIndex = 1;
                // this.getNotifications(pageIndex);
        });
    }

    handleFilterChange = (name, value) => {
        this.setState(prevState => ({
            filter: { ...prevState.filter, ...{ [name]: value } }
        }));
    }

    handleNotificationTypeChange = value => this.handleFilterChange('notificationTypes', value)

    handleFromDateChange = value => this.handleFilterChange('fromDate', value)

    handleToDateChange = value => this.handleFilterChange('toDate', value)

    handleSenderChange = value => this.handleFilterChange('senders', value)

    handleGroupHeadlineChange = value => this.handleFilterChange('groupHeadlines', value)

    handleRecipientChange = value => this.handleFilterChange('recipients', value)

    handleStatusChange = value => this.handleFilterChange('statuses', value)

    renderNotificationIDRangeTypeOption = item => {
        return (
            <option key={item.id} value={item.id}>
                {item.name}
            </option>
        );
    }

    handleChangeFilterControl = e => this.handleFilterChange(e.target.name, e.target.value)

    handleChange = e => this.setState({ [e.target.name]: e.target.value })

    getParams = () => {
        return {
            notificationTypeId: parseInt(this.state.notificationTypeId, 10),
            templateName: this.state.templateName,
            pageIndex: 1,
            pageSize: 50
        };
    }

    submitHandler = e => {
        e.preventDefault();
        // const params = this.getParams();
        // this.props.setGridParams(params);
        // this.props.getGridData(params);
        console.log(this.state.filter)
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
                        <DatePicker selected={this.state.filter.fromDate} onChange={this.handleFromDateChange} />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('To')}</label>
                    <div className='col-auto'>
                        <DatePicker selected={this.state.filter.toDate} onChange={this.handleToDateChange} />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('Sender')}</label>
                    <div className='col-auto position-relative'>
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.senders.value}
                            data={this.state.senderOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.handleSenderChange}
                        />
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
                            onChange={this.handleSenderChange}
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
                </Form.Row>
                <Form.Row bsPrefix='form-row'>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('NotificationID')}</label>
                    <div className='col-auto'>
                        <Form.Control as='select' size='sm' className='shadow-none' name='notificationIDRangeType'
                            value={this.state.notificationIDRangeType}
                            onChange={this.handleChange}
                        >
                            {notificationIDRangeType.map(this.renderNotificationIDRangeTypeOption)}
                        </Form.Control>
                    </div>
                    <div className='col-auto'>
                        <Form.Control as='input' size='sm' className='shadow-none' name='notificationIDFrom'
                            value={this.state.filter.notificationIDFrom}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    {
                        parseInt(this.state.notificationIDRangeType) === 1 ? <Fragment>
                            <div className='col-auto'>-</div>
                            <div className='col-auto'>
                                <Form.Control as='input' size='sm' className='shadow-none' name='notificationIDTo'
                                    value={this.state.filter.notificationIDTo}
                                    onChange={this.handleChangeFilterControl}
                                />
                            </div>
                        </Fragment> : null
                    }
                    <label className='col-auto col-form-label'>{LangHelper.getResource('MessageID')}</label>
                    <div className='col-auto'>
                        <Form.Control as='input' size='sm' className='shadow-none' name='messageID'
                            value={this.state.filter.messageID}
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
        gridParams
    } = state;

    return {
        gridParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);