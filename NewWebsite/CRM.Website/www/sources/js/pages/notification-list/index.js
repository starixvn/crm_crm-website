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
    errorOrResolvedContentPopup: { show: false },
    resendOptions: {
        isCheckedAll: false,
        dataList: []
    },
    settings: {
        chatTypes: [],
        statuses: []
    }
});

ReactDOM.render(
	<Provider store={store}>
		<AppContainer />
	</Provider>,
	document.getElementById('app-container'));