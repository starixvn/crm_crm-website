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

export const updateActiveStatus = params => async dispatch => {
    await axios({
        method: 'post',
        url: Site.resolveClientUrl('ChatTypeMapping/UpdateActiveStatus'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setUpateActiveResult(response.data));
        dispatch(setUpateActivePopup({ isShow: false }));
    }).catch(Site.handleAxiosError);
};

export const setUpateActiveResult = data => {
    return {
        type: types.ACTIVE_RESULT_DATA,
        data: data
    };
};

// Get Recipient by Recipient Type
export const setRecipientData = data => {
    return {
        type: types.RECIPIENT_DATA,
        data: data
    };
};

export const getRecipientByRecipientType = recipientTypeId => {
    if (recipientTypeId) {
        return dispatch => {
            return axios({
                method: 'post',
                url: Site.resolveClientUrl('ChatTypeMapping/GetRecipientByType'),
                data: { recipientTypeId: recipientTypeId },
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

// Get Group Headline by Recipients
export const setGroupHeadlineData = data => {
    return {
        type: types.GROUP_HEADLINE_DATA,
        data: data
    };
};

export const getGroupHeadlineByRecipient = recipientIds => {
    if (recipientIds) {
        return dispatch => {
            return axios({
                method: 'post',
                url: Site.resolveClientUrl('ChatTypeMapping/GetGroupHeadlineByRecipient'),
                data: { recipientIds: recipientIds },
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'RequestVerificationToken': Site.getXsrfToken()
                }
            }).then(response => {
                dispatch(setGroupHeadlineData(response.data));
            }).catch(Site.handleAxiosError);
        };
    }

    return dispatch => {
        dispatch(setGroupHeadlineData([]));
    };
};

const actions = {
    getSettings,
    getGridData,
    setGridParams,
    setUpateActivePopup,
    updateActiveStatus,
    getRecipientByRecipientType,
    getGroupHeadlineByRecipient
};

export default actions;