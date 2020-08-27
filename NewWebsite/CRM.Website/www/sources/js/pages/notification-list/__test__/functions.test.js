import LangHelper from '@jsroot/common/langHelper';
import { handleNewResendOptions } from '../functions';

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
            }
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
});