import React from 'react';
import AppContainerWrapper, { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';

describe('AppContainer', () => {
    beforeEach(() => {
        const isHost = document.createElement('div');
        isHost.setAttribute('id', 'is-host');
        document.body.appendChild(isHost);
    });

    afterEach(() => {
        const isHost = document.getElementById('is-host');
        if (isHost) {
            document.body.removeChild(isHost);
        }
    });

    describe('component with redux', () => {
        // Arrange
        const mockStore = configureMockStore();
        const initialState = {
            gridParams: {}
        };

        const store = mockStore(initialState);

        // Action
        const wrapper = shallow(<AppContainerWrapper store={store} />);

        // Assert
        it('component render correctly', () => {
            expect(wrapper).toMatchSnapshot();
        });

        it('component has correct props', () => {
            expect(wrapper.props().children.props.gridParams).toEqual(initialState.gridParams);
        });

        it('render correct components', () => {
            const component = shallow(<AppContainer />);
            expect(component.find(Filter).length).toBe(1);
            expect(component.find(DataGrid).length).toBe(1);
        });
    });
});