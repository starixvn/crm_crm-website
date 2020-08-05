import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Settings
export const setNotificationTypes = data => {
    return {
        type: types.NOTIFICATIONTYPE_DATA,
        data: data
    };
};

export const getNotificationTypes = () => {
    const notificationTypes = [
        { notificationTypeId: 1, name: 'a' },
        { notificationTypeId: 2, name: 'b' },
        { notificationTypeId: 3, name: 'c' }
    ];
    return setNotificationTypes(notificationTypes);
};

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

//create
export const setCreateTypePopup = data => {
    return {
        type: types.CREATE_POPUP_DATA,
        data: data
    };
};

export const setCreateTypeResult = data => {
    return {
        type: types.CREATE_RESULT_DATA,
        data: data
    };
};

export const createType = data => {
    return async dispatch => axios({
        url: Site.resolveClientUrl('NotificationType/Create'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setCreateTypeResult(response.data));
    }).catch(Site.handleAxiosError);
};

const actions = {
    setNotificationTypes,
    getNotificationTypes,
    setGridParams,
    getGridData,
    setCreateTypePopup
};

export default actions;