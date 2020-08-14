import { combineReducers } from 'redux';
import dataHandlerReducer from './dataHandlerReducer';
import popupHandlerReducers from './popupHandlerReducers';

export default combineReducers({
	...dataHandlerReducer,
	...popupHandlerReducers
});