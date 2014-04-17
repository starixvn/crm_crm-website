"use strict";
/* global trim, 
bindingSetting,
bindingCountry, 
bindingChildByParentId,
bindingSite,
allowDeleteFeedback,
clientTime,
SetupCalendar,
desvalue: true,
solvalue: true,
initHTMLArea,
revvalue: true,
allowUpdateReview,
accId,
getRelatedCustomer,
showAlertMessage,
successMsg,
trimSpace,
trimSpacesOnEditor,
errMsgNotPermissionUpdReviewedFB,
feedbackObjectValidate,
errMsgFeedbackDateGreatter,
errMsgResolutionDateGreatter,
initEdit,
initReview,
checkDuplicateRelated,
head,
jlinq,
gup,
inputMultipleRelatedNote,
showHideDetailBox,
hotKeyManager,
bindingLanguage,
verifyUserName
*/
/* exported settings,
orgArrUser,
ddls,
EditFeedback,
isShowCustinfoBox,
isShowDescriptionBox,
isShowFeedbackDetailBox,
isShowReviewBox,
isShowSolutionBox,
invokeHotKey_CtrlS,
invokeHotKey_CtrlE
*/
var getNumOfFeedbackItems,
    appendListFeedbacksLasted,
    feedbackEdit;

//var settings = ["Title", "MainCategory", "Product", "Status", "Role", "Station", "Response", "Communication", "Language"];
//var ddls = ["ddltitle", "ddlcategory", "ddlproduct", "ddlstatus", "ddlrole", "ddlcsextension", "ddlresponse", "ddlcommunication", "ddllanguage"];
var id;
var feedbackIsReview = false;
var listObjToValidate = ["txtsubject"];
var listEditorToValidate = ["desediter"];
var responsePage = 1;

var FeedbackItem;
var AllSetting;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var orgLengt = 0;
var orgArrUser = "";
var maxHeightOfPopUp = ($(window).height() + 50);
var userName = '';
var isShowCustinfoBox = false;
var isShowFeedbackDetailBox = false;
var isShowDescriptionBox = false;
var isShowSolutionBox = false;
var isShowReviewBox = false;

// get stringUserName from db and trim to ArrUsername for render by fcbkcomplete
function trimToArrayUserName(str) {
    var newStr = trim(str);
    var subParent = newStr.split(',');
    return subParent;
}

function bindingEvent() {
    $('#ddlcategory').change(function () {
        bindingChildByParentId('ddlsubcategory', AllSetting.Category, parseInt($('#ddlcategory').val()));
    });

    $('#ddlproduct').change(function () {
        bindingChildByParentId('ddlbettype', AllSetting.Product, parseInt($('#ddlproduct').val()));
    });
}

function loadFeedbackAfterCallAjax(rs, Id) {
    /* jshint maxstatements: 200 */
    FeedbackItem = rs;
    var customerInfo = rs.CustomerInfo;
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/SelAllSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Id":' + Id + '}',
        success: function (setting) {
            AllSetting = setting;
            bindingSetting("ddltitle", setting.Title, customerInfo.Title.Id);
            bindingSetting("ddlcategory", setting.Category, rs.Category.Id);
            bindingSetting("ddlproduct", setting.Product, rs.Product.Id);
            bindingSetting("ddlrole", setting.Role, rs.Role.Id);
            bindingSetting("ddlstatus", setting.Status, rs.Status.Id);
            bindingSetting("ddlresponse", setting.Response, rs.Response.Id);
            bindingSetting("ddlcommunication", setting.Communication, rs.Communication.Id);
            bindingLanguage("ddllanguage", setting.Language, rs.Language.Id);
            bindingSetting("ddlcsextension", setting.Extension, rs.CsExtension.Id);
            bindingCountry("ddlcountry", setting.Country, rs.Country.Id);

            bindingChildByParentId('ddlsubcategory', setting.Category, rs.Category.Id, rs.SubCategory.Id);
            bindingChildByParentId('ddlbettype', setting.Product, rs.Product.Id, rs.BetType.Id);
            bindingSite('ddlsite', setting.Site, rs.Site.Id);

            if (!allowDeleteFeedback) {
                $("select#ddlstatus option[value='11004']").remove();
            }

            bindingEvent();
        },
        error: function () {
        },
        complete: function () {
            getNumOfFeedbackItems();
            setInterval(function () {
                getNumOfFeedbackItems();
            }, 100000);
        }
    });

    $("#lblUserName").text(rs.Customer.UserName).attr("value", rs.Customer.Id);
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
    $("#mobileInfoPhone").val(customerInfo.Mobile.PhoneNumber); 
    $("#skypeInfo").val(customerInfo.Skype);
    $("#emailInfo").val(customerInfo.Email);
    $("#msnInfo").val(customerInfo.MSN);
    $("#passportInfo").val(customerInfo.PassportNumber);
    $("#bettingCodeInfo").val(customerInfo.BettingCode);
    $("#refCodes2Info").val(customerInfo.RefCodes2);
    $("#isVIP").prop("checked", customerInfo.IsVIP);
    $("#pic1Info").val(customerInfo.PIC1);
    $("#pic2Info").val(customerInfo.PIC2);
    $("#pic3Info").val(customerInfo.PIC3);
    $("#pic4Info").val(customerInfo.PIC4);

    var feedbackdate = new Date(clientTime);
    var resolutiondate = new Date(clientTime);
    resolutiondate.setMinutes(feedbackdate.getMinutes() + 1);
    if (rs.FeedbackDate != null) {
        feedbackdate = new Date(rs.FeedbackDate);
    }

    if (rs.ResolutionTime) {
        resolutiondate = new Date(rs.ResolutionTime);
    }

    $("#birthdayInfo").datepicker({
        dateFormat: "M dd, yy",
        maxDate: new Date(clientTime),
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        yearRange: '-100:+0'
    });
    if (customerInfo.BirthDayString !== '') {
        $("#birthdayInfo").datepicker("setDate", new Date(customerInfo.BirthDayString));
    }

    //-------------------            
    var arrDateValues = [feedbackdate, resolutiondate];
    var arrIds = ['txtdate', 'txtResolutionTime'];
    /* jshint newcap: false */
    SetupCalendar(arrDateValues, arrIds);
    //-------------------

    var relatedCustomer = rs.RelatedCustomer ? rs.RelatedCustomer.UserName : "";
    if (relatedCustomer !== "") {
        var i;
        $("#chkisrelated").prop("checked", true);
        var MyJson = [{ "Customer": []}];
        var arrUserName = trimToArrayUserName(relatedCustomer);
        for (i = 0; i < arrUserName.length; i++) {
            MyJson[i] = { title: arrUserName[i], value: arrUserName[i] };
        }

        $('#holder li').remove();
        $('#txtrelatedcustomer option').remove();
        for (i = 0; i < MyJson.length; i++) {
            $("#txtrelatedcustomer").trigger("addItem", MyJson[i]);
        }

        $("#btnverify").removeAttr("disabled");
        //$("#btnsubmit").removeAttr("disabled");
        //$("#btnsubmitbottom").removeAttr("disabled");
        orgLengt = MyJson.length;
    }

    desvalue = rs.Description;
    solvalue = rs.Solution;

    $("#previewboxdes").html(desvalue);
    initHTMLArea('editboxdes', 'desediter', desvalue);

    $("#previewboxsol").html(solvalue);
    initHTMLArea('editboxsol', 'solediter', solvalue);
    if (!solvalue || solvalue.trim().length === 0) {
        initEdit('sol');
    }

    $("#txtsubject").val(rs.Subject);

    if (rs.Reviewer) {
        revvalue = rs.Review;
        var strsoldate = "";
        if (rs.SolvedDate != null) {
            strsoldate = rs.SolvedDate;
        }

        if (rs.Reviewer.Id > 0) {
            $("#lblEditedBy").text("(Reviewed by " + rs.Reviewer.Username + " on " + strsoldate + ")");
        }
        $("#previewboxrev").html(revvalue);
        initHTMLArea('editboxrev', 'revediter', revvalue);
    }
    else {
        initHTMLArea('editboxrev', 'revediter', '');
    }

    //Permission
    feedbackIsReview = rs.IsReviewed;
    if (feedbackIsReview) {
        $("#chkisreview").prop('checked', true);
        initEdit('rev');
    }

    if (allowUpdateReview) {
        $('#chkisreview').prop("disabled", false);
    }

    setTimeout(function () {
        $('#default-focus').focus();
        parent.UnBlockUI();
    }, 1000);
}

// When EditFeedBack is loading
function LoadFeedback(Id) {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Feedback/GetById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Id":' + Id + '}',
        success: function (rs) {
            loadFeedbackAfterCallAjax(rs, Id);
        }
    });
}

// Get data from form to update 
function getFeedbackData() {
    /* jshint maxstatements: 86 */
    var RelatedCustID = '';
    if ($("#chkisrelated").prop("checked")) {
        RelatedCustID = $('#txtrelatedcustomer option').length === 0 ? getRelatedCustomer() : "_";
    }

    var IsUpdateReviewed = $("#chkisreview").prop("checked");
    var reviewContent = $("#revediter").htmlarea("toHtmlString");
    var Reviewer = 0;
    var Review = "";
    if (IsUpdateReviewed && reviewContent !== '<br>' && reviewContent !== '') {
        Reviewer = accId;
        Review = reviewContent;
    } 

    feedbackEdit.Id = id;
    feedbackEdit.Creator.Id = accId;
    feedbackEdit.Customer.Id = $("#lblUserName").attr("value");
    feedbackEdit.CustomerInfo = {
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
        Birthday: $('#birthdayInfo').val(),
        BirthdayString: $('#birthdayInfo').val(),
        Site: $('#ddlsite').val(),
        BettingCode: $("#bettingCodeInfo").val().trim(),
        RefCodes2: $("#refCodes2Info").val().trim(),
        IsVIP: $("#isVIP").prop("checked"),
        PIC1: $("#pic1Info").val().trim(),
        PIC2: $("#pic2Info").val().trim(),
        PIC3: $("#pic3Info").val().trim(),
        PIC4: $("#pic4Info").val().trim()
    };
    feedbackEdit.FeedbackDate = $("#txtdate").val();
    feedbackEdit.ResolutionTime = $('#txtResolutionTime').val();
    feedbackEdit.CsExtension.Id = $("#ddlcsextension").val();
    feedbackEdit.Communication.Id = $("#ddlcommunication").val();
    feedbackEdit.Category.Id = $("#ddlcategory").val();
    feedbackEdit.SubCategory.Id = $("#ddlsubcategory").val();
    feedbackEdit.Product.Id = $("#ddlproduct").val();
    feedbackEdit.BetType.Id = $("#ddlbettype").val();
    feedbackEdit.Status.Id = $("#ddlstatus").val();
    feedbackEdit.Role.Id = $("#ddlrole").val();
    feedbackEdit.Response.Id = $("#ddlresponse").val();
    feedbackEdit.Country.Id = $("#ddlcountry").val();
    feedbackEdit.Language.Id = $("#ddllanguage").val();
    feedbackEdit.RelatedCustomer.Id = 0;
    feedbackEdit.RelatedCustomer.UserName = RelatedCustID;
    feedbackEdit.Subject = $("#txtsubject").val().trim();
    feedbackEdit.Description = $("#desediter").htmlarea("toHtmlString").trim();
    feedbackEdit.Solution = $("#solediter").htmlarea("toHtmlString").trim();

    feedbackEdit.IsReviewed = IsUpdateReviewed;
    feedbackEdit.UpdateReviewMode = IsUpdateReviewed ? 1 : 2;
    feedbackEdit.Reviewer.Id = Reviewer;
    feedbackEdit.Review = Review.trim();
    userName = $("#lblUserName").html();
    return feedbackEdit;
}

// Call Edit feedback from webservice
function updateFeedbackInfo(newFeedback) {
    try { parent.BlockUI(); } catch (e) { }

    var feedback = { feedback: newFeedback };
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Feedback/EditFeedback",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(feedback),
        success: function (rs) {
            if (rs.ErrCode === 0) {
                showAlertMessage("errmsg", successMsg, 0);

                try { parent.afterUpdate(true); } catch (e) { }

                if (newFeedback.IsOverrideCrmInfo) {
                    parent.leftPanelManager.loadLeftPanel(userName, true);
                }
            }
            else {
                showAlertMessage("errmsg", rs.ErrMsg, 1);
                parent.UnBlockUI();
            }
        },
        error: function () {
            showAlertMessage("errmsg", "Update fail", 1);
        },
        complete: function () {
            try {
                parent.UnBlockUI();
            } catch (e) {
                parent.UnBlockUI();
            }

        }
    });
}

// get feedback data from function getFeedbackData() and Doing update feedback by function updateFeedbackInfo(editFeedback)
function DoUpdate() {
    var editFeedback = getFeedbackData();
    //-------check override crm info--------------
    if ($('#chkIsOverrideCrmInfo').prop('checked')) {
        var divPopup = parent.$("#div-resultpopup");
        divPopup.html('The current CRM Info will be overwritten. Do you want to continue?');
        divPopup.removeClass("display-none");
        divPopup.dialog({ width: 400, height: 150, zIndex: 500, resizable: false, modal: true,
            close: function (event, ui) {
                /* jshint unused: vars */
            },
            buttons: [{
                text: "Ok",
                click: function () {
                    editFeedback.IsOverrideCrmInfo = true;
                    divPopup.dialog("close");
                    updateFeedbackInfo(editFeedback);
                }
            },
            {
                text: 'Cancel',
                click: function () {
                    editFeedback.IsOverrideCrmInfo = false;
                    divPopup.dialog("close");
                }
            }]
        });
    }
    else {
        updateFeedbackInfo(editFeedback);
    }
}

// Verify input data, if it valid doing update edit feedback by call function DoUpdate()
function EditFeedback() {
    /* jshint maxstatements: 88 */
    $("#errmsg").empty();
    $("#lblverify").html('');

    if (!allowUpdateReview) {
        showAlertMessage("errmsg", errMsgNotPermissionUpdReviewedFB, 1);
        return false;
    }

    var isReviewChecked = $('#chkisreview').prop('checked');
    if (isReviewChecked && (trimSpace(trimSpacesOnEditor($("#revediter").htmlarea("toHtmlString"))) === '')) {
        $('#revieweditererror').removeClass('hidden').html('This information is required');
        return false;
    }

    if (feedbackObjectValidate(listEditorToValidate, listObjToValidate)) {
        var birthdayText = $("#birthdayInfo").val().trim();
        if (birthdayText.length !== 0) {
            var birthday = new Date(birthdayText);
            if (isNaN(birthday)) {
                showAlertMessage("errmsg", "Birthday is invalid!", 1);
                return false;
            }
        }

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
            showAlertMessage("errmsg", "Please chosse communication!", 1);
            return false;
        }
        if ($("#ddllanguage").val() === '0') {
            showAlertMessage("errmsg", "Please choose language!", 1);
            return false;
        }

        if ($('#hIsNotChild').val() === '1') {
            showAlertMessage("errmsg", "There is no Subcategory for Category [" +
                $("#ddlcategory option:selected").text() +
             "]. Please add a Subcategory or choose another Category.", 1);
            return false;
        }
        //----------check length of data
        var descriptionLen = $("#desediter").htmlarea("toHtmlString").trim().length;
        if (descriptionLen === 0) {
            $('#desediter').focus();
            showAlertMessage("errmsg", 'Please input description!', 1);
            return false;
        }
        if (descriptionLen > 8000) {
            $('#desediter').focus();
            showAlertMessage("errmsg", 'Max length of Description is 8000!', 1);
            return false;
        }

        var solutionElement = $("#solediter");
        var solutionValue = solutionElement.htmlarea("toHtmlString");
        if (solutionValue === '' || solutionValue === '<br>') {
            solutionElement.focus();
            showAlertMessage("errmsg", 'Please input solution!', 1);
            return false;
        }
        if (solutionValue.length > 8000) {
            solutionElement.focus();
            showAlertMessage("errmsg", 'Max length of Solution is 8000!', 1);
            return false;
        }
        if ($("#revediter").htmlarea("toHtmlString").length > 8000) {
            $('#revediter').focus();
            showAlertMessage("errmsg", 'Max length of Review is 8000!', 1);
            return false;
        }
        //----------check related customer
        /*if ($("#chkisrelated").prop("checked")) {
            var customerIds = getRelatedCustomer();
            if (customerIds === "_") {
                showAlertMessage("errmsg", errMsgNotYetVerifyFeedbackRelatedCustomer, 1);
                return false;
            }
        }*/
        //------------------------
        /* jshint newcap: false */
        DoUpdate();
    }

    return false;
}

// In the case, Review is done. If you want to update, you must remmove a old review for new edit
var updateFeedback = {
    custId: 0,
    feedbackId: 0,
    userId: 0,
    title: '',
    firstName: '',
    lastName: '',
    phone: '',
    mobilePhone: '',
    fax: '',
    email: '',
    languageID: 0,
    countryID: 0,
    feedBackDate: '',
    communitationID: 0,
    stationID: 0,
    mainCategoryID: 0,
    subCategoryID: 0,
    siteID: 0,
    roleID: 0,
    productID: 0,
    bettypeID: 0,
    responseID: 0,
    statusID: 0,
    relatedCustID: '',
    subject: '',
    description: '',
    solution: '',
    isUpdateReviewMode: 2,
    reviewer: 0,
    review: '',
    skype: '',
    msn: '',
    resolutionTime: '',
    reviewCheckBox: function () {
        isShowReviewBox = true;
        var isReviewChecked = $("#chkisreview").prop("checked");
        if (isReviewChecked) {
            initEdit('rev');
        }
        else {
            initReview('rev');
            $('#revieweditererror').addClass('hidden');
            if (feedbackIsReview) {
                $('#previewboxrev').html('');
            }
        }

        showHideDetailBox(true, 'FeedbackReview');
    },
    getData: function () {
        /* jshint maxstatements: 75 */
        userName = $("#lblUserName").html();
        updateFeedback.custId = $("#lblUserName").attr("value");
        updateFeedback.feedbackId = id;
        updateFeedback.userId = accId;
        updateFeedback.title = $("#ddltitle").val();
        updateFeedback.firstName = $("#firstNameInfo").val();
        updateFeedback.lastName = $("#lastNameInfo").val();
        updateFeedback.phone = $("#txtphone").val();
        updateFeedback.mobilePhone = $("#txtmobilephone").val();
        updateFeedback.fax = $("#txtfax").val();
        updateFeedback.email = $("#emailInfo").val();
        updateFeedback.languageID = $("#ddllanguage").val();
        updateFeedback.countryID = $("#ddlcountry").val();
        updateFeedback.feedBackDate = $("#txtdate").val();
        updateFeedback.communitationID = $("#ddlcommunication").val();
        updateFeedback.stationID = $("#ddlcsextension").val();
        updateFeedback.mainCategoryID = $("#ddlcategory").val();
        updateFeedback.subCategoryID = $("#ddlsubcategory").val();
        updateFeedback.siteID = $("#ddlsite").val();
        updateFeedback.roleID = $("#ddlrole").val();
        updateFeedback.productID = $("#ddlproduct").val();
        updateFeedback.bettypeID = $("#ddlbettype").val();
        updateFeedback.responseID = $("#ddlresponse").val();
        updateFeedback.statusID = $("#ddlstatus").val();

        updateFeedback.relatedCustID = null;

        if ($("#chkisrelated").prop("checked")) {
            if ($('#txtrelatedcustomer option').length === 0) {
                updateFeedback.RelatedCustID = getRelatedCustomer();
            }
        }
        else {
            updateFeedback.RelatedCustID = "_";
        }

        updateFeedback.subject = $("#txtsubject").val();
        updateFeedback.description = $("#desediter").htmlarea("toHtmlString");
        updateFeedback.solution = $("#solediter").htmlarea("toHtmlString");
        //updateFeedback.isUpdateReviewed = true;
        updateFeedback.reviewer = 0;
        updateFeedback.review = "";
        updateFeedback.skype = $('#skypeInfo').val();
        updateFeedback.msn = $('#msnInfo').val();
        updateFeedback.resolutionTime = $('#txtResolutionTime').val();

        feedbackEdit.Creator.Id = updateFeedback.userId;
        feedbackEdit.Id = updateFeedback.feedbackId;
        feedbackEdit.Customer.Id = updateFeedback.custId;
        feedbackEdit.CustomerInfo.Title.Id = updateFeedback.title;
        feedbackEdit.CustomerInfo.FirstName = updateFeedback.firstName.trim();
        feedbackEdit.CustomerInfo.LastName = updateFeedback.lastName.trim();
        feedbackEdit.CustomerInfo.Phone = updateFeedback.phone.trim();
        feedbackEdit.CustomerInfo.MobilePhone = updateFeedback.mobilePhone.trim();
        feedbackEdit.CustomerInfo.Fax = updateFeedback.fax.trim();
        feedbackEdit.CustomerInfo.Email = updateFeedback.email.trim();
        feedbackEdit.CustomerInfo.Skype = updateFeedback.skype.trim();
        feedbackEdit.CustomerInfo.MSN = updateFeedback.msn.trim();
        feedbackEdit.FeedbackDate = updateFeedback.feedBackDate;
        feedbackEdit.Role.Id = updateFeedback.roleID;
        feedbackEdit.Language.Id = updateFeedback.languageID;
        feedbackEdit.Country.Id = updateFeedback.countryID;
        feedbackEdit.Communication.Id = updateFeedback.communitationID;
        feedbackEdit.CsExtension.Id = updateFeedback.stationID;
        feedbackEdit.Category.Id = updateFeedback.mainCategoryID;
        feedbackEdit.SubCategory.Id = updateFeedback.subCategoryID;
        feedbackEdit.Site.Id = updateFeedback.siteID;
        feedbackEdit.Product.Id = updateFeedback.productID;
        feedbackEdit.BetType.Id = updateFeedback.bettypeID;
        feedbackEdit.Response.Id = updateFeedback.responseID;
        feedbackEdit.Status.Id = updateFeedback.statusID;
        feedbackEdit.RelatedCustomer.Id = 0;
        feedbackEdit.RelatedCustomer.UserName = updateFeedback.relatedCustID;
        feedbackEdit.Subject = updateFeedback.subject.trim();
        feedbackEdit.Description = updateFeedback.description.trim();
        feedbackEdit.Solution = updateFeedback.solution.trim();

        feedbackEdit.Reviewer.Id = updateFeedback.reviewer;
        feedbackEdit.Review = updateFeedback.review.trim();
        feedbackEdit.UpdateReviewMode = updateFeedback.isUpdateReviewMode;

        feedbackEdit.ResolutionTime = updateFeedback.resolutionTime;

        feedbackEdit.Customer.Pic1 = $("#pic1Info").val().trim();
        feedbackEdit.Customer.Pic2 = $("#pic2Info").val().trim();
        feedbackEdit.Customer.Pic3 = $("#pic3Info").val().trim();

        return feedbackEdit;
    },
    unReview: function () {
        updateFeedback.openConfirmPopup();
    },
    unReviewUpdate: function () {
        $("#lblverify").html('');
        if ($("#chkisreview").prop("checked") && (trimSpace(trimSpacesOnEditor($("#revediter").htmlarea("toHtmlString"))))) {
            $('#revieweditererror').removeClass('hidden').html('This information is required');
            return false;
        }
        if (feedbackObjectValidate(listEditorToValidate, listObjToValidate)) {

            var currentDate = new Date();
            var selectDate = Date.parse($("#txtdate").val());

            var serverTime = new Date(clientTime);
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
                showAlertMessage("errmsg", "Please chosse communication!", 1);
                return false;
            }

            var userName = $("#txtrelatedcustomer").val();
            if ($("#chkisrelated").prop("checked")) {
                if (userName === "") {
                    updateFeedback.doUpdate();
                }
                else {
                    var dups = checkDuplicateRelated();
                    if (dups && dups.length > 0) {
                        var divPopup = parent.$("#div-resultpopup");
                        divPopup.html('Duplicate related customer(s): ' + dups + '. Are you sure you want to update this feedback?');
                        divPopup.removeClass("display-none");
                        divPopup.dialog({ width: 400, height: 150, zIndex: 500, resizable: false,
                            close: function (event, ui) {
                                /* jshint unused: vars */
                            },
                            buttons: [{
                                text: "Ok",
                                click: function () {
                                    updateFeedback.verifyRelatedAndUpdate(userName);
                                    divPopup.dialog("close");
                                }
                            },
                            {
                                text: 'Cancel',
                                click: function () {
                                    divPopup.dialog("close");
                                }
                            }]
                        });
                    }
                    else {
                        updateFeedback.verifyRelatedAndUpdate(userName);
                    }
                }
            }
            else {
                updateFeedback.doUpdate();
            }

        }
        return false;
    },
    verifyRelatedAndUpdate: function (userName) {

        $("#lblverify").addClass("smallloading");
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Customer/VerifyUserName",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"UserName":"' + userName + '"}',
            success: function (rs) {
                if (rs != null && rs.length > 0) {
                    var isRelatedFail = false;
                    var userNameArr = [];
                    var userId = '';
                    $("#lblverify").removeClass("smallloading").removeClass("hidden");
                    for (var i = 0; i < rs.length; i++) {
                        if (rs[i].CustId !== '0') {
                            userNameArr.push('<font style="color:green;">- ' + rs[i].UserName + ': Valid Related Customer</font><br/>');

                            userId += (rs[i].CustId + (i === rs.length - 1 ? '' : ","));
                        } else {
                            userNameArr.push('<font style="color:red;">- ' + rs[i].UserName + ': Invalid Related Customer</font><br/>');
                            isRelatedFail = true;
                        }
                    }

                    $("#lblverify").html(userNameArr.join('')).attr('value', userId);
                    if (!isRelatedFail) {
                        updateFeedback.doUpdate();
                    }
                }
                else {
                    $("#lblverify").removeClass("smallloading").removeClass("hidden").html('Invalid Related Customer').attr("value", "");
                    $("#txtrelatedcustomer").focus();
                }
            }
        });
    },
    updateFeedbackInfo: function (newFeedback) {
        try { parent.BlockUI(); } catch (e) { }
        var feedback = { feedback: newFeedback };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Feedback/EditFeedback",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(feedback),
            success: function (rs) {
                if (rs.ErrCode === 0) {
                    showAlertMessage("errmsg", successMsg, 0);
                    parent.afterUpdate(true);
                    //-------------                    
                    window.setTimeout(function () {
                        updateFeedback.openEditFeedback(updateFeedback.feedbackId);
                        //                        if (newFeedback.IsOverrideCrmInfo) {
                        //                            parent.leftPanelManager.loadLeftPanel(userName, true);
                        //                        }
                    }, 1000);
                }
                else {
                    showAlertMessage("errmsg", rs.ErrMsg, 1);
                }

                parent.UnBlockUI();
            },
            complete: function () {
                try { parent.UnBlockUI(); } catch (e) { }
            }
        });
    },
    doUpdate: function () {
        var newFeedback = updateFeedback.getData();
        //-------check override crm info--------------
        if ($('#chkIsOverrideCrmInfo').prop('checked')) {
            var divPopup = parent.$("#div-resultpopup");
            divPopup.html('The current CRM Info will be overwritten. Do you want to continue?');
            divPopup.removeClass("display-none");
            divPopup.dialog({ width: 400, height: 120, zIndex: 500, resizable: false, modal: true,
                title: '<img src="../Content/site/Images/error.gif" id="myNewImage" /> Warning',
                close: function (event, ui) {
                    /* jshint unused: vars */
                },
                buttons: [{
                    text: "Ok",
                    click: function () {
                        newFeedback.IsOverrideCrmInfo = true;
                        divPopup.dialog("close");
                        updateFeedback.updateFeedbackInfo(newFeedback);
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
            updateFeedback.updateFeedbackInfo(newFeedback);
        }
    },
    openEditFeedback: function (id) {
        try {
            var url = '../Feedback/EditFeedback?id=' + id + '&rep=' + responsePage;
            parent.OpenPopup(url, "Update Feedback", 1020, maxHeightOfPopUp, true, false, window, 1000, function () {
                updateFeedback.reloadFrame(responsePage);
            }, function () {
                updateFeedback.reloadFrame(responsePage);
            });
        } catch (e) { }
    },
    openConfirmPopup: function () {
        var divPopup = parent.$("#div-resultpopup");
        divPopup.html('Are you sure you want to remove the review?');
        divPopup.removeClass("display-none");
        divPopup.dialog({ width: 300, height: 120, zIndex: 500, resizable: false,
            close: function (event, ui) {
                /* jshint unused: vars */
            },
            buttons: [{
                text: "Ok",
                click: function () {
                    updateFeedback.unReviewUpdate();
                    divPopup.dialog("close");
                }
            },
            {
                text: 'Cancel',
                click: function () {
                    divPopup.dialog("close");
                }
            }]
        });
    },
    reloadFrame: function (responsePage) {
        switch (responsePage) {
            case "1": //customer latest feedback                    
                parent.frames['centerFr'].topFeedback.loadLastedFeedback(false);
                break;
            case "2": //system latest feedback                    
                parent.frames['centerFr'].topFeedback.loadLastedFeedback(false);
                break;
            case "3": //manage feedback                    
                parent.frames['centerFr'].lastedFeedback.loadLastedFeedback(false);
                break;
            case "4": //deleted feedback                    
                parent.frames['centerFr'].deletedFeedback.loadDeletedFeedback(false);
                break;
            default:
                break;
        }
    }
};

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
    this.EditFeedback();
};

var invokeHotKey_CtrlE = function () {
    verifyUserName();
};

head.ready(function () {
    //try { parentWnd = findMainWindow(window); } catch (e) { }
    $("#txtsubject").keyup(function (e) {
        var keyCode = e.keyCode;
        if (keyCode === 37 || keyCode === 38 || keyCode === 39 || keyCode === 40) {
            return true;
        }
        else if (keyCode !== 13) {
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

    id = gup("id");
    if (id !== "") {
        /* jshint newcap: false */
        LoadFeedback(id);
    }
    $('#btnRelatedHelp').tipsy({
        gravity: 'nw',
        title: function () {
            return inputMultipleRelatedNote;
        }
    });
    $("#txtrelatedcustomer").fcbkcomplete({
        /* jshint camelcase: false */
        json_url: window.rootUrl + 'Customer/RelatedCustomerAutoSuggestion',
        addontab: true,
        maxitems: 30,
        input_min_size: 1,
        height: 10,
        cache: false,
        newel: true
        //                    select_all_text: "select",
    });
    if ($("#chkisrelated").attr('checked')) {
        $("#typeCust").val("");
        $("#typeCust").removeAttr("disabled");
    }

    $("#btnsubmit").removeAttr("disabled");
    $("#btnsubmitbottom").removeAttr("disabled");
    responsePage = gup('rep');

    /* register hot key */
    hotKeyManager.CachedData.ActiveDocumentManager.push({
        Value: window,
        Id: 'AddGuestFeedback'
    });
});

feedbackEdit = {
    "Id": 0,
    "IsOverrideCrmInfo": false,
    "BetType": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Category": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Communication": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Country": {
        "Id": 0,
        "CountryCode": 0,
        "CountryName": null,
        "DisplayOrder": 0,
        "IsActive": false
    },
    "CreateDate": null,
    "Creator": {
        "Id": 0,
        "IsAllowUpdateReview": false,
        "Password": null,
        "RoleId": 0,
        "Username": ""
    },
    "CsExtension": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
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
        "Title": {
            "Id": 0,
            "Child": null,
            "DisplayOrder": 0,
            "IsSub": false,
            "Parent": null,
            "Type": null,
            "Value": null
        }
    },
    "Description": "",
    "FeedbackDate": "",
    "IsReviewed": false,
    "Language": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Priority": null,
    "Product": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
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
    "Response": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Review": "",
    "Reviewer": {
        "Id": 0,
        "IsAllowUpdateReview": false,
        "Password": null,
        "RoleId": 0,
        "Username": ""
    },
    "Role": {
        "Id": 7004,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Site": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Solution": " ",
    "SolvedDate": null,
    "Status": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "StrFeedbackDate": "",
    "SubCategory": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Subject": "",
    "UpdateReviewMode": 0
};
