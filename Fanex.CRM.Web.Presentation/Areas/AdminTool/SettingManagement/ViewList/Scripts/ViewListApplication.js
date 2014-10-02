/* global sharedScriptManager */
/* exported viewListApplication */
var viewListApplication = angular.module('addmintool-settingmanagement-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(viewListApplication);