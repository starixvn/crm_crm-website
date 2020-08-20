import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Settings
export const setSettings = data => {
    return {
        type: types.SETTING_DATA,
        data: data
    };
};

export const getSettings = () => {
    Site.blockUI();
    const url = Site.resolveClientUrl('ChatTypeMapping/GetSettings');
    return dispatch => {
        axios.get(url).then(response => {
            dispatch(setSettings(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
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

export const getGridData = params => async dispatch => {
    Site.blockUI();
    console.log(params);
    const url = Site.resolveClientUrl('ChatTypeMapping/GetChatTypeMapping');
    await axios({
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

// Update Active Status
export const setUpateActivePopup = data => {
    return {
        type: types.ACTIVE_POPUP_DATA,
        data: data
    };
};

export const updateActiveStatus = params => {
    return dispatch => {
        return axios({
            method: 'post',
            url: Site.resolveClientUrl('ChatTypeMapping/UpdateActiveStatus'),
            data: params,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setUpateActiveResult(response.data));
            dispatch(setUpateActivePopup({ isShow: false }))
        }).catch(Site.handleAxiosError);
    };
};

export const setUpateActiveResult = data => {
    return {
        type: types.ACTIVE_RESULT_DATA,
        data: data
    };
};

// Get Recipient by HeadlineIds
export const setRecipientData = data => {
    return {
        type: types.RECIPIENT_DATA,
        data: data
    };
};

export const getRecipientByGroupHeadline = headlineIds => {
    if (headlineIds) {
        return dispatch => {
            return axios({
                method: 'post',
                url: Site.resolveClientUrl('ChatTypeMapping/GetRecipientByGroupHeadline'),
                data: { headlineIds: headlineIds },
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'RequestVerificationToken': Site.getXsrfToken()
                }
            }).then(response => {
                dispatch(setRecipientData(response.data));
            }).catch(Site.handleAxiosError);
        };
    }

    return dispatch => {
        dispatch(setRecipientData([]));
    };
};

const actions = {
    getSettings,
    getGridData,
    setGridParams,
    setUpateActivePopup,
    updateActiveStatus,
    getRecipientByGroupHeadline
};

export default actions;