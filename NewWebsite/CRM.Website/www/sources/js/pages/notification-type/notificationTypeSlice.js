import { createSlice } from '@reduxjs/toolkit';

const notificationTypeSlice = createSlice({
    name: 'notificationType',
    initialState: {
        gridParams: {
            pageIndex: 1,
            pageSize: 100
        },
        createNotificationTypePopup: { show: false },
        createNotificationTypeResult: null,
        editNotificationTypePopup: { show: false },
        editNotificationTypeResult: null,
        deleteNotificationTypePopup: { show: false },
        gridData: null,
        settings: {
            ChatTypes: [],
            RecipientTypes: [],
            Systems: []
        }
    },
    reducers: {
        setGridParams: (state, action) => {
            state.gridParams = action.payload;
        },
        setGridData: (state, action) => {
            state.gridData = action.payload;
        },
        setSettings: (state, action) => {
            state.settings = action.payload;
        },
        setCreateNotificationTypePopup: (state, action) => {
            state.createNotificationTypePopup = action.payload;
        },
        setCreateNotificationTypeResult: (state, action) => {
            state.createNotificationTypeResult = action.payload;
        },
        setEditNotificationTypePopup: (state, action) => {
            state.editNotificationTypePopup = action.payload;
        },
        setEditNotificationTypeResult: (state, action) => {
            state.editNotificationTypeResult = action.payload;
        },
        setDeleteNotificationTypePopup: (state, action) => {
            state.deleteNotificationTypePopup = action.payload;
        }
    }
});

const { actions, reducer } = notificationTypeSlice;

export const {
    setGridParams,
    setGridData,
    setSettings,
    setCreateNotificationTypePopup,
    setCreateNotificationTypeResult,
    setEditNotificationTypePopup,
    setEditNotificationTypeResult,
    setDeleteNotificationTypePopup
} = actions;

export default reducer;