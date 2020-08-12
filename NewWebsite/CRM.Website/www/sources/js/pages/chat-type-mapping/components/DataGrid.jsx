import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { PAGING_SIZES, defaultPageSize, ChatTypeMapping } from '@jsroot/common/constants';
import Site from '@jsroot/common/site'
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

	handleChangeActiveStaus = (conversationId, isChecked) => {
		console.log(isChecked);
	}

	handleEdit = (chatType, conversationId) => {
		console.log('Handle Edit');
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
				<HeaderCell colWidth={15}>{LangHelper.getResource('Lestalk ID')}</HeaderCell>
				<HeaderCell colWidth={15}>{LangHelper.getResource('Language')}</HeaderCell>
				<HeaderCell colWidth={20}>{LangHelper.getResource('Mapping Status')}</HeaderCell>
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
			return <Cell rowSpan={numOfRow} className='text-center'>
				<Form.Control className='checkbox-cell-style'
					id={`active-chk-${conversationId}`}
					type='checkbox'
					checked={isActive}
					onChange={(e) => this.handleChangeActiveStaus(conversationId, e.target.checked)} />
			</Cell>;
		}

		return <Cell className='text-center'>{isActive ? <span>v</span> : ''}</Cell>;
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
					<Cell rowSpan={numOfRowSpan}>{item.name}</Cell>
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
			</Fragment>
		);
	}
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
	const { gridParams, gridData, settings } = state;
	return {
		gridParams,
		gridData,
		settings
	};
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);