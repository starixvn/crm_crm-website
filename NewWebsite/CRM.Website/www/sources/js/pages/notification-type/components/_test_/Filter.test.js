
import React from 'react';
import FilterWrapper, { Filter } from '../Filter';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import { Button } from 'react-bootstrap';

describe('Filter', () => {

    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            settings: {},
            getSettings: {},
            invalidParams: {},
        };

        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<FilterWrapper store={store} />);

        // Assert
        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.invalidParams).toEqual(initialState.invalidParams);
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.invalidParams).toEqual(initialState.invalidParams);
        });

        it('component has correct initialState', () => {
            // Arrange
            const initialState = {
                filter: {
                    notificationTypeName: '',
                    recipientTypes: '',
                    chatTypes: '',
                    systems: ''
                },
                recipientTypesOptions: [],
                chatTypesOptions: [],
                systemsOptions: []
            };

            const props = {
                gridParams: {},
                settings: {},
                invalidParams: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            // Action
            const filter = shallow(<Filter {...props} />);

            // Assert
            expect(filter.state()).toEqual(initialState);
        });

        it('componentDidMount runs correctly', async () => {
            // Arrange
            const props = {
                gridParams: {},
                settings: {},
                invalidParams: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            // Action
            shallow(<Filter {...props} />);

            // Assert
            expect(props.getSettings).toHaveBeenCalledTimes(1);
        });

        it('should show create popup when click insert button', () => {
            // Arrange
            const props = {
                isHost: true,
                setCreateNotificationTypePopup: jest.fn(),
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
            const container = shallow(<Filter {...props} />);

            // Act
            container.find(Button).at(1).props().onClick();

            // Assert
            expect(props.setCreateNotificationTypePopup).toHaveBeenCalledWith({ show: true });
        });

        describe('handle change', () => {
            it('handleFilterChange runs correctly', () => {
                // Arrange
                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const filter = shallow(<Filter {...props} />);

                // Action
                filter.instance().handleFilterChange('name', 'value');

                // Assert
                expect(filter.instance().state.filter.name).toBe('value');
            });

            it('handleRecipientTypesChange call handleFilterChange', () => {
                // Arrange
                const expected = 'selectedItemsKey';
                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const filter = shallow(<Filter {...props} />);
                filter.instance().handleFilterChange = jest.fn();

                // Action
                filter.instance().recipientNotificationTypeChange(null, expected);

                // Assert
                expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('recipientTypes', expected);
            });

            it('chatNotificationTypeChange call handleFilterChange', () => {
                // Arrange
                const expected = 'selectedItemsKey';
                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const filter = shallow(<Filter {...props} />);
                filter.instance().handleFilterChange = jest.fn();

                // Action
                filter.instance().chatNotificationTypeChange(null, expected);

                // Assert
                expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('chatTypes', expected);
            });

            it('systemChange call handleFilterChange', () => {
                // Arrange
                const expected = 'selectedItemsKey';
                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const filter = shallow(<Filter {...props} />);
                filter.instance().handleFilterChange = jest.fn();

                // Action
                filter.instance().systemChange(null, expected);

                // Assert
                expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('systems', expected);
            });

            it('handle NotificationTypeName change runs correctly', () => {
                // Arrange
                const event = {
                    notificationTypeName: 'test'
                };

                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const filter = shallow(<Filter {...props} />);
                filter.instance().handleNotificationTypeChange = jest.fn();

                // Action
                filter.instance().handleNotificationTypeChange(event);

                // Assert
                expect(filter.instance().handleNotificationTypeChange).toHaveBeenCalledWith(event);
            });
        });

        describe('getParams', () => {
            it('getParams with valid filter', () => {
                // Arrange
                const state = {
                    filter: {
                        notificationTypeName: 'test',
                        recipientTypeIds: '1,2',
                        chatTypeIds: '1,2',
                        systemId: '1,2',
                    }
                };

                const props = {
                    gridParams: {
                        pageIndex: 1,
                        pageSize: 100
                    },
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const expected = {
                    notificationTypeName: state.notificationTypeName,
                    chatTypeIds: state.filter.chatTypes,
                    recipientTypeIds: state.filter.recipientTypes,
                    systemId: state.filter.systems,
                    pageIndex: 1,
                    pageSize: 100
                };

                const filter = shallow(<Filter {...props} />);
                filter.instance().setState(state);

                // Action
                const result = filter.instance().getParams();

                // Assert
                expect(result).toEqual(expected);
            });

            it('getParams with invalid filter', () => {
                // Arrange
                const state = {
                    filter: {
                        notificationTypeName: 'test',
                        recipientTypeIds: null,
                        chatTypeIds: null,
                        systemId: null,
                    }
                };

                const props = {
                    gridParams: {
                        pageIndex: 1,
                        pageSize: 100
                    },
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                };

                const expected = {
                    notificationTypeName: state.notificationTypeName,
                    chatTypeIds: state.filter.chatTypes,
                    recipientTypeIds: state.filter.recipientTypes,
                    systemId: state.filter.systems,
                    pageIndex: 1,
                    pageSize: 100
                };

                const filter = shallow(<Filter {...props} />);
                filter.instance().setState(state);

                // Action
                const result = filter.instance().getParams();

                // Assert
                expect(result).toEqual(expected);
            });
        });

        describe('submitHandler', () => {
            it('submitHandler runs with params correct', () => {
                // Arrange
                const params = { value: 'value' };
                const event = {
                    preventDefault: jest.fn()
                };

                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                    setInvalidParams: jest.fn(),
                    setGridParams: jest.fn(),
                    getGridData: jest.fn(),
                };

                const filter = shallow(<Filter {...props} />)
                filter.instance().getParams = jest.fn();
                filter.instance().getParams.mockReturnValueOnce(params);

                // Action
                filter.instance().submitHandler(event);

                // Assert
                expect(event.preventDefault).toHaveBeenCalledTimes(1);
                expect(props.setInvalidParams).toHaveBeenCalledWith({ show: false });
                expect(props.setGridParams).toHaveBeenCalledWith(params);
                expect(props.getGridData).toHaveBeenCalledTimes(1);
            });

            it('submitHandler runs with invalid params', () => {
                // Arrange
                const event = {
                    preventDefault: jest.fn()
                };

                const props = {
                    gridParams: {},
                    settings: {},
                    invalidParams: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                    setInvalidParams: jest.fn(),
                    setGridParams: jest.fn(),
                    getGridData: jest.fn(),
                };

                const filter = shallow(<Filter {...props} />)

                // Action
                filter.instance().submitHandler(event);

                // Assert
                expect(event.preventDefault).toHaveBeenCalledTimes(1);
                expect(props.setInvalidParams).toHaveBeenCalledWith({ show: false });
                expect(props.setGridParams).toHaveBeenCalledTimes(0);
                expect(props.getGridData).toHaveBeenCalledTimes(0);
            });
        });
    });
});
