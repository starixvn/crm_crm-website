import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import Content from './Content';
import ViewDetail from './ViewDetail';
import ValidateResend from './ValidateResend';
import LangHelper from '@jsroot/common/langHelper';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('is-host').checked;
    }

    closeConfirmPopup = () => this.props.setConfirmPopup({ show: false })

    handleCloseContent = () => {
        this.props.setContentPopup({ show: false });
        this.setIsRefresh();
    }

    handleCloseViewDetail = () => {
        this.props.setViewDetailPopup({ show: false });
        this.setIsRefresh();
    }

    setIsRefresh = () => {
        if (this.props.autoRefresh.value > 0) {
            this.props.setIsRefresh(true);
        }
    }

    render() {
        return (
            <Fragment>
                <Filter />
                <DataGrid isHost={this.isHost} />
                {
                    this.props.contentPopup.show &&
                    <Content
                        onClose={this.handleCloseContent}
                        data={this.props.contentPopup.data}
                    />
                }
                {
                    this.props.viewDetailPopup.show &&
                    <ViewDetail
                        onClose={this.handleCloseViewDetail}
                        data={this.props.viewDetailPopup.data}
                    />
                }
                {
                    this.props.validateResendResult.show && <ValidateResend />
                }
                {
                    this.props.confirmPopup.show &&
                    <ConfirmPopup
                        size='lg'
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>{LangHelper.getResource('ThereAreSomeChangesConfirmMessage')}</div>
                        }
                        onSubmit={() => {
                            this.closeConfirmPopup();
                            this.props.confirmPopup.onOk();
                        }}
                        onClose={this.closeConfirmPopup}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        contentPopup,
        viewDetailPopup,
        validateResendResult,
        confirmPopup,
        autoRefresh
    } = state;

    return {
        contentPopup,
        viewDetailPopup,
        validateResendResult,
        confirmPopup,
        autoRefresh
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);