/* exported remarkApplication */
/* global sharedScriptManager */
var remarkApplication = angular.module('remark-application', []);

/*Only for IE: must set no caching for GET request*/
sharedScriptManager.setNoCachingHttpRequest(remarkApplication);