import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { PAGING_SIZES } from '@jsroot/common/constants';

describe('DataGrid', () => {
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

    it('component render correctly', () => {
        // Arrange
        const mockStore = configureMockStore();
        const store = mockStore();

        // Atc
        const wrapper = shallow(<DataGridWrapper store={store} />);

        // Assert
        expect(wrapper).toMatchSnapshot();
    });

    it('render null when notificationTypeGridData invalid', () => {
        // Arrange
        const props = {
            notificationTypeGridParams: {},
            notificationTypeGridData: null,
        };

        // Action
        const dataGrid = shallow(<DataGrid {...props} />);

        // Assert
        expect(dataGrid).toBeNull;
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
                notificationTypeGridParams: {},
                notificationTypeGridData: {
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
                notificationTypeGridParams: {},
                notificationTypeGridData: {
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
        it('should call handleDelete when click delete notification type', () => {
            // Arrange
            const item = {
                notificationTypeId: 1,
                notificationTypeName: 'notification type name',
                recipientTypes: [],
                chatTypes: []
            };
            const props = {
                notificationTypeGridParams: {},
                notificationTypeGridData: {},
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
                notificationTypeGridParams: {},
                notificationTypeGridData: {},
                setDeleteNotificationTypePopup: jest.fn()
            };
            const dataGrid = shallow(<DataGrid {...props} />);

            // Action
            dataGrid.instance().handleDelete(item.id, item.name);

            // Assert
            expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledTimes(1);
            expect(props.setDeleteNotificationTypePopup).toHaveBeenCalledWith({
                show: true,
                notificationTypeId: item.id,
                name: item.name
            });
        });
    });

    describe('onPaging', () => {
        it('should call correct functions', () => {
            // Arrange
            const expectedPageIndex = 1;
            const expectedPageSize = 100;
            const props = {
                notificationTypeGridParams: {},
                notificationTypeGridData: {
                    data: [
                        {
                            recipientTypes: [],
                            chatTypes: []
                        }
                    ]
                },
                setNotificationTypeGridParams: jest.fn(),
                getNotificationTypeGridData: jest.fn()
            };

            const expectedParams = {
                pageIndex: expectedPageIndex,
                pageSize: expectedPageSize
            };

            const dataGrid = shallow(<DataGrid {...props} />);

            // Action
            dataGrid.find(Table).props().onPaging(expectedPageIndex, expectedPageSize);

            // Assert
            expect(props.setNotificationTypeGridParams).toHaveBeenCalledWith(expectedParams);
            expect(props.getNotificationTypeGridData).toHaveBeenCalledTimes(1);
        });
    });

    describe('buildNotificationTypeHeader', () => {
        it('render title correctly', () => {
            // Arrange
            const props = {
                notificationTypeGridParams: {},
                notificationTypeGridData: {
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
                notificationTypeGridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                notificationTypeGridData: {
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
                PageIndex: props.notificationTypeGridParams.pageIndex,
                PageSize: props.notificationTypeGridParams.pageSize,
                TotalItem: props.notificationTypeGridData.totalItems,
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
            expect(dataGrid.find(Table).props().adjustedHeight).toBe(10);
        });

        it('should show action when isHost = true', () => {
            // Arrange
            const dataGrid = shallow(<DataGrid />).instance();

            // Action
            const header = shallow(dataGrid.buildNotificationTypeHeader(true));

            // Assert
            expect(header.find(HeaderCell).length).toBe(6);
            expect(header.find(HeaderCell).at(5).props().colWidth).toBe(10);
            expect(header.find(HeaderCell).at(5).html()).toContain('Action');
        });

        it('should render expected header when isHost = false', () => {
            // Arrange
            const container = shallow(<DataGrid />).instance();

            // Act
            const header = shallow(container.buildNotificationTypeHeader(false));

            // Assert
            expect(header.find(HeaderCell).length).toBe(5);
        });
    });

    describe('buildNotificationTypeBody', () => {
        it('should render no data when notificationTypeGridData is empty', () => {
            // Arrange
            const props = {
                notificationTypeGridParams: {},
                notificationTypeGridData: {
                    data: []
                },
            };
            const isHost = true;

            const dataGrid = shallow(<DataGrid {...props} />);
            const expectedBody = [<Row key={`body${1}`} className="no-data"><Cell colSpan={6}>ThereIsNoData</Cell></Row>];

            // Action
            const body = dataGrid.instance().buildNotificationTypeBody(isHost);

            // Assert
            expect(body).toEqual(expectedBody);
        });

        function renderRow(item, index, isHost) {
            return <div key={index}>{isHost}</div>;
        }

        function testBodyHasData(isHost) {
            it('should render no data when notificationTypeGridData has data', () => {
                // Arrange
                const props = {
                    notificationTypeGridParams: {},
                    notificationTypeGridData: {
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
                const expectedBody = props.notificationTypeGridData.data.map((item, index) => renderRow(item, index, isHost));

                // Action
                const body = dataGrid.instance().buildNotificationTypeBody(isHost);

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
                notificationTypeGridParams: {},
                notificationTypeGridData: {}
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
                notificationTypeGridParams: {},
                notificationTypeGridData: {}
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