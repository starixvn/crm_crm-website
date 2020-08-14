import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import DatePicker from '../../../components/date-picker/index';
import { handleNewResendOptions } from '../functions';

class Filter extends Component {
    constructor(props) {
        super(props);
        const now = new Date();
        const minDate = new Date();
        minDate.setMonth(minDate.getMonth() - 1);

        this.state = {
            filter: {
                chatTypes: {
                    value: ''
                },
                statuses: {
                    value: ''
                },
                fromDate: now,
                toDate: now,
                headline: '',
                recipient: '',
                notificationIDFrom: '',
                notificationIDTo: '',
                messageID: '',
                content: ''
            },
            minDate: minDate,
            maxDate: now,
            chatTypeOptions: [],
            statusOptions: []
        };
    }
    
    componentDidUpdate = (nextProps, prevState) => {
        if(nextProps.settings !== this.props.settings) {
            // chat type
            const chatTypeIds = [];
            const chatTypeOptions = this.props.settings.chatTypes.map(x => {
                chatTypeIds.push(x.id);
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
                chatTypeOptions,
                statusOptions,
                filter: {
                    ...prevState.filter,
                    ...{
                        chatTypes: {
                            value: chatTypeIds.toString()
                        },
                        statuses: {
                            value: statuses.toString()
                        }
                    }
                }
            }), () => {
                const params = this.getParams();
                this.props.setGridParams(params);
                this.props.getGridData(params, data => handleNewResendOptions(data, this.props.setResendOptions));
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

    handleChatTypeChange = value => this.handleFilterChange('chatTypes', value)

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
            notificationTypeIds: this.state.filter.chatTypes.value,
            fromDate: this.state.filter.fromDate,
            toDate: this.state.filter.toDate,
            headline: this.state.filter.headline,
            recipientName: this.state.filter.recipient,
            messageId: this.state.filter.messageID,
            notificationIdUpper: this.state.filter.notificationIDFrom,
            notificationIdLower: this.state.filter.notificationIDTo,
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
            this.props.getGridData(params, data => handleNewResendOptions(data, this.props.setResendOptions));
        }
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
                            width={182}
                            initialValue={this.state.filter.chatTypes.value}
                            data={this.state.chatTypeOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.handleChatTypeChange}
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
                    <div className='col-auto margin-right-50'>
                        <Form.Control as='input' size='sm' className='shadow-none' name='headline'
                            value={this.state.filter.headline}
                            onChange={this.handleChangeFilterControl}
                        />
                    </div>
                    <label className='col-auto col-form-label'>{LangHelper.getResource('Recipient')}</label>
                    <div className='col-auto margin-right-50'>
                        <Form.Control as='input' size='sm' className='shadow-none' name='recipient'
                            value={this.state.filter.recipient}
                            onChange={this.handleChangeFilterControl}
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