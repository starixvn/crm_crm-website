import React, { Fragment } from 'react';
import { ViewDetail } from '../ViewDetail';
import { shallow } from 'enzyme';
import { Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';

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
});