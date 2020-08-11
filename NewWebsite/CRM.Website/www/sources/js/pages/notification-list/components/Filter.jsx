import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import DatePicker from '../../../components/date-picker/index';

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
    
    componentDidUpdate = (nextProps, prevState) => {
        if(nextProps.settings !== this.props.settings) {
            // notification type
            const notificationTypeIds = [];
            const notificationTypeOptions = this.props.settings.notificationTypes.map(x => {
                notificationTypeIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });
    
            // group headline
            const groupHeadlineIds = [];
            const groupHeadlineOptions = this.props.settings.groupHeadlines.map(x => {
                groupHeadlineIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });
    
            // recipient
            const recipientIds = [];
            const recipientOptions = this.props.settings.recipients.map(x => {
                recipientIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });
    
            // status
            const statuses = [];
            const statusOptions = this.props.settings.statuses.map(x => {
                statuses.push(x);
                return {
                    id: x,
                    name: x,
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
                            value: statuses.toString()
                        }
                    }
                }
            }), () => {
                const params = this.getParams();
                this.props.setGridParams(params);
                this.props.getGridData(params);
            });
        }
    }

    componentDidMount() {
        this.props.getSettings();
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
        this.props.setInvalidParams({
            show: false,
            isInvalidNotificationIDFrom: false,
            isInvalidNotificationIDTo: false
        });

        const params = this.getParams();
        if (this.isValidateParams(params)) {
            if (params.messageId !== '' || params.notificationIdFrom !== '') {
                params.fromDate = null;
                params.toDate = null;
            }

            this.props.setGridParams(params);
            this.props.getGridData(params);
        }
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

    isValidateParams = (params) => {
        if (params.notificationTypeIds === '') {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('NotificationType')}`
            });

            return false;
        }
    
        if (params.notificationIdFrom !== '') {
            if (params.notificationIdTo === '') {
                this.props.setInvalidParams({
                    show: true,
                    message: `${LangHelper.getResource('NotificationIDMustHaveBoth')}`,
                    isInvalidNotificationIDTo: true
                })
                
                return false;
            }
        }

        if (params.notificationIdTo !== '') {
            if (params.notificationIdFrom === '') {
                this.props.setInvalidParams({
                    show: true,
                    message: `${LangHelper.getResource('NotificationIDMustHaveBoth')}`,
                    isInvalidNotificationIDFrom: true
                })
                
                return false;
            }
        }

        if (parseInt(params.notificationIdFrom) > parseInt(params.notificationIdTo)) {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('NotificationIDFromNotGreaterThanTo')}`
            })
            
            return false;
        }

        if (params.messageId === '' && params.notificationIdFrom === '') {
            params.fromDate.setHours(0,0,0,0);
            params.toDate.setHours(0,0,0,0);
            
            if (params.toDate - params.fromDate < 0) {
                this.props.setInvalidParams({
                    show: true,
                    message: LangHelper.getResource('ToDateIsEarlierThanFromDate')
                });
    
                return false;
            }
        }

        if (params.groupHeadlineIds === '') {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('GroupHeadline')}`
            });

            return false;
        }

        if (params.recipientIds === '') {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('Recipient')}`
            });

            return false;
        }

        if (params.statuses === '') {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('Status')}`
            });

            return false;
        }

        return true;
    }

    render() {
        return (
            <Form className='form-filter'>
                <Form.Row bsPrefix='form-row' className='margin-bottom-10'>
                    <label className='col-auto col-form-label width-120'>{LangHelper.getResource('NotificationType')}</label>
                    <div className='col-auto position-relative margin-right-50'>
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
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('From')}</label>
                    <div className='col-auto margin-right-50'>
                        <DatePicker disabled={this.isDisabledDateParam()} minDate={this.state.minDate} maxDate={this.state.maxDate} selected={this.state.filter.fromDate} onChange={this.handleFromDateChange} />
                    </div>
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('To')}</label>
                    <div className='col-auto margin-right-50'>
                        <DatePicker disabled={this.isDisabledDateParam()} minDate={this.state.minDate} maxDate={this.state.maxDate} selected={this.state.filter.toDate} onChange={this.handleToDateChange} />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('GroupHeadline')}</label>
                    <div className='col-auto position-relative margin-right-50'>
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
                    <div className='col-auto position-relative margin-right-50'>
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
                    <label className='col-auto col-form-label width-120'>{LangHelper.getResource('NotificationID')}</label>
                    <div className='col-auto'>
                        <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIDFrom ? 'invalid-input': 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIDFrom'
                            value={this.state.filter.notificationIDFrom}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <div className='col-auto notification-concate'>-</div>
                    <div className='col-auto margin-right-50'>
                        <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIDTo ? 'invalid-input': 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIDTo'
                            value={this.state.filter.notificationIDTo}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('Content')}</label>
                    <div className='col-auto margin-right-50'>
                        <Form.Control as='input' size='sm' className='shadow-none' name='content'
                            value={this.state.filter.content}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('Status')}</label>
                    <div className='col-auto position-relative margin-right-50'>
                        <MultipleSelectField
                            width={182}
                            initialValue={this.state.filter.statuses.value}
                            data={this.state.statusOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            style={{ width: 150 }}
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
        setResendOptions,
        invalidParams,
        setInvalidParams,
        settings,
        getSettings
    } = state;

    return {
        gridParams,
        setResendOptions,
        invalidParams,
        setInvalidParams,
        settings,
        getSettings
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);