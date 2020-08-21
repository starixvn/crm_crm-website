import React, { Component, Fragment } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';

class ValidateResend extends Component {
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
        this.props.resendNotifications(dataList);
    }

    render() {
        const { inActiveNotificationIds, isSingle, resendDataList } = this.props.validateResendResult.result;
        const inActiveNotificationIdsString = inActiveNotificationIds.join(', ');
        const resendIds = resendDataList.map(item => item.notificationId).join(', ');
        const isAllValidate = inActiveNotificationIds.length <= 0;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered onHide={this.onClose} >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{LangHelper.getResource('ValidateResend')}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        isSingle ? 
                        <div>
                            {
                                isAllValidate ? LangHelper.getResource('ComfirmResendNotification').replace('<notificationIds>', resendIds) : LangHelper.getResource('TheRecipientOfNotificationIdHasBeenInactive').replace('<notificationId>', inActiveNotificationIdsString)
                            }
                        </div> : (
                            isAllValidate ? <div>{LangHelper.getResource('ComfirmResendNotification').replace('<notificationIds>', resendIds)}</div> :
                            <div>
                                <div>{LangHelper.getResource('TheRecipientOfNotificationIdHasBeenInactive').replace('<notificationIds>', inActiveNotificationIdsString)}</div>
                                <div>{LangHelper.getResource('TheSelectedNoficationsAreAbleToResend').replace('<notificationIds>', resendIds)}</div>
                                <div>{LangHelper.getResource('ComfirmResendListAndIgnoreNotification')}</div>
                            </div>
                        )
                    }
                </Modal.Body>
                {
                    isSingle ? 
                    <Modal.Footer>
                        {
                            inActiveNotificationIds.length > 0 ?
                            <Button onClick={this.onClose} variant='primary' size='sm'>{LangHelper.getResource('OK')}</Button> :
                            <Fragment>
                                <Button onClick={() => this.resendNotifications(resendDataList)} variant='primary' size='sm'>{LangHelper.getResource('OK')}</Button>
                                <Button onClick={this.onClose} variant='primary' size='sm'>{LangHelper.getResource('Cancel')}</Button>
                            </Fragment>
                        }
                    </Modal.Footer> : 
                    <Modal.Footer>
                        <Button onClick={() => this.resendNotifications(resendDataList)} variant='primary' size='sm'>{LangHelper.getResource('OK')}</Button>
                        <Button onClick={this.onClose} variant='primary' size='sm'>{LangHelper.getResource('Cancel')}</Button>
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
        setValidateResend,
        resendNotifications
    } = state;

    return {
        validateResendResult,
        setValidateResend,
        resendNotifications
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ValidateResend);