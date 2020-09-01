import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Option extends Component {
    static propTypes = {
        itemData: PropTypes.object.isRequired,
        id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
        onChange: PropTypes.func
    }

    constructor(props) {
        super(props);

        this.state = {
            isExpandedSubData: false
        };
    }

    onChange = event => {
        event.stopPropagation();

        const { itemData, onChange } = this.props;

        const newItemData = {
            key: itemData.key,
            value: itemData.value,
            checked: !itemData.checked
        };
        onChange && onChange(newItemData);
    }

    onClickExpandIcon = () => {
        this.setState(prevState => ({
            isExpandedSubData: !prevState.isExpandedSubData
        }));
    }

    renderSubData = subDataList => {
        return (
            <ul className='sub-data'>
                {
                    this.state.isExpandedSubData &&
                    subDataList.map((item, key) => <li key={`sub-${key}`}>{item}</li>)
                }
            </ul>
        );
    };

    render() {
        const { itemData, id } = this.props;
        const isHasSubData = itemData.subData && itemData.subData.length > 0;
        return (
            <li className="multiple-select-item">
                <input
                    id={id}
                    type="checkbox"
                    className="option-checkbox"
                    checked={itemData.checked}
                    onChange={this.onChange}
                />
                <label className="option-label" htmlFor={id}>
                    {itemData.value}
                </label>
                {
                    isHasSubData &&
                    <span className={`expand-icon fa ${this.state.isExpandedSubData ? 'fa-caret-down' : 'fa-caret-right'}`} onClick={this.onClickExpandIcon} />
                }
                {
                    isHasSubData &&
                    this.renderSubData(itemData.subData)
                }
            </li>
        );
    }
}