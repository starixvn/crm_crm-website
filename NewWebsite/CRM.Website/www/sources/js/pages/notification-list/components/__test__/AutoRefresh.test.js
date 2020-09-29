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

    it('componentDidMount runs correctly', () => {
        // Arrange
        const props = {
            autoRefresh: {
                value: 90,
                countDown: 90
            }
        };

        const autoRefresh = shallow(<AutoRefresh {...props} />);
        autoRefresh.instance().countDownFunc = jest.fn();
        document.addEventListener = jest.fn();

        // Action
        autoRefresh.instance().componentDidMount();

        // Assert
        expect(autoRefresh.instance().countDownFunc).toHaveBeenCalled();
        expect(document.addEventListener).toHaveBeenCalledWith('mousedown', expect.any(Function));
    });

    it('componentWillUnmount runs correctly', () => {
        // Arrange
        const props = {
            autoRefresh: {
                value: 90,
                countDown: 90
            }
        };

        jest.useFakeTimers();
        const autoRefresh = shallow(<AutoRefresh {...props} />);
        document.removeEventListener = jest.fn();

        // Action
        autoRefresh.instance().componentWillUnmount();

        // Assert
        expect(clearInterval).toHaveBeenCalledWith(expect.any(Function));
        expect(document.removeEventListener).toHaveBeenCalledWith('mousedown', expect.any(Function));
    });

    describe('handleClickOutside runs correctly', () => {
        it('not has autoRefresh', () => {
            // Arrange
            const expectedState = { isOpenDropdown: true };
            const event = {
                target: {}
            };

            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 90
                }
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().setState(expectedState);
            autoRefresh.instance().autoRefresh = null;
    
            // Action
            autoRefresh.instance().handleClickOutside(event);
    
            // Assert
            expect(autoRefresh.instance().state.isOpenDropdown).toBeTrue;
        });
    });
});