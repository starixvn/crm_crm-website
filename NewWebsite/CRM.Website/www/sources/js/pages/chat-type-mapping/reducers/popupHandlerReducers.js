import createReducer from '@jsroot/common/createReducer';
import types from '../actions/types';

export const personalMappingData = createReducer({}, {
	[types.PERSONAL_MAPPING_POPUP_DATA](state, action) {
		return action.data;
	}
});

export const personalMappingResult = createReducer({}, {
	[types.PERSONAL_MAPPING_UPDATE_RESULT](state, action) {
		return action.data;
	}
});

export const groupMappingData = createReducer({}, {
	[types.GROUP_MAPPING_POPUP_DATA](state, action) {
		return action.data;
	}
});

export const groupMappingResult = createReducer({}, {
	[types.GROUP_MAPPING_UPDATE_RESULT](state, action) {
		return action.data;
	}
});

const reducers = {
	personalMappingData,
	personalMappingResult,
	groupMappingData,
	groupMappingResult
};

export default reducers;