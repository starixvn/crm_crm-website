import React, { Component } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';
import functions from '../functions';

export class ValidateResend extends Component {
    constructor(props) {
        super(props);
    }

    onClose = () => {
        if (this.props.resendOptions.dataList.filter(item =>
                LangHelper.getResource('Error') === item.status && item.isResent
            ).length <= 0 && this.props.autoRefresh.value > 0) {
            this.props.setIsRefresh(true);
        }
 
        this.props.setValidateResend({
            show: false
        });
    }

    resendNotifications = notificationIds => {
        this.onClose();
        functions.resendNotifications({ notificationIds }, this.callBackSuccess);
    }

    callBackSuccess = () => {
        this.props.setIsRefresh(true);
        this.props.getGridData(this.props.gridParams, data => functions.handleNewResendOptions(data, this.props.setResendOptions));
        this.props.setAutoRefresh({
            ...this.props.autoRefresh,
            countDown: this.props.autoRefresh.value
        });
    }

    render() {
        const { inActiveNotificationIds, isSingle, resendIds } = this.props.validateResendResult.result;
        const inActiveNotificationIdsString = inActiveNotificationIds.join(', ');
        const resendIdsString = resendIds.join(', ');
        const isAllValidate = inActiveNotificationIds.length <= 0;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered
                onHide={() => this.onClose()}
                dialogAs={DraggableModalDialog}
            >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{LangHelper.getResource('Confirmation')}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        isSingle && (isAllValidate ?
                            LangHelper.formatString(LangHelper.getResource('ComfirmResendNotification'), resendIdsString) :
                            LangHelper.formatString(LangHelper.getResource('TheRecipientOfNotificationIdHasBeenInactive'), inActiveNotificationIdsString)
                        )
                    }
                    {
                        !isSingle && (isAllValidate ?
                            LangHelper.formatString(LangHelper.getResource('ComfirmResendNotification'), resendIdsString) :
                            <div>
                                <div>{LangHelper.formatString(LangHelper.getResource('TheRecipientOfNotificationIdHasBeenInactive'), inActiveNotificationIdsString)}</div>
                                <div>{LangHelper.formatString(LangHelper.getResource('TheSelectedNotificationsAreAbleToResend'), resendIdsString)}</div>
                                <div>{LangHelper.getResource('ComfirmResendListAndIgnoreNotification')}</div>
                            </div>
                        )
                    }
                </Modal.Body>
                {
                    (isSingle && inActiveNotificationIds.length > 0) ?
                        <Modal.Footer>
                            <Button onClick={() => this.onClose()} variant='primary' size='sm'>{LangHelper.getResource('OK')}</Button>
                        </Modal.Footer> :
                        <Modal.Footer>
                            <Button
                                variant='primary'
                                size='sm'
                                onClick={() =>
                                    this.resendNotifications(resendIds.toString())
                                }
                            >{LangHelper.getResource('OK')}</Button>
                            <Button onClick={() => this.onClose()} variant='primary' size='sm'>{LangHelper.getResource('Cancel')}</Button>
                        </Modal.Footer>
                }
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        validateResendResult,
        gridParams,
        resendOptions,
        autoRefresh
    } = state;

    return {
        validateResendResult,
        gridParams,
        resendOptions,
        autoRefresh
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ValidateResend);