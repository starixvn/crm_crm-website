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

// edit
export const setEditTypePopup = data => {
    return {
        type: types.EDIT_POPUP_DATA,
        data: data
    };
};

export const setEditTypeResult = data => {
    return {
        type: types.EDIT_RESULT_DATA,
        data: data
    };
};

// Delete
export const setDeleteTypePopup = data => {
    return {
        type: types.DELETE_POPUP_DATA,
        data: data
    };
};

export const setDeleteTypeResult = data => {
    return {
        type: types.DELETE_RESULT_DATA,
        data: data
    };
};

export const deleteType = typeId => {
    return async dispatch => axios({
        url: Site.resolveClientUrl(`NotificationType/Delete/${typeId}`),
        method: 'post',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => response.data).catch(Site.handleAxiosError);
};

export const getTypeById = (typeId) => {
    return async dispatch => axios(Site.resolveClientUrl(`NotificationType/GetById/${typeId}`))
        .then(response => {
            dispatch(setEditTypePopup({ show: true, type: response.data }));
        }).catch(Site.handleAxiosError);
};

export const editType = (data) => {
    return async dispatch => axios({
        url: Site.resolveClientUrl('NotificationType/Edit'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setEditTypeResult(response.data));
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
    setCreateTypePopup,
    setCreateTypeResult,
    createType,
    setEditTypePopup,
    setEditTypeResult,
    getTypeById,
    editType, 
    setDeleteTypePopup,
    setDeleteTypeResult,
    deleteType
};

export default actions;