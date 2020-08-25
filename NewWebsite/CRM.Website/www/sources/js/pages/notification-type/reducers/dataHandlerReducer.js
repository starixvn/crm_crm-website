import createReducer from '@jsroot/common/createReducer';
import types from '../actions/types';

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

// Create
export const createNotificationTypePopup = createReducer({}, {
    [types.CREATE_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const createNotificationTypeResult = createReducer({}, {
    [types.CREATE_RESULT_DATA](state, action) {
        return action.data;
    }
});

// Edit
export const editNotificationTypePopup = createReducer({}, {
    [types.EDIT_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const editNotificationTypeResult = createReducer({}, {
    [types.EDIT_RESULT_DATA](state, action) {
        return action.data;
    }
});

// Delete
export const deleteNotificationTypePopup = createReducer({}, {
    [types.DELETE_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const deleteNotificationTypeResult = createReducer({}, {
    [types.DELETE_RESULT_DATA](state, action) {
        return action.data;
    }
});

export const settings = createReducer({}, {
    [types.SETTINGS_DATA](state, action) {
        return action.data;
    }
});

export const invalidParams = createReducer({}, {
    [types.INVALID_PARAMS_DATA](state, action) {
        return action.data;
    }
});

const reducers = {
    gridParams,
    gridData,
    createNotificationTypePopup,
    createNotificationTypeResult,
    editNotificationTypePopup,
    editNotificationTypeResult,
    deleteNotificationTypePopup,
    deleteNotificationTypeResult,
    settings,
    invalidParams
};

export default reducers;