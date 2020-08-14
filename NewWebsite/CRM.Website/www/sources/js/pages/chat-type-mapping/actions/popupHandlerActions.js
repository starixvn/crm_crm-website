import types from './types';

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
export const openPersonalMappingPopup = data => {
	const popupData = {
		isShowed: true,
		formData: {
			conversationId: data.conversationId,
			botId: data.botId,
			botName: data.botName,
			letstalkId: data.letstalkId,
			groupHeadlineIds: {
				value: data.groupHeadlineIds.value,
				isValid: data.groupHeadlineIds.value !== ''
			},
			recipientTypeId: data.recipientTypeId,
			recipientIds: {
				value: data.recipientIds.value,
				isValid: data.recipientIds.value !== ''
			},
			languageId: data.languageId,
			remark: data.remark
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
		formData: {
			conversationId: '',
			botId: '',
			botName: '',
			letstalkId: '',
			groupHeadlineIds: {
				value: '',
				isValid: false
			},
			recipientTypeId: '',
			recipientIds: {
				value: '',
				isValid: false
			},
			languageId: '',
			remark: ''
		}
	};

	return {
		type: types.PERSONAL_MAPPING_POPUP_DATA,
		data: popupData
	};
};

const actions = {
	closeEditGroupMappingPopup,
	openPersonalMappingPopup,
	closePersonalMappingPopup
};

export default actions;