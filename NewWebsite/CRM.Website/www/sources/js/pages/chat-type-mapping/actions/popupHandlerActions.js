import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Insert Personal Mapping Popup
export const openInsertPersonalMappingPopup = () => {
    return {
        type: types.PERSONAL_MAPPING_INSERT_POPUP_DATA,
        data: {
            isShow: true
        }
    };
};

export const closeInsertPersonalMappingPopup = () => {
    return {
        type: types.PERSONAL_MAPPING_INSERT_POPUP_DATA,
        data: {
            isShow: false
        }
    };
};

export const setInsertPersonalMappingResult = data => {
    return {
        type: types.PERSONAL_MAPPING_INSERT_RESULT,
        data: data
    };
};

export const insertPersonalMapping = params => {
    const url = Site.resolveClientUrl('ChatTypeMapping/InsertPersonalMapping');
    return dispatch => {
        return axios({
            method: 'post',
            url: url,
            data: params,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setInsertPersonalMappingResult(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

// Update Personal Mapping
export const openUpdatePersonalMappingPopup = conversationId => {
    const url = Site.resolveClientUrl('ChatTypeMapping/GetPersonalMapping');
    return dispatch => {
        return axios({
            method: 'post',
            url: url,
            data: { conversationId: conversationId },
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setUpdatePersonalMappingData({ isShow: true, data: response.data }));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
}

export const setUpdatePersonalMappingData = data => {
    return {
        type: types.PERSONAL_MAPPING_UPDATE_DATA,
        data: data
    };
};

export const updatePersonalMapping = data => {
    const url = Site.resolveClientUrl('ChatTypeMapping/UpdatePersonalMapping');
    return dispatch => {
        return axios({
            method: 'post',
            url: url,
            data: data,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setUpdatePersonalMappingResult(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

export const closeUpdatePersonalMappingPopup = () => {
    return {
        type: types.PERSONAL_MAPPING_UPDATE_POPUP_DATA,
        data: {
            isShow: false,
            data: null
        }
    };
};

export const setUpdatePersonalMappingResult = data => {
    return {
        type: types.PERSONAL_MAPPING_UPDATE_RESULT,
        data: data
    };
}

// Update Group Mapping
export const openUpdateGroupMappingPopup = conversationId => {
    const url = Site.resolveClientUrl('ChatTypeMapping/GetGroupMapping');
    return dispatch => {
        return axios({
            method: 'post',
            url: url,
            data: { conversationId: conversationId },
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setGroupMappingUpdateData({ isShow: true, data: response.data }));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

export const closeUpdateGroupMappingPopup = () => {
    const popupData = {
        isShow: false,
        data: null
    };

    return {
        type: types.GROUP_MAPPING_POPUP_DATA,
        data: popupData
    };
}

export const setGroupMappingResult = data => {
    return {
        type: types.GROUP_MAPPING_UPDATE_RESULT,
        data: data
    };
};

export const updateGroupMapping = params => {
    const url = Site.resolveClientUrl('ChatTypeMapping/UpdateGroupMapping');
    return dispatch => {
        return axios({
            method: 'post',
            url: url,
            data: params,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'RequestVerificationToken': Site.getXsrfToken()
            }
        }).then(response => {
            dispatch(setGroupMappingResult(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

export const setGroupMappingUpdateData = data => {
    return {
        type: types.GROUP_MAPPING_UPDATE_DATA,
        data: data
    };
};

const actions = {
    openInsertPersonalMappingPopup,
    closeInsertPersonalMappingPopup,
    insertPersonalMapping,
    openUpdatePersonalMappingPopup,
    updatePersonalMapping,
    closeUpdatePersonalMappingPopup,
    openUpdateGroupMappingPopup,
    closeUpdateGroupMappingPopup,
    updateGroupMapping
};

export default actions;