import React, { Component } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { handleNewResendOptions, resendNotifications } from '../functions';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

export class ValidateResend extends Component {
    constructor(props) {
        super(props);
    }

    onClose = () => {
        this.props.setValidateResend({
            show: false
        });
    }

    resendNotifications = dataList => {
        this.onClose();
        resendNotifications(dataList, this.callBackSuccess);
    }

    callBackSuccess = () => {
        const params = { ...this.props.gridParams };
        this.props.getGridData(params, data => handleNewResendOptions(data, this.props.setResendOptions));
    }

    render() {
        const { inActiveNotificationIds, isSingle, resendDataList } = this.props.validateResendResult.result;
        const inActiveNotificationIdsString = inActiveNotificationIds.join(', ');
        const resendIds = resendDataList.map(item => item.notificationId).join(', ');
        const isAllValidate = inActiveNotificationIds.length <= 0;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered
                onHide={() => this.onClose()}
                dialogAs={DraggableModalDialog}
            >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{LangHelper.getResource('ValidateResend')}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        isSingle && (isAllValidate ?
                            LangHelper.formatString(LangHelper.getResource('ComfirmResendNotification'), resendIds) :
                            LangHelper.formatString(LangHelper.getResource('TheRecipientOfNotificationIdHasBeenInactive'), inActiveNotificationIdsString)
                        )
                    }
                    {
                        !isSingle && (isAllValidate ?
                            LangHelper.formatString(LangHelper.getResource('ComfirmResendNotification'), resendIds) :
                            <div>
                                <div>{LangHelper.formatString(LangHelper.getResource('TheRecipientOfNotificationIdHasBeenInactive'), inActiveNotificationIdsString)}</div>
                                <div>{LangHelper.formatString(LangHelper.getResource('TheSelectedNotificationsAreAbleToResend'), resendIds)}</div>
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
                            <Button onClick={() => this.resendNotifications(resendDataList)} variant='primary' size='sm'>{LangHelper.getResource('OK')}</Button>
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
        gridParams
    } = state;

    return {
        validateResendResult,
        gridParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ValidateResend);