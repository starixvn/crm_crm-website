import React from 'react';
import DatePicker from 'react-datepicker';
import { Form } from 'react-bootstrap';

export default class DatePickerField extends React.Component {
    constructor(props) {
        super(props);
    }

    openDatepicker = () => this._calendar.setOpen(true);

    render() {
        const { className, disabled, ...rest } = this.props;
        const ExampleCustomInput = ({ value, onClick }) => (
            <Form.Control as='input' readOnly disabled={disabled} onClick={onClick} value={value} />
        );

        return (
            <React.Fragment>
                <DatePicker
                    className={`form-control form-control-sm datetime-picker ${className}`}
                    dateFormat={this.props.showTimeSelect ? 'MMM dd, yyyy hh:mm aa' : 'MMM dd, yyyy'}
                    timeFormat='hh:mm aa'
                    timeIntervals={5}
                    ref={c => this._calendar = c}
                    customInput={<ExampleCustomInput />}
                    {...rest}
                />
                <i className='date-picker-icon fa fa-calendar' onClick={this.openDatepicker} />
            </React.Fragment>
        );
    }
}