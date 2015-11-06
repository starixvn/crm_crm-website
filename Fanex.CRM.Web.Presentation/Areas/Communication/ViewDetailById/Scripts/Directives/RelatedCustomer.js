"use strict";
/* global sharedScriptManager */
/* exported orgArrUser,
orgLengt,
trim,
verifyRelatedCustomer,
getRelatedCustomer,
deleteRelatedCust,
getRelatedCustomerLength,
isOldRelatedCustomer */

var orgLengt = 0;
var orgArrUser = "";
var arrCust = [];
var userIdList = [];
var userList = [];
var renderRelated = function (row) {
    $('#related td').each(function (i, td) {
        /* jshint unused: false */
        arrCust[i] = this.id;
    });

    var html;
    if (row.Id !== 0) {
        if (row.Id > 0) {
            html = '<tr>\
                        <td id =' + row.Id + ' value= ' + row.Name + ' >\
                                <a rel="friend" class="txt-customer">\
                                    <span onclick="deleteRelatedCust(' + row.Id + ');" class="icon-del"></span>' + row.Name + '</a>\
                         </td>\
                     </tr>';
        }
        else {
            html = '<tr>\
                        <td id =' + row.Id + ' value= ' + row.Name + ' >\
                                <a rel="friend" class="txt-guest">\
                                    <span onclick="deleteRelatedCust(' + row.Id + ');" class="icon-del"></span>' + row.Name + '</a>\
                        </td>\
                    </tr>';
        }
    }
    else {
        html = '<tr class="invalidcustomers">\
                    <td id =' + row.Id + ' value= ' + row.Name + ' >\
                            <a rel="friend" class="txt-invalid">\
                                <span onclick="deleteAllInValidCustomers();" class="icon-del"></span>' + row.Name + '</a>\
                    </td>\
                </tr>';
    }

    return html;
};
var completedVerifyRelatedCustomer = function (rs) {
    var i;
    if (rs != null && rs.length > 0) {
        var userId = '';
        for (i = 0; i < rs.length; i++) {
            if (parseInt(rs[i].Id) !== 0) {
                if (userIdList.indexOf(rs[i].Id) === -1) {
                    userIdList.push(rs[i].Id);
                    userList.push(rs[i]);
                }
            }
        }

        userId += userIdList.join(",");
    }

    $('#txtrelatedcustomer option').each(function (i, option) {
        $(option).remove();
    });

    $('#holder li').each(function (i, option) {
        if (this.id !== 'txtrelatedcustomer_annoninput') {
            $(option).remove();
        }
    });
    var arr = [];
    for (i = 0; i < userList.length; i++) {
        arr.push(renderRelated(userList[i]));
        arrCust.push(userList[i].Id);
    }

    $("#related").html(arr.join(''));
    if ($("#related td").length === 0) {
        $("#ColorGuide").addClass("display-none");
        $("#trRelatedInfo").addClass("display-none");
    }
    else {
        $("#ColorGuide").removeClass("display-none");
        $("#trRelatedInfo").removeClass("display-none");
    }
};

function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

var verifyRelatedCustomer = function (scope) {
    var arrUser = [];
    var userName = "";
    $('#txtrelatedcustomer option').each(function (i, option) {
        arrUser[i] = $(option).text();
    });

    userName = arrUser.join(",");
    if (userName !== "") {
        scope.BlockUI();
        sharedScriptManager.CallAjax(self,
            window.rootUrl + "CustomerSetting/VerifyUsername",
            { UserName: userName },
            completedVerifyRelatedCustomer,
            null,
            scope.unblockUI);
    }
};

var getRelatedCustomer = function () {
    var arrUser = [];
    var userName = "";
    $('#related td').each(function (i, td) {
        /* jshint unused: false */
        arrUser[i] = this.id;
    });
    userName = arrUser.join(",");
    return userName;
};
var deleteRelatedCust = function (custId) {
    $("#" + custId).parent().remove();
    userList.splice(userIdList.indexOf(custId), 1);
    userIdList.splice(userIdList.indexOf(custId), 1);
};
var getRelatedCustomerLength = function () {
    return ($(".related-customer-container #holder > li").length - 1) +
           ($(".related-customer-container #typeCust").val() !== "" ? 1 : 0);
};

var isOldRelatedCustomer = function (oldRelatedCustomer) {
    var typeCustElement = $(".related-customer-container #typeCust").val() === "" ?
        ('<li class="bit-input" id="txtrelatedcustomer_annoninput">' +
        '<input style="width: auto;" id="typeCust" class="maininput" size="1" autocomplete="off" type="text">' +
        '</li>') :
        '';

    var initTypeCustElement = '<li class="bit-input" id="txtrelatedcustomer_annoninput">' +
        '<input id="typeCust" class="maininput" size="1" autocomplete="off" type="text">' +
        '</li>';
    var newRelatedCustomer = $(".related-customer-container #holder").html().replace(/class="bit-box deleted"/g, 'class="bit-box"');
    if (oldRelatedCustomer.indexOf(initTypeCustElement, oldRelatedCustomer.length - initTypeCustElement.length) !== -1) {
        oldRelatedCustomer = oldRelatedCustomer.substring(0, oldRelatedCustomer.length - initTypeCustElement.length);
    }

    if (newRelatedCustomer.indexOf(initTypeCustElement, newRelatedCustomer.length - initTypeCustElement.length) !== -1) {
        newRelatedCustomer = newRelatedCustomer.substring(0, newRelatedCustomer.length - initTypeCustElement.length);
    }

    if (typeCustElement !== '' &&
        oldRelatedCustomer.indexOf(typeCustElement, oldRelatedCustomer.length - typeCustElement.length) === -1) {
        oldRelatedCustomer += typeCustElement;
    }

    if (newRelatedCustomer !== '' &&
        newRelatedCustomer.indexOf(typeCustElement, newRelatedCustomer.length - typeCustElement.length) === -1) {
        newRelatedCustomer += typeCustElement;
    }

    oldRelatedCustomer.replace(/class="bit-box deleted"/g, 'class="bit-box"');
    return oldRelatedCustomer === newRelatedCustomer;
};

angular.module('communicationlog-viewdetailbyid-application').directive('relatedCustomerContainer', function () {
    function bindingRelatedCustomerEvent(scope, element) {
        /* jshint unused: vars */
        var relatedTo = element.find("#txtrelatedcustomer");
        relatedTo.fcbkcomplete({
            /* jshint camelcase: false */
            json_url: window.rootUrl + 'CustomerSetting/RelatedCustomerAutoSuggestion',
            addontab: true,
            maxitems: parent.crmConstants.Enums.RelatedCustomerSetting.MaxItems,
            input_min_size: 1,
            height: 10,
            cache: false,
            newel: true
        });

        relatedTo.attr("disabled", "disabled");
        if (scope.communicationLog.RelatedCustomers && scope.communicationLog.RelatedCustomers !== '') {
            var relatedCustomers = [{ "Customer": []}];
            var arrUserName = scope.communicationLog.RelatedCustomers.split(",");
            var i;
            for (i = 0; i < arrUserName.length; i++) {
                relatedCustomers[i] = {
                    title: $.trim(arrUserName[i]),
                    value: $.trim(arrUserName[i])
                };
            }

            $('#holder li').remove();
            $('#txtrelatedcustomer option').remove();
            for (i = 0; i < relatedCustomers.length; i++) {
                $("#txtrelatedcustomer").trigger("addItem", relatedCustomers[i]);
            }

            $("#btnverify").removeAttr("disabled");
            $("#btnsubmit").removeAttr("disabled");
            $("#btnsubmitbottom").removeAttr("disabled");
            orgLengt = relatedCustomers.length;
        }

        element.find(".bit-box .closebutton").remove();
        element.find("#typeCust").attr("disabled", "disabled");
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element) {
            bindingRelatedCustomerEvent(scope, element);
        }
    };
});
