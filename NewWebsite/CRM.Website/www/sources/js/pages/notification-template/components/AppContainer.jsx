import React, { Component, Fragment } from 'react';
import { Button } from 'react-bootstrap';
import PropTypes from 'prop-types';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import Filter from './Filter';
import DataGrid from './DataGrid';
import NewNotificationTemplate from './NewNotificationTemplate';
import EditNotificationTemplate from './EditNotificationTemplate';
import ContentDetailModalDialog from './ContentDetailModalDialog';
import SweetModalDialog from '@jsroot/components/SweetModalDialog';
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
        getLanguages: function () {
            // nothing
        },
        getNotificationTypes: function () {
            // nothing
        },
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

    handleEditSuccess = () => {
        this.handleCloseEditPopup();
        this.props.getGridData(this.props.gridParams);
    }

    handleCloseViewContentPopup = () => {
        this.props.setContentDetailPopup({ show: false });
        this.props.setSelectedTemplate(null);
    }

    handleCloseDeletePopup = () => {
        this.props.setDeleteTemplatePopup({ show: false });
    }

    handleDelete = () => {
        const { templateId, name } = this.props.deleteTemplatePopup;
        const params = { notificationTemplateId: templateId, notificationTemplateName: name };
        this.props.deleteTemplate(params).then(response => {
            const data = response.data;
            this.handleCloseDeletePopup();
            this.props.getGridData(this.props.gridParams);
            if (data.isSuccessful) {
                toast.success(LangHelper.getResource('DeleteNotificationTemplateSuccessfully'));
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
                    <SweetModalDialog size="lg"
                        title={LangHelper.getResource('Confirmation')}
                        contents={
                            <div>Are you sure you want to delete <strong>{this.props.deleteTemplatePopup.name}</strong>?</div>
                        }
                        buttons={[
                            <Button variant="primary" size="sm" onClick={this.handleDelete}>
                                {LangHelper.getResource('OK')}
                            </Button>,
                            <Button variant="primary" size="sm" onClick={this.handleCloseDeletePopup}>
                                {LangHelper.getResource('Cancel')}
                            </Button>
                        ]}
                        onClose={this.handleCloseDeletePopup}
                    />
                }
                {
                    this.props.contentDetailPopup.show &&
                    <ContentDetailModalDialog onClose={this.handleCloseViewContentPopup} />
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