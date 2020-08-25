import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Grid
export const setGridParams = value => {
    return {
        type: types.GRID_PARAMS,
        data: value
    };
};

export const setGridData = data => {
    return {
        type: types.GRID_DATA,
        data: data
    };
};


//create
export const setCreateNotificationTypePopup = data => {
    return {
        type: types.CREATE_POPUP_DATA,
        data: data
    };
};

export const setCreateNotificationTypeResult = data => {
    return {
        type: types.CREATE_RESULT_DATA,
        data: data
    };
};

// edit
export const setEditNotificationTypePopup = data => {
    return {
        type: types.EDIT_POPUP_DATA,
        data: data
    };
};

export const setEditNotificationTypeResult = data => {
    return {
        type: types.EDIT_RESULT_DATA,
        data: data
    };
};

// Delete
export const setDeleteNotificationTypePopup = data => {
    return {
        type: types.DELETE_POPUP_DATA,
        data: data
    };
};

export const setDeleteNotificationTypeResult = data => {
    return {
        type: types.DELETE_RESULT_DATA,
        data: data
    };
};


// Setting
export const setSettings = data => {
    return {
        type: types.SETTINGS_DATA,
        data
    };
};

export const setInvalidParams = data => {
    return {
        type: types.INVALID_PARAMS_DATA,
        data
    };
};

export const getSettings = () => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/GetSettings');
    return dispatch => {
        return axios.get(url).then(response => {
            dispatch(setSettings(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

export const deleteNotificationType = notificationTypeId => {
    return async dispatch => axios({
        url: Site.resolveClientUrl(`NotificationType/Delete/${notificationTypeId}`),
        method: 'post',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => response.data).catch(Site.handleAxiosError);
};

export const editNotificationType = (data) => {
    return async dispatch => axios({
        url: Site.resolveClientUrl('NotificationType/Edit'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setEditNotificationTypeResult(response.data));
    }).catch(Site.handleAxiosError);
};

export const getGridData = params => {
    Site.blockUI();
    return dispatch => {
        return axios({
            method: 'post',
            url: Site.resolveClientUrl('NotificationType/GetGridData'),
            data: params,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setGridData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

export const createNotificationType = data => {
    return async dispatch => axios({
        url: Site.resolveClientUrl('NotificationType/Create'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setCreateNotificationTypeResult(response.data));
    }).catch(Site.handleAxiosError);
};

const actions = {
    setGridParams,
    getGridData,
    setGridData,
    setCreateNotificationTypePopup,
    setCreateNotificationTypeResult,
    createNotificationType,
    setEditNotificationTypePopup,
    setEditNotificationTypeResult,
    editNotificationType,
    setDeleteNotificationTypePopup,
    setDeleteNotificationTypeResult,
    deleteNotificationType,
    getSettings,
    setInvalidParams
};

export default actions;