import * as actions from '../dataHandlerActions';
import types from '../types';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('notificationTypeActionsDataHandlerActions', () => {
    beforeAll(() => {
        Site.resolveClientUrl = jest.fn(url => url);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('should create an action to set grid params', () => {
        const data = 'Test grid params';
        const expectedAction = {
            type: types.GRID_PARAMS,
            data: data
        };

        expect(actions.setGridParams(data)).toEqual(expectedAction);
    });

    it('should create an action to set grid data', () => {
        const data = 'Test grid data';
        const expectedAction = {
            type: types.GRID_DATA,
            data: data
        };

        expect(actions.setGridData(data)).toEqual(expectedAction);
    });

    it('should create an action to set notification popup data', () => {
        const data = 'Test set popup data';
        const expectedAction = {
            type: types.CREATE_POPUP_DATA,
            data: data
        };

        expect(actions.setCreateNotificationTypePopup(data)).toEqual(expectedAction);
    });

    it('should create an action to set notification popup result data', () => {
        const data = 'Test set popup result data';
        const expectedAction = {
            type: types.CREATE_RESULT_DATA,
            data: data
        };

        expect(actions.setCreateNotificationTypeResult(data)).toEqual(expectedAction);
    });

    it('should create an action to set notification popup edit data', () => {
        const data = 'Test set popup edit data';
        const expectedAction = {
            type: types.EDIT_POPUP_DATA,
            data: data
        };

        expect(actions.setEditNotificationTypePopup(data)).toEqual(expectedAction);
    });

    it('should create an action to set notification popup edit result data', () => {
        const data = 'Test set popup edit result data';
        const expectedAction = {
            type: types.EDIT_RESULT_DATA,
            data: data
        };

        expect(actions.setEditNotificationTypeResult(data)).toEqual(expectedAction);
    });

    it('should create an action to set notification type delete data', () => {
        const data = 'Test set popup delete data';
        const expectedAction = {
            type: types.DELETE_POPUP_DATA,
            data: data
        };

        expect(actions.setDeleteNotificationTypePopup(data)).toEqual(expectedAction);
    });

    it('should create an action to set notification type delete result data', () => {
        const data = 'Test set popup delete result data';
        const expectedAction = {
            type: types.DELETE_RESULT_DATA,
            data: data
        };

        expect(actions.setDeleteNotificationTypeResult(data)).toEqual(expectedAction);
    });

    it('should create an action to set setting data', () => {
        const data = 'Test set setting data';
        const expectedAction = {
            type: types.SETTINGS_DATA,
            data
        };

        expect(actions.setSettings(data)).toEqual(expectedAction);
    });

    it('should create an action to set invalid param', () => {
        const data = 'Test set invalid param';
        const expectedAction = {
            type: types.INVALID_PARAMS_DATA,
            data
        };

        expect(actions.setInvalidParams(data)).toEqual(expectedAction);
    });

    it('should handle setSettings when fetching settings has been done', async () => {
        const response = { data: {} };
        const expectedActions = [{ type: types.SETTINGS_DATA, data: response.data }];
        const store = mockStore({ settings: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.getDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getSettings()).then(() => {
            expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationType/GetSettings');
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should return response when request has been done', async () => { 
        const store = mockStore({});
        const response = { data: { isSuccessful: true, message: '' } };
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);
        const actual = await store.dispatch(actions.deleteNotificationType(1));

        expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/Delete/1');
        expect(actual).toEqual(response.data);
    });

    it('should handle setEditNotificationTypeResult when request has been done', async () => {
        const expectedParams = { notificationTypeId: 1};
        const response = { data: { isSuccessful: true, message: '' } };
        const expectedActions = [{ type: types.EDIT_RESULT_DATA, data: response.data}];
        const store = mockStore({ editNotificationTypeResult: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.editNotificationType(expectedParams));

        expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/Edit', expectedParams);
        expect(store.getActions()).toEqual(expectedActions);
    });

    it('should handle setGridData when fetching notification type have been done', async () => {
        const expectedParams = { notificationTypeId: 1 };
        const response = { data: { totalItems: 1, data: [{ id: 1, name: 'voiding bet' }] } };
        const expectedActions = [{ type: types.GRID_DATA, data: response.data }];
        const store = mockStore({ gridData: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getGridData(expectedParams));

        expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/GetNotificationType', expectedParams);
        expect(store.getActions()).toEqual(expectedActions);
    });

    it('should handle setCreateNotificationTypeResult when request has been done', async () => {
        const expectedParams = { notificationTypeId: 1 };
        const response = { data: { isSuccessful: true, message: '' } };
        const expectedActions = [{ type: types.CREATE_RESULT_DATA, data: response.data}];
        const store = mockStore({ createNotificationTypeResult: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.createNotificationType(expectedParams));

        expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationType/Create', expectedParams);
        expect(store.getActions()).toEqual(expectedActions);
    });

});