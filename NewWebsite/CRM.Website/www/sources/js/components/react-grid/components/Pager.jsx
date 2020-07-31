import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Pager extends Component {
    constructor(props) {
        super(props);

        this.state = {
            inputValue: this.props.pageOption.PageIndex
        };
    }

    static propTypes = {
        pageOption: PropTypes.shape({
            PageIndex: PropTypes.number,
            PageSize: PropTypes.number,
            PageList: PropTypes.arrayOf(PropTypes.number),
            TotalItem: PropTypes.number
        }),
        onPaging: PropTypes.func,
        isShowPagingInfo: PropTypes.bool,
        isAllowInputPageIndex: PropTypes.bool
    };

    static defaultProps = {
        pageOption: {
            PageIndex: 1,
            PageSize: 50,
            PageList: [50, 100, 200],
            TotalItem: 0
        },
        isShowPagingInfo: true,
        isAllowInputPageIndex: true
    };

    componentDidUpdate(prevProps, prevState) {
        const pageIndex = this.props.pageOption.PageIndex;
        if (pageIndex !== prevProps.pageOption.PageIndex) {
            this._updateState(pageIndex);
        }
    }

    goToPage(index, pageSize) {
        this.props.onPaging && this.props.onPaging(index, pageSize);
    }

    onKeyUpHandle = (e, totalPage) => {
        const code = e.keyCode ? e.keyCode : e.which;
        if (code === 13) {
            var value = parseInt(this.state.inputValue);
            if (value > 0 && value <= totalPage) {
                this.goToPage(value, this.props.pageOption.PageSize);
            }
        }
    }

    onClickHandle(isDisable, handleFunc) {
        if (!isDisable) {
            handleFunc();
        }
    }

    onInputChangeHandler = (e) => {
        const pattern = /^\d*$/;
        const { value } = e.target;
        const isValidNumber = pattern.test(value);
        if (isValidNumber) {
            this._updateState(value);
        }
    }

    _updateState(inputValue) {
        this.setState({ inputValue });
    }

    _renderPageSizeSelection(isDisable, pageIndex, pageSize) {
        return (
            <select className="pagesize-select" disabled={isDisable} value={pageSize} onChange={(e) => this.goToPage(1, Number(e.target.value))}>
                {this._renderPageSizeOption(this.props.pageOption.PageList)}
            </select>
        );
    }

    _renderPageSizeOption(pageList) {
        return pageList.map((item, index) => {
            return (<option key={`pagesize${index}`} value={item}>{item}</option>);
        });
    }

    _renderPageIcon(iconClass, isDisable, onClickFunc) {
        return (<a className={`fa ${iconClass} ${isDisable ? "disable" : ""}`} onClick={() => this.onClickHandle(isDisable, onClickFunc)}></a>);
    }

    _renderPageInput(index, totalPage) {
        return (
            <div className="page-input"><span>Page </span><input type="text" className="page-form" value={index} maxLength="4" size="2"
                onChange={this.onInputChangeHandler}
                onKeyUp={(e) => this.onKeyUpHandle(e, totalPage)} />
                <span>{` of ${totalPage}`} </span>
            </div>);
    }

    _renderPageLabel(index, totalPage) {
        return (<div className="page-input"><span>Page {`${index} of ${totalPage} `}</span></div>);
    }

    _renderPageInfo(index, pageSize, totalItem) {
        const viewFrom = ((index - 1) * pageSize) + 1;
        const viewTo = index * pageSize;
        return (<span className="page-info">{`Display ${viewFrom} to ${viewTo > totalItem ? totalItem : viewTo} of ${totalItem} item(s)`}</span>);
    }

    render() {
        const { isShowPagingInfo, isAllowInputPageIndex, pageOption } = this.props;
        const { PageIndex, PageSize, TotalItem, PageList } = pageOption;
        const { inputValue } = this.state;

        const isDisable = isNaN(parseInt(inputValue));

        const isShow = TotalItem > PageList[0];
        const totalPage = Math.ceil(TotalItem / PageSize);

        return (
            <div className="page-container" style={{ display: `${isShow ? 'block' : 'none'}` }}>
                Page size {this._renderPageSizeSelection(isDisable, PageIndex, PageSize)}
                {this._renderPageIcon("fa-angle-double-left", PageIndex === 1, () => this.goToPage(1, PageSize))}
                {this._renderPageIcon("fa-angle-left", isDisable || PageIndex <= 1 || PageIndex > totalPage, () => this.goToPage(PageIndex - 1, PageSize))}
                {isAllowInputPageIndex ? this._renderPageInput(inputValue, totalPage) : this._renderPageLabel(PageIndex, totalPage)}
                {this._renderPageIcon("fa-angle-right", isDisable || PageIndex >= totalPage || PageIndex < 1, () => this.goToPage(PageIndex + 1, PageSize))}
                {this._renderPageIcon("fa-angle-double-right", PageIndex === totalPage, () => this.goToPage(totalPage, PageSize))}
                {isShowPagingInfo && this._renderPageInfo(PageIndex, PageSize, TotalItem)}
            </div>);
    }
}