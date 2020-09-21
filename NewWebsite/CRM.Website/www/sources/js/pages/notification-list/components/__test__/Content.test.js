import React from 'react';
import Content from '../Content';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

describe('Content', () => {
    it('renders component correctly', () => {
        // Arrange
        const props = {
            data: {
                headlines: []
            }
        };

        // Action
        const content = shallow(<Content {...props} />);

        // Assert
        expect(content).toMatchSnapshot();
    });
    
    it('has Modal onHide', () => {
        // Arrange
        LangHelper.getResource = jest.fn(value => value);
        const props = {
            data: {
                headlines: [],
            },
            onClose: jest.fn()
        };

        const content = shallow(<Content {...props} />);

        // Action
        content.simulate('hide');

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });
    
    it('renders child element', () => {
        // Arrange
        LangHelper.getResource = jest.fn(value => value);
        const props = {
            data: {
                notificationId: 1,
                messageId: 1,
                headlines: [ {headlineName: 'name'} ],
                content: 'content'
            }
        };

        const expectedTilte = `NotificationID: ${props.data.notificationId} (MessageID: ${props.data.messageId})`;

        // Action
        const content = shallow(<Content {...props} />);

        // Assert
        expect(content.find(Modal).props().show).toBeTrue;
        expect(content.find(Modal).props().backdrop).toBe('static');
        expect(content.find(Modal).props().keyboard).toBeFalse;
        expect(content.find(Modal).props().size).toBe('lg');
        expect(content.find(Modal).props().centered).toBeTrue;
        expect(content.find(Modal).props().dialogAs).toEqual(DraggableModalDialog);
        expect(content.find(Modal.Header).props().closeButton).toBeTrue;
        expect(content.find(Modal.Title).props().as).toBe('div');
        expect(content.find(Modal.Title).html()).toContain(expectedTilte);
        expect(content.find(Modal.Body).find('div').at(0).html()).toEqual(`<div>Letstalk Group ${props.data.headlines.map(gh => gh.headlineName).join(', ')}</div>`);
        expect(content.find(Modal.Body).find('div').at(1).html()).toEqual(`<div class="border p-2 mt-2 mb-3 notification-content">${props.data.content}</div>`);
        expect(content.find(Button).props().variant).toBe('primary');
        expect(content.find(Button).props().size).toBe('sm');
        expect(content.find(Button).html()).toContain('Close');
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

        // Action
        container.find(Button).simulate('click');

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });
});