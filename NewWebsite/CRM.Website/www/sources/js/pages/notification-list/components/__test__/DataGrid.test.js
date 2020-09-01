import React from 'react';
import DataGridWrapper, { DataGrid } from '../DataGrid';
import { shallow } from 'enzyme';
import configureMockStore from 'redux-mock-store';

describe('DataGrid', () => {
    describe('component with redux', () => {
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {},
            gridData: {},
            resendOptions: {}
        };

        const store = mockStore(initialState);
        const wrapper = shallow(<DataGridWrapper store={store} />)

        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            console.log('merlin', wrapper.props())
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
            expect(wrapper.props().children.props.gridData).toEqual(initialState.gridData);
            expect(wrapper.props().children.props.resendOptions).toEqual(initialState.resendOptions);
        });
    });
});