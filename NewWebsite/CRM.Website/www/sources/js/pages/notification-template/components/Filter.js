import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            notificationTypeId: '',
            templateName: ''
        };
    }

    componentDidMount() {
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    handleNotificationTypeChange = event => {
        this.setState({
            notificationTypeId: event.target.value
        });
    }

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

    handleOpenCreatePopup = () => this.props.setCreateTemplatePopup({ show: true })

    render() {
        const { isHost } = this.props;
        return (
            <Form className="form-filter">
                <Form.Row bsPrefix="form-row">
                    <label className="col-auto col-form-label">{LangHelper.getResource('NotificationType')}</label>
                    <div className="col-auto">
                        <Form.Control as="select" size="sm" className="shadow-none"
                            value={this.state.notificationTypeId}
                            onChange={this.handleNotificationTypeChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                        </Form.Control>
                    </div>
                    <label className="col-auto col-form-label">{LangHelper.getResource('TemplateName')}</label>
                    <div className="col-auto">
                        <Form.Control type="text" size="sm" className="shadow-none"
                            value={this.state.templateName}
                            onChange={this.handleTemplateNameChange}
                        />
                    </div>
                    <div className="col-auto">
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
                    </div>
                </Form.Row>
            </Form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { notificationTypes, gridParams, createTemplatePopup } = state;
    return {
        notificationTypes,
        gridParams,
        createTemplatePopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);