import { combineReducers } from 'redux';
import dataHandlerReducer from '../dataHandlerReducer';
import reducer from '../index';

describe('index', () => {
    it('should export expected reducer function ', () => {
        const expected = combineReducers({ ...dataHandlerReducer });
        expect(typeof expected).toEqual('function');
        expect(typeof reducer).toEqual('function');
        expect(expected.toString()).toEqual(reducer.toString());
    });
});