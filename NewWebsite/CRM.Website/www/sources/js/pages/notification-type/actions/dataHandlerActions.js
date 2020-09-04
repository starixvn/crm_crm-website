import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';

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

// Setting
export const setSettings = data => {
    return {
        type: types.SETTINGS_DATA,
        data
    };
};

export const getSettings = () => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/GetSettings');
    await DataService.getDataAsync(url).then(response => {
        dispatch(setSettings(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

 export const deleteNotificationType = notificationTypeId => async dispatch => {
    const url = Site.resolveClientUrl(`NotificationType/Delete/${notificationTypeId}`);
    return await DataService.postDataAsync(url).then(response => response.data);
};

export const editNotificationType = data => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/Edit');
    await DataService.postDataAsync(url, data).then(response => {
        dispatch(setEditNotificationTypeResult(response.data));
    }).then(() => Site.unblockUI());
};

export const getGridData = params => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/GetNotificationType');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setGridData(response.data));
    }).then(() => Site.unblockUI());
};

export const createNotificationType = data => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/Create');
    await DataService.postDataAsync(url, data).then(response => {
        dispatch(setCreateNotificationTypeResult(response.data));
    }).then(() => Site.unblockUI());
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
    deleteNotificationType,
    getSettings,
};

export default actions;