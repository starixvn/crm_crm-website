import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { PAGING_SIZES, defaultPageSize, ChatTypeMapping } from '@jsroot/common/constants';
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

    onChangeActiveStaus = (conversationId, isChecked) => {
        this.props.setUpateActivePopup({ isShowPopup: true, conversationId, isChecked });
    }

    updateActiveStatus = () => {
        const params = { conversationId: this.props.updateActiveStatusPopup.conversationId, isActive: this.props.updateActiveStatusPopup.isChecked };
        this.props.updateActiveStatus(params);
    }

    onCloseConfirmPopupChangeActiveStaus = () => {
        this.props.setUpateActivePopup({ isShowPopup: false });
        this.props.getGridData(this.props.gridParams);
    }

    handleEdit = (chatTypeId, conversationId) => {
        if (parseInt(chatTypeId, 10) === ChatTypeMapping.Personal) {
            this.props.openUpdatePersonalMappingPopup(conversationId);
        } else {
            this.props.openUpdateGroupMappingPopup(conversationId);
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
                <HeaderCell colWidth={30}>{LangHelper.getResource('Name')}</HeaderCell>
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
        if (this.props.gridData.length > 0) {
            return this.props.gridData.map((item, index) => this.renderRow(item, index, isHost));
        }

        return [<Row key={`body${1}`} ><Cell className='text-center' colSpan={isHost ? 13 : 12}>{LangHelper.getResource('ThereIsNoData')}</Cell></Row>];
    }

    renderIsActiveCol = (isHost, conversationId, isActive, numOfRow) => {
        if (isHost) {
            return (
                <Cell rowSpan={numOfRow} className='text-center'>
                    <Form.Control className='checkbox-cell-style'
                        id={`active-chk-${conversationId}`}
                        type='checkbox'
                        checked={isActive}
                        onChange={(e) => this.onChangeActiveStaus(conversationId, e.target.checked)} />
                </Cell>
            );
        }

        return <Cell rowSpan={numOfRow} className='text-center'>{isActive ? <span className="fa fa-check badge-green" /> : ''}</Cell>;
    }

    renderRow = (item, index, isHost) => {
        let groupConversation = this.props.gridData.filter(i => i.conversationId === item.conversationId);
        const numOfRowSpan = groupConversation.length;
        if (index === 0 || this.props.gridData[index - 1].conversationId !== item.conversationId) {
            return (
                <Row key={index}>
                    <Cell rowSpan={numOfRowSpan} className='text-center'>{index + this.startIndex}</Cell>
                    <Cell rowSpan={numOfRowSpan}>{item.chatType}</Cell>
                    <Cell rowSpan={numOfRowSpan}>{item.botName}</Cell>
                    <Cell rowSpan={numOfRowSpan}>
                        {item.name}
                        {
                            item.chatTypeId === ChatTypeMapping.Group &&
                            <span className="icon-copy fa fa-copy" onClick={this.copyGroupIdToClipboard(item.GroupId)} title='Coppy to clipboard' />
                        }
                    </Cell>
                    <Cell rowSpan={numOfRowSpan}>{item.recipientType}</Cell>
                    <Cell rowSpan={numOfRowSpan}>{item.groupHeadline}</Cell>
                    <Cell>{item.recipient}</Cell>
                    <Cell>{item.letstalkId}</Cell>
                    <Cell rowSpan={numOfRowSpan}>{item.language}</Cell>
                    <Cell rowSpan={numOfRowSpan} className='text-center'><span className={Site.mappingStatusClass(item.mappingStatusId)}>{item.chatType === 'Group' ? item.mappingStatus : ''}</span></Cell>
                    {this.renderIsActiveCol(isHost, item.conversationId, item.isActive, numOfRowSpan)}
                    <Cell rowSpan={numOfRowSpan}>{item.remark}</Cell>
                    {
                        isHost &&
                        <Cell rowSpan={numOfRowSpan} className='text-center'>
                            <span className="fa fa-pencil icon-edit"
                                onClick={() => this.handleEdit(item.chatType, item.conversationId)}
                            />
                        </Cell>
                    }
                </Row>
            );
        }

        return (
            <Row key={index}>
                <Cell>{item.recipient}</Cell>
                <Cell>{item.letstalkId}</Cell>
            </Row>
        );
    }

    render() {
        if (!this.props.gridData) {
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

        console.log(this.props.updatePersonalMappingData);
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
                                When a group is <strong>{this.props.updateActiveStatusPopup.isChecked ? 'actived' : 'deactived'}</strong>. The system will <strong>{this.props.updateActiveStatusPopup.isChecked ? '' : 'not'}</strong> send notifications to the group.
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