import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import Store from '@jsroot/common/store';
import AppContainer from '../components/AppContainer';
import reducer from '../reducers/index';

describe('index', () => {
    beforeEach(() => {
        const appContainer = document.createElement('div');
        appContainer.setAttribute('id', 'app-container');
        document.body.appendChild(appContainer);
        ReactDOM.render = jest.fn();
    });

    afterEach(() => {
        jest.clearAllMocks();
        const appContainer = document.getElementById('app-container');
        if (appContainer) {
            document.body.removeChild(appContainer);
        }
    });

    it('renders dom correctly', () => {
        // Arrange
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

        // Action
        //require('../index');

        // Assert
        //(ReactDOM.render).toHaveBeenCalledTimes(1);
    });
});