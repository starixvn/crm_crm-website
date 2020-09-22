import ReactDOM from 'react-dom';
import Store from '@jsroot/common/store';
import reducer from '../reducers/index';

describe('index', () => {
    beforeEach(() => {
        const appContainer = document.createElement('div');
        appContainer.setAttribute('id', 'app-container');
        document.body.appendChild(appContainer);
        ReactDOM.render = jest.fn();
    });

    afterEach(() => {
        jest.clearAllMocks();
        const appContainer = document.getElementById('app-container');
        if (appContainer) {
            document.body.removeChild(appContainer);
        }
    });

    it('renders dom correctly', () => {
        // Arrange
        Store.createStore(reducer, {
            gridData: { data: null, totalItems: 0 },
            updateActiveStatusResult: null,
            insertPersonalMappingData: { isShow: false },
            insertPersonalMappingResult: null,
            updatePersonalMappingData: { isShow: false, data: null },
            updatePersonalMappingResult: null,
            updateGroupMappingData: { isShow: false, data: null },
            updateGroupMappingResult: null,
            recipients: [],
            groupHeadline: [],
            letstalkVerification: []
        });

        // Action
        require('../index');

        // Assert
        expect(ReactDOM.render).toHaveBeenCalled();
    });
});