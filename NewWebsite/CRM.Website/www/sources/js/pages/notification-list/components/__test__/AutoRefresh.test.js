import React from 'react';
import AutoRefreshWrapper, { AutoRefresh } from '../AutoRefresh';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';

describe('AutoRefresh', () => {
    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            autoRefresh: {},
            isRefresh: true
        };

        const store = mockStore(initialState);
        
        // Action
        const wrapper = shallow(<AutoRefreshWrapper store={store} />);

        // Assert
        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.gridParams).toEqual(
                initialState.gridParams
            );
            expect(wrapper.props().children.props.autoRefresh).toEqual(
                initialState.autoRefresh
            );
            expect(wrapper.props().children.props.isRefresh).toEqual(
                initialState.isRefresh
            );
        });
    });

    it('has correct default state', () => {
        // Arrange
        const props = {
            autoRefresh: {
                value: 90,
                countDown: 90
            }
        };

        // Action
        const autoRefresh = shallow(<AutoRefresh {...props} />);

        // Assert
        expect(autoRefresh.instance().state.isOpenDropdown).toBeFalse;
    });
});