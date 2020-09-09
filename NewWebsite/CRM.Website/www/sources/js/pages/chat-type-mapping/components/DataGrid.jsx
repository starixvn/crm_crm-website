import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { toast } from 'react-toastify';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { PAGING_SIZES, ChatTypeMapping, MappingStatus } from '@jsroot/common/constants';
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
        this.props.updateActiveStatus(params).then(() => {
            this.onCloseConfirmPopupChangeActiveStaus();
            if (this.props.updateActiveStatusResult) {
                toast.success(LangHelper.getResource('UpdateChatTypeMappingSuccessfully'));
                this.props.getGridData(this.props.gridParams);
            } else {
                toast.error(this.props.updateActiveStatusResult.message);
            }
        });
    }

    onCloseConfirmPopupChangeActiveStaus = () => {
        this.props.setUpateActivePopup({ isShowPopup: false });
    }

    renderRecipients = (recipient, key) => {
        return (
            <div key={key} className='recipient-item' title={`${recipient.recipientName} (${recipient.roleName})`}>
                {`${recipient.recipientName} (${recipient.roleName})`}
            </div>
        );
    }

    renderLetstalkId = (recipient, key) => <div key={key} className='recipient-item' title={recipient.letstalkId}>{recipient.letstalkId}</div>;

    handleEdit = chatMapping => {
        if (parseInt(chatMapping.chatTypeId, 10) === ChatTypeMapping.Personal) {
            this.props.openUpdatePersonalMappingPopup(chatMapping);
        } else {
            this.props.openUpdateGroupMappingPopup(chatMapping);
        }
    }

    copyGroupIdToClipboard = conversationId => {
        const input = document.createElement('input');
        input.value = conversationId;
        document.body.appendChild(input);
        input.select();
        document.execCommand('copy');
        input.remove();
        Site.displayCopiedToClipboard();
    }

    buildHeader = isHost => {
        return (
            <HeaderRow>
                <HeaderCell colWidth={10}>{LangHelper.getResource('SymbolNo')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('ChatType')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('BotName')}</HeaderCell>
                <HeaderCell colWidth={25}>{LangHelper.getResource('LetstalkName')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('RecipientType')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('Recipient')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('LetstalkID')}</HeaderCell>
                <HeaderCell colWidth={12}>{LangHelper.getResource('Language')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('MappingStatus')}</HeaderCell>
                <HeaderCell colWidth={10}>{LangHelper.getResource('Active')}</HeaderCell>
                <HeaderCell colWidth={15}>{LangHelper.getResource('Remark')}</HeaderCell>
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

        return [<Row key={`body${1}`} className="no-data"><Cell colSpan={isHost ? 13 : 12}>{LangHelper.getResource('ThereIsNoData')}</Cell></Row>];
    }

    renderIsActiveCol = (isHost, conversationGroupId, isActive, chatTypeId, mappingStatusId) => {
        const isDisabled = (chatTypeId === ChatTypeMapping.Group) && (mappingStatusId === MappingStatus.NotMapped);

        if (isHost) {
            return (
                <Cell className='text-center'>
                    <Form.Control className='checkbox-cell-style'
                        id={`active-chk-${conversationGroupId}`}
                        type='checkbox'
                        checked={isActive}
                        onChange={e => this.onChangeActiveStaus(conversationGroupId, e.target.checked)}
                        disabled={isDisabled}
                    />
                </Cell>
            );
        }

        return <Cell className='text-center'>{isActive ? <span className="fa fa-check badge-green" /> : ''}</Cell>;
    }

    renderEditIcon = (isBotRemovedStatus, chatMapping) => {
        return (
            <Cell className='text-center'>
                <div className={isBotRemovedStatus ? 'cursor-not-allowed' : ''}>
                    <span className={`fa fa-pencil icon-edit ${isBotRemovedStatus ? 'pointer-event-none' : ''}`}
                        onClick={() => this.handleEdit(chatMapping)}
                    />
                </div>
            </Cell>
        );
    }

    renderRow = (item, index, isHost) => {
        const isBotRemovedStatus = (item.chatTypeId === ChatTypeMapping.Group) && (item.mappingStatusId === MappingStatus.BotRemoved);
        return (
            <Row key={index}>
                <Cell className='text-center'>{index + this.startIndex}</Cell>
                <Cell>{item.chatType}</Cell>
                <Cell>{item.botName}</Cell>
                <Cell>
                    {item.conversationGroupName}
                </Cell>
                <Cell >{item.recipients.length > 0 ? item.recipients[0].recipientTypeName : ''}</Cell>
                <Cell >{item.headlines.length > 0 ? item.headlines.map(h => h.headlineName).join(', ') : ''}</Cell>
                <Cell className='recipient-merge-cell'>
                    {
                        item.recipients.length > 0 &&
                        item.recipients.map(this.renderRecipients)
                    }
                </Cell>
                <Cell className={item.chatTypeId === ChatTypeMapping.Personal ? '' : 'text-center'}>
                    {
                        item.chatTypeId === ChatTypeMapping.Personal ?
                            item.conversationId :
                            <span className="icon-copy fa fa-copy" onClick={() => this.copyGroupIdToClipboard(item.conversationId)} title={item.conversationId} />
                    }
                </Cell>
                <Cell >{item.language}</Cell>
                <Cell className='text-center'>
                    <span className={Site.mappingStatusClass(item.mappingStatusId)}>{item.chatTypeId === ChatTypeMapping.Group ? item.mappingStatus : ''}</span>
                </Cell>
                {this.renderIsActiveCol(isHost, item.conversationGroupId, item.isActive, item.chatTypeId, item.mappingStatusId)}
                <Cell >{item.remark}</Cell>
                {
                    isHost &&
                    this.renderEditIcon(isBotRemovedStatus, item)
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
                    shouldResetScrollPosition={false}
                />
                {
                    this.props.updateActiveStatusPopup.isShowPopup &&
                    <ConfirmPopup
                        size='lg'
                        title={LangHelper.getResource('Confirmation')}
                        content={
                            <div>
                                The Personal/Group shall be {!this.props.updateActiveStatusPopup.isChecked ? 'not ' : ''}received notification anymore.
                                Are you sure you want to {this.props.updateActiveStatusPopup.isChecked ? 'active' : 'deactive'} it?
                            </div>
                        }
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
    const { gridParams, gridData, updateActiveStatusPopup, updateActiveStatusResult, updatePersonalMappingData, updateGroupMappingData, dirtyFormConfirmation } = state;
    return {
        gridParams,
        gridData,
        updateActiveStatusPopup,
        updateActiveStatusResult,
        updatePersonalMappingData,
        updateGroupMappingData,
        dirtyFormConfirmation
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);