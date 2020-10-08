import React from 'react';
import EditGroupMappingPopupWrapper, { EditGroupMappingPopup } from '../EditGroupMappingPopup';
import { shallow, mount } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import { Form, Modal, Button } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';
import { Promise } from 'q';
import ConfirmPopup from '@jsroot/components/ConfirmPopup';
import MultipleSelect from '@jsroot/components/multiple-select/index';

describe('EditGroupMappingPopup', () => {
    const initData = {
        conversationGroupId: 1,
        botId: 1,
        botName: 'Bot1',
        conversationGroupName: 'GroupName',
        conversationId: 'conversationId',
        headlines: [{ id: 1, name: 'headline' }],
        recipients: [{ id: 1, name: 'recipient', recipientTypeId: 1 }],
        mappingStatusId: '1',
        mappingStatus: 'NotMapped',
        languageId: 1,
        remark: '123'
    };
    LangHelper.getResource = jest.fn(value => value);

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            settings: {},
            recipients: [],
            groupHeadline: [],
            gridParams: {},
            updateGroupMappingData: { data: initData },
            updateGroupMappingResult: {},
            letstalkVerification: []
        };

        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<EditGroupMappingPopupWrapper store={store} />);

        // Assert
        it('component renders correctly', () => {
            const grid = shallow(<EditGroupMappingPopupWrapper store={store} />);
            expect(grid).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.settings).toEqual(initialState.settings);
            expect(wrapper.props().children.props.recipients).toEqual(initialState.recipients);
            expect(wrapper.props().children.props.groupHeadline).toEqual(initialState.groupHeadline);
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.updateGroupMappingData).toEqual(initialState.updateGroupMappingData);
            expect(wrapper.props().children.props.updateGroupMappingResult).toEqual(initialState.updateGroupMappingResult);
            expect(wrapper.props().children.props.letstalkVerification).toEqual(initialState.letstalkVerification);
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
            updateGroupMappingData: { data: initData },
            updateGroupMappingResult: { isSuccessful: true },
            letstalkVerification: [],
            verifyLetstalkInGroup: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(() => promise),
            getGroupHeadlineByRecipient: jest.fn(),
            getGridData: jest.fn()
        };

        it('handleChange runs correctly', () => {
            const value = 'value';

            const popup = shallow(<EditGroupMappingPopup {...props} />);

            popup.instance().handleChange('recipientIds', value);

            expect(popup.instance().state.form.recipientIds).toBe(value);
        });

        it('componentDidMount should call expected props and execute correctly', () => {
            const expected = {
                bot: {
                    id: initData.botId,
                    name: initData.botName
                },
                name: initData.conversationGroupName,
                conversationId: initData.conversationId,
                recipientTypeId: initData.recipients[0].recipientTypeId,
                recipientIds: initData.recipients.map(r => r.recipientId).toString(),
                groupHeadlines: initData.headlines.map(v => v.headlineId).toString(),
                mappingStatus: {
                    id: initData.mappingStatusId,
                    name: initData.mappingStatus
                },
                languageId: initData.languageId,
                remark: initData.remark
            };
            const popup = shallow(<EditGroupMappingPopup {...props} />);

            popup.instance().verifyLetstalk = jest.fn();
            expect(popup.instance().initState).toEqual(expected);
            expect(props.getRecipientByRecipientType).toHaveBeenCalledWith(expected.recipientTypeId);
            promise.then(() => {
                expect(props.getGroupHeadlineByRecipient).toHaveBeenCalledWith('1');
                expect(popup.instance().verifyLetstalk).toHaveBeenCalledWith(['1']);
            });
        });

        it('handle functions should call correct functions with correct params when event triggered', () => {
            const event = {
                target: { value: 'value' },
                preventDefault: jest.fn()
            };
            const popup = shallow(<EditGroupMappingPopup {...props} />);
            popup.instance().handleGroupHeadlineChange = jest.fn();
            popup.instance().handleRecipientTypeChange = jest.fn();
            popup.instance().handleRecipientChange = jest.fn();
            popup.instance().verifyLetstalk = jest.fn();
            popup.instance().handleChange = jest.fn();
            popup.instance().onClosePopup = jest.fn();
            popup.instance().handleSubmit = jest.fn();

            popup.find(Form.Group).at(3).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleRecipientTypeChange).toHaveBeenCalledWith('value');

            expect(popup.find(Form.Group).at(4).find(MultipleSelect).length).toBe(1);
            expect(popup.find(Form.Group).at(6).find(MultipleSelect).length).toBe(1);

            popup.find(Form.Group).at(8).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('languageId', 'value');

            popup.find(Form.Group).at(9).find(Form.Control).at(0).simulate('change', event);
            expect(popup.instance().handleChange).toHaveBeenCalledWith('remark', 'value');
        });

        it('render correct', () => {
            const state = {
                isShowConfirmDirtyForm: true
            };
            const popup = shallow(<EditGroupMappingPopup {...props} />);
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
            updateGroupMappingData: { data: initData },
            updateGroupMappingResult: {},
            letstalkVerification: [],
            verifyLetstalkInGroup: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(() => promise),
            getGroupHeadlineByRecipient: jest.fn(),
            setGroupMappingResult: jest.fn(),
            updateGroupMapping: jest.fn(() => promise),
            closeUpdateGroupMappingPopup: jest.fn(),
            getGridData: jest.fn()
        };
        const popup = shallow(<EditGroupMappingPopup {...props} />);

        it('handleGroupHeadlineChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().handleGroupHeadlineChange('currentItem', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', 'value');
        });

        it('handleRecipientTypeChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().verifyLetstalk = jest.fn();
            popup.instance().handleRecipientTypeChange('value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipientTypeId', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipientIds', '');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', '');
            expect(popup.instance().verifyLetstalk).toHaveBeenCalledWith([]);
            expect(props.getGroupHeadlineByRecipient).toHaveBeenCalledWith('');
            expect(props.getRecipientByRecipientType).toHaveBeenCalledWith('value');
        });

        it('handleRecipientChange with call correct props with correct params', () => {
            popup.instance().handleChange = jest.fn();
            popup.instance().verifyLetstalk = jest.fn();
            popup.instance().handleRecipientChange('currentItem', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('recipientIds', 'value');
            expect(popup.instance().handleChange).toHaveBeenCalledWith('groupHeadlines', '');
            expect(popup.instance().verifyLetstalk).toHaveBeenCalledWith(['value']);
            expect(props.getGroupHeadlineByRecipient).toHaveBeenCalledWith('value');
        });

        it('verifyLetstalk with call correct props with correct params', () => {
            const recipientIds = '1,2,3';
            popup.instance().verifyLetstalk(recipientIds);
            expect(props.verifyLetstalkInGroup).not.toHaveBeenCalled();
        });

        it('renderOptions return expected result', () => {
            const item = {
                id: 1, name: 'test'
            };
            const expected = <option key={`${0}-${item.id}`} value={item.id}>{item.name}</option>;
            const actual = popup.instance().renderOptions(item, 0);
            expect(actual).toEqual(expected);
        });

        it('isValidParams return expected results invalid', () => {
            const state = {
                form: {
                    bot: { id: 1, name: 'bot' },
                    name: 'a',
                    conversationId: '1',
                    recipientTypeId: '',
                    recipientIds: '',
                    mappingStatus: { id: 1, name: '' },
                    groupHeadlines: '',
                    languageId: '',
                    remark: ''
                }
            };
            popup.instance().setState(state);
            const actual = popup.instance().isValidParams();
            expect(popup.instance().state.validate.isShow).toBeTrue;
            expect(popup.instance().state.validate.errMsgs.length).toBe(4);
            expect(actual).toBeFalse;
        });

        it('isValidParams return valid validation', () => {
            const state = {
                form: {
                    bot: { id: 1, name: 'bot' },
                    name: 'a',
                    conversationId: '1',
                    recipientTypeId: '1',
                    recipientIds: '2',
                    groupHeadlines: '3',
                    languageId: '4',
                    remark: ''
                }
            };
            popup.instance().setState(state);
            const actual = popup.instance().isValidParams();
            expect(popup.instance().state.validate.isShow).toBeFalse;
            expect(popup.instance().state.validate.errMsgs.length).toBe(0);
            expect(actual).toBeTrue;
        });

        it('getParams return expected results', () => {
            const expected = {
                conversationGroupId: 1,
                botId: 1,
                name: 'a',
                recipientTypeId: '',
                letstalkId: '1',
                groupHeadlineIds: '',
                recipientIds: '',
                languageId: '',
                remark: ''
            };
            const actual = popup.instance().getParams();
            expect(actual).toEqual(expected);
        });

        it('handleSumit will call expected props when InValid params', () => {
            const event = { preventDefault: jest.fn() };
            popup.instance().isValidParams = jest.fn().mockReturnValue(false);
            popup.instance().handleSubmit(event);
            expect(props.setGroupMappingResult).toHaveBeenCalledWith(null);
            expect(props.closeUpdateGroupMappingPopup).not.toHaveBeenCalled();
            expect(props.getGridData).not.toHaveBeenCalled();
        });

        it('handleSumit will call expected props when Valid params', () => {
            const event = { preventDefault: jest.fn() };
            popup.instance().isValidParams = jest.fn().mockReturnValue(true);
            popup.instance().getParams = jest.fn().mockReturnValue('valueParams');
            popup.instance().handleSubmit(event);

            expect(props.setGroupMappingResult).toHaveBeenCalledWith(null);
            expect(props.updateGroupMapping).toHaveBeenCalledWith('valueParams');
            promise.then(() => {
                expect(props.closeUpdateGroupMappingPopup).toHaveBeenCalled();
                expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
            });
        });

        it('isDirtyForm return expected true value', () => {
            const initState = {
                bot: { id: 1, name: 'bot' },
                name: 'a',
                conversationId: '1',
                recipientTypeId: '1',
                recipientIds: '2',
                groupHeadlines: '3',
                languageId: '4',
                remark: ''
            };

            const state = {
                form: {
                    bot: { id: 1, name: 'bot' },
                    name: 'a',
                    conversationId: '1',
                    recipientTypeId: '1',
                    recipientIds: '2',
                    groupHeadlines: '3',
                    languageId: '4',
                    remark: ''
                }
            };

            popup.instance().initState = initState;
            popup.instance().setState(state);
            const actual = popup.instance().isDirtyForm();
            expect(actual).toBeTrue;
        });

        it('isDirtyForm return expected false value', () => {
            const initState = {
                bot: { id: 1, name: 'bot' },
                name: 'a',
                conversationId: '1',
                recipientTypeId: '1',
                recipientIds: '2',
                groupHeadlines: '3',
                languageId: '4',
                remark: ''
            };

            const state = {
                form: {
                    bot: { id: 1, name: 'bot' },
                    name: 'a',
                    conversationId: '1',
                    recipientTypeId: '1',
                    recipientIds: '2',
                    groupHeadlines: '3,4',
                    languageId: '4',
                    remark: ''
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
            expect(props.closeUpdateGroupMappingPopup).not.toHaveBeenCalled();
        });

        it('onClosePopup when is not dirty form will call correct props', () => {
            const initState = {
                isShowConfirmDirtyForm: false
            };
            popup.instance().setState(initState);
            popup.instance().isDirtyForm = jest.fn().mockReturnValue(false);
            popup.instance().onClosePopup();
            expect(popup.instance().state.isShowConfirmDirtyForm).toBeFalse;
            expect(props.closeUpdateGroupMappingPopup).toHaveBeenCalled();
        });

        it('closeConfirmDirtyForm will do expected action to change state dirtyFormConfirmation', () => {
            const initState = {
                dirtyFormConfirmation: { isShow: true }
            };
            popup.instance().setState(initState);
            popup.instance().closeConfirmDirtyForm();
            expect(popup.instance().state.isShowConfirmDirtyForm).toBeFalse;
        });
    });
});