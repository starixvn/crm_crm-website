(function () {
    "use strict";
    /* global crmConstants,
    viewResources */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('updateInformApplication').directive('informHeadlineListContainer', function () {
        var KEPT_FOR_REFERENCE_STATUS_ID = 45007;
        var MIN_PAGE_SIZE = 100;
        var MAX_PAGE_SIZE = 300;
        var INFORM_SPECIAL_CASE = [1, 5, 12];
        var _selfScope;

        function generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue, disabled) {
            var html = '<select ' + (disabled ? 'disabled' : '') + '  data-binding-name="' + bindingName + '" style="width: 100px">';

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var value = item[valueProperty];
                html += '<option value="' + item[valueProperty] + '" ' +
                    (value && selectedValue && value.toString() === selectedValue.toString() ? 'selected' : '') +
                    ' >' + item[textProperty] + '</option>';
            }
            html += '</select>';

            return html;
        }
        function htmlEncode(value) {
            return String(value)
                .replace(/&/g, '&amp;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&#39;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }
        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }
        function hasKeepThisAccountOption(caseId) {
            return INFORM_SPECIAL_CASE.indexOf(parseInt(caseId)) > -1;
        }

        function renderDropdownCell(columnModel, data, bindingName, valueProperty, textProperty, selectedValue) {
            if (columnModel.Value.InformHeadlineStatusId === KEPT_FOR_REFERENCE_STATUS_ID) {
                return '<span class="disabled-box">' + viewResources.KeptForReference + '</span>';
            }

            return generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue, columnModel.Value.Case > 0);
        }

        function renderStatusDropdownCell(columnModel) {
            var html = "";
            if (columnModel.Value.InformHeadlineStatusId === KEPT_FOR_REFERENCE_STATUS_ID) {
                return '<span class="disabled-box">' + viewResources.KeptForReference + '</span>';
            }
            else if (hasKeepThisAccountOption(columnModel.Value.Case)) {
                html = '<div class="checkbox display-inline-block"><label><input class="iskeep" value="true" type="checkbox" data-binding-name="IsKeep" />' +
                    viewResources.KeepThisAccount + '</label></div>';
            }

            return html + generateDropdown(_selfScope.$parent.informStatuses,
                "InformHeadlineStatusId", "SettingId", "SettingValue",
                columnModel.Value.InformHeadlineStatusId, columnModel.Value.Case > 0);
        }

        function renderExtensionDropdownCell(columnModel) {
            return renderDropdownCell(columnModel, _selfScope.$parent.stations,
                "Extension", "StationId", "Name", columnModel.Value.Extension);
        }

        function renderProductStatusDropdownCell(columnModel) {
            return renderDropdownCell(columnModel, _selfScope.$parent.productStatuses,
                "ProductStatus", "StatusId", "Name", columnModel.Value.ProductStatus);
        }

        function renderRemarkCell(columnModel) {
            return '<textarea data-binding-name="Remark">' + htmlDecode(columnModel.Value.Remark) + '</textarea>';
        }

        function getDescriptionCase(dataItem, caseId) {
            var message = viewResources['ChangedHeadlineCase' + caseId];
            switch (parseInt(caseId)) {
                case 1:
                case 5:
                    {
                        message = message.replace('(A)', dataItem.OldUserName || dataItem.UserName);
                        break;
                    }
                case 2:
                case 4:
                    {
                        message = message.replace('(A)', dataItem.NewUserName || dataItem.UserName);
                        break;
                    }
                case 3:
                    {
                        message = message.replace('(A)', dataItem.OldUserName);
                        message = message.replace('(B)', dataItem.NewUserName);
                        break;
                    }
                default:
                    break;
            }

            return message;
        }

        function addTooltip(e) {
            var $target = angular.element(e.target);
            var rowIndex = parseInt($target.closest("tr").data("rowindex"));
            var caseId = $target.data("case");
            var message = getDescriptionCase(_selfScope.dataSource[rowIndex], caseId);

            $target.attr("data-original-title", message).tooltip({ placement: "right" }).tooltip("show");
            $target.unbind("hover", addTooltip);
        }

        function loadCompleted($table) {
            $table.find(".description-tooltip").hover(addTooltip);
            $("textarea").each(function () {
                $(this).height(this.scrollHeight);
            });
            setTimeout(function () {
                $table.resize();
            }, 20);
        }

        function generateBodyRows(scope, pageIndex, pageSize) {
            var bodyData = [];
            var rowsCount = scope.dataSource.length;
            var firstRowIndex = pageIndex * pageSize;
            if (rowsCount >= firstRowIndex) {
                var lastRowIndex = firstRowIndex + pageSize;
                if (lastRowIndex > rowsCount) {
                    lastRowIndex = rowsCount;
                }

                for (var i = firstRowIndex; i < lastRowIndex; i++) {
                    var item = scope.dataSource[i];

                    var isUseOld = item.Case === 1 || item.Case === 5;
                    var userName = item.HeadlineId > 0 ? item.UserName : (isUseOld ? item.OldUserName : item.NewUserName);
                    var headlineId = item.HeadlineId > 0 ? item.HeadlineId : (isUseOld ? item.OldCustId : item.NewCustId);

                    bodyData.push({
                        Columns: [
                            {
                                Name: "DirectAccount", Value: item.Case === 0 ?
                                    userName : '<span class="description-tooltip" data-case=' + item.Case + '>' + userName + '</span>'
                            },
                            { Name: "Status", Value: item, CustomCellRender: renderStatusDropdownCell },
                            { Name: "UpdatedTime", Value: item.ClientUpdatedTime },
                            { Name: "Remark", Value: item, CustomCellRender: renderRemarkCell },
                            { Name: "Handler", Value: item.Handler },
                            { Name: "Extension", Value: item, CustomCellRender: renderExtensionDropdownCell },
                            { Name: "ProductStatus", Value: item, CustomCellRender: renderProductStatusDropdownCell },
                        ],
                        CssClass: item.Case > 0 ? 'updated' : (item.GroupId > 0 ? 'headline' : 'direct'),
                        CustomAttr: 'data-headline-id=' + headlineId + ' data-rowindex=' + i
                    });
                }
            }

            return bodyData;
        }
        function renderListData(scope, element) {
            _selfScope = scope;
            if (scope.dataSource === null) {
                return;
            }

            var $table = element.find("#inform-headline-list");
            $table.empty();

            var gridOptions = {
                columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 120, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Status, Width: 130, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    {
                        HeaderText: viewResources.UpdatedTime, Width: 100, Name: "UpdatedTime", HeaderAlign: "center",
                        FormatDateTime: "MMM DD, YYYY hh:mm:ss A"
                    },
                    { HeaderText: viewResources.Remark, Width: 420, Name: "Remark", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Handler, Width: 100, Name: "Handler", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Extension, Width: 110, Name: "Extension", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.ProductStatus, Width: 110, Name: "ProductStatus", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: generateBodyRows(scope, 0, MIN_PAGE_SIZE),
                numberOfFrozenColumn: 0,
                fullWidth: true,
                showDistinctRow: false,
                showHighlightRow: true,
                showTree: true,
                treeParentPaddingLeft: 5,
                headerColumnHeight: 29,
                bodyRowHeight: 20,
                gridExpandHeight: 250,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: MIN_PAGE_SIZE,
                    maxItemsPerPage: MAX_PAGE_SIZE,
                    currentPage: 0,
                    totalItem: scope.dataSource.length,
                    startPerPage: MIN_PAGE_SIZE,
                    pageStep: MIN_PAGE_SIZE,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        gridOptions.pagerOption.itemsPerPage = pageSize;
                        gridOptions.pagerOption.pageSize = pageSize;
                        gridOptions.pagerOption.currentPage = pageIndex;
                        gridOptions.bodyRows = generateBodyRows(scope, pageIndex, pageSize);
                        $table.fanexGrid(gridOptions);

                        loadCompleted($table);
                    }
                }
            };

            $table.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            loadCompleted($table);
        }

        return {
            restrict: 'A',
            require: '^ngModel',
            scope: {
                dataSource: '=ngModel',
                updatedItems: '='
            },
            link: function (scope, element) {
                scope.updatedItems = scope.updatedItems || {};
                scope.$watch("dataSource", function (oldval, newval) {
                    /* jshint unused: false */
                    scope.updatedItems = {};
                    renderListData(scope, element);
                });

                var $table = element.find("#inform-headline-list");

                function updateDataSource($target, useHtmlEncode) {
                    var $tr = $target.closest("tr");
                    var headlineId = $tr.data("headline-id");
                    var rowIndex = parseInt($tr.data("rowindex"));
                    var bindingName = $target.data("binding-name");

                    var dataItem = scope.dataSource[rowIndex];
                    dataItem[bindingName] = $target.is(":checkbox") ? $target.is(":checked") : (useHtmlEncode ? htmlEncode($target.val()) : $target.val());
                    scope.updatedItems[headlineId] = dataItem;
                }

                $table.on("change", "select, input[type=checkbox]", function (e) {
                    updateDataSource(angular.element(e.target));
                });

                $table.on("change", "textarea", function (e) {
                    updateDataSource(angular.element(e.target), true);
                });
            }
        };
    });
})();