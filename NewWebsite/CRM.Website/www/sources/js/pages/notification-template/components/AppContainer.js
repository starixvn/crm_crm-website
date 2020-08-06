import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import NewNotificationTemplate from './NewNotificationTemplate';
import EditNotificationTemplate from './EditNotificationTemplate';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('IsHost').checked;
    }

    componentDidMount() {
        this.props.getNotificationTypes();
    }

    handleCloseCreatePopup = () => {
        this.props.setCreateTemplatePopup({ show: false });
        this.props.setCreateTemplateResult(null);
    }

    handleCreateSuccess = () => {
        this.handleCloseCreatePopup();
        this.props.getGridData(this.props.gridParams);
    }

    handleCloseEditPopup = () => {
        this.props.setEditTemplatePopup({ show: false });
        this.props.setEditTemplateResult(null);
    }

    handleEditSuccess = () => {
        this.handleCloseEditPopup();
        this.props.getGridData(this.props.gridParams);
    }

    render() {
        return (
            <Fragment>
                <Filter isHost={this.isHost} />
                <DataGrid isHost={this.isHost} />
                {
                    this.props.createTemplatePopup.show &&
                    <NewNotificationTemplate
                        onClose={this.handleCloseCreatePopup}
                        onSuccess={this.handleCreateSuccess}
                    />
                }
                {
                    this.props.editTemplatePopup.show &&
                    <EditNotificationTemplate
                        onClose={this.handleCloseEditPopup}
                        onSuccess={this.handleEditSuccess}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { gridParams, createTemplatePopup, editTemplatePopup } = state;
    return {
        gridParams,
        createTemplatePopup,
        editTemplatePopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);