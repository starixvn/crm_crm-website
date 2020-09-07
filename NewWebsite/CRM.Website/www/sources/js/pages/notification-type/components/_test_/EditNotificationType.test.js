import React from 'react';
import { Button } from 'react-bootstrap';
import { EditNotificationType } from '../EditNotificationType';
import { shallow, mount } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import ResponseMessage from '@jsroot/components/ResponseMessage';

describe('EditNotificationType', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        //Arrange
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
                chatTypes: []
            },
            getSettings: {},
        };

        // Act
        const container = shallow(<EditNotificationType {...props} />);

        // Assert
        expect(container).toMatchSnapshot();
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
                message: '' || LangHelper.getResource('EditNotificationTypeSuccessfully')
            }
        };

        const container = mount(<EditNotificationType {...props} />);

        // Action
        const responseMessage = container.find(ResponseMessage);

        // Assert
        expect(responseMessage.props().isSuccessful).toBe(props.editNotificationTypeResult.isSuccessful);
        expect(responseMessage.props().message).toBe(props.editNotificationTypeResult.message);
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
                getSettings: {}
            };

            const filter = shallow(<EditNotificationType {...props} />);

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
                getSettings: {}
            };

            const filter = shallow(<EditNotificationType {...props} />);

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
                getSettings: {}
            };

            const filter = shallow(<EditNotificationType {...props} />);

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
                getSettings: {}
            };

            const filter = shallow(<EditNotificationType {...props} />);

            // Action
            const result = filter.instance().validate(values);

            // Assert
            expect(result).toBeFalse;
            expect(result.chatTypeIds).toBe(`${LangHelper.getResource('ChatTypeRequired')}`);
        });
    });
});