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
export const createTypePopup = createReducer({}, {
    [types.CREATE_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const createTypeResult = createReducer({}, {
    [types.CREATE_RESULT_DATA](state, action) {
        return action.data;
    }
});

// Edit
export const editTypePopup = createReducer({}, {
    [types.EDIT_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const editTypeResult = createReducer({}, {
    [types.EDIT_RESULT_DATA](state, action) {
        return action.data;
    }
});

// Delete
export const deleteTypePopup = createReducer({}, {
    [types.DELETE_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const deleteTypeResult = createReducer({}, {
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
    createTypePopup,
    createTypeResult,
    editTypePopup,
    editTypeResult,
    deleteTypePopup,
    deleteTypeResult,
    settings,
    invalidParams
};

export default reducers;