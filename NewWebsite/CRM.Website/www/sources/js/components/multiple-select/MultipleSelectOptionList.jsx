import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Option from './Option';

export default class MultipleSelectOptionList extends Component {
    static propTypes = {
        dataSource: PropTypes.arrayOf(PropTypes.object).isRequired,
        id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
        onChange: PropTypes.func
    }

    constructor(props) {
        super(props);
    }

    _renderOptionList() {
        const { dataSource, onChange, id } = this.props;
        if (!dataSource || !dataSource.length) {
            return <span>No information</span>;
        }

        const optionList = dataSource.map((item, index) => {
            return item.visible && (
                <Option
                    id={`${id}-optionItem-${index}`}
                    key={`${id}optionItem${index}`}
                    itemData={item}
                    onChange={onChange}
                />
            );
        });

        return optionList;
    }

    _calculateMinHeightOptionList = maxLengOfSubData => {
        let minHeight = 0;
        if (maxLengOfSubData && maxLengOfSubData > 0 && maxLengOfSubData < 5) {
            minHeight = maxLengthOfSubData * 40;
        }

        if (maxLengOfSubData > 5) {
            minHeight = 150;
        }

        return minHeight;
    }

    render() {
        const dataSource = this.props.dataSource;
        const maxLengthOfSubData = dataSource.length > 0 ? Math.max(...dataSource.filter(item => item.subData).map(i => i.subData.length)) : 0;
        const minHeight = this._calculateMinHeightOptionList(maxLengthOfSubData);
        return (
            <ul className="multiple-select-options" style={{ minHeight: minHeight }}>
                {this._renderOptionList()}
            </ul>
        );
    }
}