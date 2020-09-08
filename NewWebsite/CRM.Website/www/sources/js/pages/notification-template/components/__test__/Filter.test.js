import React from 'react';
import { Button, Form } from 'react-bootstrap';
import { Filter } from '../Filter';
import { shallow } from 'enzyme';
import LangHelper from '@jsroot/common/langHelper';

describe('Filter', () => {
    beforeAll(() => {
        LangHelper.getResource = jest.fn((key) => key);
    });
    afterAll(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<Filter />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    describe('methods', () => {
        describe('handleNotificationTypeChange', () => {
            it('should calls setState when change notification type', () => {
                // Arrange
                const expected = {
                    notificationTypeId: '1',
                    notificationTemplateName: ''
                };
                const container = shallow(<Filter />);

                // Act
                container.instance().handleNotificationTypeChange({ target: { value: '1' } });

                // Assert
                expect(container.state()).toEqual(expected);
            });
        });

        describe('handleTemplateNameChange', () => {
            it('should calls setState when change template name', () => {
                // Arrange
                const expected = {
                    notificationTypeId: '',
                    notificationTemplateName: 'void abnormal bet'
                };
                const container = shallow(<Filter />);

                // Act
                container.instance().handleTemplateNameChange({ target: { value: 'void abnormal bet' } });

                // Assert
                expect(container.state()).toEqual(expected);
            });
        });

        describe('getParams', () => {
            it('return expected params', () => {
                // Arrange
                const expected = {
                    notificationTypeId: NaN,
                    notificationTemplateName: '',
                    pageIndex: 1,
                    pageSize: 100
                };
                const container = shallow(<Filter />).instance();

                // Act
                const actual = container.getParams();

                // Assert
                expect(actual).toEqual(expected);
            });
        });

        describe('handleSubmit', () => {
            it('should calls exactly functions', () => {
                // Arrange
                const expectedParams = {
                    notificationTypeId: NaN,
                    notificationTemplateName: '',
                    pageIndex: 1,
                    pageSize: 100
                };
                const props = {
                    setGridParams: jest.fn(),
                    getGridData: jest.fn()
                };
                const container = shallow(<Filter {...props} />).instance();
                container.getParams = jest.fn(() => expectedParams);

                // Act
                container.handleSubmit({ preventDefault: jest.fn() });

                // Assert
                expect(props.setGridParams).toHaveBeenCalledWith(expectedParams);
                expect(props.getGridData).toHaveBeenCalledWith(expectedParams);
            });
        });

        describe('handleOpenCreatePopup', () => {
            it('should calls exactly functions', () => {
                // Arrange
                const props = {
                    isHost: true,
                    setCreateTemplatePopup: jest.fn()
                };
                const container = shallow(<Filter {...props} />).instance();

                // Act
                container.handleOpenCreatePopup();

                // Assert
                expect(props.setCreateTemplatePopup).toHaveBeenCalledWith({ show: true });
            });
        });
    });
});