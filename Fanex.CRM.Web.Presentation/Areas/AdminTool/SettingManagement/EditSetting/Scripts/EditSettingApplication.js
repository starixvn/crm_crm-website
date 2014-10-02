/* global sharedScriptManager */
/* exported editSettingApplication */
var editSettingApplication = angular.module('addmintool-settingmanagement-editsetting-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(editSettingApplication);