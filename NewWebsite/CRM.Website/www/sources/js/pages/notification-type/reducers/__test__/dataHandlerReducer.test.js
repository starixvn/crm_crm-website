import {
    gridParams,
    gridData,
    createNotificationTypePopup,
    createNotificationTypeResult,
    editNotificationTypePopup,
    editNotificationTypeResult,
    deleteNotificationTypePopup,
    settings,
} from '../dataHandlerReducer';
import types from '../../actions/types';

describe('dataHandlerReducer.test', () => {
    it('should handle gridParams', () => {
        const action = {
            type: types.GRID_PARAMS,
            data: {}
        };

        const result = gridParams({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle gridData', () => {
        const action = {
            type: types.GRID_DATA,
            data: {}
        };

        const result = gridData({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle createNotificationTypePopup', () => {
        const action = {
            type: types.CREATE_POPUP_DATA,
            data: {}
        };

        const result = createNotificationTypePopup({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle createNotificationTypeResult', () => {
        const action = {
            type: types.CREATE_RESULT_DATA,
            data: {}
        };

        const result = createNotificationTypeResult({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle editNotificationTypePopup', () => {
        const action = {
            type: types.EDIT_POPUP_DATA,
            data: {}
        };

        const result = editNotificationTypePopup({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle editNotificationTypeResult', () => {
        const action = {
            type: types.EDIT_RESULT_DATA,
            data: {}
        };

        const result = editNotificationTypeResult({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle deleteNotificationTypePopup', () => {
        const action = {
            type: types.DELETE_POPUP_DATA,
            data: {}
        };

        const result = deleteNotificationTypePopup({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle settings', () => {
        const action = {
            type: types.SETTINGS_DATA,
            data: {}
        };

        const result = settings({}, action);
        expect(result).toEqual(action.data);
    });
});