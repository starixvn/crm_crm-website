import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import Content from './Content';
import ErrorResolvedContent from './ErrorResolvedContent';
import ValidateResend from './ValidateResend';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('is-host').checked;
    }

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
                    this.props.invalidParams.show && <div className='invalid-params-box'><i className='fa fa-exclamation-triangle'></i>{this.props.invalidParams.message}</div>
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
                    <ErrorResolvedContent
                        onClose={() => this.props.setErrorOrResolvedContentPopup({ show: false })}
                        data={this.props.errorOrResolvedContentPopup.data}
                    />
                }
                {
                    this.props.validateResendResult.show && <ValidateResend />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        contentPopup,
        setContentPopup,
        errorOrResolvedContentPopup,
        setErrorOrResolvedContentPopup,
        invalidParams,
        messageResult,
        validateResendResult
    } = state;

    return {
        contentPopup,
        setContentPopup,
        errorOrResolvedContentPopup,
        setErrorOrResolvedContentPopup,
        invalidParams,
        messageResult,
        validateResendResult
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);