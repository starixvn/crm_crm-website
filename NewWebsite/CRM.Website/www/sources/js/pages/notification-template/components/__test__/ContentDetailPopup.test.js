import React from 'react';
import { Button, Modal } from 'react-bootstrap';
import { ContentDetailPopup } from '../ContentDetailPopup';
import { shallow } from 'enzyme';

describe('ContentDetailPopup', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('render popup', () => {
        // Arrange
        const props = {
            template: { title: 'notification template', contents: [] }
        };

        // Act
        const container = shallow(<ContentDetailPopup {...props} />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('render child element', () => {
        // Arrange
        const props = {
            template: {
                title: 'notification template',
                contents: [{ languageId: 1, language: 'EN', content: 'content' }]
            }
        };

        // Act
        const container = shallow(<ContentDetailPopup {...props} />);

        // Assert
        expect(container.find(Modal.Title).html()).toContain('notification template');
        expect(container.find(Modal.Body).find('.content').length).toBe(1);
        expect(container.find(Modal.Body).find('.content').html()).toEqual('<div class="border p-2 mt-2 mb-3 content">content</div>');
    });

    it('simulates close button', () => {
        // Arrange
        const props = {
            template: { title: 'notification template', contents: [] },
            onClose: jest.fn()
        };
        const container = shallow(<ContentDetailPopup {...props} />);

        // Act
        container.find(Button).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });
});