import LangHelper from '@jsroot/common/langHelper';
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';
import React from 'react';
import { toast } from 'react-toastify';

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
        return recipients.map(recipient => `${recipient.recipientName} (${recipient.roleName})`).join(', ');
    }

    return '';
};

export const resendNotifications = async (dataList, callback) => {
    Site.blockUI();
    const url = Site.resolveClientUrl('NotificationList/ResendNotificationList');
    await DataService.postDataAsync(url, dataList).then(response => {
        const { message, isSuccessful } = response.data;
        if (typeof callback === 'function') {
            callback();
        }

        if (isSuccessful) {
            toast.success(message);
        }
        else {
            toast.error(message);
        }
    }).then(() => Site.unblockUI());
};

const functions = {
    handleNewResendOptions,
    getHeadlines,
    getRecipients,
    resendNotifications
};

export default functions;