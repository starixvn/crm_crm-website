import React from 'react';
import { Alert } from 'react-bootstrap';
import ResponseMessage from '../ResponseMessage';
import { shallow } from 'enzyme';

describe('ResponseMessage', () => {
    it('has correct snapshots', () => {
        // Action
        const component = shallow(<ResponseMessage />);

        // Assert
        expect(component).toMatchSnapshot();
    });

    describe('render', () => {
        it('has regular children', () => {
            const props = {
                message: 'message'
            };

            // Action
            const component = shallow(<ResponseMessage {...props} />);
    
            // Assert
            expect(component.find(Alert).props().className).toBe('d-flex');
            expect(component.find('div')).toHaveLength(2);
            expect(component.find('div').at(1).html()).toBe(`<div class="ml-2">${props.message}</div>`);
        });

        it('has correct children when is successful', () => {
            // Arrange
            const props = {
                isSuccessful: true,
                message: 'message'
            };
    
            // Action
            const component = shallow(<ResponseMessage {...props} />);
    
            // Assert
            expect(component.find(Alert).props().variant).toBe('success');
            expect(component.find('i').props().className).toBe('fa fa-check');
        });
        
        it('has correct children when is not successful', () => {
            // Arrange
            const props = {
                isSuccessful: false,
                message: 'message'
            };
    
            // Action
            const component = shallow(<ResponseMessage {...props} />);
    
            // Assert
            expect(component.find(Alert).props().variant).toBe('danger');
            expect(component.find('i').props().className).toBe('fa fa-exclamation-triangle');
        });
    });
});