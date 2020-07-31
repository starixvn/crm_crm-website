import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
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

    render() {
        return (
            <form className="form-filter">
                <div className="form-row">
                    <label className="col-auto col-form-label">{LangHelper.getResource('NotificationType')}</label>
                    <div className="col-auto">
                        <select className="form-control form-control-sm shadow-none"
                            value={this.state.notificationTypeId}
                            onChange={this.handleNotificationTypeChange}
                        >
                            <option value="">{LangHelper.getResource('All')}</option>
                            {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                        </select>
                    </div>
                    <label className="col-auto col-form-label">{LangHelper.getResource('TemplateName')}</label>
                    <div className="col-auto">
                        <input type="text" className="form-control form-control-sm shadow-none"
                            value={this.state.templateName}
                            onChange={this.handleTemplateNameChange}
                        />
                    </div>
                    <div className="col-auto">
                        <button className="btn btn-primary btn-sm mr-2"
                            onClick={this.submitHandler}>{LangHelper.getResource('Submit')}</button>
                        <button className="btn btn-primary btn-sm">{LangHelper.getResource('Insert')}</button>
                    </div>
                </div>
            </form>
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