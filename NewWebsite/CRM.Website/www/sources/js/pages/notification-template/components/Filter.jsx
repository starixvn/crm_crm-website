import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions';
import LangHelper from '@jsroot/common/langHelper';

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            notificationTypeId: '',
            notificationTemplateName: ''
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
            notificationTemplateName: event.target.value
        });
    }

    getParams = () => {
        return {
            notificationTypeId: parseInt(this.state.notificationTypeId, 10),
            notificationTemplateName: this.state.notificationTemplateName,
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
                {item.notificationTypeName}
            </option>
        );
    }

    handleOpenCreatePopup = () => this.props.setCreateTemplatePopup({ show: true })

    render() {
        const { isHost } = this.props;
        return (
            <Form className="form-filter">
                <div className="form-row">
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('NotificationType')}</Form.Label>
                        <Form.Control as="select" size="sm" className="shadow-none"
                            value={this.state.notificationTypeId}
                            onChange={this.handleNotificationTypeChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                        </Form.Control>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>{LangHelper.getResource('TemplateName')}</Form.Label>
                        <Form.Control type="text" size="sm" className="shadow-none"
                            value={this.state.notificationTemplateName}
                            onChange={this.handleTemplateNameChange}
                        />
                    </Form.Group>
                    <div>
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
                </div>
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