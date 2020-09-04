import React from 'react';
import ValidateResendWrapper, { ValidateResend } from '../ValidateResend';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';
import functions, { handleNewResendOptions } from '../../functions';
import { Modal, Button } from 'react-bootstrap';

describe('ValidateResend', () => {
    describe('component with redux', () => {
        const mockStore = configureMockStore();
        const initialState = {
            validateResendResult: {},
            gridParams: {}
        };

        const store = mockStore(initialState);
        const wrapper = shallow(<ValidateResendWrapper store={store} />);

        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.validateResendResult).toEqual(initialState.validateResendResult);
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
        });
    });

    describe('functions run correctly', () => {
        describe('onClose', () => {
            it('onClose calls setValidateResend', () => {
                // Arrange
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [],
                            resendDataList: []
                        }
                    },
                    gridParams: {},
                    setValidateResend: jest.fn()
                };

                const validateResend = shallow(<ValidateResend {...props} />);

                // Action
                validateResend.instance().onClose();

                // Assert
                expect(props.setValidateResend).toHaveBeenCalledWith({ show: false });
            });
        });

        describe('resendNotifications', () => {
            it('resendNotifications runs correctly', () => {
                // Arrange
                const expectedDataList = [ 'value' ];
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [],
                            resendDataList: []
                        }
                    },
                    gridParams: {},
                    setInvalidParams: jest.fn(),
                    resendNotifications: jest.fn()
                };

                const validateResend = shallow(<ValidateResend {...props} />);
                validateResend.instance().onClose = jest.fn();
                validateResend.instance().callBackSuccess = jest.fn();

                // Action
                validateResend.instance().resendNotifications(expectedDataList);

                // Assert
                expect(validateResend.instance().onClose).toHaveBeenCalledTimes(1);
                expect(props.setInvalidParams).toHaveBeenCalledWith({ show: false });
                expect(props.resendNotifications).toHaveBeenCalledWith(expectedDataList, validateResend.instance().callBackSuccess);
            });
        });

        describe('callBackSuccess', () => {
            jest.useFakeTimers();

            it('callBackSuccess runs correctly', () => {
                // Arrange
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [],
                            resendDataList: []
                        }
                    },
                    gridParams: {
                        value: 'value'
                    },
                    getGridData: jest.fn((params, callback) => callback([])),
                    setMessageResult: jest.fn(),
                    setResendOptions: jest.fn()
                };

                const validateResend = shallow(<ValidateResend {...props} />);
                functions.handleNewResendOptions = jest.fn(data => handleNewResendOptions(data, props.setResendOptions));

                // Action
                validateResend.instance().callBackSuccess();
                jest.runAllTimers();

                // Assert
                expect(props.getGridData).toHaveBeenCalledTimes(1);
                expect(setTimeout).toHaveBeenCalledTimes(1);
                expect(setTimeout).toHaveBeenLastCalledWith(expect.any(Function), 2000);
                expect(props.setMessageResult).toHaveBeenCalledWith({ show: false });
            });
        });
    });

    describe('events fire correctly', () => {
        it('Modal onHide calls onClose', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendDataList: []
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().onClose = jest.fn();

            // Action
            validateResend.find(Modal).simulate('hide');

            // Assert
            expect(validateResend.instance().onClose).toHaveBeenCalledTimes(1);
        });

        it('Single Modal.Footer click button OK calls onClose', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [ 'value' ],
                        resendDataList: [],
                        isSingle: true
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().onClose = jest.fn();

            // Action
            validateResend.find(Button).simulate('click');

            // Assert
            expect(validateResend.instance().onClose).toHaveBeenCalledTimes(1);
        });

        it('Single Modal.Footer click button OK calls resendNotifications', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendDataList: [ 'value' ],
                        isSingle: true
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().resendNotifications = jest.fn();

            // Action
            validateResend.find(Button).at(0).simulate('click');

            // Assert
            expect(validateResend.instance().resendNotifications).toHaveBeenCalledWith(props.validateResendResult.result.resendDataList);
        });

        it('Single Modal.Footer click button Cancel calls onClose', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendDataList: [ 'value' ],
                        isSingle: true
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().onClose = jest.fn();

            // Action
            validateResend.find(Button).at(1).simulate('click');

            // Assert
            expect(validateResend.instance().onClose).toHaveBeenCalledTimes(1);
        });

        it('Multiple Modal.Footer click button OK calls resendNotifications', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendDataList: [ 'value' ],
                        isSingle: false
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().resendNotifications = jest.fn();

            // Action
            validateResend.find(Button).at(0).simulate('click');

            // Assert
            expect(validateResend.instance().resendNotifications).toHaveBeenCalledWith(props.validateResendResult.result.resendDataList);
        });

        it('Multiple Modal.Footer click button Cancel calls onClose', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendDataList: [ 'value' ],
                        isSingle: false
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().onClose = jest.fn();

            // Action
            validateResend.find(Button).at(1).simulate('click');

            // Assert
            expect(validateResend.instance().onClose).toHaveBeenCalledTimes(1);
        });
    });
});