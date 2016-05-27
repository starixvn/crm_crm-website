(function () {
    "use strict";
    var NUM_OF_COLUMNS = 3;

    directiveManager = directiveManager || {};
    directiveManager.registerRelatedCustomerResult = function (angularApplication) {
        angularApplication.directive('relatedcustomerresult', function () {
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
                    scope.relatedCustomers = customers.filter(function (value) {
                        return value.Id !== custId;
                    });

                    scope.$apply();
                });
                table.find('.icon-del-all-invalid').unbind('click').bind('click', function () {
                    $('.invalidcustomers').remove();
                    scope.relatedCustomers = customers.filter(function (value) {
                        return value.Id !== 0;
                    });

                    scope.$apply();
                });
            };

            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attrs) {
                    /* jshint unused: false */
                    scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                        renderRelatedCustomers(scope, newVal);
                    });
                }
            };
        });
    }
})();