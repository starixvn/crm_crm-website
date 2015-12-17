"use strict";
/* global sharedScriptManager
*/
/* exported orgArrUser,
trim,
verifyRelatedCustomer,
getRelatedCustomer,
deleteRelatedCust,
getRelatedCustomerLength,
deleteAllInValidCustomers */

var orgLengt = 0;
var orgArrUser = "";
var arrCust = [];
var userIdList = [];
var userList = [];
var renderRelated = function (row) {
    if (!row) {
        return;
    }

    var html;
    if (row && row.Id !== 0) {
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
            if (userIdList.indexOf(rs[i].Id) === -1 || rs[i].Id === 0) {
                userIdList.push(rs[i].Id);
                userList.push(rs[i]);
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
    arrCust = [];
    for (i = 0; i < userList.length; i++) {
        arr.push(renderRelated(userList[i]));
        arrCust.push(userList[i].Id);
    }

    $("#related").html(arr.join(''));
    if ($("#related td").length === 0) {
        $("#ColorGuide").addClass("display-none");
        $("#trRelatedInfo").addClass("display-none");
        $("#related-container").addClass("display-none");
    }
    else {
        $("#ColorGuide").removeClass("display-none");
        $("#trRelatedInfo").removeClass("display-none");
        $("#related-container").removeClass("display-none");
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

var deleteAllInValidCustomers = function () {
    var length = arrCust.length;
    var arrCustPos = [];
    var arrHtmlPos = [];
    var userIdListPos = [];

    $('.invalidcustomers').remove();
    for (var i = 0; i < length; i++) {
        if (parseInt(arrCust[i]) !== 0) {
            arrCustPos.push(arrCust[i]);
            arrHtmlPos.push(userList[i]);
            userIdListPos.push(userIdList[i]);
        }
    }

    userList = [];
    userList = arrHtmlPos;

    arrCust = [];
    arrCust = arrCustPos;

    userIdList = [];
    userIdList = userIdListPos;
};

angular.module('communicationlog-insert-application').directive('relatedCustomerContainer', function () {
    function bindingRelatedCustomerEvent(scope, element) {
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

        relatedTo.attr("disabled", null);
        var typeCust = element.find("#typeCust");
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

        typeCust.removeAttr("disabled");
        element.change(function () {
            scope.$apply(function () {
                scope.updateEditFlag();
            });
        });
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element) {
            bindingRelatedCustomerEvent(scope, element);
        }
    };
});