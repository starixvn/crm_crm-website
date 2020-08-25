import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';
import { convertDateToString } from '../../../common/functions';

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
    if (params.messageId !== '' || params.notificationIdLower !== '') {
        params.fromDate = null;
        params.toDate = null;
    }
    else {
        params.fromDate = convertDateToString(params.fromDate);
        params.toDate = convertDateToString(params.toDate);
    }
    
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
    await axios({
        method: 'get',
        url: Site.resolveClientUrl('NotificationList/GetSettings')
    }).then(response => {
        dispatch(setSettings(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

export const setViewDetail = data => {
    return {
        type: types.ERROR_RESOLVED_CONTENT_DATA,
        data
    };
};

export const editNotificationList = params => async dispatch => {
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
        dispatch(setViewDetail(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

export const setMessageResult = data => {
    return {
        type: types.MESSAGE_RESULT_DATA,
        data
    };
};

export const validateResend = params => async dispatch => {
    Site.blockUI();
    await axios({
        method: 'post',
        url: Site.resolveClientUrl('NotificationList/ValidateNotificationList'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setValidateResend({
            show: true,
            result: response.data
        }));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
}

export const setValidateResend = data => {
    return {
        type: types.VALIDATE_RESEND_DATA,
        data
    };
};

export const resendNotifications = (dataList, done) => async dispatch => {
    Site.blockUI();
    await axios({
        method: 'post',
        url: Site.resolveClientUrl('NotificationList/ResendNotificationList'),
        data: dataList,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        const { message, isSuccessful } = response.data;
        done && done();
        dispatch(setMessageResult({
            show: true,
            message,
            isSuccessful
        }));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
}

export const setConfirmPopup = data => {
    return {
        type: types.CONFIRM_POPUP_DATA,
        data
    };
};

export const getHeadlineOptions = searchText => async dispatch => {
    const params = {
        username: searchText,
        limit: 10
    };

    await axios({
        method: 'post',
        url: Site.resolveClientUrl('Common/GetHeadlines'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        const options = response.data.map(item => {
            return { value: item.name, label: item.name }
        });

        dispatch(setHeadlineOptions(options))
    }).catch(Site.handleAxiosError);
}

export const getRecipientOptions = searchText => async dispatch => {
    const params = {
        recipientName: searchText,
        limit: 10
    };

    await axios({
        method: 'post',
        url: Site.resolveClientUrl('Common/GetRecipients'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        const options = response.data.map(item => {
            return { value: item.name, label: item.name }
        });

        dispatch(setRecipientOptions(options))
    }).catch(Site.handleAxiosError);
}

export const setHeadlineOptions = data => {
    return {
        type: types.HEADLINE_OPTIONS_DATA,
        data
    };
}

export const setRecipientOptions = data => {
    return {
        type: types.RECIPIENT_OPTIONS_DATA,
        data
    };
}

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
    resendNotifications,
    setMessageResult,
    setConfirmPopup,
    getHeadlineOptions,
    setHeadlineOptions,
    getRecipientOptions,
    setRecipientOptions
};

export default actions;