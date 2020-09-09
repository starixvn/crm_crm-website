import React from 'react';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import NewNotificationTypeWrapper, { NewNotificationType } from '../NewNotificationType';
import { shallow, mount } from 'enzyme';
import MultipleSelect from '../../../../components/multiple-select/index';
import configureMockStore from 'redux-mock-store';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import { Formik } from 'formik';
import LangHelper from '@jsroot/common/langHelper';

const mockStore = configureMockStore();

describe('NewNotificationType', () => {
    beforeAll(() => {
        LangHelper.getResource = jest.fn((key) => key);
    });
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('redux connect', () => {
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
        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.createNotificationTypeResult).toEqual(initialState.createNotificationTypeResult);
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.getSettings).toEqual(initialState.getSettings);
        });
    });

    it('renders Modal, Formik components', () => {
        //Act
        const wrapper = shallow(<NewNotificationType />);

        //Assert
        expect(wrapper.find(Modal)).toHaveLength(1);
        expect(wrapper.find(Formik)).toHaveLength(1);
    });

    it('renders MultipleSelect components', () => {
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
        it('handleSubmit run correctly when submit success', () => {
            //Arrange
            const secondParam = {
                setSubmitting: jest.fn()
            };
            const props = {
                createNotificationType: jest.fn(() => Promise.resolve())
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

            // Assert
            expect(secondParam.setSubmitting).toHaveBeenCalledTimes(1);
            expect(props.createNotificationType).toHaveBeenCalledTimes(1);
        });
    });
});
