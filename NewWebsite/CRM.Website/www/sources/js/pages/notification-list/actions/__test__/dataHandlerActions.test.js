import * as actions from '../dataHandlerActions';
import types from '../types';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
import { EndTimeString } from '@jsroot/common/constants';
import { convertDateToString } from '@jsroot/common/functions';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('notificationListActionsDataHandlerActions', () => {
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
            data
        };

        expect(actions.setGridParams(data)).toEqual(expectedAction);
    });

    it('should create an action to set grid data', () => {
        const data = 'Test grid data';
        const expectedAction = {
            type: types.GRID_DATA,
            data
        };

        expect(actions.setGridData(data)).toEqual(expectedAction);
    });
    
    it('should params change when fetching grid data with params have messageId or notificationIdLower', async () => {
        const expectedParams = { messageId: 1, notificationIdLower: '', fromDate: '08/21/2020', toDate: '08/21/2020'};
        const expectedDone = jest.fn();
        const response = { data: { totalItems: 1, data: [{ id: 1, name: 'abnormal bet' }] } };
        const store = mockStore({ gridData: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getGridData(expectedParams, expectedDone)).then(() => {
            expect(expectedParams.fromDate).toBeNull();
            expect(expectedParams.toDate).toBeNull();
        });
    });
    
    it('should params change when fetching grid data with params not have messageId and notificationIdLower', async () => {
        const now = new Date();
        const nowString = convertDateToString(now);
        const expectedParams = { messageId: '', notificationIdLower: '', fromDate: now, toDate: now};
        const expectedDone = jest.fn();
        const response = { data: { totalItems: 1, data: [{ id: 1, name: 'abnormal bet' }] } };
        const store = mockStore({ gridData: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getGridData(expectedParams, expectedDone)).then(() => {
            expect(expectedParams.fromDate).toEqual(nowString);
            expect(expectedParams.toDate).toEqual(`${nowString} ${EndTimeString}`);
        });
    });
    
    it('should handle setGridData when fetching grid data has been done', async () => {
        const expectedParams = { messageId: null, notificationIdLower: null, fromDate: '08/21/2020', toDate: '08/21/2020'};
        const expectedDone = jest.fn();
        const response = { data: { totalItems: 1, data: [{ id: 1, name: 'abnormal bet' }] } };
        const expectedActions = [{ type: types.GRID_DATA, data: response.data }];
        const store = mockStore({ gridData: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getGridData(expectedParams, expectedDone)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationList/GetGridData', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
            expect(expectedDone).toHaveBeenCalledTimes(1);
        });
    });

    it('should create an action to set content popup data', () => {
        const data = 'Test set content popup data';
        const expectedAction = {
            type: types.CONTENT_POPUP_DATA,
            data
        };

        expect(actions.setContentPopup(data)).toEqual(expectedAction);
    });

    it('should create an action to set error or resolved content popup data', () => {
        const data = 'Test set error or resolved content popup data';
        const expectedAction = {
            type: types.VIEW_DETAIL_POPUP_DATA,
            data
        };

        expect(actions.setViewDetailPopup(data)).toEqual(expectedAction);
    });

    it('should create an action to set resend options data', () => {
        const data = 'Test set resend options data';
        const expectedAction = {
            type: types.RESEND_OPTIONS_DATA,
            data
        };

        expect(actions.setResendOptions(data)).toEqual(expectedAction);
    });

    it('should create an action to set invalid params data', () => {
        const data = 'Test set invalid params data';
        const expectedAction = {
            type: types.INVALID_PARAMS_DATA,
            data
        };

        expect(actions.setInvalidParams(data)).toEqual(expectedAction);
    });

    it('should create an action to set settings data', () => {
        const data = 'Test set settings data';
        const expectedAction = {
            type: types.SETTINGS_DATA,
            data
        };

        expect(actions.setSettings(data)).toEqual(expectedAction);
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
            expect(DataService.getDataAsync).toHaveBeenCalledWith('NotificationList/GetSettings');
            expect(store.getActions()).toEqual(expectedActions);
        });
    });
    
    it('should handle setViewDetail when edit notification has been done', async () => {
        const expectedParams = { remark: 'remark'};
        const response = { data: {} };
        const expectedActions = [{ type: types.VIEW_DETAIL_DATA, data: response.data }];
        const store = mockStore({ viewDetail: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.editNotificationList(expectedParams)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationList/EditNotificationList', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should create an action to set error resolved content data', () => {
        const data = 'Test set error resolved content data';
        const expectedAction = {
            type: types.VIEW_DETAIL_DATA,
            data
        };

        expect(actions.setViewDetail(data)).toEqual(expectedAction);
    });

    it('should create an action to set message result data', () => {
        const data = 'Test set message result data';
        const expectedAction = {
            type: types.MESSAGE_RESULT_DATA,
            data
        };

        expect(actions.setMessageResult(data)).toEqual(expectedAction);
    });
    
    it('should handle setValidateResend when validate resend has been done', async () => {
        const expectedParams = { notificationIds: '1,2' };
        const response = { data: {} };
        const expectedActions = [{ type: types.VALIDATE_RESEND_DATA, data: { show: true, result: response.data } }];
        const store = mockStore({ validateResendResult: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.validateResend(expectedParams)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationList/ValidateNotificationList', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should create an action to set validate resend data', () => {
        const data = 'Test set validate resend data';
        const expectedAction = {
            type: types.VALIDATE_RESEND_DATA,
            data
        };

        expect(actions.setValidateResend(data)).toEqual(expectedAction);
    });
    
    it('should handle setMessageResult when resend notification list has been done', async () => {
        const expectedParams = [1, 2];
        const expectedDone = jest.fn();
        const response = { data: { isSuccessful: true, message: 'message' } };
        const expectedActions = [{ type: types.MESSAGE_RESULT_DATA, data: { show: true, ...response.data} }];
        const store = mockStore({ messageResult: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.resendNotifications(expectedParams, expectedDone)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('NotificationList/ResendNotificationList', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
            expect(expectedDone).toHaveBeenCalledTimes(1);
        });
    });

    it('should create an action to set confirm popup data', () => {
        const data = 'Test set confirm popup data';
        const expectedAction = {
            type: types.CONFIRM_POPUP_DATA,
            data
        };

        expect(actions.setConfirmPopup(data)).toEqual(expectedAction);
    });
    
    it('should handle setHeadlineOptions when fetch headline options has been done', async () => {
        const searchText = 'searchText';
        const expectedParams = { username: searchText, limit: 10 };
        const response = { data: [ { name: 'name' } ] };
        const options = response.data.map(item => {
            return { value: item.name, label: item.name }
        });

        const expectedActions = [{ type: types.HEADLINE_OPTIONS_DATA, data: options }];
        const store = mockStore({ headlineOptions: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getHeadlineOptions(searchText)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('Common/GetHeadlines', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });
    
    it('should handle setRecipientOptions when fetch recipient options has been done', async () => {
        const searchText = 'searchText';
        const expectedParams = { recipientName: searchText, limit: 10 };
        const response = { data: [ { name: 'name' } ] };
        const options = response.data.map(item => {
            return { value: item.name, label: item.name }
        });

        const expectedActions = [{ type: types.RECIPIENT_OPTIONS_DATA, data: options }];
        const store = mockStore({ recipientOptions: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getRecipientOptions(searchText)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('Common/GetRecipients', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should create an action to set headline options data', () => {
        const data = 'Test set headline options data';
        const expectedAction = {
            type: types.HEADLINE_OPTIONS_DATA,
            data
        };

        expect(actions.setHeadlineOptions(data)).toEqual(expectedAction);
    });

    it('should create an action to set recipient options data', () => {
        const data = 'Test set recipient options data';
        const expectedAction = {
            type: types.RECIPIENT_OPTIONS_DATA,
            data
        };

        expect(actions.setRecipientOptions(data)).toEqual(expectedAction);
    });
});