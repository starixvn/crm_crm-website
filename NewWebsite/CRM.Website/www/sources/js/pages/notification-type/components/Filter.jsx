import React, { Component } from 'react';
import { Button,Form } from 'react-bootstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';

class Filter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            notificationType: '',
            chatTypeId: '',
            recipientId: '',
            systemId: ''
        };
               
    }

    componentDidMount() {
        const params = this.getParams();
        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    handleChatTypeChange = event => {
        this.setState({
            chatTypeId: event.target.value
        });
    }

    handleRecipientTypeChange = event => {
        this.setState({
            recipientTypeId: event.target.value
        });
    }

    handleSystemChange = event => {
        this.setState({
            systemId: event.target.value
        });
    }

    handleNotificationTypeChange = event => {
        this.setState({
            notificationType: event.target.value
        });
    }

    getParams = () => {
        return {
            notificationType: this.state.notificationType,
            chatTypeId: parseInt(this.state.chatTypeId, 10),
            recipientTypeId: parseInt(this.state.recipientTypeId, 10),
            systemId: parseInt(this.state.systemTypeId, 10),
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

    handleOpenCreatePopup = () => this.props.setCreateTypePopup({ show: true })

    render() {
        const { isHost } = this.props;
        return (
            <form className="form-filter">
                <div className="form-row"> 
                    <label className="col-auto col-form-label">{LangHelper.getResource('NotificationType')}</label>
                    <div className="col-auto">
                        <input type="text" className="form-control form-control-sm shadow-none"
                            value={this.state.notificationType}
                            onChange={this.handleNotificationTypeChange}
                        />
                    </div>                

                    <label className="col-auto col-form-label">{LangHelper.getResource('RecipientType')}</label>
                    <div className="col-auto">
                        <select className="form-control form-control-sm shadow-none"
                            value={this.state.recipientTypeId}
                            onChange={this.handleRecipientTypeChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            <option value="">{LangHelper.getResource('PIC of headline')}</option>  
                        </select>
                    </div>

                    <label className="col-auto col-form-label">{LangHelper.getResource('ChatType')}</label>
                    <div className="col-auto">
                        <select className="form-control form-control-sm shadow-none"
                            value={this.state.chatTypeId}
                            onChange={this.handleChatTypeChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            <option value="">{LangHelper.getResource('Personal')}</option>
                            <option value="">{LangHelper.getResource('Group')}</option>
                        </select>
                    </div>

                    <label className="col-auto col-form-label">{LangHelper.getResource('System')}</label>
                    <div className="col-auto">
                        <select className="form-control form-control-sm shadow-none"
                            value={this.state.systemTypeId}
                            onChange={this.handleSystemChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            <option value="">{LangHelper.getResource('Special Delete Site')}</option>
                        </select>
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
                </div>
            </form>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { notificationTypes, gridParams, createTypePopup } = state;
    return {
        notificationTypes,
        gridParams,
        createTypePopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);