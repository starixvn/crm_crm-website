import types from './types';
import Site from '@jsroot/common/site';
import { setGroupHeadlineData } from './dataHandlerActions';
import DataService from '@jsroot/common/dataService';

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
        dispatch(setGroupHeadlineData([]));
    };
};

export const insertPersonalMapping = params => async dispatch => {
    const url = Site.resolveClientUrl('ChatTypeMapping/InsertPersonalMapping');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setInsertPersonalMappingResult(response.data));
    }).catch(Site.handleAxiosError);
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
        dispatch(setGroupHeadlineData([]));
    };
};

export const updatePersonalMapping = params => async dispatch => {
    const url = Site.resolveClientUrl(`ChatTypeMapping/UpdatePersonalMapping`);
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setUpdatePersonalMappingResult(response.data));
    }).catch(Site.handleAxiosError);
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
        dispatch(setLetstalkVerifationData([]));
    };
};

export const verifyLetstalkInGroup = params => async dispatch => {
    if (params.botId > 0 && params.groupConversationId !== '' && params.recipients.length > 0) {
        const url = Site.resolveClientUrl('ChatTypeMapping/VerifyLetstalkInGroup');
        await DataService.postDataAsync(url, params).then(response => {
            dispatch(setLetstalkVerifationData(response.data));
        }).catch(Site.handleAxiosError);
    } else {
        dispatch(setLetstalkVerifationData([]));
    }
};

export const updateGroupMapping = params => async dispatch => {
    const url = Site.resolveClientUrl('ChatTypeMapping/UpdateGroupMapping');
    await DataService.postDataAsync(url, params).then(response => {
        dispatch(setGroupMappingResult(response.data));
    }).catch(Site.handleAxiosError);
};

const setLetstalkDisplayName = name => {
    return {
        type: types.LETSTALK_DISPLAY_NAME,
        data: name
    };
};
export const getLetstalkDisplayName = (botId, letstalkId) => async dispatch => {
    if (botId && letstalkId) {
        const url = Site.resolveClientUrl('ChatTypeMapping/GetLetstalkDisplayName');
        const params = { botId: botId, letstalkId: letstalkId };
        await DataService.getDataAsync(url, params).then(response => {
            dispatch(setLetstalkDisplayName(response.data));
        }).catch(Site.handleAxiosError);
    } else {
        dispatch(setLetstalkDisplayName(''));
    }
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
    verifyLetstalkInGroup,
    getLetstalkDisplayName
};

export default actions;