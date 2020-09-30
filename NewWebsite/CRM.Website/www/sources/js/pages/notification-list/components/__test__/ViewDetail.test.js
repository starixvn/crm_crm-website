import React from 'react';
import ViewDetailWrapper, { ViewDetail } from '../ViewDetail';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import configureMockStore from 'redux-mock-store';
import functions, { getHeadlines, getRecipients } from '../../functions';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

describe('ViewDetail', () => {
    LangHelper.getResource = jest.fn(value => value);
    
    beforeEach(() => {
        const appContainer = document.createElement('div');
        appContainer.setAttribute('id', 'view-detail-modal-body');
        document.body.appendChild(appContainer);
    });

    afterEach(() => {
        const appContainer = document.getElementById('view-detail-modal-body');
        if (appContainer) {
            document.body.removeChild(appContainer);
        }
    });

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
        expect(viewDetail.find(Modal.Body).find('div').at(0).html()).toEqual(`<div id="view-detail-modal-body">${`NotificationID: <b>${props.data.notificationId}`}</b></div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(1).html()).toEqual(`<div>${`NotificationTemplate: <b>${props.data.title}`}</b></div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(2).html()).toEqual(`<div>${`SentAt: <b>${props.data.sendDateString} ${props.data.sendTimeString}`}</b></div>`);
        expect(viewDetail.find(Modal.Body).find('div').at(3).html()).toEqual('<div class="error-occured-message error-resolved-row">ErrorOccuredWhileSendingTheNotificationToTheFollowingRecipient</div>');
        expect(viewDetail.find(Modal.Body).find('div').at(4).html()).toContain('Remark:');
        expect(viewDetail.find(Table).props().width).toBe(document.getElementById('view-detail-modal-body').clientWidth - 1);
        expect(viewDetail.find(Table).props().autoWidth).toBeFalse;
        expect(viewDetail.find(Table).props().containerPadding).toBe(0);
    });

    it('renders correctly when state isResolved true', () => {
        // Arrange
        const state = {
            isResolved: true,
            remark: 'remark'
        };

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
        viewDetail.instance().setState(state);

        // Assert
        expect(viewDetail.find(Modal.Title).html()).toContain('Resolved');
        expect(viewDetail.find('div').at(4).html()).toContain(state.remark);
        expect(viewDetail.find('div').at(4).html()).toContain(state.remark);
    });

    it('buildHeader', () => {
        // Arrange
        const state = {
            modalWidth: 10
        };

        const expectedHeader = <HeaderRow>
            <HeaderCell colWidth={0.2 * state.modalWidth}>{'LetstalkName'}</HeaderCell>
            <HeaderCell colWidth={0.2 * state.modalWidth}>{'GroupHeadline'}</HeaderCell>
            <HeaderCell colWidth={0.2 * state.modalWidth}>{'Recipient'}</HeaderCell>
            <HeaderCell colWidth={0.4 * state.modalWidth}>{'Reason'}</HeaderCell>
        </HeaderRow>;

        const props = {
            data: {},
            viewDetail: {
                result: {}
            }
        };

        const viewDetail = shallow(<ViewDetail {...props} />);
        viewDetail.setState(state);
        
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
                reason: '',
                letstalkName: 'letstalkName'
            },
            viewDetail: {
                result: {}
            }
        };

        const viewDetail = shallow(<ViewDetail {...props} />);
        const viewDetailInstance = viewDetail.instance();
        const expectedBody = [<Row key={props.data.notificationId}>
            <Cell>{props.data.letstalkName}</Cell>
            <Cell>{getHeadlines(props.data.headlines)}</Cell>
            <Cell>{getRecipients(props.data.recipients)}</Cell>
            <Cell className='error-color'>{props.data.reason}</Cell>
        </Row>];

        // Action
        const body = viewDetailInstance.buildBody(props.data);

        // Assert
        expect(body).toEqual(expectedBody);
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
            const close = jest.fn();
            const state = {
                isResolved: true
            };
    
            const props = {
                data: {},
                viewDetail: {
                    result: {}
                },
                gridParams: {},
                autoRefresh: {
                    value: 'value'
                },
                setViewDetail: jest.fn(),
                getGridData: jest.fn((params, callback) => callback()),
                onClose: jest.fn(),
                setAutoRefresh: jest.fn()
            };
    
            const viewDetail = shallow(<ViewDetail {...props} />);
            viewDetail.instance().setState(state);
            functions.handleNewResendOptions = jest.fn();
    
            // Action
            viewDetail.instance().onClose(close);
    
            // Assert
            expect(props.setViewDetail).toHaveBeenCalledWith({ resolvedBy: '' });
            expect(close).toHaveBeenCalled();
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams, expect.any(Function));
            expect(props.setAutoRefresh).toHaveBeenCalledWith({
                ...props.autoRefresh,
                countDown: props.autoRefresh.value
            });
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