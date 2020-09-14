import React from 'react';
import { Form, Button } from 'react-bootstrap';
import EditNotificationTypeWrapper, { EditNotificationType } from '../EditNotificationType';
import { shallow, mount } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import configureMockStore from 'redux-mock-store';
import MultipleSelect from '../../../../components/multiple-select/index';
import { Formik } from 'formik';
import functions from '../../common.functions';

const mockStore = configureMockStore();

describe('EditNotificationType', () => {
    beforeAll(() => {
        jest.useFakeTimers();
        LangHelper.getResource = jest.fn((key) => key);
    });
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('redux connect', () => {
        // Arrange
        const initialState = {
            editNotificationTypeResult: {},
            editNotificationTypePopup: {},
            settings: {},
            getSettings: {}
        };
        const store = mockStore(initialState);

        // Act
        const wrapper = shallow(<EditNotificationTypeWrapper store={store} />);

        // Assert
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

    it('component render message correctly', () => {
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
                    notificationTypeId: 1,
                    recipientTypes: [
                        {
                            recipientTypeId: 1
                        }
                    ],
                    chatTypes: [
                        {
                            chatTypeId: 3
                        }
                    ]
                }

            },
            gridParams: {},
            settings: {
                recipientTypes: [
                    {
                        id: 1,
                        name: 'recipient 1'
                    },
                    {
                        id: 2,
                        name: 'recipient 2'
                    }
                ],
                chatTypes: [
                    {
                        id: 3,
                        name: 'chat 1'
                    },
                    {
                        id: 4,
                        name: 'chat 2'
                    }
                ],
                systems: [],
            }
        };
        const recipientTypesOptions = props.settings.recipientTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: props.editNotificationTypePopup.data.recipientTypes.some(y => y.recipientTypeId === x.id) ? true : false
            };
        });
        const chatTypesOptions = props.settings.chatTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: props.editNotificationTypePopup.data.chatTypes.some(y => y.chatTypeId === x.id) ? true : false
            };
        });

        // Action
        const container = shallow(<EditNotificationType {...props} />);
        container.instance().componentDidMount();

        // Assert
        expect(container.instance().state.recipientTypesOptions).toEqual(recipientTypesOptions);
        expect(container.instance().state.chatTypesOptions).toEqual(chatTypesOptions);
    });

    describe('handleSubmit', () => {
        it('handleSubmit run correctly when submit success', async () => {
            // Arrange
            const promise = Promise.resolve({});
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
                editNotificationType: jest.fn(() => promise),
                editNotificationTypeResult: { isSuccessful: true, message: '' },
                onSuccess: jest.fn()
            }
            const values = {
                notificationTypeName: '',
                chatTypeIds: '',
                recipientTypeIds: '',
                systemId: ''
            }
            const container = shallow(<EditNotificationType {...props} />).instance();

            // Act
            container.handleSubmit(values, secondParam);
            await promise;
            jest.runAllTimers()

            // Assert
            expect(secondParam.setSubmitting).toHaveBeenCalledTimes(1);
            expect(props.editNotificationType).toHaveBeenCalledTimes(1);
            expect(props.onSuccess).toHaveBeenCalled();
        });

        it('handleSubmit run correctly when submit unsuccess', async () => {
            // Arrange
            const promise = Promise.resolve({});
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
                editNotificationType: jest.fn(() => promise),
                editNotificationTypeResult: { isSuccessful: false, message: '' },
                onSuccess: jest.fn()
            }
            const values = {
                notificationTypeName: '',
                chatTypeIds: '',
                recipientTypeIds: '',
                systemId: ''
            }
            const container = shallow(<EditNotificationType {...props} />).instance();

            // Act
            container.handleSubmit(values, secondParam);
            await promise;
            jest.runAllTimers()

            // Assert
            expect(secondParam.setSubmitting).toHaveBeenCalledTimes(2);
            expect(props.editNotificationType).toHaveBeenCalledTimes(1);
            expect(secondParam.setSubmitting.mock.calls[0][0]).toBe(true);
            expect(secondParam.setSubmitting.mock.calls[1][0]).toBe(false);
        });
    });

    describe('handle onChange MultipleSelect', () => {
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
        const container = shallow(<EditNotificationType {...props} />);
        const form = shallow(container.instance().renderForm(param));

        it('should call correct functions when change recipientTypeIds', () => {
            //Act
            form.find(MultipleSelect).at(0).props().onChange();
            //Assert
            expect(param.setFieldValue).toHaveBeenCalled();
        });

        it('should call correct functions when change chatTypeIds', () => {
            // Act
            form.find(MultipleSelect).at(1).props().onChange();

            // Assert
            expect(param.setFieldValue).toHaveBeenCalled();
        });
    });

    describe('validate', () => {
        it('should call correct functions when validate', () => {
            // Arrange
            const promise = Promise.resolve({});
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
                }
            };
            const values = {};
            const container = shallow(<EditNotificationType {...props} />);
            functions.validateFormik = jest.fn(() => promise);

            // Act
            container.find(Formik).props().validate(values);

            // Assert
            promise.then(() => {
                expect(functions.validateFormik).toHaveBeenCalled(1);
            });
        });
    });

    describe('initialValues', () => {
        it('initialValues correctly', () => {
            // Arrange
            const props = {
                editNotificationTypePopup: {
                    data: {
                        notificationTypeId: 1,
                        notificationTypeName: 'test',
                        systemId: 1,
                        recipientTypes: [
                            {
                                recipientTypeId: 1
                            }
                        ],
                        chatTypes: [
                            {
                                chatTypeId: 1
                            }
                        ]
                    }
                },
                settings: {
                    recipientTypes: [],
                    chatTypes: [],
                    systems: []
                }
            };
            const values = {
                notificationTypeId: props.editNotificationTypePopup.data.notificationTypeId,
                notificationTypeName: props.editNotificationTypePopup.data.notificationTypeName,
                recipientTypeIds: props.editNotificationTypePopup.data.recipientTypes.map(x => x.recipientTypeId).toString(),
                chatTypeIds: props.editNotificationTypePopup.data.chatTypes.map(x => x.chatTypeId).toString(),
                systemId: props.editNotificationTypePopup.data.systemId
            };
            const container = shallow(<EditNotificationType {...props} />);

            // Act
            const form = mount(container.instance().render());

            // Assert
            expect(form.find(Formik).props().initialValues).toEqual(values);
        });
    });
});

describe('isInvalid', () => {
    it('isInvalid Notification Type Name correctly', () => {
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
            }
        };
        const param = {
            values: {},
            errors: {
                notificationTypeName: 'test'
            },
            touched: {
                notificationTypeName: true
            },
            handleChange: jest.fn(),
            handleSubmit: jest.fn(),
            isSubmitting: jest.fn(),
            setFieldValue: jest.fn(),
            isInvalid: jest.fn()
        };
        const container = shallow(<EditNotificationType {...props} />);

        // Act
        const form = shallow(container.instance().renderForm(param));

        // Assert
        expect(form.find(Form.Control).at(0).props().isInvalid).toEqual(param.errors.notificationTypeName);
    });

    it('isInvalid Recipient Type correctly', () => {
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
            }
        };
        const param = {
            values: {},
            errors: {
                recipientTypeIds: 'test'
            },
            touched: {
                recipientTypeIds: true
            },
            handleChange: jest.fn(),
            handleSubmit: jest.fn(),
            isSubmitting: jest.fn(),
            setFieldValue: jest.fn(),
            isInvalid: jest.fn()
        };
        const container = shallow(<EditNotificationType {...props} />);

        // Act
        const form = shallow(container.instance().renderForm(param));

        // Assert
        expect(form.find(MultipleSelect).at(0).props().isInvalid).toEqual(param.errors.recipientTypeIds);
    });

    it('isInvalid Chat Type correctly', () => {
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
            }
        };
        const param = {
            values: {},
            errors: {
                chatTypeIds: 'test'
            },
            touched: {
                chatTypeIds: true
            },
            handleChange: jest.fn(),
            handleSubmit: jest.fn(),
            isSubmitting: jest.fn(),
            setFieldValue: jest.fn(),
            isInvalid: jest.fn()
        };
        const container = shallow(<EditNotificationType {...props} />);

        // Act
        const form = shallow(container.instance().renderForm(param));

        // Assert
        expect(form.find(MultipleSelect).at(1).props().isInvalid).toEqual(param.errors.chatTypeIds);
    });

    it('isInvalid System correctly', () => {
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
            }
        };
        const param = {
            values: {},
            errors: {
                system: 'test'
            },
            touched: {
                system: true
            },
            handleChange: jest.fn(),
            handleSubmit: jest.fn(),
            isSubmitting: jest.fn(),
            setFieldValue: jest.fn(),
            isInvalid: jest.fn()
        };
        const container = shallow(<EditNotificationType {...props} />);

        // Act
        const form = shallow(container.instance().renderForm(param));

        // Assert
        expect(form.find(Form.Control).at(1).props().isInvalid).toEqual(param.errors.system);
    });
});

