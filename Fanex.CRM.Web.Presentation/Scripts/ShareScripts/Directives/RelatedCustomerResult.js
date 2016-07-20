(function () {
    "use strict";
    var NUM_OF_COLUMNS = 3;

    directiveManager = directiveManager || {};
    directiveManager.registerRelatedCustomerResult = function (angularApplication) {
        angularApplication.directive('relatedcustomerresult', ['$http', '$q', '$uiBlockerService',
         function ($http, $q, $uiBlockerService) {
            var renderHtmlCell = function (customer) {
                if (!customer) {
                    return;
                }

                var html;
                var customerName = escapeHtml(customer.Name);
                if (customer && customer.Id !== 0) {
                    if (customer.Id > 0) {
                        html = '<td id="' + customer.Id + '" value="' + customerName + '">\
                                <div rel="friend" class="txt-customer">\
                                    <div data-customer-id="' + customer.Id + '" class="icon-del icon-del-single"></div>\
                                    <div class="customer-name">' + customerName + '</div>\
                                </div>\
                         </td>';
                    }
                    else {
                        html = '<td id="' + customer.Id + '" value="' + customerName + '">\
                                <div rel="friend" class="txt-guest">\
                                    <div data-customer-id="' + customer.Id + '" class="icon-del icon-del-single"></div>\
                                    <div class="customer-name">' + customerName + '</div>\
                                </div>\
                        </td>';
                    }
                }
                else {
                    html = '<td class="invalidcustomers" id="' + customer.Id + '" value="' + customerName + '">\
                                <div rel="friend" class="txt-invalid">\
                                    <div data-customer-id="' + customer.Id + '" class="icon-del icon-del-all-invalid"></div>\
                                    <div class="customer-name">' + customerName + '</div>\
                                </div>\
                        </td>';
                }

                return html;
            };

            var renderRelatedCustomers = function (scope, customers) {

                if (!customers || customers.length === 0) {
                    return;
                }

                var rows = [];

                var columnIndex = 0;
                var row = "";
                angular.forEach(customers, function (customer, index) {
                    /* jshint unused: false */
                    var cell = renderHtmlCell(customer);
                    if (columnIndex < NUM_OF_COLUMNS) {
                        row += cell;
                        columnIndex++;
                    }
                    
                    if (columnIndex == NUM_OF_COLUMNS){
                        rows.push("<tr>" + row + "</tr>");
                        row = "";
                        columnIndex = 0;
                    }
                });

                if (row != "") {
                    rows.push("<tr>" + row + "</tr>");
                }

                var table = $('#related');
                table.html(rows.join(''));
                table.find('.icon-del-single').unbind('click').bind('click', function () {
                    var custId = parseInt($(this).data('customer-id'));
                    $('#' + custId).parent().remove();
                    scope.relatedCustomerResultDirective.relatedCustomers = customers.filter(function (value) {
                        return value.Id !== custId;
                    });

                    scope.$apply();
                });
                table.find('.icon-del-all-invalid').unbind('click').bind('click', function () {
                    $('.invalidcustomers').remove();
                    scope.relatedCustomerResultDirective.relatedCustomers = customers.filter(function (value) {
                        return value.Id !== 0;
                    });

                    scope.$apply();
                });
            };
            function combineCustomers (currentCustomers, newCustomers) {
                var customers = (currentCustomers || []).concat(newCustomers);
                // unique customer.
                for (var i = 0; i < customers.length; ++i) {
                    for (var j = i + 1; j < customers.length; ++j) {
                        if (isDuplicatedCustomer(customers[i], customers[j])) {
                            customers.splice(j--, 1);
                        }
                    }
                }

                return customers;
            }

            function isDuplicatedCustomer(source, target) {
                return source.Id === target.Id && source.Name === target.Name;
            }

            function escapeHtml(text) {
                var entityMap = {
                    "&": "&amp;",
                    "<": "&lt;",
                    ">": "&gt;",
                    '"': '&quot;',
                    "'": '&#39;',
                    "/": '&#x2F;'
                };

                return text.replace(/[&<>"'\/]/g, function (s) {
                    return entityMap[s];
                });
            }

            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attrs) {
                    /* jshint unused: false */
                    scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                        renderRelatedCustomers(scope, newVal);
                    });
                    scope.relatedCustomerResultDirective = {};
                    scope.relatedCustomerResultDirective.verifyUsernames = function (username) {
                        if (username === '') {
                            return;
                        }

                        $uiBlockerService.blockUI();
                        var url = window.rootUrl + 'CustomerSetting/VerifyUsername';
                        var promise = sharedScriptManager.registerAngularService($http, $q, url, { Username: username });
                        promise.then(function (data) {
                            var customers = [];
                            angular.forEach(data.Result, function (customer, index) {
                                /* jshint unused: false */
                                customers.push({ Id: customer.Id, Name: customer.Name });
                            });

                            scope.relatedCustomerResultDirective.relatedCustomers = combineCustomers(scope.relatedCustomerResultDirective.relatedCustomers, customers);
                            scope.customersInput = '';
                            $uiBlockerService.unblockUI();
                        },
                        function () {
                            $uiBlockerService.unblockUI();
                        });
                    };

                    scope.relatedCustomerResultDirective.normalizeUsername = function (username) {
                        return username.trim().replace(/,\s+/g, ",").replace(/^,+|,+$/g, "").replace(/,{2,}/g, ",");
                    }

                    scope.relatedCustomerResultDirective.isValidUsername = function (username) {
                        var usernameList = username.split(",").filter(function (value) {
                            return value.length > 50;
                        });

                        return usernameList.length == 0;
                    };

                    scope.relatedCustomerResultDirective.getRelatedCustomerId = function () {
                        var IdString = "";
                        var relatedCustomers = scope.relatedCustomerResultDirective.relatedCustomers || [];
                        relatedCustomers = relatedCustomers.filter(function (value) {
                            return value.Id !== 0;
                        });
                        if (relatedCustomers.length > 0) {
                            var relatedCustomerIds = [];
                            angular.forEach(relatedCustomers, function (customer) {
                                relatedCustomerIds.push(customer.Id);
                            });

                            IdString = relatedCustomerIds.join(',');
                        }

                        return IdString;
                    };

                    scope.relatedCustomerResultDirective.loadRelatedCustomers = function (relatedCustomers) {                        
                        renderRelatedCustomers(scope, relatedCustomers);
                        scope.relatedCustomerResultDirective.relatedCustomers = relatedCustomers;
                    };
                }
            };
        }]);
    }
})();