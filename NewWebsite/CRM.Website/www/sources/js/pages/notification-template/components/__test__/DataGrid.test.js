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
            it('should calls exactly functions', () => {
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
            it('should calls exactly functions', () => {
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
            it('should calls exactly functions', () => {
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
            it('should calls exactly functions', () => {
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
            const props = {
                gridParams: { pageSize: 100, pageIndex: 1 },
                gridData: {
                    totalItems: 1,
                    data: []
                }
            };
            //it('should render header correctly when isHost = false', () => {
            //    // Arrange
            //    const container = shallow(<DataGrid {...props} />).instance();
            //    const expected = JSON.stringify(<HeaderRow>
            //        <HeaderCell colWidth={5}>SymbolNo</HeaderCell>
            //        <HeaderCell colWidth={20}>NotificationType</HeaderCell>
            //        <HeaderCell colWidth={20}>TemplateName</HeaderCell>
            //        <HeaderCell colWidth={100}>Title/Content</HeaderCell>
            //    </HeaderRow>);

            //    // Act
            //    const header = container.buildHeader(false);

            //    // Assert
            //    expect(JSON.stringify(header)).toEqual(expected);
            //});

            //it('should render header correctly when isHost = true', () => {
            //    // Arrange
            //    const container = shallow(<DataGrid {...props} />).instance();
            //    const expected = JSON.stringify(<HeaderRow>
            //        <HeaderCell colWidth={5}>SymbolNo</HeaderCell>
            //        <HeaderCell colWidth={20}>NotificationType</HeaderCell>
            //        <HeaderCell colWidth={20}>TemplateName</HeaderCell>
            //        <HeaderCell colWidth={100}>Title/Content</HeaderCell>
            //        <HeaderCell colWidth={10}>Action</HeaderCell>
            //    </HeaderRow>);

            //    // Act
            //    const header = container.buildHeader(true);

            //    // Assert
            //    expect(JSON.stringify(header)).toEqual(expected);
            //});
        });
    });
});