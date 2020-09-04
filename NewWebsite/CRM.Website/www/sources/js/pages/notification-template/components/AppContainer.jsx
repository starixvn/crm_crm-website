import React, { Component, Fragment } from 'react';
import PropTypes from 'prop-types';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import Filter from './Filter';
import DataGrid from './DataGrid';
import NewNotificationTemplate from './NewNotificationTemplate';
import EditNotificationTemplate from './EditNotificationTemplate';
import ContentDetailPopup from './ContentDetailPopup';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import LangHelper from '@jsroot/common/langHelper';

export class AppContainer extends Component {
    static propTypes = {
        contentDetailPopup: PropTypes.shape({ show: PropTypes.bool }).isRequired,
        createTemplatePopup: PropTypes.shape({ show: PropTypes.bool }).isRequired,
        deleteTemplatePopup: PropTypes.shape({ show: PropTypes.bool }).isRequired,
        editTemplatePopup: PropTypes.shape({ show: PropTypes.bool }).isRequired,
        getLanguages: PropTypes.func,
        getNotificationTypes: PropTypes.func,
        isHost: PropTypes.bool.isRequired
    };

    static defaultProps = {
        contentDetailPopup: { show: false },
        createTemplatePopup: { show: false },
        deleteTemplatePopup: { show: false },
        editTemplatePopup: { show: false },
        getLanguages: function () { },
        getNotificationTypes: function () { },
        isHost: false
    };

    constructor(props) {
        super(props);
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
        this.props.setSelectedTemplate(null);
        this.props.setEditTemplateResult(null);
    }

    handleCloseViewContentPopup = () => {
        this.props.setContentDetailPopup({ show: false });
        this.props.setSelectedTemplate(null);
    }

    handleEditSuccess = () => {
        this.handleCloseEditPopup();
        this.props.getGridData(this.props.gridParams);
    }

    handleCloseDeletePopup = () => {
        this.props.setDeleteTemplatePopup({ show: false });
    }

    handleDelete = () => {
        this.props.deleteTemplate(this.props.deleteTemplatePopup.templateId).then(data => {
            if (data.isSuccessful) {
                toast.success(LangHelper.getResource('DeleteNotificationTemplateSuccessfully'));
                this.handleCloseDeletePopup();
                this.props.getGridData(this.props.gridParams);
            }
            else {
                toast.error(data.message);
            }
        });
    }

    render() {
        return (
            <Fragment>
                <Filter isHost={this.props.isHost} />
                <DataGrid isHost={this.props.isHost} />
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
                    <ConfirmPopup size="lg"
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>Are you sure you want to delete <strong>{this.props.deleteTemplatePopup.name}</strong>?</div>
                        }
                        onSubmit={this.handleDelete}
                        onClose={this.handleCloseDeletePopup}
                    />
                }
                {
                    this.props.contentDetailPopup.show &&
                    <ContentDetailPopup onClose={this.handleCloseViewContentPopup} />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        gridParams, createTemplatePopup, editTemplatePopup,
        deleteTemplatePopup, deleteTemplateResult, contentDetailPopup
    } = state;
    return {
        gridParams, createTemplatePopup, editTemplatePopup,
        deleteTemplatePopup, deleteTemplateResult, contentDetailPopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);