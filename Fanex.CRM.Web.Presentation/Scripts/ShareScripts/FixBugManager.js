"use strict";
/* exported fixBugManager */
var FixBugManager = function () {
    var self = this;
    self.fixJqueryParamMethod = function () {
        (function ($, oldFunction) {
            $.param = function (a, traditional) {
                var s = oldFunction.apply(oldFunction, [a, traditional]);
                //Return the resulting serialization;
                return s.replace('+', '%20');
            };
        })(jQuery, jQuery.param);
    };
};

var fixBugManager = new FixBugManager();
