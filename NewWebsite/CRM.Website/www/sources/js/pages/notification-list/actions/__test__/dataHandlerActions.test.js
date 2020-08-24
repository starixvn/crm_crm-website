import * as actions from '../dataHandlerActions';
import types from '../types';
// import axios from 'axios';
// import configureMockStore from 'redux-mock-store';
// import thunk from 'redux-thunk';
// import Site from '@jsroot/common/site';

// const middlewares = [thunk];
// const mockStore = configureMockStore(middlewares);

// jest.mock('axios');

describe('notificationListActionsDataHandlerActions', () => {
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
    
    // it('should handle setGridData when fetching grid data has been done', async () => {
    //     const data = [{ id: 1, name: 'void abnormal bet' }];
    //     const params = { messageId: null, notificationIdLower: null, fromDate: '08/21/2020', toDate: '08/21/2020'};
    //     const done = () => console.log('merlin');
    //     Site.resolveClientUrl = jest.fn((url) => url);
    //     axios.post.mockImplementationOnce(() => Promise.resolve(data));
    //     const expectedActions = [
    //         { type: types.GRID_DATA }
    //     ];

    //     const store = mockStore({ gridData: [] });
    //     await store.dispatch(actions.getGridData(params, done)).then(() => {
    //         expect(axios.post).toHaveBeenCalledWith('Common/GetNotificationTypes');
    //         expect(store.getActions()).toEqual(expectedActions);
    //         expect(done).toHaveBeenCalledTimes(1);
    //     });
    // });

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

    it('should create an action to set error resolved content data', () => {
        const data = 'Test set error resolved content data';
        const expectedAction = {
            type: types.ERROR_RESOLVED_CONTENT_DATA,
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

    it('should create an action to set validate resend data', () => {
        const data = 'Test set validate resend data';
        const expectedAction = {
            type: types.VALIDATE_RESEND_DATA,
            data
        };

        expect(actions.setValidateResend(data)).toEqual(expectedAction);
    })
});