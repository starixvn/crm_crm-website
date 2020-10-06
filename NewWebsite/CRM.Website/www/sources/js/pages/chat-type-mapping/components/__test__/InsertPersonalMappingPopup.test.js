import React from 'react';
import InsertPersonalMappingPopupWrapper, { InsertPersonalMappingPopup } from '../InsertPersonalMappingPopup';
import { shallow, mount } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import { Form } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import Site from '@jsroot/common/site';
import { contentPopup } from '../../../notification-list/reducers/dataHandlerReducer';
import { toast } from 'react-toastify';
import { Promise } from 'q';
import SelectWithSearch from 'react-select';
import MultipleSelect from '@jsroot/components/multiple-select/index';

describe('DataGrid', () => {
    LangHelper.getResource = jest.fn(value => value);

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            settings: {
                bots: [{ id: 1, name: 'Bot1' }],
                recipientTypes: [],
                languages: []
            },
            recipients: [],
            groupHeadline: [],
            gridParams: {},
            insertPersonalMappingResult: {},
            letstalkName: ''
        };

        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<InsertPersonalMappingPopupWrapper store={store} />);

        // Assert
        it('component renders correctly', () => {
            const grid = mount(<InsertPersonalMappingPopupWrapper store={store} />);
            expect(grid).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.recipients).toEqual(initialState.recipients);
            expect(wrapper.props().children.props.groupHeadline).toEqual(initialState.groupHeadline);
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.insertPersonalMappingResult).toEqual(initialState.insertPersonalMappingResult);
            expect(wrapper.props().children.props.letstalkName).toEqual(initialState.letstalkName);
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

    describe('handle change function tests', () => {
        const promise = Promise.resolve([]);
        const props = {
            settings: {
                bots: [{ id: 1, name: 'Bot1' }],
                recipientTypes: [],
                languages: []
            },
            recipients: [],
            groupHeadline: [],
            gridParams: {},
            insertPersonalMappingResult: {},
            letstalkName: '',
            getLetstalkDisplayName: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(),
            getGroupHeadlineByRecipient: jest.fn(),
            setInsertPersonalMappingResult: jest.fn(),
            insertPersonalMapping: jest.fn(),
            closeInsertPersonalMappingPopup: jest.fn(),
            getGridData: jest.fn()
        };

        it('handleChange runs correctly', () => {
            // Arrange
            const value = 'value';

            const popup = shallow(<InsertPersonalMappingPopup {...props} />);

            // Action
            popup.instance().handleChange('chatType', value);

            // Assert
            expect(popup.instance().state.form.chatType).toBe(value);
        });

        it('handle functions should call correct functions with correct params when event triggered', () => {
            const event = {
                target: { value: 'value' }
            };
            const popup = shallow(<InsertPersonalMappingPopup {...props} />);
            popup.instance().handleBotChange = jest.fn();
            popup.instance().handleGroupHeadlineChange = jest.fn();
            popup.instance().handleRecipientTypeChange = jest.fn();
            popup.instance().handleRecipientChange = jest.fn();
            popup.instance().handleLetstalkSelect = jest.fn();
            popup.instance().handleChange = jest.fn();

            popup.find(Form.Group).at(0).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleBotChange).toHaveBeenCalledWith('value');

            popup.find(Form.Group).at(3).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleRecipientTypeChange).toHaveBeenCalledWith('value');

            expect(popup.find(Form.Group).at(4).find(SelectWithSearch).length).toBe(1);
            expect(popup.find(Form.Group).at(6).find(MultipleSelect).length).toBe(1);

            popup.find(Form.Group).at(7).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('languageId', 'value');
        });
    });

    describe('functions test', () => {
        const promise = Promise.resolve([]);
        const props = {
            settings: {
                bots: [{ id: 1, name: 'Bot1' }],
                recipientTypes: [],
                languages: []
            },
            recipients: [],
            groupHeadline: [],
            gridParams: {},
            insertPersonalMappingResult: {},
            letstalkName: '',
            getLetstalkDisplayName: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(),
            getGroupHeadlineByRecipient: jest.fn(),
            setInsertPersonalMappingResult: jest.fn(),
            insertPersonalMapping: jest.fn(),
            closeInsertPersonalMappingPopup: jest.fn(),
            getGridData: jest.fn()
        };
        const popup = shallow(<InsertPersonalMappingPopup {...props} />);

        it('handleBotChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().handleBotChange('value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('botId', 'value');
            expect(props.getLetstalkDisplayName).toHaveBeenCalledWith('value', '');
            promise.then(() => {
                expect(popup.instance().handleChange).toHaveBeenCalledWith('displayName', props.letstalkName);
            });
        });

        it('handleGroupHeadlineChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().handleGroupHeadlineChange('currentItem', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', 'value');
        });

        it('handleRecipientTypeChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().handleRecipientTypeChange('value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipientType', 'value');
            expect(props.getRecipientByRecipientType).toHaveBeenCalledWith('value');
        });

        it('handleRecipientChange with call correct props with correct params', () => {
            const option = { value: 'value' };
            popup.instance().handleChange = jest.fn();
            popup.instance().handleRecipientChange(option);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipient', option);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('letstalkId', '');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('displayName', '');
            expect(props.getGroupHeadlineByRecipient).toHaveBeenCalledWith('value');
        });
    });
});