import React, { Component, Fragment } from 'react';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { getHeadlines, getRecipients } from '../functions';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

export class ViewDetail extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isResolved: false
        };
    }
    
    componentDidMount = () =>
        this.setState({
            remark: this.props.data.resolvedRemark ?? '',
            oldRemark: this.props.data.resolvedRemark ?? '',
        })

    buildHeader = () =>
        <HeaderRow>
            <HeaderCell colWidth={25}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
            <HeaderCell colWidth={25}>{LangHelper.getResource('Recipient')}</HeaderCell>
            <HeaderCell colWidth={50}>{LangHelper.getResource('Reason')}</HeaderCell>
        </HeaderRow>

    buildBody = data =>
        [<Row key={data.notificationId}>
            <Cell>{getHeadlines(data.headlines)}</Cell>
            <Cell>{getRecipients(data.recipients)}</Cell>
            <Cell className='error-color'>{data.reason}</Cell>
        </Row>]

    editNotificationList = notificationId =>
        this.setState({ isResolved: true }, () =>
            this.props.editNotificationList({
                notificationId,
                remark: this.state.remark
            })
        )

    handleChange = e => {
        this.setState({ [e.target.name]: e.target.value});
    }

    onClose = close => {
        if (this.state.isResolved) {
            this.props.setViewDetail({
                resolvedBy: '',
                result: {
                    code: 1,
                    message: '',
                    isSuccessful: false
                }
            });

            close();
            this.props.getGridData({...this.props.gridParams});
        }
        else {
            if (this.state.remark !== this.state.oldRemark) {
                this.props.setConfirmPopup({
                    show: true,
                    onOk: close
                });
            }
            else {
                close();
            }
        }
    }

    render() {
        const { data, onClose } = this.props;

        return (
            <Modal show backdrop='static' keyboard={false} size='lg' centered
                onHide={() => this.onClose(onClose)}
                dialogAs={DraggableModalDialog}
            >
                <Modal.Header closeButton>
                    <Modal.Title as='div'>{LangHelper.getResource('ErrorNotification')}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        {`${LangHelper.getResource('NotificationID')}: `}
                        <b>{data.notificationId}</b>
                    </div>
                    <div>
                        {`${LangHelper.getResource('NotificationTemplate')}: `}
                        <b>{data.title}</b>
                    </div>
                    <div>
                        {`${LangHelper.getResource('SentAt')}: `}
                    <b>{`${data.sendDateString} ${data.sendTimeString}`}</b>
                    </div>
                    <div className='error-occured-message error-resolved-row'>{LangHelper.getResource('ErrorOccuredWhileSendingTheNotificationToTheFollowingRecipient')}</div>
                    <Table
                        minWidth={500}
                        maxWidth={772}
                        header={this.buildHeader()}
                        body={this.buildBody(data)}
                        containerPadding={0}
                        adjustedHeight={50}
                    />
                    {
                        this.props.viewDetail.result.isSuccessful ? <Fragment>
                            <div className='error-resolved-row'>{LangHelper.getResource('Remark') + ': ' + LangHelper.getResource('ResendDone')}</div>
                            <div>{LangHelper.getResource('ResolvedBy') + ': ' + this.props.viewDetail.resolvedBy}</div>
                        </Fragment> : <Fragment>
                            <div className='error-resolved-row'>{LangHelper.getResource('Remark') + ':'}</div>
                            <textarea className='view-detail-remark' rows='3' onChange={e => this.handleChange(e)} name='remark' value={this.state.remark} />
                        </Fragment>
                    }
                </Modal.Body>
                <Modal.Footer>
                    {
                        !this.props.viewDetail.result.isSuccessful &&
                            <Button
                                onClick={() => this.editNotificationList(data.notificationId)}
                                variant='primary' size='sm'
                            >{LangHelper.getResource('Resolved')}</Button>
                    }
                    <Button
                        onClick={() => this.onClose(onClose)}
                        variant='primary' size='sm'
                    >
                        {this.state.isResolved ? LangHelper.getResource('Close') : LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        viewDetail,
        gridParams
    } = state;

    return {
        viewDetail,
        gridParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ViewDetail);