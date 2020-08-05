import createReducer from '@jsroot/common/createReducer';
import types from '../actions/types';

// Setting
export const notificationTypes = createReducer({}, {
    [types.NOTIFICATIONTYPE_DATA](state, action) {
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
export const createTypePopup = createReducer({}, {
    [types.CREATE_POPUP_DATA](state, action) {
        return action.data;
    }
});

const reducers = {
    notificationTypes,
    gridParams,
    gridData,
    createTypePopup
};

export default reducers;