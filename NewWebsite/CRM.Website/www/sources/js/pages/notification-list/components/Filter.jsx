import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import DatePicker from '../../../components/date-picker/index';
import { handleNewResendOptions } from '../functions';
import { getNowGMTAdd8, convertDateToString } from '../../../common/functions';

class Filter extends Component {
    constructor(props) {
        super(props);
        const now = getNowGMTAdd8();

        this.state = {
            filter: {
                notificationTypes: {
                    value: ''
                },
                statuses: {
                    value: ''
                },
                fromDate: now,
                toDate: now,
                headline: '',
                recipient: '',
                notificationIdLower: '',
                notificationIdUpper: '',
                messageID: '',
                content: ''
            },
            maxDate: now,
            notificationTypeOptions: [],
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
                statusOptions,
                filter: {
                    ...prevState.filter,
                    ...{
                        notificationTypes: {
                            value: notificationTypeIds.toString()
                        },
                        statuses: {
                            value: statuses.toString()
                        }
                    }
                }
            }), () => {
                const params = this.getParams();
                this.props.setGridParams(params);
                this.props.getGridData({...params}, data => handleNewResendOptions(data, this.props.setResendOptions));
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
            headline: this.state.filter.headline,
            recipientName: this.state.filter.recipient,
            messageId: this.state.filter.messageID,
            notificationIdUpper: this.state.filter.notificationIdLower,
            notificationIdLower: this.state.filter.notificationIdUpper,
            content: this.state.filter.content,
            status: this.state.filter.statuses.value,
            pageIndex: this.props.gridParams.pageIndex,
            pageSize: this.props.gridParams.pageSize
        };
    }

    submitHandler = e => {
        e.preventDefault();
        this.props.setInvalidParams({
            show: false,
            isInvalidNotificationIdLower: false,
            isInvalidNotificationIdUpper: false
        });

        const params = this.getParams();
        if (this.isValidateParams(params)) {
            this.props.setGridParams(params);
            this.props.getGridData({...params}, data => handleNewResendOptions(data, this.props.setResendOptions));
        }
    }

    isDisabledDateParam = () => {
        return this.state.filter.messageID !== '' || this.state.filter.notificationIdLower !== '' || this.state.filter.notificationIdUpper !== '';
    }

    isValidateParams = (params) => {
        if (params.notificationTypeIds === '') {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('NotificationType')}`
            });

            return false;
        }
    
        if (params.notificationIdLower !== '') {
            if (params.notificationIdUpper === '') {
                this.props.setInvalidParams({
                    show: true,
                    message: `${LangHelper.getResource('NotificationIDMustHaveBoth')}`,
                    isInvalidNotificationIdUpper: true
                })
                
                return false;
            }
        }

        if (params.notificationIdUpper !== '') {
            if (params.notificationIdLower === '') {
                this.props.setInvalidParams({
                    show: true,
                    message: `${LangHelper.getResource('NotificationIDMustHaveBoth')}`,
                    isInvalidNotificationIdLower: true
                })
                
                return false;
            }
        }

        if (parseInt(params.notificationIdLower) > parseInt(params.notificationIdUpper)) {
            this.props.setInvalidParams({
                show: true,
                message: `${LangHelper.getResource('NotificationIDFromNotGreaterThanTo')}`
            })
            
            return false;
        }

        if (params.messageId === '' && params.notificationIdLower === '') {
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

        if (params.status === '') {
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
                            width={150}
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
                        <DatePicker width={150} disabled={this.isDisabledDateParam()} maxDate={this.state.maxDate} selected={this.state.filter.fromDate} onChange={this.handleFromDateChange} />
                    </div>
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('To')}</label>
                    <div className='col-auto margin-right-50'>
                        <DatePicker disabled={this.isDisabledDateParam()}  maxDate={this.state.maxDate} selected={this.state.filter.toDate} onChange={this.handleToDateChange} />
                    </div>
                    <div className='flex-box'>
                        <label className='label-in-div col-form-label'>{LangHelper.getResource('GroupHeadline')}</label>
                        <div className='col-auto margin-right-50'>
                            <Form.Control as='input' size='sm' className='shadow-none width-150' name='headline'
                                value={this.state.filter.headline}
                                onChange={this.handleChangeFilterControl}
                            />
                        </div>
                    </div>
                    <div className='flex-box'>
                        <label className='label-in-div col-form-label'>{LangHelper.getResource('Recipient')}</label>
                        <div className='col-auto margin-right-50'>
                            <Form.Control as='input' size='sm' className='shadow-none width-150' name='recipient'
                                value={this.state.filter.recipient}
                                onChange={this.handleChangeFilterControl}
                            />
                        </div>
                    </div>
                    <div className='flex-box'>
                        <label className='label-in-div col-form-label'>{LangHelper.getResource('MessageID')}</label>
                        <div className='col-auto'>
                            <Form.Control as='input' type='text' pattern='[0-9]*' size='sm' className='shadow-none width-150' name='messageID'
                                value={this.state.filter.messageID}
                                onChange={this.handleChangeFilterControl}
                            />
                        </div>
                    </div>
                </Form.Row>
                <Form.Row bsPrefix='form-row'>
                    <label className='col-auto col-form-label width-120'>{LangHelper.getResource('NotificationID')}</label>
                    <div className='col-auto'>
                        <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdLower ? 'invalid-input': 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIdLower'
                            value={this.state.filter.notificationIdLower}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <div className='col-auto notification-concate'>_</div>
                    <div className='col-auto margin-right-50'>
                        <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdUpper ? 'invalid-input': 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIdUpper'
                            value={this.state.filter.notificationIdUpper}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('Content')}</label>
                    <div className='col-auto margin-right-50'>
                        <Form.Control as='input' size='sm' className='shadow-none width-150' name='content'
                            value={this.state.filter.content}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label width-70'>{LangHelper.getResource('Status')}</label>
                    <div className='col-auto position-relative margin-right-50'>
                        <MultipleSelectField
                            width={150}
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