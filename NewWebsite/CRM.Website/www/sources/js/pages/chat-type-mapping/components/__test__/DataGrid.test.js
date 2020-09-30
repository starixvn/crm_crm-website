import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import LangHelper from '@jsroot/common/langHelper';
import { Table, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';
import { PAGING_SIZES } from '@jsroot/common/constants';

describe('DataGrid', () => {
    LangHelper.getResource = jest.fn(value => value);

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            gridData: {},
            updateActiveStatusPopup: {},
            updateActiveStatusResult: {},
            updatePersonalMappingData: {},
            updateGroupMappingData: {}
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
            expect(wrapper.props().children.props.updateActiveStatusPopup).toEqual(initialState.updateActiveStatusPopup);
            expect(wrapper.props().children.props.updateActiveStatusResult).toEqual(initialState.updateActiveStatusResult);
            expect(wrapper.props().children.props.updatePersonalMappingData).toEqual(initialState.updatePersonalMappingData);
            expect(wrapper.props().children.props.updateGroupMappingData).toEqual(initialState.updateGroupMappingData);
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
                resendOptions: {},
                updateActiveStatusPopup: {},
                updateActiveStatusResult: {},
                updatePersonalMappingData: {},
                updateGroupMappingData: {}
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
        it('has correct first div', () => {
            // Arrange
            const props = {
                gridParams: {},
                gridData: {
                    data: [
                        {
                            chatType: 'Personal',
                            botName: 'Bot1',
                            recipients: [],
                            headlines: []
                        }
                    ]
                },
                updateActiveStatusPopup: {},
                updateActiveStatusResult: {},
                updatePersonalMappingData: {},
                updateGroupMappingData: {}
            };

            // Action
            const dataGrid = shallow(<DataGrid {...props} />);

            // Assert
            expect(dataGrid.find('div').first().props().className).toEqual('table-title');
            expect(dataGrid.find('div').first().html()).toContain('ChatTypeMapping');
        });

        it('contains Table having correct props', () => {
            // Arrange
            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                gridData: {
                    totalItems: 1000,
                    data: [
                        {
                            chatType: 'Personal',
                            botName: 'Bot1',
                            recipients: [],
                            headlines: []
                        }
                    ]
                },
                updateActiveStatusPopup: {},
                updateActiveStatusResult: {},
                updatePersonalMappingData: {},
                updateGroupMappingData: {}
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
            expect(dataGrid.find(Table).props().minWidth).toBe(960);
            expect(dataGrid.find(Table).props().autoWidth).toBeTrue;
            expect(dataGrid.find(Table).props().isPaging).toBeTrue;
            expect(dataGrid.find(Table).props().pageOption).toEqual(pagingOption);
            expect(dataGrid.find(Table).props().containerPadding).toEqual(0);
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
                            chatType: 'Personal',
                            botName: 'Bot1',
                            recipients: [],
                            headlines: []
                        }
                    ]
                },
                setGridParams: jest.fn(),
                getGridData: jest.fn(),
                updateActiveStatusPopup: {},
                updateActiveStatusResult: {},
                updatePersonalMappingData: {},
                updateGroupMappingData: {}
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
            expect(props.getGridData).toHaveBeenCalled();
        });

        describe('buildBody', () => {
            function testBuildBodyNoData(isHost, colSpan) {
                it('buildBody nodata', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        gridData: {
                            data: []
                        },
                        updateActiveStatusPopup: {},
                        updateActiveStatusResult: {},
                        updatePersonalMappingData: {},
                        updateGroupMappingData: {}
                    };

                    const dataGrid = shallow(<DataGrid {...props} />);
                    const expectedBody = [<Row key={`body${1}`} className='no-data' ><Cell colSpan={colSpan}>{'ThereIsNoData'}</Cell></Row>];

                    // Action
                    const body = dataGrid.instance().buildBody(isHost);

                    // Assert
                    expect(body).toEqual(expectedBody);
                });
            }

            testBuildBodyNoData(true, 13);
            testBuildBodyNoData(false, 12);

            function renderRow(item, index, isHost) {
                return <div key={index}>{isHost ? item.notificationId : item.status}</div>;
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
                                    chatType: 'Personal',
                                    botName: 'Bot1',
                                    recipients: [],
                                    headlines: []
                                }
                            ]
                        },
                        updateActiveStatusPopup: {},
                        updateActiveStatusResult: {},
                        updatePersonalMappingData: {},
                        updateGroupMappingData: {}
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

            testBuildBodyHasData(true);
            testBuildBodyHasData(false);
        });
    });
});