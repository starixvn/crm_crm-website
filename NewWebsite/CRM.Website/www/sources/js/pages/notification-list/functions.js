import LangHelper from '@jsroot/common/langHelper';

export const handleNewResendOptions = (data, done) => {
    const resendList = data.filter(item => item.status === LangHelper.getResource('Error'));
    const dataList = data.map(item => {
        return {
            notificationId: item.notificationId,
            messageId: item.messageId,
            status: item.status,
            isResent: false
        }
    });

    done({
        isCheckedAll: false,
        isDisabledResend: true,
        isHaveResendAll: resendList.length > 0,
        dataList
    });
};

const functions = {
    handleNewResendOptions
}

export default functions;