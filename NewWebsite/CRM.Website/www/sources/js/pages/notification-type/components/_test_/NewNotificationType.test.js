import React from 'react';
import { Button } from 'react-bootstrap';
import { NewNotificationType } from '../NewNotificationType';
import { shallow, mount } from 'enzyme';
import MultipleSelect from '../../../../components/multiple-select/index';
import configureMockStore from 'redux-mock-store';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';

const mockStore = configureMockStore();
const store = mockStore({});

describe('NewNotificationType', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<NewNotificationType />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('render component correctly', () => {
        const props = {
            createNotificationTypeResult: {},
            settings: {},
            getSettings: {}
        };

        const container = shallow(<NewNotificationType {...props} />);

        expect(container).toMatchSnapshot();
    });
});