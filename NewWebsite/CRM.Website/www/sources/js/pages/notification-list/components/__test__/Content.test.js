import React from 'react';
import Content from '../Content';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';

describe('Content', () => {
    it('render component correctly', () => {
        // Arrange
        const props = {
            data: {
                headlines: []
            }
        };

        // Act
        const content = shallow(<Content {...props} />);

        // Assert
        expect(content).toMatchSnapshot();
    });
    
    it('render child element', () => {
        // Arrange
        const props = {
            data: {
                notificationId: 1,
                messageId: 1,
                headlines: [ {headlineName: 'name'} ],
                content: 'content'
            }
        };

        const expectedTilte = `${LangHelper.getResource('NotificationID')}: ${props.data.notificationId} (${LangHelper.getResource('MesageID')}: ${props.data.messageId})`;

        // Act
        const content = shallow(<Content {...props} />);

        // Assert
        expect(content.find(Modal.Title).html()).toContain(expectedTilte);
        expect(content.find(Modal.Body).find('div').at(0).html()).toEqual(`<div>Name: Group ${props.data.headlines.map(gh => gh.headlineName).join(', ')}</div>`);
        expect(content.find(Modal.Body).find('div').at(1).html()).toEqual(`<div class="border p-2 mt-2 mb-3 notification-content">${props.data.content}</div>`);
    });

    it('simulates close button', () => {
        // Arrange
        const props = {
            data: {
                notificationId: 1,
                messageId: 1,
                headlines: [ {headlineName: 'name'} ],
                content: 'content'
            },
            onClose: jest.fn()
        };

        const container = shallow(<Content {...props} />);

        // Act
        container.find(Button).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });
});