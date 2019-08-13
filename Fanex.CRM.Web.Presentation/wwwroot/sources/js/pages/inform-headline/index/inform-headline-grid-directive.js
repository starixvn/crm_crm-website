(function (angular) {
    'use strict';
    /* global crmConstants, viewResources */

    angular.module('informHeadlineModule').directive('informHeadlineGrid', informHeadlineGrid);
    informHeadlineGrid.$inject = ['$document'];

    function informHeadlineGrid($document) {
        const INT_FORMAT = '0,00';

        function renderListData(scope, element) {
            const listData = scope.listData;
            if (listData === null) {
                return;
            }

            const canContact = scope.contactPolicy !== 'None';
            let $table = $(element);
            let bodyData = [];
            $table.empty();
            const rowsCount = listData.length;
            if (rowsCount > 0) {
                for (var i = 0; i < rowsCount; i++) {
                    const item = listData[i];

                    bodyData.push({
                        Columns: [
                            { Name: 'Edit', Value: '<button type="button" class="action-button">&nbsp;</button>' },
                            { Name: 'Title', Value: canContact ? '<a class="title" href="javascript:void(0)">' + item.Title + '</a>' : item.Title },
                            { Name: 'CreatedDate', Value: item.ClientCreatedDate },
                            { Name: 'NoDirectAccounts', Value: item.Total },
                            { Name: 'Status', Value: item.Status },
                            { Name: 'Done', Value: item.Done },
                            { Name: 'RequestToCallBack', Value: item.RequestToCallBack },
                            { Name: 'NoNeedToCall', Value: item.NoNeedToCall },
                            { Name: 'NoReply', Value: item.NoReply },
                            { Name: 'Uncontactable', Value: item.Uncontactable },
                            { Name: 'InternalAccount', Value: item.InternalAccount },
                            { Name: 'NeverTouch', Value: item.NeverTouch },
                            { Name: 'KeptForReference', Value: item.KeptForReference }
                        ],
                        CssClass: 'status-' + item.Status.toLowerCase(),
                        CustomAttr: 'data-title-id=' + item.TitleId
                    });
                }
            }

            const gridOptions = {
                columns: [
                    { HeaderText: viewResources.Edit, Width: 50, Name: 'Edit', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: viewResources.Title, Width: 250, Name: 'Title', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: viewResources.CreatedDate, Width: 100, Name: 'CreatedDate', HeaderAlign: 'center' },
                    {
                        HeaderText: viewResources.NoDirectAccounts, Width: 100, Name: 'NoDirectAccounts',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    { HeaderText: viewResources.Status, Width: 100, Name: 'Status', HeaderAlign: 'center', CellAlign: 'center' },
                    {
                        HeaderText: viewResources.Done, Width: 100, Name: 'Done',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.RequestToCallBack, Width: 100, Name: 'RequestToCallBack',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.NoNeedToCall, Width: 100, Name: 'NoNeedToCall',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.NoReply, Width: 100, Name: 'NoReply',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.Uncontactable, Width: 100, Name: 'Uncontactable',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.InternalAccount, Width: 100, Name: 'InternalAccount',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.NeverTouch, Width: 100, Name: 'NeverTouch',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    },
                    {
                        HeaderText: viewResources.KeptForReference, Width: 100, Name: 'KeptForReference',
                        HeaderAlign: 'center', CellAlign: 'center', FormatNumber: INT_FORMAT
                    }
                ],
                showTitle: true,
                titleInfo: {
                    text: viewResources.InformHeadlinesCases,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportInformHeadlines();
                    }
                },
                bodyRows: bodyData,
                showDistinctRow: false,
                columnsNeedHide: rowsCount === 0 || scope.searchPolicy === 'Host' ? null : [0]
            };

            $table.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    renderListData(scope, element);
                });

                let $table = $(element);
                let $contextMenu = angular.element('#contextMenu');
                $document.click(function () {
                    $contextMenu.hide();
                });

                angular.element('.grid-container').scroll(function () {
                    $contextMenu.hide();
                });

                $contextMenu.on('click', 'a', function (e) {
                    const $element = angular.element(e.target);
                    const titleId = $contextMenu.data('title-id');
                    if ($element.hasClass('edit-menu-item')) {
                        scope.showUpdateCaseDialog(titleId);
                    } else if ($element.hasClass('view-logs')) {
                        scope.viewLogs(titleId);
                    } else {
                        const status = $element.data('status');
                        scope.updateInformStatus(titleId, status);
                    }
                });

                $table.on('click', '.action-button', function (e) {
                    const $target = angular.element(e.target);
                    const titleId = $target.closest('tr').data('title-id');
                    $contextMenu.data('title-id', titleId);
                    $contextMenu.find('li').hide();
                    if ($target.closest('tr').hasClass('status-closed')) {
                        $contextMenu.find('.status-closed').show();
                    } else {
                        $contextMenu.find(':not(.status-closed)').show();
                    }

                    const offset = $target.offset();
                    $contextMenu.show().css({
                        display: 'block',
                        left: offset.left,
                        top: offset.top + $target.height()
                    });

                    return false;
                });

                $table.on('click', '.title', function (e) {
                    const $target = angular.element(e.target);
                    const titleId = $target.closest('tr').data('title-id');
                    const isClosed = $target.closest('tr').hasClass('status-closed');
                    scope.showContactDialog(titleId, isClosed);
                });
            }
        };
    }
})(angular);