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
	const url = Site.resolveClientUrl('ChatTypeMapping/GroupMapping');
	const headers = {
		'Content-Type': 'application/json',
		'RequestVerificationToken': Site.getXsrfToken()
	};

	return async dispatch => {
		try {
			const request = await axios.post(url, JSON.stringify(params), { headers: headers });
			const data = await request.data;
			dispatch(setUpdateData(data));
		} catch (err) {
			errorResponseHandler();
		}
	};
};

export const setPersonalUpdateData = data => {
	return {
		type: types.PERSONAL_MAPPING_UPDATE_DATA,
		data: data
	};
};

export const updatePersonalMapping = params => {
	const url = Site.resolveClientUrl('ChatTypeMapping/PersonalMapping');
	const headers = {
		'Content-Type': 'application/json',
		'RequestVerificationToken': Site.getXsrfToken()
	};

	return async dispatch => {
		try {
			const request = await axios.post(url, JSON.stringify(params), { headers: headers });
			const data = await request.data;
			dispatch(setUpdateData(data));
		} catch (err) {
			errorResponseHandler();
		}
	};
};

export const updateActiveStatus = (conversationId, isActive) => {
	const url = Site.resolveClientUrl('ChatTypeMapping/UpdateActiveStatus');
	const headers = {
		'Content-Type': 'application/json',
		'RequestVerificationToken': Site.getXsrfToken()
	};

	return async dispatch => {
		try {
			const request = await axios.post(url, JSON.stringify({ conversationId, isActive }), { headers: headers });
			const data = await request.data;
			dispatch(setUpdateData(data));
		} catch (err) {
			errorResponseHandler();
		}
	};
};

export const refreshUpdateData = () => {
	return dispatch => {
		dispatch(setPersonalUpdateData(null));
		dispatch(setGroupMappingUpdateData(null));
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
	updateActiveStatus
};

export default actions;