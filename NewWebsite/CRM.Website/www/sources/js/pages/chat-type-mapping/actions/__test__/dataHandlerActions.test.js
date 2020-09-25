import * as actions from '../dataHandlerActions';
import types from '../types';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('dataHandlerActions', () => {
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

    it('should handle setGridData when fetching grid data has been done', async () => {
        const expectedParams = { chatType: '', letstalkName: '', groupHeadline: '', recipient: '', letstalkId: '', mappingStatuses: '' };
        const expectedDone = jest.fn();
        const response = { data: { totalItems: 1, data: [{ id: 1, chatTypeId: 1 }] } };
        const expectedActions = [{ type: types.GRID_DATA, data: response.data }];
        const store = mockStore({ gridData: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getGridData(expectedParams, expectedDone)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('ChatTypeMapping/GetChatTypeMapping', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should handle setSettings when fetching settings has been done', async () => {
        const response = { data: {} };
        const expectedActions = [{ type: types.SETTING_DATA, data: response.data }];
        const store = mockStore({ settings: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.getDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getSettings()).then(() => {
            expect(DataService.getDataAsync).toHaveBeenCalledWith('ChatTypeMapping/GetSettings');
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should create an action to set settings data', () => {
        const data = 'Test set settings data';
        const expectedAction = {
            type: types.SETTING_DATA,
            data
        };

        expect(actions.setSettings(data)).toEqual(expectedAction);
    });

    it('should create an action to set UpdateActivePopup', () => {
        const data = 'Test set updateActivePopup data';
        const expectedAction = {
            type: types.ACTIVE_POPUP_DATA,
            data
        };

        expect(actions.setUpateActivePopup(data)).toEqual(expectedAction);
    });

    it('should handle setUpateActivePopup when update Active Status has been done', async () => {
        const response = { data: { isSuccessful: false } };
        const expectedParams = { conversationGroupId: 1, isActive: false };
        const expectedActions = [{ type: types.ACTIVE_RESULT_DATA, data: response.data }];
        const store = mockStore({ updateActiveStatusResult: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();
        const promise = Promise.resolve(response);
        DataService.postDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.updateActiveStatus(expectedParams)).then(() => {
            expect(DataService.postDataAsync).toHaveBeenCalledWith('ChatTypeMapping/UpdateActiveStatus', expectedParams);
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should create an action to set Recipient Data', () => {
        const data = 'Test set Recipient data';
        const expectedAction = {
            type: types.RECIPIENT_DATA,
            data
        };

        expect(actions.setRecipientData(data)).toEqual(expectedAction);
    });

    it('should handle setRecipientData when getRecipientByRecipientType and not call API when recipientTypeId equals zero', () => {
        const recipientTypeId = 0;
        const expectedActions = [{ type: types.RECIPIENT_DATA, data: [] }];
        const store = mockStore({ recipients: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();

        store.dispatch(actions.getRecipientByRecipientType(recipientTypeId)).then(() => {
            expect(DataService.getDataAsync).not.toHaveBeenCalled();
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should handle setRecipientData when getRecipientByRecipientType and call API when recipientTypeId biger than zero', async () => {
        const recipientTypeId = 100;
        const response = { data: [{ id: 1, name: 'recipient 1' }] };
        const expectedActions = [{ type: types.RECIPIENT_DATA, data: response.data }];
        const store = mockStore({ recipients: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();

        const promise = Promise.resolve(response);
        DataService.getDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getRecipientByRecipientType(recipientTypeId)).then(() => {
            expect(DataService.getDataAsync).toHaveBeenCalledWith('ChatTypeMapping/GetRecipientByType', { recipientTypeId: recipientTypeId });
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should create an action to set Group healine Data', () => {
        const data = 'Test set group headline data';
        const expectedAction = {
            type: types.GROUP_HEADLINE_DATA,
            data
        };

        expect(actions.setGroupHeadlineData(data)).toEqual(expectedAction);
    });

    it('should handle setGroupHeadlineData when getGroupHeadlineByRecipient and not call API when recipientIds is empty', () => {
        const recipientIds = '';
        const expectedActions = [{ type: types.GROUP_HEADLINE_DATA, data: [] }];
        const store = mockStore({ groupHeadline: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();

        store.dispatch(actions.getGroupHeadlineByRecipient(recipientIds)).then(() => {
            expect(DataService.getDataAsync).not.toHaveBeenCalled();
            expect(store.getActions()).toEqual(expectedActions);
        });
    });

    it('should handle setGroupHeadlineData when getGroupHeadlineByRecipient and call API when recipientIds is not empty', async () => {
        const recipientIds = '1,2,3';
        const response = { data: [{ id: 1, name: 'headline 1' }] };
        const expectedActions = [{ type: types.GROUP_HEADLINE_DATA, data: response.data }];
        const store = mockStore({ groupHeadline: null });
        Site.blockUI = jest.fn();
        Site.unblockUI = jest.fn();

        const promise = Promise.resolve(response);
        DataService.getDataAsync = jest.fn(() => promise);

        await store.dispatch(actions.getGroupHeadlineByRecipient(recipientIds)).then(() => {
            expect(DataService.getDataAsync).toHaveBeenCalledWith('ChatTypeMapping/GetGroupHeadlineByRecipient', { recipientIds: recipientIds });
            expect(store.getActions()).toEqual(expectedActions);
        });
    });
});