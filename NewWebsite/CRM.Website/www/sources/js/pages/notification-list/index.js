import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
    gridParams: {
        pageIndex: 1,
        pageSize: 100
    },
    invalidParams: {
        show: false
    },
    contentPopup: { show: false },
    viewDetailPopup: { show: false },
    resendOptions: {
        isCheckedAll: false,
        isDisabledResend: true,
        isHaveResendAll: false,
        dataList: []
    },
    settings: {
        notificationTypes: [],
        statuses: []
    },
    headlineOptions: [],
    recipientOptions: [],
    viewDetail: {
        resolvedBy: '',
        result: {
            code: 1,
            message: '',
            isSuccessful: false
        }
    },
    messageResult: {
        isSuccessful: false,
        show: false,
        message: ''
    },
    validateResendResult: {
        show: false,
        result: {
            isSingle: false,
            inActiveNotificationIds: [],
            resendDataList: []
        }
    },
    confirmPopup: {
        show: false,
        onOk: null
    }
});

ReactDOM.render(
	<Provider store={store}>
		<AppContainer />
	</Provider>,
	document.getElementById('app-container'));