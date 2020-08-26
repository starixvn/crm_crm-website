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

export const viewDetailPopup = createReducer({}, {
    [types.VIEW_DETAIL_POPUP_DATA](state, action) {
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

export const viewDetail = createReducer({}, {
    [types.VIEW_DETAIL_DATA](state, action) {
        return action.data;
    }
});

export const messageResult = createReducer({}, {
    [types.MESSAGE_RESULT_DATA](state, action) {
        return action.data;
    }
});

export const validateResendResult = createReducer({}, {
    [types.VALIDATE_RESEND_DATA](state, action) {
        return action.data;
    }
});

export const confirmPopup = createReducer({}, {
    [types.CONFIRM_POPUP_DATA](state, action) {
        return action.data;
    }
});

export const headlineOptions = createReducer({}, {
    [types.HEADLINE_OPTIONS_DATA](state, action) {
        return action.data;
    }
});

export const recipientOptions = createReducer({}, {
    [types.RECIPIENT_OPTIONS_DATA](state, action) {
        return action.data;
    }
});

const reducers = {
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
};

export default reducers;