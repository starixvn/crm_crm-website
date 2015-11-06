(function () {
    "use strict";
    /* global updateInformResources,
    crmConstants */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('customer-informheadline-update').directive('tableInformHeadlineContainer', function () {
        var gridOptions, tableInformHeadline;

        function getGridOptions(bodyData, scope) {
            function ReRenderAfterPaged(page_index, itemsPerPage) {
                var onPageChanged = function () {
                    gridOptions.pagerOption.currentPage = page_index;
                    gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                    tableInformHeadline.fanexGrid(gridOptions);
                    scope.getInforms(page_index, itemsPerPage);
                };

                if (scope.isDataChanged()) {
                    scope.showConfirmIgnoreChange(onPageChanged);
                    return false;
                }

                onPageChanged();
                return false;
            }

            return {
                columns: [{ HeaderText: updateInformResources.Headline, Width: 150, Name: 'ColHeadline', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: updateInformResources.Status, Width: 150, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: updateInformResources.UpdatedTime, Width: 105, Name: 'ColUpdatedTime', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: updateInformResources.Remark, Width: 340, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: updateInformResources.Handler, Width: 150, Name: 'ColHandler', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: updateInformResources.Extension, Width: 80, Name: 'ColExtension', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: updateInformResources.ProductStatus, Width: 110, Name: 'ColProductStatus', HeaderAlign: 'center', CellAlign: 'center'}],
                bodyRows: bodyData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: false,
                gridHeight: 710,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.pagerOption.itemsPerPage,
                    currentPage: scope.pagerOption.currentPage,
                    itemsPerPageArray: scope.pagerOption.itemsPerPageArray,
                    totalItem: scope.pagerOption.totalItem,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
        }

        function getGridBody(informs, scope) {
            var bodyData = [];
            var info, cell;
            var reportLength = informs.length;
            var rowHeight;
            if (reportLength < 10) {
                rowHeight = 630 / reportLength;
            }

            for (var i = 0; i < reportLength; i++) {
                info = informs[i];
                cell = [{ Name: 'ColHeadline', Value: info.HeadlineName },
                    { Name: 'ColStatus', Value: renderSelectStatus(info.StatusId, scope, i) },
                    { Name: 'ColUpdatedTime', Value: info.UpdatedTimeString },
                    { Name: 'ColRemark', Value: renderTextboxRemark(info.Remark, i, rowHeight) },
                    { Name: 'ColHandler', Value: info.Handler },
                    { Name: 'ColExtension', Value: renderExtension(info.ExtensionId, scope, i) },
                    { Name: 'ColProductStatus', Value: renderProductStatus(info.ProductStatus, scope, i)}];


                bodyData.push({ Columns: cell, RowHeight: rowHeight });
            }

            return bodyData;
        }

        function renderInform(informs, scope, element) {
            var dataLength = !informs ? 0 : informs.length;
            var bodyData = dataLength === 0 ? [] : getGridBody(informs, scope);
            gridOptions = getGridOptions(bodyData, scope);
            tableInformHeadline = element.find('#table-inform-headline');
            tableInformHeadline.empty()
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);
        }

        function renderSelectStatus(selectedValue, scope, index) {
            var html = [];
            html.push('<select class="status" data-index="' + index + '">');
            $.each(scope.statusOptions.DataList, function (i, item) {
                //jshint unused:false
                html.push('<option value="', item.Id, '"');
                if (selectedValue === item.Id) {
                    html.push(' selected ');
                }

                html.push('>', item.Name, '</option>');
            });

            html.push('</select>');
            return html.join('');
        }

        function renderExtension(selectedValue, scope, index) {
            var html = [];
            html.push('<select class="extension" data-index="' + index + '">');
            $.each(scope.extensions, function (i, item) {
                //jshint unused:false
                html.push('<option value="', item.Id, '"');
                if (selectedValue === item.Id) {
                    html.push(' selected ');
                }

                html.push('>', item.Value, '</option>');
            });

            html.push('</select>');
            return html.join('');
        }

        function renderProductStatus(selectedValue, scope, index) {
            var html = [];
            html.push('<select class="productStatus" data-index="' + index + '">');
            $.each(scope.productStatus, function (i, item) {
                //jshint unused:false
                html.push('<option value="', item.Id, '"');
                if (selectedValue === item.Id) {
                    html.push(' selected ');
                }

                html.push('>', item.Name, '</option>');
            });

            html.push('</select>');
            return html.join('');
        }

        function renderTextboxRemark(remark, index, rowHeight) {
            var style = rowHeight ? 'style="resize: vertical; width: 98%; height: ' + (rowHeight - 10) + '"'
            : 'style="resize: vertical; width: 98%; height: 62px;"';

            return '<textarea class="textbox remark" ' + style + ' rows="3" data-index="' + index + '">' + remark + '</textarea>';
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    //jshint unused:false
                    renderInform(newValues, scope, element);
                    if (newValues && newValues.length > 0) {
                        element.find('.status')
                        .unbind('change')
                        .bind('change', function () {
                            var $this = $(this);
                            var index = $this.data('index');
                            newValues[index].StatusId = $this.val();
                            newValues[index].IsChanged = true;
                            scope.updateIsStatusChanged(true);
                        });

                        element.find('.remark')
                        .unbind('keyup')
                        .bind('keyup', function () {
                            var $this = $(this);
                            var index = $this.data('index');
                            if (newValues[index].Remark !== $this.val()) {
                                newValues[index].Remark = $this.val();
                                newValues[index].IsChanged = true;
                                scope.updateIsStatusChanged(true);
                            }
                        });

                        element.find('.extension')
                        .unbind('change')
                        .bind('change', function () {
                            var $this = $(this);
                            var index = $this.data('index');
                            newValues[index].ExtensionId = $this.val();
                            newValues[index].IsChanged = true;
                            scope.updateIsStatusChanged(true);
                        });

                        element.find('.productStatus')
                        .unbind('change')
                        .bind('change', function () {
                            var $this = $(this);
                            var index = $this.data('index');
                            newValues[index].ProductStatus = $this.val();
                            newValues[index].IsChanged = true;
                            scope.updateIsStatusChanged(true);
                        });
                    }
                });
            }
        };
    });
})();