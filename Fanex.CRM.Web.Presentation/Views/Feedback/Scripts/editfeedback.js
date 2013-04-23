/*globals _comma_separated_list_of_variables_*/
var settings = ["Title", "MainCategory", "Product", "Status", "Role", "Station", "Response", "Communication", "Language"];
var ddls = ["ddltitle", "ddlcategory", "ddlproduct", "ddlstatus", "ddlrole", "ddlcsextension", "ddlresponse", "ddlcommunication", "ddllanguage"];
var id;
var feedbackIsReview = false;
var parentWnd;
var listObjToValidate = ["txtsubject"];
var listEditorToValidate = ["desediter"];
var responsePage = 1;

var FeedbackItem;
var AllSetting;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var orgLengt = 0;
var orgArrUser = "";
var maxHeightOfPopUp = ($(window).height() + 50)
var userName = '';
// get stringUserName from db and trim to ArrUsername for render by fcbkcomplete
function trimToArrayUserName(str)
{
    var newStr = trim(str);
    var subParent = newStr.split(',');
    return subParent;
}

// When EditFeedBack is loading
function LoadFeedback(Id) {
    
    try { parentWnd.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Feedback/GetById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Id":' + Id + '}',
        success: function (rs)
        {
            FeedbackItem = rs;

            $.ajax({
                type: "POST",
                url: window.rootUrl + "AdminTool/SelAllSetting",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"Id":' + Id + '}',
                success: function (setting)
                {
                    AllSetting = setting;

                    bindingSetting("ddltitle", setting.Title, rs.CustomerInfo.Title.Id);
                    bindingSetting("ddlcategory", setting.Category, rs.Category.Id);
                    bindingSetting("ddlproduct", setting.Product, rs.Product.Id);
                    bindingSetting("ddlrole", setting.Role, rs.Role.Id);
                    bindingSetting("ddlstatus", setting.Status, rs.Status.Id);
                    bindingSetting("ddlresponse", setting.Response, rs.Response.Id);
                    bindingSetting("ddlcommunication", setting.Communication, rs.Communication.Id);
                    bindingSetting("ddllanguage", setting.Language, rs.Language.Id);
                    bindingSetting("ddlcsextension", setting.Extension, rs.CsExtension.Id);
                    bindingCountry("ddlcountry", setting.Country, rs.Country.Id);

                    bindingChildByParentId('ddlsubcategory', setting.Category, rs.Category.Id, rs.SubCategory.Id);
                    bindingChildByParentId('ddlbettype', setting.Product, rs.Product.Id, rs.BetType.Id);
                    bindingSite('ddlsite', setting.Site, rs.Site.Id);

                    if (!allowDeleteFeedback)
                        $("select#ddlstatus option[value='11004']").remove();
                },
                error: function ()
                {

                },
                complete: function ()
                {
                    getNumOfFeedbackItems();
                    setInterval("getNumOfFeedbackItems()", 100000);
                }
            });


            $("#txtfirstname").val(rs.CustomerInfo.FirstName);
            $("#txtlastname").val(rs.CustomerInfo.LastName);
            $("#txtphone").val(rs.CustomerInfo.Phone);
            $("#txtmobilephone").val(rs.CustomerInfo.MobilePhone);
            $("#txtfax").val(rs.CustomerInfo.Fax);
            $("#txtEmail").val(rs.CustomerInfo.Email);
            $("#txtSkype").val(rs.CustomerInfo.Skype);
            $("#txtMSN").val(rs.CustomerInfo.MSN);
            $("#lblUserName").text(rs.Customer.UserName).attr("value", rs.Customer.Id);

            //insert by David 08-03-2012
            $("#txtPIC1").val(rs.Customer.Pic1);
            $("#txtPIC2").val(rs.Customer.Pic2);
            $("#txtPIC3").val(rs.Customer.Pic3);
            //
            var feedbackdate = new Date(clientTime);
            var resolutiondate = new Date(clientTime);
            resolutiondate.setMinutes(feedbackdate.getMinutes() + 1);
            $('#hServerTime').val(feedbackdate);
            if (rs.FeedbackDate != null)
                feedbackdate = new Date(rs.FeedbackDate);
            if (rs.ResolutionTime)
                resolutiondate = new Date(rs.ResolutionTime);
            //-------------------            
            var arrDateValues = [feedbackdate, resolutiondate];
            var arrIds = ['txtdate', 'txtResolutionTime'];
            SetupCalendar(arrDateValues, arrIds);
            //-------------------

            if (rs.RelatedCustomer != null && rs.RelatedCustomer.UserName != "")
            {

                $("#chkisrelated").attr("checked", true);
                //                $("#txtrelatedcustomer").removeAttr("disabled").val(rs.RelatedCustomer.UserName);
                var MyJson = [{ "Customer": []}];
                var arrUserName = trimToArrayUserName(rs.RelatedCustomer.UserName);
                for (var i = 0; i < arrUserName.length; i++)
                {
                    MyJson[i] = { title: arrUserName[i], value: arrUserName[i] };
                }
                $('#holder li').remove();
                $('#txtrelatedcustomer option').remove();


                for (var i = 0; i < MyJson.length; i++)
                {
                    $("#txtrelatedcustomer").trigger("addItem", MyJson[i]);
                }
                $("#btnverify").removeAttr("disabled");
                $("#btnsubmit").removeAttr("disabled");
                $("#btnsubmitbottom").removeAttr("disabled");
                orgLengt = MyJson.length;
            }

            desvalue = rs.Description;
            solvalue = rs.Solution;

            $("#previewboxdes").html(desvalue);
            initHTMLArea('editboxdes', 'desediter', desvalue);

            $("#previewboxsol").html(solvalue);
            initHTMLArea('editboxsol', 'solediter', solvalue);

            $("#txtsubject").val(rs.Subject);

            if (rs.Reviewer != null)
            {
                revvalue = rs.Review;
                var strsoldate = "";
                if (rs.SolvedDate != null)
                {
                    strsoldate = rs.SolvedDate;
                }
                $("#lblEditedBy").text("(Reviewed by " + rs.Reviewer.Username + " on " + strsoldate + ")");


                $("#previewboxrev").html(revvalue);
                initHTMLArea('editboxrev', 'revediter', revvalue);
            }
            else
            {
                initHTMLArea('editboxrev', 'revediter', '');
            }

            //Permission
            feedbackIsReview = rs.IsReviewed;

            if (allowUpdateReview)
            {
                if (feedbackIsReview)
                {
                    $(".updateFeedback").addClass("hidden");
                    $("#btnUnReview").removeClass("hidden");
                } else
                {
                    $("#chkisreview").removeClass("hidden");
                    $("#lblUpdateReview").removeClass("hidden");
                    $("#btnrevedit").removeClass("hidden");
                    $(".updateFeedback").removeClass("hidden");
                }
            }
            else
            {
                if (feedbackIsReview)
                {
                    $(".updateFeedback").addClass("hidden");
                }
            }

            setTimeout('parentWnd.UnBlockUI();', 1000);

        }
    });
};

// Get data from form to update 
function getFeedbackData(){
    var data = {};
    var CustId = $("#lblUserName").attr("value");
    userName = $('#lblUserName').html();
    var FeedbackId = id;
    var UserId = accId;
    var Title = $("#ddltitle").val();
    var FirstName = $("#txtfirstname").val();
    var LastName = $("#txtlastname").val();
    var Phone = $("#txtphone").val();
    var MobilePhone = $("#txtmobilephone").val();
    var Fax = $("#txtfax").val();
    var Email = $("#txtEmail").val();
    var LanguageID = $("#ddllanguage").val();
    var CountryID = $("#ddlcountry").val();
    var FeedBackDate = $("#txtdate").val();
    var CommunitationID = $("#ddlcommunication").val();
    var StationID = $("#ddlcsextension").val();
    var MainCategoryID = $("#ddlcategory").val();
    var SubCategoryID = $("#ddlsubcategory").val();
    var SiteID = $("#ddlsite").val();
    var RoleID = $("#ddlrole").val();
    var ProductID = $("#ddlproduct").val();
    var BettypeID = $("#ddlbettype").val();
    var ResponseID = $("#ddlresponse").val();
    var StatusID = $("#ddlstatus").val();
    var RelatedCustID = null;
    if ($("#chkisrelated").attr("checked")) {        
        if ($('#txtrelatedcustomer option').length == 0)
        {
            RelatedCustID = getRelatedCustomer();
        }
    }
    else RelatedCustID = "_";
    var Subject = $("#txtsubject").val();
    var Description = $("#desediter").htmlarea("toHtmlString");
    var Solution = $("#solediter").htmlarea("toHtmlString");
    var IsUpdateReviewed = $("#chkisreview").attr("checked");
    var updateReviewMode = IsUpdateReviewed ? 1 : 0;
    var reviewContent = $("#revediter").htmlarea("toHtmlString");
    if (IsUpdateReviewed && reviewContent != '<br>' && reviewContent != '')
    {
        var Reviewer = accId;
        var Review = reviewContent;
    }
    else
    {
        var Reviewer = 0;
        var Review = "";
    }

    var skype = $('#txtSkype').val();
    var msn = $('#txtMSN').val();
    var resolutionTitme = $('#txtResolutionTime').val();

    // insert by David.Tran 07-03-2012
    var pic1 = $("#txtPIC1").val();
    var pic2 = $("#txtPIC2").val();
    var pic3 = $("#txtPIC3").val();
    feedbackEdit.Customer.Pic1 = pic1.trim();
    feedbackEdit.Customer.Pic2 = pic2.trim();
    feedbackEdit.Customer.Pic3 = pic3.trim();
    
    feedbackEdit.Id = FeedbackId;
    feedbackEdit.Customer.Id = CustId;
    //feedbackEdit.Customer.UserName = UserName;
    feedbackEdit.CustomerInfo.Title.Id = Title;
    feedbackEdit.CustomerInfo.FirstName = FirstName.trim();
    feedbackEdit.CustomerInfo.LastName = LastName.trim();
    feedbackEdit.CustomerInfo.Phone = Phone.trim();
    feedbackEdit.CustomerInfo.MobilePhone = MobilePhone.trim();
    feedbackEdit.CustomerInfo.Fax = Fax.trim();
    feedbackEdit.CustomerInfo.Email = Email.trim();
    feedbackEdit.CustomerInfo.Skype = skype.trim();
    feedbackEdit.CustomerInfo.MSN = msn.trim();
    feedbackEdit.FeedbackDate = FeedBackDate;
    feedbackEdit.Role.Id = RoleID;
    feedbackEdit.Language.Id = LanguageID;
    feedbackEdit.Country.Id = CountryID;
    feedbackEdit.Creator.Id = UserId;
    feedbackEdit.Communication.Id = CommunitationID;
    feedbackEdit.CsExtension.Id = StationID;
    feedbackEdit.Category.Id = MainCategoryID;
    feedbackEdit.SubCategory.Id = SubCategoryID;
    feedbackEdit.Site.Id = SiteID;
    feedbackEdit.Product.Id = ProductID;
    feedbackEdit.BetType.Id = BettypeID;
    feedbackEdit.Response.Id = ResponseID;
    feedbackEdit.Status.Id = StatusID;
    feedbackEdit.RelatedCustomer.Id = 0;

    feedbackEdit.RelatedCustomer.UserName = RelatedCustID;

    feedbackEdit.Subject = Subject.trim();
    feedbackEdit.Description = Description.trim();
    feedbackEdit.Solution = Solution.trim();

    feedbackEdit.Reviewer.Id = Reviewer;
    feedbackEdit.Review = Review.trim();
    feedbackEdit.UpdateReviewMode = updateReviewMode;

    feedbackEdit.ResolutionTime = resolutionTitme;
    return feedbackEdit;
};

// Call Edit feedback from webservice
function updateFeedbackInfo(newFeedback)
{
    try { parent.BlockUI(); } catch (e) { }

    var feedback = { feedback: newFeedback };
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Feedback/EditFeedback",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(feedback),
        success: function (rs) {
            if (rs.ErrCode == 0) {
                showAlertMessage("errmsg", successMsg, 0);
                
                try { parentWnd.afterUpdate(true); } catch (e) { }
                
                if (newFeedback.IsOverrideCrmInfo) {
                    parentWnd.submitSearch(userName);
                }
            }
            else {
                showAlertMessage("errmsg", rs.ErrMsg, 1);
                //parentWnd.UnBlockUI();
            }
        },
        error: function () {
            showAlertMessage("errmsg", "Update fail", 1);
        },
        complete: function () {
            try {
                parentWnd.UnBlockUI();
            } catch (e) {
                parent.UnBlockUI();
            }

        }
    });
}

// get feedback data from function getFeedbackData() and Doing update feedback by function updateFeedbackInfo(newFeedback)
function DoUpdate()
{
    var newFeedback = getFeedbackData();
    //-------check override crm info--------------
    if ($('#chkIsOverrideCrmInfo').attr('checked'))
    {
        $("#divPopup").html('The current CRM Info will be overwritten. Do you want to continue?');
        $("#divPopup").dialog({ width: 400, height: 120, zIndex: 500, resizable: false,
            close: function (event, ui) { },
            buttons: [
                            {
                                text: "Ok",
                                click: function () {
                                    newFeedback.IsOverrideCrmInfo = true;
                                    $(this).dialog("close");
                                    updateFeedbackInfo(newFeedback);

                                   
                                }
                            },
                            {
                                text: 'Cancel',
                                click: function () {
                                    newFeedback.IsOverrideCrmInfo = false;
                                    $(this).dialog("close");
                                    //updateFeedbackInfo(newFeedback);
                                }
                            }
                        ]
        });
    }
    else    
        updateFeedbackInfo(newFeedback);
};

// Verify input data, if it valid doing update edit feedback by call function DoUpdate()
function EditFeedback()
{

    $("#lblverify").html('');

    if ($("#chkisreview").attr("checked") && (trimSpace(trimSpacesOnEditor($("#revediter").htmlarea("toHtmlString"))) == '' ))
    {
        $('#revieweditererror').removeClass('hidden').html('This information is required');
        return false;
    }

    if (feedbackIsReview && !allowUpdateReview)
    {
        showAlertMessage("errmsg", errMsgNotPermissionUpdReviewedFB, 1);
        return false;
    }

    if (feedbackObjectValidate(listEditorToValidate, listObjToValidate))
    {
        var currentDate = new Date();
        var selectDate = Date.parse($("#txtdate").val());

        var serverTime = new Date($('#hServerTime').val());
        serverTime.setMinutes(currentDate.getMinutes());
        serverTime.setSeconds(currentDate.getSeconds());

        if (selectDate > serverTime) {
            showAlertMessage("errmsg", errMsgFeedbackDateGreatter, 1);
            return false;
        }
        if (Date.parse($("#txtdate").val()) >= Date.parse($("#txtResolutionTime").val()))
        {
            showAlertMessage("errmsg", errMsgResolutionDateGreatter, 1);
            return false;
        }
        if ($('#ddlcommunication option:selected').text().toLowerCase() == 'n/a')
        {
            showAlertMessage("errmsg", "Please chosse communication!", 1);
            return false;
        }

        if ($('#hIsNotChild').val() == '1')
        {
            showAlertMessage("errmsg", "There is no Subcategory for Category [" + $("#ddlcategory option:selected").text() + "]. Please add a Subcategory or choose another Category.", 1);
            return false;
        }
        //----------check length of data

        if ($('#txtfirstname').val().length > 50)
        {
            $('#txtfirstname').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of First Name is 50', 1);
            return false;
        }
        if ($('#txtlastname').val().length > 50)
        {
            $('#txtlastname').focus();
            showAlertMessage("errmsg", 'Max length of Last Name is 50', 1);
            return false;
        }
        if ($('#txtphone').val().length > 50)
        {
            $('#txtphone').focus();
            showAlertMessage("errmsg", 'Max length of Phone is 50', 1);
            return false;
        }
        if ($('#txtmobilephone').val().length > 50)
        {
            $('#txtmobilephone').focus();
            showAlertMessage("errmsg", 'Max length of Mobile phone is 50', 1);
            return false;
        }
        if ($('#txtfax').val().length > 50)
        {
            $('#txtfax').focus();
            showAlertMessage("errmsg", 'Max length of Fax is 50', 1);
            return false;
        }
        if ($('#txtEmail').val().length > 254)
        {
            $('#txtEmail').focus();
            showAlertMessage("errmsg", 'Max length of Email is 254', 1);
            return false;
        }
        if ($('#txtSkype').val().length > 100)
        {
            $('#txtSkype').focus();
            showAlertMessage("errmsg", 'Max length of Skype is 100', 1);
            return false;
        }
        if ($('#txtMSN').val().length > 100)
        {
            $('#txtMSN').focus();
            showAlertMessage("errmsg", 'Max length of MSN is 100', 1);
            return false;
        }
        if ($('#txtsubject').val().length > 200)
        {
            $('#txtsubject').focus();
            showAlertMessage("errmsg", 'Max length of Subject is 200', 1);
            return false;
        }
        if ($('#txtPIC1').val().length > 100)
            if ($('#txtPIC1').val().length > 50)
            {
                $('#txtPIC1').focus();
                //$('#txtfirstname').css({ color: 'red'});
                showAlertMessage("errmsg", 'Max length of PIC1 is 100', 1);
                return false;
            }
        if ($('#txtPIC2').val().length > 100)
        {
            $('#txtPIC2').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of PIC2 is 100', 1);
            return false;
        }
        if ($('#txtPIC3').val().length > 100)
        {
            $('#txtPIC3').focus();
            //$('#txtfirstname').css({ color: 'red'});
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
        DoUpdate();


    }
    return false;
};

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
    resolutionTitme: '',
    reviewCheckBox: function () {
        if ($("#chkisreview").attr("checked")) {
            initEdit('rev');
        }
        else {
            initReview('rev');
            $('#revieweditererror').addClass('hidden');
        }
    },
    getData: function () {
        userName = $("#lblUserName").html();
        updateFeedback.custId = $("#lblUserName").attr("value");
        updateFeedback.feedbackId = id;
        updateFeedback.userId = accId;
        updateFeedback.title = $("#ddltitle").val();
        updateFeedback.firstName = $("#txtfirstname").val();
        updateFeedback.lastName = $("#txtlastname").val();
        updateFeedback.phone = $("#txtphone").val();
        updateFeedback.mobilePhone = $("#txtmobilephone").val();
        updateFeedback.fax = $("#txtfax").val();
        updateFeedback.email = $("#txtEmail").val();
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

        if ($("#chkisrelated").attr("checked")) {
            if ($('#txtrelatedcustomer option').length == 0) {
                updateFeedback.RelatedCustID = getRelatedCustomer();
            }
        }
        else
            updateFeedback.RelatedCustID = "_";

        updateFeedback.subject = $("#txtsubject").val();
        updateFeedback.description = $("#desediter").htmlarea("toHtmlString");
        updateFeedback.solution = $("#solediter").htmlarea("toHtmlString");
        //updateFeedback.isUpdateReviewed = true;
        updateFeedback.reviewer = 0;
        updateFeedback.review = "";
        updateFeedback.skype = $('#txtSkype').val();
        updateFeedback.msn = $('#txtMSN').val();
        updateFeedback.resolutionTitme = $('#txtResolutionTime').val();

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

        feedbackEdit.ResolutionTime = updateFeedback.resolutionTitme;

        feedbackEdit.Customer.Pic1 = $("#txtPIC1").val().trim();
        feedbackEdit.Customer.Pic2 = $("#txtPIC2").val().trim();
        feedbackEdit.Customer.Pic3 = $("#txtPIC3").val().trim();

        return feedbackEdit;
    },
    unReview: function () {
        updateFeedback.openConfirmPopup();
    },
    unReviewUpdate: function () {
        $("#lblverify").html('');
        if ($("#chkisreview").attr("checked") && (trimSpace(trimSpacesOnEditor($("#revediter").htmlarea("toHtmlString"))))) {
            $('#revieweditererror').removeClass('hidden').html('This information is required');
            return false;
        }
        if (feedbackObjectValidate(listEditorToValidate, listObjToValidate)) {

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
            if ($('#ddlcommunication option:selected').text().toLowerCase() == 'n/a') {
                showAlertMessage("errmsg", "Please chosse communication!", 1);
                return false;
            }

            var userName = $("#txtrelatedcustomer").val();
            if ($("#chkisrelated").attr("checked")) {
                if (userName == "") {
                    updateFeedback.doUpdate();
                }
                else {
                    var dups = checkDuplicateRelated();
                    if (dups && dups.length > 0) {
                        $("#divPopup").html('Duplicate related customer(s): ' + dups + '. Are you sure you want to update this feedback?');
                        $("#divPopup").dialog({ width: 400, height: 120, zIndex: 500, resizable: false,
                            close: function (event, ui) { },
                            buttons: [
                            {
                                text: "Ok",
                                click: function () {
                                    updateFeedback.verifyRelatedAndUpdate(userName);
                                    $(this).dialog("close");
                                }
                            },
                            {
                                text: 'Cancel',
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                ]
                        });
                    }
                    else
                        updateFeedback.verifyRelatedAndUpdate(userName);
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
                        if (rs[i].CustId != '0') {
                            userNameArr.push('<font style="color:green;">- ' + rs[i].UserName + ': Valid Related Customer</font><br/>');

                            userId += (rs[i].CustId + (i == rs.length - 1 ? '' : ","));
                        } else {
                            userNameArr.push('<font style="color:red;">- ' + rs[i].UserName + ': Invalid Related Customer</font><br/>');
                            isRelatedFail = true;
                        }
                    }

                    $("#lblverify").html(userNameArr.join('')).attr('value', userId);
                    if (!isRelatedFail)
                        updateFeedback.doUpdate();
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
                if (rs.ErrCode == 0) {
                    showAlertMessage("errmsg", successMsg, 0);
                    //parentWnd.afterUpdate(true);  
                    //-------------                    
                    window.setTimeout(function () {
                        updateFeedback.openEditFeedback(updateFeedback.feedbackId);
//                        if (newFeedback.IsOverrideCrmInfo) {
//                            parentWnd.submitSearch(userName);
//                        }
                    }, 1000);
                }
                else
                    showAlertMessage("errmsg", rs.ErrMsg, 1);
                //parentWnd.UnBlockUI();
            },
            complete: function () {
                try { parentWnd.UnBlockUI(); } catch (e) { }
            }
        });
    },
    doUpdate: function () {

        var newFeedback = updateFeedback.getData();
        //-------check override crm info--------------
        if ($('#chkIsOverrideCrmInfo').attr('checked')) {
            $("#divPopup").html('The current CRM Info will be overwritten. Do you want to continue?');
            $("#divPopup").dialog({ width: 400, height: 120, zIndex: 500, resizable: false, modal: true,
                title: '<img src="../Content/Black2011/Images/error.gif" id="myNewImage" /> Warning',
                close: function (event, ui) { },
                buttons: [
                            {
                                text: "Ok",
                                click: function () {
                                    newFeedback.IsOverrideCrmInfo = true;
                                    $(this).dialog("close");
                                    updateFeedback.updateFeedbackInfo(newFeedback);
                                }
                            },
                            {
                                text: 'Cancel',
                                click: function () {
                                    newFeedback.IsOverrideCrmInfo = false;
                                    $(this).dialog("close");
                                    //updateFeedback.updateFeedbackInfo(newFeedback);
                                }
                            }
                        ]
            });
        }
        else {
            updateFeedback.updateFeedbackInfo(newFeedback);
        }
    },
    openEditFeedback: function (id) {
        try {
            var url = '../Feedback/EditFeedback?id=' + id + '&rep=' + responsePage;
            parentWnd.OpenPopup(url, "Update Feedback", 1020, maxHeightOfPopUp, true, false, window, 1000, function () {
                updateFeedback.reloadFrame(responsePage);
            }, function () {
                updateFeedback.reloadFrame(responsePage);
            });
        } catch (e) { }
    },
    openConfirmPopup: function () {
        $("#divPopup").html('Are you sure you want to remove the review?');
        $("#divPopup").dialog({ width: 300, height: 120, zIndex: 500, resizable: false,
            close: function (event, ui) { },
            buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            updateFeedback.unReviewUpdate();
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: 'Cancel',
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
        });
    },
    reloadFrame: function (responsePage) {
        switch (responsePage) {
            case "1": //customer latest feedback                    
                parentWnd.frames['centerFr'].topFeedback.loadLastedFeedback(false);
                break;
            case "2": //system latest feedback                    
                parentWnd.frames['centerFr'].topFeedback.loadLastedFeedback(false);
                break;
            case "3": //manage feedback                    
                parentWnd.frames['centerFr'].lastedFeedback.loadLastedFeedback(false);
                break;
            case "4": //deleted feedback                    
                parentWnd.frames['centerFr'].deletedFeedback.loadDeletedFeedback(false);
                break;
            default:
                break;
        }
    }
};

//mark because it may conflict with other function 
function getNumOfFeedbackItems()
{
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetNumOfFeedbackSubjectTemplateItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs)
        {
            numberOfFeedbackItem = rs;
            listFeedbacksLasted = [];
            var numOfRange = Math.round((numberOfFeedbackItem / 100000) + 0.5);
            for (var i = 1; i <= numOfRange; i++)
            {
                appendListFeedbacksLasted(i);
            }
        },
        error: function ()
        {
        },
        complete: function ()
        {
        }
    });
};

function appendListFeedbacksLasted(range)
{
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetFeedbackSubjectTemplateItemsRange",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{ "range": "' + range.toString() + '" }',
        success: function (rs)
        {
            if (rs != null && rs.length > 0)
            {
                listFeedbacksLasted = listFeedbacksLasted.concat(rs);
            }
        },
        error: function ()
        {
        },
        complete: function ()
        {
        }
    });
};

head.ready(function () {
    try { parentWnd = findMainWindow(window); } catch (e) { }

    $("#txtsubject").keyup(function (e) {
        if (e.keyCode == 37 || e.keyCode == 38 || e.keyCode == 39 || e.keyCode == 40) return true;
        else if (e.keyCode == 13) {
            //search($(this).val());
        }
        else {
            var rows = [];
            if ($(this).val() != "") {
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
    
    if (id != "") {
        LoadFeedback(id);
    }
    $('#btnRelatedHelp').tipsy({ gravity: 'nw', title: function () { return inputMultipleRelatedNote } });
    $("#txtrelatedcustomer").fcbkcomplete({
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
});

var feedbackEdit = { "Id": 0, "IsOverrideCrmInfo": false, "BetType": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Category": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Communication": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Country": { "Id": 0, "CountryCode": 0, "CountryName": null, "DisplayOrder": 0, "IsActive": false }, "CreateDate": null, "Creator": { "Id": 0, "IsAllowUpdateReview": false, "Password": null, "RoleId": 0, "Username": "" }, "CsExtension": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Customer": { "Id": 0, "Answer": null, "BankInfo": null, "BettingCode": null, "Country": 0, "CountryName": null, "CreatedDate": null, "Creator": null, "CustomerInfo": null, "IsVip": false, "Language": 0, "NickName": null, "Pic1": null, "Pic2": null, "Pic3": null, "Question": null, "RefCode2": null, "RelatedCustomer": null, "Remark": null, "RoleId": 0, "RoleName": "", "Site": null, "SpecialRemark": null, "UserName": "" }, "CustomerInfo": { "BirthDay": null, "Email": " ", "Fax": " ", "FirstName": " ", "LastName": " ", "MSN": " ", "MobilePhone": " ", "Phone": " ", "Skype": " ", "Title": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null} }, "Description": "", "FeedbackDate": "", "IsReviewed": false, "Language": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Priority": null, "Product": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "RelatedCustomer": { "Id": 0, "Answer": null, "BankInfo": null, "BettingCode": null, "Country": 0, "CountryName": null, "CreatedDate": null, "Creator": null, "CustomerInfo": null, "IsVip": false, "Language": 0, "NickName": null, "Pic1": null, "Pic2": null, "Pic3": null, "Question": null, "RefCode2": null, "RelatedCustomer": null, "Remark": null, "RoleId": 0, "RoleName": "", "Site": null, "SpecialRemark": null, "UserName": "" }, "ResolutionTime": null, "Response": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Review": "", "Reviewer": { "Id": 0, "IsAllowUpdateReview": false, "Password": null, "RoleId": 0, "Username": "" }, "Role": { "Id": 7004, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Site": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Solution": " ", "SolvedDate": null, "Status": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "StrFeedbackDate": "", "SubCategory": { "Id": 0, "Child": null, "DisplayOrder": 0, "IsSub": false, "Parent": null, "Type": null, "Value": null }, "Subject": "", "UpdateReviewMode": 0 };
