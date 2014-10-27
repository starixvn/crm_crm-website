'use strict';
/* global updateCustomerFeedbackApplication */
/* jshint unused: vars */
updateCustomerFeedbackApplication.directive('relatedcustomerresult', function () {
    var renderRow = function (customer) {
        if (!customer) {
            return;
        }

        var html;
        if (customer && customer.Id !== 0) {
            if (customer.Id > 0) {
                html = '<tr>\
                        <td id="' + customer.Id + '" value="' + customer.Name + '">\
                                <a rel="friend" class="txt-customer">\
                                    <span data-customer-id="' + customer.Id + '" class="icon-del icon-del-single"></span>' + customer.Name + '</a>\
                         </td>\
                     </tr>';
            }
            else {
                html = '<tr>\
                        <td id="' + customer.Id + '" value="' + customer.Name + '">\
                                <a rel="friend" class="txt-guest">\
                                    <span data-customer-id="' + customer.Id + '" class="icon-del icon-del-single"></span>' + customer.Name + '</a>\
                        </td>\
                    </tr>';
            }
        }
        else {
            html = '<tr class="invalidcustomers">\
                        <td id="' + customer.Id + '" value="' + customer.Name + '">\
                                <a rel="friend" class="txt-invalid">\
                                    <span data-customer-id="' + customer.Id + '" class="icon-del icon-del-all-invalid"></span>' + customer.Name + '</a>\
                        </td>\
                    </tr>';
        }

        return html;
    };

    var renderRelatedCustomers = function (scope, customers) {
        if (!customers || customers.length === 0) {
            return;
        }

        $('#txtrelatedcustomer option').remove();
        $('#holder li.bit-box').remove();

        var rows = [];
        angular.forEach(customers, function (customer, index) {
            rows.push(renderRow(customer));
        });

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
            scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                renderRelatedCustomers(scope, newVal);
            });
        }
    };
});