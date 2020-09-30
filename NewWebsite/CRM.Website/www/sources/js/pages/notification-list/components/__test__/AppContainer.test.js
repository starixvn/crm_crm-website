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
            confirmPopup: {},
            autoRefresh: {}
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
            expect(wrapper.props().children.props.autoRefresh).toEqual(
                initialState.autoRefresh
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

    it('handleCloseContent runs correctly', () => {
        // Arrange
        const props = {
            contentPopup: {},
            viewDetailPopup: {},
            invalidParams: {},
            validateResendResult: {},
            confirmPopup: {},
            setContentPopup: jest.fn(),
            setIsRefresh: jest.fn()
        };

        const appContainer = shallow(<AppContainer {...props} />);
        appContainer.instance().setIsRefresh = jest.fn();

        // Action
        appContainer.instance().handleCloseContent();

        // Assert
        expect(props.setContentPopup).toHaveBeenCalledWith({ show: false });
        expect(props.setIsRefresh).toHaveBeenCalledWith(true);
    });

    it('handleCloseViewDetail runs correctly', () => {
        // Arrange
        const props = {
            contentPopup: {},
            viewDetailPopup: {},
            invalidParams: {},
            validateResendResult: {},
            confirmPopup: {},
            setViewDetailPopup: jest.fn(),
            setIsRefresh: jest.fn()
        };

        const appContainer = shallow(<AppContainer {...props} />);
        appContainer.instance().setIsRefresh = jest.fn();

        // Action
        appContainer.instance().handleCloseViewDetail();

        // Assert
        expect(props.setViewDetailPopup).toHaveBeenCalledWith({ show: false });
        expect(props.setIsRefresh).toHaveBeenCalledWith(true);
    });

    describe('render', () => {
        it('has regular children', () => {
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
                expect(appContainer.find(Content).props().data).toEqual(props.contentPopup.data);
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
                appContainer.instance().handleCloseContent = jest.fn();
    
                // Action
                appContainer.find(Content).simulate('close');
    
                // Assert
                expect(appContainer.instance().handleCloseContent).toHaveBeenCalled();
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
                    confirmPopup: {}
                };

                const appContainer = shallow(<AppContainer {...props} />);
                appContainer.instance().handleCloseViewDetail = jest.fn();
        
                // Action
                appContainer.find(ViewDetail).simulate('close');
        
                // Assert
                expect(appContainer.instance().handleCloseViewDetail).toHaveBeenCalled();
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

        describe('has ConfirmPopup child', () => {
            LangHelper.getResource = jest.fn(value => value);
    
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
                const content = shallow(appContainer.find(ConfirmPopup).props().content);
    
                // Assert
                expect(appContainer.find(ConfirmPopup).props().size).toBe('lg');
                expect(appContainer.find(ConfirmPopup).props().title).toBe('Confirmation');
                expect(content.find('div').html()).toBe('<div>ThereAreSomeChangesConfirmMessage</div>');
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
                    }
                };
    
                const appContainer = shallow(<AppContainer {...props} />);
                appContainer.instance().closeConfirmPopup = jest.fn();
    
                // Action
                appContainer.find(ConfirmPopup).simulate('close');
    
                // Assert
                expect(appContainer.instance().closeConfirmPopup).toHaveBeenCalled();
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
