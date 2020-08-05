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
    //const url = Site.resolveClientUrl('Common/GetNotificationTypes');
    //return dispatch => {
    //    return axios.get(url).then(response => {
    //        dispatch(setNotificationTypes(response.data));
    //    }).catch(Site.axiosError);
    //};
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
            url: Site.resolveClientUrl('NotificationTemplate/GetGridData'),
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
export const setCreateTemplatePopup = data => {
    return {
        type: types.CREATE_POPUP_DATA,
        data: data
    };
};

export const setCreateTemplateResult = data => {
    return {
        type: types.CREATE_RESULT_DATA,
        data: data
    };
};

export const createTemplate = data => {
    return async dispatch => axios({
        url: Site.resolveClientUrl('NotificationTemplate/Create'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setCreateTemplateResult(response.data));
    }).catch(Site.handleAxiosError);
};

const actions = {
    setNotificationTypes,
    getNotificationTypes,
    setGridParams,
    getGridData,
    setCreateTemplatePopup,
    setCreateTemplateResult,
    createTemplate
};

export default actions;