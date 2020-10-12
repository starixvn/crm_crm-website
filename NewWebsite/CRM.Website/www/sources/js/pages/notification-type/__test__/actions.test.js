import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
import { ActionCreators } from '../actions';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('actionCreators', () => {
    beforeAll(() => {
        Site.resolveClientUrl = jest.fn((url) => url);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('getSettings', () => {
        it('should handle setSettings when fetching Settings have been done', async () => {
            // Arrange
            const response = { data: [{ id: 1, name: 'test' }] };
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setSettings.type, payload: response.data }];
            const store = mockStore({
                settings: {
                    ChatTypes: [],
                    RecipientTypes: [],
                    Systems: []
                }
            });

            // Act
            await store.dispatch(ActionCreators.getSettings());

            // Assert
            expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationType/GetSettings');
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('getnotificationTypeGridData', () => {
        it('should handle setnotificationTypeGridData when fetching notification type have been done', async () => {
            // Arrange
            const expectedParams = { notificationTypeId: 1 };
            const response = { data: { totalItems: 1, data: [{ id: 1, name: 'abnormal bet' }] } };
            const expectedActions = [{ type: ActionCreators.setNotificationTypeGridData.type, payload: response.data }];
            const store = mockStore({ notificationTypeGridData: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            await store.dispatch(ActionCreators.getNotificationTypeGridData(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/GetNotificationType', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('createNotificationType', () => {
        it('should handle setCreateNotificationTypeResult when request has been done', async () => {
            // Arrange
            const expectedParams = {
                data: {
                    notificationTypeName: 'test',
                    recipientTypeIds: '1',
                    chatTypeIds: '1',
                    systemId: '1'
                }
            };
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const response = { data: { isSuccessful: true, message: '' } };
            const expectedActions = [{ type: ActionCreators.setCreateNotificationTypeResult.type, payload: response.data }];
            const store = mockStore({ createNotificationTypeResult: null });
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            await store.dispatch(ActionCreators.createNotificationType(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/Create', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('editNotificationType', () => {
        it('should handle setEditNotificationTypeResult when request has been done', async () => {
            // Arrange
            const expectedParams = { notificationTypeId: 1 };
            const response = { data: { isSuccessful: true, message: '' } };
            const expectedActions = [{ type: ActionCreators.setEditNotificationTypeResult.type, payload: response.data }];
            const store = mockStore({ editNotificationTypeResult: null });
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            await store.dispatch(ActionCreators.editNotificationType(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/Edit', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('deleteNotificationType', () => {
        it('should return response when request has been done', async () => {
            // Arrange
            const expectedParams = { data: { notificationTypeId: 1, notificationTypeName: 'test' } };
            const store = mockStore({});
            const response = { data: { isSuccessful: true, message: '' } };
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            const actual = await store.dispatch(ActionCreators.deleteNotificationType(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/Delete', expectedParams);
            expect(actual).toEqual(response.data);
        });
    });
});