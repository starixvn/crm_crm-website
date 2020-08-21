import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { Form } from 'react-bootstrap';
import { handleNewResendOptions } from '../functions';

export class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    onPaging = (pageIndex, pageSize) => {
        const params = { ...this.props.gridParams };
        params['pageIndex'] = pageIndex;
        params['pageSize'] = pageSize;

        this.props.setGridParams(params);
        this.props.getGridData({...params}, data => handleNewResendOptions(data, this.props.setResendOptions));
    }

    buildHeader = isHost => {
        return (
            <HeaderRow>
                <HeaderCell colWidth={5}>{LangHelper.getResource('SymbolNo')}</HeaderCell>
                <HeaderCell colWidth={15}>
                    <div>{LangHelper.getResource('NotificationID')}</div>
                    <div>{LangHelper.getResource('MessageID')}</div>
                </HeaderCell>
                <HeaderCell colWidth={30}>{LangHelper.getResource('Sender')}/{LangHelper.getResource('NotificationType')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('SendDateTimeGMTA8')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('Recipient')}</HeaderCell>
                <HeaderCell colWidth={30}>{LangHelper.getResource('Title')}/{LangHelper.getResource('Content')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('Status')}</HeaderCell>
                {
                    isHost && <HeaderCell colWidth={10}>{LangHelper.getResource('Action')}</HeaderCell>
                }
                {
                    isHost && <HeaderCell colWidth={10}>
                        {
                            !this.props.resendOptions.isDisabledResend ? 
                            <div className='link-click' onClick={this.resendNotificationAll}>{LangHelper.getResource('Resend')}</div> :
                            <div>{LangHelper.getResource('Resend')}</div>
                        }
                        {
                            this.props.resendOptions.isHaveResendAll && <Form.Control className='checkbox-cell-style margin-top-10' type='checkbox' onChange={this.handleChangeResendAllCheckbox} checked={this.props.resendOptions.isCheckedAll} />
                        }
                    </HeaderCell>
                }
            </HeaderRow>
        );
    }

    buildBody = isHost => {
        const { pageIndex, pageSize } = this.props.gridParams;

        this.startIndex = (pageIndex - 1) * pageSize + 1;

        if (this.props.gridData.data.length > 0) {
            return this.props.gridData.data.map((item, index) => this.renderRow(item, index, isHost));
        }

        return [<Row key={`body${1}`} className="no-data" ><Cell className='text-center' colSpan={isHost ? 10 : 8}>{LangHelper.getResource('ThereIsNoData')}</Cell></Row>];
    }

    openContentPopup = item => {
        if (item.status === LangHelper.getResource('Error')) {
            this.props.setErrorOrResolvedContentPopup({
                show: true,
                data: item
            });
        }
        else {
            this.props.setContentPopup({
                show: true,
                data: item
            });
        }
    }

    isError = status => status.toLowerCase() === LangHelper.getResource('Error').toLowerCase();

    validateResend = notificationId => {
        this.props.validateResend({
            notificationIds: notificationId,
            isSingle: true
        });
    }

    renderRow = (item, index, isHost) => (
        <Row key={index} >
            <Cell className='text-center'>{index + this.startIndex}</Cell>
            <Cell className='text-center'>
                <div>{item.notificationId}</div>
                <div>{item.messageId}</div>
            </Cell>
            <Cell>
                <div>{item.sender}</div>
                <div>{item.notificationTypeName}</div>
            </Cell>
            <Cell className='text-center'>
                <div>{item.sendDateString}</div>
                <div>{item.sendTimeString}</div>
            </Cell>
            <Cell className='text-center'>{this.getHeadlines(item.headlines)}</Cell>
            <Cell className='text-center'>{this.getRecipients(item.recipients)}</Cell>
            <Cell className='link-click' onClick={() => this.openContentPopup(item)}>{item.title}</Cell>
            <Cell className={`text-center status-font ${item.status.toLowerCase()}-color`}>{item.status}</Cell>
            {
                isHost &&
                <Cell className='text-center'>
                    {
                        this.isError(item.status) && <div onClick={() => this.openContentPopup(item)} className='action-style link-click'>{LangHelper.getResource('ViewDetail')}</div>
                    }
                    {
                        this.isError(item.status) && <div onClick={() => this.validateResend(item.notificationId)} className='action-style link-click'>{LangHelper.getResource('Resend')}</div>
                    }
                </Cell>
            }
            {
                isHost &&
                <Cell className='text-center'>
                    { this.isError(item.status) && <Form.Control className='checkbox-cell-style'
                        id={`resend-${item.notificationId}-${item.messageId}`}
                        type='checkbox'
                        checked={this.props.resendOptions.dataList[index] && this.props.resendOptions.dataList[index].isResent}
                        onChange={() => this.handleChangeResendCheckbox(index)} />}
                </Cell>
            }
        </Row>
    )

    getHeadlines = headlines => {
        if (headlines && headlines.length > 0) {
            return headlines.map(headline => headline.headlineName).join(', ')
        }
        
        return '';
    }

    getRecipients = recipients => {
        if (recipients && recipients.length > 0) {
            return recipients.map((recipient, index) => <div key={index}>{`${recipient.recipientName} (${recipient.roleName})`}</div>)
        }
        
        return '';
    }

    handleChangeResendCheckbox = (index) => {
        const dataList = [...this.props.resendOptions.dataList];
        dataList[index].isResent = !this.props.resendOptions.dataList[index].isResent;
        const resendList = dataList.filter(item => this.isError(item.status) && item.isResent);
        const notCheckedDataList = dataList.filter(item => this.isError(item.status) && !item.isResent);
        this.props.setResendOptions({
            isCheckedAll: notCheckedDataList.length <= 0,
            isDisabledResend: resendList.length <= 0,
            isHaveResendAll: this.props.resendOptions.isHaveResendAll,
            dataList
        });
    }

    handleChangeResendAllCheckbox = () => {
        const checked = !this.props.resendOptions.isCheckedAll;
        const dataList = [...this.props.resendOptions.dataList];
        dataList.forEach(item => item.isResent = checked);
        this.props.setResendOptions({
            isCheckedAll: checked,
            isDisabledResend: !checked,
            isHaveResendAll: this.props.resendOptions.isHaveResendAll,
            dataList
        }, false);
    }

    resendNotificationAll = () => {
        const resendList = this.props.resendOptions.dataList.filter(item => this.isError(item.status) && item.isResent);
        this.props.validateResend({
            notificationIds: resendList.map(item => item.notificationId).join(','),
            isSingle: false
        })
    }

    render() {
        if (!this.props.gridData || !this.props.gridData.data) {
            return null;
        }

        const { totalItems } = this.props.gridData;
        const { pageIndex, pageSize } = this.props.gridParams;
        const { isHost } = this.props;

        const pagingOption = {
            PageIndex: pageIndex,
            PageSize: pageSize,
            TotalItem: totalItems,
            PageList: [100, 200, 300, 400, 500, 1000]
        };

        return (
            <Fragment>
                <div className='table-title'>{LangHelper.getResource('NotificationList')}</div>
                <Table
                    minWidth={960}
                    maxWidth={document.getElementById('app-container').clientWidth}
                    autoWidth
                    header={this.buildHeader(isHost)}
                    body={this.buildBody(isHost)}
                    isPaging
                    pageOption={pagingOption}
                    onPaging={this.onPaging}
                    containerPadding={0}
                    adjustedHeight={50}
                />
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        gridParams,
        gridData,
        setContentPopup,
        setErrorOrResolvedContentPopup,
        resendOptions,
        setResendOptions,
        getGridData,
        validateResend
    } = state;

    return {
        gridParams,
        gridData,
        setContentPopup,
        setErrorOrResolvedContentPopup,
        resendOptions,
        setResendOptions,
        getGridData,
        validateResend
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);