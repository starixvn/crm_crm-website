import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
    gridData: { data: null, totalItems: 0 },
    insertPersonalMappingData: { isShow: false },
    insertPersonalMappingResult: null,
    updatePersonalMappingData: { isShow: false, data: null },
    updatePersonalMappingResult: null,
    updateGroupMappingData: { isShow: false, data: null },
    recipients: [],
    groupHeadline: []
});

ReactDOM.render(
    <Provider store={store}>
        <AppContainer />
    </Provider>,
    document.getElementById('app-container')
);