/* global sharedScriptManager */
/* exported insertSettingApplication */
var insertSettingApplication = angular.module('addmintool-settingmanagement-insertnewsetting-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(insertSettingApplication);