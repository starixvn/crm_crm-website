import React from 'react';
import { Button } from 'react-bootstrap';
import { EditNotificationTemplate } from '../EditNotificationTemplate';
import { shallow, mount } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';

describe('EditNotificationTemplate', () => {
    beforeAll(() => {
        jest.useFakeTimers();
        LangHelper.getResource = jest.fn((key) => key);
    });

    afterAll(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<EditNotificationTemplate />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            onClose: jest.fn()
        };
        const container = mount(<EditNotificationTemplate {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });

    describe('methods', () => {
        describe('handleSubmit', () => {
            it('should call exactly functions when edit successful', async () => {
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
                    editTemplate: jest.fn(() => promise),
                    onSuccess: jest.fn(),
                    editTemplateResult: { isSuccessful: true, message: '' }
                };
                const container = shallow(<EditNotificationTemplate {...props} />).instance();

                // Act
                container.handleSubmit(values, formikBag);
                await promise;
                jest.runAllTimers();

                // Assert
                expect(formikBag.setSubmitting.mock.calls.length).toBe(1);
                expect(formikBag.setSubmitting.mock.calls[0][0]).toBe(true);

                expect(props.editTemplate).toHaveBeenCalledWith({
                    notificationTypeId: 1,
                    notificationTemplateName: '',
                    title: '',
                    contents: [{ languageId: 1, content: 'en' }]
                });
                expect(props.onSuccess).toHaveBeenCalled();
            });

            it('should call exactly functions when edit unsuccessful', async () => {
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
                    editTemplate: jest.fn(() => promise),
                    onSuccess: jest.fn(),
                    editTemplateResult: { isSuccessful: false, message: '' }
                };
                const container = shallow(<EditNotificationTemplate {...props} />).instance();

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