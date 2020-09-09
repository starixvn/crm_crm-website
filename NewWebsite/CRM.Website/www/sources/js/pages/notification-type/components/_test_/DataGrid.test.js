import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { PAGING_SIZES } from '@jsroot/common/constants';

describe('DataGrid', () => {

    describe('component with redux', () => {

        beforeEach(() => {
            LangHelper.getResource = jest.fn(value => value);
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

        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            gridData: {},
        };

        const store = mockStore(initialState);
        const wrapper = shallow(<DataGridWrapper store={store} />);

        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.gridData).toEqual(initialState.gridData);
        });

        it('render null when gridData empty', () => {
            // Arrange
            const props = {
                gridParams: {},
                gridData: null,
            };

            // Action
            const dataGrid = shallow(<DataGrid {...props} />);

            // Assert
            expect(dataGrid).toBeNull;
        });

        describe('buildHeader', () => {

            it('title correct', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {
                        data: []
                    },
                };

                // Action
                const dataGrid = shallow(<DataGrid {...props} />);

                // Assert
                expect(dataGrid.find('div').html()).toEqual(`<div class="table-title">NotificationType</div>`);
            });

            it('contains table having correct props', () => {
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
                                recipientTypes: [],
                                chatTypes: []
                            }
                        ]
                    }
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
                expect(dataGrid.find(Table).props().containerPadding).toBe(0);
                expect(dataGrid.find(Table).props().adjustedHeight).toBe(50);
            });
        });

        describe('handle Edit', () => {
            it('handleEdit', () => {
                // Arrange
                const props = {
                    setEditNotificationTypePopup: jest.fn()
                };

                const dataGrid = shallow(<DataGrid {...props} />);

                // Action
                dataGrid.instance().handleEdit();

                // Assert
                expect(props.setEditNotificationTypePopup).toHaveBeenCalledTimes(1);
            });

            it('should call handleEdit when click edit notification type', () => {
                // Arrange
                const item = {
                    recipientTypes: [],
                    chatTypes: []
                };
                const props = {
                    gridParams: {},
                    gridData: {
                        data: [
                            {
                                recipientTypes: [],
                                chatTypes: []
                            }
                        ]
                    },
                    isHost: true
                };

                const dataGrid = shallow(<DataGrid {...props} />);
                dataGrid.instance().handleEdit = jest.fn();
                const row = shallow(dataGrid.instance().renderRow(item, 1, true));

                // Action
                row.find(Cell).at(5).find('span').at(0).simulate('click');

                // Assert
                expect(dataGrid.instance().handleEdit).toHaveBeenCalledWith(item);
            });

            it('should call setEditNotificationTypePopup when edit notification type', () => {
                // Arrange
                const item = {
                    recipientTypes: [],
                    chatTypes: []
                };
                const props = {
                    gridParams: {},
                    gridData: {
                        data: [
                            {
                                recipientTypes: [],
                                chatTypes: []
                            }
                        ]
                    },
                    setEditNotificationTypePopup: jest.fn()
                };

                const dataGrid = shallow(<DataGrid {...props} />);

                // Action
                dataGrid.instance().handleEdit(item);

                // Assert
                expect(props.setEditNotificationTypePopup).toHaveBeenCalledWith({ show: true, data: item });
            });
        });

        describe('handle Delete', () => {
            it('handleDelete', () => {
                // Arrange
                const props = {
                    setDeleteNotificationTypePopup: jest.fn()
                };

                const dataGrid = shallow(<DataGrid {...props} />);

                // Action
                dataGrid.instance().handleDelete();

                // Assert
                expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledTimes(1);
            });

            it('should call handleDelete when click delete notification type', () => {
                // Arrange
                const item = {
                    notificationTypeId: 1,
                    notificationTypeName: 'notification type name',
                    recipientTypes: [],
                    chatTypes: []
                };
                const props = {
                    gridParams: {},
                    gridData: {},
                    isHost: true
                };

                const dataGrid = shallow(<DataGrid {...props} />);
                dataGrid.instance().handleDelete = jest.fn();
                const row = shallow(dataGrid.instance().renderRow(item, 1, true));

                // Action
                row.find(Cell).at(5).find('span').at(1).simulate('click');

                // Assert
                expect(dataGrid.instance().handleDelete).toHaveBeenCalledWith(item.notificationTypeId, item.notificationTypeName);
            });

            it('should call setDeleteNotificationTypePopup when delete notification type', () => {
                // Arrange
                const item = {
                    id: 1,
                    name: 'notification type name',
                    recipientTypes: [],
                    chatTypes: []
                };
                const props = {
                    gridParams: {},
                    gridData: {},
                    setDeleteNotificationTypePopup: jest.fn()
                };

                const dataGrid = shallow(<DataGrid {...props} />);

                // Action
                dataGrid.instance().handleDelete(item.id, item.name);

                // Assert
                expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledWith({
                    show: true,
                    notificationTypeId: item.id,
                    name: item.name
                });

            });
        });

        describe('buildHeader', () => {
            it('onPaging', () => {
                // Arrange
                const expectedPageIndex = 1;
                const expectedPageSize = 100;
                const props = {
                    gridParams: {},
                    gridData: {
                        data: [
                            {
                                recipientTypes: [],
                                chatTypes: []
                            }
                        ]
                    },
                    setGridParams: jest.fn(),
                    getGridData: jest.fn()
                };

                const expectedParams = {
                    pageIndex: expectedPageIndex,
                    pageSize: expectedPageSize
                };

                const dataGrid = shallow(<DataGrid {...props} />);

                // Action
                dataGrid.find(Table).props().onPaging(expectedPageIndex, expectedPageSize);

                // Assert
                expect(props.setGridParams).toHaveBeenCalledWith(expectedParams);
                expect(props.getGridData).toHaveBeenCalledTimes(1);
            });

            it('buildHeader render correct header', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {}
                };

                const dataGrid = shallow(<DataGrid {...props} />);

                // Action
                const header = shallow(dataGrid.instance().buildHeader());

                // Assert
                expect(header.find(HeaderCell).at(0).props().colWidth).toBe(5);
                expect(header.find(HeaderCell).at(0).html()).toContain('SymbolNo');
                expect(header.find(HeaderCell).at(1).props().colWidth).toBe(20);
                expect(header.find(HeaderCell).at(1).html()).toContain('NotificationType');
                expect(header.find(HeaderCell).at(2).props().colWidth).toBe(20);
                expect(header.find(HeaderCell).at(2).html()).toContain('RecipientType');
                expect(header.find(HeaderCell).at(3).props().colWidth).toBe(20);
                expect(header.find(HeaderCell).at(3).html()).toContain('ChatType');
                expect(header.find(HeaderCell).at(4).props().colWidth).toBe(20);
                expect(header.find(HeaderCell).at(4).html()).toContain('System');
            });

            it('should show action when isHost true', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {}
                };
    
                const dataGrid = shallow(<DataGrid {...props} />);
    
                // Action
                const header = shallow(dataGrid.instance().buildHeader(true));
                
                // Assert
                expect(header.find(HeaderCell).at(5).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(5).html()).toContain('Action');
            });
        });

        describe('buildBody', () => {
            it('build body no data', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {
                        data: []
                    },
                };
                const isHost = true;

                const dataGrid = shallow(<DataGrid {...props} />);
                const expectedBody = [<Row key={`body${1}`} className="no-data"><Cell colSpan={isHost ? 6 : 5}>ThereIsNoData</Cell></Row>];

                // Action
                const body = dataGrid.instance().buildBody(isHost);

                // Assert
                expect(body).toEqual(expectedBody);
            });

            function renderRow(item, index, isHost) {
                return <div key={index}>{isHost}</div>;
            }

            function testBodyHasData(isHost) {
                it('build body has data', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        gridData: {
                            data: [
                                {
                                    recipientTypes: [],
                                    chatTypes: []
                                }
                            ]
                        }
                    };

                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().renderRow = jest.fn(renderRow);
                    const expectedBody = props.gridData.data.map((item, index) => renderRow(item, index, isHost));

                    // Action
                    const body = dataGrid.instance().buildBody(isHost);

                    // Assert
                    expect(body).toEqual(expectedBody);
                });
            }

            testBodyHasData(true);
            testBodyHasData(false);
        });

        describe('renderRow', () => {
            function RenderRow(item, index, isHost) {
                it('render correctly row', () => {
                    // Arrange
                    const startIndex = 1;
                    const props = {
                        gridParams: {},
                        gridData: {
                            data: [
                                {
                                    recipientTypes: [],
                                    chatTypes: []
                                }
                            ]
                        }
                    };

                    const dataGrid = shallow(<DataGrid {...props} />);
                    dataGrid.instance().startIndex = startIndex;
                    const expectedRow =
                        <Row key={item.notificationTypeId} >
                            <Cell className="text-center">{index + startIndex}</Cell>
                            <Cell>{item.notificationTypeName}</Cell>
                            <Cell>{item.recipientTypes.map(x => x.recipientTypeName).join(', ')}</Cell>
                            <Cell>{item.chatTypes.map(x => x.chatTypeName).join(', ')}</Cell>
                            <Cell>{item.systemName}</Cell>
                            {
                                isHost &&
                                <Cell className="text-center">
                                    <span className="rounded-circle fa fa-pencil icon-edit"
                                        onClick={() => this.handleEdit(item)}
                                    />
                                    <span className="rounded-circle fa fa-trash icon-delete"
                                        onClick={() => this.handleDelete(item.notificationTypeId, item.notificationTypeName)}
                                    />
                                </Cell>
                            }
                        </Row>

                    // Action
                    const result = dataGrid.instance().renderRow(item, index, isHost);

                    // Assert
                    expect(JSON.stringify(result)).toEqual(JSON.stringify(expectedRow));
                });
            }

            RenderRow({ recipientTypes: [], chatTypes: [] }, 1, true);
            RenderRow({ recipientTypes: [], chatTypes: [] }, 1, false);
        });
    });
});