import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Settings

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
    const url = Site.resolveClientUrl('NotificationTemplate/GetGridData');
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
            dispatch(setGridData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

const actions = {
    setGridParams,
    getGridData
};

export default actions;