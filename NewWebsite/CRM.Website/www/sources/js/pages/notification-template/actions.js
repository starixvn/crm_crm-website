import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
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
    await DataService.getDataAsync(url).then(response => {
        dispatch(setNotificationTypes(response.data));
    });
};

const getLanguages = () => async dispatch => {
    const url = Site.resolveClientUrl('NotificationTemplate/GetLanguages');
    await DataService.getDataAsync(url).then(response => {
        dispatch(setLanguages(response.data));
    });
};

const getGridData = params => async dispatch => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationTemplate/GetGridData');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setGridData(response.data));
    }).then(() => Site.unblockUI());
};

const getTemplateById = (templateId, callback) => async dispatch => {
    const url = Site.resolveClientUrl(`NotificationTemplate/GetById/${templateId}`);
    await DataService.getDataAsync(url).then(response => {
        dispatch(setSelectedTemplate(response.data));
        if (typeof callback === 'function') {
            callback();
        }
    });
};

const createTemplate = data => async dispatch => {
    const url = Site.resolveClientUrl('NotificationTemplate/Create');
    await DataService.postDataAsync(url, data).then(response => {
        dispatch(setCreateTemplateResult(response.data));
    });
};

const editTemplate = (data) => async dispatch => {
    const url = Site.resolveClientUrl('NotificationTemplate/Edit');
    await DataService.postDataAsync(url, data).then(response => {
        dispatch(setEditTemplateResult(response.data));
    });
};

const deleteTemplate = templateId => async dispatch => {
    const url = Site.resolveClientUrl(`NotificationTemplate/Delete/${templateId}`);
    return await DataService.postDataAsync(url).then(response => response.data);
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