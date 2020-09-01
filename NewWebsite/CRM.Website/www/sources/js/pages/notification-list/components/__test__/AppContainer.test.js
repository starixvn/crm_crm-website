import React from 'react';
import AppContainerWrapper, { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import Content from '../Content';
import ViewDetail from '../ViewDetail';
import ValidateResend from '../ValidateResend';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';

describe('AppContainer', () => {
    beforeEach(() => {
        const isHost = document.createElement('div');
        isHost.setAttribute('id', 'is-host');
        document.body.appendChild(isHost);
    });

    afterEach(() => {
        const isHost = document.getElementById('container');
        if (isHost) {
            document.body.removeChild(isHost);
        }
    });

    describe('component with redux', () => {
        const mockStore = configureMockStore();
        const initialState = {
            contentPopup: {},
            viewDetailPopup: {},
            invalidParams: {},
            messageResult: {},
            validateResendResult: {},
            confirmPopup: {},
        };

        const store = mockStore(initialState);
        const wrapper = shallow(<AppContainerWrapper store={store} />);

        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.contentPopup).toEqual(
                initialState.contentPopup
            );
            expect(wrapper.props().children.props.viewDetailPopup).toEqual(
                initialState.viewDetailPopup
            );
            expect(wrapper.props().children.props.invalidParams).toEqual(
                initialState.invalidParams
            );
            expect(wrapper.props().children.props.messageResult).toEqual(
                initialState.messageResult
            );
            expect(wrapper.props().children.props.validateResendResult).toEqual(
                initialState.validateResendResult
            );
            expect(wrapper.props().children.props.confirmPopup).toEqual(
                initialState.confirmPopup
            );
        });
    });

    it('render component correctly', () => {
        const props = {
            contentPopup: {},
            viewDetailPopup: {},
            invalidParams: {},
            messageResult: {},
            validateResendResult: {},
            confirmPopup: {},
        };

        const appContainer = shallow(<AppContainer {...props} />);

        expect(appContainer).toMatchSnapshot();
    });

    describe('messageResult is showed', () => {
        it('render correctly component ReponseMessage', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                invalidParams: {},
                messageResult: {
                    show: true
                },
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ResponseMessage).length).toBe(1);
        });

        function testMessageResult(message) {
            it('component render correctly message', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
                    invalidParams: {},
                    messageResult: {
                        show: true,
                        isSuccessful: true,
                        message: message || LangHelper.getResource('ResentNotificationSuccessfully'),
                    },
                    validateResendResult: {},
                    confirmPopup: {},
                };
    
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Action
                const responseMessage = appContainer.find(ResponseMessage);
    
                // Assert
                expect(responseMessage.props().isSuccessful).toBe(props.messageResult.isSuccessful);
                expect(responseMessage.props().message).toBe(props.messageResult.message);
            });
        }

        testMessageResult();
        testMessageResult(null);
        testMessageResult('');
        testMessageResult('some message');
    });

    describe('invalidParams is showed', () => {
        it('render correctly component ReponseMessage', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {
                    show: true
                },
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ResponseMessage).length).toBe(1);
        });

        function testMessageResult(message) {
            it('component render correctly message', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
                    messageResult: {},
                    invalidParams: {
                        show: true,
                        message: message === undefined ? '' : message,
                    },
                    validateResendResult: {},
                    confirmPopup: {},
                };
    
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Action
                const responseMessage = appContainer.find(ResponseMessage);
    
                // Assert
                expect(responseMessage.props().isSuccessful).toBe(false);
                expect(responseMessage.props().message).toBe(props.invalidParams.message);
            });
        }

        testMessageResult();
        testMessageResult(null);
        testMessageResult('');
        testMessageResult('some message');
    });

    it('render correctly component Filter', () => {
        // Arrange
        const props = {
            contentPopup: {},
            viewDetailPopup: {},
            invalidParams: {},
            messageResult: {},
            validateResendResult: {},
            confirmPopup: {}
        };

        // Action
        const appContainer = shallow(<AppContainer {...props} />);

        // Assert
        expect(appContainer.find(Filter).length).toBe(1);
    });

    describe('DataGrid', () => {
        it('render correctly component DataGrid', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                invalidParams: {},
                messageResult: {},
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(DataGrid).length).toBe(1);
        });

        function testDataGridWithIsHost(isHost) {
            it('render correctly component DataGrid with isHost', () => {
                // Arrange
                document.getElementById('is-host').checked = isHost;
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
                    invalidParams: {},
                    messageResult: {},
                    validateResendResult: {},
                    confirmPopup: {},
                };
    
                // Action
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Assert
                expect(appContainer.find(DataGrid).length).toBe(1);
                expect(appContainer.find(DataGrid).props().isHost).toBe(isHost);
            });
        }

        testDataGridWithIsHost(true);
        testDataGridWithIsHost(false);
    });

    describe('contentPopup is showed', () => {
        it('render correctly component Content', () => {
            // Arrange
            const props = {
                contentPopup: {
                    show: true
                },
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(Content).length).toBe(1);
        });

        it('Content have correct props', () => {
            // Arrange
            const props = {
                contentPopup: {
                    show: true,
                    data: {}
                },
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(Content).props().data).toEqual(props.contentPopup.data);
        });

        it('Content closes correctly', () => {
            // Arrange
            const props = {
                contentPopup: {
                    show: true,
                    data: {}
                },
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {},
                setContentPopup: jest.fn()
            };

            const appContainer = shallow(<AppContainer {...props} />);

            // Action
            appContainer.find(Content).props().onClose();

            // Assert
            expect(props.setContentPopup).toHaveBeenCalledWith({ show: false });
        });
    });

    describe('viewDetailPopup is showed', () => {
        it('render correctly component ViewDetail', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {
                    show: true
                },
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ViewDetail).length).toBe(1);
        });

        it('ViewDetail have correct props', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {
                    show: true,
                    data: {}
                },
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ViewDetail).props().data).toEqual(props.viewDetailPopup.data);
        });

        it('ViewDetail closes correctly', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {
                    show: true,
                    data: {}
                },
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {},
                setViewDetailPopup: jest.fn()
            };

            const appContainer = shallow(<AppContainer {...props} />);

            // Action
            appContainer.find(ViewDetail).props().onClose();

            // Assert
            expect(props.setViewDetailPopup).toHaveBeenCalledWith({ show: false });
        });
    });

    it('validateResendResult is showed', () => {
        // Arrange
        const props = {
            contentPopup: {},
            viewDetailPopup: {},
            messageResult: {},
            invalidParams: {},
            validateResendResult: {
                show: true
            },
            confirmPopup: {}
        };

        // Action
        const appContainer = shallow(<AppContainer {...props} />);

        // Assert
        expect(appContainer.find(ValidateResend).length).toBe(1);
    });

    describe('confirmPopup is showed', () => {
        it('render correctly component ConfirmPopup', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {
                    show: true
                }
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ConfirmPopup).length).toBe(1);
        });

        it('ConfirmPopup have correct props', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {
                    show: true
                }
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer.find(ConfirmPopup).props().size).toEqual('lg');
            expect(appContainer.find(ConfirmPopup).props().title).toEqual(LangHelper.getResource('Confirmation'));
        });

        it('ConfirmPopup have correct content', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {
                    show: true
                }
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);
            const content = appContainer.find(ConfirmPopup).props().content;

            // Assert
            expect(content.type).toBe('div');
            expect(content.props.children).toEqual(LangHelper.getResource('ThereAreSomeChangesConfirmMessage'));
        });

        it('ConfirmPopup closes correctly', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {
                    show: true
                },
                setConfirmPopup: jest.fn()
            };

            const appContainer = shallow(<AppContainer {...props} />);

            // Action
            appContainer.find(ConfirmPopup).props().onClose();

            // Assert
            expect(props.setConfirmPopup).toHaveBeenCalledWith({ show: false });
        });

        it('ConfirmPopup submits correctly', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                messageResult: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {
                    show: true,
                    onOk: jest.fn()
                },
                setConfirmPopup: jest.fn()
            };

            const appContainer = shallow(<AppContainer {...props} />);

            // Action
            appContainer.find(ConfirmPopup).props().onSubmit();

            // Assert
            expect(props.confirmPopup.onOk).toHaveBeenCalledTimes(1);
            expect(props.setConfirmPopup).toHaveBeenCalledWith({ show: false });
        });
    });
});
