import { combineReducers } from 'redux';
import dataHandlerReducer from '../dataHandlerReducer';
import popupHandlerReducers from '../popupHandlerReducers';
import reducer from '../index';

describe('index', () => {
    it('should export expected reducer function ', () => {
        const expected = combineReducers({ ...dataHandlerReducer, ...popupHandlerReducers });
        expect(typeof expected).toEqual('function');
        expect(typeof reducer).toEqual('function');
        expect(expected.toString()).toEqual(reducer.toString());
    });
});