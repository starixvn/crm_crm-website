import React, { Component } from 'react';
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

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            filter: {
                notificationTypes: {
                    value: ''
                },
                notificationTypeOptions: [],
                fromDate: new Date()
            },
            notificationTypeId: '',
            templateName: ''
        };
    }

    componentDidMount() {
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
        const notificationTypeIds = [];
        const notificationTypeOptions = fakeNotificationType.map(x => {
            notificationTypeIds.push(x.notificationTypeID);
            return {
                id: x.notificationTypeID,
                name: x.notificationTypeName,
                checked: true
            };
        });

        this.setState(prevState => ({
            notificationTypeOptions,
            filter: {
                ...prevState.filter,
                ...{
                    notificationTypes: {
                        value: notificationTypeIds.toString()
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

    handleTemplateNameChange = event => {
        this.setState({
            templateName: event.target.value
        });
    }

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
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    renderNotificationTypeOption = item => {
        return (
            <option key={item.notificationTypeId} value={item.notificationTypeId}>
                {item.name}
            </option>
        );
    }

    handleFromDateChange = value => this.handleFilterChange('fromDate', value)

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
                    <label className='col-auto col-form-label'>{LangHelper.getResource('TemplateName')}</label>
                    <div className='col-auto'>
                        <DatePicker selected={this.state.filter.fromDate} onChange={this.handleFromDateChange} />
                    </div>
                    <div className='col-auto'>
                        <Button className='mr-2' variant='primary' size='sm'
                            onClick={this.submitHandler}>{LangHelper.getResource('Submit')}</Button>
                        <Button variant='primary' size='sm'>{LangHelper.getResource('Insert')}</Button>
                    </div>
                </Form.Row>
            </Form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { notificationTypes, gridParams } = state;
    return {
        notificationTypes,
        gridParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);