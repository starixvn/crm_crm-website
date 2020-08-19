import React, { Component } from 'react';
import MultipleSelect from '../../../components/multiple-select/index';

export default class MultipleSelectField extends Component {
    constructor(props) {
        super(props);
    }

    onChange = value => {
        this.props.onChange({
            value: value,
            isValid: this.isValid(value)
        });
    }

    isValid = value => value !== '';

    render() {
        let defaultTexts = { SelectOptions: 'Please Select', All: 'All', SelectedItemCount: '# selected' };
        return (
            <div style={{ width: this.props.width ? this.props.width : 'auto' }}
                className={this.props.isSummited && !this.isValid(this.props.selectedValue) ? 'invalid multiple-select-field' : 'multiple-select-field'}
            >
                {
                    this.props.data &&
                    <MultipleSelect id={this.props.id}
                        dataSource={this.props.data}
                        keyField={'id'}
                        valueField={'name'}
                        statusField={'checked'}
                        hasSearchBox={this.props.isSearchable}
                        hasAllOption={this.props.data.length > 0}
                        texts={this.props.texts || defaultTexts}
                        isAllTextShown={this.props.isAllTextShown || false}
                        onChange={(currentItem, selectedItemsKey) => this.onChange(selectedItemsKey)}
                    />
                }
            </div>
        );
    }
}