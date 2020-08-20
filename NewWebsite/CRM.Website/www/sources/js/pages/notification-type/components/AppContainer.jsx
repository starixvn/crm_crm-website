import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import NewNotificationType from './NewNotificationType';
import EditNotificationType from './EditNotificationType';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import LangHelper from '@jsroot/common/langHelper';
import ResponseMessage from '@jsroot/components/ResponseMessage';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('IsHost').checked;
    }

    handleCloseCreatePopup = () => {
        this.props.setCreateTypePopup({ show: false });
        this.props.setCreateTypeResult(null);
    }

    handleCloseEditPopup = () => {
        this.props.setEditTypePopup({ show: false });
        this.props.setEditTypeResult(null);
    }

    handleCreateSuccess = () => {
        this.handleCloseCreatePopup();
        this.props.getGridData(this.props.gridParams);
    }

    handleEditSuccess = () => {
        this.handleCloseEditPopup();
        this.props.getGridData(this.props.gridParams);
    }

    handleCloseDeletePopup = () => {
        this.props.setDeleteTypePopup({ show: false });
        this.props.setDeleteTypeResult(null);
    }

    handleDelete = () => {
        this.props.deleteType(this.props.deleteTypePopup.typeId).then(data => {
            if (data.isSuccessful) {
                data.message = LangHelper.getResource('DeleteNotificationTypeSuccessfully')
                setTimeout(() => {
                    this.handleCloseDeletePopup();
                    this.props.getGridData(this.props.gridParams);
                }, 3000);
            }

            this.props.setDeleteTypeResult(data);
        });
    }
    render() {
        return (
            <Fragment>
                {
                    this.props.invalidParams.show &&
                     <ResponseMessage isSuccessful={false}
                            message={this.props.invalidParams.message}
                        />
                }
                <Filter isHost={this.isHost} />
                <DataGrid isHost={this.isHost} />
                {
                    this.props.createTypePopup.show &&
                    <NewNotificationType
                        onClose={this.handleCloseCreatePopup}
                        onSuccess={this.handleCreateSuccess}
                    />
                }
                {
                    this.props.editTypePopup.show &&
                    <EditNotificationType
                        onClose={this.handleCloseEditPopup}
                        onSuccess={this.handleEditSuccess}
                    />
                }
                {
                    this.props.deleteTypePopup.show &&
                    <ConfirmPopup size="lg"
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>Are you sure you want to delete <strong>{this.props.deleteTypePopup.name}</strong>?</div>
                        }
                        requestResponse={this.props.deleteTypeResult}
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
    const { gridParams, createTypePopup, editTypePopup, deleteTypePopup, deleteTypeResult, invalidParams } = state;
    return {
        gridParams,
        createTypePopup,
        editTypePopup,
        deleteTypePopup,
        deleteTypeResult,
        invalidParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);