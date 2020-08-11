import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Grid
export const setGridParams = data => {
    return {
        type: types.GRID_PARAMS,
        data
    };
};

export const setGridData = data => {
    return {
        type: types.GRID_DATA,
        data
    };
};

export const getGridData = (params, done) => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationList/GetGridData');
    return dispatch => {
        return axios.get(url, { params: params }).then(response => {
            done && done(response.data.data);
            dispatch(setGridData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

export const setContentPopup = data => {
    return {
        type: types.CONTENT_POPUP_DATA,
        data
    };
};

export const setErrorOrResolvedContentPopup = data => {
    return {
        type: types.ERROR_RESOLVED_CONTENT_POPUP_DATA,
        data
    };
};

export const setResendOptions = data => {
    return dispatch => dispatch(setResendOptionsData(data));
};

export const setResendOptionsData = data => {
    return {
        type: types.RESEND_OPTIONS_DATA,
        data
    };
};

export const setInvalidParams = data => {
    return {
        type: types.INVALID_PARAMS_DATA,
        data
    };
};

export const setSettings = data => {
    return {
        type: types.SETTINGS_DATA,
        data
    };
};

export const getSettings = () => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationList/GetSettings');
    return dispatch => {
        return axios.get(url).then(response => {
            dispatch(setSettings(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

const actions = {
    setGridParams,
    getGridData,
    setContentPopup,
    setResendOptions,
    setErrorOrResolvedContentPopup,
    setInvalidParams,
    getSettings
};

export default actions;