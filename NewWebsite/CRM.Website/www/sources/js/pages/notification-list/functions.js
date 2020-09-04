import LangHelper from '@jsroot/common/langHelper';
import React from 'react';

export const handleNewResendOptions = (data, done) => {
    const resendList = data.filter(item => item.status === LangHelper.getResource('Error'));
    const dataList = data.map(item => {
        return {
            notificationId: item.notificationId,
            status: item.status,
            isResent: false
        };
    });

    done({
        isCheckedAll: false,
        isDisabledResend: true,
        isHaveResendAll: resendList.length > 0,
        dataList
    });
};

export const getHeadlines = headlines => {
    if (headlines && headlines.length > 0) {
        return headlines.map(headline => headline.headlineName).join(', ');
    }

    return '';
};

export const getRecipients = recipients => {
    if (recipients && recipients.length > 0) {
        return recipients.map((recipient, index) => <div key={index}>{`${recipient.recipientName} (${recipient.roleName})`}</div>);
    }

    return '';
};

const functions = {
    handleNewResendOptions,
    getHeadlines,
    getRecipients
};

export default functions;