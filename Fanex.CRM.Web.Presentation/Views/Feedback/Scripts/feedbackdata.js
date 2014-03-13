'use strict';
function FeedbackData(userId, customerId, clientTime) {
    var self = this;

    /* private variables and functions */
    var feedback = undefined;
    var listFeedbacksLasted = [];

    function appendListFeedbacksLasted() {
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'AdminTool/GetFeedbackSubjectTemplateItemsRange',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: '{ "range": "' + range.toString() + '" }',
            success: function (rs) {
                if (rs && rs.length > 0) {
                    self.listFeedbacksLasted = self.listFeedbacksLasted.concat(rs);
                }
            },
            error: function () {
            },
            complete: function () {
            }
        });
    }

    /* public properties */
    this.userId = userId;
    this.customerId = customerId;
    this.clientTime = clientTime;

    this.getFeedback = function () {
        if (this.feedback === undefined) {
            this.feedback = {
                personal: {
                    title: $('#ddltitle'),
                    firstName: $('#firstNameInfo'),
                    lastName: $('#lastNameInfo'),
                    contactNumber1: {
                        phoneNumber: $('#contact1InfoPhone'),
                        countryCode: $('#contact1InfoCountry'),
                        areaCode: $('#contact1InfoArea')
                    },
                    contactNumber2: {
                        phoneNumber: $('#contact2InfoPhone'),
                        countryCode: $('#contact2InfoCountry'),
                        areaCode: $('#contact2InfoArea')
                    },
                    contactNumber3: {
                        phoneNumber: $('#contact3InfoPhone'),
                        countryCode: $('#contact3InfoCountry'),
                        areaCode: $('#contact3InfoArea')
                    },
                    fax: {
                        faxNumber: $('#faxInfoFax'),
                        countryCode: $('#faxInfoCountry'),
                        areaCode: $('#faxInfoArea')
                    },
                    mobile: {
                        phoneNumber: $('#mobileInfoPhone'),
                        countryCode: $('#mobileInfoCountry'),
                        areaCode: $('#mobileInfoArea')
                    },
                    skype: $('#skypeInfo'),
                    email: $('#emailInfo'),
                    msn: $('#msnInfo'),
                    passportNumber: $('#passportInfo'),
                    birthday: $('#birthdayInfo'),
                    site: $('#ddlsite'),
                    country: $('#ddlcountry'),
                    languages: $('#ddllanguage'),
                    bettingCode: $('#bettingCodeInfo'),
                    refCodes2: $('#refCodes2Info'),
                    isVIP: $('#isVIP'),
                    pic1: $('#pic1Info'),
                    pic2: $('#pic2Info'),
                    pic3: $('#pic3Info'),
                    pic4: $('#pic4Info')
                },
                details: {
                    feedbackDate: $('#txtdate'),
                    csExtension: $('#ddlcsextension'),
                    resolutionTime: $('#txtResolutionTime'),
                    communication: $('#ddlcommunication'),
                    category: $('#ddlcategory'),
                    subCategory: $('#ddlsubcategory'),
                    product: $('#ddlproduct'),
                    betType: $('#ddlbettype'),
                    status: $('#ddlstatus'),
                    response: $('#ddlresponse'),
                    isRelatedCustomer: $('#chkisrelated'),
                    subject: $('#txtsubject')
                },
                description: $('#desediter'),
                solution: $('#solediter')
            };

            var identity = $('#txtidentity');
            if (identity) {
                this.feedback.identity = identity;
            }

            var isOverrideCrm = $('#chkIsOverrideCrmInfo');
            if (isOverrideCrm) {
                this.feedback.personal.isOverrideCrm = isOverrideCrm;
            }

            var role = $('#ddlrole');
            if (role) {
                this.feedback.details.role = role;
            }

            var isReviewed = $('#chkisreview');
            var review = $('#revediter');
            if (isReviewed && review) {
                this.feedback.isReviewed = isReviewed;
                this.feedback.review = review;
            }
        }

        return this.feedback;
    };

    this.getListFeedbacksLasted = function () {
        return this.listFeedbacksLasted;
    };

    /* public methods */
    this.getNumOfFeedbackItems = function () {
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'AdminTool/GetNumOfFeedbackSubjectTemplateItem',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: '',
            success: function (rs) {
                self.listFeedbacksLasted = [];
                var numOfRange = Math.round((rs / 100000) + 0.5);
                for (var i = 1; i <= numOfRange; i++) {
                    self.appendListFeedbacksLasted(i);
                }
            },
            error: function () {
            },
            complete: function () {
            }
        });
    };
}

FeedbackData.prototype.clearMessage = function () {
    clearMessage('errmsg');
};

FeedbackData.prototype.showMessage = function (message, type) {
    if (!type) {
        type = 0;
    }

    showAlertMessage('errmsg', message, type);
};

FeedbackData.prototype.blockUI = function () {
    try { parent.BlockUI(); } catch (e) { }
};

FeedbackData.prototype.unblockUI = function () {
    try { parent.UnBlockUI(); } catch (e) { }
};

FeedbackData.prototype.checkSpecialCharacters = function (identity) {
    var iChars = "!`#%^&*()=[]\\\';,/{}|\":<>?~";
    for (var i = 0; i < identity.length; i++) {
        if (iChars.indexOf(identity.charAt(i)) !== -1) {
            this.showMessage('Your Indentity has special characters. ' + iChars, 1);
            return false;
        }
    }
    
    this.clearMessage();
    return true;
};

FeedbackData.prototype.setupDateTimepicker = function (elements) {
    var config = {
        dateFormat: 'M dd, yy',
        timeFormat: 'hh:mm TT',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        ampm: true
    };

    $.each(elements, function (ind, val) {
        val.element.datetimepicker(config);
        val.element.datepicker('setDate', val.value);
    });
}

FeedbackData.prototype.getRelatedCustomers = function () {
    var arrUser = [];
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
        arrUser[i] = this.id;
    });

    if (arrUser.length === 0) {
        return '_';
    }

    return arrUser.join(',');
};

FeedbackData.prototype.validateFeedbackData = function () {
    var feedback = this.getFeedback();
    if (this.customerId <= 0) {
        var identity = feedback.identity.val().trim();
        if (identity.length === 0) {
            this.showMessage('Please input Indentity!', 1);
            return false;
        }

        var isSpecialCharacters = this.checkSpecialCharacters(identity);
        if (!isSpecialCharacters) {
            return false;
        }
    }

    var birthdayText = feedback.personal.birthday.val().trim();
    if (birthdayText.length !== 0) {
        var birthday = new Date(birthdayText);
        if (isNaN(birthday)) {
            this.showMessage('Birthday is invalid!', 1);
            return false;
        }
    }

    var languageIds = feedback.personal.languages.fanexMultiSelect('getItems', true);
    if (!languageIds || languageIds.length === 0) {
        this.showMessage('"Please choose language!', 1);
        return false;
    }

    var details = feedback.details;
    var currentDate = new Date();
    var serverTime = new Date(this.clientTime);
    serverTime.setMinutes(currentDate.getMinutes());
    serverTime.setSeconds(currentDate.getSeconds());

    var feedbackDate = Date.parse(details.feedbackDate.val());
    if (feedbackDate > serverTime) {
        this.showMessage(errMsgFeedbackDateGreatter, 1);
        return false;
    }
    if (feedbackDate >= Date.parse(details.resolutionTime.val())) {
        this.showMessage(errMsgResolutionDateGreatter);
        return false;
    }

    if (details.communication.find('option:selected').text().toLowerCase() === 'n/a') {
        this.showMessage('Please choose communication!', 1);
        return false;
    }

    if (details.product.val() === '0') {
        this.showMessage('Please choose product!', 1);
        return false;
    }

    var relatedCustIds = details.isRelatedCustomer.prop('checked') ? this.getRelatedCustomers() : '';
    if (relatedCustIds === '_') {
        this.showMessage('You must verify the username of related customer before adding this feedback!', 1);
        return false;
    }

    if (details.subject.val().trim().length === 0) {
        this.showMessage('Please input subject!', 1);
        return false;
    }

    if (feedback.description.htmlarea('toHtmlString').trim().length === 0) {
        this.showMessage('Please input description!', 1);
        return false;
    }

    return true;
};

FeedbackData.prototype.getFeedbackData = function () {
    if (!this.validateFeedbackData()) {
        return undefined;
    }

    var feedback = this.getFeedback();
    var personal = feedback.personal;
    var details = feedback.details;

    /* related customers */
    var relatedCustIds = details.isRelatedCustomer.prop('checked') ? this.getRelatedCustomers() : '';

    /* reviewed */
    var reviewer = 0, review = '';
    var isReviewed = feedback.isReviewed ? feedback.isReviewed.prop('checked') : false;
    if (isReviewed) {
        reviewer = this.getUserId();
        review = feedback.review.htmlarea('toHtmlString').trim();
    }

    var feedbackData = {
        Creator: {
            Id: this.userId
        },
        Customer: {
            Id: this.customerId,
            UserName: this.customerId <= 0 ? feedback.identity.val().trim() : ''
        },
        CustomerInfo: {
            Title: {
                Id: personal.title.val().trim()
            },
            FirstName: personal.firstName.val().trim(),
            LastName: personal.lastName.val().trim(),
            ContactNumber1: {
                PhoneNumber: personal.contactNumber1.phoneNumber.val().trim(),
                CountryCode: personal.contactNumber1.countryCode.val().trim(),
                AreaCode: personal.contactNumber1.areaCode.val().trim()
            },
            ContactNumber2: {
                PhoneNumber: personal.contactNumber2.phoneNumber.val().trim(),
                CountryCode: personal.contactNumber2.countryCode.val().trim(),
                AreaCode: personal.contactNumber2.areaCode.val().trim()
            },
            ContactNumber3: {
                PhoneNumber: personal.contactNumber3.phoneNumber.val().trim(),
                CountryCode: personal.contactNumber3.countryCode.val().trim(),
                AreaCode: personal.contactNumber3.areaCode.val().trim()
            },
            Fax: {
                FaxNumber: personal.fax.faxNumber.val().trim(),
                CountryCode: personal.fax.countryCode.val().trim(),
                AreaCode: personal.fax.areaCode.val().trim()
            },
            Mobile: {
                PhoneNumber: personal.mobile.phoneNumber.val().trim(),
                CountryCode: personal.mobile.countryCode.val().trim(),
                AreaCode: personal.mobile.areaCode.val().trim()
            },
            Skype: personal.skype.val().trim(),
            Email: personal.email.val().trim(),
            MSN: personal.msn.val().trim(),
            PassportNumber: personal.passportNumber.val().trim(),
            Birthday: personal.birthday.datepicker('getDate'),
            Site: personal.site.val(),
            Country: personal.country.val(),
            Languages: personal.languages.fanexMultiSelect('getItems', true),
            BettingCode: personal.bettingCode.val().trim(),
            RefCodes2: personal.refCodes2.val().trim(),
            IsVIP: personal.isVIP.prop('checked'),
            PIC1: personal.pic1.val().trim(),
            PIC2: personal.pic2.val().trim(),
            PIC3: personal.pic3.val().trim(),
            PIC4: personal.pic4.val().trim()
        },
        FeedBackDate: details.feedbackDate.val(),
        ResolutionTime: details.resolutionTime.val(),
        CsExtension: {
            Id: details.csExtension.val()
        },
        Communication: {
            Id: details.communication.val()
        },
        Category: {
            Id: details.category.val()
        },
        SubCategory: {
            Id: details.subCategory.val()
        },
        Product: {
            Id: details.product.val()
        },
        Bettype: {
            Id: details.betType.val()
        },
        Status: {
            Id: details.status.val()
        },
        Role: {
            Id: this.customerId <= 0 ? 7004 : details.role.val()
        },
        Response: {
            Id: details.response.val()
        },
        RelatedCustomer: {
            Id: 0,
            UserName: relatedCustIds
        },
        Subject: details.subject.val().trim(),
        Description: feedback.description.htmlarea('toHtmlString').trim(),
        Solution: feedback.solution.htmlarea('toHtmlString').trim(),
        IsReviewed: isReviewed,
        UpdateReviewMode: isReviewed ? 1 : 0,
        Reviewer: {
            Id: reviewer
        },
        Review: review
    };

    return feedbackData;
};

FeedbackData.prototype.setFeedbackData = function (data) {
    var customerInfo = data.CustomerInfo;
    var personal = this.getFeedback().personal;
    var details = this.getFeedback().details;

    personal.firstName.val(customerInfo.FirstName);
    personal.lastName.val(customerInfo.LastName);
    personal.contactNumber1.countryCode.val(customerInfo.ContactNumber1.CountryCode);
    personal.contactNumber1.areaCode.val(customerInfo.ContactNumber1.AreaCode);
    personal.contactNumber1.phoneNumber.val(customerInfo.ContactNumber1.PhoneNumber);
    personal.contactNumber2.countryCode.val(customerInfo.ContactNumber2.CountryCode);
    personal.contactNumber2.areaCode.val(customerInfo.ContactNumber2.AreaCode);
    personal.contactNumber2.phoneNumber.val(customerInfo.ContactNumber2.PhoneNumber);
    personal.contactNumber3.countryCode.val(customerInfo.ContactNumber3.CountryCode);
    personal.contactNumber3.areaCode.val(customerInfo.ContactNumber3.AreaCode);
    personal.contactNumber3.phoneNumber.val(customerInfo.ContactNumber3.PhoneNumber);
    personal.fax.countryCode.val(customerInfo.Fax.CountryCode);
    personal.fax.areaCode.val(customerInfo.Fax.AreaCode);
    personal.fax.faxNumber.val(customerInfo.Fax.FaxNumber);
    personal.mobile.countryCode.val(customerInfo.Mobile.CountryCode);
    personal.mobile.areaCode.val(customerInfo.Mobile.AreaCode);
    personal.mobile.phoneNumber.val(customerInfo.Mobile.PhoneNumber);
    personal.skype.val(customerInfo.Skype);
    personal.email.val(customerInfo.Email);
    personal.msn.val(customerInfo.MSN);
    personal.passportNumber.val(customerInfo.PassportNumber);
    personal.birthday.datepicker('setDate', new Date(customerInfo.BirthDayString));
    personal.bettingCode.val(customerInfo.BettingCode);
    personal.refCodes2.val(customerInfo.RefCodes2);
    personal.isVIP.prop('checked', customerInfo.IsVIP);
    personal.pic1.val(customerInfo.PIC1);
    personal.pic2.val(customerInfo.PIC2);
    personal.pic3.val(customerInfo.PIC3);
    personal.pic4.val(customerInfo.PIC4);

    details.feedbackDate.datepicker('setDate', new Date(data.FeedbackDate));
    details.resolutionTime.datepicker('setDate', new Date(data.ResolutionTime));
    details.csExtension.val(data.CsExtension.Id);
    details.communication.val(data.Communication.Id);
    details.category.val(data.Category.Id);
    details.subCategory.val(data.SubCategory.Id);
    details.product.val(data.Product.Id);
    details.betType.val(data.BetType.Id);
    details.status.val(data.Status.Id);
    details.response.val(data.Response.Id);

    var relatedCustomer = data.RelatedCustomer ? data.RelatedCustomer.UserName : '';
    if (relatedCustomer.length !== 0) {
        details.isRelatedCustomer.prop('checked', true);

        var MyJson = [{ Customer: []}];
        var arrUserName = trimToArrayUserName(relatedCustomer);
        for (var i = 0; i < arrUserName.length; i++) {
            MyJson[i] = { title: arrUserName[i], value: arrUserName[i] };
        }

        $('#holder li').remove();
        $('#txtrelatedcustomer option').remove();
        for (var i = 0; i < MyJson.length; i++) {
            $('#txtrelatedcustomer').trigger('addItem', MyJson[i]);
        }

        $('#btnverify').removeAttr('disabled');
        $('#btnsubmit').removeAttr('disabled');
        $('#btnsubmitbottom').removeAttr('disabled');
    }
    details.subject.val(data.Subject);

    $('#previewboxdes').html(data.Description);
    $('#previewboxsol').html(data.Solution);

    if (data.Reviewer) {
        var revvalue = data.Review;
        var strsoldate = data.SolvedDate ? data.SolvedDate : '';

        $('#lblEditedBy').text('(Reviewed by ' + data.Reviewer.Username + ' on ' + strsoldate + ')');
        $('#previewboxrev').html(revvalue);
    }
};

FeedbackData.prototype.addGuestFeedback = function (successCallback) {
    this.blockUI();
    var feedback = this.getFeedbackData();
    if (feedback === undefined) {
        this.unblockUI();
        return;
    }

    $.ajax({
        type: 'POST',
        url: window.rootUrl + 'Feedback/AddGuestFeedback',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ feedback: feedback }),
        success: function (rs) {
            if (rs.ErrCode === 0) {
                FeedbackData.prototype.showMessage(successMsg);
                if (successCallback && typeof successCallback === 'function') {
                    successCallback();
                }
                //parent.$('#isGuest').prop('checked', true);
                //parent.leftPanelManager.loadLeftPanel($("#txtidentity").val().trim(), true);
            }
            else {
                FeedbackData.prototype.showMessage(rs.ErrMsg, 1);
            }
        },
        complete: function () {
            FeedbackData.prototype.unblockUI();
        }
    });
};

FeedbackData.prototype.initialize = function () {
    /* editor html controls */
    initHTMLArea('editboxdes', 'desediter', '');
    initHTMLArea('editboxsol', 'solediter', '');
    // initHTMLArea('editboxrev', 'revediter', '');

    if (this.customerId <= 0) {
        $('#txtidentity').blur(function () {
            FeedbackData.prototype.checkSpecialCharacters($(this).val().trim());
        });
    }

    /* datetime picker html controls */
    var feedbackdate = new Date(this.clientTime);
    var resolutiondate = new Date(this.clientTime);
    resolutiondate.setMinutes(feedbackdate.getMinutes() + 1);

    this.setupDateTimepicker([
        { element: $('#txtdate'), value: feedbackdate },
        { element: $('#txtResolutionTime'), value: resolutiondate }
    ]);

    /* date picker html controls */
    $('#birthdayInfo').datepicker({
        dateFormat: 'M dd, yy',
        maxDate: new Date(this.clientTime),
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    });

    /* tooltip html controls */
    $('#btnRelatedHelp').tipsy({
        gravity: 'nw',
        title: function () {
            return inputMultipleRelatedNote;
        }
    });

    /* auto-complete html controls */
    $('#txtrelatedcustomer').fcbkcomplete({
        /* jshint camelcase: false */
        json_url: window.rootUrl + 'Customer/RelatedCustomerAutoSuggestion',
        addontab: true,
        maxitems: 30,
        input_min_size: 1,
        height: 10,
        cache: false,
        newel: true
    });
    var typeCust = $('#typeCust');
    typeCust.attr('tabindex', 46);

    /* add input listen for auto complete Subject name input text box */
    $('#txtsubject').keyup(function (e) {
        var keyCode = e.keyCode;
        if (keyCode === 37 || keyCode === 38 || keyCode === 39 || keyCode === 40) {
            return true;
        }
        else if (keyCode !== 13) {
            var $this = $(this);
            var rows = [];
            if ($this.val() !== '') {
                rows = jlinq.from(listFeedbacksLasted).starts('Subject', $this.val()).sort('Subject').select();
                rows = jlinq.from(rows).skip(0).take(15);
                if (rows.length > 0) {
                    $this.autoComplete(rows, 'Subject');
                }
                else {
                    $this.removeAutoComplete();
                }
            }
            else {
                $this.removeAutoComplete();
            }
        }
    });
};