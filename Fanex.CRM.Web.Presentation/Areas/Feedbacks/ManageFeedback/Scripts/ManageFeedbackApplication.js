/* exported manageFBApplication */
/* global RegisterTemplate */
var manageFBApplication = angular.module('feedback-manage-application', ['$uiBlockerApplication']);

var manageFBRegister = new RegisterTemplate(manageFBApplication);
manageFBRegister.registerDatePicker();
manageFBRegister.registerMainCategoryPicker();
manageFBRegister.registerSubCategoryPicker();