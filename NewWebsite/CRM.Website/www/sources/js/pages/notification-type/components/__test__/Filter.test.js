
import React from 'react';
import FilterWrapper, { Filter } from '../Filter';
import { shallow, mount } from 'enzyme';
import configureMockStore from 'redux-mock-store';

describe('Filter', () => {

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('component render correctly', () => {
        // Arrange
        const mockStore = configureMockStore();
        const store = mockStore();

        // Action
        const wrapper = shallow(<FilterWrapper store={store} />);

        // Assert
        expect(wrapper).toMatchSnapshot();
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
            notificationTypeGridParams: {},
            settings: {},
            getSettings: jest.fn(() => Promise.resolve({ data: {} }))
        };

        // Action
        const filter = shallow(<Filter {...props} />);

        // Assert
        expect(filter.state()).toEqual(initialState);
    });

    it('componentDidMount runs correctly', async () => {
        // Arrange
        const id = 1;
        const promise = Promise.resolve({ data: {} });
        const props = {
            notificationTypeGridParams: {},
            settings: {
                recipientTypes: [
                    {
                        id: id,
                        name: 'name1'
                    }
                ],
                chatTypes: [
                    {
                        id: id,
                        name: 'name1'
                    }
                ],
                systems: [
                    {
                        id: id,
                        name: 'name1'
                    }
                ]
            },
            getSettings: jest.fn(() => promise),
            recipientTypesOptions: [],
            chatTypesOptions: [],
            systemsOptions: []
        };
        const recipientTypesOptions = props.settings.recipientTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });
        const chatTypesOptions = props.settings.chatTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });
        const systemsOptions = props.settings.systems.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: true
            };
        });

        // Action
        const filter = mount(<Filter {...props} />);

        // Assert
        expect(props.getSettings).toHaveBeenCalledTimes(1);
        promise.then(() => {
            expect(filter.instance().state.recipientTypesOptions).toEqual(recipientTypesOptions);
            expect(filter.instance().state.chatTypesOptions).toEqual(chatTypesOptions);
            expect(filter.instance().state.systemsOptions).toEqual(systemsOptions);
            expect(filter.instance().state.filter.recipientTypeIds).toEqual(id.toString());
            expect(filter.instance().state.filter.chatTypeIds).toEqual(id.toString());
            expect(filter.instance().state.filter.systems).toEqual(id.toString());
        });
    });

    describe('methods', () => {
        it('should calls setState when change notification type', () => {
            // Arrange
            const props = {
                notificationTypeGridParams: {},
                settings: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
            const expected = {
                notificationTypeName: 'test'
            };
            const container = shallow(<Filter {...props} />);

            // Act
            container.instance().handleNotificationTypeChange({ target: { value: 'test' } });

            // Assert
            expect(container.state().notificationTypeName).toEqual(expected.notificationTypeName);
        });

        it('handleFilterChange runs correctly', () => {
            // Arrange
            const props = {
                notificationTypeGridParams: {},
                settings: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const filter = shallow(<Filter {...props} />);

            // Action
            filter.instance().handleFilterChange('name', 'value');

            // Assert
            expect(filter.instance().state.filter.name).toBe('value');
        });

        it('should call handleFilterChange when change recipient type', () => {
            // Arrange
            const expected = 'selectedItemsKey';
            const props = {
                notificationTypeGridParams: {},
                settings: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().recipientNotificationTypeChange(null, expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('recipientTypes', expected);
        });

        it('should call handleFilterChange when change chat type', () => {
            // Arrange
            const expected = 'selectedItemsKey';
            const props = {
                notificationTypeGridParams: {},
                settings: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().chatNotificationTypeChange(null, expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('chatTypes', expected);
        });

        it('should call handleFilterChange when change system', () => {
            // Arrange
            const expected = 'selectedItemsKey';
            const props = {
                notificationTypeGridParams: {},
                settings: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().systemChange(null, expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('systems', expected);
        });

        it('should call handleOpenCreatePopup when click insert button', () => {
            // Arrange
            const props = {
                isHost: true,
                setCreateNotificationTypePopup: jest.fn(),
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
            const container = shallow(<Filter {...props} />);

            // Act
            container.instance().handleOpenCreatePopup()

            // Assert
            expect(props.setCreateNotificationTypePopup).toHaveBeenCalledWith({ show: true });
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
                    notificationTypeGridParams: {
                        pageIndex: 1,
                        pageSize: 100
                    },
                    settings: {},
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
                    notificationTypeGridParams: {
                        pageIndex: 1,
                        pageSize: 100
                    },
                    settings: {},
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
            it('submitHandler runs with valid params', () => {
                // Arrange
                const params = { value: 'value' };
                const event = {
                    preventDefault: jest.fn()
                };

                const props = {
                    notificationTypeGridParams: {},
                    settings: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                    setNotificationTypeGridParams: jest.fn(),
                    getNotificationTypeGridData: jest.fn(),
                };

                const filter = shallow(<Filter {...props} />)
                filter.instance().getParams = jest.fn();
                filter.instance().getParams.mockReturnValueOnce(params);

                // Action
                filter.instance().submitHandler(event);

                // Assert
                expect(event.preventDefault).toHaveBeenCalledTimes(1);
                expect(props.setNotificationTypeGridParams).toHaveBeenCalledWith(params);
                expect(props.getNotificationTypeGridData).toHaveBeenCalledTimes(1);
            });

            it('submitHandler runs with invalid params', () => {
                // Arrange
                const event = {
                    preventDefault: jest.fn()
                };

                const props = {
                    notificationTypeGridParams: {},
                    settings: {},
                    getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                    setNotificationTypeGridParams: jest.fn(),
                    getNotificationTypeGridData: jest.fn(),
                };

                const filter = shallow(<Filter {...props} />)

                // Action
                filter.instance().submitHandler(event);

                // Assert
                expect(event.preventDefault).toHaveBeenCalledTimes(1);
                expect(props.setNotificationTypeGridParams).toHaveBeenCalledTimes(0);
                expect(props.getNotificationTypeGridData).toHaveBeenCalledTimes(0);
            });
        });
    });
});
