import React from 'react';
import { ModalDialog } from 'react-bootstrap';
import Draggable from 'react-draggable';
import DraggableModalDialog from '../DraggableModalDialog';
import { shallow } from 'enzyme';

describe('DraggableModalDialog', () => {
    it('has correct snapshots', () => {
        // Action
        const component = shallow(<DraggableModalDialog />);

        // Assert
        expect(component).toMatchSnapshot();
    });

    it('has correct children with props', () => {
        // Arrange
        const props = {
            value: 'value'
        };

        // Action
        const component = shallow(<DraggableModalDialog {...props} />);

        // Assert
        expect(component.find(Draggable).props().handle).toBe('.modal-header');
        expect(component.find(Draggable).props().grid).toEqual([10, 10]);
        expect(component.find(ModalDialog).props()).toEqual(props);
    });
});