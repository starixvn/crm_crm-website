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

    describe('getNotificationTypes', () => {
        it('should handle setNotificationTypes when fetching notification types have been done', async () => {
            // Arrange
            const response = { data: [{ id: 1, name: 'void abnormal bet' }] };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setNotificationTypes.type, payload: response.data }];
            const store = mockStore({ notificationTypes: [] });

            // Act
            await store.dispatch(ActionCreators.getNotificationTypes());

            // Assert
            expect(DataService.getDataAsync).toHaveBeenCalledWith('Common/GetNotificationTypes');
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('getLanguages', () => {
        it('should handle setLanguages when fetching languages have been done', async () => {
            // Arrange
            const response = { data: [{ id: 1, name: 'en' }] };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setLanguages.type, payload: response.data }];
            const store = mockStore({ languages: [] });

            // Act
            await store.dispatch(ActionCreators.getLanguages());

            // Assert
            expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationTemplate/GetLanguages');
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('getGridData', () => {
        it('should handle setGridData when fetching templates have been done', async () => {
            // Arrange
            const expectedParams = { notificationTypeId: 1 };
            const response = { data: { totalItems: 1, data: [{ id: 1, name: 'abnormal bet' }] } };
            const expectedActions = [{ type: ActionCreators.setGridData.type, payload: response.data }];
            const store = mockStore({ gridData: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            await store.dispatch(ActionCreators.getGridData(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationTemplate/GetNotificationTemplate', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('getTemplateById', () => {
        it('should handle setSelectedTemplate when fetching template has been done', async () => {
            // Arrange
            const response = { data: { id: 1, name: 'abnormal bet' } };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setSelectedTemplate.type, payload: response.data }];
            const store = mockStore({ selectedTemplate: null });

            // Act
            await store.dispatch(ActionCreators.getTemplateById(1));

            // Assert
            expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationTemplate/GetById/1');
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should call callback function', async () => {
            // Arrange
            const response = { data: { id: 1, name: 'abnormal bet' } };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const callbackFunc = jest.fn();
            const store = mockStore({ selectedTemplate: null });

            // Act
            await store.dispatch(ActionCreators.getTemplateById(1, callbackFunc));

            // Assert
            expect(callbackFunc).toHaveBeenCalled();
        });
    });

    describe('createTemplate', () => {
        it('should handle setCreateTemplateResult when request has been done', async () => {
            // Arrange
            const expectedParams = { notificationTypeId: 1 };
            const response = { data: { isSuccessful: true, message: '' } };
            const expectedActions = [{ type: ActionCreators.setCreateTemplateResult.type, payload: response.data }];
            const store = mockStore({ createTemplateResult: null });
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            await store.dispatch(ActionCreators.createTemplate(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationTemplate/Create', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('editTemplate', () => {
        it('should handle setEditTemplateResult when request has been done', async () => {
            // Arrange
            const expectedParams = { notificationTemplateId: 1, notificationTypeId: 1 };
            const response = { data: { isSuccessful: true, message: '' } };
            const expectedActions = [{ type: ActionCreators.setEditTemplateResult.type, payload: response.data }];
            const store = mockStore({ editTemplateResult: null });
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            await store.dispatch(ActionCreators.editTemplate(expectedParams));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationTemplate/Edit', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    describe('deleteTemplate', () => {
        it('should return response when request has been done', async () => {
            // Arrange
            const store = mockStore({});
            const response = { data: { isSuccessful: true, message: '' } };
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Act
            const actual = await store.dispatch(ActionCreators.deleteTemplate(1));

            // Assert
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationTemplate/Delete/1');
            expect(actual).toEqual(response);
        });
    });
});