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
            getLetstalkDisplayName: jest.fn(() => promise),
            getRecipientByRecipientType: jest.fn(() => promise),
            getGroupHeadlineByRecipient: jest.fn(),
            setUpdatePersonalMappingResult: jest.fn(),
            updatePersonalMapping: jest.fn(),
            closeUpdatePersonalMappingPopup: jest.fn(),
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

            popup.find(Form.Group).at(4).find(MultipleSelect).at(0).simulate('onChange', 'currentItem', 'value');
            expect(popup.find(Form.Group).at(4).find(MultipleSelect).length).toBe(1);
            expect(popup.instance().handleRecipientChange).toHaveBeenCalledWith('currentItem', 'value');

            //popup.find(Form.Group).at(7).find(Form.Control).at(0).simulate('change', event);
            //expect(popup.instance().handleChange).toHaveBeenCalledWith('languageId', 'value');

            //popup.find(Form.Group).at(8).find(Form.Control).at(0).simulate('change', event);
            //expect(popup.instance().handleChange).toHaveBeenCalledWith('remark', 'value');
        });

        //it('render correct', () => {
        //    const state = {
        //        isShowConfirmDirtyForm: true
        //    };
        //    const popup = shallow(<EditGroupMappingPopup {...props} />);
        //    popup.instance().setState(state);

        //    expect(popup.find(ConfirmPopup).length).toBe(1);
        //});
    });
});