import React from 'react';
import { Button } from 'react-bootstrap';
import { ContentDetailPopup } from '../ContentDetailPopup';
import { shallow } from 'enzyme';

describe('ContentDetailPopup', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<ContentDetailPopup />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            onClose: jest.fn()
        };
        const container = shallow(<ContentDetailPopup {...props} />);

        // Act
        container.find(Button).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });
});