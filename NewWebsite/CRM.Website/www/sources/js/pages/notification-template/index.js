import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';

import notificationTemplateReducer from './notificationTemplateSlice';
import AppContainer from './components/AppContainer';

const store = configureStore({
    reducer: notificationTemplateReducer
});

ReactDOM.render(
    <Provider store={store}>
        <AppContainer />
    </Provider>,
 document.getElementById('app-container'));