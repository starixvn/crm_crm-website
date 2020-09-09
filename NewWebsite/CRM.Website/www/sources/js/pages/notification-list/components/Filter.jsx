import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import DateTimeService from '@jsroot/common/dateTimeService';
import LangHelper from '@jsroot/common/langHelper';
import DatePicker from '@jsroot/components/date-picker/index';
import { handleNewResendOptions } from '../functions';
import SelectWithSearch from 'react-select';
import MultipleSelect from '@jsroot/components/multiple-select/index';
import Site from '@jsroot/common/site';
import { toast } from 'react-toastify';

export class Filter extends Component {
    constructor(props) {
        super(props);
        const now = new Date(DateTimeService.toShortDateFormat(DateTimeService.toClientDateTime(new Date())));

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
            statusOptions: []
        };
    }

    componentDidMount() {
        this.props.getSettings().then(() => {
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

            // status
            const statusOptions = this.props.settings.statuses.map(x => {
                return {
                    id: x,
                    name: LangHelper.getResource(x),
                    checked: true
                };
            });

            this.setState(prevState => ({
                notificationTypeOptions,
                statusOptions,
                filter: {
                    ...prevState.filter,
                    notificationTypes: notificationTypeIds.toString(),
                    statuses: this.props.settings.statuses.toString()
                }
            }), () => {
                const params = this.getParams();
                this.props.setGridParams(params);
                this.props.getGridData({ ...params }, data => handleNewResendOptions(data, this.props.setResendOptions));
            });
        });
    }

    handleFilterChange = (name, value) => {
        this.setState(prevState => ({
            filter: {
                ...prevState.filter,
                [name]: value
            }
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

    changeToDateAfterFromDateChanged = value => {
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

    handleChange = e => this.handleFilterChange(e.target.name, e.target.value)

    getParams = () => {
        return {
            notificationTypeIds: this.state.filter.notificationTypes,
            fromDate: DateTimeService.startOfDay(this.state.filter.fromDate),
            toDate: DateTimeService.endOfDay(this.state.filter.toDate),
            headline: this.state.filter.headline ? this.state.filter.headline.value : '',
            recipientName: this.state.filter.recipient ? this.state.filter.recipient.value : '',
            messageId: this.state.filter.messageID,
            notificationIdLower: this.state.filter.notificationIdLower,
            notificationIdUpper: this.state.filter.notificationIdUpper,
            content: this.state.filter.content,
            status: this.state.filter.statuses,
            pageIndex: this.props.gridParams.pageIndex,
            pageSize: this.props.gridParams.pageSize
        };
    }

    submitHandler = e => {
        e.preventDefault();
        this.props.setInvalidParams({});
        const params = this.getParams();
        if (this.isValidateParams(params)) {
            this.props.setGridParams(params);
            this.props.getGridData({ ...params }, data => handleNewResendOptions(data, this.props.setResendOptions));
        }
    }

    isDisabledDateParam = () =>
        this.state.filter.messageID !== '' || this.state.filter.notificationIdLower
        !== '' || this.state.filter.notificationIdUpper !== ''

    isValidateParams = params => {
        if (params.notificationTypeIds === '') {
            toast.error(`${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('NotificationType')}`);
            return false;
        }

        if (params.notificationIdLower !== '' && params.notificationIdUpper === '') {
            this.props.setInvalidParams({ isInvalidNotificationIdUpper: true });
            toast.error(`${LangHelper.getResource('NotificationIDMustHaveBoth')}`);
            return false;
        }

        if (params.notificationIdUpper !== '' && params.notificationIdLower === '') {
            this.props.setInvalidParams({ isInvalidNotificationIdLower: true });
            toast.error(`${LangHelper.getResource('NotificationIDMustHaveBoth')}`);
            return false;
        }

        if (parseInt(params.notificationIdLower, 10) > parseInt(params.notificationIdUpper, 10)) {
            toast.error(`${LangHelper.getResource('NotificationIDFromNotGreaterThanTo')}`);
            return false;
        }

        if (params.status === '') {
            toast.error(`${LangHelper.getResource('PleaseChoose')} a ${LangHelper.getResource('Status')}`);
            return false;
        }

        return true;
    }

    debounceHandleHeadlineInputChange = Site.debounce(searchText => {
        if (searchText.length >= 2) {
            this.props.getHeadlineOptions(searchText);
        }
        else {
            this.props.setHeadlineOptions([]);
        }
    })

    debounceHandleRecipientInputChange = Site.debounce(searchText => {
        if (searchText.length >= 2) {
            this.props.getRecipientOptions(searchText);
        }
        else {
            this.props.setRecipientOptions([]);
        }
    })

    render() {
        return (
            <Form className='form-filter'>
                <Form.Row bsPrefix='form-row'>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('NotificationType')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                id='notification-type'
                                dataSource={this.state.notificationTypeOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={(currentItem, selectedItemsKey) => this.handleNotificationTypeChange(currentItem, selectedItemsKey)}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('From')}</Form.Label>
                        <div className='react-datepicker-wrapper-container form-control-wrapper'>
                            <DatePicker disabled={this.isDisabledDateParam()}
                                maxDate={this.state.maxDate}
                                selected={this.state.filter.fromDate}
                                onChange={value => this.handleFromDateChange(value)}
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
                                onChange={value => this.handleToDateChange(value)}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('GroupHeadline')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <SelectWithSearch
                                className='basic-single'
                                name='headline'
                                onChange={value => this.handleHeadlineChange(value)}
                                options={this.props.headlineOptions}
                                onInputChange={searchText => this.debounceHandleHeadlineInputChange(searchText)}
                                value={this.state.filter.headline}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Recipient')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <SelectWithSearch
                                className='basic-single'
                                name='recipient'
                                onChange={value => this.handleRecipientChange(value)}
                                options={this.props.recipientOptions}
                                onInputChange={searchText => this.debounceHandleRecipientInputChange(searchText)}
                                value={this.state.filter.recipient}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('MessageID')}</Form.Label>
                        <Form.Control as='input' type='text' pattern='[0-9]*' size='sm' className='shadow-none' name='messageID'
                            value={this.state.filter.messageID}
                            onChange={e => this.handleChangeFilterControl(e)}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('NotificationID')}</Form.Label>
                        <div className='form-control-wrapper d-flex align-items-center notification-id-container'>
                            <Form.Control
                                className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdLower ?
                                    'invalid-input' : 'valid-input'} notification-id-input`}
                                as='input' type='text' pattern='[0-9]*' size='sm'
                                name='notificationIdLower'
                                value={this.state.filter.notificationIdLower}
                                onChange={e => this.handleChangeFilterControl(e)}
                            />
                            <div className='notification-concate text-center'>-</div>
                            <Form.Control
                                className={`shadow-none ${this.props.invalidParams.isInvalidNotificationIdUpper ?
                                    'invalid-input' : 'valid-input'} notification-id-input`}
                                as='input' type='text' pattern='[0-9]*' size='sm'
                                name='notificationIdUpper'
                                value={this.state.filter.notificationIdUpper}
                                onChange={e => this.handleChangeFilterControl(e)}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Content')}</Form.Label>
                        <Form.Control as='input' type='text' size='sm' className='shadow-none' name='content'
                            value={this.state.filter.content}
                            onChange={e => this.handleChange(e)}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('Status')}</Form.Label>
                        <div className='form-control-wrapper'>
                            <MultipleSelect
                                dataSource={this.state.statusOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isSummited={false}
                                isAllTextShown
                                onChange={(currentItem, selectedItemsKey) => this.handleStatusChange(currentItem, selectedItemsKey)}
                            />
                        </div>
                    </Form.Group>
                    <Form.Group>
                        <Button variant='primary' size='sm' onClick={e => this.submitHandler(e)}>
                            {LangHelper.getResource('Submit')}
                        </Button>
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
        invalidParams,
        settings,
        headlineOptions,
        recipientOptions
    } = state;

    return {
        gridParams,
        invalidParams,
        settings,
        headlineOptions,
        recipientOptions
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);