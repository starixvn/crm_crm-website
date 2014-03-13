"use strict";
/* global bindingSetting,
bindingCountry,
bindingChildByParentId,
bindingSite,
allowDeleteFeedback,
feedbackObjectValidate,
showAlertMessage,
successMsg,
accId,
getRelatedCustomer,
dropdownMultiSelect,
head,
clientTime,
initHTMLArea,
errMsgFeedbackDateGreatter,
errMsgResolutionDateGreatter,
SetupCalendar,
inputMultipleRelatedNote,
jlinq,
errMsgNotYetVerifyFeedbackRelatedCustomer
*/
/* exported settings,
ddls,
numberOfFinish,
orgLengt,
AddGuestFeedback,
bindingEvent,
isShowCustinfoBox,
isShowDescriptionBox,
isShowFeedbackDetailBox,
isShowReviewBox,
isShowSolutionBox */
var DoAdd,
    getFeedbacktData,
    getNumOfFeedbackItems,
    appendListFeedbacksLasted,
    bindingEvent;

var settings = ["Title", "Status", "Station", "Response", "Communication"];
var ddls = ["ddltitle", "ddlstatus", "ddlcsextension", "ddlresponse", "ddlcommunication"];
var listObjToValidate = ["txtsubject", "txtidentity"];
var listEditorToValidate = ["desediter"];
var listComboboxesToValidate = ["desediter"];

var numberOfFinish = 0;
var AllSetting;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var orgLengt = 0;
var isShowCustinfoBox = false;
var isShowFeedbackDetailBox = false;
var isShowDescriptionBox = false;
var isShowSolutionBox = false;
var isShowReviewBox = false;
var feedbackData = undefined;

function bindingEvent() {
    $('#ddlcategory').change(function () {
        bindingChildByParentId('ddlsubcategory', AllSetting.Category, parseInt($('#ddlcategory').val()));
    });

    $('#ddlproduct').change(function () {
        bindingChildByParentId('ddlbettype', AllSetting.Product, parseInt($('#ddlproduct').val()));
    });
}

function loadFeedbackSetting() {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/SelAllSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Id":' + 0 + '}',
        success: function (setting) {
            AllSetting = setting;
            // Identity Information
            bindingSetting("ddltitle", setting.Title, 13000);
            bindingSite("ddlsite", setting.Site, 0);
            dropdownMultiSelect("ddllanguage", "ddllanguage", setting.Language);

            // Details
            bindingSetting("ddlcategory", setting.Category, 0);
            bindingSetting("ddlproduct", setting.Product, 0);
            bindingSetting("ddlstatus", setting.Status, 0);
            bindingSetting("ddlresponse", setting.Response, 0);
            bindingSetting("ddlcommunication", setting.Communication, 0);
            bindingSetting("ddlcsextension", setting.Extension, 0);
            bindingCountry("ddlcountry", setting.Country, 0);

            bindingChildByParentId('ddlsubcategory', setting.Category, (setting.Category ? setting.Category[0].Id : 3000), 0);
            bindingChildByParentId('ddlbettype', setting.Product, 0, 0);

            if (!allowDeleteFeedback) {
                $("select#ddlstatus option[value='11004']").remove();
            }

            bindingEvent();
        },
        error: function () { },
        complete: function () {
            try {
                parent.UnBlockUI();
            } catch (e) { }
            
            //get lasted feedbacks data
            getNumOfFeedbackItems();
            setInterval(function () {
                getNumOfFeedbackItems();
            }, 100000);
        }
    });
}

function AddGuestFeedback() {
    var successCallback = function () {
        parent.$('#isGuest').attr('checked', true);

        var identity = feedbackData.getFeedback().identity.val().trim();
        parent.leftPanelManager.loadLeftPanel(identity, true);
    };
  
    feedbackData.addGuestFeedback(successCallback);
}

//mark because it may conflict with other function 
getNumOfFeedbackItems = function () {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetNumOfFeedbackSubjectTemplateItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            numberOfFeedbackItem = rs;
            listFeedbacksLasted = [];
            var numOfRange = Math.round((numberOfFeedbackItem / 100000) + 0.5);
            for (var i = 1; i <= numOfRange; i++) {
                appendListFeedbacksLasted(i);
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });
};

appendListFeedbacksLasted = function (range) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetFeedbackSubjectTemplateItemsRange",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{ "range": "' + range.toString() + '" }',
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                listFeedbacksLasted = listFeedbacksLasted.concat(rs);
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });
};

/* jshint camelcase: false */
var invokeHotKey_CtrlS = function () {
    AddGuestFeedback();
};

head.ready(function () {
    try { parent.BlockUI(); } catch (e) { }

    loadFeedbackSetting();
    feedbackData = new FeedbackData(accId, 0, clientTime);
    feedbackData.initialize();

    /* register hot key */
    hotKeyManager.CachedData.ActiveDocumentManager.push({
        Value: window,
        Id: 'AddGuestFeedback'
    });

    $("#txtidentity").focus();
});