"use strict";
/* exported crmConstants */
var CRMConstants = function () {
    var self = this;
    self.Enums = {
        AppearanceStatus: {
            Collapse: "collapse",
            Expand: "expand"
        },

        CssClass: {
            DisplayNone: "display-none"
        },

        Site: {
            NotApply: 0
        },

        Country: {
            NotApply: 0
        },

        Language: {
            NotApply: 0
        },

        Error: {
            None: 0
        },
        FormatConstant: {
            ShortDate_String_Moment: 'MMM D, YYYY'
        }
    };
};

var crmConstants = new CRMConstants();