import React from 'react';
import ViewDetailWrapper, { ViewDetail } from '../ViewDetail';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import configureMockStore from 'redux-mock-store';

describe('ViewDetail', () => {
    it('render component correctly', () => {
        // Arrange
        const props = {
            data: {
                headlines: []
            },
            viewDetail: {
                result: {}
            }
        };

        // Action
        const viewDetail = shallow(<ViewDetail {...props} />);

        // Assert
        expect(viewDetail).toMatchSnapshot();
    });

    it('render general child element', () => {
        // Arrange
        const props = {
            data: {
                headlines: [],
                notificationId: 1,
                title: 'title',
                sendDateString: 'sendDateString',
                sendTimeString: 'sendTimeString'
            },
            viewDetail: {
                result: {}
            }
        };

        // Action
        const viewDetail = shallow(<ViewDetail {...props} />);

        // Assert
        expect(viewDetail.find(Modal.Title).html()).toContain(LangHelper.getResource('ErrorNotification'));
        expect(viewDetail.find(Modal.Body).find('div').at(0).html()).toEqual(`<div>${`${LangHelper.getResource('NotificationID')}: ${props.data.notificationId}`}</div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(1).html()).toEqual(`<div>${`${LangHelper.getResource('NotificationTemplate')}: ${props.data.title}`}</div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(2).html()).toEqual(`<div>${`${LangHelper.getResource('SentAt')}: ${props.data.sendDateString} ${props.data.sendTimeString}`}</div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(3).html()).toEqual(`<div class="error-occured-message error-resolved-row">${LangHelper.getResource('ErrorOccuredWhileSendingTheNotificationToTheFollowingRecipient')}</div>`);
        expect(viewDetail.find(Modal.Body).find(Table).length).toBe(1);
    });

    it('render child element when view dettail result is successful', () => {
        // Arrange
        const props = {
            data: {
                headlines: [],
                notificationId: 1,
                title: 'title',
                sendDateString: 'sendDateString',
                sendTimeString: 'sendTimeString'
            },
            viewDetail: {
                resolvedBy: 'sa',
                result: {
                    isSuccessful: true
                }
            }
        };

        // Action
        const viewDetail = shallow(<ViewDetail {...props} />);

        // Assert
        expect(viewDetail.find(Modal.Body).find('div').at(4).html()).toEqual(`<div class="error-resolved-row">${LangHelper.getResource('Remark') + ': ' + LangHelper.getResource('ResendDone')}</div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(5).html()).toEqual(`<div>${LangHelper.getResource('ResolvedBy') + ': ' + props.viewDetail.resolvedBy}</div>`);
        expect(viewDetail.find(Modal.Footer).find(Button).length).toBe(1);
        expect(viewDetail.find(Modal.Footer).find(Button).at(0).html()).toContain(LangHelper.getResource('Cancel'));
    });

    it('render child element when view dettail result is not successful', () => {
        // Arrange
        const props = {
            data: {
                headlines: [],
                notificationId: 1,
                title: 'title',
                sendDateString: 'sendDateString',
                sendTimeString: 'sendTimeString'
            },
            viewDetail: {
                resolvedBy: 'sa',
                result: {
                    isSuccessful: false
                }
            }
        };

        // Action
        const viewDetail = shallow(<ViewDetail {...props} />);

        // Assert
        expect(viewDetail.find(Modal.Body).find('div').at(4).html()).toEqual(`<div class="error-resolved-row">${LangHelper.getResource('Remark') + ':'}</div>`);
        expect(viewDetail.find(Modal.Body).find('textarea').html()).toEqual(`<textarea class="view-detail-remark" rows="3" name="remark"></textarea>`);
        expect(viewDetail.find(Modal.Footer).find(Button).length).toBe(2);
        expect(viewDetail.find(Modal.Footer).find(Button).at(0).html()).toContain(LangHelper.getResource('Resolved'));
        expect(viewDetail.find(Modal.Footer).find(Button).at(1).html()).toContain(LangHelper.getResource('Cancel'));
    });

    it('render Table component with expected props', () => {
        // Arrange
        const props = {
            data: {},
            viewDetail: {
                result: {}
            }
        };

        // Action
        const viewDetail = shallow(<ViewDetail {...props} />);

        // Assert
        expect(viewDetail.find(Table).props().minWidth).toBe(500);
        expect(viewDetail.find(Table).props().maxWidth).toBe(772);
        expect(viewDetail.find(Table).props().containerPadding).toBe(0);
        expect(viewDetail.find(Table).props().adjustedHeight).toBe(50);
    });

    it('buildHeader', () => {
        // Arrange
        const expectedHeader = <HeaderRow>
            <HeaderCell colWidth={25}>{LangHelper.getResource('GroupHeadline')}</HeaderCell>
            <HeaderCell colWidth={25}>{LangHelper.getResource('Recipient')}</HeaderCell>
            <HeaderCell colWidth={50}>{LangHelper.getResource('Reason')}</HeaderCell>
        </HeaderRow>;

        const props = {
            data: {},
            viewDetail: {
                result: {}
            }
        };

        const viewDetail = shallow(<ViewDetail {...props} />);
        
        // Action
        const header = viewDetail.instance().buildHeader();

        // Assert
        expect(header).toEqual(expectedHeader);
    });

    it('buildBody', () => {
        // Arrange
        const props = {
            data: {
                notificationId: 1,
                headlines: [],
                recipients: [],
                reason: ''
            },
            viewDetail: {
                result: {}
            }
        };

        const viewDetail = shallow(<ViewDetail {...props} />);
        const viewDetailInstance = viewDetail.instance();
        const expectedBody = [<Row key={props.data.notificationId}>
            <Cell>{viewDetailInstance.getHeadlines(props.data.headlines)}</Cell>
            <Cell>{viewDetailInstance.getRecipients(props.data.recipients)}</Cell>
            <Cell className='error-color'>{props.data.reason}</Cell>
        </Row>];

        // Action
        const header = viewDetailInstance.buildBody(props.data);

        // Assert
        expect(header).toEqual(expectedBody);
    });
    
    describe('getHeadlines', () => {
        function getHeadlines_Test(headlines, expected) {
            it('getHeadlines_Test', () => {
                // Arrange
                const props = {
                    data: {},
                    viewDetail: {
                        result: {}
                    }
                };
        
                const viewDetail = shallow(<ViewDetail {...props} />);
                
                // Action
                const result = viewDetail.instance().getHeadlines(headlines);
        
                // Assert
                expect(result).toEqual(expected);
            });
        }
    
        getHeadlines_Test(null, '');
        getHeadlines_Test(undefined, '');
        getHeadlines_Test([], '');
        getHeadlines_Test([ { headlineName: 'name1' }, { headlineName: 'name2' } ], 'name1, name2');
    });
    
    describe('getRecipients', () => {
        const recipients = [ { recipientName: 'recipient1', roleName: 'role1' }, { recipientName: 'recipient2', roleName: 'role2' } ];
        function getRecipients_Test(recipients, expected) {
            it('getRecipients_Test', () => {
                // Arrange
                const props = {
                    data: {},
                    viewDetail: {
                        result: {}
                    }
                };
        
                const viewDetail = shallow(<ViewDetail {...props} />);
                
                // Action
                const result = viewDetail.instance().getRecipients(recipients);
        
                // Assert
                expect(result).toEqual(expected);
            });
        }
    
        getRecipients_Test(null, '');
        getRecipients_Test(undefined, '');
        getRecipients_Test([], '');
        getRecipients_Test(recipients, recipients.map((recipient, index) => <div key={index}>{`${recipient.recipientName} (${recipient.roleName})`}</div>));
    });

    describe('handleChange', () => {
        it('handleChange called when textarea change', () => {
            // Arrange
            const event = {
                target: { name: 'remark', value: 'the-value' }
            };
    
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                }
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().handleChange = jest.fn();
    
            // Action
            viewDetail.find('textarea').simulate('change', event);
    
            // Assert
            expect(viewDetail.instance().handleChange).toHaveBeenCalled();
        });

        it('handleChange called setState correctly', () => {
            // Arrange
            const event = {
                target: { name: 'remark', value: 'the-value' }
            };
    
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                }
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
    
            // Action
            viewDetail.find('textarea').props().onChange(event);
    
            // Assert
            expect(viewDetail.state('remark')).toBe('the-value');
        });
    });

    describe('onClose', () => {
        it('onClose called with isResolved is true executes correctly', () => {
            // Arrange
            const state = {
                isResolved: true
            };
    
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                },
                gridParams: {},
                setViewDetail: jest.fn(),
                getGridData: jest.fn(),
                onClose: jest.fn()
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().setState(state);
    
            // Action
            viewDetail.find(Modal).props().onHide();
    
            // Assert
            expect(props.setViewDetail).toHaveBeenCalledWith({
                resolvedBy: '',
                result: {
                    code: 1,
                    message: '',
                    isSuccessful: false
                }
            });
            expect(props.onClose).toHaveBeenCalledTimes(1);
            expect(props.getGridData).toHaveBeenCalledWith({...props.gridParams});
        });

        it('onClose called with isResolved is false and remark not change executes correctly', () => {
            // Arrange
            const state = {
                isResolved: false,
                remark: '',
                oldRemark: ''
            };
    
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                },
                gridParams: {},
                onClose: jest.fn()
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().setState(state);
    
            // Action
            viewDetail.find(Modal).props().onHide();
    
            // Assert
            expect(props.onClose).toHaveBeenCalledTimes(1);
        });

        it('onClose called with isResolved is false and remark change executes correctly', () => {
            // Arrange
            const state = {
                isResolved: false,
                remark: 'new',
                oldRemark: 'old'
            };
    
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                },
                gridParams: {},
                setConfirmPopup: jest.fn(),
                onClose: jest.fn()
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().setState(state);
    
            // Action
            viewDetail.find(Modal).props().onHide();
    
            // Assert
            expect(props.setConfirmPopup).toHaveBeenCalledWith({
                show: true,
                onOk: props.onClose
            });
        });

        it('onClose called when click close button', () => {
            // Arrange
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                }
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().onClose = jest.fn();
    
            // Action
            viewDetail.find(Button).at(1).simulate('click');
    
            // Assert
            expect(viewDetail.instance().onClose).toHaveBeenCalledTimes(1);
        });

        it('onClose called when trigger hide event', () => {
            // Arrange
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                }
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().onClose = jest.fn();
    
            // Action
            viewDetail.simulate('hide');
    
            // Assert
            expect(viewDetail.instance().onClose).toHaveBeenCalledTimes(1);
        });
    });

    describe('editNotificationList', () => {
        it('editNotificationList called when click resolved button', () => {
            // Arrange
            const props = {
                data: {},
                viewDetail: {
                    result: {
                        isSuccessful: false
                    }
                }
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().editNotificationList = jest.fn();
    
            // Action
            viewDetail.find(Modal.Footer).find(Button).at(0).simulate('click');
    
            // Assert
            expect(viewDetail.instance().editNotificationList).toHaveBeenCalled();
        });

        it('editNotificationList called and state change correctly', () => {
            // Arrange
            const notificationId = 1;
            const state = {
                isResolved: false,
                remark: 'remark'
            };

            const props = {
                data: {},
                viewDetail: {
                    result: {}
                },
                editNotificationList: jest.fn()
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.setState(state);
    
            // Action
            viewDetail.instance().editNotificationList(notificationId);
    
            // Assert
            expect(viewDetail.state('isResolved')).toBeTrue;
            expect(props.editNotificationList).toHaveBeenCalledWith({
                notificationId,
                remark: viewDetail.state('remark')
            });
        });
    });

    describe('redux connect', () => {
        const mockStore = configureMockStore();
        const initialState = {
            viewDetail: {
                result: {}
            },
            gridParams: {
                notificationTypeId: 1
            }
        };

        const store = mockStore(initialState);
        const wrapper = shallow(<ViewDetailWrapper store={store} />);

        it('component has correct props', () => {
            expect(wrapper.props().children.props.viewDetail).toEqual(initialState.viewDetail);
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
        });
    });
});