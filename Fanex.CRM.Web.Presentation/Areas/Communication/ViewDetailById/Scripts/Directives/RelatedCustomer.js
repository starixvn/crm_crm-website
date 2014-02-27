"use strict";
/* global editLogApplication,
sharedScriptManager */
/* exported orgArrUser, 
trim,
verifyRelatedCustomer,
checkDuplicateRelated,
getRelatedCustomer,
deleteRelatedCust,
getRelatedCustomerLength,
isOldRelatedCustomer,
verifyRelatedCustomerForUpdate */
var orgLengt = 0;
var orgArrUser = "";
var arrCust = [];
var userIdList = [];
var userList = [];
var renderRelated = function (row) {
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
        arrCust[i] = this.id;
    });

    var html;
    if (row.CustId !== 0) {
        if (row.CustId > 0) {
            html = '<tr>\
                        <td id =' + row.CustId + ' value= ' + row.UserName + ' >\
                                <a rel="friend" class="txt-customer">\
                                    <span onclick="deleteRelatedCust(' + row.CustId + ');" class="icon-del"></span>' + row.UserName + '</a>\
                         </td>\
                     </tr>';
        }
        else {
            html = '<tr>\
                        <td id =' + row.CustId + ' value= ' + row.UserName + ' >\
                                <a rel="friend" class="txt-guest">\
                                    <span onclick="deleteRelatedCust(' + row.CustId + ');" class="icon-del"></span>' + row.UserName + '</a>\
                        </td>\
                    </tr>';
        }
    }
    else {
        html = '<tr class="invalidcustomers">\
                    <td id =' + row.CustId + ' value= ' + row.UserName + ' >\
                            <a rel="friend" class="txt-invalid">\
                                <span onclick="deleteAllInValidCustomers();" class="icon-del"></span>' + row.UserName + '</a>\
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
            if (parseInt(rs[i].CustId) !== 0) {
                if (userIdList.indexOf(rs[i].CustId) === -1) {
                    userIdList.push(rs[i].CustId);
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
        arrCust.push(userList[i].CustId);
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

viewDetailByIdApplication.directive('relatedCustomerContainer', function () {

    function bindingRelatedCustomerEvent(scope, element, attrs) {
        /* jshint unused: vars */
        var relatedTo = element.find("#txtrelatedcustomer");
        relatedTo.fcbkcomplete({
            /* jshint camelcase: false */
            json_url: window.rootUrl + 'Customer/RelatedCustomerAutoSuggestion',
            addontab: true,
            maxitems: 30,
            input_min_size: 1,
            height: 10,
            cache: false,
            newel: true
        });

        relatedTo.attr("disabled", "disabled");
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
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            bindingRelatedCustomerEvent(scope, element, attrs);
        }
    };
});

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
            window.rootUrl + "Customer/VerifyUserName",
            { UserName: userName },
            completedVerifyRelatedCustomer,
            null,
            scope.unblockUI);
    }
};
var verifyRelatedCustomerForUpdate = function (scope) {
    var arrUser = [];
    var userName = "";
    $('#txtrelatedcustomer option').each(function (i, option) {
        arrUser[i] = $(option).text();
    });

    userName = arrUser.join(",");
    if (userName !== "") {
        scope.BlockUI();
        sharedScriptManager.CallAjax(self,
            window.rootUrl + "Customer/VerifyUserName",
            { UserName: userName },
            completedVerifyRelatedCustomer,
            null,
            scope.updateLog);
    }
};
var checkContainsValue = function (a, e) {
    for (var j = 0; j < a.length; j++) {
        if ($.trim(a[j].toLowerCase()) === $.trim(e.toLowerCase())) {
            return true;
        }
    }

    return false;
};
var checkDuplicateRelated = function () {
    var arrUser = [];
    var userName = "";
    $('#txtrelatedcustomer option').each(function (i, option) {
        arrUser[i] = $(option).text();
    });

    userName = arrUser.join(",");
    var relatedVal = arrUser;
    var dups = [];
    var items = [];
    if (relatedVal) {
        for (var j = 0; j < relatedVal.length; j++) {
            if (!checkContainsValue(items, relatedVal[j])) {
                items.push(relatedVal[j]);
            }
            else {
                dups.push(relatedVal[j]);
            }
        }

        return dups;
    }
};
var getRelatedCustomer = function () {
    var arrUser = [];
    var userName = "";
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
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