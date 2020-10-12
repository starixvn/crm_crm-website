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

    it('renders default component correctly', () => {
        // Act
        const container = shallow(<AppContainer/>);

        // Assert
        expect(container).toMatchSnapshot();
        expect(container.find(Filter).length).toBe(1);
        expect(container.find(DataGrid).length).toBe(1);
        expect(container.find(NewNotificationTypeWrapper).length).toBe(0);
        expect(container.find(EditNotificationTypeWrapper).length).toBe(0);
    });

    describe('create notification type', () => {
        it('render component NewNotificationType correctly', () => {
            // Arrange
            const props = {
                createNotificationTypePopup: { show: true }
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
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseCreatePopup();

            // Assert
            expect(props.setCreateNotificationTypePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setCreateNotificationTypeResult).toHaveBeenCalledWith(null);
        });

        it('should call correct functions when create notification type success', () => {
            // Arrange
            const props = {
                getNotificationTypeGridData: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseCreatePopup = jest.fn();

            // Act
            container.handleCreateSuccess();

            // Assert
            expect(props.getNotificationTypeGridData).toHaveBeenCalledWith(props.notificationTypeGridParams);
            expect(container.handleCloseCreatePopup).toHaveBeenCalled();
        });
    });

    describe('edit notification type', () => {
        it('render component EditNotificationType correctly', () => {
            // Arrange
            const props = {
                editNotificationTypePopup: { show: true }
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
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseEditPopup();

            // Assert
            expect(props.setEditNotificationTypePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setEditNotificationTypeResult).toHaveBeenCalledWith(null);
        });

        it('should call correct functions when edit notification type successful', () => {
            // Arrange
            const props = {
                getNotificationTypeGridData: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();

            container.handleCloseEditPopup = jest.fn();

            // Act
            container.handleEditSuccess();

            // Assert
            expect(props.getNotificationTypeGridData).toHaveBeenCalledWith(props.notificationTypeGridParams);
            expect(container.handleCloseEditPopup).toHaveBeenCalled();
        });
    });

    describe('delete notification type', () => {
        it('render component DeleteNotificationType correctly', () => {
            // Arrange
            const props = {
                deleteNotificationTypePopup: { show: true, name: 'test123' },
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
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseDeletePopup();

            // Assert
            expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledWith({ show: false });
        });

        it('should call correct functions when delete successful', async () => {
            // Arrange
            const promise = Promise.resolve({ isSuccessful: true, message: '' });
            const props = {
                deleteNotificationTypePopup: { notificationTypeId: 1, name: 'voiding bet'},
                deleteNotificationType: jest.fn(() => promise),
                getNotificationTypeGridData: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseDeletePopup = jest.fn();

            // Act
            container.handleDelete();
            await promise;

            // Assert
            expect(props.deleteNotificationType).toHaveBeenCalledWith({ notificationTypeId: 1, notificationTypeName: 'voiding bet'});
            expect(toast.success).toHaveBeenCalledWith('DeleteNotificationTypeSuccessfully');
            expect(container.handleCloseDeletePopup).toHaveBeenCalled();
            expect(props.getNotificationTypeGridData).toHaveBeenCalledWith(props.notificationTypeGridParams);

        });

        it('should call correct when delete unsuccessful', async () => {
            // Arrange
            const promise = Promise.resolve({ isSuccessful: false, message: 'unsuccessful' });
            const props = {
                deleteNotificationTypePopup: { notificationTypeId: 1, name: 'voiding bet' },
                deleteNotificationType: jest.fn(() => promise),
                getNotificationTypeGridData: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseDeletePopup = jest.fn();

            // Act
            container.handleDelete();
            await promise;

            // Assert
            expect(props.deleteNotificationType).toHaveBeenCalledWith({notificationTypeId: 1, notificationTypeName: 'voiding bet'});
            expect(toast.error).toHaveBeenCalledWith('unsuccessful');
            expect(container.handleCloseDeletePopup).toHaveBeenCalled();
            expect(props.getNotificationTypeGridData).toHaveBeenCalledWith(props.notificationTypeGridParams);
        });
    });
});