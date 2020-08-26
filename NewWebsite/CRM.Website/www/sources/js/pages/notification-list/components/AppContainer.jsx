import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import Content from './Content';
import ViewDetail from './ViewDetail';
import ValidateResend from './ValidateResend';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('is-host').checked;
    }

    closeConfirmPopup = () => this.props.setConfirmPopup({ show: false })

    render() {
        return (
            <Fragment>
                {
                    
                    this.props.messageResult.show &&
                    <ResponseMessage isSuccessful={this.props.messageResult.isSuccessful}
                        message={this.props.messageResult.message || LangHelper.getResource('ResentNotificationSuccessfully')}
                    />
                }
                {
                    this.props.invalidParams.show && <ResponseMessage isSuccessful={false}
                        message={this.props.invalidParams.message}
                    />
                }
                <Filter />
                <DataGrid isHost={this.isHost} />
                {
                    this.props.contentPopup.show &&
                    <Content
                        onClose={() => this.props.setContentPopup({ show: false })}
                        data={this.props.contentPopup.data}
                    />
                }
                {
                    this.props.errorOrResolvedContentPopup.show &&
                    <ViewDetail
                        onClose={() => this.props.setViewDetailPopup({ show: false })}
                        data={this.props.errorOrResolvedContentPopup.data}
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
        errorOrResolvedContentPopup,
        invalidParams,
        messageResult,
        validateResendResult,
        confirmPopup
    } = state;

    return {
        contentPopup,
        errorOrResolvedContentPopup,
        invalidParams,
        messageResult,
        validateResendResult,
        confirmPopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);