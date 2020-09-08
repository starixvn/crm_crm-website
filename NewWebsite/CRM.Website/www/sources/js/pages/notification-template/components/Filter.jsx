import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions';
import LangHelper from '@jsroot/common/langHelper';
import { renderNotificationTypeOption } from '../common.functions';

export class Filter extends Component {
    static propTypes = {
        notificationTypes: PropTypes.arrayOf(PropTypes.object)
    };

    static defaultProps = {
        notificationTypes: []
    };

    constructor(props) {
        super(props);
        this.state = {
            notificationTypeId: '',
            notificationTemplateName: ''
        };
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
            pageSize: 100
        };
    }

    handleSubmit = event => {
        event.preventDefault();
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    handleOpenCreatePopup = () => this.props.setCreateTemplatePopup({ show: true })

    render() {
        return (
            <Form className="form-filter" onSubmit={this.handleSubmit}>
                <Form.Row bsPrefix='form-row'>
                    <Form.Group>
                        <Form.Label htmlFor="filterNotificationTypeId">{LangHelper.getResource('NotificationType')}</Form.Label>
                        <Form.Control as="select" size="sm" className="shadow-none" id="filterNotificationTypeId"
                            value={this.state.notificationTypeId}
                            onChange={this.handleNotificationTypeChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            {this.props.notificationTypes.map(renderNotificationTypeOption)}
                        </Form.Control>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label htmlFor="filterNotificationTemplateName">{LangHelper.getResource('TemplateName')}</Form.Label>
                        <Form.Control type="text" size="sm" className="shadow-none"
                            id="filterNotificationTemplateName" autoComplete="off"
                            value={this.state.notificationTemplateName}
                            onChange={this.handleTemplateNameChange}
                        />
                    </Form.Group>
                    <Form.Group>
                        <Button className="mr-2" variant="primary" size="sm"
                            onClick={this.handleSubmit}
                        >
                            {LangHelper.getResource('Submit')}
                        </Button>
                        {
                            this.props.isHost &&
                            <Button variant="primary" size="sm"
                                onClick={this.handleOpenCreatePopup}
                            >
                                {LangHelper.getResource('Insert')}
                            </Button>
                        }
                    </Form.Group>
                </Form.Row>
            </Form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { notificationTypes } = state;
    return { notificationTypes };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);