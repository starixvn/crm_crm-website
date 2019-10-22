import { escapeHtml } from '../../../shared/common-functions';

(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').directive('relatedCustomerGrid', relatedCustomerGrid);
    relatedCustomerGrid.$inject = [];

    function relatedCustomerGrid() {
        const NUM_OF_COLUMNS = 4;
        const renderHtmlCell = function (customer) {
            if (!customer) {
                return;
            }

            let html;
            const customerName = escapeHtml(customer.Name);
            if (customer && customer.Id !== 0) {
                if (customer.Id > 0) {
                    html = '<td>\
                                <div class="txt-customer">\
                                    <div data-customer-id="' + customer.Id + '" class="fa fa-times"></div>\
                                    <div class="customer-name">' + customerName + '</div>\
                                </div>\
                         </td>';
                }
                else {
                    html = '<td>\
                                <div class="txt-guest">\
                                    <div data-customer-id="' + customer.Id + '" class="fa fa-times"></div>\
                                    <div class="customer-name">' + customerName + '</div>\
                                </div>\
                        </td>';
                }
            } else {
                html = '<td>\
                                <div class="txt-invalid">\
                                    <div data-customer-id="' + customer.Id + '" class="fa fa-times"></div>\
                                    <div class="customer-name">' + customerName + '</div>\
                                </div>\
                        </td>';
            }

            return html;
        };
        const renderRelatedCustomers = function (scope, element, customers) {
            if (!customers || customers.length === 0) {
                return;
            }

            let rows = [],
                columnIndex = 0,
                row = '';
            angular.forEach(customers, function (customer) {
                var cell = renderHtmlCell(customer);
                if (columnIndex < NUM_OF_COLUMNS) {
                    row += cell;
                    columnIndex++;
                }

                if (columnIndex === NUM_OF_COLUMNS) {
                    rows.push('<tr>' + row + '</tr>');
                    row = '';
                    columnIndex = 0;
                }
            });

            if (row !== '') {
                rows.push('<tr>' + row + '</tr>');
            }

            $(element).html('<table>' + rows.join('') + '</table>');
            $('.fa-times', $(element)).off('click').on('click', function () {
                const customerId = parseInt($(this).data('customer-id'), 10);
                scope.onDeleteCustomer(customerId);
            });
        };

        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                customers: '=ngModel',
                onDeleteCustomer: '='
            },
            link: function (scope, element) {
                scope.$watch('customers', function (newVal, oldVal) {
                    renderRelatedCustomers(scope, element, newVal);
                });
            }
        };
    }
})(angular);