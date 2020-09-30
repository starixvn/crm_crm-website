import React from 'react';
import FilterWrapper, { Filter } from '../Filter';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';
import { Button, Form } from 'react-bootstrap';
import MultipleSelect from '@jsroot/components/multiple-select/index';
import InsertPersonalMappingPopup from '../InsertPersonalMappingPopup';
import { toast } from 'react-toastify';

describe('Filter', () => {
    LangHelper.getResource = jest.fn(value => value);

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            settings: {
                mappingStatuses: [{ id: 1, name: 'Text' }]
            },
            insertPersonalMappingData: {},
            insertPersonalMappingResult: {}
        };

        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<FilterWrapper store={store} />);

        // Assert
        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.insertPersonalMappingData).toEqual(initialState.insertPersonalMappingData);
            expect(wrapper.props().children.props.insertPersonalMappingResult).toEqual(initialState.insertPersonalMappingResult);
        });
    });

    it('component has correct initialState', () => {
        // Arrange
        const initialState = {
            filter: {
                chatType: '',
                name: '',
                groupHeadline: '',
                recipient: '',
                letstalkID: '',
                mappingStatuses: '',
                languageId: '',
                botId: '',
                active: ''
            },
            settings: {
                chatTypes: [],
                mappingStatuses: [],
                languages: [],
                bots: []
            }
        };

        const props = {
            settings: {
                chatTypes: [],
                mappingStatuses: [{ id: 1, name: 'Text' }],
                languages: [],
                bots: []
            },
            insertPersonalMappingData: {},
            insertPersonalMappingResult: {},
            getSettings: jest.fn(() => Promise.resolve({
                chatTypes: [],
                mappingStatuses: [],
                languages: [],
                bots: []
            }))
        };

        const filter = shallow(<Filter {...props} />);

        expect(filter.state()).toEqual(initialState);
    });

    it('componentDidMount runs correctly', async () => {
        // Arrange
        const promise = Promise.resolve({});
        const params = {
            value: 'value'
        };

        const props = {
            settings: {
                chatTypes: [],
                mappingStatuses: [],
                languages: [],
                bots: []
            },
            insertPersonalMappingData: {},
            insertPersonalMappingResult: {},
            getSettings: jest.fn(() => promise)
        };

        const filter = shallow(<Filter {...props} />);
        filter.instance().getParams = jest.fn().mockReturnValue(params);

        expect(props.getSettings).toHaveBeenCalled();
        promise.then(() => {
            expect(filter.instance().state.settings).toEqual(settings);
        });
    });

    it('handleFilterChange runs correctly', () => {
        // Arrange
        const value = 'value';
        const props = {
            settings: {
                chatTypes: [],
                mappingStatuses: [],
                languages: [],
                bots: []
            },
            insertPersonalMappingData: {},
            insertPersonalMappingResult: {},
            getSettings: jest.fn(() => Promise.resolve({}))
        };

        const filter = shallow(<Filter {...props} />);

        // Action
        filter.instance().onChange('name', value);

        // Assert
        expect(filter.instance().state.filter.name).toBe(value);
    });

    describe('getParams', () => {
        it('getParams with valid headline and recipient returns correctly', () => {
            // Arrange
            const state = {
                filter: {
                    chatType: '',
                    name: '',
                    groupHeadline: '',
                    recipient: '',
                    letstalkID: '',
                    mappingStatuses: '',
                    languageId: '',
                    botId: '',
                    active: ''
                }
            };

            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const expected = {
                chatType: state.filter.chatType,
                botId: state.filter.botId,
                name: state.filter.name,
                groupHeadline: state.filter.groupHeadline,
                recipientIds: state.filter.recipient,
                letstalkId: state.filter.letstalkID,
                mappingStatuses: state.filter.mappingStatuses,
                languageId: state.filter.languageId,
                isActive: state.filter.active,
                pageIndex: props.gridParams.pageIndex,
                pageSize: props.gridParams.pageSize
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            const result = filter.instance().getParams();

            expect(result).toEqual(expected);
        });
    });

    describe('isValidParams', () => {
        it('isValidParams with valid mappingStatus returns true', () => {
            // Arrange
            const state = {
                filter: {
                    mappingStatuses: '1,2,3'
                }
            };

            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            const result = filter.instance().isValidParams();

            expect(result).toBeTrue;
        });

        it('isValidParams with invalid mappingStatus returns false', () => {
            // Arrange
            const state = {
                filter: {
                    mappingStatuses: ''
                }
            };

            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);

            const result = filter.instance().isValidParams();

            expect(result).toBeFalse;
        });

        it('isValidParams with invalid mappingStatus call toast', () => {
            // Arrange
            const event = {
                preventDefault: jest.fn()
            };
            const props = {
                gridParams: {
                    pageIndex: 1,
                    pageSize: 100
                },
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            toast.error = jest.fn();
            filter.instance().isValidParams = jest.fn().mockReturnValue(false);
            filter.instance().submitHandler = jest.fn();

            filter.find(Form).simulate('submit', event);

            expect(toast.error).toHaveBeenCalled();
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
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({})),
                setGridParams: jest.fn(),
                getGridData: jest.fn((params) => Promise.resolve([]))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().isValidParams = jest.fn().mockReturnValue(true);
            filter.instance().getParams = jest.fn().mockReturnValue(params);

            // Action
            filter.instance().submitHandler(event);

            // Assert
            expect(event.preventDefault).toHaveBeenCalled();
            expect(props.setGridParams).toHaveBeenCalledWith(params);
            expect(props.getGridData).toHaveBeenCalledWith({ ...params });
        });
    });

    describe('handle form group change', () => {
        it('handle form fields onChage event', () => {
            const event = {
                target: {
                    value: 'value'
                }
            };

            // Arrange
            const state = {
                filter: {
                    chatType: '',
                    name: '',
                    groupHeadline: '',
                    recipient: '',
                    letstalkID: '',
                    mappingStatuses: '1,2,3',
                    languageId: '',
                    botId: '',
                    active: ''
                }
            };

            const props = {
                isHost: true,
                gridParams: {},
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({})),
                setGridParams: jest.fn(),
                getGridData: jest.fn((params) => Promise.resolve([]))
            };

            const filter = shallow(<Filter {...props} />);
            filter.instance().setState(state);
            filter.instance().onChange = jest.fn();

            filter.find(Form.Group).at(0).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('chatType', event.target.value);

            filter.find(Form.Group).at(1).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('name', event.target.value);

            filter.find(Form.Group).at(2).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('groupHeadline', event.target.value);

            filter.find(Form.Group).at(3).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('recipient', event.target.value);

            filter.find(Form.Group).at(4).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('letstalkID', event.target.value);

            filter.find(Form.Group).at(5).find(MultipleSelect).simulate('change', 'currentItem', event.target.value);
            expect(filter.instance().onChange).toHaveBeenCalledWith('mappingStatuses', event.target.value);

            filter.find(Form.Group).at(6).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('languageId', event.target.value);

            filter.find(Form.Group).at(7).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('active', event.target.value);

            filter.find(Form.Group).at(8).find(Form.Control).simulate('change', event);
            expect(filter.instance().onChange).toHaveBeenCalledWith('botId', event.target.value);
        });
    });

    describe('Insert personal mapping', () => {
        it('If not have isHost permission will not show Insert Personal Chat Mapping button', () => {
            const props = {
                gridParams: {},
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({}))
            };

            const filter = shallow(<Filter {...props} />);
            expect(filter.find(Form.Group).find(Button).length).toBe(1);
            expect(filter.find(Form.Group).find(Button).at(0).html()).not.toContain('InsertPersonalMappingBtn');
        });

        it('If have host permission will show Insert Personal Chat Mapping button and should call props openInsertPersonalMappingPopup when click', () => {
            const props = {
                isHost: true,
                gridParams: {},
                insertPersonalMappingData: {},
                insertPersonalMappingResult: {},
                getSettings: jest.fn(() => Promise.resolve({})),
                openInsertPersonalMappingPopup: jest.fn()
            };

            const filter = shallow(<Filter {...props} />);
            expect(filter.find(Form.Group).find(Button).length).toBe(2);
            expect(filter.find(Form.Group).find(Button).at(1).html()).toContain('InsertPersonalMappingBtn');
            filter.find(Button).at(1).simulate('click');
            expect(props.openInsertPersonalMappingPopup).toHaveBeenCalled();
        });
    });

    describe('InsertPersonalMappingPopup', () => {
        const props = {
            isHost: true,
            gridParams: {},
            insertPersonalMappingData: { isShow: true },
            insertPersonalMappingResult: {},
            getSettings: jest.fn(() => Promise.resolve({})),
            openInsertPersonalMappingPopup: jest.fn()
        };

        const filter = shallow(<Filter {...props} />);
        expect(filter.find(InsertPersonalMappingPopup).length).toBe(1);
    });
});