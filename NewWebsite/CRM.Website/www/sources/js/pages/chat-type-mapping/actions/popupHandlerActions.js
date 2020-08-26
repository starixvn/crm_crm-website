import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Insert Personal Mapping Popup
const setInsertPersonalMappingPopup = isShow => {
    return {
        type: types.PERSONAL_MAPPING_INSERT_POPUP_DATA,
        data: {
            isShow: isShow
        }
    };
};

export const setInsertPersonalMappingResult = data => {
    return {
        type: types.PERSONAL_MAPPING_INSERT_RESULT,
        data: data
    };
};

export const openInsertPersonalMappingPopup = () => {
    return dispatch => {
        dispatch(setInsertPersonalMappingPopup(true));
    };
};

export const closeInsertPersonalMappingPopup = () => {
    return dispatch => {
        dispatch(setInsertPersonalMappingPopup(false));
        dispatch(setInsertPersonalMappingResult(null));
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
const setUpdatePersonalMappingData = (isShow, data) => {
    return {
        type: types.PERSONAL_MAPPING_UPDATE_POPUP_DATA,
        data: {
            isShow: isShow,
            data: data
        }
    };
};

export const setUpdatePersonalMappingResult = data => {
    return {
        type: types.PERSONAL_MAPPING_UPDATE_RESULT,
        data: data
    };
};

export const openUpdatePersonalMappingPopup = (data) => {
    return dispatch => {
        dispatch(setUpdatePersonalMappingData(true, data));
    };
};

export const closeUpdatePersonalMappingPopup = () => {
    return dispatch => {
        dispatch(setUpdatePersonalMappingData(false, null));
        dispatch(setUpdatePersonalMappingResult(null));
    };
};

export const updatePersonalMapping = params => {
    const url = Site.resolveClientUrl(`ChatTypeMapping/UpdatePersonalMapping`);
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
            dispatch(setUpdatePersonalMappingResult(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
    };
};

// Update Group Mapping
const setUpdateGroupMappingData = (isShow, data) => {
    return {
        type: types.GROUP_MAPPING_POPUP_DATA,
        data: {
            isShow: isShow,
            data: data
        }
    };
};

const setLetstalkVerifationData = data => {
    return {
        type: types.GROUP_MAPPING_LETSTALK_VERIFICATION_DATA,
        data: data
    };
};

export const setGroupMappingResult = data => {
    return {
        type: types.GROUP_MAPPING_UPDATE_RESULT,
        data: data
    };
};

export const openUpdateGroupMappingPopup = data => {
    return dispatch => {
        dispatch(setUpdateGroupMappingData(true, data));
    };
};

export const closeUpdateGroupMappingPopup = () => {
    return dispatch => {
        dispatch(setUpdateGroupMappingData(false, null));
        dispatch(setGroupMappingResult(null));
    };
};

export const verifyLetstalkInGroup = params => {
    const url = Site.resolveClientUrl('ChatTypeMapping/VerifyLetstalkInGroup');
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
            dispatch(setLetstalkVerifationData(response.data));
        }).catch(Site.handleAxiosError).then(() => Site.unblockUI());
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

const actions = {
    openInsertPersonalMappingPopup,
    closeInsertPersonalMappingPopup,
    insertPersonalMapping,
    setInsertPersonalMappingResult,
    openUpdatePersonalMappingPopup,
    updatePersonalMapping,
    closeUpdatePersonalMappingPopup,
    setUpdatePersonalMappingResult,
    openUpdateGroupMappingPopup,
    closeUpdateGroupMappingPopup,
    updateGroupMapping,
    setGroupMappingResult,
    verifyLetstalkInGroup
};

export default actions;