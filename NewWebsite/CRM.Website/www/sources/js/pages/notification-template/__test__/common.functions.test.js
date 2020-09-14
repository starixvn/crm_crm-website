import React from 'react';
import LangHelper from '@jsroot/common/langHelper';
import * as commonFuns from '../common.functions';

describe('common functions', () => {
    beforeAll(() => {
        LangHelper.getResource = jest.fn((key) => key);
    });

    afterAll(() => {
        jest.clearAllMocks();
    });

    describe('renderNotificationTypeOption', () => {
        it('should render option correctly', () => {
            // Arrange
            const notificationType = {
                notificationTypeId: 1,
                notificationTypeName: 'void abnormal bet'
            };

            // Act
            const option = commonFuns.renderNotificationTypeOption(notificationType);

            // Assert
            expect(option).toEqual(<option key={1} value={1}>void abnormal bet</option>);
        });
    });

    describe('validate', () => {
        it('return expected errors when values are invalid', () => {
            // Arrange
            const values = {};
            const expected = {
                notificationTemplateName: 'TemplateNameRequired',
                notificationTypeId: 'NotificationTypeRequired',
                title: 'TitleRequired'
            };

            // Act
            const errors = commonFuns.validateFormik(values);

            // Assert
            expect(errors).toEqual(expected);
        });

        it('return expected errors when notificationTemplateName is empty', () => {
            // Arrange
            const values = { notificationTypeId: 1, title: 'title', notificationTemplateName: '' };
            const expected = {
                notificationTemplateName: 'TemplateNameRequired'
            };

            // Act
            const errors = commonFuns.validateFormik(values);

            // Assert
            expect(errors).toEqual(expected);
        });

        it('return expected errors when notificationTemplateName is over 1000 characters', () => {
            // Arrange
            let name = '';
            for (let i = 0; i < 1001; i++) {
                name += 'a';
            }
            const values = { notificationTemplateName: name, notificationTypeId: 1, title: 'title' };
            const expected = {
                notificationTemplateName: 'over characters'
            };
            LangHelper.formatString = jest.fn(() => 'over characters');

            // Act
            const errors = commonFuns.validateFormik(values);

            // Assert
            expect(errors).toEqual(expected);
            expect(LangHelper.formatString).toHaveBeenCalledWith('FieldMaxLength', 'TemplateName', 1000);
        });

        it('return expected errors when title is empty', () => {
            // Arrange
            const values = { notificationTypeId: 1, notificationTemplateName: 'void abnormal bet', title: '' };
            const expected = {
                title: 'TitleRequired'
            };

            // Act
            const errors = commonFuns.validateFormik(values);

            // Assert
            expect(errors).toEqual(expected);
        });

        it('return expected errors when title is over 2000 characters', () => {
            // Arrange
            let title = '';
            for (let i = 0; i < 2001; i++) {
                title += 'a';
            }
            const values = { notificationTemplateName: 'void abnormal bet', notificationTypeId: 1, title: title };
            const expected = {
                title: 'over characters'
            };
            LangHelper.formatString = jest.fn(() => 'over characters');

            // Act
            const errors = commonFuns.validateFormik(values);

            // Assert
            expect(errors).toEqual(expected);
            expect(LangHelper.formatString).toHaveBeenCalledWith('FieldMaxLength', 'Title', 2000);
        });

        it('return expected errors when contents are invalid', () => {
            // Arrange
            const values = {
                notificationTemplateName: 'void abnormal bet',
                notificationTypeId: 1,
                title: 'void abnormal bet'
            };
            const expected = {
                language_1: 'required'
            };
            const languages = [{ settingId: 1, settingValue: 'EN' }];
            LangHelper.formatString = jest.fn(() => 'required');

            // Act
            const errors = commonFuns.validateFormik(values, languages);

            // Assert
            expect(errors).toEqual(expected);
            expect(LangHelper.formatString).toHaveBeenCalledWith('FieldRequired', 'Content (EN)');
        });
    });
});