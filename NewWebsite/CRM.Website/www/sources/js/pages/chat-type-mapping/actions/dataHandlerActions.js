import types from './types';
import axios from 'axios';
import Site from '@jsroot/common/site';

// Settings
export const setSettings = data => {
	return {
		type: types.SETTING_DATA,
		data: data
	};
};

export const getSettings = () => {
	Site.blockUI();
	const url = Site.resolveClientUrl('ChatTypeMapping/GetSettings');
	return dispatch => {
		axios.get(url).then(response => {
			dispatch(setSettings(response.data));
		}).catch(Site.handleAxiosError).then(() => Site.unblockUI());
	};
};

// Grid
export const setGridParams = value => {
	return {
		type: types.GRID_PARAMS,
		data: value
	};
};

export const setGridData = data => {
	return {
		type: types.GRID_DATA,
		data: data
	};
};

export const getGridData = params => {
	Site.blockUI();
	const url = Site.resolveClientUrl('ChatTypeMapping/GetChatTypeMapping');
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
			dispatch(setGridData(response.data));
		}).catch(Site.handleAxiosError).then(() => Site.unblockUI());
	};
};

export const setGroupMappingUpdateData = data => {
	return {
		type: types.GROUP_MAPPING_UPDATE_DATA,
		data: data
	};
};

export const updateGroupMapping = params => {
};

export const setPersonalUpdateData = data => {
	return {
		type: types.PERSONAL_MAPPING_UPDATE_DATA,
		data: data
	};
};

export const updatePersonalMapping = params => {
};

// Update Active Status
export const setUpateActivePopup = data => {
	return {
		type: types.ACTIVE_POPUP_DATA,
		data: data
	};
};

export const updateActiveStatus = params => {
	return dispatch => {
		return axios({
			method: 'post',
			url: Site.resolveClientUrl('ChatTypeMapping/UpdateActiveStatus'),
			data: params,
			headers: {
				'X-Requested-With': 'XMLHttpRequest',
				'RequestVerificationToken': Site.getXsrfToken()
			}
		}).then(response => {
			dispatch(setUpateActiveResult(response.data));
			dispatch(setUpateActivePopup({ isShow: false }))
		}).catch(Site.handleAxiosError);
	};
}

export const setUpateActiveResult = data => {
	return {
		type: types.ACTIVE_RESULT_DATA,
		data: data
	};
};

export const refreshUpdateData = () => {
	return dispatch => {
		dispatch(setPersonalUpdateData(null));
		dispatch(setGroupMappingUpdateData(null));
		dispatch(setUpateActivePopup(null));
		dispatch(setUpateActiveResult(null));
	}
};

const actions = {
	getSettings,
	getGridData,
	setGridParams,
	setPersonalUpdateData,
	setGroupMappingUpdateData,
	updateGroupMapping,
	updatePersonalMapping,
	refreshUpdateData,
	setUpateActivePopup,
	updateActiveStatus
};

export default actions;