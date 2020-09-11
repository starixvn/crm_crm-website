import React from 'react';
import { Button, Form, Modal } from 'react-bootstrap';
import NewNotificationTypeWrapper, { NewNotificationType } from '../NewNotificationType';
import { shallow, mount } from 'enzyme';
import MultipleSelect from '../../../../components/multiple-select/index';
import configureMockStore from 'redux-mock-store';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';
import { Formik } from 'formik';

const mockStore = configureMockStore();

describe('NewNotificationType', () => {
    beforeAll(() => {
        jest.useFakeTimers();
        LangHelper.getResource = jest.fn((key) => key);
    });
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('redux connect', () => {
        it('component has correct props', () => {
            //Arrange
            const initialState = {
                createNotificationTypeResult: {},
                settings: {},
                getSettings: {}
            };
            const store = mockStore(initialState);

            //Act
            const wrapper = shallow(<NewNotificationTypeWrapper store={store} />);

            //Assert
            expect(wrapper.props().children.props.createNotificationTypeResult).toEqual(initialState.createNotificationTypeResult);
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.getSettings).toEqual(initialState.getSettings);
        });
    });

    it('renders default component correctly', () => {
        // Arrange
        const props = {
            createNotificationTypeResult: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            },
            getSettings: {}
        };

        //Act
        const wrapper = mount(<NewNotificationType {...props} />);

        //Assert
        expect(wrapper).toMatchSnapshot();
        expect(wrapper.find(Form)).toHaveLength(1);
        expect(wrapper.find(Modal.Body)).toHaveLength(1);
        expect(wrapper.find(Modal.Footer)).toHaveLength(1);
        expect(wrapper.find(MultipleSelect)).toHaveLength(2);
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            createNotificationTypeResult: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            },
            getSettings: {},
            onClose: jest.fn()
        };

        const container = mount(<NewNotificationType {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });

    it('component render correctly message', () => {
        // Arrange
        const props = {
            createNotificationTypeResult: {
                show: true,
                isSuccessful: false,
                message: '' || 'InsertNotificationTypeSuccessfully'
            },
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            },
            getSettings: {}
        };

        const container = mount(<NewNotificationType {...props} />);

        // Action
        const responseMessage = container.find(ResponseMessage);

        // Assert
        expect(responseMessage.props().isSuccessful).toBe(props.createNotificationTypeResult.isSuccessful);
        expect(responseMessage.props().message).toBe(props.createNotificationTypeResult.message);
    });

    describe('handleSubmit', () => {
        it('handleSubmit run correctly when submit success', async () => {
            //Arrange
            const promise = Promise.resolve({});
            const secondParam = {
                setSubmitting: jest.fn()
            };
            const props = {
                createNotificationType: jest.fn(() => promise),
                createNotificationTypeResult: { isSuccessful: true, message: '' },
                onSuccess: jest.fn()
            }
            const values = {
                notificationTypeName: '',
                chatTypeIds: '',
                recipientTypeIds: '',
                systemId: ''
            }
            const container = shallow(<NewNotificationType {...props} />).instance();

            //Act
            container.handleSubmit(values, secondParam);

            await promise;
            jest.runAllTimers()

            // Assert
            expect(secondParam.setSubmitting).toHaveBeenCalledTimes(1);
            expect(props.createNotificationType).toHaveBeenCalledTimes(1);
            expect(props.onSuccess).toHaveBeenCalled();
        });

        it('handleSubmit run correctly when submit unsuccess', async () => {
            //Arrange
            const promise = Promise.resolve({});
            const secondParam = {
                setSubmitting: jest.fn()
            };
            const props = {
                createNotificationType: jest.fn(() => promise),
                createNotificationTypeResult: { isSuccessful: false, message: '' },
                onSuccess: jest.fn()
            }
            const values = {
                notificationTypeName: '',
                chatTypeIds: '',
                recipientTypeIds: '',
                systemId: ''
            }
            const container = shallow(<NewNotificationType {...props} />).instance();

            //Act
            container.handleSubmit(values, secondParam);
            await promise;
            jest.runAllTimers()

            // Assert
            expect(secondParam.setSubmitting).toHaveBeenCalledTimes(2);
            expect(props.createNotificationType).toHaveBeenCalledTimes(1);
            expect(secondParam.setSubmitting.mock.calls[0][0]).toBe(true);
            expect(secondParam.setSubmitting.mock.calls[1][0]).toBe(false);
        });
    });

    describe('handle onChange MultipleSelect', () => {
        // Arrange
        const props = {
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            }
        };
        const param = {
            values: {},
            errors: {},
            touched: {},
            handleChange: jest.fn(),
            handleSubmit: jest.fn(),
            isSubmitting: jest.fn(),
            setFieldValue: jest.fn()
        };
        const container = shallow(<NewNotificationType {...props} />);
        const form = shallow(container.instance().renderForm(param));

        it('should call correct functions when change recipientTypeIds', () => {
            //Act
            form.find(MultipleSelect).at(0).props().onChange();

            //Assert
            expect(param.setFieldValue).toHaveBeenCalled();
        });

        it('should call correct functions when change chatTypeIds', () => {
            //Act
            form.find(MultipleSelect).at(1).props().onChange();

            //Assert
            expect(param.setFieldValue).toHaveBeenCalled();
        });
    });

    it('should call correct functions when validate', () => {
        // Arrange
        const props = {
            createNotificationTypeResult: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            },
            getSettings: {},
            validateFormik: jest.fn()
        };
        const values = {};
        const container = shallow(<NewNotificationType {...props} />);
        const form = mount(container.instance().render());

        //Act
        form.find(Formik).props().validate(values);

        //Assert
        expect(props.validateFormik).toHaveBeenCalledTimes(0);
    });
});
