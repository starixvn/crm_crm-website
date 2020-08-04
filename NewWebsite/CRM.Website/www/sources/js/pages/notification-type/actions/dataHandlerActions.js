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
    const url = Site.resolveClientUrl('NotificationType/GetGridData');
    return dispatch => {
        return axios({
            method: 'post',
            url: url,
            data: params,
            headers: {
                '__RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setGridData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

const actions = {
    setNotificationTypes,
    getNotificationTypes,
    setGridParams,
    getGridData
};

export default actions;