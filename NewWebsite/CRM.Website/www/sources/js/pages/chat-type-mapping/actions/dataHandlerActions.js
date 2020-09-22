import types from './types';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';

// Settings
export const setSettings = data => {
    return {
        type: types.SETTING_DATA,
        data: data
    };
};

export const getSettings = () => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('ChatTypeMapping/GetSettings');
    await DataService.getDataAsync(url).then(response => {
        dispatch(setSettings(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
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
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setGridData(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

// Update Active Status
const setUpateActiveResult = data => {
    return {
        type: types.ACTIVE_RESULT_DATA,
        data: data
    };
};

export const setUpateActivePopup = data => {
    return {
        type: types.ACTIVE_POPUP_DATA,
        data: data
    };
};

export const updateActiveStatus = params => async dispatch => {
    Site.blockUI();
    await DataService.postDataAsync(Site.resolveClientUrl('ChatTypeMapping/UpdateActiveStatus'), params)
        .then(response => {
            dispatch(setUpateActiveResult(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

// Get Recipient by Recipient Type
export const setRecipientData = data => {
    return {
        type: types.RECIPIENT_DATA,
        data: data
    };
};

export const getRecipientByRecipientType = recipientTypeId => async dispatch => {
    if (recipientTypeId > 0) {
        Site.blockUI();
        const url = Site.resolveClientUrl('ChatTypeMapping/GetRecipientByType');
        await DataService.getDataAsync(url, { recipientTypeId: recipientTypeId }).then(response => {
            dispatch(setRecipientData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
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

export const getGroupHeadlineByRecipient = recipientIds => async dispatch => {
    if (recipientIds) {
        Site.blockUI();
        const url = Site.resolveClientUrl('ChatTypeMapping/GetGroupHeadlineByRecipient');
        await DataService.getDataAsync(url, { recipientIds: recipientIds }).then(response => {
            dispatch(setGroupHeadlineData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    } else {
        dispatch(setGroupHeadlineData([]));
    }
};

const actions = {
    getSettings,
    getGridData,
    setGridParams,
    setUpateActivePopup,
    updateActiveStatus,
    getRecipientByRecipientType,
    getGroupHeadlineByRecipient,
};

export default actions;