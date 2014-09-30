/* global sharedScriptManager */
/* exported viewListApplication */
var viewListApplication = angular.module('addmintool-settingrelation-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(viewListApplication);