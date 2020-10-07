import React from 'react';
import EditPersonalMappingPopupWrapper, { EditPersonalMappingPopup } from '../EditPersonalMappingPopup';
import { shallow, mount } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import { Form, Modal, Button } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Promise } from 'q';
import SelectWithSearch from 'react-select';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import MultipleSelect from '@jsroot/components/multiple-select/index';

describe('EditPersonalMappingPopup', () => {
    const initData = {
        conversationGroupId: 1,
        botId: 1,
        name: 'Name',
        conversationId: 'aaa',
        recipientType: 1,
        headlines: [{ id: 1, name: 'headline' }],
        recipients: [{ id: 1, name: 'recipient' }],
        letstalkId: '12',
        languageId: 1,
        remark: '123'
    };
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
            updatePersonalMappingData: { data: initData },
            updatePersonalMappingResult: {}
        };

        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<EditPersonalMappingPopupWrapper store={store} />);

        // Assert
        it('component renders correctly', () => {
            const grid = shallow(<EditPersonalMappingPopupWrapper store={store} />);
            expect(grid).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.recipients).toEqual(initialState.recipients);
            expect(wrapper.props().children.props.groupHeadline).toEqual(initialState.groupHeadline);
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.updatePersonalMappingResult).toEqual(initialState.updatePersonalMappingResult);
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
            updatePersonalMappingData: { data: initData },
            updatePersonalMappingResult: { isSuccessful: true },
            letstalkName: '',
            getLetstalkDisplayName: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(),
            getGroupHeadlineByRecipient: jest.fn(),
            setUpdatePersonalMappingResult: jest.fn(),
            updatePersonalMapping: jest.fn(),
            closeUpdatePersonalMappingPopup: jest.fn(),
            getGridData: jest.fn()
        };

        it('handleChange runs correctly', () => {
            // Arrange
            const value = 'value';

            const popup = shallow(<EditPersonalMappingPopup {...props} />);

            // Action
            popup.instance().handleChange('chatType', value);

            // Assert
            expect(popup.instance().state.form.chatType).toBe(value);
        });

        it('handle functions should call correct functions with correct params when event triggered', () => {
            const event = {
                target: { value: 'value' },
                preventDefault: jest.fn()
            };
            const popup = mount(<EditPersonalMappingPopup {...props} />);
            popup.instance().handleGroupHeadlineChange = jest.fn();
            popup.instance().handleRecipientTypeChange = jest.fn();
            popup.instance().handleRecipientChange = jest.fn();
            popup.instance().handleLetstalkSelect = jest.fn();
            popup.instance().handleChange = jest.fn();
            popup.instance().onClosePopup = jest.fn();
            popup.instance().handleSubmit = jest.fn();

            popup.find(Form.Group).at(0).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('botId', 'value');

            popup.find(Form.Group).at(3).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleRecipientTypeChange).toHaveBeenCalledWith('value');

            expect(popup.find(Form.Group).at(4).find(SelectWithSearch).length).toBe(1);
            expect(popup.find(Form.Group).at(6).find(MultipleSelect).length).toBe(1);

            popup.find(Form.Group).at(7).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('languageId', 'value');

            popup.find(Form.Group).at(8).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('remark', 'value');
        });

        it('render correct', () => {
            const state = {
                isShowConfirmDirtyForm: true
            };
            const popup = shallow(<EditPersonalMappingPopup {...props} />);
            popup.instance().setState(state);

            expect(popup.find(ConfirmPopup).length).toBe(1);
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
            updatePersonalMappingData: { data: initData },
            updatePersonalMappingResult: {},
            letstalkName: '',
            getLetstalkDisplayName: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(),
            getGroupHeadlineByRecipient: jest.fn(),
            setUpdatePersonalMappingResult: jest.fn(),
            updatePersonalMapping: jest.fn(() => promise),
            closeUpdatePersonalMappingPopup: jest.fn(),
            getGridData: jest.fn()
        };
        const popup = shallow(<EditPersonalMappingPopup {...props} />);

        it('handleGroupHeadlineChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().handleGroupHeadlineChange('currentItem', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', 'value');
        });

        it('handleRecipientTypeChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().handleRecipientTypeChange('value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipientType', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipient', { value: '' });
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', '');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('conversationId', '');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('name', '');
            expect(props.getRecipientByRecipientType).toHaveBeenCalledWith('value');
        });

        it('handleRecipientChange with call correct props with correct params', () => {
            const option = { value: 'value', letstalkIds: [1, 2, 3] };
            popup.instance().handleChange = jest.fn();
            popup.instance().handleRecipientChange(option);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipient', option);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', '');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('conversationId', '');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('name', '');
            expect(popup.instance().state.settings.letstalkIds).toEqual('');
            expect(props.getGroupHeadlineByRecipient).toHaveBeenCalledWith('value');
        });

        it('handleLetstalkSelect with call correct props with correct params', () => {
            const event = {
                target: { value: 'value' }
            };
            popup.instance().handleChange = jest.fn();
            popup.instance().handleLetstalkSelect(event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('conversationId', 'value');
            expect(props.getLetstalkDisplayName).toHaveBeenCalledWith(1, 'value');
            promise.then(() => {
                expect(popup.instance().handleChange).toHaveBeenCalledWith('name', props.letstalkName);
            });
        });

        it('renderOptions return expected result', () => {
            const item = {
                id: 1, name: 'test'
            };
            const expected = <option key={`${0}-${item.id}`} value={item.id}>{item.name}</option>;
            const actual = popup.instance().renderOptions(item, 0);
            expect(actual).toEqual(expected);
        });

        it('renderLetstalkIds when state.letstalkIds is null return null', () => {
            const state = {
                settings: { letstalkIds: '' }
            };
            popup.instance().setState(state);
            const actual = popup.instance().renderLetstalkIds();
            expect(actual).toBe(null);
        });

        it('renderLetstalkIds when state.letstalkIds is correct return expected radio input', () => {
            popup.instance().state.settings.letstalkIds = '1,2,3,4';
            const actual = popup.instance().renderLetstalkIds();
            expect(actual.length).toBe(4);
        });

        it('isValidParams return expected results invalid', () => {
            const state = {
                form: {
                    botId: '',
                    displayName: '',
                    recipientType: '',
                    groupHeadlines: '',
                    recipient: '',
                    letstalkId: '',
                    languageId: '',
                }
            };
            popup.instance().setState(state);
            const actual = popup.instance().isValidParams();
            expect(popup.instance().state.validate.isShow).toBeTrue;
            expect(popup.instance().state.validate.errMsgs.length).toBe(6);
            expect(actual).toBeFalse;
        });

        it('isValidParams return valid validation', () => {
            const state = {
                form: {
                    botId: '1',
                    recipientType: 'a',
                    recipient: '1',
                    conversationId: '1',
                    groupHeadlines: '1',
                    languageId: '1'
                }
            };
            popup.instance().setState(state);
            const actual = popup.instance().isValidParams();
            expect(popup.instance().state.validate.isShow).toBeFalse;
            expect(popup.instance().state.validate.errMsgs.length).toBe(0);
            expect(actual).toBeTrue;
        });

        it('getParams return expected results', () => {
            const state = {
                form: {
                    botId: '1',
                    recipientType: 'a',
                    recipient: '1',
                    conversationId: '1',
                    groupHeadlines: '1',
                    languageId: '1'
                }
            };
            popup.instance().setState(state);
            const expected = {
                conversationGroupId: 1,
                botId: state.form.botId,
                recipientTypeId: state.form.recipientType,
                letstalkId: state.form.conversationId,
                groupHeadlineIds: state.form.groupHeadlines,
                recipientIds: state.form.recipient.value,
                languageId: state.form.languageId,
                remark: state.form.remark
            };
            const actual = popup.instance().getParams();
            expect(actual).toEqual(expected);
        });

        it('handleSumit will call expected props when InValid params', () => {
            const event = { preventDefault: jest.fn() };
            popup.instance().isValidParams = jest.fn().mockReturnValue(false);
            popup.instance().handleSubmit(event);
            expect(props.setUpdatePersonalMappingResult).toHaveBeenCalledWith(null);
            expect(props.closeUpdatePersonalMappingPopup).not.toHaveBeenCalled();
            expect(props.getGridData).not.toHaveBeenCalled();
        });

        it('handleSumit will call expected props when Valid params', () => {
            const event = { preventDefault: jest.fn() };
            popup.instance().isValidParams = jest.fn().mockReturnValue(true);
            popup.instance().getParams = jest.fn().mockReturnValue('valueParams');
            popup.instance().handleSubmit(event);

            expect(props.setUpdatePersonalMappingResult).toHaveBeenCalledWith(null);
            expect(props.updatePersonalMapping).toHaveBeenCalledWith('valueParams');
            promise.then(() => {
                expect(props.closeUpdatePersonalMappingPopup).toHaveBeenCalled();
                expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
            });
        });

        it('isDirtyForm return expected true value', () => {
            const initState = {
                botId: '',
                displayName: '',
                recipientType: '',
                groupHeadlines: '',
                recipient: '',
                letstalkId: '',
                languageId: ''
            };

            const state = {
                form: {
                    botId: '1',
                    displayName: 'a',
                    recipientType: '1',
                    groupHeadlines: '1',
                    recipient: '1',
                    letstalkId: '1',
                    languageId: '1'
                }
            };

            popup.instance().initState = initState;
            popup.instance().setState(state);
            const actual = popup.instance().isDirtyForm();
            expect(actual).toBeTrue;
        });

        it('isDirtyForm return expected false value', () => {
            const initState = {
                botId: '',
                displayName: '',
                recipientType: '',
                groupHeadlines: '',
                recipient: '',
                letstalkId: '',
                languageId: ''
            };

            const state = {
                form: {
                    botId: '',
                    displayName: '',
                    recipientType: '',
                    groupHeadlines: '',
                    recipient: '',
                    letstalkId: '',
                    languageId: ''
                }
            };

            popup.instance().initState = initState;
            popup.instance().setState(state);
            const actual = popup.instance().isDirtyForm();
            expect(actual).toBeFalse;
        });

        it('onClosePopup when is dirty form will call setState', () => {
            const initState = {
                dirtyFormConfirmation: { isShow: false }
            };
            popup.instance().setState(initState);
            popup.instance().isDirtyForm = jest.fn().mockReturnValue(true);
            popup.instance().onClosePopup();
            expect(popup.instance().state.isShowConfirmDirtyForm).toBeTrue;
            expect(props.closeUpdatePersonalMappingPopup).not.toHaveBeenCalled();
        });

        it('onClosePopup when is not dirty form will call correct props', () => {
            const initState = {
                isShowConfirmDirtyForm: false
            };
            popup.instance().setState(initState);
            popup.instance().isDirtyForm = jest.fn().mockReturnValue(false);
            popup.instance().onClosePopup();
            expect(popup.instance().state.isShowConfirmDirtyForm).toBeFalse;
            expect(props.closeUpdatePersonalMappingPopup).toHaveBeenCalled();
        });

        it('closeConfirmDirtyForm will do expected action to change state dirtyFormConfirmation', () => {
            const initState = {
                dirtyFormConfirmation: { isShow: true }
            };
            popup.instance().setState(initState);
            popup.instance().closeConfirmDirtyForm();
            expect(popup.instance().state.dirtyFormConfirmation.isShow).toBeFalse;
        });
    });
});