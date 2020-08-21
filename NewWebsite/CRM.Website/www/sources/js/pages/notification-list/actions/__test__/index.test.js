import dataHandlerActions from '../dataHandlerActions';
import ActionCreators from '../index';

describe('notificationListActionsIndex', () => {
    it('should be imported action from data handler actions', () => {
        const expected = {
            ...dataHandlerActions
        };

        expect(ActionCreators).toEqual(ActionCreators);
    })
})