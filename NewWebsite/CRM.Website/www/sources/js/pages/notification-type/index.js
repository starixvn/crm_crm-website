import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';
import Layout from '@jsroot/components/Layout';
import AppContainer from './components/AppContainer';
import notificationTypeReducer from './notificationTypeSlice';

const store = configureStore({
    reducer: notificationTypeReducer
});

const isHost = document.getElementById('IsHost').checked;

ReactDOM.render(
    <Provider store={store}>
        <Layout>
            <AppContainer isHost={isHost} />
        </Layout>
    </Provider>,
    document.getElementById('app-container'));