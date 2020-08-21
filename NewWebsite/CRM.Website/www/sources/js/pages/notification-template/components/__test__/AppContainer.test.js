import React from 'react';
import { AppContainer } from '../AppContainer';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import { shallow } from 'enzyme';

const mockStore = configureMockStore();
const store = mockStore({});

describe('AppContainer', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('render AppContainer', () => {
        // Act
        const wrapper = shallow(
            <Provider store={store}>
                <AppContainer />
            </Provider>
        );

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
            const container = shallow(
                <Provider store={store}>
                    <AppContainer {...props} />
                </Provider>
            );

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