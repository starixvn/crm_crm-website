import React from 'react';
import ValidateResendWrapper, { ValidateResend } from '../ValidateResend';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import functions, { handleNewResendOptions } from '../../functions';
import { Modal, Button } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

describe('ValidateResend', () => {
    LangHelper.getResource = jest.fn(value => value);

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
                            resendIds: []
                        }
                    },
                    resendOptions: {
                        dataList: []
                    },
                    gridParams: {},
                    setValidateResend: jest.fn(),
                    setIsRefresh: jest.fn()
                };

                const validateResend = shallow(<ValidateResend {...props} />);

                // Action
                validateResend.instance().onClose();

                // Assert
                expect(props.setValidateResend).toHaveBeenCalledWith({ show: false });
            });
            
            it('should not call setIsRefresh with resendOptions dataList has error notification resend', () => {
                // Arrange
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [],
                            resendIds: []
                        }
                    },
                    resendOptions: {
                        dataList: [
                            {
                                isResent: true,
                                status: 'Error'
                            }
                        ]
                    },
                    gridParams: {},
                    setValidateResend: jest.fn(),
                    setIsRefresh: jest.fn()
                };

                const validateResend = shallow(<ValidateResend {...props} />);

                // Action
                validateResend.instance().onClose();

                // Assert
                expect(props.setIsRefresh).toHaveBeenCalledTimes(0);
            });
            
            it('should call setIsRefresh with resendOptions dataList not has error notification resend', () => {
                // Arrange
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [],
                            resendIds: []
                        }
                    },
                    resendOptions: {
                        dataList: []
                    },
                    gridParams: {},
                    setValidateResend: jest.fn(),
                    setIsRefresh: jest.fn()
                };

                const validateResend = shallow(<ValidateResend {...props} />);

                // Action
                validateResend.instance().onClose();

                // Assert
                expect(props.setIsRefresh).toHaveBeenCalledWith(true);
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
                        resendIds: []
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
                        resendIds: []
                    }
                },
                gridParams: {
                    value: 'value'
                },
                autoRefresh: {
                    value: 90
                },
                getGridData: jest.fn((params, callback) => callback([])),
                setResendOptions: jest.fn(),
                setIsRefresh: jest.fn(),
                setAutoRefresh: jest.fn()
            };

            const validateResend = shallow(<ValidateResend {...props} />);

            // Action
            validateResend.instance().callBackSuccess();

            // Assert
            expect(props.setIsRefresh).toHaveBeenCalledWith(true);
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams, expect.any(Function));
            expect(props.setAutoRefresh).toHaveBeenCalledWith({
                ...props.autoRefresh,
                countDown: props.autoRefresh.value
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
                        resendIds: []
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
                        resendIds: [],
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
                        resendIds: [ 1 ],
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
            expect(validateResend.instance().resendNotifications)
                .toHaveBeenCalledWith(props.validateResendResult.result.resendIds.toString());
        });

        it('Single Modal.Footer click button Cancel calls onClose', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendIds: [ 'value' ],
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
                        resendIds: [ 1 ],
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
            expect(validateResend.instance().resendNotifications)
                .toHaveBeenCalledWith(props.validateResendResult.result.resendIds.toString());
        });

        it('Multiple Modal.Footer click button Cancel calls onClose', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendIds: [ 'value' ],
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

    describe('render', () => {
        LangHelper.formatString = jest.fn(value => value);

        it('has regular children', () => {
            // Arrange
            const props = {
                validateResendResult: {
                    result: {
                        inActiveNotificationIds: [],
                        resendIds: []
                    }
                },
                gridParams: {}
            };

            // Action
            const validateResend = shallow(<ValidateResend {...props} />);

            // Assert
            expect(validateResend.find(Modal).props().show).toBeTrue;
            expect(validateResend.find(Modal).props().backdrop).toBe('static');
            expect(validateResend.find(Modal).props().keyboard).toBeFalse;
            expect(validateResend.find(Modal).props().size).toBe('lg');
            expect(validateResend.find(Modal).props().centered).toBeTrue;
            expect(validateResend.find(Modal.Header).props().closeButton).toBeTrue;
            expect(validateResend.find(Modal.Title).props().as).toBe('div');
            expect(validateResend.find(Modal.Title).html()).toContain('Confirmation');
            expect(validateResend.find(Modal).props().dialogAs).toEqual(DraggableModalDialog);
        });

        describe('has Modal.Body', () => {
            describe('isSingle', () => {
                it('has not invalid notification ids', () => {
                    // Arrange
                    const props = {
                        validateResendResult: {
                            result: {
                                inActiveNotificationIds: [],
                                resendIds: [ 1 ],
                                isSingle: true
                            }
                        },
                        gridParams: {}
                    };
        
                    // Action
                    const validateResend = shallow(<ValidateResend {...props} />);
    
                    // Assert
                    expect(validateResend.find(Modal.Body).html()).toContain('ComfirmResendNotification');
                });
                
                it('has invalid notification ids', () => {
                    // Arrange
                    const props = {
                        validateResendResult: {
                            result: {
                                inActiveNotificationIds: [ 1 ],
                                resendIds: [],
                                isSingle: true
                            }
                        },
                        gridParams: {}
                    };
        
                    // Action
                    const validateResend = shallow(<ValidateResend {...props} />);
    
                    // Assert
                    expect(validateResend.find(Modal.Body).html()).toContain('TheRecipientOfNotificationIdHasBeenInactive');
                });
            });

            describe('not isSingle', () => {
                it('has all invalid notification ids', () => {
                    // Arrange
                    const props = {
                        validateResendResult: {
                            result: {
                                inActiveNotificationIds: [ 1 ],
                                resendIds: [],
                                isSingle: false
                            }
                        },
                        gridParams: {}
                    };
        
                    // Action
                    const validateResend = shallow(<ValidateResend {...props} />);
    
                    // Assert
                    expect(validateResend.find(Modal.Body).html()).toContain('TheRecipientOfNotificationIdHasBeenInactive');
                });

                it('has all valid notification ids', () => {
                    // Arrange
                    const props = {
                        validateResendResult: {
                            result: {
                                inActiveNotificationIds: [],
                                resendIds: [ 1 ],
                                isSingle: false
                            }
                        },
                        gridParams: {}
                    };
        
                    // Action
                    const validateResend = shallow(<ValidateResend {...props} />);
    
                    // Assert
                    expect(validateResend.find(Modal.Body).html()).toContain('ComfirmResendNotification');
                });
                
                it('has both valid and invalid notification ids', () => {
                    // Arrange
                    const props = {
                        validateResendResult: {
                            result: {
                                inActiveNotificationIds: [ 1 ],
                                resendIds: [ 1 ],
                                isSingle: false
                            }
                        },
                        gridParams: {}
                    };
        
                    // Action
                    const validateResend = shallow(<ValidateResend {...props} />);
    
                    // Assert
                    expect(validateResend.find(Modal.Body).find('div').length).toBe(4);
                    expect(validateResend.find(Modal.Body).find('div').at(1).html())
                        .toContain('TheRecipientOfNotificationIdHasBeenInactive');
                    expect(validateResend.find(Modal.Body).find('div').at(2).html())
                        .toContain('TheSelectedNotificationsAreAbleToResend');
                    expect(validateResend.find(Modal.Body).find('div').at(3).html())
                        .toContain('ComfirmResendListAndIgnoreNotification');
                });
            });
        });

        describe('has Modal.Footer', () => {
            it('has only OK button', () => {
                // Arrange
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [ 1 ],
                            resendIds: [],
                            isSingle: true
                        }
                    },
                    gridParams: {}
                };
    
                // Action
                const validateResend = shallow(<ValidateResend {...props} />);

                // Assert
                expect(validateResend.find(Modal.Footer).find(Button).html()).toContain('OK');
                expect(validateResend.find(Modal.Footer).find(Button).props().variant).toBe('primary');
                expect(validateResend.find(Modal.Footer).find(Button).props().size).toBe('sm');
            });
            
            it('has OK and Cancel button', () => {
                // Arrange
                const props = {
                    validateResendResult: {
                        result: {
                            inActiveNotificationIds: [],
                            resendIds: [ 1 ],
                            isSingle: true
                        }
                    },
                    gridParams: {}
                };
    
                // Action
                const validateResend = shallow(<ValidateResend {...props} />);

                // Assert
                expect(validateResend.find(Modal.Footer).find(Button).at(0).html()).toContain('OK');
                expect(validateResend.find(Modal.Footer).find(Button).at(0).props().variant).toBe('primary');
                expect(validateResend.find(Modal.Footer).find(Button).at(0).props().size).toBe('sm');
                expect(validateResend.find(Modal.Footer).find(Button).at(1).html()).toContain('Cancel');
                expect(validateResend.find(Modal.Footer).find(Button).at(1).props().variant).toBe('primary');
                expect(validateResend.find(Modal.Footer).find(Button).at(1).props().size).toBe('sm');
            });
        });
    });
});