import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import Layout from '@jsroot/components/Layout';
import Store from '@jsroot/common/store';
import AppContainer from './components/AppContainer';
import reducer from './reducers/index';

const store = Store.createStore(reducer, {
    createNotificationTypePopup: { show: false },
    createNotificationTypeResult: null,
    editNotificationTypePopup: { show: false },
    editNotificationTypeResult: null,
    deleteNotificationTypePopup: { show: false },
    gridData: null,
    settings: {
        ChatTypes: [],
        RecipientTypes: [],
        Systems: []
    }
});
const isHost = document.getElementById('IsHost').checked;

ReactDOM.render(
    <Provider store={store}>
        <Layout>
            <AppContainer isHost={isHost} />
        </Layout>
    </Provider>,
    document.getElementById('app-container'));