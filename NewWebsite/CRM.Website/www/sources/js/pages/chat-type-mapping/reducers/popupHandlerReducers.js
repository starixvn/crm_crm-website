import createReducer from '@jsroot/common/createReducer';
import types from '../actions/types';

export const insertPersonalMappingData = createReducer({}, {
	[types.PERSONAL_MAPPING_INSERT_POPUP_DATA](state, action) {
		return action.data;
	}
});

export const insertPersonalMappingResult = createReducer({}, {
	[types.PERSONAL_MAPPING_INSERT_RESULT](state, action) {
		return action.data;
	}
});

export const updatePersonalMappingData = createReducer({}, {
	[types.PERSONAL_MAPPING_UPDATE_POPUP_DATA](state, action) {
		return action.data;
	}
});

export const updatePersonalMappingResult = createReducer({}, {
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
	insertPersonalMappingData,
	insertPersonalMappingResult,
	updatePersonalMappingData,
	updatePersonalMappingResult,
	groupMappingData,
	groupMappingResult
};

export default reducers;