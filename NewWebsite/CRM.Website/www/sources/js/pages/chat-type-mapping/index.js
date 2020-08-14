import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
	gridData: null,
	personalMappingData: {
		isShowed: false,
		formData: {
			conversationId: 0,
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
	}
});

ReactDOM.render(
	<Provider store={store}>
		<AppContainer />
	</Provider>,
	document.getElementById('app-container')
);