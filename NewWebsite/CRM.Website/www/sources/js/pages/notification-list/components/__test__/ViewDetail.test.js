import React from 'react';
import ViewDetailWrapper, { ViewDetail } from '../ViewDetail';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import configureMockStore from 'redux-mock-store';
import { getHeadlines, getRecipients } from '../../functions';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

describe('ViewDetail', () => {
    LangHelper.getResource = jest.fn(value => value);

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
        expect(viewDetail.find(Modal).props().show).toBeTrue;
        expect(viewDetail.find(Modal).props().backdrop).toBe('static');
        expect(viewDetail.find(Modal).props().keyboard).toBeFalse;
        expect(viewDetail.find(Modal).props().size).toBe('lg');
        expect(viewDetail.find(Modal).props().centered).toBeTrue;
        expect(viewDetail.find(Modal).props().dialogAs).toEqual(DraggableModalDialog);
        expect(viewDetail.find(Modal.Header).props().closeButton).toBeTrue;
        expect(viewDetail.find(Modal.Title).props().as).toBe('div');
        expect(viewDetail.find(Modal.Title).html()).toContain('ErrorNotification');
        expect(viewDetail.find(Modal.Body).find('div').at(0).html()).toEqual(`<div>${`NotificationID: <b>${props.data.notificationId}`}</b></div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(1).html()).toEqual(`<div>${`NotificationTemplate: <b>${props.data.title}`}</b></div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(2).html()).toEqual(`<div>${`SentAt: <b>${props.data.sendDateString} ${props.data.sendTimeString}`}</b></div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(3).html()).toEqual('<div class="error-occured-message error-resolved-row">ErrorOccuredWhileSendingTheNotificationToTheFollowingRecipient</div>');
        expect(viewDetail.find(Table).props().minWidth).toBe(500);
        expect(viewDetail.find(Table).props().maxWidth).toBe(772);
        expect(viewDetail.find(Table).props().containerPadding).toBe(0);
        expect(viewDetail.find(Table).props().adjustedHeight).toBe(50);
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
        expect(viewDetail.find(Modal.Body).find('div').at(4).html()).toEqual('<div class="error-resolved-row">Remark: ResendDone</div>');
        expect(viewDetail.find(Modal.Body).find('div').at(5).html()).toEqual(`<div>ResolvedBy: ${props.viewDetail.resolvedBy}</div>`);
        expect(viewDetail.find(Modal.Footer).find(Button).length).toBe(1);
        expect(viewDetail.find(Modal.Footer).find(Button).at(0).html()).toContain('Cancel');
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
        expect(viewDetail.find(Modal.Body).find('div').at(4).html()).toEqual('<div class="error-resolved-row">Remark:</div>');
        expect(viewDetail.find(Modal.Body).find('textarea').html()).toEqual('<textarea class="view-detail-remark" rows="3" name="remark"></textarea>');
        expect(viewDetail.find(Modal.Footer).find(Button).length).toBe(2);
        expect(viewDetail.find(Modal.Footer).find(Button).at(0).html()).toContain('Resolved');
        expect(viewDetail.find(Modal.Footer).find(Button).at(1).html()).toContain('Cancel');
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
            <HeaderCell colWidth={25}>{'GroupHeadline'}</HeaderCell>
            <HeaderCell colWidth={25}>{'Recipient'}</HeaderCell>
            <HeaderCell colWidth={50}>{'Reason'}</HeaderCell>
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
            <Cell>{getHeadlines(props.data.headlines)}</Cell>
            <Cell>{getRecipients(props.data.recipients)}</Cell>
            <Cell className='error-color'>{props.data.reason}</Cell>
        </Row>];

        // Action
        const header = viewDetailInstance.buildBody(props.data);

        // Assert
        expect(header).toEqual(expectedBody);
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
            expect(props.onClose).toHaveBeenCalled();
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
            expect(props.onClose).toHaveBeenCalled();
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
            expect(viewDetail.instance().onClose).toHaveBeenCalled();
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
            expect(viewDetail.instance().onClose).toHaveBeenCalled();
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