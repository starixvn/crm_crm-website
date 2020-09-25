import {
    settings,
    gridParams,
    gridData,
    updateActiveStatusPopup,
    updateActiveStatusResult,
    recipients,
    groupHeadline
} from '../dataHandlerReducer';
import types from '../../actions/types';

describe('dataHandlerReducer.test', () => {
    it('should handle settings', () => {
        const action = {
            type: types.SETTING_DATA,
            data: {}
        };

        const result = settings({}, action);
        expect(result).toEqual(action.data);
    });

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

    it('should handle updateActiveStatusPopup', () => {
        const action = {
            type: types.ACTIVE_POPUP_DATA,
            data: {}
        };

        const result = updateActiveStatusPopup({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle updateActiveStatusResult', () => {
        const action = {
            type: types.ACTIVE_RESULT_DATA,
            data: {}
        };

        const result = updateActiveStatusResult({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle recipients', () => {
        const action = {
            type: types.RECIPIENT_DATA,
            data: {}
        };

        const result = recipients({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle grouHeadline', () => {
        const action = {
            type: types.GROUP_HEADLINE_DATA,
            data: {}
        };

        const result = groupHeadline({}, action);
        expect(result).toEqual(action.data);
    });
});