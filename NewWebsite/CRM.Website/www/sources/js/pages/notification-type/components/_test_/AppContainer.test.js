import React from 'react';
import AppContainerWrapper, { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
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

    it('component has correct props', () => {
        const initialState = {
            gridParams: {},
            createNotificationTypePopup: {},
            editNotificationTypePopup: {},
            deleteNotificationTypePopup: {}
        };

        const store = mockStore(initialState);

        const wrapper = shallow(<AppContainerWrapper store={store} />)

        expect(wrapper.props().children.props.gridParams).toEqual(
            initialState.gridParams
        );
        expect(wrapper.props().children.props.createNotificationTypePopup).toEqual(
            initialState.createNotificationTypePopup
        );
        expect(wrapper.props().children.props.editNotificationTypePopup).toEqual(
            initialState.editNotificationTypePopup
        );
        expect(wrapper.props().children.props.deleteNotificationTypePopup).toEqual(
            initialState.deleteNotificationTypePopup
        );
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

        it('should call correct when create notification type success', () => {
            // Arrange
            const props = {
                getGridData: jest.fn()
            };
            const container = new AppContainer(props);
            container.handleCloseCreatePopup = jest.fn();

            // Act
            container.handleCreateSuccess();

            // Assert
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
            expect(container.handleCloseCreatePopup).toHaveBeenCalled();


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

        it('should call correct when edit notification type success', () => {
            // Arrange
            const props = {
                getGridData: jest.fn()
            };
            const container = new AppContainer(props);
            container.handleCloseEditPopup = jest.fn();

            // Act
            container.handleEditSuccess();

            // Assert       
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
            expect(container.handleCloseEditPopup).toHaveBeenCalled();
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

        it('should call setDeleteNotificationTypePopup when close popup', () => {
            // Arrange
            const props = {
                setDeleteNotificationTypePopup: jest.fn(),
                deleteNotificationTypePopup: { show: true }
            };
            const container = new AppContainer(props);

            // Act
            container.handleCloseDeletePopup();

            // Assert
            expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledWith({ show: false });
        });

        it('should call deleteNotificationType when run handleDelete', () => {
            // Arrange
            const props = {
                deleteNotificationType: jest.fn(() => Promise.resolve({ data: {} })),
                deleteNotificationTypePopup: {},
                getGridData: jest.fn()
            };

            const container = new AppContainer(props);

            // Act
            container.handleDelete();

            // Assert
            expect(props.deleteNotificationType).toHaveBeenCalledTimes(1);
        });

        // it('should call handleCloseDeletePopup, getGridData when delete success', () => {
        //     // Arrange
        //     const props = {
        //         deleteNotificationType: jest.fn(() => Promise.resolve({ data:{ isSuccessful: true}})),
        //         deleteNotificationTypePopup: {},
        //         getGridData: jest.fn()
        //     };

        //     const container = new AppContainer(props);
        //     container.handleCloseDeletePopup = jest.fn();

        //     // Act
        //     container.handleDelete();

        //     // Assert
        //     //expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
        //     expect(container.handleCloseDeletePopup).toHaveBeenCalled();
        // });
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
                deleteNotificationTypePopup: { show: true },
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