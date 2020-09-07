import React from 'react';
import { Button } from 'react-bootstrap';
import { EditNotificationTemplate } from '../EditNotificationTemplate';
import { shallow, mount } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';

describe('EditNotificationTemplate', () => {
    beforeAll(() => {
        LangHelper.getResource = jest.fn((key) => key);
    });

    afterAll(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<EditNotificationTemplate />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            onClose: jest.fn()
        };
        const container = mount(<EditNotificationTemplate {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });

    describe('methods', () => {
    });
});