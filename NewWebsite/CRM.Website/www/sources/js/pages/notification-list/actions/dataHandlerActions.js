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
        return axios({
            method: 'post',
            url: url,
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

const actions = {
    setGridParams,
    getGridData,
    setContentPopup,
    setResendOptions,
    setErrorOrResolvedContentPopup
};

export default actions;