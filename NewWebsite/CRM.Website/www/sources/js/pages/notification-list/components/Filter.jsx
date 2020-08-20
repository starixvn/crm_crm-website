import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import DatePicker from '../../../components/date-picker/index';
import { handleNewResendOptions } from '../functions';
import { getNowGMTAdd8 } from '../../../common/functions';
import SelectWithSearch from 'react-select';

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
                headline: null,
                recipient: null,
                notificationIdLower: '',
                notificationIdUpper: '',
                messageID: '',
                content: ''
            },
            maxDate: now,
            notificationTypeOptions: [],
            headlineOptions: [],
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

            // recipient
            const recipientOptions = this.props.settings.recipients.map(x => {
                return {
                    value: x.name,
                    label: x.name
                };
            });

            // headline
            const headlineOptions = this.props.settings.headlines.map(x => {
                return {
                    value: x.name,
                    label: x.name
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
                headlineOptions,
                recipientOptions,
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

    handleHeadlineChange = value => this.handleFilterChange('headline', value)

    handleRecipientChange = value => this.handleFilterChange('recipient', value)

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
            headline: this.state.filter.headline ? this.state.filter.headline.value : null,
            recipientName: this.state.filter.recipient ? this.state.filter.recipient.value : null,
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
                <Form.Row bsPrefix='form-row'>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('NotificationType')}</Form.Label>
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
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('From')}</Form.Label>
                        <div className='react-datepicker-wrapper-container'>
                            <DatePicker width={150} disabled={this.isDisabledDateParam()} maxDate={this.state.maxDate} selected={this.state.filter.fromDate} onChange={this.handleFromDateChange} />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('To')}</Form.Label>
                        <div className='react-datepicker-wrapper-container'>
                            <DatePicker disabled={this.isDisabledDateParam()}  maxDate={this.state.maxDate} selected={this.state.filter.toDate} onChange={this.handleToDateChange} />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('GroupHeadline')}</Form.Label>
                        <SelectWithSearch
                            className='basic-single custom-select-with-search'
                            isSearchable
                            isClearable
                            name='headline'
                            onChange={this.handleHeadlineChange}
                            options={this.state.headlineOptions}
                            value={this.state.filter.headline}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Recipient')}</Form.Label>
                        <SelectWithSearch
                            className='basic-single custom-select-with-search'
                            isSearchable
                            isClearable
                            name='recipient'
                            onChange={this.handleRecipientChange}
                            options={this.state.recipientOptions}
                            value={this.state.filter.recipient}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('MessageID')}</Form.Label>
                        <Form.Control as='input' type='text' pattern='[0-9]*' size='sm' className='shadow-none width-150' name='messageID'
                            value={this.state.filter.messageID}
                            onChange={this.handleChangeFilterControl}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('NotificationID')}</Form.Label>
                        <div className='flex-box'>
                            <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdLower ? 'invalid-input': 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIdLower'
                                value={this.state.filter.notificationIdLower}
                                onChange={this.handleChangeFilterControl}
                            />
                            <div className='notification-concate'>_</div>
                            <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdUpper ? 'invalid-input': 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIdUpper'
                                value={this.state.filter.notificationIdUpper}
                                onChange={this.handleChangeFilterControl}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Status')}</Form.Label>
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
                    </Form.Group>
                    <div>
                        <Button variant='primary' size='sm' onClick={this.submitHandler}>{LangHelper.getResource('Submit')}</Button>
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