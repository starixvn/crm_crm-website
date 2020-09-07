import React from 'react';
import { ContentDetailModalDialog } from '../ContentDetailModalDialog';
import SweetModalDialog from '@jsroot/components/SweetModalDialog';
import { Button } from 'react-bootstrap';
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

    it('props should be mapped correctly', () => {
        // Arrange
        const props = {
            template: {
                id: 1,
                name: 'void abnormal bet',
                title: 'void abnormal bet',
                contents: [
                    { languageId: 1, language: 'EN', content: 'content' }
                ]
            },
            onClose: jest.fn()
        };
        const container = shallow(<ContentDetailModalDialog {...props} />);

        // Act
        const dialog = container.find(SweetModalDialog);

        // Assert
        expect(dialog.length).toBe(1);
        expect(dialog.props().title).toBe('void abnormal bet');
        expect(dialog.props().buttons)
            .toEqual([<Button variant="primary" size="sm" onClick={props.onClose}>Close</Button>]);
        //expect(dialog.props().contents)
        //    .toEqual(<Fragment>
        //        <Fragment key={1}>
        //            <div className="font-weight-bold">Content (EN)</div>
        //            <div className="border p-2 mt-2 mb-3 content">content</div>
        //        </Fragment>
        //    </Fragment>);
    });
});