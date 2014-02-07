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
        /* jshint camelcase: false */
        FormatConstant: {
            ShortDate_String_Moment: 'MMM D, YYYY',
            M_DD_YY_STRING: 'M dd, yy',
            MM_DD_YYYY_HH_MM_TT_String_Moment: 'MM/DD/YYYY hh:mm A',
            HH_MM_TT_TimePicker: 'hh:mm TT',
            MM_DD_YYYY_HH_MM_TT_StringFormat: "{0}/{1}/{2} {3}:{4} {5}",
            StartTime_RemarkPrefix: 'YYYYMMDD/hh:mmA'
        },

        DateTime: {
            AM: 'AM',
            PM: 'PM'
        }
    };
};

var crmConstants = new CRMConstants();