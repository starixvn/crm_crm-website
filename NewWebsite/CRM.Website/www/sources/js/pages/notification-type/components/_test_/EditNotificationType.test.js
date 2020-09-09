import React from 'react';
import { Button } from 'react-bootstrap';
import EditNotificationTypeWrapper, { EditNotificationType } from '../EditNotificationType';
import { shallow, mount } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import configureMockStore from 'redux-mock-store';

const mockStore = configureMockStore();

describe('EditNotificationType', () => {
    beforeAll(() => {
        LangHelper.getResource = jest.fn((key) => key);
    });
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('redux connect', () => {
        //Arrange
        const initialState = {
            editNotificationTypeResult: {},
            editNotificationTypePopup: {},
            settings: {},
            getSettings: {}
        };
        const store = mockStore(initialState);

        //Act
        const wrapper = shallow(<EditNotificationTypeWrapper store={store} />);

        //Assert
        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.editNotificationTypeResult).toEqual(initialState.editNotificationTypeResult);
            expect(wrapper.props().children.props.editNotificationTypePopup).toEqual(initialState.editNotificationTypePopup);
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.getSettings).toEqual(initialState.getSettings);
        });
    });

    it('should close popup when click close button', () => {
        // Arrange
        const props = {
            editNotificationTypeResult: {},
            editNotificationTypePopup: {
                data: {
                    recipientTypes: [],
                    chatTypes: []
                }
            },
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            },
            getSettings: {},
            onClose: jest.fn()
        };
        const container = mount(<EditNotificationType {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.onClose).toHaveBeenCalled();
    });

    it('component render correctly message', () => {
        // Arrange
        const props = {
            editNotificationTypePopup: {
                data: {
                    recipientTypes: [],
                    chatTypes: []
                }
            },
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: []
            },
            editNotificationTypeResult: {
                show: true,
                isSuccessful: false,
                message: '' || 'EditNotificationTypeSuccessfully'
            }
        };

        const container = mount(<EditNotificationType {...props} />);

        // Action
        const responseMessage = container.find(ResponseMessage);

        // Assert
        expect(responseMessage.props().isSuccessful).toBe(props.editNotificationTypeResult.isSuccessful);
        expect(responseMessage.props().message).toBe(props.editNotificationTypeResult.message);
    });

    it('componentDidMount runs correctly', async () => {
        // Arrange
        const props = {
            editNotificationTypeResult: {},
            editNotificationTypePopup: {
                data: {
                    recipientTypes: [],
                    chatTypes: []
                }
            },
            gridParams: {},
            settings: {
                recipientTypes: [],
                chatTypes: [],
                systems: [],
            },
            getSettings: jest.fn(() => Promise.resolve({ data: {} }))
        };

        const recipientTypesOptions = props.settings.recipientTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: props.editNotificationTypePopup.recipientTypes.some(y => y.recipientTypeId === x.id) ? true : false
            };
        });

        const chatTypesOptions = props.settings.chatTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });

        // Action
        const filter = shallow(<EditNotificationType {...props} />);

        // Assert
        expect(filter.instance().state.recipientTypesOptions).toEqual(recipientTypesOptions);
        expect(filter.instance().state.chatTypesOptions).toEqual(chatTypesOptions);
    });

    describe('handleSubmit', () => {
        it('handleSubmit run correctly when submit success ', () => {
            //Arrange
            const secondParam = {
                setSubmitting: jest.fn()
            };
            const props = {
                editNotificationTypePopup: {
                    data: {
                        recipientTypes: [],
                        chatTypes: []
                    }
                },
                settings: {
                    recipientTypes: [],
                    chatTypes: [],
                    systems: []
                },
                editNotificationType: jest.fn(() => Promise.resolve({})),
                editNotificationTypeResult: { isSuccesful: true }
            }
            const values = { value: 'value' }
            const container = shallow(<EditNotificationType {...props} />).instance();

            //Act
            container.handleSubmit(values, secondParam);

            // Assert
            expect(secondParam.setSubmitting).toHaveBeenCalled();
            expect(props.editNotificationType).toHaveBeenCalled();
        });
    });
});