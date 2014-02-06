editLogApplication.directive('relatedCustomerContainer', function () {

    function bindingRelatedCustomerEvent(scope, element, attrs) {
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

        relatedTo.attr("disabled", null);
        var typeCust = element.find("#typeCust");
        if (scope.communicationLog.RelatedCustomers && scope.communicationLog.RelatedCustomers != '') {
            var relatedCustomers = [{ "Customer": []}];
            var arrUserName = scope.communicationLog.RelatedCustomers.split(",");
            for (var i = 0; i < arrUserName.length; i++) {
                relatedCustomers[i] = {
                    title: $.trim(arrUserName[i]),
                    value: $.trim(arrUserName[i])
                };
            }

            $('#holder li').remove();
            $('#txtrelatedcustomer option').remove();
            for (var i = 0; i < relatedCustomers.length; i++) {
                $("#txtrelatedcustomer").trigger("addItem", relatedCustomers[i]);
            }

            $("#btnverify").removeAttr("disabled");
            $("#btnsubmit").removeAttr("disabled");
            $("#btnsubmitbottom").removeAttr("disabled");
            orgLengt = relatedCustomers.length;
        }

        typeCust.removeAttr("disabled");
        element.change(function () {
            scope.isEditingRelatedCustomer = true;
        });
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            bindingRelatedCustomerEvent(scope, element, attrs);
        }
    };
});

var orgLengt = 0;
var orgArrUser = "";
var arrCust = [];

function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
};
verifyRelatedCustomer = function (scope) {
    var arrUser = [];
    var userName = "";
    var i;
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
completedVerifyRelatedCustomer = function (rs) {
    if (rs != null && rs.length > 0) {
        var userId = '';
        var userIdList = [];
        for (i = 0; i < rs.length; i++) {
            if (parseInt(rs[i].CustId) !== 0) {
                userIdList.push(rs[i].CustId);
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
    for (i = 0; i < rs.length; i++) {
        arr.push(renderRelated(rs[i]));
        arrCust.push(rs[i].CustId);
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
checkDuplicateRelated = function () {
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
checkContainsValue = function (a, e) {
    for (var j = 0; j < a.length; j++) {
        if ($.trim(a[j].toLowerCase()) === $.trim(e.toLowerCase())) {
            return true;
        }
    }

    return false;
};
renderRelated = function (row) {
    var userName = "";
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
        arrCust[i] = this.id;
    });

    if (arrCust != null && arrCust.length > 0) {
        for (var i = 0; i < arrCust.length; i++) {
            if (row.CustId === arrCust[i] && row.CustId !== 0) {
                userName = userName + arrCust[i];
                return null;
            }
        }
    }

    var html;
    if (row.CustId !== 0) {
        if (row.CustId > 0) {
            html = '<tr>\
                        <td id =' + row.CustId + ' value= ' + row.UserName + ' style="border: 0px;">\
                            <div>\
                                <a rel="friend" onclick="DeleteRelatedCust(' + row.CustId + ');">[x]</a>\
                                &nbsp;&nbsp;\
                                <font style="color:green;">' + row.UserName + '</font>\
                             </div>\
                         </td>\
                     </tr>';
        }
        else {
            html = '<tr>\
                        <td id =' + row.CustId + ' value= ' + row.UserName + ' style="border: 0px;">\
                            <div>\
                                <a rel="friend" onclick="DeleteRelatedCust(' + row.CustId + ');">[x]</a>\
                                &nbsp;&nbsp;\
                                <font style="color:#0066CC;">' + row.UserName + '</font>\
                            </div>\
                        </td>\
                    </tr>';
        }
    }
    else {
        html = '<tr class="invalidcustomers">\
                    <td id =' + row.CustId + ' value= ' + row.UserName + ' style="border: 0px;">\
                        <div>\
                            <a rel="friend" onclick="deleteAllInValidCustomers();">[x]</a>\
                            &nbsp;&nbsp;\
                            <font style="color:red;">' + row.UserName + '</font>\
                        </div>\
                    </td>\
                </tr>';
    }

    return html;
};
DeleteRelatedCust = function (custId) {
    $("#" + custId).parent().remove();
};
getRelatedCustomer = function () {
    var arrUser = [];
    var userName = "";
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
        arrUser[i] = this.id;
    });
    userName = arrUser.join(",");
    return userName;
};