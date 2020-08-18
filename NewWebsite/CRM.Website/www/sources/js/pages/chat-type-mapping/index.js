import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
    gridData: null,
    insertPersonalMappingData: { isShow: false },
    updatePersonalMappingData: { isShow: false, data: null },
    groupMappingData: { isShow: false, data: null },
    recipients: []
});

ReactDOM.render(
    <Provider store={store}>
        <AppContainer />
    </Provider>,
    document.getElementById('app-container')
);