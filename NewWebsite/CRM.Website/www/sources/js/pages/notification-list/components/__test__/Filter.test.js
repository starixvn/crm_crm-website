import React from 'react';
import FilterWrapper, { Filter } from '../Filter';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';
import DateTimeService from '@jsroot/common/dateTimeService';
import functions, { handleNewResendOptions } from '../../functions';
import MultipleSelect from '@jsroot/components/multiple-select/index';
import DatePicker from '@jsroot/components/date-picker/index';
import SelectWithSearch from 'react-select';
import { Form, Button } from 'react-bootstrap';

describe('Filter', () => {
    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            invalidParams: {},
            settings: {},
            headlineOptions: {},
            recipientOptions: {}
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
            expect(wrapper.props().children.props.headlineOptions).toEqual(initialState.headlineOptions);
            expect(wrapper.props().children.props.recipientOptions).toEqual(initialState.recipientOptions);
        });
    });

    it('component has correct initialState', () => {
        // Arrange
        const now = new Date(DateTimeService.toShortDateFormat(DateTimeService.toClientDateTime(new Date())));
        const initialState = {
            filter: {
                notificationTypes: '',
                statuses: '',
                fromDate: now,
                toDate: now,
                headline: null,
                recipient: null,
                notificationIdLower: '',
                notificationIdUpper: '',
                messageID: '',
                content: ''
            },
            maxDate: now,
            minToDate: now,
            maxToDate: now,
            notificationTypeOptions: [],
            statusOptions: []
        };

        const props = {
            gridParams: {},
            invalidParams: {},
            settings: {
                notificationTypes: [],
                statuses: []
            },
            headlineOptions: {},
            recipientOptions: {},
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
            invalidParams: {},
            settings: {
                notificationTypes: [
                    {
                        notificationTypeId: 1,
                        notificationTypeName: 'name1'
                    }
                ],
                statuses: ['Error']
            },
            headlineOptions: {},
            recipientOptions: {},
            getSettings: jest.fn(() => Promise.resolve({ data: {} }))
        };

        // Action
        const filter = shallow(<Filter {...props} />);

        // Assert
        expect(props.getSettings).toHaveBeenCalledTimes(1);
    });

    describe('handleFilterChange', () => {
        it('handleFilterChange runs correctly', () => {
            // Arrange
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);

            // Action
            filter.instance().handleFilterChange('name', 'value');

            // Assert
            expect(filter.instance().state.filter.name).toBe('value');
        });
    });

    describe('handleNotificationTypeChange', () => {
        it('handleNotificationTypeChange call handleFilterChange', () => {
            // Arrange
            const expected = 'selectedItemsKey';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleNotificationTypeChange(null, expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('notificationTypes', expected);
        });
    });

    describe('handleFromDateChange', () => {
        it('handleFromDateChange run correctly', () => {
            // Arrange
            const expected = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().changeToDateAfterFromDateChanged = jest.fn();
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleFromDateChange(expected);

            // Assert
            expect(filter.instance().changeToDateAfterFromDateChanged).toHaveBeenCalledWith(expected);
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('fromDate', expected);
            expect(filter.instance().state.minToDate).toEqual(expected);
        });

        it('maxToDate changes when handleFromDateChange run with toDate less than maxDate', () => {
            // Arrange
            const toDate = '09/03/2020';
            const expected = new Date(toDate);
            expected.setMonth(expected.getMonth() + 1);
            expected.setDate(expected.getDate() - 1);
            const maxDate = new Date('10/04/2020');
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.setState({ maxDate });

            // Action
            filter.instance().handleFromDateChange(toDate);

            // Assert
            expect(filter.instance().state.maxToDate).toEqual(expected);
        });

        it('maxToDate changes when handleFromDateChange run with toDate not less than maxDate', () => {
            // Arrange
            const toDate = '09/03/2020';
            const maxDate = new Date('8/04/2020');
            const maxToDate = 'max to date';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.setState({ maxDate, maxToDate });

            // Action
            filter.instance().handleFromDateChange(toDate);

            // Assert
            expect(filter.instance().state.maxToDate).toEqual(maxToDate);
        });
    });

    describe('changeToDateAfterFromDateChanged', () => {
        it('changeToDateAfterFromDateChanged calls handleFilterChange run with value', () => {
            // Arrange
            const expected = new Date('09/04/2020');
            const state = {
                filter: {
                    toDate: new Date('09/03/2020')
                }
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.setState(state);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().changeToDateAfterFromDateChanged(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('toDate', expected);
        });

        it('changeToDateAfterFromDateChanged calls handleFilterChange run with filter toDate greater than maxToDate', () => {
            // Arrange
            const value = new Date('09/03/2020');
            const state = {
                filter: {
                    toDate: new Date('09/03/2020')
                },
                maxToDate: new Date('09/02/2020')
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.setState(state);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().changeToDateAfterFromDateChanged(value);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('toDate', state.maxToDate);
        });

        it('changeToDateAfterFromDateChanged not calls handleFilterChange', () => {
            // Arrange
            const value = new Date('09/03/2020');
            const state = {
                filter: {
                    toDate: new Date('09/03/2020')
                },
                maxToDate: new Date('09/03/2020')
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.setState(state);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().changeToDateAfterFromDateChanged(value);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledTimes(0);
        });
    });

    describe('handleToDateChange', () => {
        it('handleToDateChange runs correctly', () => {
            // Arrange
            const expected = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleToDateChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('toDate', expected);
        });
    });

    describe('handleHeadlineChange', () => {
        it('handleHeadlineChange runs correctly', () => {
            // Arrange
            const expected = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleHeadlineChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('headline', expected);
        });
    });

    describe('handleRecipientChange', () => {
        it('handleRecipientChange runs correctly', () => {
            // Arrange
            const expected = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleRecipientChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('recipient', expected);
        });
    });

    describe('handleStatusChange', () => {
        it('handleStatusChange runs correctly', () => {
            // Arrange
            const expected = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleStatusChange(null, expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('statuses', expected);
        });
    });

    describe('handleChangeFilterControl', () => {
        it('handleChangeFilterControl valid event runs correctly', () => {
            // Arrange
            const event = {
                target: {
                    name: 'name',
                    value: 'value',
                    validity: {
                        valid: true
                    }
                }
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleChangeFilterControl(event);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith(event.target.name, event.target.value);
        });
        
        it('handleChangeFilterControl invalid event runs correctly', () => {
            // Arrange
            const event = {
                target: {
                    validity: {
                        valid: false
                    }
                }
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleChangeFilterControl(event);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledTimes(0);
        });
    });

    describe('handleChange', () => {
        it('handleChange runs correctly', () => {
            // Arrange
            const event = {
                target: {
                    name: 'name',
                    value: 'value'
                }
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFilterChange = jest.fn();

            // Action
            filter.instance().handleChange(event);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith(event.target.name, event.target.value);
        });
    });

    describe('getParams', () => {
        it('getParams with valid headline and recipient returns correctly', () => {
            // Arrange
            const state = {
                filter: {
                    notificationTypes: 'notification types',
                    fromDate: '09/03/2020',
                    toDate: '09/03/2020',
                    headline: {
                        value: 'headline'
                    },
                    recipient: {
                        value: 'recipient'
                    },
                    messageID: 'messageID',
                    notificationIdLower: 'notificationIdLower',
                    notificationIdUpper: 'notificationIdUpper',
                    content: 'content',
                    status: 'statuses'
                }
            };

            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const expected = {
                notificationTypeIds: state.filter.notificationTypes,
                fromDate: DateTimeService.startOfDay(state.filter.fromDate),
                toDate: DateTimeService.endOfDay(state.filter.toDate),
                headline: state.filter.headline.value,
                recipientName: state.filter.recipient.value,
                messageId: state.filter.messageID,
                notificationIdLower: state.filter.notificationIdLower,
                notificationIdUpper: state.filter.notificationIdUpper,
                content: state.filter.content,
                status: state.filter.statuses,
                pageIndex: props.gridParams.pageIndex,
                pageSize: props.gridParams.pageSize
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            // Action
            const result = filter.instance().getParams();

            // Assert
            expect(result).toEqual(expected);
        });
        
        it('getParams with invalid headline and recipient returns correctly', () => {
            // Arrange
            const state = {
                filter: {
                    notificationTypes: 'notification types',
                    fromDate: '09/03/2020',
                    toDate: '09/03/2020',
                    headline: null,
                    recipient: null,
                    messageID: 'messageID',
                    notificationIdLower: 'notificationIdLower',
                    notificationIdUpper: 'notificationIdUpper',
                    content: 'content',
                    status: 'statuses'
                }
            };

            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const expected = {
                notificationTypeIds: state.filter.notificationTypes,
                fromDate: DateTimeService.startOfDay(state.filter.fromDate),
                toDate: DateTimeService.endOfDay(state.filter.toDate),
                headline: '',
                recipientName: '',
                messageId: state.filter.messageID,
                notificationIdLower: state.filter.notificationIdLower,
                notificationIdUpper: state.filter.notificationIdUpper,
                content: state.filter.content,
                status: state.filter.statuses,
                pageIndex: props.gridParams.pageIndex,
                pageSize: props.gridParams.pageSize
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            // Action
            const result = filter.instance().getParams();

            // Assert
            expect(result).toEqual(expected);
        });
    });

    describe('isDisabledDateParam', () => {
        it('isDisabledDateParam returns true', () => {
            // Arrange
            const state = {
                filter: {
                    notificationIdUpper: 'value'
                }
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            // Action
            const result = filter.instance().isDisabledDateParam();

            // Assert
            expect(result).toBeTrue;
        });
        
        it('isDisabledDateParam returns false', () => {
            // Arrange
            const state = {
                filter: {
                    messageID: '',
                    notificationIdLower: '',
                    notificationIdUpper: ''
                }
            };
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            // Action
            const result = filter.instance().isDisabledDateParam();

            // Assert
            expect(result).toBeFalse;
        });
    });

    describe('isValidateParams', () => {
        it('isValidateParams returns true', () => {
            // Arrange
            const params = {
                notificationTypeIds: 'notificationIds',
                notificationIdLower: '1',
                notificationIdUpper: '10',
                status: 'status'
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);

            // Action
            const result = filter.instance().isValidateParams(params);

            // Assert
            expect(result).toBeTrue;
        });
    });

    describe('debounceHandleHeadlineInputChange', () => {
        jest.useFakeTimers();

        it('searchText valid', () => {
            // Arrange
            const searchText = 'text with length > 2';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                getHeadlineOptions: jest.fn()
            };
    
            const filter = shallow(<Filter {...props} />);

            // Action
            filter.instance().debounceHandleHeadlineInputChange(searchText);
            jest.runAllTimers();

            // Assert
            expect(props.getHeadlineOptions).toHaveBeenCalledWith(searchText);
        });
        
        it('searchText invalid', () => {
            // Arrange
            const searchText = '';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                setHeadlineOptions: jest.fn()
            };
    
            const filter = shallow(<Filter {...props} />);

            // Action
            filter.instance().debounceHandleHeadlineInputChange(searchText);
            jest.runAllTimers();

            // Assert
            expect(props.setHeadlineOptions).toHaveBeenCalledWith([]);
        });
    });

    describe('debounceHandleRecipientInputChange', () => {
        jest.useFakeTimers();

        it('searchText valid', () => {
            // Arrange
            const searchText = 'text with length > 2';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                getRecipientOptions: jest.fn()
            };
    
            const filter = shallow(<Filter {...props} />);

            // Action
            filter.instance().debounceHandleRecipientInputChange(searchText);
            jest.runAllTimers();

            // Assert
            expect(props.getRecipientOptions).toHaveBeenCalledWith(searchText);
        });
        
        it('searchText invalid', () => {
            // Arrange
            const searchText = '';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} })),
                setRecipientOptions: jest.fn()
            };
    
            const filter = shallow(<Filter {...props} />);

            // Action
            filter.instance().debounceHandleRecipientInputChange(searchText);
            jest.runAllTimers();

            // Assert
            expect(props.setRecipientOptions).toHaveBeenCalledWith([]);
        });
    });

    describe('component render correctly', () => {
        it('notification types MultipleSelect change correctly', () => {            
            // Arrange
            const currentItem = 'currentItem';
            const selectedItemsKey = 'selectedItemsKey';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleNotificationTypeChange = jest.fn();

            // Action
            filter.find(MultipleSelect).at(0).simulate('change', currentItem, selectedItemsKey);

            // Assert
            expect(filter.instance().handleNotificationTypeChange).toHaveBeenCalledWith(currentItem, selectedItemsKey);
        });
        
        it('fromDate DatePicker change correctly', () => {            
            // Arrange
            const value = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleFromDateChange = jest.fn();

            // Action
            filter.find(DatePicker).at(0).simulate('change', value);

            // Assert
            expect(filter.instance().handleFromDateChange).toHaveBeenCalledWith(value);
        });
        
        it('toDate DatePicker change correctly', () => {            
            // Arrange
            const value = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleToDateChange = jest.fn();

            // Action
            filter.find(DatePicker).at(1).simulate('change', value);

            // Assert
            expect(filter.instance().handleToDateChange).toHaveBeenCalledWith(value);
        });
        
        it('headline SelectWithSearch change correctly', () => {
            // Arrange
            const value = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleHeadlineChange = jest.fn();

            // Action
            filter.find(SelectWithSearch).at(0).simulate('change', value);

            // Assert
            expect(filter.instance().handleHeadlineChange).toHaveBeenCalledWith(value);
        });
        
        it('headline SelectWithSearch inputChange correctly', () => {            
            // Arrange
            const searchText = 'searchText';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().debounceHandleHeadlineInputChange = jest.fn();

            // Action
            filter.find(SelectWithSearch).at(0).simulate('inputChange', searchText);

            // Assert
            expect(filter.instance().debounceHandleHeadlineInputChange).toHaveBeenCalledWith(searchText);
        });
        
        it('recipient SelectWithSearch change correctly', () => {
            // Arrange
            const value = 'value';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleRecipientChange = jest.fn();

            // Action
            filter.find(SelectWithSearch).at(1).simulate('change', value);

            // Assert
            expect(filter.instance().handleRecipientChange).toHaveBeenCalledWith(value);
        });
        
        it('recipient SelectWithSearch inputChange correctly', () => {            
            // Arrange
            const searchText = 'searchText';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().debounceHandleRecipientInputChange = jest.fn();

            // Action
            filter.find(SelectWithSearch).at(1).simulate('inputChange', searchText);

            // Assert
            expect(filter.instance().debounceHandleRecipientInputChange).toHaveBeenCalledWith(searchText);
        });
        
        it('messageID Form.Control change correctly', () => {            
            // Arrange
            const event = {
                target: {
                    validity: {}
                }
            };
            
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleChangeFilterControl = jest.fn();

            // Action
            filter.find(Form.Control).at(0).simulate('change', event);

            // Assert
            expect(filter.instance().handleChangeFilterControl).toHaveBeenCalledWith(event);
        });
        
        it('notificationIdLower Form.Control change correctly', () => {            
            // Arrange
            const event = {
                target: {
                    validity: {}
                }
            };
            
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleChangeFilterControl = jest.fn();

            // Action
            filter.find(Form.Control).at(1).simulate('change', event);

            // Assert
            expect(filter.instance().handleChangeFilterControl).toHaveBeenCalledWith(event);
        });
        
        it('notificationIdUpper Form.Control change correctly', () => {            
            // Arrange
            const event = {
                target: {
                    validity: {}
                }
            };
            
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleChangeFilterControl = jest.fn();

            // Action
            filter.find(Form.Control).at(2).simulate('change', event);

            // Assert
            expect(filter.instance().handleChangeFilterControl).toHaveBeenCalledWith(event);
        });
        
        it('content Form.Control change correctly', () => {            
            // Arrange
            const event = {
                target: {
                    name: 'name',
                    value: 'value'
                }
            };
            
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleChange = jest.fn();

            // Action
            filter.find(Form.Control).at(3).simulate('change', event);

            // Assert
            expect(filter.instance().handleChange).toHaveBeenCalledWith(event);
        });
        
        it('status MultipleSelect change correctly', () => {            
            // Arrange
            const currentItem = 'currentItem';
            const selectedItemsKey = 'selectedItemsKey';
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().handleStatusChange = jest.fn();

            // Action
            filter.find(MultipleSelect).at(1).simulate('change', currentItem, selectedItemsKey);

            // Assert
            expect(filter.instance().handleStatusChange).toHaveBeenCalledWith(currentItem, selectedItemsKey);
        });
        
        it('submit Button click correctly', () => {            
            // Arrange
            const event = {
                preventDefault: jest.fn()
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };
    
            const filter = shallow(<Filter {...props} />);
            filter.instance().submitHandler = jest.fn();

            // Action
            filter.find(Button).simulate('click', event);

            // Assert
            expect(filter.instance().submitHandler).toHaveBeenCalledWith(event);
        });
    });
});