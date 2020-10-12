import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
import {
    setNotificationTypeGridParams,
    setNotificationTypeGridData,
    setSettings,
    setCreateNotificationTypePopup,
    setCreateNotificationTypeResult,
    setEditNotificationTypePopup,
    setEditNotificationTypeResult,
    setDeleteNotificationTypePopup
} from './notificationTypeSlice';

export const getSettings = () => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/GetSettings');
    await DataService.getDataAsync(url).then(response => {
        dispatch(setSettings(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

export const deleteNotificationType = data => async dispatch => {
    const url = Site.resolveClientUrl(`NotificationType/Delete`);
    return await DataService.postDataAsync(url, data).then(response => response.data);
};

export const editNotificationType = data => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/Edit');
    await DataService.postDataAsync(url, data).then(response => {
        dispatch(setEditNotificationTypeResult(response.data));
    }).then(() => Site.unblockUI());
};

export const getNotificationTypeGridData = params => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/GetNotificationType');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setNotificationTypeGridData(response.data));
    }).then(() => Site.unblockUI());
};

export const createNotificationType = data => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationType/Create');
    await DataService.postDataAsync(url, data).then(response => {
        dispatch(setCreateNotificationTypeResult(response.data));
    }).then(() => Site.unblockUI());
};

export const ActionCreators = {
    setNotificationTypeGridParams,
    getNotificationTypeGridData,
    setNotificationTypeGridData,
    setSettings,
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