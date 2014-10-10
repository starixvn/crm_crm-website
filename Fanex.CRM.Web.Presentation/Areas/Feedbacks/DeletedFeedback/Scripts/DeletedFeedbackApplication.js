/* exported deletedFBApplication */
/* global RegisterTemplate */
var deletedFBApplication = angular.module('deleted-feedback-application', ['$uiBlockerApplication']);

var deletedFBRegister = new RegisterTemplate(deletedFBApplication);
deletedFBRegister.registerDatePicker();
deletedFBRegister.registerMainCategoryPicker();
deletedFBRegister.registerSubCategoryPicker();