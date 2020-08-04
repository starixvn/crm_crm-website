import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import LangHelper from '@jsroot/common/langHelper';
import { PAGING_SIZES } from '@jsroot/common/constants';

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
                <HeaderCell colWidth={20}>{LangHelper.getResource('NotificationType')}</HeaderCell>
                <HeaderCell colWidth={20}>{LangHelper.getResource('TemplateName')}</HeaderCell>
                <HeaderCell colWidth={100}>
                    {LangHelper.getResource('Title')}/{LangHelper.getResource('Content')}
                </HeaderCell>
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

        return [<Row key={`body${1}`} ><Cell colSpan="5">There is no data</Cell></Row>];
    }

    renderRow = (item, index, isHost) => (
        <Row key={item.notificationTemplateId} >
            <Cell className="text-center">{index + this.startIndex}</Cell>
            <Cell>{item.notificationType}</Cell>
            <Cell>{item.name}</Cell>
            <Cell></Cell>
            {
                isHost &&
                <Cell className="text-center">
                    <span className="icon-edit" data-templateid={item.notificationTemplateId} />
                    <span className="icon-delete" data-templateid={item.notificationTemplateId} />
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
            PageList: PAGING_SIZES
        };

        return (
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