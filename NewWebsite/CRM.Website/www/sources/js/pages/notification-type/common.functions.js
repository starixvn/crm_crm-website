import React from 'react';
import LangHelper from '@jsroot/common/langHelper';

export const renderSystemOption = item => {
    return (
        <option key={item.id} value={item.id} >{item.name}</option>
    );
};

export const validateFormik = (values) => {
    const errors = {}

    if (!values.notificationTypeName || values.notificationTypeName.trim().length === 0) {
        errors.notificationTypeName = LangHelper.getResource('NotificationTypeNameRequired');
    } else {
        if (values.notificationTypeName.trim().length > 500) {
            errors.notificationTypeName = LangHelper.formatString(
                LangHelper.getResource('FieldMaxLength'),
                LangHelper.getResource('NotificationTypeName'),
                500);
        }
    }

    if (!values.recipientTypeIds) {
        errors.recipientTypeIds = LangHelper.getResource('RecipientTypeRequired');
    }

    if (!values.chatTypeIds) {
        errors.chatTypeIds = LangHelper.getResource('ChatTypeRequired');
    }

    if (!values.systemId) {
        errors.systemId = LangHelper.getResource('SystemRequired');
    }

    return errors;
};