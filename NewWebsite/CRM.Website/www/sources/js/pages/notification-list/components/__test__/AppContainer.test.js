import React from 'react';
import AppContainerWrapper, { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import Content from '../Content';
import ViewDetail from '../ViewDetail';
import ValidateResend from '../ValidateResend';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
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
        const isHost = document.getElementById('is-host');
        if (isHost) {
            document.body.removeChild(isHost);
        }
    });

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            contentPopup: {},
            viewDetailPopup: {},
            validateResendResult: {},
            confirmPopup: {}
        };

        const store = mockStore(initialState);
        
        // Action
        const wrapper = shallow(<AppContainerWrapper store={store} />);

        // Assert
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
            expect(wrapper.props().children.props.validateResendResult).toEqual(
                initialState.validateResendResult
            );
            expect(wrapper.props().children.props.confirmPopup).toEqual(
                initialState.confirmPopup
            );
        });
    });

    describe('closeConfirmPopup', () => {
        it('runs correctly', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {},
                setConfirmPopup: jest.fn()
            };

            const appContainer = shallow(<AppContainer {...props} />);
            // Action
            appContainer.instance().closeConfirmPopup();
    
            // Assert
            expect(props.setConfirmPopup).toHaveBeenCalledWith({ show: false });
        });
    });

    describe('render', () => {
        it('has regular child', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                invalidParams: {},
                validateResendResult: {},
                confirmPopup: {}
            };

            // Action
            const appContainer = shallow(<AppContainer {...props} />);

            // Assert
            expect(appContainer).toMatchSnapshot();
            expect(appContainer.find(Filter).length).toBe(1);
            expect(appContainer.find(DataGrid).props().isHost).toEqual(document.getElementById('is-host').checked);
        });

        describe('Content', () => {
            it('has Content child', () => {
                // Arrange
                const props = {
                    contentPopup: {
                        show: true,
                        data: {}
                    },
                    viewDetailPopup: {},
                    invalidParams: {},
                    validateResendResult: {},
                    confirmPopup: {}
                };
    
                // Action
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Assert
                expect(appContainer).toMatchSnapshot();
                expect(appContainer.find(Content).props().data).toEqual(props.contentPopup.data)
            });
    
            it('runs onClose', () => {
                // Arrange
                const props = {
                    contentPopup: {
                        show: true,
                        data: {}
                    },
                    viewDetailPopup: {},
                    invalidParams: {},
                    validateResendResult: {},
                    confirmPopup: {},
                    setContentPopup: jest.fn()
                };
    
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Action
                appContainer.find(Content).simulate('close');
    
                // Assert
                expect(props.setContentPopup).toHaveBeenCalledWith({ show: false });
            });
        });

        describe('ViewDetail', () => {
            it('has ViewDetail child', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {
                        show: true,
                        data: {}
                    },
                    invalidParams: {},
                    validateResendResult: {},
                    confirmPopup: {}
                };
        
                // Action
                const appContainer = shallow(<AppContainer {...props} />);
        
                // Assert
                expect(appContainer).toMatchSnapshot();
                expect(appContainer.find(ViewDetail).props().data).toEqual(props.viewDetailPopup.data);
            });

            it('runs onClose', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {
                        show: true,
                        data: {}
                    },
                    invalidParams: {},
                    validateResendResult: {},
                    confirmPopup: {},
                    setViewDetailPopup: jest.fn()
                };

                const appContainer = shallow(<AppContainer {...props} />);
        
                // Action
                appContainer.find(ViewDetail).simulate('close');
        
                // Assert
                expect(props.setViewDetailPopup).toHaveBeenCalledWith({ show: false });
            });
        });

        it('has ValidateResend', () => {
            // Arrange
            const props = {
                contentPopup: {},
                viewDetailPopup: {},
                invalidParams: {},
                validateResendResult: {
                    show: true
                },
                confirmPopup: {}
            };
    
            // Action
            const appContainer = shallow(<AppContainer {...props} />);
    
            // Assert
            expect(appContainer).toMatchSnapshot();
            expect(appContainer.find(ValidateResend).length).toBe(1);
        });

        describe('ConfirmPopup', () => {
            LangHelper.getResource = jest.fn(value => value);
    
            it('has ConfirmPopup child', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
                    invalidParams: {},
                    validateResendResult: {},
                    confirmPopup: {
                        show: true
                    }
                };
    
                // Action
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Assert
                expect(appContainer).toMatchSnapshot();
                expect(appContainer.find(ConfirmPopup).length).toBe(1);
            });
    
            it('has correct props', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
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
                expect(appContainer.find(ConfirmPopup).props().title).toEqual('Confirmation');
            });
    
            it('has correct content', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
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
                expect(content.props.children).toEqual('ThereAreSomeChangesConfirmMessage');
            });
    
            it('runs onClose', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
                    invalidParams: {},
                    validateResendResult: {},
                    confirmPopup: {
                        show: true
                    },
                    setConfirmPopup: jest.fn()
                };
    
                const appContainer = shallow(<AppContainer {...props} />);
    
                // Action
                appContainer.find(ConfirmPopup).simulate('close');
    
                // Assert
                expect(props.setConfirmPopup).toHaveBeenCalledWith({ show: false });
            });
    
            it('runs onSubmit', () => {
                // Arrange
                const props = {
                    contentPopup: {},
                    viewDetailPopup: {},
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
                appContainer.find(ConfirmPopup).simulate('submit');
    
                // Assert
                expect(props.confirmPopup.onOk).toHaveBeenCalled();
                expect(props.setConfirmPopup).toHaveBeenCalledWith({ show: false });
            });
        });
    });
});
