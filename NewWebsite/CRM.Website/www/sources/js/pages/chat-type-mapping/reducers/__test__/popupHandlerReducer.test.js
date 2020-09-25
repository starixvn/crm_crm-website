import {
    insertPersonalMappingData,
    insertPersonalMappingResult,
    updatePersonalMappingData,
    updatePersonalMappingResult,
    updateGroupMappingData,
    updateGroupMappingResult,
    letstalkVerification,
    letstalkName
} from '../popupHandlerReducers';
import types from '../../actions/types';

describe('dataHandlerReducer.test', () => {
    it('should handle insertPersonalMappingData', () => {
        const action = {
            type: types.PERSONAL_MAPPING_INSERT_POPUP_DATA,
            data: {}
        };

        const result = insertPersonalMappingData({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle insertPersonalMappingResult', () => {
        const action = {
            type: types.PERSONAL_MAPPING_INSERT_RESULT,
            data: {}
        };

        const result = insertPersonalMappingResult({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle updatePersonalMappingData', () => {
        const action = {
            type: types.PERSONAL_MAPPING_UPDATE_POPUP_DATA,
            data: {}
        };

        const result = updatePersonalMappingData({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle updatePersonalMappingResult', () => {
        const action = {
            type: types.PERSONAL_MAPPING_UPDATE_RESULT,
            data: {}
        };

        const result = updatePersonalMappingResult({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle updateGroupMappingData', () => {
        const action = {
            type: types.GROUP_MAPPING_POPUP_DATA,
            data: {}
        };

        const result = updateGroupMappingData({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle updateGroupMappingResult', () => {
        const action = {
            type: types.GROUP_MAPPING_UPDATE_RESULT,
            data: {}
        };

        const result = updateGroupMappingResult({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle letstalkVerification', () => {
        const action = {
            type: types.GROUP_MAPPING_LETSTALK_VERIFICATION_DATA,
            data: {}
        };

        const result = letstalkVerification({}, action);
        expect(result).toEqual(action.data);
    });

    it('should handle letstalkName', () => {
        const action = {
            type: types.LETSTALK_DISPLAY_NAME,
            data: {}
        };

        const result = letstalkName({}, action);
        expect(result).toEqual(action.data);
    });
});