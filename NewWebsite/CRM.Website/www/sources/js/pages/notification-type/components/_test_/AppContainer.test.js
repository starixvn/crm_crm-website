import React from 'react';
import AppContainerWrapper, { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import LangHelper from '@jsroot/common/langHelper';
import { shallow } from 'enzyme';
import { toast } from 'react-toastify';
import NewNotificationTypeWrapper from '../NewNotificationType';
import EditNotificationTypeWrapper from '../EditNotificationType';

jest.mock('react-toastify');
const mockStore = configureMockStore();
const store = mockStore({});

describe('AppContainer', () => {
    beforeEach(() => {
        LangHelper.getResource = jest.fn(value => value);
    });

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

    it('renders default component correctly', () => {
        // Arrange
        const props = {
            isHost: false,
            createNotificationTypePopup: {},
            editNotificationTypePopup: {},
            deleteNotificationTypePopup: {}
        };

        // Act
        const container = shallow(<AppContainer {...props} />);

        // Assert
        expect(container.find(Filter).length).toBe(1);
        expect(container.find(Filter).props().isHost).toBe(props.isHost);
        expect(container.find(DataGrid).length).toBe(1);
        expect(container.find(DataGrid).props().isHost).toBe(props.isHost);
        expect(container.find(NewNotificationTypeWrapper).length).toBe(0);
        expect(container.find(EditNotificationTypeWrapper).length).toBe(0);
    });

    describe('create notification type', () => {
        it('render component NewNotificationType correctly', () => {
            // Arrange
            const props = {
                createNotificationTypePopup: { show: true },
                editNotificationTypePopup: {},
                deleteNotificationTypePopup: {}
            };

            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(NewNotificationTypeWrapper).length).toBe(1);
        });

        it('should call correct functions when close popup', () => {
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

        it('should call correct functions when create notification type success', () => {
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
        it('render component EditNotificationType correctly', () => {
            // Arrange
            const props = {
                createNotificationTypePopup: {},
                editNotificationTypePopup: { show: true },
                deleteNotificationTypePopup: {}
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(EditNotificationTypeWrapper).length).toBe(1);
        });

        it('should call correct functions when close popup', () => {
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

        it('should call correct functions when edit notification type successful', () => {
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
        it('render component DeleteNotificationType correctly', () => {
            // Arrange
            const props = {
                deleteNotificationTypePopup: { show: true, name: 'test123' },
                gridParams: {},
                createNotificationTypePopup: {},
                editNotificationTypePopup: {}
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(ConfirmPopup).length).toBe(1);
            expect(container.find(ConfirmPopup).props().size).toEqual('lg');
            expect(container.find(ConfirmPopup).props().title).toEqual('Confirmation');
            expect(container.find(ConfirmPopup).props().content)
                .toEqual(<div>Are you sure you want to delete <strong>test123</strong>?</div>);
        });

        it('should call correct functions when close popup', () => {
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

        it('should calls correct functions when delete successful', async () => {
            // Arrange
            const promise = Promise.resolve({ isSuccessful: true, message: '' });
            const props = {
                deleteNotificationTypePopup: { notificationTypeId: 1 },
                deleteNotificationType: jest.fn(() => promise),
                getGridData: jest.fn(),
                gridParams: {},
                createNotificationTypePopup: {},
                editNotificationTypePopup: {}
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseDeletePopup = jest.fn();

            // Act
            container.handleDelete();
            await promise;

            // Assert
            expect(props.deleteNotificationType).toHaveBeenCalledWith(1);
            expect(toast.success).toHaveBeenCalledWith('DeleteNotificationTypeSuccessfully');
            expect(container.handleCloseDeletePopup).toHaveBeenCalled();
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);

        });

        it('should calls toast.error when delete unsuccessful', async () => {
            // Arrange
            const promise = Promise.resolve({ isSuccessful: false, message: 'unsuccessful' });
            const props = {
                deleteNotificationTypePopup: { notificationTypeId: 1 },
                deleteNotificationType: jest.fn(() => promise),
                getGridData: jest.fn(),
                gridParams: {},
                createNotificationTypePopup: {},
                editNotificationTypePopup: {}
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleDelete();
            await promise;

            // Assert
            expect(props.deleteNotificationType).toHaveBeenCalledWith(1);
            expect(toast.error).toHaveBeenCalledWith('unsuccessful');
        });
    });
});