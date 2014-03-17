"use strict";
/* global directiveManager,
crmConstants,
viewResources */
/* jshint unused: vars */
directiveManager = directiveManager || {};
directiveManager.registerDropdownSiteContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        //        var defaultRegisterParams = {
        //            PlugInName: null,
        //            OptionName: null,
        //            PickerId: null
        //        };

        //        var defaultOption = {
        //            DataList: null,
        //            OnChangeEvent: null,
        //            IsIncludeNA: null,
        //        };

        var dataList = [];
        function renderDropdown(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null ||
                options.DataList === null) {
                return;
            }

            dataList = options.DataList;
            var dropdown = element.find("#" + registerParams.PickerId);
            dropdown.empty();

            var optionHtml = '';
            if (options.IsIncludeNA) {
                optionHtml =
                '<option value="' + crmConstants.Enums.Site.NotApply + '">' + options.AllText + '</option>';
            }

            var siteGroupLength = options.DataList.length;
            for (var i = 0; i < siteGroupLength; i++) {
                var siteGroup = options.DataList[i];
                optionHtml += '<optgroup label="' + siteGroup.Name +
                    '" value="' + siteGroup.Id + '">';
                var siteLength = siteGroup.ChildSites.length;
                for (var j = 0; j < siteLength; j++) {
                    var site = siteGroup.ChildSites[j];
                    optionHtml += '<option value="' + site.Id + '"' +
                        (site.Id === options.SelectedId ?
                        ' selected="selected"' :
                        '') +
                    '>' + site.Name + '</option>';
                }

                optionHtml += '</optgroup>';
            }

            dropdown.append(optionHtml);
            element.change(function () {
                if (options.OnChangeEvent &&
                    typeof (options.OnChangeEvent) === "function") {
                    var selectedItem = getSelectedItem(scope, dropdown.find('option:selected').val());
                    options.OnChangeEvent(selectedItem);
                }
            });
        }

        function getSelectedItem(scope, selectedId) {
            var options = scope[registerParams.OptionName];
            if (options === null ||
                options.DataList === null) {
                return;
            }

            selectedId = parseInt(selectedId);
            if (selectedId === crmConstants.Enums.Site.NotApply) {
                return {
                    Id: crmConstants.Enums.Site.NotApply,
                    Name: options.AllText
                };
            }

            var dataListLength = dataList.length;
            var selectedItem = null;
            for (var i = 0; i < dataListLength; i++) {
                var data = dataList[i];
                var siteLength = data.ChildSites.length;
                for (var j = 0; j < siteLength; j++) {
                    var site = data.ChildSites[j];
                    if (site.Id === selectedId) {
                        selectedItem = site;
                        break;
                    }
                }

            }

            return selectedItem;
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: vars */
                    renderDropdown(scope, element, attrs);
                });
            }
        };
    });
};

