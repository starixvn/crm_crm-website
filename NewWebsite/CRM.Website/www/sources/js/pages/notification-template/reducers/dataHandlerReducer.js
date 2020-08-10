import createReducer from '@jsroot/common/createReducer';
import types from '../actions/types';

// Setting
export const notificationTypes = createReducer({}, {
    [types.NOTIFICATIONTYPE_DATA](state, action) {
        return action.data;
    }
});

export const languages = createReducer({}, {
    [types.LANGUAGE_DATA](state, action) {
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

// Create
export const createTemplatePopup = createReducer({}, {
    [types.CREATE_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const createTemplateResult = createReducer({}, {
    [types.CREATE_RESULT_DATA](state, action) {
        return action.data;
    }
});

// Edit
export const editTemplatePopup = createReducer({}, {
    [types.EDIT_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const editTemplateResult = createReducer({}, {
    [types.EDIT_RESULT_DATA](state, action) {
        return action.data;
    }
});

// Delete
export const deleteTemplatePopup = createReducer({}, {
    [types.DELETE_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const deleteTemplateResult = createReducer({}, {
    [types.DELETE_RESULT_DATA](state, action) {
        return action.data;
    }
});

const reducers = {
    notificationTypes,
    languages,
    gridParams,
    gridData,
    createTemplatePopup,
    createTemplateResult,
    editTemplatePopup,
    editTemplateResult,
    deleteTemplatePopup,
    deleteTemplateResult
};

export default reducers;