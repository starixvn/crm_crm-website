import types from './types';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';

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

export const getGridData = (params, callback) => async dispatch => {
    Site.blockUI();
    if (params.messageId !== '' || params.notificationIdLower !== '') {
        params.fromDate = null;
        params.toDate = null;
    }
    
    const url = Site.resolveClientUrl('NotificationList/GetGridData');
    await DataService.postDataAsync(url, params).then(response => {
        if (typeof callback === 'function') {
            callback(response.data.data);
        }
        
        dispatch(setGridData(response.data));
    }).then(() => Site.unblockUI());
};

export const setContentPopup = data => {
    return {
        type: types.CONTENT_POPUP_DATA,
        data
    };
};

export const setViewDetailPopup = data => {
    return {
        type: types.VIEW_DETAIL_POPUP_DATA,
        data
    };
};

export const setResendOptions = data => {
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

export const getSettings = () => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationList/GetSettings');
    await DataService.getDataAsync(url).then(response => {
        dispatch(setSettings(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

export const setViewDetail = data => {
    return {
        type: types.VIEW_DETAIL_DATA,
        data
    };
};

export const editNotificationList = params => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationList/EditNotificationList');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setViewDetail(response.data));
    }).then(() => Site.unblockUI());
};

export const setMessageResult = data => {
    return {
        type: types.MESSAGE_RESULT_DATA,
        data
    };
};

export const validateResend = params => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationList/ValidateNotificationList');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setValidateResend({
            show: true,
            result: response.data
        }));
    }).then(() => Site.unblockUI());
};

export const setValidateResend = data => {
    return {
        type: types.VALIDATE_RESEND_DATA,
        data
    };
};

export const setConfirmPopup = data => {
    return {
        type: types.CONFIRM_POPUP_DATA,
        data
    };
};

export const getHeadlineOptions = searchText => async dispatch => {
    const params = {
        username: searchText,
        limit: 10,
        excludeNotAssignedGroup: true
    };

    const url = Site.resolveClientUrl('Common/GetHeadlines');
    await DataService.postDataAsync(url, params).then(response => {
        const options = response.data.map(item => {
            return { value: item.name, label: item.name };
        });

        dispatch(setHeadlineOptions(options));
    });
};

export const getRecipientOptions = searchText => async dispatch => {
    const params = {
        recipientName: searchText,
        limit: 10
    };

    const url = Site.resolveClientUrl('Common/GetRecipients');
    await DataService.postDataAsync(url, params).then(response => {
        const options = response.data.map(item => {
            return { value: item.name, label: item.name };
        });

        dispatch(setRecipientOptions(options));
    });
};

export const setHeadlineOptions = data => {
    return {
        type: types.HEADLINE_OPTIONS_DATA,
        data
    };
};

export const setRecipientOptions = data => {
    return {
        type: types.RECIPIENT_OPTIONS_DATA,
        data
    };
};

const actions = {
    setGridParams,
    getGridData,
    setContentPopup,
    setResendOptions,
    setViewDetailPopup,
    setInvalidParams,
    getSettings,
    editNotificationList,
    setViewDetail,
    validateResend,
    setValidateResend,
    setMessageResult,
    setConfirmPopup,
    getHeadlineOptions,
    setHeadlineOptions,
    getRecipientOptions,
    setRecipientOptions
};

export default actions;