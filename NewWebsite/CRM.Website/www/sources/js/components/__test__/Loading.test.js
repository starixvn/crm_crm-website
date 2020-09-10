import React from 'react';
import Loading from '../Loading';
import { shallow } from 'enzyme';

describe('Loading', () => {
    it('has correct snapshots', () => {
        // Action
        const component = shallow(<Loading />);

        // Assert
        expect(component).toMatchSnapshot();
    });

    it('renderChildCircle runs correctly', () => {
        // Arrange
        const component = shallow(<Loading />);
        const expected = [];
        for (let i = 1; i <= 12; i++) {
            expected.push(<div key={`loading-${i}`} className={`sk-circle${i} sk-circle`} />);
        }

        // Action
        const result = component.instance().renderChildCircle();

        // Assert
        expect(result).toEqual(expected);
    });

    it('render returns correct component', () => {
        // Arrange
        const props = {
            width: 1,
            height: 2
        };

        // Action
        const component = shallow(<Loading {...props} />);
        component.instance().renderChildCircle = jest.fn();

        // Assert
        expect(component.find('div').at(0).props().className).toBe('sk-fading-circle');
        expect(component.find('div').at(0).props().style).toEqual({
            width: props.width,
            height: props.height
        });
        expect(component.find('div')).toHaveLength(13);
    });
});