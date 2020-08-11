import types from './types';

// Update Group Mapping Popup
export const openEditGroupMappingPopup = item => {
	const popupData = {
		isShowed: true,
		groupMappingItem: {
			groupID: item.groupID,
			conversationID: item.conversationID,
			mappingStatusId: item.mappingStatusID,
			mappingStatusName: item.mappingStatusName,
			botName: item.botName,
			groupName: item.groupName,
			recipientTypeId: {
				value: item.recipientTypeID,
				isValid: item.recipientTypeID !== 0
			},
			recipientIds: {
				value: item.recipientIDs,
				isValid: item.recipientIDs !== ''
			},
			groupHeadlineIds: {
				value: item.groupHeadlineIds,
				isValid: item.groupHeadlineIds !== ''
			},
			languageIds: {
				value: item.languageIds,
				isValid: item.languageIds !== ''
			},
			remark: {
				value: item.remark
			}
		}
	};

	return {
		type: types.GROUP_MAPPING_POPUP_DATA,
		data: popupData
	};
};

export const closeEditGroupMappingPopup = () => {
	const popupData = {
		isShowed: false,
		editingItem: {}
	};

	return {
		type: types.GROUP_MAPPING_POPUP_DATA,
		data: popupData
	};
};

// Update Personal Mapping Popup
export const openPersonalMappingPopup = item => {
	const popupData = {
		isShowed: true,
		personalMappingItem: {
			botId: {
				value: item.botId,
				isValid: item.botId !== 0
			},
			botName: item.groupID,
			letstalkName: item.letstalkName,
			letstalkId: {
				value: item.letstalkId,
				isValid: item.letstalkId !== ''
			},
			recipientTypeId: {
				value: item.recipientTypeID,
				isValid: item.recipientTypeID !== 0
			},
			recipientIds: {
				value: item.recipientIDs,
				isValid: item.recipientIDs !== ''
			},
			languageIds: {
				value: item.languageIds,
				isValid: item.languageIds !== ''
			},
			remark: {
				value: item.remark
			}
		}
	};

	return {
		type: types.PERSONAL_MAPPING_POPUP_DATA,
		data: popupData
	};
};

export const closePersonalMappingPopup = () => {
	const popupData = {
		isShowed: false,
		editingItem: {}
	};

	return {
		type: types.PERSONAL_MAPPING_POPUP_DATA,
		data: popupData
	};
};

const actions = {
	openEditGroupMappingPopup,
	closeEditGroupMappingPopup,
	openPersonalMappingPopup,
	closePersonalMappingPopup
};

export default actions;