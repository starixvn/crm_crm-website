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

        Title: {
            NotApply: 0
        },
        CommunicationTool: {
            NotApply: 1005
        },
        MainCategory: {
            NotApply: 0
        },
        SubCategory: {
            NotApply: 0
        },

        Country: {
            NotApply: 0
        },

        Language: {
            NotApply: 0,
            English: 1200,
            Mandarin: 1201,
            Cantonese: 1202
        },

        Product: {
            NotApply: 0
        },

        BetType: {
            NotApply: 0
        },

        Status: {
            All: 0
        },

        Extension: {
            All: 0
        },

        Action: {
            All: 0
        },

        Error: {
            None: 0,
            Forbidden: 403
        },
        /* jshint camelcase: false */
        FormatConstant: {
            ShortDate_String_Moment: 'MMM DD, YYYY',
            M_DD_YY_STRING: 'M dd, yy',
            MM_DD_YYYY_HH_MM_TT_String_Moment: 'MMM DD, YYYY hh:mm A',
            MM_DD_YYYY_HH_MM_TT_2Line_String_Moment: 'MMM DD, YYYY<br/>hh:mm A',
            MM_DD_YYYY_HH_MM_SS_TT_2Line_String_Moment: 'MMM DD, YYYY<br/>hh:mm:ss A',
            HH_MM_TT_String_Moment: 'hh:mm A',
            HH_MM_TT_TimePicker: 'hh:mm TT',
            MM_DD_YY_TimePicker: 'M dd, yy',
            MM_DD_YYYY_HH_MM_TT_StringFormat: "{0}/{1}/{2} {3}:{4} {5}",
            StartTime_RemarkPrefix: 'YYYYMMDD/hh:mmA',
            MMM_DD_YYYY_WeekPicker: 'MMM DD, YYYY',
            HH_MM_SS_MMM_DD_YYYY_String_Moment: 'hh:mm:ss A MMM DD, YYYY',

            IntNumber: '0,0',
            TwoDecimalNumber: '0,0.00'
        },

        DateTime: {
            AM: 'AM',
            PM: 'PM'
        },

        SortDirection: {
            Asc: "ASC",
            Desc: "DESC"
        },
        SortDataType: {
            Number: "number",
            String: "string",
            Date: "Date"
        },

        RoleId: {
            Member: 1,
            CRMMember: 7000
        },

        CalendarPickerType: {
            FromDate: "FromDate",
            ToDate: "ToDate",
            SinglePicker: "SinglePicker"
        },

        PeriodType: {
            NotApply: 0,
            Daily: 1,
            Weekly: 2,
            Monthly: 3,
            Yearly: 4
        },

        MultiSelectType: {
            GetIdList: 0,
            GetFullInfoList: 1
        },

        MessageType: {
            Success: 1,
            Warning: 2,
            Error: 3
        },
        UpdateReviewedMode: {
            NotChange: 0,
            Change: 1,
            Reset: 2
        },
        RelatedCustomerSetting: {
            MaxItems: 500
        }
    };

    self.MonthNames = ['January', 'February', 'March', 'April',
                       'May', 'June', 'July', 'August',
                       'September', 'October', 'November', 'December'];
    self.MarginBottomPage = 15;
};

var crmConstants = new CRMConstants();