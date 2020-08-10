import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import NewNotificationTemplate from './NewNotificationTemplate';
import EditNotificationTemplate from './EditNotificationTemplate';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import LangHelper from '@jsroot/common/langHelper';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('IsHost').checked;
    }

    componentDidMount() {
        this.props.getNotificationTypes();
        this.props.getLanguages();
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

    handleCloseDeletePopup = () => {
        this.props.setDeleteTemplatePopup({ show: false });
        this.props.setDeleteTemplateResult(null);
    }

    handleDelete = () => {
        this.props.deleteTemplate(this.props.deleteTemplatePopup.templateId).then(data => {
            if (data.isSuccessful) {
                data.message = LangHelper.getResource('DeleteNotificationTemplateSuccessfully')
                setTimeout(() => {
                    this.handleCloseDeletePopup();
                    this.props.getGridData(this.props.gridParams);
                }, 3000);
            }

            this.props.setDeleteTemplateResult(data);
        });
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
                {
                    this.props.deleteTemplatePopup.show &&
                    <ConfirmPopup
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>Are you sure you want to delete <strong>{this.props.deleteTemplatePopup.name}</strong>?</div>
                        }
                        requestResponse={this.props.deleteTemplateResult}
                        onSubmit={this.handleDelete}
                        onClose={this.handleCloseDeletePopup}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { gridParams, createTemplatePopup, editTemplatePopup, deleteTemplatePopup, deleteTemplateResult } = state;
    return {
        gridParams,
        createTemplatePopup,
        editTemplatePopup,
        deleteTemplatePopup,
        deleteTemplateResult
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);