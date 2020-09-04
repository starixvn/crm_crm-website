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
            gridParams: {
                pageIndex: 1,
                pageSize: 100
            },
            invalidParams: {
                show: false
            },
            contentPopup: { show: false },
            viewDetailPopup: { show: false },
            resendOptions: {
                isCheckedAll: false,
                isDisabledResend: true,
                isHaveResendAll: false,
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
            messageResult: {
                isSuccessful: false,
                show: false,
                message: ''
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

        // Action
        require('../index');

        // Assert
        expect(ReactDOM.render).toHaveBeenCalledTimes(1);
    });
});