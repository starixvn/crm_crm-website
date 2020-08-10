import { createSlice } from '@reduxjs/toolkit';

const notificationTemplateSlice = createSlice({
    name: 'notificationTemplate',
    initialState: {
        notificationTemplates: []
    },
    reducers: {
        createTemplate(state, action) { },
        editTemplate(state, action) { },
        deleteTemplate(state, action) { }
    }
});

const { actions, reducer } = notificationTemplateSlice;

export const { createTemplate, editTemplate, deleteTemplate } = actions;

export default reducer;