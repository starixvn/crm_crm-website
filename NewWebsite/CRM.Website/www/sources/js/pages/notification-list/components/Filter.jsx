import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import DatePicker from '../../../components/date-picker/index';
import { handleNewResendOptions } from '../functions';
import { getNowGMTAdd8 } from '../../../common/functions';
import SelectWithSearch from 'react-select';
import MultipleSelect from '../../../components/multiple-select/index';

class Filter extends Component {
    constructor(props) {
        super(props);
        const now = getNowGMTAdd8();

        this.state = {
            filter: {
                notificationTypes: '',
                statuses: '',
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
            minToDate: now,
            maxToDate: now,
            notificationTypeOptions: [],
            headlineOptions: [],
            recipientOptions: [],
            statusOptions: []
        };
    }

    componentDidUpdate = (nextProps, prevState) => {
        if (nextProps.settings !== this.props.settings) {
            // notification type
            const notificationTypeIds = [];
            const notificationTypeOptions = this.props.settings.notificationTypes.map(x => {
                notificationTypeIds.push(x.notificationTypeId);
                return {
                    id: x.notificationTypeId,
                    name: x.notificationTypeName,
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
                        notificationTypes: notificationTypeIds.toString(),
                        statuses: statuses.toString()
                    }
                }
            }), () => {
                const params = this.getParams();
                this.props.setGridParams(params);
                this.props.getGridData({ ...params }, data => handleNewResendOptions(data, this.props.setResendOptions));
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

    handleNotificationTypeChange = (currentItem, selectedItemsKey) => this.handleFilterChange('notificationTypes', selectedItemsKey)

    handleFromDateChange = value => {
        const toDate = new Date(value);
        toDate.setMonth(toDate.getMonth() + 1);
        toDate.setDate(toDate.getDate() - 1);
        this.setState({ minToDate: value });
        this.handleFilterChange('fromDate', value);
        if (toDate - this.state.maxDate < 0) {
            this.setState({ maxToDate: toDate }, () => this.changeToDateAfterFromDateChanged(value));
        }
        else {
            this.changeToDateAfterFromDateChanged(value);
        }
    }

    changeToDateAfterFromDateChanged = (value) => {
        if (this.state.filter.toDate - value < 0) {
            this.handleFilterChange('toDate', value);
        }
        else if (this.state.filter.toDate - this.state.maxToDate > 0) {
            this.handleFilterChange('toDate', this.state.maxToDate);
        }
    }

    handleToDateChange = value => this.handleFilterChange('toDate', value)

    handleHeadlineChange = value => this.handleFilterChange('headline', value)

    handleRecipientChange = value => this.handleFilterChange('recipient', value)

    handleStatusChange = (currentItem, selectedItemsKey) => this.handleFilterChange('statuses', selectedItemsKey)

    handleChangeFilterControl = e => {
        if (e.target.validity.valid) {
            this.handleFilterChange(e.target.name, e.target.value);
        }
    }

    handleChange = e => this.setState({ [e.target.name]: e.target.value })

    getParams = () => {
        return {
            notificationTypeIds: this.state.filter.notificationTypes,
            fromDate: this.state.filter.fromDate,
            toDate: this.state.filter.toDate,
            headline: this.state.filter.headline ? this.state.filter.headline.value : '',
            recipientName: this.state.filter.recipient ? this.state.filter.recipient.value : '',
            messageId: this.state.filter.messageID,
            notificationIdUpper: this.state.filter.notificationIdLower,
            notificationIdLower: this.state.filter.notificationIdUpper,
            content: this.state.filter.content,
            status: this.state.filter.statuses,
            pageIndex: this.props.gridParams.pageIndex,
            pageSize: this.props.gridParams.pageSize
        };
    }

    submitHandler = e => {
        e.preventDefault();
        this.props.setMessageResult({ show: false });
        this.props.setInvalidParams({ show: false });

        const params = this.getParams();
        if (this.isValidateParams(params)) {
            this.props.setGridParams(params);
            this.props.getGridData({ ...params }, data => handleNewResendOptions(data, this.props.setResendOptions));
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
            params.fromDate.setHours(0, 0, 0, 0);
            params.toDate.setHours(0, 0, 0, 0);

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
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                initialValue={this.state.filter.notificationTypes}
                                dataSource={this.state.notificationTypeOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={this.handleNotificationTypeChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('From')}</Form.Label>
                        <div className='react-datepicker-wrapper-container form-control-wrapper'>
                            <DatePicker disabled={this.isDisabledDateParam()}
                                maxDate={this.state.maxDate}
                                selected={this.state.filter.fromDate}
                                onChange={this.handleFromDateChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('To')}</Form.Label>
                        <div className='react-datepicker-wrapper-container form-control-wrapper'>
                            <DatePicker disabled={this.isDisabledDateParam()}
                                minDate={this.state.minToDate}
                                maxDate={this.state.maxToDate}
                                selected={this.state.filter.toDate}
                                onChange={this.handleToDateChange}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('GroupHeadline')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <SelectWithSearch
                                className='basic-single'
                                isSearchable
                                isClearable
                                name='headline'
                                onChange={this.handleHeadlineChange}
                                options={this.state.headlineOptions}
                                value={this.state.filter.headline}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Recipient')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <SelectWithSearch
                                className='basic-single'
                                isSearchable
                                isClearable
                                name='recipient'
                                onChange={this.handleRecipientChange}
                                options={this.state.recipientOptions}
                                value={this.state.filter.recipient}
                            />
                        </div>
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
                        <div className='form-control-wrapper d-flex align-items-center notification-id-container'>
                            <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdLower ? 'invalid-input' : 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIdLower'
                                value={this.state.filter.notificationIdLower}
                                onChange={this.handleChangeFilterControl}
                            />
                            <div className='notification-concate text-center'>-</div>
                            <Form.Control className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdUpper ? 'invalid-input' : 'valid-input'} notification-id-input`} as='input' type='text' pattern='[0-9]*' size='sm' name='notificationIdUpper'
                                value={this.state.filter.notificationIdUpper}
                                onChange={this.handleChangeFilterControl}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Status')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                initialValue={this.state.filter.statuses}
                                dataSource={this.state.statusOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={this.handleStatusChange}
                            />
                        </div>
                    </Form.Group>
                    <div>
                        <Button variant='primary' size='sm' onClick={this.submitHandler}>
                            {LangHelper.getResource('Submit')}
                        </Button>
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
        invalidParams,
        settings
    } = state;

    return {
        gridParams,
        invalidParams,
        settings
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);