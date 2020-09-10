import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Alert } from 'react-bootstrap';
import lodash from 'lodash';
import FormikErrorMessage from '../FormikErrorMessage';
import { shallow } from 'enzyme';

describe('FormikErrorMessage', () => {
    it('has correct snapshots', () => {
        // Action
        const component = shallow(<FormikErrorMessage />);

        // Assert
        expect(component).toMatchSnapshot();
    });

    describe('render', () => {
        it('returns null with empty errors', () => {
            // Arrange
            const props = {
                errors: []
            };

            // Action
            const component = shallow(<FormikErrorMessage {...props} />);

            // Assert
            expect(component).toBeNull;
        });
        
        it('returns null with empty touched', () => {
            // Arrange
            const props = {
                errors: [ { value: true } ],
                touched: []
            };

            // Action
            const component = shallow(<FormikErrorMessage {...props} />);

            // Assert
            expect(component).toBeNull;
        });

        describe('returns correct component', () => {
            it('has correct Alert', () => {
                // Arrange
                const props = {
                    errors: [{ value: true }],
                    touched: [{ value: true }]
                };
    
                // Action
                const component = shallow(<FormikErrorMessage {...props} />);

                // Assert
                expect(component.find(Alert).props().variant).toBe('danger');
            });

            it('has correct ul', () => {
                // Arrange
                const props = {
                    errors: [{ value: true }],
                    touched: [{ value: true }]
                };
    
                // Action
                const component = shallow(<FormikErrorMessage {...props} />);

                // Assert
                expect(component.find('ul').props().className).toBe('list-message-error');
            });

            it('has ul with correct children', () => {
                // Arrange
                const props = {
                    errors: [{ value: 'error1' }, { value: 'error2' }],
                    touched: [{ value: false }, { value: false }],
                    fieldNames: [ '0', '1' ]
                };

                const expected = props.fieldNames.map(field => <li key={field}>{props.errors[field]}</li>);
    
                // Action
                const component = shallow(<FormikErrorMessage {...props} />);
                component.instance().validateFormikField = jest.fn(() => true);

                // Assert
                expect(component.find('ul').props().children).toEqual(expected);
            });
        });
    });

    describe('validateFormikField', () => {
        it('runs with touched and error is true', () => {
            // Arrange
            const props = {
                errors: [false],
                touched: [false]
            };

            const component = shallow(<FormikErrorMessage {...props} />);

            // Action
            const result = component.instance().validateFormikField(props.errors, props.touched, 0);

            // Assert
            expect(result).toBeTrue;
        });

        it('runs with touched and error is false', () => {
            // Arrange
            const props = {
                errors: [false],
                touched: [true]
            };

            const component = shallow(<FormikErrorMessage {...props} />);

            // Action
            const result = component.instance().validateFormikField(props.errors, props.touched, 0);

            // Assert
            expect(result).toBeFalse;
        });
    });
});