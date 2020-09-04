import React from 'react';
import { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import LangHelper from '@jsroot/common/langHelper';
import { shallow } from 'enzyme';

const mockStore = configureMockStore();
const store = mockStore({});

describe('AppContainer', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const wrapper = shallow(
            <Provider store={store}>
                <AppContainer />
            </Provider>
        );

        // Assert
        expect(wrapper).toMatchSnapshot();
    });

    describe('create notification type', () => {
        it('render popup', () => {
            // Arrange
            const props = {
                createNotificationTypePopup: { show: true }
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

        it('should call setCreateNotificationTypePopup, setCreateNotificationTypeResult when close popup', () => {
            // Arrange
            const props = {
                setCreateNotificationTypePopup: jest.fn(),
                setCreateNotificationTypeResult: jest.fn(),
                createNotificationTypePopup: { show: true }
            };
            const container = new AppContainer(props);

            // Act
            container.handleCloseCreatePopup();

            // Assert
            expect(props.setCreateNotificationTypePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setCreateNotificationTypeResult).toHaveBeenCalledWith(null);
        });
    });

    describe('edit notification type', () => {
        it('render popup', () => {
            // Arrange
            const props = {
                editNotificationTypePopup: { show: true }
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

        it('should call setEditNotificationTypePopup, setEditNotificationTypeResult when close popup', () => {
            // Arrange
            const props = {
                setEditNotificationTypePopup: jest.fn(),
                setEditNotificationTypeResult: jest.fn(),
                editNotificationTypePopup: { show: true }
            };
            const container = new AppContainer(props);

            // Act
            container.handleCloseEditPopup();

            // Assert
            expect(props.setEditNotificationTypePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setEditNotificationTypeResult).toHaveBeenCalledWith(null);
        });
    });

    describe('delete notification type', () => {
        it('render popup', () => {
            // Arrange
            const props = {
                deleteNotificationTypePopup: { show: true }
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

        it('should call setDeleteNotificationTypePopup, setDeleteNotificationTypeResult when close popup', () => {
            // Arrange
            const props = {
                setDeleteNotificationTypePopup: jest.fn(),
                setDeleteNotificationTypeResult: jest.fn(),
                deleteNotificationTypePopup: { show: true }
            };
            const container = new AppContainer(props);

            // Act
            container.handleCloseDeletePopup();

            // Assert
            expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledWith({ show: false });
        });
    });

    it('render correctly component Filter', () => {
        // Arrange
        const props = {
            gridParams: {},
            createNotificationTypePopup: {},
            editNotificationTypePopup: {},
            deleteNotificationTypePopup: {},
        };

        // Action
        const appContainer = shallow(<AppContainer {...props} />);

        // Assert
        expect(appContainer.find(Filter).length).toBe(1);
    });

    it('render correctly component DataGrid', () => {
        // Arrange
        const props = {
            gridParams: {},
            createNotificationTypePopup: {},
            editNotificationTypePopup: {},
            deleteNotificationTypePopup: {},
        };

        // Action
        const appContainer = shallow(<AppContainer {...props} />);

        // Assert
        expect(appContainer.find(DataGrid).length).toBe(1);
    });

    describe('confirmPopup is showed', () => {
        it('render correctly component ConfirmPopup', () => {
            // Arrange
            const props = {
                gridParams: {},
                createNotificationTypePopup: {},
                editNotificationTypePopup: {},
                deleteNotificationTypePopup: { show: true},
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ConfirmPopup).length).toBe(1);
            expect(appContainer.find(ConfirmPopup).props().size).toEqual('lg');
            expect(appContainer.find(ConfirmPopup).props().title).toEqual(LangHelper.getResource('Confirmation'));
        });
    });
});