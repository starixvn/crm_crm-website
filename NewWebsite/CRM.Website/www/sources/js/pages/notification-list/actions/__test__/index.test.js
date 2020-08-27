import dataHandlerActions from '../dataHandlerActions';
import ActionCreators from '../index';

describe('index', () => {
    it('should be imported action from data handler actions', () => {
        const expected = {
            ...dataHandlerActions
        };

        expect(ActionCreators).toEqual(ActionCreators);
    })
});