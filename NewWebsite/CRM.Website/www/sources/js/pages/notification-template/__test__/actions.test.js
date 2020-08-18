import axios from 'axios';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import { ActionCreators } from '../actions';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

jest.mock('axios');

describe('actionCreators', () => {
    describe('getNotificationTypes', () => {
        it('should handle setNotificationTypes when fetching notification types has been done', async () => {
            const data = [{ id: 1, name: 'void abnormal bet' }];
            Site.resolveClientUrl = jest.fn((url) => url);
            axios.get.mockImplementationOnce(() => Promise.resolve(data));
            const expectedActions = [
                { type: ActionCreators.setNotificationTypes.type }
            ];
            const store = mockStore({ notificationTypes: [] });

            await store.dispatch(ActionCreators.getNotificationTypes()).then(() => {
                expect(axios.get).toHaveBeenCalledWith('Common/GetNotificationTypes');
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });

    describe('getLanguages', () => {
        it('should handle setLanguages when fetching languages has been done', async () => {
            const data = [{ id: 1, name: 'en' }];
            Site.resolveClientUrl = jest.fn((url) => url);
            axios.get.mockImplementationOnce(() => Promise.resolve(data));
            const expectedActions = [
                { type: ActionCreators.setLanguages.type }
            ];
            const store = mockStore({ languages: [] });

            await store.dispatch(ActionCreators.getLanguages()).then(() => {
                expect(axios.get).toHaveBeenCalledWith('NotificationTemplate/GetLanguages');
                expect(store.getActions()).toEqual(expectedActions);
            });
        });
    });
});