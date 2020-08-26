import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { PAGING_SIZES, ChatTypeMapping } from '@jsroot/common/constants';
import Site from '@jsroot/common/site';
import { Form } from 'react-bootstrap';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import EditGroupMappingPopup from './EditGroupMappingPopup';
import EditPersonalMappingPopup from './EditPersonalMappingPopup';

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

    onChangeActiveStaus = (conversationGroupId, isChecked) => {
        this.props.setUpateActivePopup({ isShowPopup: true, conversationGroupId, isChecked });
    }

    updateActiveStatus = () => {
        const params = { conversationGroupId: this.props.updateActiveStatusPopup.conversationGroupId, isActive: this.props.updateActiveStatusPopup.isChecked };
        this.props.updateActiveStatus(params);
        this.props.getGridData(this.props.gridParams);
    }

    onCloseConfirmPopupChangeActiveStaus = () => {
        this.props.setUpateActivePopup({ isShowPopup: false });
    }

    renderRecipients = (recipient, key) => <div key={key} className='recipient-item'>{`${recipient.recipientName} (${recipient.roleName})`}</div>;

    renderLetstalkId = (recipient, key) => <div key={key} className='recipient-item'>{recipient.letstalkId}</div>;

    handleEdit = (chatTypeId, data) => {
        if (parseInt(chatTypeId, 10) === ChatTypeMapping.Personal) {
            this.props.openUpdatePersonalMappingPopup(data);
        } else {
            this.props.openUpdateGroupMappingPopup(data);
        }
    }

    copyGroupIdToClipboard = groupId => {
        //const input = document.createElement('input');
        //input.value = groupId;
        //document.body.appendChild(input);
        //input.select();
        //document.execCommand('copy');
        //input.remove();
    }

    buildHeader = isHost => {
        return (
            <HeaderRow style={{ height: 40 }}>
                <HeaderCell colWidth={10}>{LangHelper.getResource('SymbolNo')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('ChatType')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('BotName')}</HeaderCell>
                <HeaderCell colWidth={30}>{LangHelper.getResource('LetstalkName')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('RecipientType')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('Recipient')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('LetstalkID')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('Language')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('MappingStatus')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('Active')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('Remark')}</HeaderCell>
                {
                    isHost && <HeaderCell colWidth={10}>{LangHelper.getResource('Action')}</HeaderCell>
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

        return [<Row key={`body${1}`} ><Cell className='text-center' colSpan={isHost ? 13 : 12}>{LangHelper.getResource('ThereIsNoData')}</Cell></Row>];
    }

    renderIsActiveCol = (isHost, conversationGroupId, isActive) => {
        if (isHost) {
            return (
                <Cell className='text-center'>
                    <Form.Control className='checkbox-cell-style'
                        id={`active-chk-${conversationGroupId}`}
                        type='checkbox'
                        checked={isActive}
                        onChange={(e) => this.onChangeActiveStaus(conversationGroupId, e.target.checked)} />
                </Cell>
            );
        }

        return <Cell rowSpan={numOfRow} className='text-center'>{isActive ? <span className="fa fa-check badge-green" /> : ''}</Cell>;
    }

    renderRow = (item, index, isHost) => {
        return (
            <Row key={index}>
                <Cell className='text-center'>{index + this.startIndex}</Cell>
                <Cell>{item.chatType}</Cell>
                <Cell>{item.botName}</Cell>
                <Cell>
                    {item.conversationGroupName}
                    {
                        item.chatTypeId === ChatTypeMapping.Group &&
                        <span className="icon-copy fa fa-copy" onClick={this.copyGroupIdToClipboard(item.LetstalkId)} title='Coppy to clipboard' />
                    }
                </Cell>
                <Cell >{item.recipients[0].recipientTypeName}</Cell>
                <Cell >{item.headlines.map(h => h.headlineName).join(', ')}</Cell>
                <Cell className='recipient-merge-cell'>
                    {
                        item.recipients.map(this.renderRecipients)
                    }
                </Cell>
                <Cell className='recipient-merge-cell'>
                    {
                        item.recipients.map(this.renderLetstalkId)
                    }
                </Cell>
                <Cell >{item.language}</Cell>
                <Cell className='text-center'>
                    <span className={Site.mappingStatusClass(item.mappingStatusId)}>{item.chatTypeId === ChatTypeMapping.Group ? item.mappingStatus : ''}</span>
                </Cell>
                {this.renderIsActiveCol(isHost, item.conversationGroupId, item.isActive)}
                <Cell >{item.remark}</Cell>
                {
                    isHost &&
                    <Cell className='text-center'>
                        <span className="fa fa-pencil icon-edit"
                            onClick={() => this.handleEdit(item.chatTypeId, item)}
                        />
                    </Cell>
                }
            </Row>
        );
    }

    render() {
        if (!this.props.gridData.data) {
            return null;
        }

        const { totalItems } = this.props.gridData;
        const { pageIndex, pageSize } = this.props.gridParams;
        const { isHost } = this.props;

        const pagingOption = {
            PageIndex: pageIndex,
            PageSize: pageSize,
            TotalItem: totalItems,
            PageList: PAGING_SIZES
        };

        return (
            <Fragment>
                <div className='table-title'>{LangHelper.getResource('ChatTypeMapping')}</div>
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
                {
                    this.props.updateActiveStatusPopup.isShowPopup &&
                    <ConfirmPopup
                        size='lg'
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>
                                When a group/personal is <strong>{this.props.updateActiveStatusPopup.isChecked ? 'actived' : 'deactived'}</strong>. The system will <strong>{this.props.updateActiveStatusPopup.isChecked ? '' : 'not'}</strong> send notifications to the group.
								Do you want to proceed?
							</div>
                        }
                        requestResponse={this.props.updateActiveStatusResult}
                        onSubmit={this.updateActiveStatus}
                        onClose={this.onCloseConfirmPopupChangeActiveStaus}
                    />
                }
                {
                    this.props.updatePersonalMappingData.isShow &&
                    <EditPersonalMappingPopup />
                }
                {
                    this.props.updateGroupMappingData.isShow &&
                    <EditGroupMappingPopup />
                }

            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { gridParams, gridData, updateActiveStatusPopup, updatePersonalMappingData, updateGroupMappingData } = state;
    return {
        gridParams,
        gridData,
        updateActiveStatusPopup,
        updatePersonalMappingData,
        updateGroupMappingData
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);