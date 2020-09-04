import React from 'react';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { NewNotificationType } from '../NewNotificationType';
import { shallow, mount } from 'enzyme';
import MultipleSelect from '../../../../components/multiple-select/index';
import configureMockStore from 'redux-mock-store';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import { Formik } from 'formik';
import LangHelper from '@jsroot/common/langHelper';

const mockStore = configureMockStore();
const store = mockStore({});

describe('NewNotificationType', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<NewNotificationType />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('render component correctly', () => {
        const props = {
            createNotificationTypeResult: {},
            settings: {},
            getSettings: {}
        };

        const container = shallow(<NewNotificationType {...props} />);

        expect(container).toMatchSnapshot();
    });

    it('renders Modal, Formik components', () => {
        const wrapper = shallow(<NewNotificationType />);

        expect(wrapper.find(Modal)).toHaveLength(1);
        expect(wrapper.find(Formik)).toHaveLength(1);
    });

    it('renders MultipleSelect components', () => {
        const props = {
            createNotificationTypeResult: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems:[]
            },
            getSettings: {}
        };

        const wrapper = mount(<NewNotificationType {...props} />);

        expect(wrapper.find(MultipleSelect)).toHaveLength(2);
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            createNotificationTypeResult: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems:[]
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

    it('should submit when click save button', () =>{
        //Arrange
        const props = {
            createNotificationTypeResult: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems:[]
            },
            getSettings: {},
            onSubmit: jest.fn()
        };

        const container = mount(<NewNotificationType {...props}/>);

        //Act
        container.find(Form).simulate('submit');

        //Assert
        expect(props.onSubmit).toHaveBeenCalledTimes(0);
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
                systems:[]
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
});
