import React from 'react';
import { Button } from 'react-bootstrap';
import { NewNotificationTemplate } from '../NewNotificationTemplate';
import { shallow, mount } from 'enzyme';

describe('NewNotificationTemplate', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<NewNotificationTemplate />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            onClose: jest.fn()
        };
        const container = mount(<NewNotificationTemplate {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });
});