import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
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

        it('render null when gridData invalid', () => {
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

            it('render title correctly', () => {
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

            it('table props should be mapped correctly', () => {
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
            it('should call correct functions  ', () => {
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

            it('should call handleEdit when click icon edit notification type', () => {
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
            describe('onPaging', () => {
            it('should call correct functions', () => {
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
        });

            it('should show action when isHost = true', () => {
                // Arrange
                const dataGrid = shallow(<DataGrid />).instance();

                // Action
                const header = shallow(dataGrid.buildHeader(true));

                // Assert
                expect(header.find(HeaderCell).length).toBe(6);
                expect(header.find(HeaderCell).at(5).props().colWidth).toBe(10);
                expect(header.find(HeaderCell).at(5).html()).toContain('Action');
            });

            it('should render expected header when isHost = false', () => {
                // Arrange
                const container = shallow(<DataGrid />).instance();

                // Act
                const header = shallow(container.buildHeader(false));

                // Assert
                expect(header.find(HeaderCell).length).toBe(5);
            });
        });

        describe('buildBody', () => {
            it('should render no data when gridData is empty', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {
                        data: []
                    },
                };
                const isHost = true;

                const dataGrid = shallow(<DataGrid {...props} />);
                const expectedBody = [<Row key={`body${1}`} className="no-data"><Cell colSpan={6}>ThereIsNoData</Cell></Row>];

                // Action
                const body = dataGrid.instance().buildBody(isHost);

                // Assert
                expect(body).toEqual(expectedBody);
            });

            function renderRow(item, index, isHost) {
                return <div key={index}>{isHost}</div>;
            }

            function testBodyHasData(isHost) {
                it('should render no data when gridData has data', () => {
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
            it('should render expected row when isHost = true', () => {
                // Arrange
                const index = 1;
                const startIndex = 1;
                const props = {
                    gridParams: {},
                    gridData: {}
                };
                const item = {
                    notificationTypeName: 'test',
                    recipientTypes: [{ recipientTypeName: 'name1' }, { recipientTypeName: 'name2' }],
                    chatTypes: [{ chatTypeName: 'name3' }, { chatTypeName: 'name4' }],
                    systemName: 'test'
                }

                const dataGrid = shallow(<DataGrid {...props} />);
                dataGrid.instance().startIndex = startIndex;

                // Action
                const row = shallow(dataGrid.instance().renderRow(item, index, true));

                // Assert
                expect(row.find(Cell).length).toBe(6);
                expect(row.find(Cell).at(0).props().className).toBe('text-center');
                expect(row.find(Cell).at(0).html()).toContain(index + startIndex);
                expect(row.find(Cell).at(1).html()).toContain(item.notificationTypeName);
                expect(row.find(Cell).at(2).html()).toContain(item.recipientTypes.map(x => x.recipientTypeName).join(', '));
                expect(row.find(Cell).at(3).html()).toContain(item.chatTypes.map(x => x.chatTypeName).join(', '));
                expect(row.find(Cell).at(4).html()).toContain(item.systemName);
                expect(row.find(Cell).at(5).props().className).toBe('text-center');
                expect(row.find(Cell).at(5).find('span').at(0).props().className).toBe('rounded-circle fa fa-pencil icon-edit');
                expect(row.find(Cell).at(5).find('span').at(1).props().className).toBe('rounded-circle fa fa-trash icon-delete');
            });

            it('should render expected row when isHost = false', () => {
                // Arrange
                const index = 1;
                const startIndex = 1;
                const props = {
                    gridParams: {},
                    gridData: {}
                };
                const item = {
                    notificationTypeName: 'test',
                    recipientTypes: [{ recipientTypeName: 'name1' }, { recipientTypeName: 'name2' }],
                    chatTypes: [{ chatTypeName: 'name3' }, { chatTypeName: 'name4' }],
                    systemName: 'test'
                }

                const dataGrid = shallow(<DataGrid {...props} />);
                dataGrid.instance().startIndex = startIndex;

                // Action
                const row = shallow(dataGrid.instance().renderRow(item, index, false));

                // Assert
                expect(row.find(Cell).length).toBe(5);
                expect(row.find(Cell).at(0).props().className).toBe('text-center');
                expect(row.find(Cell).at(0).html()).toContain(index + startIndex);
                expect(row.find(Cell).at(1).html()).toContain(item.notificationTypeName);
                expect(row.find(Cell).at(2).html()).toContain(item.recipientTypes.map(x => x.recipientTypeName).join(', '));
                expect(row.find(Cell).at(3).html()).toContain(item.chatTypes.map(x => x.chatTypeName).join(', '));
                expect(row.find(Cell).at(4).html()).toContain(item.systemName);
            });
        });
    });
});