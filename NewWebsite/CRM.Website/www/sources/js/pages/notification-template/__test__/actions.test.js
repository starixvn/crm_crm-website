import axios from 'axios';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import Site from '@jsroot/common/site';
import { ActionCreators } from '../actions';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

jest.mock('axios');

describe('actionCreators', () => {
    beforeAll(() => {
        Site.resolveClientUrl = jest.fn((url) => url);
        Site.handleAxiosError = jest.fn();
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('getNotificationTypes', () => {
        it('should handle setNotificationTypes when fetching notification types has been done', async () => {
            const data = [{ id: 1, name: 'void abnormal bet' }];
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

        it('should handle error on failure', async () => {
            const error = { error: 'some error' };
            axios.get.mockRejectedValueOnce(error);
            const store = mockStore({ notificationTypes: [] });

            await store.dispatch(ActionCreators.getNotificationTypes()).catch(() => {
                expect(Site.handleAxiosError).toHaveBeenCalledWith(error);
            });
        });
    });

    describe('getLanguages', () => {
        it('should handle setLanguages when fetching languages has been done', async () => {
            const data = [{ id: 1, name: 'en' }];
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

        it('should handle error on failure', async () => {
            const error = { error: 'some error' };
            axios.get.mockRejectedValueOnce(error);
            const store = mockStore({ notificationTypes: [] });

            await store.dispatch(ActionCreators.getNotificationTypes()).catch(() => {
                expect(Site.handleAxiosError).toHaveBeenCalledWith(error);
            });
        });
    });

    describe('getGridData', () => {
        it('should handle setGridData when fetching notification template has been done', async () => {
            const expectedParams = { notificationTypeId: 1 };
            const expectedData = {
                totalItems: 1,
                data: [{ id: 1, name: 'abnormal bet' }]
            };
            const expectedActions = [
                { type: ActionCreators.setGridData.type, payload: expectedData.data }
            ];
            const store = mockStore({ gridData: null });
            jest.mock('axios', () => jest.fn());
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            let getXsrfToken = jest.fn(() => { return 'token'; });
            Site.getXsrfToken = getXsrfToken.bind(Site);
            axios.mockResolvedValueOnce(expectedData);

            await store.dispatch(ActionCreators.getGridData(expectedParams)).then(() => {
                expect(axios).toHaveBeenCalledWith(
                    {
                        url: 'NotificationTemplate/GetGridData',
                        method: 'post',
                        data: expectedParams,
                        headers: { 'RequestVerificationToken': 'token', 'X-Requested-With': 'XMLHttpRequest' }
                    });
                expect(store.getActions()).toEqual(expectedActions);
            });
        });

        it('should handle error on failure', async () => {
            const error = { error: 'some error' };
            Site.blockUI = jest.fn();
            Site.unblockUI = jest.fn();
            let getXsrfToken = jest.fn(() => { return 'token'; });
            Site.getXsrfToken = getXsrfToken.bind(Site);
            jest.mock('axios', () => jest.fn());
            axios.mockRejectedValueOnce(error);
            const store = mockStore({ gridData: null });

            await store.dispatch(ActionCreators.getGridData({})).catch(() => {
                expect(Site.handleAxiosError).toHaveBeenCalledWith(error);
            });
        });
    });
});