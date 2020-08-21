import React from 'react';
import { Button } from 'react-bootstrap';
import { Filter } from '../Filter';
import { shallow } from 'enzyme';

describe('Filter', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('render filter component', () => {
        // Act
        const container = shallow(<Filter />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('simulates submit button', () => {
        // Arrange
        const param = {
            notificationTypeId: NaN,
            notificationTemplateName: '',
            pageIndex: 1,
            pageSize: 100
        };
        const props = {
            setGridParams: jest.fn(),
            getGridData: jest.fn()
        };
        const container = shallow(<Filter {...props} />);

        // Act
        container.find(Button).first().props().onClick({ preventDefault: jest.fn() });

        // Assert
        expect(props.setGridParams).toHaveBeenCalledWith(param);
        expect(props.getGridData).toHaveBeenCalledWith(param);
    });

    it('simulates insert button', () => {
        // Arrange
        const props = {
            isHost: true,
            setCreateTemplatePopup: jest.fn()
        };
        const container = shallow(<Filter {...props} />);

        // Act
        container.find(Button).at(1).props().onClick();

        // Assert
        expect(props.setCreateTemplatePopup).toHaveBeenCalledWith({ show: true });
    });
});