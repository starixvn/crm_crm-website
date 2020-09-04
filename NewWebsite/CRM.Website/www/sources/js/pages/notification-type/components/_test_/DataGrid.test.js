import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import LangHelper from '@jsroot/common/langHelper';
import { HeaderRow, HeaderCell, Row, Cell } from '@jsroot/components/react-grid/index';

describe('DataGrid', () => {

    describe('component with redux', () => {

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
            expect(dataGrid.find('div').html()).toEqual(`<div class="table-title">${LangHelper.getResource('NotificationType')}</div>`);
        });

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

        describe('buildHeader', () => {
            it('buildHeader render correct header', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    gridData: {},
                };
                const isHost = true;

                const dataGrid = shallow(<DataGrid {...props} />);
                const expectedHeader =
                    <HeaderRow>
                        <HeaderCell colWidth={5}>{LangHelper.getResource('SymbolNo')}</HeaderCell>
                        <HeaderCell colWidth={20}>{LangHelper.getResource('NotificationType')}</HeaderCell>
                        <HeaderCell colWidth={20}>{LangHelper.getResource('RecipientType')}</HeaderCell>
                        <HeaderCell colWidth={20}>{LangHelper.getResource('ChatType')}</HeaderCell>
                        <HeaderCell colWidth={20}>{LangHelper.getResource('System')}</HeaderCell>
                        {
                            isHost && <HeaderCell colWidth={10}>{LangHelper.getResource('Action')}</HeaderCell>
                        }
                    </HeaderRow>;

                // Action
                const header = dataGrid.instance().buildHeader(isHost);

                // Assert
                expect(header).toEqual(expectedHeader);
            });
        });

        it('buildBody', () => {
            // Arrange
            const props = {
                gridParams: {},
                gridData: {
                    data: []
                },
            };
            const isHost = true;

            const dataGrid = shallow(<DataGrid {...props} />);
            const expectedBody = [<Row key={`body${1}`} className="no-data"><Cell colSpan={isHost ? 6 : 5}>{LangHelper.getResource('ThereIsNoData')}</Cell></Row>];

            // Action
            const body = dataGrid.instance().buildBody(isHost);

            // Assert
            expect(body).toEqual(expectedBody);
        });
    });
});