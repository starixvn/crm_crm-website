import React from 'react';
import AutoRefreshWrapper, { AutoRefresh } from '../AutoRefresh';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';
import configureMockStore from 'redux-mock-store';
import functions from '../../functions';
import { AUTO_REFRESH_TIMES } from '@jsroot/common/constants';

describe('AutoRefresh', () => {
    LangHelper.getResource = jest.fn(value => value);

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

    describe('handleClickOutside runs', () => {
        it('should state isOpenDropdown not change when not has autoRefresh', () => {
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
            autoRefresh.instance().componentDidMount();
    
            // Action
            autoRefresh.instance().handleClickOutside(event);
    
            // Assert
            expect(autoRefresh.instance().state.isOpenDropdown).toBeTrue;
        });
        
        it('should state isOpenDropdown not change when contains event target', () => {
            // Arrange
            const expectedState = { isOpenDropdown: true };
            const mockRef = {
                event: {},
                contains: jest.fn().mockReturnValue(true)
            };

            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 90
                }
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().autoRefresh.current = mockRef;
            autoRefresh.instance().setState(expectedState);
    
            // Action
            autoRefresh.instance().handleClickOutside(mockRef.event);
    
            // Assert
            expect(autoRefresh.instance().state.isOpenDropdown).toBeTrue;
        });
        
        it('should state isOpenDropdown not change when isOpenDropdown false', () => {
            // Arrange
            const expectedState = { isOpenDropdown: false };
            const mockRef = {
                event: {},
                contains: jest.fn().mockReturnValue(false)
            };

            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 90
                }
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().autoRefresh.current = mockRef;
            autoRefresh.instance().setState(expectedState);
    
            // Action
            autoRefresh.instance().handleClickOutside(mockRef.event);
    
            // Assert
            expect(autoRefresh.instance().state.isOpenDropdown).toBeFalse;
        });
        
        it('should state isOpenDropdown change', () => {
            // Arrange
            const expectedState = { isOpenDropdown: true };
            const mockRef = {
                event: {},
                contains: jest.fn().mockReturnValue(false)
            };

            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 90
                }
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().autoRefresh.current = mockRef;
            autoRefresh.instance().setState(expectedState);
    
            // Action
            autoRefresh.instance().handleClickOutside(mockRef.event);
    
            // Assert
            expect(autoRefresh.instance().state.isOpenDropdown).toBeFalse;
        });
    });

    describe('componentDidUpdate runs', () => {
        it('should call setAutoRefresh not when props not changes', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                },
                setAutoRefresh: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);

            // Action
            autoRefresh.instance().componentDidUpdate(props);

            // Assert
            expect(props.setAutoRefresh).toHaveBeenCalledTimes(0);
        });
        
        it('should call setAutoRefresh when props changes', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                },
                setAutoRefresh: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);

            // Action
            autoRefresh.instance().componentDidUpdate({ autoRefresh: {}});

            // Assert
            expect(props.setAutoRefresh).toHaveBeenCalledWith({
                ...props.autoRefresh,
                countDown: props.autoRefresh.value
            });
        });
        
        it('should not call setAutoRefresh and getGridData when autoRefresh countDown not equal zero', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    countDown: 90
                },
                setAutoRefresh: jest.fn(),
                getGridData: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);

            // Action
            autoRefresh.instance().componentDidUpdate(props);

            // Assert
            expect(props.setAutoRefresh).toHaveBeenCalledTimes(0);
            expect(props.getGridData).toHaveBeenCalledTimes(0);
        });
        
        it('should not call setAutoRefresh and getGridData when isRefresh false', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    countDown: 0
                },
                isRefresh: false,
                setAutoRefresh: jest.fn(),
                getGridData: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);

            // Action
            autoRefresh.instance().componentDidUpdate(props);

            // Assert
            expect(props.setAutoRefresh).toHaveBeenCalledTimes(0);
            expect(props.getGridData).toHaveBeenCalledTimes(0);
        });
        
        it('should not call setAutoRefresh and getGridData when autoRefresh value not greater than zero', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 0,
                    countDown: 0
                },
                isRefresh: true,
                setAutoRefresh: jest.fn(),
                getGridData: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);

            // Action
            autoRefresh.instance().componentDidUpdate(props);

            // Assert
            expect(props.setAutoRefresh).toHaveBeenCalledTimes(0);
            expect(props.getGridData).toHaveBeenCalledTimes(0);
        });
        
        it('should call setAutoRefresh and getGridData when autoRefresh countDown equal zero, isRefresh true and autoRefresh value greater than zero', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 0
                },
                isRefresh: true,
                gridParams: {},
                setAutoRefresh: jest.fn(),
                getGridData: jest.fn((params, callback) => callback([])),
                setResendOptions: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            functions.handleNewResendOptions = jest.fn();

            // Action
            autoRefresh.instance().componentDidUpdate(props);

            // Assert
            expect(props.setAutoRefresh).toHaveBeenCalledWith({
                ...props.autoRefresh,
                countDown: props.autoRefresh.value
            });
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams, expect.any(Function));
        });
    });

    describe('countDownFunc runs', () => {
        it('should not call setAutoRefresh when isRefresh false', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                },
                isRefresh: false,
                setAutoRefresh: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            jest.useFakeTimers();

            // Action
            autoRefresh.instance().countDownFunc();

            // Assert
            expect(setInterval).toHaveBeenCalledWith(expect.any(Function), 1000);
            jest.advanceTimersByTime(1000);
            expect(props.setAutoRefresh).toHaveBeenCalledTimes(0);
        });
        
        it('should call setAutoRefresh when isRefresh true', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 90
                },
                isRefresh: true,
                setAutoRefresh: jest.fn()
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            jest.useFakeTimers();

            // Action
            autoRefresh.instance().countDownFunc();

            // Assert
            expect(setInterval).toHaveBeenCalledWith(expect.any(Function), 1000);
            jest.advanceTimersByTime(1000);
            expect(props.setAutoRefresh).toHaveBeenCalledWith({
                ...props.autoRefresh,
                countDown: props.autoRefresh.countDown - 1
            });
        });
    });

    it('handleAutoRefreshChange runs correctly', () => {
        // Arrange
        const value = 10;
        const state = {
            isOpenDropdown: true
        };

        const props = {
            autoRefresh: {
                value: 90
            },
            setIsRefresh: jest.fn(),
            setAutoRefresh: jest.fn()
        };

        const autoRefresh = shallow(<AutoRefresh {...props} />);
        autoRefresh.instance().setState(state);

        // Action
        autoRefresh.instance().handleAutoRefreshChange(value);

        // Assert
        expect(autoRefresh.instance().state.isOpenDropdown).toBeFalse;
        expect(props.setIsRefresh).toHaveBeenCalledWith(value !== 0);
        expect(props.setAutoRefresh).toHaveBeenCalledWith({
            ...props.autoRefresh,
            value: value
        });
    });

    it('handleRefresh runs correctly', () => {
        // Arrange
        const props = {
            autoRefresh: {
                value: 90
            },
            gridParams: {},
            getGridData: jest.fn((params, callback) => callback([])),
            setAutoRefresh: jest.fn(),
            setResendOptions: jest.fn()
        };

        const autoRefresh = shallow(<AutoRefresh {...props} />);

        // Action
        autoRefresh.instance().handleRefresh();

        // Assert
        expect(props.setAutoRefresh).toHaveBeenCalledWith({
            ...props.autoRefresh,
            countDown: props.autoRefresh.value
        });
        expect(props.getGridData).toHaveBeenCalledWith(props.gridParams, expect.any(Function));
    });

    it('toggleAutoFreshDropdown runs correctly', () => {
        // Arrange
        const state = { isOpenDropdown: false };
        const props = {
            autoRefresh: {
                value: 90
            }
        };

        const autoRefresh = shallow(<AutoRefresh {...props} />);
        autoRefresh.instance().setState(state);

        // Action
        autoRefresh.instance().toggleAutoFreshDropdown();

        // Assert
        expect(autoRefresh.instance().state.isOpenDropdown).toBeTrue;
    });

    describe('render', () => {
        it('returns correct first div', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
    
            // Assert
            expect(autoRefresh.find('div').first().props().className).toBe('d-flex align-items-center');
        });
        
        it('returns correct second div', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
    
            // Assert
            expect(autoRefresh.find('div').at(1).props().className).toBe('auto-refresh');
        });
        
        it('returns correct third div', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
    
            // Assert
            expect(autoRefresh.find('div').at(2).props().className)
                .toBe('d-flex justify-content-between align-items-center auto-refresh-value');
        });
        
        it('has third div onClick', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90
                }
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().toggleAutoFreshDropdown = jest.fn();

            // Action
            autoRefresh.find('div').at(2).simulate('click');
    
            // Assert
            expect(autoRefresh.instance().toggleAutoFreshDropdown).toHaveBeenCalled();
        });
        
        it('has fourth div when value not equal zero', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 90,
                    countDown: 15
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
    
            // Assert
            expect(autoRefresh.find('div').at(3).html()).toBe(`<div>${props.autoRefresh.countDown}</div>`);
        });
        
        it('has fourth div when value not zero', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
    
            // Assert
            expect(autoRefresh.find('div').at(3).html()).toBe('<div>NotRefresh</div>');
        });
        
        it('has first span when isOpenDropdown true', () => {
            // Arrange
            const state = { isOpenDropdown: true };
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().setState(state);
    
            // Assert
            expect(autoRefresh.find('span').first().html()).toBe(`<span class="fa fa-caret-up"></span>`);
        });
        
        it('has first span when isOpenDropdown false', () => {
            // Arrange
            const state = { isOpenDropdown: false };
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().setState(state);
    
            // Assert
            expect(autoRefresh.find('span').first().html()).toBe(`<span class="fa fa-caret-down"></span>`);
        });
        
        it('has ul when isOpenDropdown true', () => {
            // Arrange
            const state = { isOpenDropdown: true };
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().setState(state);
    
            // Assert
            expect(autoRefresh.find('ul').props().className).toBe('auto-refresh-dropdown');
        });
        
        it('has ul with correct children', () => {
            // Arrange
            const state = { isOpenDropdown: true };
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().setState(state);
    
            // Assert
            for (let index = 0; index < AUTO_REFRESH_TIMES.length; index++) {
                expect(autoRefresh.find('ul').find('li').at(index).props().value).toEqual(AUTO_REFRESH_TIMES[index]);
                expect(autoRefresh.find('ul').find('li').at(index).html()).toContain(AUTO_REFRESH_TIMES[index]);
            }

            expect(autoRefresh.find('ul').find('li').at(AUTO_REFRESH_TIMES.length).props().value).toEqual(0);
            expect(autoRefresh.find('ul').find('li').at(AUTO_REFRESH_TIMES.length).html()).toContain('NotRefresh');
        });
        
        function simulateClickLi(index, value) {
            it(`at index ${index}`, () => {
                // Arrange
                const state = { isOpenDropdown: true };
                const props = {
                    autoRefresh: {
                        value: 0
                    }
                };
    
                const autoRefresh = shallow(<AutoRefresh {...props} />);
                autoRefresh.instance().setState(state);
                autoRefresh.instance().handleAutoRefreshChange = jest.fn();

                // Action
                autoRefresh.find('ul').find('li').at(index).simulate('click', value);
        
                // Assert
                expect(autoRefresh.instance().handleAutoRefreshChange).toHaveBeenCalledWith(value);
            });
        }

        describe('has ul with li onclick', () => {
            for (let index = 0; index < AUTO_REFRESH_TIMES.length; index++) {
                simulateClickLi(index, AUTO_REFRESH_TIMES[index]);
            }

            simulateClickLi(AUTO_REFRESH_TIMES.length, 0);
        });
        
        it('has correct second span', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            // Action
            const autoRefresh = shallow(<AutoRefresh {...props} />);
    
            // Assert
            expect(autoRefresh.find('span').at(1).html()).toBe(`<span class="fa fa-refresh link-click"></span>`);
        });
        
        it('has correct second span click', () => {
            // Arrange
            const props = {
                autoRefresh: {
                    value: 0
                }
            };

            const autoRefresh = shallow(<AutoRefresh {...props} />);
            autoRefresh.instance().handleRefresh = jest.fn();
            
            // Action
            autoRefresh.find('span').at(1).simulate('click')
            // Assert
            expect(autoRefresh.instance().handleRefresh).toHaveBeenCalled();
        });
    });
});