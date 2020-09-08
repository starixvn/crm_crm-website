import React from 'react';
import ValidateResendWrapper, { ValidateResend } from '../ValidateResend';
import { shallow } from 'enzyme';
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

        it('resendNotifications runs correctly', () => {
            // Arrange
            const promise = Promise.resolve({});
            const dataList = [];
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        isSingle: true,
                        resendDataList: []
                    }
                },
                gridParams: {}
            };

            const validateResend = shallow(<ValidateResend {...props} />);
            validateResend.instance().onClose = jest.fn();
            functions.resendNotifications = jest.fn(() => promise);

            // Action
            validateResend.instance().resendNotifications(dataList);

            // Assert
            expect(validateResend.instance().onClose).toHaveBeenCalled();
            promise.then(() => {
                expect(functions.resendNotifications).toHaveBeenCalledWith(dataList, validateResend.instance().callBackSuccess);
            });
        });

        it('callBackSuccess runs correctly', () => {
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        isSingle: true,
                        resendDataList: []
                    }
                },
                gridParams: {
                    value: 'value'
                },
                getGridData: jest.fn((params, callback) => callback([])),
                setResendOptions: jest.fn()
            };

            functions.handleNewResendOptions = jest.fn(data => handleNewResendOptions(data, props.setResendOptions));
            const validateResend = shallow(<ValidateResend {...props} />);

            // Action
            validateResend.instance().callBackSuccess();

            // Assert
            expect(props.getGridData).toHaveBeenCalledWith({...props.gridParams}, expect.any(Function));
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
            expect(validateResend.instance().onClose).toHaveBeenCalled();
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
            expect(validateResend.instance().onClose).toHaveBeenCalled();
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
            expect(validateResend.instance().onClose).toHaveBeenCalled();
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
            expect(validateResend.instance().onClose).toHaveBeenCalled();
        });
    });
});