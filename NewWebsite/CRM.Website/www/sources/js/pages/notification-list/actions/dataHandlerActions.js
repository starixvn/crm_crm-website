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

export const getGridData = (params, done) => async dispatch => {
    Site.blockUI();
    await axios({
        method: 'post',
        url: Site.resolveClientUrl('NotificationList/GetGridData'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        done && done(response.data.data);
        dispatch(setGridData(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
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

export const setErrorResolvedContent = data => {
    return {
        type: types.ERROR_RESOLVED_CONTENT_DATA,
        data
    };
};

export const resendNotification = (params, isUpdateContent) => async dispatch => {
    Site.blockUI();
    await axios({
        method: 'post',
        url: Site.resolveClientUrl('NotificationList/EditNotificationList'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        if (isUpdateContent) {
            dispatch(setErrorResolvedContent(response.data));
        }
        else {
            dispatch(setMessageResult({
                show: true,
                isSuccessful: response.data.result.isSuccessful,
                message: response.data.result.message
            }));
        }
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

export const setMessageResult = data => {
    return {
        type: types.MESSAGE_RESULT_DATA,
        data
    };
};

const actions = {
    setGridParams,
    getGridData,
    setContentPopup,
    setResendOptions,
    setErrorOrResolvedContentPopup,
    setInvalidParams,
    getSettings,
    resendNotification,
    setErrorResolvedContent
};

export default actions;