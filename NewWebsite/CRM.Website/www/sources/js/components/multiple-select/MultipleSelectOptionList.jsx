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

    render() {
        const dataSource = this.props.dataSource;
        const maxLengthOfSubData = dataSource.length > 0 ? Math.max(...dataSource.map(item => item.subData && item.subData.length)) : 0;
        const minHeight = maxLengthOfSubData === 0 || maxLengthOfSubData > 6 ? 0 : maxLengthOfSubData * 40;
        return (
            <ul className="multiple-select-options" style={{ minHeight: minHeight }}>
                {this._renderOptionList()}
            </ul>
        );
    }
}