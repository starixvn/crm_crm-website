/* global sharedScriptManager */
/* exported addRelationApplication */
var addRelationApplication = angular.module('addmintool-settingrelation-addrelation-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(addRelationApplication);