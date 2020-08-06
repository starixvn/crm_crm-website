import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { Form } from 'react-bootstrap';

export class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    onPaging = (pageIndex, pageSize) => {
        const params = { ...this.props.gridParams };
        params['pageIndex'] = pageIndex;
        params['pageSize'] = pageSize;

        this.props.setGridParams(params);
        this.props.getGridData(params);
    }

    buildHeader = isHost => {
        return (
            <HeaderRow>
                <HeaderCell colWidth={5}>{LangHelper.getResource('SymbolNo')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('NotificationID') + ' ' + LangHelper.getResource('MessageID')}</HeaderCell>
                <HeaderCell colWidth={60}>{LangHelper.getResource('Sender')}/{LangHelper.getResource('NotificationType')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('SendDateTimeGMTA8')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('Recipient')}</HeaderCell>
                <HeaderCell colWidth={60}>{LangHelper.getResource('Title')}/{LangHelper.getResource('Content')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('Status')}</HeaderCell>
                {
                    isHost && <HeaderCell colWidth={10}>{LangHelper.getResource('Action')}</HeaderCell>
                }
                {
                    isHost && <HeaderCell colWidth={10}>{LangHelper.getResource('Resend')}</HeaderCell>
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

        return [<Row key={`body${1}`} ><Cell className='text-center' colSpan={isHost ? 10 : 8}>{LangHelper.getResource('ThereIsNoData')}</Cell></Row>];
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
                <div>{item.notificationType}</div>
            </Cell>
            <Cell className='text-center'>
                <div>{item.sendDateString}</div>
                <div>{item.sendTimeString}</div>
            </Cell>
            <Cell className='text-center'>{item.groupHeadlines.map(gh => gh.name).join(', ')}</Cell>
            <Cell className='text-center'>{item.recipients.map((recipient, index) => <div key={index}>{`${recipient.name} (${recipient.role.name})`}</div>)}</Cell>
            <Cell>{item.title}</Cell>
            <Cell>{item.status}</Cell>
            {
                isHost &&
                <Cell className='text-center'>
                    <span className='icon-edit' data-templateid={item.notificationTemplateId} />
                    <span className='icon-delete' data-templateid={item.notificationTemplateId} />
                </Cell>
            }
            {
                isHost &&
                <Cell className='text-center'>
                    <Form.Control type='checkbox' />
                </Cell>
            }
        </Row>
    )

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
            PageList: [10, 20, 50, 100, 500]
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
    const { gridParams, gridData } = state;
    return {
        gridParams,
        gridData
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);