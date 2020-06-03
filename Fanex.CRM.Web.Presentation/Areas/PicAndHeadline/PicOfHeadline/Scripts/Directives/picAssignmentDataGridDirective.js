(function () {
	"use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').directive('picAssignmentDataGridDirective', function () {
		var cachedData = {
			scope: null,
			reportTable: null,
			gridData: null,
			gridOptions: null
		};

		function getGridHeight() {
			return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 230 : 50);
		}

		function getGripOtions(gridData) {
			var bodyData = [];
			var footerData = [];
			var reportLength = gridData ? gridData.Items.length : 0;
			var mergeHeadline = 0;
			var mergePIC = 0;
			const isDisabledChkboxSelectAll = IsDisabledSelectAllCheckbox(gridData.Items);
			if (reportLength > 0) {
				for (var i = 0; i < reportLength; i++) {
					var assignment = gridData.Items[i];
					var columns = [{ Name: "ColNo", Value: assignment.RowNum }];

					if (mergeHeadline === 0) {
						mergeHeadline = gridData.Items.filter(item => item.HeadlineID === assignment.HeadlineID).length;
						const pics = gridData.Items.filter(item => item.HeadlineID === assignment.HeadlineID && item.PIC === assignment.PIC);
						mergePIC = pics.length;
						const accounts = pics.map(pic => pic.DirectAccountID);
						columns.push(
							{ Name: "ColGroupHeadline", Value: assignment.HeadlineName, MergeRowNumber: mergeHeadline },
							{ Name: "ColHeadlineType", Value: assignment.HeadlineType, MergeRowNumber: mergeHeadline },
							{
								Name: "ColPIC",
								Value: getPICCell(assignment.PicList, assignment.HeadlineID, accounts, gridData.IsHost),
								MergeRowNumber: mergePIC,
								CellAlign: assignment.PicList === null ? 'center' : 'left'
							}
						);
					}

					if (mergePIC === 0 && mergeHeadline !== 0) {
						const pics = gridData.Items.filter(item => item.HeadlineID === assignment.HeadlineID && item.PIC === assignment.PIC);
						mergePIC = pics.length;
						const accounts = pics.map(pic => pic.DirectAccountID);
						columns.push(
							{
								Name: "ColPIC",
								Value: getPICCell(assignment.PicList, assignment.HeadlineID, accounts.toString()),
								MergeRowNumber: mergePIC,
								CellAlign: assignment.PicList === null ? 'center' : 'left'
							}
						);
					}

					const isDisabled = assignment.PicList === null || assignment.PicList.length === 0;
					columns.push(
						{ Name: "ColDirectAccount", Value: assignment.DirectAccount },
						{ Name: "ColStatus", Value: assignment.Status },
						{ Name: "ColSite", Value: assignment.Site },
						{ Name: "ColCurrency", Value: assignment.Currency },
						{ Name: "ColPosition", Value: assignment.Position }
					);

					if (gridData.IsHost) {
						columns.push(
							{ Name: "ColAction", Value: `<input class='item-remove-assign' data-row-index='${assignment.RowNum}' type='checkbox' ${isDisabled ? 'disabled' : ''}/>` }
						);
					}

					bodyData.push({
						Columns: columns
					});
					mergeHeadline--;
					mergePIC--;
				}
			}

			var headerColumns = [
				{ HeaderText: viewResources.No, Width: 70, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
				{ HeaderText: viewResources.GroupHeadline, Width: 150, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "center" },
				{ HeaderText: viewResources.HeadlineType, Width: 100, Name: "ColHeadlineType", HeaderAlign: "center", CellAlign: "center" },
				{ HeaderText: viewResources.PIC, Width: 150, Name: "ColPIC", HeaderAlign: "center", CellAlign: "left" },
				{ HeaderText: viewResources.DirectAccount, Width: 152, Name: "ColDirectAccount", HeaderAlign: "center", CellAlign: "left" },
				{ HeaderText: viewResources.Status, Width: 100, Name: "ColStatus", HeaderAlign: "center", CellAlign: "left" },
				{ HeaderText: viewResources.Site, Width: 150, Name: "ColSite", HeaderAlign: "center", CellAlign: "left" },
				{ HeaderText: viewResources.Currency, Width: 100, Name: "ColCurrency", HeaderAlign: "center", CellAlign: "left" },
				{ HeaderText: viewResources.Position, Width: 100, Name: "ColPosition", HeaderAlign: "center", CellAlign: "left" }

			];

			if (gridData.IsHost) {
				headerColumns.push(
					{
						HeaderText: `<input type='button' value='Remove' id='remove-btn' disabled/>
							<input type='checkbox' id='select-all-chkbox' ${isDisabledChkboxSelectAll ? 'disabled' : ''}/>`,
						Width: 70,
						Name: "ColAction",
						HeaderAlign: "center",
						CellAlign: "center"
					}
				);
			}

			var gridOptions = {
				columns: headerColumns,
				showTitle: true,
				titleInfo: {
					text: viewResources.PicAssignment,
					showExcelButton: true,
					customButtons: `<div class="total-unassigned-account">There are <b>${gridData.TotalUnassignedDirectAccount}</b> unassigned direct accounts</div>`,
					exportExcelFunction: function () {
						cachedData.scope.exportReport();
					}
				},
				bodyRows: bodyData,
				footerRows: footerData,
				numberOfFrozenColumn: 0,
				fullWidth: true,
				fullHeight: true,
				gridExpandHeight: getGridHeight(),
				showDistinctRow: false,
				showHighlightRow: true,
				headerColumnHeight: 29,
				bodyRowHeight: 29,
				showPager: true,
				showTree: false,
				customPager: true,
				allowTruncate: true,
				pagerOption: {
					itemsPerPage: cachedData.scope.searchCriteria.PageSize,
					currentPage: cachedData.scope.searchCriteria.PageIndex - 1,
					itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
					totalItem: gridData.Total,
					pagerSelectCallBack: ReRenderAfterPaged
				}
			};

			return gridOptions;
		}

		function IsDisabledSelectAllCheckbox(data) {
			let isDisabled = true;
			for (var i = 0; i < data.length; i++) {
				if (data[i].PicList !== null && data[i].PicList.length > 0) {
					isDisabled = false;
					return isDisabled;
				}
			}

			return isDisabled;
		}

		function ReRenderAfterPaged(page_index, itemsPerPage) {
			cachedData.scope.pagingCustomerReport(page_index + 1, itemsPerPage);
			return false;
		}

		function renderPicOfHeadlineGrid(scope, element) {
			if (scope.picOfHeadlineOptions.DataList === null) {
				return;
			}

			cachedData.scope = scope;
			var tablePicOfHeadline = element.find("#table-picOfHeadline");
			cachedData.reportTable = tablePicOfHeadline;
			var gridData = scope.picOfHeadlineOptions.DataList;
			cachedData.gridData = gridData;
			var gridOptions = getGripOtions(gridData);
			cachedData.gridOptions = gridOptions;
			tablePicOfHeadline.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

			if (gridData.IsHost) {
				initClickCheckBoxEvent(tablePicOfHeadline, scope);
			}
		}

		function initClickCheckBoxEvent($table, scope) {
			var chkbox = $table.find(".item-remove-assign");
			chkbox.unbind("click");
			chkbox.click(function (e) {
				var rowIndex = parseInt($(this).attr("data-row-index"));
				var itemIndex = scope.picOfHeadlineOptions.DataList.Items.findIndex(item => item.RowNum === rowIndex);
				let assignment = scope.picOfHeadlineOptions.DataList.Items[itemIndex];

				scope.onClickSingleCheckbox(assignment, e.target.checked);
			});

			var selectAllChkbox = $table.find("#select-all-chkbox");
			selectAllChkbox.unbind("click");
			selectAllChkbox.click(function (e) {
				var checkboxs = $table.find(".item-remove-assign");
				for (var i = 0; i < checkboxs.length; i++) {
					if (!checkboxs[i].hasAttribute('disabled')) {
						checkboxs[i].checked = e.target.checked;
					}
				}
				scope.onClickSelectAllCheckbox(e.target.checked);
			});

			var removeBtn = $table.find("#remove-btn");
			removeBtn.unbind("click");
			removeBtn.click(function (e) {
				scope.onClickRemoveBtn();
			});

			var asignPics = $table.find(".assignPic");
			asignPics.unbind("click");
			asignPics.click(function () {
				let headlineId = parseInt($(this).attr("data-headlineId"));
				let dataAccounts = $(this).attr("data-account");
				let accounts = dataAccounts.split(',');
				let accountList = scope.picOfHeadlineOptions.DataList.Items.filter(item => accounts.some(acc => item.DirectAccountID === parseInt(acc))).map(i => i.DirectAccount);
				scope.onClickPlusBtn(headlineId, accountList);
			});
		}

		function getPICCell(pics, headlineId, accountIds, isHost) {
			if (pics === null || pics.length === 0) {
				if (isHost) {
					return `<span class='assign-pic-icon assignPic' title='Assign PIC' data-headlineId=${headlineId} data-account='${accountIds}'>+</span>`;
				}

				return `<span>-</span>`;
			}
			else {
				let picCellValue = ``;
				for (var i = 0; i < pics.length; i++) {
					picCellValue += `<p class='pic-name'>` + pics[i].PicName + ` (` + pics[i].Role + `)</p>`;
				}
				return picCellValue;
			}
		}

		return {
			restrict: 'A',
			scope: false,
			link: function (scope, element, attrs) {
				scope.$watch('picOfHeadlineOptions', function (oldval, newval) {
					if (!scope.picOfHeadlineOptions.IsUpdateGrid) {
						return;
					}

					if (scope.picOfHeadlineOptions.IsResize && cachedData.reportTable) {
						cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
						scope.picOfHeadlineOptions.IsResize = false;
					} else {
						renderPicOfHeadlineGrid(scope, element);
					}

					scope.picOfHeadlineOptions.IsUpdateGrid = false;
				});
			}
		};
	});
})();