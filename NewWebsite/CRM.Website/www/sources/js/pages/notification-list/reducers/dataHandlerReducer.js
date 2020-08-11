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

export const contentPopup = createReducer({}, {
    [types.CONTENT_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const resendOptions = createReducer({}, {
    [types.RESEND_OPTIONS_DATA](state, action) {
        return action.data;
    }
});

export const errorOrResolvedContentPopup = createReducer({}, {
    [types.ERROR_RESOLVED_CONTENT_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const invalidParams = createReducer({}, {
    [types.INVALID_PARAMS_DATA](state, action) {
        return action.data;
    }
});

export const settings = createReducer({}, {
    [types.SETTINGS_DATA](state, action) {
        return action.data;
    }
});

const reducers = {
    gridParams,
    gridData,
    contentPopup,
    errorOrResolvedContentPopup,
    resendOptions,
    invalidParams,
    settings
};

export default reducers;