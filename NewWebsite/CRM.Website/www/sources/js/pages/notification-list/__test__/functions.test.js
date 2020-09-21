import LangHelper from '@jsroot/common/langHelper';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
import { handleNewResendOptions, getHeadlines, getRecipients, resendNotifications } from '../functions';
import React from 'react';
import { toast } from 'react-toastify';

describe('function', () => {
    it('handleNewResendOptions should be call callback function with correct params', () => {
        const expectedData = [ { notificationId: 1, status: 'Error'} ];
        const expectedCallBack = jest.fn();
        const resendList = expectedData.filter(item => item.status === LangHelper.getResource('Error'));
        const dataList = expectedData.map(item => {
            return {
                notificationId: item.notificationId,
                status: item.status,
                isResent: false
            };
        });

        const expectedParams = {
            isCheckedAll: false,
            isDisabledResend: true,
            isHaveResendAll: resendList.length > 0,
            dataList
        };

        handleNewResendOptions(expectedData, expectedCallBack);

        expect(expectedCallBack).toHaveBeenCalledWith(expectedParams);
    });
    
    describe('getHeadlines', () => {
        function getHeadlines_Test(headlines, expected) {
            it('getHeadlines_Test', () => {
                // Action
                const result = getHeadlines(headlines);
        
                // Assert
                expect(result).toEqual(expected);
            });
        }
    
        getHeadlines_Test(null, '');
        getHeadlines_Test(undefined, '');
        getHeadlines_Test([], '');
        getHeadlines_Test([ { headlineName: 'name1' }, { headlineName: 'name2' } ], 'name1, name2');
    });
    
    describe('getRecipients', () => {
        const recipients = [ { recipientName: 'recipient1', roleName: 'role1' }, { recipientName: 'recipient2', roleName: 'role2' } ];
        function getRecipientsTest(recipients, expected) {
            it('getRecipientsTest', () => {
                // Action
                const result = getRecipients(recipients);
        
                // Assert
                expect(result).toEqual(expected);
            });
        }
    
        getRecipientsTest(null, '');
        getRecipientsTest(undefined, '');
        getRecipientsTest([], '');
        getRecipientsTest(recipients, recipients.map(recipient => `${recipient.recipientName} (${recipient.roleName})`).join(', '));
    });

    describe('resendNotifications', () => {
        it('post async', async () => {
            // Arrange
            const dataList = [1, 2];
            const response = { data: {} };
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Action
            await resendNotifications(dataList);

            // Assert
            promise.then(() => {
                expect(DataService.postDataAsync).toHaveBeenCalledWith('/NotificationList/ResendNotificationList', dataList);
            });
        });
        
        it('calls function callback', async () => {
            // Arrange
            const dataList = [1, 2];
            const response = { data: {} };
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const callback = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);

            // Action
            await resendNotifications(dataList, callback);

            // Assert
            promise.then(() => {
                expect(callback).toHaveBeenCalled();
            });
        });
        
        it('response is successfull', async () => {
            // Arrange
            const dataList = [1, 2];
            const response = {
                data:
                {
                    message: 'message',
                    isSuccessful: true
                }
            };
            
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const callback = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);
            toast.success = jest.fn();

            // Action
            await resendNotifications(dataList, callback);

            // Assert
            promise.then(() => {
                expect(toast.success).toHaveBeenCalledWith(response.message);
            });
        });
        
        it('response is successfull', async () => {
            // Arrange
            const dataList = [1, 2];
            const response = { data: {
                message: 'message',
                isSuccessful: false
            } };
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            const callback = jest.fn();
            const promise = Promise.resolve(response);
            DataService.postDataAsync = jest.fn(() => promise);
            toast.error = jest.fn();

            // Action
            await resendNotifications(dataList, callback);

            // Assert
            promise.then(() => {
                expect(toast.error).toHaveBeenCalledWith(response.message);
            });
        });
    });
});