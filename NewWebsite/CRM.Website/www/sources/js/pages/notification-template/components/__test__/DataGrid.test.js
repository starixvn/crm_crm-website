import React from 'react';
import { DataGrid } from '../DataGrid';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { PAGING_SIZES } from '@jsroot/common/constants';
import LangHelper from '@jsroot/common/langHelper';
import { shallow } from 'enzyme';

describe('DataGrid', () => {
    beforeAll(() => {
        LangHelper.getResource = jest.fn((key) => key);

        const appContainer = document.createElement('div');
        appContainer.setAttribute('id', 'app-container');
        document.body.appendChild(appContainer);
    });

    afterAll(() => {
        const appContainer = document.getElementById('app-container');
        if (appContainer) {
            document.body.removeChild(appContainer);
        }

        jest.clearAllMocks();
    });

    function testInvalidGridData(gridData) {
        it('renders null when gridData is invalid', () => {
            // Arrange
            const props = {
                gridData
            };

            // Act
            const container = shallow(<DataGrid {...props} />);

            // Assert
            expect(container).toBeNull;
        });
    }

    testInvalidGridData(null);
    testInvalidGridData({});
    testInvalidGridData({ data: null });

    describe('default elements', () => {
        const props = {
            gridParams: { pageSize: 100, pageIndex: 1 },
            gridData: {
                totalItems: 1,
                data: []
            }
        };

        it('renders table title correctly', () => {
            // Act
            const container = shallow(<DataGrid {...props} />);

            // Assert
            expect(LangHelper.getResource).toHaveBeenCalledWith('NotificationTemplate');
            expect(container.find('div').html()).toEqual('<div class="table-title">NotificationTemplate</div>');
        });

        it('Table props should be mapped correctly', () => {
            // Arrange
            const pagingOption = {
                PageIndex: 1,
                PageSize: 100,
                TotalItem: 1,
                PageList: PAGING_SIZES
            };
            const container = shallow(<DataGrid {...props} />);

            // Act
            const table = container.find(Table);

            // Assert
            expect(table.length).toBe(1);
            expect(table.props().minWidth).toBe(720);
            expect(table.props().autoWidth).toBeTrue;
            expect(table.props().isPaging).toBeTrue;
            expect(table.props().pageOption).toEqual(pagingOption);
            expect(table.props().containerPadding).toBe(0);
            expect(table.props().adjustedHeight).toBe(10);
        });
    });

    describe('methods', () => {
        describe('onPaging', () => {
            it('should call exactly functions', () => {
                // Arrange
                const props = {
                    gridParams: { pageSize: 100, pageIndex: 1 },
                    gridData: {
                        totalItems: 1,
                        data: []
                    },
                    setGridParams: jest.fn(),
                    getGridData: jest.fn()
                };
                const expectedGridParams = { pageIndex: 1, pageSize: 100 };
                const container = shallow(<DataGrid {...props} />).instance();

                // Act
                container.onPaging(expectedGridParams.pageIndex, expectedGridParams.pageSize);

                // Assert
                expect(props.setGridParams).toHaveBeenCalledWith(expectedGridParams);
                expect(props.getGridData).toHaveBeenCalledWith(expectedGridParams);
            });
        });

        describe('handleEdit', () => {
            it('should call exactly functions', () => {
                // Arrange
                const props = {
                    gridParams: { pageSize: 100, pageIndex: 1 },
                    gridData: {
                        totalItems: 1,
                        data: []
                    },
                    getTemplateById: jest.fn(),
                    setEditTemplatePopup: jest.fn()
                };
                const container = shallow(<DataGrid {...props} />).instance();

                // Act
                container.handleEdit(1);

                // Assert
                expect(props.getTemplateById).toHaveBeenCalledWith(1, expect.any(Function));
            });
        });

        describe('handleViewContent', () => {
            it('should call exactly functions', () => {
                // Arrange
                const props = {
                    gridParams: { pageSize: 100, pageIndex: 1 },
                    gridData: {
                        totalItems: 1,
                        data: []
                    },
                    getTemplateById: jest.fn(),
                    setContentDetailPopup: jest.fn()
                };
                const container = shallow(<DataGrid {...props} />).instance();

                // Act
                container.handleViewContent(1);

                // Assert
                expect(props.getTemplateById).toHaveBeenCalledWith(1, expect.any(Function));
            });
        });

        describe('handleDelete', () => {
            it('should call exactly functions', () => {
                // Arrange
                const props = {
                    gridParams: { pageSize: 100, pageIndex: 1 },
                    gridData: {
                        totalItems: 1,
                        data: []
                    },
                    setDeleteTemplatePopup: jest.fn()
                };
                const container = shallow(<DataGrid {...props} />).instance();

                // Act
                container.handleDelete(1, 'voiding abnormal bet');

                // Assert
                expect(props.setDeleteTemplatePopup).toHaveBeenCalledWith({ show: true, templateId: 1, name: 'voiding abnormal bet' });
            });
        });

        describe('buildHeader', () => {
            it('should render expected header when isHost = false', () => {
                // Arrange
                const container = shallow(<DataGrid />).instance();

                // Act
                const header = shallow(container.buildHeader(false));

                // Assert
                expect(header.find(HeaderCell).length).toBe(4);
            });

            it('should render expected header when isHost = true', () => {
                // Arrange
                // Arrange
                const container = shallow(<DataGrid />).instance();

                // Act
                const header = shallow(container.buildHeader(true));

                // Assert
                expect(header.find(HeaderCell).length).toBe(5);
            });
        });

        describe('buildBody', () => {
            it('should render no data when gridData is empty', () => {
                // Arrange
                const props = {
                    gridParams: { pageSize: 100, pageIndex: 1 },
                    gridData: {
                        totalItems: 1,
                        data: []
                    }
                };
                const container = shallow(<DataGrid {...props} />).instance();

                // Act
                const actual = container.buildBody(true);

                // Assert
                expect(actual).toEqual([<Row key={`body${1}`} className="no-data"><Cell colSpan="5">ThereIsNoData</Cell></Row>]);
            });

            it('should render expected rows when gridData has data', () => {
                // Arrange
                const props = {
                    gridParams: { pageSize: 100, pageIndex: 1 },
                    gridData: {
                        totalItems: 1,
                        data: [{ notificationTemplateId: 1 }]
                    }
                };
                const container = shallow(<DataGrid {...props} />).instance();
                container.renderRow = jest.fn(() => <div />);

                // Act
                const actual = container.buildBody(true);

                // Assert
                expect(actual).toEqual([<div />]);
            });
        });

        describe('renderRow', () => {
            it('should render expected row when isHost = false', () => {
                // Arrange
                const item = {
                    notificationTemplateId: 1, notificationTypeName: 'type',
                    notificationTemplateName: 'name', title: 'title'
                };
                const container = shallow(<DataGrid />).instance();

                // Act
                const row = shallow(container.renderRow(item, 1, false));

                // Assert
                expect(row.find(Cell).length).toBe(4);
                //expect(row).toEqual(<Row key={1}>
                //    <Cell className="text-center">{1}</Cell>
                //    <Cell>type</Cell>
                //    <Cell>name</Cell>
                //    <Cell><a href="#" onClick={() => container.handleViewContent(1)}>title</a></Cell>
                //</Row>);
            });

            it('should render expected row when isHost = true', () => {
                // Arrange
                const item = {
                    notificationTemplateId: 1, notificationTypeName: 'type',
                    notificationTemplateName: 'name', title: 'title'
                };
                const container = shallow(<DataGrid />).instance();

                // Act
                const row = shallow(container.renderRow(item, 1, true));

                // Assert
                expect(row.find(Cell).length).toBe(5);
                //expect(row).toEqual(<Row key={1}>
                //    <Cell className="text-center">{1}</Cell>
                //    <Cell>type</Cell>
                //    <Cell>name</Cell>
                //    <Cell><a href="#" onClick={() => container.handleViewContent(1)}>title</a></Cell>
                //</Row>);
            });
        });
    });
});