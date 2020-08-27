import {
    gridParams,
    gridData,
    contentPopup,
    viewDetailPopup,
    resendOptions,
    invalidParams,
    settings,
    viewDetail,
    messageResult,
    validateResendResult,
    confirmPopup,
    headlineOptions,
    recipientOptions
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
    
    it('should handle contentPopup', () => {
        const action = {
            type: types.CONTENT_POPUP_DATA,
            data: {}
        };

        const result = contentPopup({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle viewDetailPopup', () => {
        const action = {
            type: types.VIEW_DETAIL_POPUP_DATA,
            data: {}
        };

        const result = viewDetailPopup({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle resendOptions', () => {
        const action = {
            type: types.RESEND_OPTIONS_DATA,
            data: {}
        };

        const result = resendOptions({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle invalidParams', () => {
        const action = {
            type: types.INVALID_PARAMS_DATA,
            data: {}
        };

        const result = invalidParams({}, action);
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
    
    it('should handle viewDetail', () => {
        const action = {
            type: types.VIEW_DETAIL_DATA,
            data: {}
        };

        const result = viewDetail({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle messageResult', () => {
        const action = {
            type: types.MESSAGE_RESULT_DATA,
            data: {}
        };

        const result = messageResult({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle validateResendResult', () => {
        const action = {
            type: types.VALIDATE_RESEND_DATA,
            data: {}
        };

        const result = validateResendResult({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle confirmPopup', () => {
        const action = {
            type: types.CONFIRM_POPUP_DATA,
            data: {}
        };

        const result = confirmPopup({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle headlineOptions', () => {
        const action = {
            type: types.HEADLINE_OPTIONS_DATA,
            data: {}
        };

        const result = headlineOptions({}, action);
        expect(result).toEqual(action.data);
    });
    
    it('should handle recipientOptions', () => {
        const action = {
            type: types.RECIPIENT_OPTIONS_DATA,
            data: {}
        };

        const result = recipientOptions({}, action);
        expect(result).toEqual(action.data);
    });
});