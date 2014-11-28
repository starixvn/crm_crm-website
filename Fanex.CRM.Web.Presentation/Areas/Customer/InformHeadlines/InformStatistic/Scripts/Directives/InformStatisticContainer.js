'use strict';
/*jshint unused: vars*/
/* global informHeadlineStatisticApplication,
informStatisticResourceView,
crmConstants,
canUpdateStatus */
informHeadlineStatisticApplication.directive('tableInformStatisticContainer', function () {
    var tableStatistic, gridOptions;
    function getGridColumnStatus(text, name, width) {
        return {
            HeaderText: text,
            Width: width,
            Name: name,
            HeaderAlign: 'center',
            CellAlign: 'right',
            FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
        };
    }

    function getGridColumnOption() {
        return [{ HeaderText: informStatisticResourceView.Title, Width: 300, Name: 'ColTitle', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: informStatisticResourceView.CreatedDate, Width: 80, Name: 'ColCreatedDate', HeaderAlign: 'center', CellAlign: 'center' },
            getGridColumnStatus(informStatisticResourceView.NoOfHeadlines, 'ColNoOfHeadline', 80),
            getGridColumnStatus(informStatisticResourceView.NoOfHeadlinesToInform, 'ColNoOfHeadlinesToInform', 105),
            getGridColumnStatus(informStatisticResourceView.Done, 'ColDone', 80),
            getGridColumnStatus(informStatisticResourceView.RequestCallback, 'ColRequestCallback', 80),
            getGridColumnStatus(informStatisticResourceView.NoNeedToCall, 'ColNoNeedToCall', 80),
            getGridColumnStatus(informStatisticResourceView.NoReply, 'ColNoReply', 80),
            getGridColumnStatus(informStatisticResourceView.Uncontactable, 'ColUncontactable', 90),
            getGridColumnStatus(informStatisticResourceView.InternalAccount, 'ColInternalAccount', 80),
            getGridColumnStatus(informStatisticResourceView.NeverTouch, 'ColNeverTouch', 80)];
    }

    function getGridOptions(exportExcelCallback) {
        return {
            columns: getGridColumnOption(),
            showTitle: true,
            titleInfo: {
                text: '',
                showExcelButton: true,
                exportExcelFunction: exportExcelCallback
            },
            bodyRows: [],
            numberOfFrozenColumn: 3,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 100,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29
        };
    }

    function getGridBody(informs) {
        var bodyData = [];
        var length = informs ? informs.length : 0;
        if (length > 0) {
            var cell, inform, titleRow;
            for (var i = 0; i < length; i++) {
                inform = informs[i];
                titleRow = '<span class="edittitle-link" data-titleid="' + inform.TitleId + '">' + inform.Title + '</span>';
                cell = [{ Name: 'ColTitle', Value: titleRow },
                    { Name: 'ColCreatedDate', Value: inform.CreatedDateString },
                    { Name: 'ColNoOfHeadline', Value: inform.Total },
                    { Name: 'ColNoOfHeadlinesToInform', Value: inform.NoOfHeadlinesToInform },
                    { Name: 'ColDone', Value: inform.Done },
                    { Name: 'ColRequestCallback', Value: inform.RequestToCallback },
                    { Name: 'ColNoNeedToCall', Value: inform.NoNeedToCall },
                    { Name: 'ColNoReply', Value: inform.NoReply },
                    { Name: 'ColUncontactable', Value: inform.Uncontactable },
                    { Name: 'ColInternalAccount', Value: inform.InternalAccount },
                    { Name: 'ColNeverTouch', Value: inform.NeverTouch}];

                bodyData.push({ Columns: cell, CssClass: 'edittitle-link-row' });
            }
        }

        return bodyData;
    }

    function renderStatistic(scope, tableStatistic, statistic) {
        if (!statistic) {
            return;
        }

        var reportTitle = statistic.ReportTitle;
        var informs = statistic.Informs;

        gridOptions.titleInfo.text = reportTitle;
        gridOptions.bodyRows = getGridBody(informs);

        tableStatistic.empty()
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);
    }

    function openUpdateInform(scope, titleElement) {
        var titleId = titleElement.data('titleid');
        scope.openUpdateInform(titleId);
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            tableStatistic = element.find('#table-inform-statistic');
            gridOptions = getGridOptions(scope.exportInformStatisticExcel);

            scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                renderStatistic(scope, tableStatistic, newValue);

                if (canUpdateStatus === 1) {
                    var editTitleRows = tableStatistic.find('.edittitle-link-row');
                    editTitleRows
                        .unbind('dblclick')
                        .bind('dblclick', function () {
                            var titleRow = $(this).find('.edittitle-link');
                            openUpdateInform(scope, titleRow);
                        });
                    var editTitleLinks = tableStatistic.find('.edittitle-link');
                    editTitleLinks
                        .unbind('click')
                        .bind('click', function () {
                            openUpdateInform(scope, $(this));
                        });
                }
            });
        }
    };
});