"use strict";
/* global jlinq, 
showAlertMessage,
errMsgFeedbackDateGreatter, 
accId, 
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
gup 
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
var bindingEvent;
var feedbackNew = {
    "Id": 0,
    "IsOverrideCrmInfo": false,
    "BetType": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Category": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Communication": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Country": { "Id": 0, "CountryCode": 0, "CountryName": null, "DisplayOrder": 0, "IsActive": false },
    "CreateDate": null,
    "Creator": { "Id": 0, "IsAllowUpdateReview": false, "Password": null, "RoleId": 0, "Username": "" },
    "CsExtension": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Customer": {
        "Id": 0,
        "Answer": null,
        "BankInfo": null,
        "BettingCode": null,
        "Country": 0,
        "CountryName": null,
        "CreatedDate": null,
        "Creator": null,
        "CustomerInfo": null,
        "IsVip": false,
        "Language": 0,
        "NickName": null,
        "Pic1": null,
        "Pic2": null,
        "Pic3": null,
        "Question": null,
        "RefCode2": null,
        "RelatedCustomer": null,
        "Remark": null,
        "RoleId": 0,
        "RoleName": "",
        "Site": null,
        "SpecialRemark": null,
        "UserName": ""
    },
    "CustomerInfo": {
        "BirthDay": null,
        "Email": " ",
        "Fax": " ",
        "FirstName": " ",
        "LastName": " ",
        "MSN": " ",
        "MobilePhone": " ",
        "Phone": " ",
        "Skype": " ",
        "Title": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }
    },
    "Description": "",
    "FeedbackDate": "/Date(1317709320000+0700)/",
    "IsReviewed": false,
    "Language": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Priority": null, "Product": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "RelatedCustomer": {
        "Id": 0,
        "Answer": null,
        "BankInfo": null,
        "BettingCode": null,
        "Country": 0,
        "CountryName": null,
        "CreatedDate": null,
        "Creator": null,
        "CustomerInfo": null,
        "IsVip": false,
        "Language": 0,
        "NickName": null,
        "Pic1": null,
        "Pic2": null,
        "Pic3": null,
        "Question": null,
        "RefCode2": null,
        "RelatedCustomer": null,
        "Remark": null,
        "RoleId": 0,
        "RoleName": "",
        "Site": null,
        "SpecialRemark": null,
        "UserName": ""
    },
    "ResolutionTime": null,
    "Response": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Review": "", "Reviewer": { "Id": 0, "IsAllowUpdateReview": false, "Password": null, "RoleId": 0, "Username": "" },
    "Role": { "Id": 7004, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Site": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Solution": " ",
    "SolvedDate": null,
    "Status": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "StrFeedbackDate": "",
    "SubCategory": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null },
    "Subject": "", "UpdateReviewMode": 0
};
var settings = ["Title", "Status", "Station", "Response", "Communication", "Language"];
var ddls = ["ddltitle", "ddlstatus", "ddlcsextension", "ddlresponse", "ddlcommunication"];
var listObjToValidate = ["txtsubject"];
var listEditorToValidate = ["desediter"];
var custName = "";
var roleName = "";
var custId = 0;
var role = 0;
var numberOfFinish = 0;
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

var cachedElements = {
    personalInfo: {},
    details: {},
    description: '',
    solution: ''
};

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
    var feedbackData = {
        Reviewer: 0,
        Review: "",
        CustId: custId,
        UserName: custName,
        Creator: accId,
        RoleID: $("#ddlrole").val(),
        RelatedCustID: "",
        Description: $("#desediter").htmlarea("toHtmlString"),
        Solution: $("#solediter").htmlarea("toHtmlString"),
        IsUpdateReviewed: $("#chkisreview").attr("checked")
    };

    if ($("#chkisrelated").attr("checked")) {
        feedbackData.RelatedCustID = getRelatedCustomer();
    }

    if (feedbackData.IsUpdateReviewed) {
        feedbackData.Reviewer = accId;
        feedbackData.Review = $("#revediter").htmlarea("toHtmlString");
    }

    var feedbackNew = getDetailsTabData();
    feedbackNew.CustomerInfo = getPersonalInfoTabData();
    feedbackNew.Description = cachedElements.description.htmlarea("toHtmlString");
    feedbackNew.Solution = cachedElements.solution.htmlarea("toHtmlString");

    feedbackNew.Customer.Id = feedbackData.CustId;
    feedbackNew.Customer.UserName = feedbackData.UserName;
    feedbackNew.Role.Id = feedbackData.RoleID;
    feedbackNew.Creator.Id = feedbackData.Creator;
    feedbackNew.RelatedCustomer.Id = 0;
    feedbackNew.RelatedCustomer.UserName = feedbackData.RelatedCustID;
    
    feedbackNew.IsReviewed = feedbackData.IsUpdateReviewed;
    if (feedbackData.IsUpdateReviewed) {
        feedbackNew.Reviewer.Id = feedbackData.Reviewer;
        feedbackNew.Review = feedbackData.Review.trim();
    }
    else {
        feedbackNew.Reviewer.Id = 0;
    }

    return feedbackNew;
}

function DoAdd() {
    var newFeedback = getFeedbackData();
    //-------check override crm info--------------
    if ($('#chkIsOverrideCrmInfo').attr('checked')) {
        $("#divOverrideCRMInfo").html('Current CRM Info will be overrided, please double check before Submit!');
        $("#divOverrideCRMInfo").dialog({ width: 400, height: 120, zIndex: 500, resizable: false,
            close: function (event, ui) {
                /* jshint unused: vars */
            },
            buttons: [{
                text: "Ok",
                click: function () {
                    newFeedback.IsOverrideCrmInfo = true;
                    $(this).dialog("close");
                    addFeedbackInfo(newFeedback);
                }
            },
            {
                text: 'Cancel',
                click: function () {
                    newFeedback.IsOverrideCrmInfo = false;
                    $(this).dialog("close");
                    //addFeedbackInfo(newFeedback);
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
    //check related customer
    if (feedbackObjectValidate(listEditorToValidate, listObjToValidate)) {
        //does not submit when feedback date > current date
        var currentDate = new Date();
        var selectDate = Date.parse($("#txtdate").val());

        var serverTime = new Date($('#hServerTime').val());
        serverTime.setMinutes(currentDate.getMinutes());
        serverTime.setSeconds(currentDate.getSeconds());

        if (selectDate > serverTime) {
            showAlertMessage("errmsg", errMsgFeedbackDateGreatter, 1);
            return false;
        }
        if (Date.parse($("#txtdate").val()) >= Date.parse($("#txtResolutionTime").val())) {
            showAlertMessage("errmsg", errMsgResolutionDateGreatter, 1);
            return false;
        }
        if ($('#ddlcommunication option:selected').text().toLowerCase() === 'n/a') {
            showAlertMessage("errmsg", "Please choose communication!", 1);
            return false;
        }
        if ($('#hIsNotChild').val() === '1') {
            showAlertMessage("errmsg",
            "There is no Subcategory for Category [" + $("#ddlcategory option:selected").text() + "]. Please add a Subcategory or choose another Category.", 1);
            return false;
        }
        //----------check length of data

        if ($('#txtfirstname').val().length > 50) {
            $('#txtfirstname').focus();
            showAlertMessage("errmsg", 'Max length of First Name is 50', 1);
            return false;
        }
        if ($('#txtlastname').val().length > 50) {
            $('#txtlastname').focus();
            showAlertMessage("errmsg", 'Max length of Last Name is 50', 1);
            return false;
        }
        if ($('#txtphone').val().length > 50) {
            $('#txtphone').focus();
            showAlertMessage("errmsg", 'Max length of Phone is 50', 1);
            return false;
        }
        if ($('#txtmobilephone').val().length > 50) {
            $('#txtmobilephone').focus();
            showAlertMessage("errmsg", 'Max length of Mobile phone is 50', 1);
            return false;
        }
        if ($('#txtfax').val().length > 50) {
            $('#txtfax').focus();
            showAlertMessage("errmsg", 'Max length of Fax is 50', 1);
            return false;
        }
        if ($('#txtEmail').val().length > 254) {
            $('#txtEmail').focus();
            showAlertMessage("errmsg", 'Max length of Email is 254', 1);
            return false;
        }
        if ($('#txtSkype').val().length > 100) {
            $('#txtSkype').focus();
            showAlertMessage("errmsg", 'Max length of Skype is 100', 1);
            return false;
        }
        if ($('#txtMSN').val().length > 100) {
            $('#txtMSN').focus();
            showAlertMessage("errmsg", 'Max length of MSN is 100', 1);
            return false;
        }
        if ($('#txtsubject').val().length > 200) {
            $('#txtsubject').focus();
            showAlertMessage("errmsg", 'Max length of Subject is 200', 1);
            return false;
        }

        if ($('#txtPIC1').val().length > 100) {
            $('#txtPIC1').focus();
            showAlertMessage("errmsg", 'Max length of PIC1 is 100', 1);
            return false;
        }

        if ($('#txtPIC2').val().length > 100) {
            $('#txtPIC2').focus();
            showAlertMessage("errmsg", 'Max length of PIC2 is 100', 1);
            return false;
        }

        if ($('#txtPIC3').val().length > 100) {
            $('#txtPIC3').focus();
            showAlertMessage("errmsg", 'Max length of PIC3 is 100', 1);
            return false;
        }

        if ($("#desediter").htmlarea("toHtmlString").length > 8000) {
            $('#desediter').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of Description is 8000', 1);
            return false;
        }
        if ($("#solediter").htmlarea("toHtmlString").length > 8000) {
            $('#solediter').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of Solution is 8000', 1);
            return false;
        }
        if ($("#revediter").htmlarea("toHtmlString").length > 8000) {
            $('#revediter').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of Review is 8000', 1);
            return false;
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
            bindPersonalInfoTab(rs.CustomerInfo);
            $.ajax({
                type: "POST",
                url: window.rootUrl + "AdminTool/SelAllSetting",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"Id":' + 0 + '}',
                success: function (setting) {
                    AllSetting = setting;

                    bindingSetting("ddltitle", setting.Title, rs.CustomerInfo.Title.Id);
                    bindingSetting("ddlcategory", setting.Category, 0);
                    bindingSetting("ddlproduct", setting.Product, 0);
                    bindingSetting("ddlrole", setting.Role, rs.RoleId);
                    bindingSetting("ddlstatus", setting.Status, 0);
                    bindingSetting("ddlresponse", setting.Response, 0);
                    bindingSetting("ddlcommunication", setting.Communication, 0);
                    bindingSetting("ddllanguage", setting.Language, rs.Language);
                    bindingSetting("ddlcsextension", setting.Extension, 0);
                    bindingCountry("ddlcountry", setting.Country, rs.Country);

                    bindingChildByParentId('ddlsubcategory', setting.Category, (setting.Category ? setting.Category[0].Id : 3000), 0);
                    bindingChildByParentId('ddlbettype', setting.Product, 0, 0);
                    bindingSite('ddlsite', setting.Site, rs.Site.Id);

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

            if (rs.RelatedCustomer != null && rs.RelatedCustomer.UserName !== "") {
                $("#chkisrelated").attr("checked", true);
                //                $("#txtrelatedcustomer").removeAttr("disabled").val(rs.RelatedCustomer.UserName);
                $("#btnverify").removeAttr("disabled");
            }
        }
    });
}

function cacheHtmlElements() {
    cachedElements.personalInfo = {
        Title: $("#ddltitle"),
        FirstName: $("#txtfirstname"),
        LastName: $("#txtlastname"),
        ContactNumber1: {
            PhoneNumber: $("#Contact1"),
            CountryCode: $("#Contact1CountryCode"),
            AreaCode: $("#Contact1AreaCode")
        },
        ContactNumber2: {
            PhoneNumber: $("#Contact2"),
            CountryCode: $("#Contact2CountryCode"),
            AreaCode: $("#Contact2AreaCode")
        },
        ContactNumber3: {
            PhoneNumber: $("#Contact3"),
            CountryCode: $("#Contact3CountryCode"),
            AreaCode: $("#Contact3AreaCode")
        },
        Fax: {
            FaxNumber: $("#txtfax"),
            FaxCountryCode: $("#FaxCountryCode"),
            FaxAreaCode: $("#FaxAreaCode")
        },
        Mobile: {
            PhoneNumber: $("#Mobile"),
            CountryCode: $("#MobileCountryCode"),
            AreaCode: $("#MobileAreaCode")
        },
        Skype: $("#txtSkype"),
        Email: $("#txtEmail"),
        MSN: $("#txtMSN"),
        PassportNumber: $("#PassportNumber"),
        Birthday: $("#Birthday"),
        Site: $("#ddlsite"),
        Languages: $("#ddllanguage"),
        BettingCode: $("#BettingCode"),
        RefCodes2: $("#RefCodes2"),
        IsVIP: $("#IsVIP"),
        PIC1: $("#txtPIC1"),
        PIC2: $("#txtPIC2"),
        PIC3: $("#txtPIC3"),
        PIC4: $("#PIC4")
    };
    
    cachedElements.details = {
        FeedbackDate: $("#txtdate"),
        CsExtension: $("#ddlcsextension"),
        ResolutionTime: $("#txtResolutionTime"),
        Communication: $("#ddlcommunication"),
        Category: $("#ddlcategory"),
        SubCategory: $("#ddlsubcategory"),
        Product: $("#ddlproduct"),
        BetType : $("#ddlbettype"),
        Status: $("#ddlstatus"),
        Response: $("#ddlresponse"),
        IsRelatedCustomer: $("#chkisrelated"),
        Subject: $("#txtsubject")
    };

    cachedElements.description = $("#desediter");
    cachedElements.solution = $("#solediter");
}

function bindPersonalInfoTab(customer) {
    var personal = cachedElements.personalInfo;
    personal.FirstName.val(customer.FirstName);
    personal.LastName.val(customer.LastName);
    personal.ContactNumber1.PhoneNumber.val(customer.ContactNumber1.PhoneNumber);
    personal.ContactNumber1.CountryCode.val(customer.ContactNumber1.CountryCode);
    personal.ContactNumber1.AreaCode.val(customer.ContactNumber1.AreaCode);
    personal.ContactNumber2.PhoneNumber.val(customer.ContactNumber2.PhoneNumber);
    personal.ContactNumber2.CountryCode.val(customer.ContactNumber2.CountryCode);
    personal.ContactNumber2.AreaCode.val(customer.ContactNumber2.AreaCode);
    personal.ContactNumber3.PhoneNumber.val(customer.ContactNumber3.PhoneNumber);
    personal.ContactNumber3.CountryCode.val(customer.ContactNumber3.CountryCode);
    personal.ContactNumber3.AreaCode.val(customer.ContactNumber3.AreaCode);
    personal.Fax.FaxNumber.val(customer.Fax.FaxNumber);
    personal.Fax.CountryCode.val(customer.Fax.CountryCode);
    personal.Fax.AreaCode.val(customer.Fax.AreaCode);
    personal.Mobile.PhoneNumber.val(customer.Mobile.PhoneNumber);
    personal.Mobile.CountryCode.val(customer.Mobile.CountryCode);
    personal.Mobile.AreaCode.val(customer.Mobile.AreaCode);
    personal.Skype.val(customer.Skype);
    personal.Email.val(customer.Email);
    personal.MSN.val(customer.MSN);
    personal.PassportNumber.val(customer.PassportNumber);
    personal.Birthday.val(customer.Birthday);
    personal.Site.val(customer.Site);
//    if (personal.Languages.fanexMultiSelect('getItems', true).length === customer.Languages.length) {
//        personal.Languages.fanexMultiSelect('setCheckAll', true);
//    }
//    else {

//    }
    personal.Country.val(customer.Country);
    personal.BettingCode.val(customer.BettingCode);
    personal.RefCodes2.val(customer.RefCodes2);
    personal.IsVIP.prop("checked", customer.IsVIP);
    personal.PIC1.val(customer.PIC1);
    personal.PIC2.val(customer.PIC2);
    personal.PIC3.val(customer.PIC3);
    personal.PIC3.val(customer.PIC3);
}

function getPersonalInfoTabData() {
    var personal = cachedElements.personalInfo;
    return {
        Title: {
            Id: personal.Title.val().trim()
        },
        FirstName: personal.FirstName.val().trim(),
        LastName: personal.LastName.val().trim(),
        ContactNumber1: {
            PhoneNumber: personal.ContactNumber1.PhoneNumber.val().trim(),
            CountryCode: personal.ContactNumber1.CountryCode.val().trim(),
            AreaCode: personal.ContactNumber1.AreaCode.val().trim()
        },
        ContactNumber2: {
            PhoneNumber: personal.ContactNumber2.PhoneNumber.val().trim(),
            CountryCode: personal.ContactNumber2.CountryCode.val().trim(),
            AreaCode: personal.ContactNumber2.AreaCode.val().trim()
        },
        ContactNumber3: {
            PhoneNumber: personal.ContactNumber3.PhoneNumber.val().trim(),
            CountryCode: personal.ContactNumber3.CountryCode.val().trim(),
            AreaCode: personal.ContactNumber3.AreaCode.val().trim()
        },
        Fax: {
            FaxNumber: personal.Fax.FaxNumber.val().trim(),
            CountryCode: personal.Fax.CountryCode.val().trim(),
            AreaCode: personal.Fax.AreaCode.val().trim()
        },
        Mobile: {
            PhoneNumber: personal.Mobile.PhoneNumber.val().trim(),
            CountryCode: personal.Mobile.CountryCode.val().trim(),
            AreaCode: personal.Mobile.AreaCode.val().trim()
        },
        Skype: personal.Skype.val().trim(),
        Email: personal.Email.val().trim(),
        MSN: personal.MSN.val().trim(),
        PassportNumber: personal.PassportNumber.val().trim(),
        Birthday: personal.Birthday.datepicker("getDate"),
        Site: personal.Site.val(),
        Languages: personal.Languages.fanexMultiSelect('getItems', true).join(','),
        BettingCode: personal.BettingCode.val().trim(),
        RefCodes2: personal.RefCodes2.val().trim(),
        IsVIP: personal.IsVIP.prop("checked"),
        PIC1: personal.PIC1.val().trim(),
        PIC2: personal.PIC2.val().trim(),
        PIC3: personal.PIC3.val().trim(),
        PIC4: personal.PIC4.val().trim()
    };
}

function getDetailsTabData() {
    var details = cachedElements.details;
    var relatedCustomers = details.IsRelatedCustomer.prop("checked") ? getRelatedCustomer() : [];
    return {
        FeedbackDate: details.FeedbackDate.datetimepicker("getDate"),
        CsExtension: {
            Id: details.CSExtension.val()
        },
        ResolutionTime: details.ResolutionTime.datetimepicker("getDate"),
        Communication: {
            Id: details.Communication.val()
        },
        Category: {
            Id: details.Category.val()
        },
        SubCategory: {
            Id: details.SubCategory.val()
        },
        Product: {
            Id: details.Product.val()
        },
        BetType : {
            Id: details.BetType.val()
        },
        Status: {
            Id: details.Status.val()
        },
        Response: {
            Id: details.Response.val()
        },
        RelatedCustomer: relatedCustomers,
        Subject: details.Response.val().trim()
    };
}

head.ready(function () {
    initHTMLArea('editboxdes', 'desediter', '');
    initHTMLArea('editboxsol', 'solediter', '');
    initHTMLArea('editboxrev', 'revediter', '');

    //Bind data to dropdown list
    try { parent.BlockUI(); } catch (e) { }
    custId = gup("custid") === "" ? 0 : parseInt(gup("custid"));
    custName = $.URLDecode(gup("custName"));

    $("#lblUserName").attr("value", custId).html(custName);
    getCRMInfoById(custId);

    var feedbackdate = new Date(clientTime);
    var resolutiondate = new Date(clientTime);
    resolutiondate.setMinutes(feedbackdate.getMinutes() + 1);
    $('#hServerTime').val(feedbackdate);

    var arrDateValues = [feedbackdate.format("m/d/Y H:i"), resolutiondate.format("m/d/Y H:i")];
    var arrIds = ['txtdate', 'txtResolutionTime'];
    /* jshint newcap: false */
    SetupCalendar(arrDateValues, arrIds);

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

    cacheHtmlElements();
});
