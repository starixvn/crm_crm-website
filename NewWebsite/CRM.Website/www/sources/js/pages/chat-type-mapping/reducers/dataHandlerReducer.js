import createReducer from '@jsroot/common/createReducer';
import types from '../actions/types';

// Setting
export const settings = createReducer({}, {
	[types.SETTING_DATA](state, action) {
		return action.data;
	}
});

// Grid
export const gridParams = createReducer({}, {
	[types.GRID_PARAMS](state, action) {
		return action.data;
	}
});

export const gridData = createReducer({}, {
	[types.GRID_DATA](state, action) {
		return action.data;
	}
});

//Update status active
export const updateActiveStatusPopup = createReducer({}, {
	[types.ACTIVE_POPUP_DATA](state, action) {
		return action.data;
	}
});

export const updateActiveStatusResult = createReducer({}, {
	[types.ACTIVE_RESULT_DATA](state, action) {
		return action.data;
	}
});

// Recipients

export const recipients = createReducer({}, {
	[types.RECIPIENT_DATA](state, action) {
		return action.data;
	}
});

const reducers = {
	settings,
	gridParams,
	gridData,
	updateActiveStatusPopup,
	updateActiveStatusResult,
	recipients
};

export default reducers;