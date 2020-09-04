import { createSlice } from '@reduxjs/toolkit';

const notificationTemplateSlice = createSlice({
    name: 'notificationTemplate',
    initialState: {
        notificationTypes: [],
        languages: [],
        gridParams: {
            notificationTypeId: '',
            notificationTemplateName: '',
            pageIndex: 1,
            pageSize: 100
        },
        gridData: null,
        selectedTemplate: null,
        createTemplatePopup: { show: false },
        createTemplateResult: null,
        editTemplatePopup: { show: false },
        editTemplateResult: null,
        deleteTemplatePopup: { show: false },
        contentDetailPopup: { show: false }
    },
    reducers: {
        setNotificationTypes: (state, action) => {
            state.notificationTypes = action.payload;
        },
        setLanguages: (state, action) => {
            state.languages = action.payload;
        },
        setGridParams: (state, action) => {
            state.gridParams = action.payload;
        },
        setGridData: (state, action) => {
            state.gridData = action.payload;
        },
        setSelectedTemplate: (state, action) => {
            state.selectedTemplate = action.payload;
        },
        setCreateTemplatePopup: (state, action) => {
            state.createTemplatePopup = action.payload;
        },
        setCreateTemplateResult: (state, action) => {
            state.createTemplateResult = action.payload;
        },
        setEditTemplatePopup: (state, action) => {
            state.editTemplatePopup = action.payload;
        },
        setEditTemplateResult: (state, action) => {
            state.editTemplateResult = action.payload;
        },
        setDeleteTemplatePopup: (state, action) => {
            state.deleteTemplatePopup = action.payload;
        },
        setContentDetailPopup: (state, action) => {
            state.contentDetailPopup = action.payload;
        }
    }
});

const { actions, reducer } = notificationTemplateSlice;

export const {
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
    setContentDetailPopup
} = actions;

export default reducer;