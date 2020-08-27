import React from 'react';
import Content from '../Content';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';

describe('Content', () => {
    it('render component correctly', () => {
        // Arrange
        const props = {
            data: {
                notificationId: 1,
                messageId: 1,
                headlines: [ {headlineName: 'name'} ],
                content: 'content'
            }
        };

        // Act
        const content = shallow(<Content {...props} />);

        // Assert
        expect(content).toMatchSnapshot();
    });
});