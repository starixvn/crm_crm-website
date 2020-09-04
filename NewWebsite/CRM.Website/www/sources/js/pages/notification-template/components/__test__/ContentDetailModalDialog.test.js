import React from 'react';
import { ContentDetailModalDialog } from '../ContentDetailModalDialog';
import { shallow } from 'enzyme';

describe('ContentDetailModalDialog', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<ContentDetailModalDialog />);

        // Assert
        expect(container).toMatchSnapshot();
    });
});