import React from 'react';
import { ToastContainer, Slide } from 'react-toastify';
import Layout from '../Layout';
import { shallow } from 'enzyme';

describe('Layout', () => {
    it('has correct snapshots', () => {
        // Action
        const component = shallow(<Layout />);

        // Assert
        expect(component).toMatchSnapshot();
    });
    
    describe('render', () => {
        it('has correct children', () => {
            // Arrange
            const Expected = () => <div>Expected</div>;
    
            const props = {
                children: <Expected />
            };
    
            // Action
            const component = shallow(<Layout {...props} />);
    
            // Assert
            expect(component.find(ToastContainer).props().limit).toBe(1);
            expect(component.find(ToastContainer).props().position).toBe('top-center');
            expect(component.find(ToastContainer).props().autoClose).toBe(3000);
            expect(component.find(ToastContainer).props().hideProgressBar).toBeFalse;
            expect(component.find(ToastContainer).props().transition).toEqual(Slide);
            expect(component.find(ToastContainer).props().closeOnClick).toBeFalse;
            expect(component.find(ToastContainer).props().pauseOnHover).toBeTrue;
            expect(component.find(ToastContainer).props().draggable).toBeFalse;
            expect(component.find(Expected).length).toBe(1);
        });
    });
});