import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { PAGING_SIZES } from '@jsroot/common/constants';
import { Form } from 'react-bootstrap';
import functions, { handleNewResendOptions, getHeadlines, getRecipients } from '../../functions';

describe('DataGrid', () => {
    LangHelper.getResource = jest.fn(value => value);

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            gridData: {},
            resendOptions: {}
        };
        
        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<DataGridWrapper store={store} />);

        // Assert
        it('component renders correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.gridData).toEqual(initialState.gridData);
            expect(wrapper.props().children.props.resendOptions).toEqual(initialState.resendOptions);
        });
    });
    
    beforeEach(() => {
        const appContainer = document.createElement('div');
        appContainer.setAttribute('id', 'app-container');
        document.body.appendChild(appContainer);
    });

    afterEach(() => {
        const appContainer = document.getElementById('app-container');
        if (appContainer) {
            document.body.removeChild(appContainer);
        }
    });

    function testGridDataNull(gridData) {
        it('renders null when gridData empty', () => {
            // Arrange
            const props = {
                gridParams: {},
                gridData: gridData,
                resendOptions: {}
            };
    
            // Action
            const dataGrid = shallow(<DataGrid {...props} />);

            // Assert
            expect(dataGrid).toBeNull;
        });
    }

    testGridDataNull();
    testGridDataNull(null);
    testGridDataNull({});
    testGridDataNull({ data: [] });

    describe('renders correctly child component', () => {
        it('has correct title div', () => {
            // Arrange
            const props = {
                gridParams: {},
                gridData: {
                    data: [
                        {
                            notificationId: 1,
                            status: 'Error'
                        }
                    ]
                },
                resendOptions: {}
            };

            // Action
            const dataGrid = shallow(<DataGrid {...props} />);

            // Assert
            expect(dataGrid.find('div').html()).toEqual(`<div class="table-title">NotificationList</div>`);
        });

        it('contains Table having correct props', () => {
            // Arrange
            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                gridData: {
                    totalItems: 100,
                    data: [
                        {
                            notificationId: 1,
                            status: 'Error'
                        }
                    ]
                },
                resendOptions: {}
            };

            const pagingOption = {
                PageIndex: props.gridParams.pageIndex,
                PageSize: props.gridParams.pageSize,
                TotalItem: props.gridData.totalItems,
                PageList: PAGING_SIZES
            };

            // Action
            const dataGrid = shallow(<DataGrid {...props} />);

            // Assert
            expect(dataGrid.find(Table).length).toBe(1);
            expect(dataGrid.find(Table).props().minWidth).toBe(960);
            expect(dataGrid.find(Table).props().autoWidth).toBeTrue;
            expect(dataGrid.find(Table).props().isPaging).toBeTrue;
            expect(dataGrid.find(Table).props().pageOption).toEqual(pagingOption);
        });
    });

    describe('function component runs correctly', () => {
        it('runs onPaging', () => {
            // Arrange
            const expectedPageIndex = 1;
            const expectedPageSize = 100;
            const props = {
                gridParams: {},
                gridData: {
                    data: [
                        {
                            notificationId: 1,
                            status: 'Error'
                        }
                    ]
                },
                resendOptions: {},
                setGridParams: jest.fn(),
                getGridData: jest.fn((params, callback) => callback([])),
                setResendOptions: jest.fn()
            };

            const expectedParams = {
                pageIndex: expectedPageIndex,
                pageSize: expectedPageSize
            };

            const dataGrid = shallow(<DataGrid {...props} />);
            functions.handleNewResendOptions = jest.fn(data => handleNewResendOptions(data, props.setResendOptions));

            // Action
            dataGrid.find(Table).props().onPaging(expectedPageIndex, expectedPageSize);
            
            // Assert
            expect(props.setGridParams).toHaveBeenCalledWith(expectedParams);
            expect(props.getGridData).toHaveBeenCalled();
            expect(props.setResendOptions).toHaveBeenCalled();
        });

        describe('buildHeader', () => {
            it('renders regular header', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {}
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader());
                
                // Assert
                expect(header.find(HeaderCell).at(0).props().colWidth).toBe(5);
                expect(header.find(HeaderCell).at(0).html()).toContain('SymbolNo');
                expect(header.find(HeaderCell).at(1).props().colWidth).toBe(15);
                expect(header.find(HeaderCell).at(1).find('div').at(0).html()).toContain('NotificationID');
                expect(header.find(HeaderCell).at(1).find('div').at(1).html()).toContain('MessageID');
                expect(header.find(HeaderCell).at(2).props().colWidth).toBe(30);
                expect(header.find(HeaderCell).at(2).html()).toContain('Sender');
                expect(header.find(HeaderCell).at(2).html()).toContain('NotificationType');
                expect(header.find(HeaderCell).at(3).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(3).html()).toContain('SendDateTimeGMTA8');
                expect(header.find(HeaderCell).at(4).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(4).html()).toContain('GroupHeadline');
                expect(header.find(HeaderCell).at(5).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(5).html()).toContain('Recipient');
                expect(header.find(HeaderCell).at(6).props().colWidth).toBe(30);
                expect(header.find(HeaderCell).at(6).html()).toContain('Title');
                expect(header.find(HeaderCell).at(6).html()).toContain('Content');
                expect(header.find(HeaderCell).at(7).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(7).html()).toContain('Status');
            });

            it('has action column', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {}
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader(true));
                
                // Assert
                expect(header.find(HeaderCell).at(8).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(8).html()).toContain('Action');
            });
            
            it('has resend column', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {}
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader(true));
                
                // Assert
                expect(header.find(HeaderCell).at(9).props().colWidth).toBe(10);
            });
            
            it('has disable resend', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {
                        isDisabledResend: true
                    }
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader(true));
                
                // Assert
                expect(header.find(HeaderCell).at(9).find('div').html()).toEqual('<div>Resend</div>');
            });
            
            it('has enable resend', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {
                        isDisabledResend: false
                    }
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader(true));
                
                // Assert
                expect(header.find(HeaderCell).at(9).find('div').html()).toContain('Resend');
                expect(header.find(HeaderCell).at(9).find('div').html()).toContain('class="link-click"');
            });
            
            it('has enable resend simulating onClick', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {
                        isDisabledResend: false
                    }
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
                dataGrid.instance().resendNotificationAll = jest.fn();
                const header = shallow(dataGrid.instance().buildHeader(true));
    
                // Action
                header.find(HeaderCell).at(9).find('div').simulate('click');

                // Assert
                expect(dataGrid.instance().resendNotificationAll).toHaveBeenCalled();
            });
            
            it('has resend all check box', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {
                        isHaveResendAll: true
                    }
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader(true));

                // Assert
                expect(header.find(HeaderCell).at(9).find(Form.Control).props().className).toBe('checkbox-cell-style margin-top-10');
                expect(header.find(HeaderCell).at(9).find(Form.Control).props().type).toBe('checkbox');
                expect(header.find(HeaderCell).at(9).find(Form.Control).props().checked).toEqual(props.resendOptions.isCheckedAll);
            });
            
            it('has resend all check box changing', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                    resendOptions: {
                        isHaveResendAll: true
                    }
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
                dataGrid.instance().handleChangeResendAllCheckbox = jest.fn();
                const header = shallow(dataGrid.instance().buildHeader(true));
    
                // Action
                header.find(HeaderCell).at(9).find(Form.Control).simulate('change');

                // Assert
                expect(dataGrid.instance().handleChangeResendAllCheckbox).toHaveBeenCalled();
            });
        });

        describe('buildBody', () => {
            function testBuildBodyNoData(isHost) {
                it('buildBody nodata', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        gridData: {
                            data: []
                        },
                        resendOptions: {}
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    const expectedBody = [<Row key={`body${1}`} className='no-data' ><Cell colSpan={isHost ? 10 : 8}>{'ThereIsNoData'}</Cell></Row>];
        
                    // Action
                    const body = dataGrid.instance().buildBody(isHost);
                    
                    // Assert
                    expect(body).toEqual(expectedBody);
                });
            }

            testBuildBodyNoData(true);
            testBuildBodyNoData(false);

            function renderRow(item, index, isHost) {
                return <div key={index}>{isHost? item.notificationId : item.status}</div>;
            }

            function testBuildBodyHasData(isHost) {
                it('buildBody has data', () => {
                    // Arrange
                    const props = {
                        gridParams: {
                            pageIndex: 1,
                            pageSize: 100
                        },
                        gridData: {
                            data: [
                                {
                                    notificationId: 1,
                                    status: 'Error'
                                }
                            ]
                        },
                        resendOptions: {}
                    };
                    const startIndex = (props.pageIndex - 1) * props.pageSize + 1;
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().renderRow = jest.fn(renderRow);
                    const expectedBody = props.gridData.data.map((item, index) => renderRow(item, index, isHost));
        
                    // Action
                    const body = dataGrid.instance().buildBody(isHost);
                    
                    // Assert
                    expect(body).toEqual(expectedBody);
                });
            }

            testBuildBodyHasData(true);
            testBuildBodyHasData(false);
        });

        describe('renderRow', () => {
            function testRenderRow(item, index, isHost) {
                it('render correctly row', () => {
                    // Arrange
                    const startIndex = 1;
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {
                            dataList: []
                        }
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().startIndex = startIndex;

                    const expectedRow = <Row key={index} >
                        <Cell className='text-center'>{index + startIndex}</Cell>
                        <Cell className='text-center'>
                            <div>{item.notificationId}</div>
                            <div>{item.messageId}</div>
                        </Cell>
                        <Cell>
                            <div>{item.sender}</div>
                            <div>{item.notificationTypeName}</div>
                        </Cell>
                        <Cell className='text-center'>
                            <div>{item.sendDateString}</div>
                            <div>{item.sendTimeString}</div>
                        </Cell>
                        <Cell className='text-center'>{getHeadlines(item.headlines)}</Cell>
                        <Cell className='text-center'>{getRecipients(item.recipients)}</Cell>
                        <Cell className='link-click' onClick={() => dataGrid.instance().openContentPopup(item)}>{item.title}</Cell>
                        <Cell className={`text-center status-font ${item.status.toLowerCase()}-color`}>{item.status}</Cell>
                        {
                            isHost &&
                            <Cell className='text-center'>
                                {
                                    (dataGrid.instance().isError(item.status) || dataGrid.instance().isResolved(item.status)) &&
                                        <div
                                            onClick={() => dataGrid.instance()(item)}
                                            className='action-style link-click'
                                        >{'ViewDetail'}</div>
                                }
                                {
                                    dataGrid.instance().isError(item.status) &&
                                    <div
                                        onClick={() => dataGrid.instance().validateResend(item.notificationId)}
                                        className='action-style link-click'
                                    >{'Resend'}</div>
                                }
                            </Cell>
                        }
                        {
                            isHost &&
                            <Cell className='text-center'>
                                { dataGrid.instance().isError(item.status) && <Form.Control className='checkbox-cell-style'
                                    id={`resend-${item.notificationId}-${item.messageId}`}
                                    type='checkbox'
                                    checked={props.resendOptions.dataList[index] && props.resendOptions.dataList[index].isResent}
                                    onChange={() => dataGrid.instance().handleChangeResendCheckbox(index)}
                                                                              />}
                            </Cell>
                        }
                    </Row>;

                    // Action
                    const result = dataGrid.instance().renderRow(item, index, isHost);

                    // Assert
                    expect(JSON.stringify(result)).toEqual(JSON.stringify(expectedRow));
                });
            }

            testRenderRow({ status: 'Error' }, 1, true);
            testRenderRow({ status: 'Resolved' }, 1, true);
            testRenderRow({ status: 'Draft' }, 1, false);
            testRenderRow({ status: 'Spent' }, 1, true);
            testRenderRow({ status: 'Spending' }, 1, false);
            testRenderRow({ status: 'Duplicated' }, 1, true);
            
            describe ('openContentPopup', () => {
                it('click title cell run openContentPopup', () => {
                    // Arrange
                    const item = { status: 'Error' };
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {
                            dataList: []
                        },
                        isHost: true
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().openContentPopup = jest.fn();
                    const row = shallow(dataGrid.instance().renderRow(item, 1, true));
    
                    // Action
                    row.find(Cell).at(6).simulate('click');
    
                    // Assert
                    expect(dataGrid.instance().openContentPopup).toHaveBeenCalledWith(item);
                });

                it('openContentPopup run setContentPopup', () => {
                    // Arrange
                    const item = { status: 'Error' };
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {},
                        setContentPopup: jest.fn()
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
    
                    // Action
                    dataGrid.instance().openContentPopup(item);
    
                    // Assert
                    expect(props.setContentPopup).toHaveBeenCalledWith({
                        show: true,
                        data: item
                    });
                });
            });
            
            describe('openViewDetailPopup', () => {
                it('click view detail run openViewDetailPopup', () => {
                    // Arrange
                    const item = { status: 'Error' };
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {
                            dataList: []
                        },
                        isHost: true
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().openViewDetailPopup = jest.fn();
                    const row = shallow(dataGrid.instance().renderRow(item, 1, true));
    
                    // Action
                    row.find(Cell).at(8).find('div').at(0).simulate('click');
    
                    // Assert
                    expect(dataGrid.instance().openViewDetailPopup).toHaveBeenCalledWith(item);
                });

                it('openViewDetailPopup run setViewDetailPopup', () => {
                    // Arrange
                    const item = { status: 'Error' };
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {},
                        setViewDetailPopup: jest.fn()
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
    
                    // Action
                    dataGrid.instance().openViewDetailPopup(item);
    
                    // Assert
                    expect(props.setViewDetailPopup).toHaveBeenCalledWith({
                        show: true,
                        data: item
                    });
                });
            });
            
            describe('validateResend', () => {
                it('click resend run validateResend', () => {
                    // Arrange
                    const item = { status: 'Error', notificationId: 1 };
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {
                            dataList: []
                        },
                        isHost: true
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().validateResend = jest.fn();
                    const row = shallow(dataGrid.instance().renderRow(item, 1, true));
    
                    // Action
                    row.find(Cell).at(8).find('div').at(1).simulate('click');
    
                    // Assert
                    expect(dataGrid.instance().validateResend).toHaveBeenCalledWith(item.notificationId);
                });

                it('validateResend run validateResend', () => {
                    // Arrange
                    const notificationId = 1;
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {},
                        validateResend: jest.fn()
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
    
                    // Action
                    dataGrid.instance().validateResend(notificationId);
    
                    // Assert
                    expect(props.validateResend).toHaveBeenCalledWith({
                        notificationIds: notificationId,
                        isSingle: true
                    });
                });
            });
            
            describe('handleChangeResendCheckbox', () => {
                it('change resend checkbox run handleChangeResendCheckbox', () => {
                    // Arrange
                    const item = { status: 'Error', notificationId: 1 };
                    const index = 1;
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {
                            dataList: []
                        },
                        isHost: true
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().handleChangeResendCheckbox = jest.fn();
                    const row = shallow(dataGrid.instance().renderRow(item, index, true));
    
                    // Action
                    row.find(Cell).at(9).find(Form.Control).simulate('change');
    
                    // Assert
                    expect(dataGrid.instance().handleChangeResendCheckbox).toHaveBeenCalledWith(index);
                });
                
                it('handleChangeResendCheckbox run setResendOptions', () => {
                    // Arrange
                    const index = 0;
                    const props = {
                        gridParams: {},
                        gridData: {},
                        resendOptions: {
                            dataList: [
                                {
                                    status: 'Error',
                                    isResent: true
                                }
                            ],
                            isHaveResendAll: true
                        },
                        setResendOptions: jest.fn()
                    };
        
                    const dataGrid = shallow(<DataGrid {...props} />);
                    const dataList = [
                        {
                            status: 'Error',
                            isResent: false
                        }
                    ];
    
                    // Action
                    dataGrid.instance().handleChangeResendCheckbox(0);
    
                    // Assert
                    expect(props.setResendOptions).toHaveBeenCalledWith({
                        isCheckedAll: false,
                        isDisabledResend: true,
                        isHaveResendAll: true,
                        shouldResetScrollPosition: false,
                        dataList
                    });
                });
            });
        });
    });
});