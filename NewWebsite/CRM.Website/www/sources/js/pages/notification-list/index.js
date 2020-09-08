import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import Layout from '@jsroot/components/Layout';
import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
    gridParams: {
        pageIndex: 1,
        pageSize: 100
    },
    invalidParams: {},
    contentPopup: { show: false },
    viewDetailPopup: { show: false },
    resendOptions: {
        isCheckedAll: false,
        isDisabledResend: true,
        isHaveResendAll: false,
        shouldResetScrollPosition: true,
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
        <Layout>
            <AppContainer />
        </Layout>
	</Provider>,
	document.getElementById('app-container'));