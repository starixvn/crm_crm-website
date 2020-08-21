import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';

import notificationTemplateReducer from './notificationTemplateSlice';
import AppContainer from './components/AppContainer';

const store = configureStore({
    reducer: notificationTemplateReducer
});

const isHost = document.getElementById('IsHost').checked;

ReactDOM.render(
    <Provider store={store}>
        <AppContainer isHost={isHost} />
    </Provider>,
 document.getElementById('app-container'));