import React from 'react';
import { AppContainer } from '../AppContainer';
import { shallow } from 'enzyme';

describe('AppContainer', () => {
    afterAll(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const wrapper = shallow(<AppContainer />);

        // Assert
        expect(wrapper).toMatchSnapshot();
    });

    describe('create notification template', () => {
        it('render popup', () => {
            // Arrange
            const props = {
                createTemplatePopup: { show: true }
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
        });

        it('should call setCreateTemplatePopup, setCreateTemplateResult when close popup', () => {
            // Arrange
            const props = {
                setCreateTemplatePopup: jest.fn(),
                setCreateTemplateResult: jest.fn(),
                createTemplatePopup: { show: true }
            };
            const container = new AppContainer(props);

            // Act
            container.handleCloseCreatePopup();

            // Assert
            expect(props.setCreateTemplatePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setCreateTemplateResult).toHaveBeenCalledWith(null);
        });
    });
});