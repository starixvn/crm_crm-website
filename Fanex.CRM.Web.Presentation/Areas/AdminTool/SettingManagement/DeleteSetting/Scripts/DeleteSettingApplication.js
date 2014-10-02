/* global sharedScriptManager */
/* exported deleteSettingApplication */
var deleteSettingApplication = angular.module('addmintool-settingmanagement-deletesetting',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(deleteSettingApplication);