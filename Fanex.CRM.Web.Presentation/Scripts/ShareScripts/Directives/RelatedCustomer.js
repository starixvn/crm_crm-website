"use strict";
/* global directiveManager: true,
sharedScriptManager */
/* jshint unused: false,
camelcase: false,
newcap: false */
/* Must refer to: 
~/Scripts/common.js
*/
/*exported orgLengt, orgArrUser, trim */
var orgLengt = 0;
var orgArrUser = "";
var arrCust = [];
var userIdList = [];
var userList = [];
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

var directiveManager = directiveManager || {};
directiveManager.registerRelatedCustomerDirective = function (angularApplication, registerParams) {
    var isInit = true;
    var selfOptions = null;
    function blockUI() {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    }
    function unblockUI() {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    }
    function InitDirective(scope, element, attrs, options) {
        var picker = element.find("#" + registerParams.PickerId);
        picker.fcbkcomplete({
            json_url: options.SuggestLink,
            addontab: true,
            maxitems: options.MaxItems,
            input_min_size: 1,
            height: 10,
            cache: false,
            newel: true
        });

        picker = element.find("#" + registerParams.PickerId);
        picker.change(function () {
            scope.updateEditProcess(getRelatedCustomerLength(element));
            scope.$apply();
        });
    }

    function isOptionReady(options) {
        return options !== null &&
            options !== undefined;
    }

    function UpdateDirective(scope, element, attrs) {
        var options = scope[registerParams.OptionName];
        if (!isOptionReady(options)) {
            return;
        }

        if (isInit) {
            InitDirective(scope, element, attrs, options);
            isInit = false;
        }

        if (!isInit && options.Action === "checkIsRelated") {
            checkIsRelated(scope, element, attrs, options);
        }

        if (!isInit && options.IsUpdate && options.Action === "verifyUserName") {
            verifyUserName(scope, element, attrs, options);
        }

        if (options.CompleteUpdate &&
            typeof (options.CompleteUpdate) === "function") {
            options.CompleteUpdate();
        }
    }
    function checkIsRelated(scope, element, attrs, options) {
        if (options.HasRelated) {
            $("#typeCust").removeProp("disabled");

            var arrUser;
            $('#txtrelatedcustomer').each(function () {
                arrUser = $(this).val();
            });

            if (arrUser != null && arrUser.length > 0) {
                $("#btnverify").removeProp("disabled");
            }
            $("#lblverify").removeClass("hidden");
            if ($("#related").text().trim().length > 0) {
                $("#ColorGuide").removeClass("hidden");
                $("#trRelatedInfo").removeClass("hidden");
            }
        }
        else {
            $("#typeCust").prop("disabled", true);
            $("#btnverify").prop("disabled", true);
            $("#lblverify").addClass("hidden");
            $("#ColorGuide").addClass("hidden");
            $("#trRelatedInfo").addClass("hidden");
        }
    }
    function verifyUserName(scope, element, attrs, options) {
        var arrUser = [];
        var userName = "";
        $('#txtrelatedcustomer option').each(function (i, option) {
            arrUser[i] = $(option).text();
        });

        userName = arrUser.join(",");
        selfOptions = options;
        if (userName !== "") {
            blockUI();
            sharedScriptManager.CallAjax(null,
                options.VerifyUsernameLink,
                { Username: userName },
                completedVerifyRelatedCustomer,
                null,
                unblockUI);
        }
    }

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

        $('#txtrelatedcustomer option').remove();

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

        $("#related").find(".icon-del-single").click(function () {
            deleteRelatedCust(parseInt($(this).attr("data-customer-id")));
            updateScopeRelatedCustomer();
        });
        $("#related").find(".icon-del-all-invalid").click(function () {
            deleteAllInValidCustomers();
            updateScopeRelatedCustomer();
        });

        updateScopeRelatedCustomer();
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
    var updateScopeRelatedCustomer = function () {
        if (selfOptions.OnChangeEvent &&
            typeof (selfOptions.OnChangeEvent) === "function") {
            selfOptions.OnChangeEvent(getRelatedCustomer());
        }
    };

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
                                    <span data-customer-id="' + row.Id + '" class="icon-del icon-del-single"></span>' + row.Name + '</a>\
                         </td>\
                     </tr>';
            }
            else {
                html = '<tr>\
                        <td id =' + row.Id + ' value= ' + row.Name + ' >\
                                <a rel="friend" class="txt-guest">\
                                    <span data-customer-id="' + row.Id + '" class="icon-del icon-del-single"></span>' + row.Name + '</a>\
                        </td>\
                    </tr>';
            }
        }
        else {
            html = '<tr class="invalidcustomers">\
                        <td id =' + row.Id + ' value= ' + row.Name + ' >\
                                <a rel="friend" class="txt-invalid">\
                                    <span data-customer-id="' + row.Id + '" class="icon-del icon-del-all-invalid"></span>' + row.Name + '</a>\
                        </td>\
                    </tr>';
        }

        return html;
    };

    var deleteRelatedCust = function (custId) {
        $("#" + custId).parent().remove();
        userList.splice(userIdList.indexOf(custId), 1);
        userIdList.splice(userIdList.indexOf(custId), 1);
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
    var getRelatedCustomer = function () {
        var arrUser = [];
        var userName = "";
        $('#related td').each(function (i, td) {
            arrUser[i] = this.id;
        });
        userName = arrUser.join(",");
        return userName;
    };
    var getRelatedCustomerLength = function (element) {
        return (element.find("#holder > li").length - 1) +
           (element.find("typeCust").val() !== "" ? 1 : 0);
    };
    angularApplication.directive(registerParams.PlugInName, function () {
        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    UpdateDirective(scope, element, attrs);
                });
            }
        };
    });
};