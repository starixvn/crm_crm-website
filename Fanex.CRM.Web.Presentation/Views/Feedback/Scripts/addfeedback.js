"use strict";
/* global jlinq, 
showAlertMessage,
errMsgFeedbackDateGreatter, 
errMsgResolutionDateGreatter, 
successMsg, 
bindingSetting, 
initHTMLArea, 
bindingSetting, 
bindingChildByParentId, 
getRelatedCustomer,
feedbackObjectValidate, 
bindingCountry, 
bindingSite, 
allowDeleteFeedback, 
clientTime, 
SetupCalendar,
inputMultipleRelatedNote, 
head, 
gup,
accId,
dropdownMultiSelect,
errMsgNotYetVerifyFeedbackRelatedCustomer
*/
/* exported settings, 
ddls,
roleName,
role,
numberOfFinish,
isonauto,
orgLengt,
AddFeedback,
trim,
bindingEvent,
isShowCustinfoBox,
isShowDescriptionBox,
isShowFeedbackDetailBox,
isShowReviewBox,
isShowSolutionBox
*/
//var settings = ["Title", "Status", "Station", "Response", "Communication"];
//var ddls = ["ddltitle", "ddlstatus", "ddlcsextension", "ddlresponse", "ddlcommunication"];
var listObjToValidate = ["txtsubject"];
var listEditorToValidate = ["desediter"];
var custName = "";
//var roleName = "";
var custId = 0;
//var role = 0;
//var numberOfFinish = 0;
var AllSetting;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var isonauto = false;
var orgLengt = 0;
var isShowCustinfoBox = false;
var isShowFeedbackDetailBox = false;
var isShowDescriptionBox = false;
var isShowSolutionBox = false;
var isShowReviewBox = false;

function bindingEvent() {
    $('#ddlcategory').change(function () {
        bindingChildByParentId('ddlsubcategory', AllSetting.Category, parseInt($('#ddlcategory').val()));
    });

    $('#ddlproduct').change(function () {
        bindingChildByParentId('ddlbettype', AllSetting.Product, parseInt($('#ddlproduct').val()));
    });
}

function addFeedbackInfo(newFeedback) {
    //-------end----------------------------------
    try { parent.BlockUI(); } catch (e) { }
    var feedback = { feedback: newFeedback };
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Feedback/AddFeedback",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(feedback),
        success: function (rs) {
            if (rs.ErrCode === 0) {
                showAlertMessage("errmsg", successMsg, 0);
                parent.leftPanelManager.loadLeftPanel(null, true);
            }
            else {
                showAlertMessage("errmsg", rs.ErrMsg, 1);
            }
        },
        error: function () {
            showAlertMessage("errmsg", "Insert failed", 1);
        },
        complete: function () { try { parent.UnBlockUI(); } catch (e) { } }
    });
}

function getFeedbackData() {
    var newFeedback = {
        Customer: {
            Id: custId,
            UserName: custName
        },
        CustomerInfo: {
            Title: {
                Id: $("#ddltitle").val()
            },
            FirstName: $("#firstNameInfo").val().trim(),
            LastName: $("#lastNameInfo").val().trim(),
            ContactNumber1: {
                CountryCode: $("#contact1InfoCountry").val().trim(),
                AreaCode: $("#contact1InfoArea").val().trim(),
                PhoneNumber: $("#contact1InfoPhone").val().trim()
            },
            ContactNumber2: {
                CountryCode: $("#contact2InfoCountry").val().trim(),
                AreaCode: $("#contact2InfoArea").val().trim(),
                PhoneNumber: $("#contact2InfoPhone").val().trim()
            },
            ContactNumber3: {
                CountryCode: $("#contact3InfoCountry").val().trim(),
                AreaCode: $("#contact3InfoArea").val().trim(),
                PhoneNumber: $("#contact3InfoPhone").val().trim()
            },
            Fax: {
                CountryCode: $("#faxInfoCountry").val().trim(),
                AreaCode: $("#faxInfoArea").val().trim(),
                FaxNumber: $("#faxInfoFax").val().trim()
            },
            Mobile: {
                CountryCode: $("#mobileInfoCountry").val().trim(),
                AreaCode: $("#mobileInfoArea").val().trim(),
                PhoneNumber: $("#mobileInfoPhone").val().trim()
            },
            Skype: $('#skypeInfo').val().trim(),
            Email: $("#emailInfo").val().trim(),
            MSN: $('#msnInfo').val().trim(),
            PassportNumber: $('#passportInfo').val().trim(),
            Birthday: $('#birthdayInfo').datepicker("getDate"),
            BirthdayString: $('#birthdayInfo').val(),
            Site: $('#ddlsite').val(),
            BettingCode: $("#bettingCodeInfo").val().trim(),
            RefCodes2: $("#refCodes2Info").val().trim(),
            IsVIP: $("#isVIP").prop("checked"),
            PIC1: $("#pic1Info").val().trim(),
            PIC2: $("#pic2Info").val().trim(),
            PIC3: $("#pic3Info").val().trim(),
            PIC4: $("#pic4Info").val().trim()
        },
        FeedBackDate: $("#txtdate").val(),
        ResolutionTime: $('#txtResolutionTime').val(),
        CsExtension: {
            Id: $("#ddlcsextension").val()
        },
        Communication: {
            Id: $("#ddlcommunication").val()
        },
        Category: {
            Id: $("#ddlcategory").val()
        },
        SubCategory: {
            Id: $("#ddlsubcategory").val()
        },
        Product: {
            Id: $("#ddlproduct").val()
        },
        Bettype: {
            Id: $("#ddlbettype").val()
        },
        Country: {
            Id: $("#ddlcountry").val()
        },
        Language: {
            Id: $("#ddllanguage").val()
        },
        Status: {
            Id: $("#ddlstatus").val()
        },
        Role: {
            Id: $("#ddlrole").val()
        },
        Response: {
            Id: $("#ddlresponse").val()
        },
        Subject: $("#txtsubject").val().trim(),
        Description: $("#desediter").htmlarea("toHtmlString"),
        Solution: $("#solediter").htmlarea("toHtmlString"),
        IsReviewed: $("#chkisreview").prop("checked")
    };

    if ($("#chkisrelated").prop("checked")) {
        newFeedback.RelatedCustomer = {
            Id: 0,
            UserName: getRelatedCustomer()
        };
    }

    if (newFeedback.IsReviewed) {
        newFeedback.Reviewer = {
            Id: accId
        };
        newFeedback.Review = $("#revediter").htmlarea("toHtmlString");
    }

    return newFeedback;
}

function DoAdd() {
    var newFeedback = getFeedbackData();
    //-------check override crm info--------------
    if ($('#chkIsOverrideCrmInfo').prop('checked')) {
        var divPopup = parent.$("#div-resultpopup");
        divPopup.html('Current CRM Info will be overrided, please double check before Submit!');
        divPopup.removeClass("display-none");
        divPopup.dialog({ width: 400, height: 150, zIndex: 500, resizable: false, modal: true,
            close: function (event, ui) {
                /* jshint unused: vars */
            },
            buttons: [{
                text: "Ok",
                click: function () {
                    newFeedback.IsOverrideCrmInfo = true;
                    divPopup.dialog("close");
                    addFeedbackInfo(newFeedback);
                }
            },
            {
                text: 'Cancel',
                click: function () {
                    newFeedback.IsOverrideCrmInfo = false;
                    divPopup.dialog("close");
                }
            }]
        });
    }
    else {
        addFeedbackInfo(newFeedback);
    }
}

function AddFeedback() {
    /* jshint maxstatements: 81 */
    $("#errmsg").empty();
    $("#lblverify").html('');
    // check input parameters
    if (feedbackObjectValidate(listEditorToValidate, listObjToValidate)) {
        var birthdayText = $("#birthdayInfo").val().trim();
        if (birthdayText.length !== 0) {
            var birthday = new Date(birthdayText);
            if (isNaN(birthday)) {
                showAlertMessage("errmsg", "Birthday is invalid!", 1);
                return false;
            }
        }

        //does not submit when feedback date > current date
        var currentDate = new Date();
        var serverTime = new Date(clientTime);
        serverTime.setMinutes(currentDate.getMinutes());
        serverTime.setSeconds(currentDate.getSeconds());

        var feedbackDate = Date.parse($("#txtdate").val());
        if (feedbackDate > serverTime) {
            showAlertMessage("errmsg", errMsgFeedbackDateGreatter, 1);
            return false;
        }
        if (feedbackDate >= Date.parse($("#txtResolutionTime").val())) {
            showAlertMessage("errmsg", errMsgResolutionDateGreatter, 1);
            return false;
        }
        if ($('#ddlcommunication option:selected').text().toLowerCase() === 'n/a') {
            showAlertMessage("errmsg", "Please choose communication!", 1);
            return false;
        }
        if (parseInt($('#ddlproduct option:selected').val()) === 0) {
            showAlertMessage("errmsg", "Please choose product!", 1);
            return false;
        }
        if (parseInt($('#ddlbettype option:selected').val()) === 0) {
            showAlertMessage("errmsg", "Please choose bet type!", 1);
            return false;
        }
        if ($("#ddllanguage").val() === '0') {
            showAlertMessage("errmsg", "Please choose language!", 1);
            return false;
        }
        if ($('#hIsNotChild').val() === '1') {
            showAlertMessage("errmsg",
            "There is no Subcategory for Category [" + $("#ddlcategory option:selected").text() + "]. Please add a Subcategory or choose another Category.", 1);
            return false;
        }
        //----------check length of data
        if ($("#desediter").htmlarea("toHtmlString").length > 8000) {
            $('#desediter').focus();
            showAlertMessage("errmsg", 'Max length of Description is 8000!', 1);
            return false;
        }
        if ($("#solediter").htmlarea("toHtmlString").length > 8000) {
            $('#solediter').focus();
            showAlertMessage("errmsg", 'Max length of Solution is 8000!', 1);
            return false;
        }
        if ($("#revediter").htmlarea("toHtmlString").length > 8000) {
            $('#revediter').focus();
            showAlertMessage("errmsg", 'Max length of Review is 8000!', 1);
            return false;
        }
        //----------check related customer
        if ($("#chkisrelated").prop("checked")) {
            var customerIds = getRelatedCustomer();
            if (customerIds === "_") {
                showAlertMessage("errmsg", errMsgNotYetVerifyFeedbackRelatedCustomer, 1);
                return false;
            }
        }
        //------------------------
        /* jshint newcap: false */
        DoAdd();
    }
}

// insert by David.Tran 07-03-2012
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

function appendListFeedbacksLasted(range) {
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
}

//mark because it may conflict with other function 
function getNumOfFeedbackItems() {
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
}

function getCRMInfoById(custId) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Customer/GetCustomerInfoById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"custId":' + custId + '}',
        success: function (rs) {
            var customerInfo = rs.CustomerInfo;
            $("#firstNameInfo").val(customerInfo.FirstName);
            $("#lastNameInfo").val(customerInfo.LastName);
            $("#contact1InfoCountry").val(customerInfo.ContactNumber1.CountryCode);
            $("#contact1InfoArea").val(customerInfo.ContactNumber1.AreaCode);
            $("#contact1InfoPhone").val(customerInfo.ContactNumber1.PhoneNumber);
            $("#contact2InfoCountry").val(customerInfo.ContactNumber2.CountryCode);
            $("#contact2InfoArea").val(customerInfo.ContactNumber2.AreaCode);
            $("#contact2InfoPhone").val(customerInfo.ContactNumber2.PhoneNumber);
            $("#contact3InfoCountry").val(customerInfo.ContactNumber3.CountryCode);
            $("#contact3InfoArea").val(customerInfo.ContactNumber3.AreaCode);
            $("#contact3InfoPhone").val(customerInfo.ContactNumber3.PhoneNumber);
            $("#faxInfoCountry").val(customerInfo.Fax.CountryCode);
            $("#faxInfoArea").val(customerInfo.Fax.AreaCode);
            $("#faxInfoFax").val(customerInfo.Fax.FaxNumber);
            $("#mobileInfoCountry").val(customerInfo.Mobile.CountryCode);
            $("#mobileInfoArea").val(customerInfo.Mobile.AreaCode);
            $("#mobileInfoPhone").val(customerInfo.Mobile.FaxNumber);
            $("#skypeInfo").val(customerInfo.Skype);
            $("#emailInfo").val(customerInfo.Email);
            $("#msnInfo").val(customerInfo.MSN);
            $("#passportInfo").val(customerInfo.PassportNumber);
            if (customerInfo.BirthDayString !== '') {
                $("#birthdayInfo").datepicker("setDate", new Date(customerInfo.BirthDayString));
            }
            $("#bettingCodeInfo").val(customerInfo.BettingCode);
            $("#refCodes2Info").val(customerInfo.RefCodes2);
            $("#isVIP").prop("checked", customerInfo.IsVIP);
            $("#pic1Info").val(customerInfo.PIC1);
            $("#pic2Info").val(customerInfo.PIC2);
            $("#pic3Info").val(customerInfo.PIC3);
            $("#pic4Info").val(customerInfo.PIC4);

            $.ajax({
                type: "POST",
                url: window.rootUrl + "AdminTool/SelAllSetting",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"Id":' + 0 + '}',
                success: function (setting) {
                    AllSetting = setting;
                    bindingSetting("ddltitle", setting.Title, customerInfo.Title.Id);
                    bindingSite('ddlsite', setting.Site, customerInfo.Site);

                    bindingSetting("ddlcsextension", setting.Extension, 0);
                    bindingSetting("ddlcategory", setting.Category, 0);
                    bindingSetting("ddlproduct", setting.Product, 0);
                    bindingSetting("ddlrole", setting.Role, rs.RoleId);
                    bindingCountry("ddlcountry", setting.Country, 0);
                    bindingLanguage("ddllanguage", setting.Language, 0);
                    bindingSetting("ddlstatus", setting.Status, 0);
                    bindingSetting("ddlresponse", setting.Response, 0);
                    bindingSetting("ddlcommunication", setting.Communication, 0);
                    // Select childs.
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
                    setInterval(function () { getNumOfFeedbackItems(); }, 100000);
                    //add input listen for auto complete Subject name input text box
                }
            });
            /*if (rs.RelatedCustomer != null && rs.RelatedCustomer.UserName !== "") {
            $("#chkisrelated").prop("checked", true);
            $("#btnverify").removeAttr("disabled");
            }*/
        }
    });
}

/* jshint camelcase: false */
var invokeHotKey_CtrlS = function () {
    AddFeedback();
};

head.ready(function () {
    initHTMLArea('editboxdes', 'desediter', '');
    initHTMLArea('editboxsol', 'solediter', '');
    initHTMLArea('editboxrev', 'revediter', '');

    //Bind data to dropdown list
    try { parent.BlockUI(); } catch (e) { }
    custId = gup("custid") === "" ? 0 : parseInt(gup("custid"));
    custName = $.URLDecode(gup("custName"));
    $("#lblUserName").attr("value", custId).html(custName);

    var feedbackdate = new Date(clientTime);
    var resolutiondate = new Date(clientTime);
    resolutiondate.setMinutes(feedbackdate.getMinutes() + 1);

    var arrDateValues = [feedbackdate, resolutiondate];
    var arrIds = ['txtdate', 'txtResolutionTime'];
    /* jshint newcap: false */
    SetupCalendar(arrDateValues, arrIds);
    $("#birthdayInfo").datepicker({
        dateFormat: "M dd, yy",
        maxDate: new Date(clientTime),
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        yearRange: '-100:+0'
    });

    $('#btnRelatedHelp').tipsy({ gravity: 'nw', title: function () { return inputMultipleRelatedNote; } });
    $("#txtrelatedcustomer").fcbkcomplete({
        /* jshint camelcase: false */
        json_url: window.rootUrl + 'Customer/RelatedCustomerAutoSuggestion',
        addontab: true,
        maxitems: 30,
        input_min_size: 1,
        height: 10,
        cache: false,
        newel: true
    });
    if ($("#chkisrelated").prop('checked')) {
        $("#typeCust").val("");
        $("#typeCust").removeAttr("disabled");
    }
    $("#btnsubmit").removeAttr("disabled");
    $("#btnsubmitbottom").removeAttr("disabled");

    //add input listen for auto complete Subject name input text box
    $("#txtsubject").keyup(function (e) {
        var keyCode = e.keyCode;
        if (keyCode === 37 || keyCode === 38 || keyCode === 39 || keyCode === 40) {
            return true;
        }
        else if (keyCode === 13) {
            return;
        }
        else {
            var rows = [];
            if ($(this).val() !== "") {
                rows = jlinq.from(listFeedbacksLasted).starts("Subject", $(this).val()).sort("Subject").select();
                rows = jlinq.from(rows).skip(0).take(15);
                if (rows.length > 0) {
                    $(this).autoComplete(rows, "Subject");
                }
                else {
                    $(this).removeAutoComplete();
                }
            }
            else {
                $(this).removeAutoComplete();
            }
        }
    });

    getCRMInfoById(custId);
    $("#ddltitle").focus();
    $("#typeCust").attr("tabindex", 48);

    /* register hot key */
    hotKeyManager.CachedData.ActiveDocumentManager.push({
        Value: window,
        Id: 'AddGuestFeedback'
    });
});
