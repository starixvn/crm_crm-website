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

    describe('renderSystemOption', () => {
        it('should render option correctly', () => {
            // Arrange
            const systems = {
                id: 1,
                name: 'system 1'
            };

            // Act
            const option = commonFuns.renderSystemOption(systems);

            // Assert
            expect(option).toEqual(<option key={1} value={1}>system 1</option>);
        });
    });

    describe('validate', () => {
        it('validate returns true', () => {
            // Arrange
            const values = {
                notificationTypeName: 'notificationTypeName',
                recipientTypeIds: '1',
                chatTypeIds: '1',
                systemId: '1'
            };

            // Action
            const result = commonFuns.validateFormik(values);

            // Assert
            expect(result).toBeTrue;
        });

        it('invalid notificationTypeName', () => {
            // Arrange
            const values = {
                notificationTypeName: '',
                recipientTypeIds: '1',
                chatTypeIds: '1',
                systemId: '1'
            };
            const expected = {
                notificationTypeName: 'NotificationTypeNameRequired'
            };

            // Action
            const result = commonFuns.validateFormik(values);

            // Assert
            expect(result).toBeFalse;
            expect(result).toEqual(expected);
        });

        it('invalid recipientTypeIds', () => {
            // Arrange
            const values = {
                notificationTypeName: 'test',
                recipientTypeIds: '',
                chatTypeIds: '1',
                systemId: '1'
            };
            const expected = {
                recipientTypeIds: 'RecipientTypeRequired'
            };

            // Action
            const result = commonFuns.validateFormik(values);

            // Assert
            expect(result).toBeFalse;
            expect(result).toEqual(expected);
        });

        it('invalid chatTypeIds', () => {
            // Arrange
            const values = {
                notificationTypeName: 'test',
                recipientTypeIds: '1',
                chatTypeIds: '',
                systemId: '1'
            };
            const expected = {
                chatTypeIds: 'ChatTypeRequired'
            };

            // Action
            const result = commonFuns.validateFormik(values);

            // Assert
            expect(result).toBeFalse;
            expect(result).toEqual(expected);
        });

        it('invalid systemId', () => {
            // Arrange
            const values = {
                notificationTypeName: 'test',
                recipientTypeIds: '1',
                chatTypeIds: '1',
                systemId: ''
            };
            const expected = {
                systemId: 'SystemRequired'
            };

            // Action
            const result = commonFuns.validateFormik(values);

            // Assert
            expect(result).toBeFalse;
            expect(result).toEqual(expected);
        });

        it('return expected errors when notificationTypeName is over 500 characters', () => {
            // Arrange
            let name = '';
            for (let i = 0; i < 501; i++) {
                name += 'a';
            }
            const values = { notificationTypeName: name };
            const expected = {
                notificationTypeName: 'over characters'
            };
            LangHelper.formatString = jest.fn(() => 'over characters');

            // Act
            const errors = commonFuns.validateFormik(values);

            // Assert
            expect(errors.notificationTypeName).toEqual(expected.notificationTypeName);

            expect(LangHelper.formatString).toHaveBeenCalledWith('FieldMaxLength', 'NotificationTypeName', 500);
        });
    });
});