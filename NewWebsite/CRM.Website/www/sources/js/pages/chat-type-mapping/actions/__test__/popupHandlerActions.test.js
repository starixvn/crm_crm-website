import * as actions from '../popupHandlerActions';
import types from '../types';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('popupHandlerActions', () => {
    beforeAll(() => {
        Site.resolveClientUrl = jest.fn(url => url);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('Insert personal mapping', () => {
        it('should create an action to set personal mapping result', () => {
            const data = { isSuccessful: true, message: '' };
            const expectedAction = {
                type: types.PERSONAL_MAPPING_INSERT_RESULT,
                data
            };

            expect(actions.setInsertPersonalMappingResult(data)).toEqual(expectedAction);
        });

        it('should disptach set insert personal mapping popup action when open insert personal mapping popup', () => {
            const store = mockStore({ insertPersonalMappingData: null });
            const expectedActions = [{ type: types.PERSONAL_MAPPING_INSERT_POPUP_DATA, data: { isShow: true } }];
            store.dispatch(actions.openInsertPersonalMappingPopup());
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should disptach expected actions when close insert personal mapping popup', () => {
            const store = mockStore({ insertPersonalMappingData: null, insertPersonalMappingResult: null });
            const expectedActions = [
                { type: types.PERSONAL_MAPPING_INSERT_POPUP_DATA, data: { isShow: false } },
                { type: types.PERSONAL_MAPPING_INSERT_RESULT, data: null },
                { type: types.RECIPIENT_DATA, data: [] },
                { type: types.GROUP_HEADLINE_DATA, data: [] }
            ];
            store.dispatch(actions.closeInsertPersonalMappingPopup());
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should handle setInsertPersonalMappingResult when post Api insert personal mapping', async () => {
            const expectedParams = { chatTypeId: '', letstalkName: '', groupHeadline: '', recipient: '', letstalkId: '' };
            const response = { data: { isSuccessful: true, message: 'Success!' } };
            const expectedActions = [{ type: types.PERSONAL_MAPPING_INSERT_RESULT, data: response.data }];
            const store = mockStore({ updatePersonalMappingData: null, updatePersonalMappingResult: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            await store.dispatch(actions.insertPersonalMapping(expectedParams)).then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('ChatTypeMapping/InsertPersonalMapping', expectedParams);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('Update personal mapping', () => {
        it('should create an action to set personal mapping update result', () => {
            const data = { isSuccessful: true, message: '' };
            const expectedAction = {
                type: types.PERSONAL_MAPPING_UPDATE_RESULT,
                data
            };

            expect(actions.setUpdatePersonalMappingResult(data)).toEqual(expectedAction);
        });

        it('should disptach set update personal mapping popup action when open update personal mapping popup', () => {
            const data = 'Update personal mapping popup data';
            const store = mockStore({ updatePersonalMappingData: null });
            const expectedActions = [{ type: types.PERSONAL_MAPPING_UPDATE_POPUP_DATA, data: { isShow: true, data } }];
            store.dispatch(actions.openUpdatePersonalMappingPopup(data));
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should disptach expected actions when close update personal mapping popup', () => {
            const store = mockStore({ updatePersonalMappingData: null, updatePersonalMappingResult: null });
            const expectedActions = [
                { type: types.PERSONAL_MAPPING_UPDATE_POPUP_DATA, data: { isShow: false, data: null } },
                { type: types.PERSONAL_MAPPING_UPDATE_RESULT, data: null },
                { type: types.RECIPIENT_DATA, data: [] }
                { type: types.GROUP_HEADLINE_DATA, data: [] }
            ];
            store.dispatch(actions.closeUpdatePersonalMappingPopup());
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should handle setUpdatePersonalMappingResult when post Api update personal mapping', async () => {
            const expectedParams = { chatTypeId: '', letstalkName: '', groupHeadline: '', recipient: '', letstalkId: '' };
            const response = { data: { isSuccessful: true, message: 'Success!' } };
            const expectedActions = [{ type: types.PERSONAL_MAPPING_UPDATE_RESULT, data: response.data }];
            const store = mockStore({ updatePersonalMappingData: null, updatePersonalMappingResult: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            await store.dispatch(actions.updatePersonalMapping(expectedParams)).then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('ChatTypeMapping/UpdatePersonalMapping', expectedParams);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('Update group mapping', () => {
        it('should create an action to set group mapping update result', () => {
            const data = { isSuccessful: true, message: '' };
            const expectedAction = {
                type: types.GROUP_MAPPING_UPDATE_RESULT,
                data
            };

            expect(actions.setGroupMappingResult(data)).toEqual(expectedAction);
        });

        it('should disptach set update group mapping popup action when open update group mapping popup', () => {
            const data = 'Update group mapping popup data';
            const store = mockStore({ updateGroupMappingData: null });
            const expectedActions = [{ type: types.GROUP_MAPPING_POPUP_DATA, data: { isShow: true, data } }];
            store.dispatch(actions.openUpdateGroupMappingPopup(data));
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should disptach expected actions when close update group mapping popup', () => {
            const store = mockStore({ updateGroupMappingData: null, updateGroupMappingResult: null });
            const expectedActions = [
                { type: types.GROUP_MAPPING_POPUP_DATA, data: { isShow: false, data: null } },
                { type: types.GROUP_MAPPING_UPDATE_RESULT, data: null },
                { type: types.GROUP_MAPPING_LETSTALK_VERIFICATION_DATA, data: [] }
            ];
            store.dispatch(actions.closeUpdateGroupMappingPopup());
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should handle setGroupMappingResult when post Api update group mapping', async () => {
            const expectedParams = { chatTypeId: '', letstalkName: '', groupHeadline: '', recipient: '', letstalkId: '' };
            const response = { data: { isSuccessful: true, message: 'Success!' } };
            const expectedActions = [{ type: types.GROUP_MAPPING_UPDATE_RESULT, data: response.data }];
            const store = mockStore({ updateGroupMappingData: null, updateGroupMappingResult: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            await store.dispatch(actions.updateGroupMapping(expectedParams)).then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('ChatTypeMapping/UpdateGroupMapping', expectedParams);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('letstalk verify', () => {
        it('should handle setLetstalkVerifationData with empty array when invalid params', () => {
            const params = { botId: 0, groupConversationId: '', recipients: [] };
            const expectedActions = [{ type: types.GROUP_MAPPING_LETSTALK_VERIFICATION_DATA, data: [] }];
            const store = mockStore({ letstalkVerification: null });

            store.dispatch(actions.verifyLetstalkInGroup(params));
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should handle setLetstalkVerifationData with call Api when valid params', async () => {
            const params = { botId: 10000, groupConversationId: 'conversationGroupId', recipients: [{ letstalkId: [1, 2, 3] }] };
            const response = { data: [{ letstalkId: 'letstalk1', isFriend: true }] };
            const expectedActions = [{ type: types.GROUP_MAPPING_LETSTALK_VERIFICATION_DATA, data: response.data }];
            const store = mockStore({ letstalkVerification: null });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            await store.dispatch(actions.verifyLetstalkInGroup(params)).then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('ChatTypeMapping/VerifyLetstalkInGroup', params);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });

        it('should handle setLetstalkDisplayName with empty string when invalid botId and LetstalkId', () => {
            const params = { botId: 0, letstalkId: '' };
            const expectedActions = [{ type: types.LETSTALK_DISPLAY_NAME, data: '' }];
            const store = mockStore({ letstalkDisplayName: '' });

            store.dispatch(actions.getLetstalkDisplayName(params.botId, params.letstalkId));
            expect(store.getActions()).toEqual(expectedActions);
        });

        it('should handle setLetstalkDisplayName with call Api when valid botId and LetstalkId', async () => {
            const params = { botId: 10000, letstalkId: 'letstalkId' };
            const response = { data: 'Letstalk name' };
            const expectedActions = [{ type: types.LETSTALK_DISPLAY_NAME, data: response.data }];
            const store = mockStore({ letstalkDisplayName: '' });
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.getDataAsync = jest.fn(() => promise);

            await store.dispatch(actions.getLetstalkDisplayName(params.botId, params.letstalkId)).then(() => {
                expect(DataService.getDataAsync).toHaveBeenCalledWith('ChatTypeMapping/GetLetstalkDisplayName', params);
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });
});