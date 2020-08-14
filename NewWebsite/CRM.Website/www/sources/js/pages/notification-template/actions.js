import axios from 'axios';
import Site from '@jsroot/common/site';
import {
    setNotificationTypes,
    setLanguages,
    setGridParams,
    setGridData,
    setSelectedTemplate,
    setCreateTemplatePopup,
    setCreateTemplateResult,
    setEditTemplatePopup,
    setEditTemplateResult,
    setDeleteTemplatePopup,
    setDeleteTemplateResult,
    setContentDetailPopup
} from './notificationTemplateSlice';

const getNotificationTypes = () => async dispatch => {
    const url = Site.resolveClientUrl('Common/GetNotificationTypes');
    await axios.get(url).then(response => {
        dispatch(setNotificationTypes(response.data));
    }).catch(Site.handleAxiosError);
};

const getLanguages = () => async dispatch => {
    const url = Site.resolveClientUrl('NotificationTemplate/GetLanguages');
    await axios.get(url).then(response => {
        dispatch(setLanguages(response.data));
    }).catch(Site.handleAxiosError);
};

const getGridData = params => async dispatch => {
    Site.blockUI();
    await axios({
        method: 'post',
        url: Site.resolveClientUrl('NotificationTemplate/GetGridData'),
        data: params,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setGridData(response.data));
    }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
};

const getTemplateById = (templateId, callback) => async dispatch => {
    await axios(Site.resolveClientUrl(`NotificationTemplate/GetById/${templateId}`))
        .then(response => {
            dispatch(setSelectedTemplate(response.data));

            if (typeof callback === 'function') {
                callback();
            }
        }).catch(Site.handleAxiosError);
};

const createTemplate = data => async dispatch => {
    await axios({
        url: Site.resolveClientUrl('NotificationTemplate/Create'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setCreateTemplateResult(response.data));
    }).catch(Site.handleAxiosError);
};

const editTemplate = (data) => async dispatch => {
    await axios({
        url: Site.resolveClientUrl('NotificationTemplate/Edit'),
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => {
        dispatch(setEditTemplateResult(response.data));
    }).catch(Site.handleAxiosError);
};

const deleteTemplate = templateId => async dispatch => {
    return await axios({
        url: Site.resolveClientUrl(`NotificationTemplate/Delete/${templateId}`),
        method: 'post',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': Site.getXsrfToken()
        }
    }).then(response => response.data).catch(Site.handleAxiosError);
};

export const ActionCreators = {
    setNotificationTypes,
    setLanguages,
    setGridParams,
    setGridData,
    setSelectedTemplate,
    setCreateTemplatePopup,
    setCreateTemplateResult,
    setEditTemplatePopup,
    setEditTemplateResult,
    setDeleteTemplatePopup,
    setDeleteTemplateResult,
    setContentDetailPopup,
    getNotificationTypes,
    getLanguages,
    getGridData,
    getTemplateById,
    createTemplate,
    editTemplate,
    deleteTemplate
};