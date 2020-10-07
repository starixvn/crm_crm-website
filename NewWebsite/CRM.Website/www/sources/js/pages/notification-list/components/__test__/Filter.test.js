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
import { toast } from 'react-toastify';

describe('Filter', () => {
    LangHelper.getResource = jest.fn(value => value);

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            invalidParams: {},
            settings: {},
            headlineOptions: {},
            recipientOptions: {},
            autoRefresh: {}
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
            expect(wrapper.props().children.props.autoRefresh).toEqual(initialState.autoRefresh);
        });
    });

    it('component has correct initialState', () => {
        // Arrange
        const date = '09/09/2020';
        DateTimeService.toShortDateFormat = jest.fn(() => date);
        const now = new Date(date);
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
                messageId: '',
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
            getSettings: jest.fn(() => Promise.resolve({}))
        };

        // Action
        const filter = shallow(<Filter {...props} />);

        // Assert
        expect(filter.state()).toEqual(initialState);
    });

    it('componentDidMount runs correctly', async () => {
        // Arrange
        functions.handleNewResendOptions = jest.fn(data => handleNewResendOptions(data, props.setResendOptions));
        const notificationTypeId = 1;
        const status = 'Error';
        const promise = Promise.resolve({});
        const params = {
            value: 'value'
        };

        const props = {
            gridParams: {},
            invalidParams: {},
            settings: {
                notificationTypes: [
                    {
                        notificationTypeId: notificationTypeId,
                        notificationTypeName: 'name1'
                    }
                ],
                statuses: [status]
            },
            headlineOptions: {},
            recipientOptions: {},
            getSettings: jest.fn(() => promise),
            setGridParams: jest.fn(),
            getGridData: jest.fn((params, callback) => callback([]))
        };

        const notificationTypeOptions = props.settings.notificationTypes.map(x => {
            return {
                id: x.notificationTypeId,
                name: x.notificationTypeName,
                checked: true
            };
        });

        const statusOptions = props.settings.statuses.map(x => {
            return {
                id: x,
                name: x,
                checked: true
            };
        });

        // Action
        const filter = shallow(<Filter {...props} />);
        filter.instance().getParams = jest.fn().mockReturnValue(params);

        // Assert
        expect(props.getSettings).toHaveBeenCalled();
        promise.then(() => {
            expect(filter.instance().state.notificationTypeOptions).toEqual(notificationTypeOptions);
            expect(filter.instance().state.statusOptions).toEqual(statusOptions);
            expect(filter.instance().state.filter.notificationTypes).toEqual(notificationTypeId.toString());
            expect(filter.instance().state.filter.statuses).toEqual(status.toString());
        });
    });

    it('handleFilterChange runs correctly', () => {
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

        // Action
        filter.instance().handleFilterChange('name', value);

        // Assert
        expect(filter.instance().state.filter.name).toBe(value);
    });

    it('handleFilterChangeProps runs correctly', () => {
        // Arrange
        const name = 'name';
        const value = 'value';
        const props = {
            gridParams: {},
            invalidParams: {},
            settings: {},
            headlineOptions: {},
            recipientOptions: {},
            setGridParams: jest.fn(),
            getSettings: jest.fn(() => Promise.resolve({ data: {} }))
        };

        const filter = shallow(<Filter {...props} />);
        filter.instance().setNotShouldResetScrollPosition = jest.fn();

        // Action
        filter.instance().handleFilterChangeProps(name, value);

        // Assert
        expect(props.setGridParams).toHaveBeenCalledWith({
            ...props.gridParams,
            [name]: value
        });

        expect(filter.instance().setNotShouldResetScrollPosition).toHaveBeenCalled();
    });

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
        filter.instance().handleFilterChangeProps = jest.fn();

        // Action
        filter.instance().handleNotificationTypeChange(null, expected);

        // Assert
        expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('notificationTypes', expected);
        expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('notificationTypeIds', expected);
    });

    describe('handleFromDateChange', () => {
        it('runs regularly', () => {
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
            filter.instance().handleFilterChangeProps = jest.fn();
            filter.instance().handleFilterChange = jest.fn();
            DateTimeService.startOfDay = jest.fn(value => value);

            // Action
            filter.instance().handleFromDateChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('fromDate', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('fromDate', expected);
            expect(filter.instance().state.minToDate).toEqual(expected);
        });

        describe('runs with new maxToDate greater than state maxDate' , () => {
            it('filter toDate greater than maxToDate' , () => {
                // Arrange
                const value = '09/25/2020';
                const maxDate = new Date('10/24/2020');
                const state = {
                    maxDate,
                    filter: {
                        toDate: new Date('10/26/2020')
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
                filter.instance().handleFilterChangeProps = jest.fn();
                filter.instance().handleFilterChange = jest.fn();
                filter.instance().handleToDateChange = jest.fn();

                // Action
                filter.instance().handleFromDateChange(value);

                // Assert
                expect(filter.instance().handleToDateChange).toHaveBeenCalledWith(maxDate);
                expect(filter.instance().state.maxToDate).toEqual(maxDate);
            });

            it('filter toDate not greater than maxToDate and less than value' , () => {
                // Arrange
                const value = new Date('09/25/2020');
                const maxDate = new Date('10/23/2020');
                const state = {
                    maxDate,
                    filter: {
                        toDate: new Date('09/22/2020')
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
                filter.instance().handleFilterChangeProps = jest.fn();
                filter.instance().handleFilterChange = jest.fn();
                filter.instance().handleToDateChange = jest.fn();

                // Action
                filter.instance().handleFromDateChange(value);

                // Assert
                expect(filter.instance().handleToDateChange).toHaveBeenCalledWith(value);
                expect(filter.instance().state.maxToDate).toEqual(maxDate);
            });

            it('not change toDate' , () => {
                // Arrange
                const value = new Date('09/25/2020');
                const maxDate = new Date('10/23/2020');
                const state = {
                    maxDate,
                    filter: {
                        toDate: new Date('09/26/2020')
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
                filter.instance().handleFilterChangeProps = jest.fn();
                filter.instance().handleFilterChange = jest.fn();
                filter.instance().handleToDateChange = jest.fn();

                // Action
                filter.instance().handleFromDateChange(value);

                // Assert
                expect(filter.instance().handleToDateChange).toHaveBeenCalledTimes(0);
                expect(filter.instance().state.maxToDate).toEqual(maxDate);
            });
        });
        

        describe('runs with new maxToDate not greater than state maxDate' , () => {
            it('filter toDate greater than maxToDate' , () => {
                // Arrange
                const value = '09/30/2020';
                const maxDate = new Date('10/30/2020');
                const state = {
                    maxDate,
                    filter: {
                        toDate: new Date('11/01/2020')
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
                filter.instance().handleFilterChangeProps = jest.fn();
                filter.instance().handleFilterChange = jest.fn();
                filter.instance().handleToDateChange = jest.fn();

                // Action
                filter.instance().handleFromDateChange(value);

                // Assert
                expect(filter.instance().handleToDateChange).toHaveBeenCalledWith(maxDate);
                expect(filter.instance().state.maxToDate).toEqual(maxDate);
            });

            it('filter toDate not greater than maxToDate and less than value' , () => {
                // Arrange
                const value = new Date('09/30/2020');
                const maxDate = new Date('10/30/2020');
                const state = {
                    maxDate,
                    filter: {
                        toDate: new Date('09/29/2020')
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
                filter.instance().handleFilterChangeProps = jest.fn();
                filter.instance().handleFilterChange = jest.fn();
                filter.instance().handleToDateChange = jest.fn();

                // Action
                filter.instance().handleFromDateChange(value);

                // Assert
                expect(filter.instance().handleToDateChange).toHaveBeenCalledWith(value);
                expect(filter.instance().state.maxToDate).toEqual(maxDate);
            });

            it('not change toDate' , () => {
                // Arrange
                const value = new Date('09/30/2020');
                const maxDate = new Date('10/30/2020');
                const state = {
                    maxDate,
                    filter: {
                        toDate: new Date('10/01/2020')
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
                filter.instance().handleFilterChangeProps = jest.fn();
                filter.instance().handleFilterChange = jest.fn();
                filter.instance().handleToDateChange = jest.fn();

                // Action
                filter.instance().handleFromDateChange(value);

                // Assert
                expect(filter.instance().handleToDateChange).toHaveBeenCalledTimes(0);
                expect(filter.instance().state.maxToDate).toEqual(maxDate);
            });
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
            filter.instance().handleFilterChangeProps = jest.fn();
            DateTimeService.startOfDay = jest.fn(value => value);

            // Action
            filter.instance().handleToDateChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('toDate', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('toDate', expected);
        });
    });

    describe('handleHeadlineChange', () => {
        it('handleHeadlineChange runs with valid value', () => {
            // Arrange
            const expected = {
                value: 'value'
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleHeadlineChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('headline', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('headline', expected.value);
        });
        
        it('handleHeadlineChange runs with invalid value', () => {
            // Arrange
            const expected = null;
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleHeadlineChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('headline', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('headline', '');
        });
    });

    describe('handleRecipientChange', () => {
        it('handleRecipientChange runs with valid value', () => {
            // Arrange
            const expected = {
                value: 'value'
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleRecipientChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('recipient', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('recipientName', expected.value);
        });
        
        it('handleRecipientChange runs with invalid value', () => {
            // Arrange
            const expected = null;
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleRecipientChange(expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('recipient', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('recipientName', '');
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleStatusChange(null, expected);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith('statuses', expected);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith('status', expected);
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleChangeFilterControl(event);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith(event.target.name, event.target.value);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith(event.target.name, event.target.value);
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
            filter.instance().handleFilterChangeProps = jest.fn();

            // Action
            filter.instance().handleChange(event);

            // Assert
            expect(filter.instance().handleFilterChange).toHaveBeenCalledWith(event.target.name, event.target.value);
            expect(filter.instance().handleFilterChangeProps).toHaveBeenCalledWith(event.target.name, event.target.value);
        });
    });

    describe('getParams', () => {
        DateTimeService.toLongDateTimeFormat = jest.fn(value => value);

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
                    messageId: 'messageID',
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
                messageId: state.filter.messageId,
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
                    messageId: 'messageID',
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
                messageId: state.filter.messageId,
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

    it('setNotShouldResetScrollPosition', () => {
        // Arrange
        const props = {
            resendOptions: {},
            invalidParams: {},
            getSettings: jest.fn(() => Promise.resolve({})),
            setResendOptions: jest.fn()
        }

        var filter = shallow(<Filter {...props} />);

        // Action
        filter.instance().setNotShouldResetScrollPosition();

        // Assert
        expect(props.setResendOptions).toHaveBeenCalledWith({
            shouldResetScrollPosition: false,
            ...props.resendOptions
        });
    });

    describe('submitHandler', () => {
        it('valid params', () => {
            // Arrange
            const params = {
                value: 'value'
            };

            const event = {
                preventDefault: jest.fn()
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                autoRefresh: {
                    value: 10
                },
                getSettings: jest.fn(() => Promise.resolve({})),
                setInvalidParams: jest.fn(),
                setGridParams: jest.fn(),
                getGridData: jest.fn((params, callback) => callback([])),
                setResendOptions: jest.fn(),
                setAutoRefresh: jest.fn()
            };

            functions.handleNewResendOptions = jest.fn(data => handleNewResendOptions(data, props.setResendOptions));
            const filter = shallow(<Filter {...props} />);
            filter.instance().isValidateParams = jest.fn().mockReturnValue(true);
            filter.instance().getParams = jest.fn().mockReturnValue(params);

            // Action
            filter.instance().submitHandler(event);

            // Assert
            expect(event.preventDefault).toHaveBeenCalled();
            expect(props.setInvalidParams).toHaveBeenCalledWith({});
            expect(props.setGridParams).toHaveBeenCalledWith(params);
            expect(props.getGridData).toHaveBeenCalledWith({ ...params }, expect.any(Function));
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

        it('notification type id empty', () => {
            // Arrange
            const params = {
                notificationTypeIds: ''
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            toast.error = jest.fn();

            // Action
            const result = filter.instance().isValidateParams(params);

            // Assert
            expect(result).toBeFalse;
            expect(toast.error).toHaveBeenCalledWith('PleaseChoose a NotificationType');
        });

        it('notification id upper empty', () => {
            // Arrange
            const params = {
                notificationTypeIds: 'notificationTypeIds',
                notificationIdLower: '1',
                notificationIdUpper: ''
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({})),
                setInvalidParams: jest.fn()
            };

            const filter = shallow(<Filter {...props} />);
            toast.error = jest.fn();

            // Action
            const result = filter.instance().isValidateParams(params);

            // Assert
            expect(result).toBeFalse;
            expect(toast.error).toHaveBeenCalledWith('NotificationIDMustHaveBoth');
            expect(props.setInvalidParams).toHaveBeenCalledWith({ isInvalidNotificationIdUpper: true });
        });

        it('notification id lower empty', () => {
            // Arrange
            const params = {
                notificationTypeIds: 'notificationTypeIds',
                notificationIdLower: '',
                notificationIdUpper: '1'
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({})),
                setInvalidParams: jest.fn()
            };

            const filter = shallow(<Filter {...props} />);
            toast.error = jest.fn();

            // Action
            const result = filter.instance().isValidateParams(params);

            // Assert
            expect(result).toBeFalse;
            expect(toast.error).toHaveBeenCalledWith('NotificationIDMustHaveBoth');
            expect(props.setInvalidParams).toHaveBeenCalledWith({ isInvalidNotificationIdLower: true });
        });

        it('notification id lower greater than upper', () => {
            // Arrange
            const params = {
                notificationTypeIds: 'notificationTypeIds',
                notificationIdLower: '2',
                notificationIdUpper: '1'
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            toast.error = jest.fn();

            // Action
            const result = filter.instance().isValidateParams(params);

            // Assert
            expect(result).toBeFalse;
            expect(toast.error).toHaveBeenCalledWith('NotificationIDFromNotGreaterThanTo');
        });

        it('status empty', () => {
            // Arrange
            const params = {
                notificationTypeIds: 'notificationTypeIds',
                notificationIdLower: '1',
                notificationIdUpper: '2',
                status: ''
            };

            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            toast.error = jest.fn();

            // Action
            const result = filter.instance().isValidateParams(params);

            // Assert
            expect(result).toBeFalse;
            expect(toast.error).toHaveBeenCalledWith('PleaseChoose a Status');
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

    describe('render', () => {
        it('has regular children', () => {
            // Arrange
            const props = {
                gridParams: {},
                invalidParams: {},
                settings: {},
                headlineOptions: {},
                recipientOptions: {},
                getSettings: jest.fn(() => Promise.resolve({ data: {} }))
            };

            const initialState = {
                notificationTypeOptions: [1],
                statusOptions: ['Error'],
                maxDate: new Date(),
                minToDate: new Date(),
                maxToDate: new Date(),
                filter: {
                    fromDate: new Date(),
                    toDate: new Date(),
                    headline: {
                        value: 'headline'
                    },
                    recipient: {
                        value: 'recipient'
                    },
                    messageID: 1,
                    notificationIdLower: 1,
                    notificationIdUpper: 1,
                    content: 'content'
                }
            };

            // Action
            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(initialState);
            filter.instance().isDisabledDateParam = jest.fn().mockReturnValue(false);

            // Assert
            expect(filter.find(Form).props().className).toBe('form-filter');
            expect(filter.find(Form.Row).props().bsPrefix).toBe('form-row');
            expect(filter.find(Form.Group).at(0).find(Form.Label).html()).toContain('NotificationType');
            expect(filter.find(Form.Group).at(0).find('div').props().className).toBe('form-control-wrapper');
            expect(filter.find(Form.Group).at(0).find('div').find(MultipleSelect).props().id).toBe('notification-type');
            expect(filter.find(Form.Group).at(0).find('div').find(MultipleSelect).props().dataSource)
                .toEqual(initialState.notificationTypeOptions);
            expect(filter.find(Form.Group).at(0).find('div').find(MultipleSelect).props().keyField).toBe('id');
            expect(filter.find(Form.Group).at(0).find('div').find(MultipleSelect).props().statusField).toBe('checked');
            expect(filter.find(Form.Group).at(0).find('div').find(MultipleSelect).props().isSummited).toBeFalse;
            expect(filter.find(Form.Group).at(0).find('div').find(MultipleSelect).props().isAllTextShown).toBeTrue;
            expect(filter.find(Form.Group).at(1).find(Form.Label).html()).toContain('From');
            expect(filter.find(Form.Group).at(1).find('div').props().className)
                .toBe('react-datepicker-wrapper-container form-control-wrapper');
            expect(filter.find(Form.Group).at(1).find('div').find(DatePicker).props().disabled).toBeFalse;
            expect(filter.find(Form.Group).at(1).find('div').find(DatePicker).props().maxDate).toEqual(initialState.maxDate);
            expect(filter.find(Form.Group).at(1).find('div').find(DatePicker).props().selected).toEqual(initialState.filter.fromDate);
            expect(filter.find(Form.Group).at(2).find(Form.Label).html()).toContain('To');
            expect(filter.find(Form.Group).at(2).find('div').props().className)
                .toBe('react-datepicker-wrapper-container form-control-wrapper');
            expect(filter.find(Form.Group).at(2).find('div').find(DatePicker).props().disabled).toBeFalse;
            expect(filter.find(Form.Group).at(2).find('div').find(DatePicker).props().minDate).toEqual(initialState.minToDate);
            expect(filter.find(Form.Group).at(2).find('div').find(DatePicker).props().maxDate).toEqual(initialState.maxToDate);
            expect(filter.find(Form.Group).at(2).find('div').find(DatePicker).props().selected).toEqual(initialState.filter.toDate);
            expect(filter.find(Form.Group).at(3).find(Form.Label).html()).toContain('GroupHeadline');
            expect(filter.find(Form.Group).at(3).find('div').props().className).toBe('form-control-wrapper');
            expect(filter.find(Form.Group).at(3).find('div').find(SelectWithSearch).props().className).toBe('basic-single');
            expect(filter.find(Form.Group).at(3).find('div').find(SelectWithSearch).props().name).toBe('headline');
            expect(filter.find(Form.Group).at(3).find('div').find(SelectWithSearch).props().options).toEqual(props.headlineOptions);
            expect(filter.find(Form.Group).at(3).find('div').find(SelectWithSearch).props().value).toEqual(initialState.filter.headline);
            expect(filter.find(Form.Group).at(4).find(Form.Label).html()).toContain('Recipient');
            expect(filter.find(Form.Group).at(4).find('div').props().className).toBe('form-control-wrapper');
            expect(filter.find(Form.Group).at(4).find('div').find(SelectWithSearch).props().className).toBe('basic-single');
            expect(filter.find(Form.Group).at(4).find('div').find(SelectWithSearch).props().name).toBe('recipient');
            expect(filter.find(Form.Group).at(4).find('div').find(SelectWithSearch).props().options).toEqual(props.recipientOptions);
            expect(filter.find(Form.Group).at(4).find('div').find(SelectWithSearch).props().value).toEqual(initialState.filter.recipient);
            expect(filter.find(Form.Group).at(5).find(Form.Label).html()).toContain('MessageID');
            expect(filter.find(Form.Group).at(5).find(Form.Control).props().as).toBe('input');
            expect(filter.find(Form.Group).at(5).find(Form.Control).props().type).toBe('text');
            expect(filter.find(Form.Group).at(5).find(Form.Control).props().pattern).toBe('[0-9]*');
            expect(filter.find(Form.Group).at(5).find(Form.Control).props().className).toBe('shadow-none');
            expect(filter.find(Form.Group).at(5).find(Form.Control).props().name).toBe('messageId');
            expect(filter.find(Form.Group).at(5).find(Form.Control).props().value).toBe(initialState.filter.messageId);
            expect(filter.find(Form.Group).at(6).find(Form.Label).html()).toContain('NotificationID');
            expect(filter.find(Form.Group).at(6).find('div').at(0).props().className)
                .toBe('form-control-wrapper d-flex align-items-center notification-id-container');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().as).toBe('input');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().type).toBe('text');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().pattern).toBe('[0-9]*');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().size).toBe('sm');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().name).toBe('notificationIdLower');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().value)
                .toEqual(initialState.filter.notificationIdLower);
            expect(filter.find(Form.Group).at(6).find('div').at(1).html())
                .toEqual('<div class="notification-concate text-center">-</div>');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().as).toBe('input');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().type).toBe('text');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().pattern).toBe('[0-9]*');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().size).toBe('sm');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().name).toBe('notificationIdUpper');
            expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().value)
                .toEqual(initialState.filter.notificationIdUpper);
            expect(filter.find(Form.Group).at(7).find(Form.Label).html()).toContain('Content');
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().as).toBe('input');
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().type).toBe('text');
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().size).toBe('sm');
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().className).toBe('shadow-none');
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().name).toBe('content');
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().value)
                .toEqual(initialState.filter.content);
            expect(filter.find(Form.Group).at(7).find(Form.Control).props().autoComplete).toBe('off');
            expect(filter.find(Form.Group).at(8).find(Form.Label).html()).toContain('Status');
            expect(filter.find(Form.Group).at(8).find('div').props().className).toBe('form-control-wrapper');
            expect(filter.find(Form.Group).at(8).find('div').find(MultipleSelect).props().dataSource)
                .toEqual(initialState.statusOptions);
            expect(filter.find(Form.Group).at(8).find('div').find(MultipleSelect).props().keyField).toBe('id');
            expect(filter.find(Form.Group).at(8).find('div').find(MultipleSelect).props().statusField).toBe('checked');
            expect(filter.find(Form.Group).at(8).find('div').find(MultipleSelect).props().isSummited).toBeFalse;
            expect(filter.find(Form.Group).at(8).find('div').find(MultipleSelect).props().isAllTextShown).toBeTrue;
            expect(filter.find(Form.Group).at(9).find(Button).html()).toContain('Submit');
            expect(filter.find(Form.Group).at(9).find(Button).props().variant).toBe('primary');
            expect(filter.find(Form.Group).at(9).find(Button).props().size).toBe('sm');
        });

        describe('has Form.Group 6', () => {
            describe('has Form.Control 0', () => {
                it('has correct valid className', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        invalidParams: {
                            isInvalidNotificationIdLower: false
                        },
                        settings: {},
                        headlineOptions: {},
                        recipientOptions: {},
                        getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                    };

                    // Action
                    const filter = shallow(<Filter {...props} />);

                    // Assert
                    expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().className)
                        .toBe('shadow-none valid-input notification-id-input');
                });

                it('has correct invalid className', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        invalidParams: {
                            isInvalidNotificationIdLower: true
                        },
                        settings: {},
                        headlineOptions: {},
                        recipientOptions: {},
                        getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                    };

                    // Action
                    const filter = shallow(<Filter {...props} />);

                    // Assert
                    expect(filter.find(Form.Group).at(6).find(Form.Control).at(0).props().className)
                        .toBe('shadow-none invalid-input notification-id-input');
                });
            });

            describe('has Form.Control 1', () => {
                it('has correct valid className', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        invalidParams: {
                            isInvalidNotificationIdUpper: false
                        },
                        settings: {},
                        headlineOptions: {},
                        recipientOptions: {},
                        getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                    };

                    // Action
                    const filter = shallow(<Filter {...props} />);

                    // Assert
                    expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().className)
                        .toBe('shadow-none valid-input notification-id-input');
                });

                it('has correct invalid className', () => {
                    // Arrange
                    const props = {
                        gridParams: {},
                        invalidParams: {
                            isInvalidNotificationIdUpper: true
                        },
                        settings: {},
                        headlineOptions: {},
                        recipientOptions: {},
                        getSettings: jest.fn(() => Promise.resolve({ data: {} }))
                    };

                    // Action
                    const filter = shallow(<Filter {...props} />);

                    // Assert
                    expect(filter.find(Form.Group).at(6).find(Form.Control).at(1).props().className)
                        .toBe('shadow-none invalid-input notification-id-input');
                });
            });
        });

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

        it('has Form running onSubmit correctly', () => {
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
            filter.find(Form).simulate('submit', event);

            // Assert
            expect(filter.instance().submitHandler).toHaveBeenCalledWith(event);
        });
    });
});