import React from 'react';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { NewNotificationType } from '../NewNotificationType';
import { shallow, mount } from 'enzyme';
import MultipleSelect from '../../../../components/multiple-select/index';
import configureMockStore from 'redux-mock-store';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import { Formik } from 'formik';
import LangHelper from '@jsroot/common/langHelper';

const mockStore = configureMockStore();
const store = mockStore({});

describe('NewNotificationType', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('render component correctly', () => {
        // Arrange
        const props = {
            createNotificationTypeResult: {},
            settings: {},
            getSettings: {}
        };

        //Act
        const container = shallow(<NewNotificationType {...props} />);

        //Asert
        expect(container).toMatchSnapshot();
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
                message: '' || LangHelper.getResource('InsertNotificationTypeSuccessfully')
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

    describe('validate', () => {
        it('validate returns true', () => {
            // Arrange
            const values = {
                notificationTypeName: 'notificationTypeName',
                recipientTypeIds: '1',
                chatTypeIds: '1',
                systemId: '1'
            };

            const filter = shallow(<NewNotificationType />);

            // Action
            const result = filter.instance().validate(values);

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

            const filter = shallow(<NewNotificationType />);

            // Action
            const result = filter.instance().validate(values);

            // Assert
            expect(result).toBeFalse;
            expect(result.notificationTypeName).toBe(`${LangHelper.getResource('NotificationTypeNameRequired')}`);
        });

        it('invalid recipientTypeIds', () => {
            // Arrange
            const values = {
                notificationTypeName: 'test',
                recipientTypeIds: '',
                chatTypeIds: '1',
                systemId: '1'
            };

            const filter = shallow(<NewNotificationType />);

            // Action
            const result = filter.instance().validate(values);

            // Assert
            expect(result).toBeFalse;
            expect(result.recipientTypeIds).toBe(`${LangHelper.getResource('RecipientTypeRequired')}`);
        });

        it('invalid chatTypeIds', () => {
            // Arrange
            const values = {
                notificationTypeName: 'test',
                recipientTypeIds: '1',
                chatTypeIds: '',
                systemId: '1'
            };

            const filter = shallow(<NewNotificationType />);

            // Action
            const result = filter.instance().validate(values);

            // Assert
            expect(result).toBeFalse;
            expect(result.chatTypeIds).toBe(`${LangHelper.getResource('ChatTypeRequired')}`);
        });

        it('invalid systemId', () => {
            // Arrange
            const values = {
                notificationTypeName: 'test',
                recipientTypeIds: '1',
                chatTypeIds: '1',
                systemId: ''
            };

            const filter = shallow(<NewNotificationType />);

            // Action
            const result = filter.instance().validate(values);

            // Assert
            expect(result).toBeFalse;
            expect(result.systemId).toBe(`${LangHelper.getResource('SystemRequired')}`);
        });
    });
});
