'use strict';
/* global updateInformHeadlineApplication,
updateInformResources,
crmConstants */
/* jshint camelcase: false */
/* jshint unused: vars */
updateInformHeadlineApplication.directive('tableInformHeadlineContainer', function () {
    var gridOptions, tableInformHeadline;

    function getGridOptions(bodyData, scope) {
        function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
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
                    { HeaderText: updateInformResources.Remark, Width: 250, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: updateInformResources.Handler, Width: 150, Name: 'ColHandler', HeaderAlign: 'center', CellAlign: 'left'}],
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: false,
            //gridExpandHeight: 800,
            gridHeight: 800,
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
        for (var i = 0, reportLength = informs.length; i < reportLength; i++) {
            info = informs[i];
            cell = [{ Name: 'ColHeadline', Value: info.HeadlineName },
                    { Name: 'ColStatus', Value: renderSelectStatus(info.StatusId, scope, i) },
                    { Name: 'ColUpdatedTime', Value: info.UpdatedTimeString },
                    { Name: 'ColRemark', Value: renderTextboxRemark(info.Remark, i) },
                    { Name: 'ColHandler', Value: info.Creator}];


            bodyData.push({ Columns: cell });
        }

        return bodyData;
    }

    function renderInform(informs, scope, element, attrs) {
        var bodyData = (!informs || informs.length === 0) ? [] : getGridBody(informs, scope);
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
            html.push('<option value="', item.Id, '"');
            if (selectedValue === item.Id) {
                html.push(' selected ');
            }

            html.push('>', item.Name, '</option>');
        });

        html.push('</select>');
        return html.join('');
    }

    function renderTextboxRemark(remark, index) {
        return '<input type="text" class="textbox remark" value="' + remark + '" data-index="' + index + '"/>';
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                renderInform(newValues, scope, element, attrs);
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
                        .unbind('blur')
                        .bind('blur', function () {
                            var $this = $(this);
                            var index = $this.data('index');
                            if (newValues[index].Remark !== $this.val()) {
                                newValues[index].Remark = $this.val();
                                newValues[index].IsChanged = true;
                                scope.updateIsStatusChanged(true);
                            }
                        });
                }
            });
        }
    };
});