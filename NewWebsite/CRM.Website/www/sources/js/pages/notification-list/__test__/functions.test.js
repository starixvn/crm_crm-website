import LangHelper from '@jsroot/common/langHelper';
import { handleNewResendOptions, getHeadlines, getRecipients } from '../functions';
import React from 'react';

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
        function getRecipients_Test(recipients, expected) {
            it('getRecipients_Test', () => {
                // Action
                const result = getRecipients(recipients);
        
                // Assert
                expect(result).toEqual(expected);
            });
        }
    
        getRecipients_Test(null, '');
        getRecipients_Test(undefined, '');
        getRecipients_Test([], '');
        getRecipients_Test(recipients, recipients.map((recipient, index) => <div key={index}>{`${recipient.recipientName} (${recipient.roleName})`}</div>));
    });
});