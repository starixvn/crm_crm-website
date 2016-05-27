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
                if (customer && customer.Id !== 0) {
                    if (customer.Id > 0) {
                        html = '<td id="' + customer.Id + '" value="' + customer.Name + '">\
                                <a rel="friend" class="txt-customer">\
                                    <span data-customer-id="' + customer.Id + '" class="icon-del icon-del-single"></span>' + customer.Name + '</a>\
                         </td>';
                    }
                    else {
                        html = '<td id="' + customer.Id + '" value="' + customer.Name + '">\
                                <a rel="friend" class="txt-guest">\
                                    <span data-customer-id="' + customer.Id + '" class="icon-del icon-del-single"></span>' + customer.Name + '</a>\
                        </td>';
                    }
                }
                else {
                    html = '<td class="invalidcustomers" id="' + customer.Id + '" value="' + customer.Name + '">\
                                <a rel="friend" class="txt-invalid">\
                                    <span data-customer-id="' + customer.Id + '" class="icon-del icon-del-all-invalid"></span>' + customer.Name + '</a>\
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
                        if (customers[i].Id === customers[j].Id) {
                            customers.splice(j--, 1);
                        }
                    }
                }

                return customers;
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
                        username = username.trim().replace(/,\s+/g, ",").replace(/^,+|,+$/g, "").replace(/,{2,}/g, ",");
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
                            attrs.ngModel = '';

                            renderRelatedCustomers(scope, scope.relatedCustomerResultDirective.relatedCustomers);
                            $uiBlockerService.unblockUI();
                        },
                        function () {
                            $uiBlockerService.unblockUI();
                        });
                    };
                }
            };
        }]);
    }
})();