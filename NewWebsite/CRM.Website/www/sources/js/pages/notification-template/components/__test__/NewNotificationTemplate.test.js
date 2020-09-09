import React from 'react';
import { Button } from 'react-bootstrap';
import { NewNotificationTemplate } from '../NewNotificationTemplate';
import { shallow, mount } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';

describe('NewNotificationTemplate', () => {
    beforeAll(() => {
        jest.useFakeTimers();
        LangHelper.getResource = jest.fn((key) => key);
    });

    afterAll(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<NewNotificationTemplate />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            onClose: jest.fn()
        };
        const container = mount(<NewNotificationTemplate {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
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
    });
});