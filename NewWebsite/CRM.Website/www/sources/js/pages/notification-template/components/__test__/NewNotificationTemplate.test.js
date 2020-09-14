import React from 'react';
import { Formik } from 'formik';
import { NewNotificationTemplate } from '../NewNotificationTemplate';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import * as commonFunctions from '../../common.functions';

describe('NewNotificationTemplate', () => {
    beforeAll(() => {
        jest.useFakeTimers();
        LangHelper.getResource = jest.fn((key) => key);
    });

    afterAll(() => {
        jest.clearAllMocks();
    });

    it('should render correctly', () => {
        // Act
        const container = shallow(<NewNotificationTemplate />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    describe('methods', () => {
        describe('handleSubmit', () => {
            it('should call exactly functions when create successful', async () => {
                // Arrange
                const promise = Promise.resolve({});
                const values = {
                    notificationTypeId: 1,
                    notificationTemplateName: '',
                    title: '',
                    language_1: 'en'
                };
                const formikBag = {
                    setSubmitting: jest.fn()
                };
                const props = {
                    languages: [{
                        settingId: 1
                    }],
                    createTemplate: jest.fn(() => promise),
                    onSuccess: jest.fn(),
                    createTemplateResult: { isSuccessful: true, message: '' }
                };
                const container = shallow(<NewNotificationTemplate {...props} />).instance();

                // Act
                container.handleSubmit(values, formikBag);
                await promise;
                jest.runAllTimers();

                // Assert
                expect(formikBag.setSubmitting.mock.calls.length).toBe(1);
                expect(formikBag.setSubmitting.mock.calls[0][0]).toBe(true);

                expect(props.createTemplate).toHaveBeenCalledWith({
                    notificationTypeId: 1,
                    notificationTemplateName: '',
                    title: '',
                    contents: [{ languageId: 1, content: 'en' }]
                });
                expect(props.onSuccess).toHaveBeenCalled();
            });

            it('should call exactly functions when create unsuccessful', async () => {
                // Arrange
                const promise = Promise.resolve({});
                const values = {
                    notificationTypeId: 1,
                    notificationTemplateName: '',
                    title: ''
                };
                const formikBag = {
                    setSubmitting: jest.fn()
                };
                const props = {
                    languages: [],
                    createTemplate: jest.fn(() => promise),
                    onSuccess: jest.fn(),
                    createTemplateResult: { isSuccessful: false, message: '' }
                };
                const container = shallow(<NewNotificationTemplate {...props} />).instance();

                // Act
                container.handleSubmit(values, formikBag);
                await promise;
                jest.runAllTimers();

                // Assert
                expect(formikBag.setSubmitting.mock.calls.length).toBe(2);
                expect(formikBag.setSubmitting.mock.calls[0][0]).toBe(true);
                expect(formikBag.setSubmitting.mock.calls[1][0]).toBe(false);
            });
        });

        describe('renderForm', () => {
            const formikProps = {
                values: {},
                errors: {},
                touched: {}
            };

            it('should render correctly when languages do not have data', () => {
                // Arrange
                const container = shallow(<NewNotificationTemplate />).instance();

                // Act
                const form = shallow(container.renderForm(formikProps));

                // Assert
                expect(form).toMatchSnapshot();
            });

            it('should render correctly when languages have data', () => {
                // Arrange
                const props = {
                    languages: [{ settingId: 1, settingValue: 'EN' }]
                };
                const container = shallow(<NewNotificationTemplate {...props} />).instance();

                // Act
                const form = shallow(container.renderForm(formikProps));

                // Assert
                expect(form).toMatchSnapshot();
            });

            it('should show response message', () => {
                // Arrange
                const props = {
                    createTemplateResult: { isSuccessful: false, message: 'unsuccessful' }
                };
                const container = shallow(<NewNotificationTemplate {...props} />).instance();

                // Act
                const form = shallow(container.renderForm(formikProps));

                // Assert
                expect(form).toMatchSnapshot();
            });
        });
    });
});