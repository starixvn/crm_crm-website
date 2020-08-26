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
        it('should handle setNotificationTypes when fetching notification types has been done', async () => {
            const response = { data: [{ id: 1, name: 'void abnormal bet' }] };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setNotificationTypes.type, payload: response.data }];
            const store = mockStore({ notificationTypes: [] });

            await store.dispatch(ActionCreators.getNotificationTypes()).then(() => {
                expect(DataService.getDataAsync).toHaveBeenCalledWith('Common/GetNotificationTypes');
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('getLanguages', () => {
        it('should handle setLanguages when fetching languages has been done', async () => {
            const response = { data: [{ id: 1, name: 'en' }] };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setLanguages.type, payload: response.data }];
            const store = mockStore({ languages: [] });

            await store.dispatch(ActionCreators.getLanguages()).then(() => {
                expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationTemplate/GetLanguages');
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('getGridData', () => {
        it('should handle setGridData when fetching notification template has been done', async () => {
            const expectedParams = { notificationTypeId: 1 };
            const response = { data: { totalItems: 1, data: [{ id: 1, name: 'abnormal bet' }] } };
            const expectedActions = [{ type: ActionCreators.setGridData.type, payload: response.data }];
            const store = mockStore({ gridData: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            await store.dispatch(ActionCreators.getGridData(expectedParams)).then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationTemplate/GetGridData', expectedParams);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('getTemplateById', () => {
        it('should handle setSelectedTemplate when fetching template has been done', async () => {
            const response = { data: { id: 1, name: 'abnormal bet' } };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const expectedActions = [{ type: ActionCreators.setSelectedTemplate.type, payload: response.data }];
            const store = mockStore({ selectedTemplate: null });

            await store.dispatch(ActionCreators.getTemplateById(1)).then(() => {
                expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationTemplate/GetById/1');
                expect(store.getActions()).toEqual(expectedActions);
            });
        });

        it('should call callback function', async () => {
            const response = { data: { id: 1, name: 'abnormal bet' } };
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);
            const callbackFunc = jest.fn();
            const store = mockStore({ selectedTemplate: null });

            await store.dispatch(ActionCreators.getTemplateById(1, callbackFunc)).then(() => {
                expect(callbackFunc).toHaveBeenCalled();
            });
        });
    });

    describe('createTemplate', () => {
        it('should handle setCreateTemplateResult when create successfully', async () => {
            const expectedParams = { notificationTypeId: 1 };
            const response = { data: { isSuccessful: true, message: '' } };
            const expectedActions = [{ type: ActionCreators.setCreateTemplateResult.type, payload: response.data }];
            const store = mockStore({ createTemplateResult: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            await store.dispatch(ActionCreators.createTemplate(expectedParams)).then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationTemplate/Create', expectedParams);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });
});