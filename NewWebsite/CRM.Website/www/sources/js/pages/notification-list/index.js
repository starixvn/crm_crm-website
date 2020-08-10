import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
    contentPopup: { show: false },
    errorOrResolvedContentPopup: { show: false },
    resendOptions: {
        isCheckedAll: false,
        dataList: []
    }
});

ReactDOM.render(
    <Provider store={store}>
        <AppContainer />
    </Provider>,
 document.getElementById('app-container'));