(function () {
    "use strict";
    /* global
    crmConstants,
    viewResources */
    angular.module('customerdetail-application').directive('dropdownSiteContainer', function () {
        function renderDropdown(scope, element) {
            if (scope.siteList) {
                var dropdown = element.find("#customerinfodetail-siteoptions");
                dropdown.empty();
                var optionHtml =
                '<option value="' + crmConstants.Enums.Site.NotApply + '"' +
                    (scope.customerDetail.SiteSetting.Id === crmConstants.Enums.Site.NotApply ?
                    ' selected="selected"' :
                    '') +
                    '>' + viewResources.Select + '</option>';

                var siteGroupLength = scope.siteList.length;
                for (var i = 0; i < siteGroupLength; i++) {
                    var siteGroup = scope.siteList[i];
                    optionHtml += '<optgroup label="' + siteGroup.Name +
                    '" value="' + siteGroup.Id + '">';
                    var siteLength = siteGroup.ChildSites.length;
                    for (var j = 0; j < siteLength; j++) {
                        var site = siteGroup.ChildSites[j];
                        optionHtml += '<option value="' + site.Id + '"' +
                        (scope.customerDetail.SiteSetting.Id === site.Id ?
                        ' selected="selected"' :
                        '') + '>' + site.Name + '</option>';
                    }

                    optionHtml += '</optgroup>';
                }

                dropdown.append(optionHtml);
                element.change(function () {
                    scope.updateSelectedSite(dropdown.find('option:selected').val());
                });
            }
        }
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    }).directive('dropdownCountryContainer', function () {
        function renderDropdown(scope, element) {
            if (scope.countryList) {
                var dropdown = element.find("#customerinfodetail-countryoptions");
                dropdown.empty();
                /* jshint camelcase: false */
                dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                    (scope.customerDetail.Country.Id === crmConstants.Enums.Country.NotApply ? ' selected="selected"' : '') +
                    '">' + viewResources.Symbol_NotApply +
                '</option>');
                var countryListLength = scope.countryList.length;
                for (var i = 0; i < countryListLength; i++) {
                    var country = scope.countryList[i];
                    dropdown.append(
                    '<option' +
                        ' value="' + country.Id + '"' +
                        (scope.customerDetail.Country.Id === country.Id ? ' selected="selected"' : '') +
                    '>' + country.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedCountry(dropdown.find('option:selected').val());
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    }).directive('dropdownProvinceContainer', function () {
        function renderDropdown(scope, element) {
            if (scope.provinceList) {
                var dropdown = element.find("#customerinfodetail-provinceoptions");
                dropdown.empty();
                /* jshint camelcase: false */
                dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                    (scope.customerDetail.Province.Id === crmConstants.Enums.Country.NotApply ? ' selected="selected"' : '') +
                    '">' + viewResources.Symbol_NotApply +
                '</option>');
                var provinceListLength = scope.provinceList.length;
                for (var i = 0; i < provinceListLength; i++) {
                    var province = scope.provinceList[i];
                    dropdown.append(
                    '<option' +
                        ' value="' + province.Id + '"' +
                        (scope.customerDetail.Province.Id === province.Id ? ' selected="selected"' : '') +
                    '>' + province.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedProvince(dropdown.find('option:selected').val());
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    }).directive('dropdownCountryBankContainer', function () {
        function renderDropdown(scope, element) {
            if (scope.countryList) {
                var dropdown = element.find("#customerinfodetail-countryoptions-bank");
                dropdown.empty();
                /* jshint camelcase: false */
                dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                    (scope.customerDetail.BankInfo.AddressInfo.Country.Id === crmConstants.Enums.Country.NotApply ? ' selected="selected"' : '') +
                    '">' + viewResources.Symbol_NotApply +
                '</option>');
                var countryListLength = scope.countryList.length;
                for (var i = 0; i < countryListLength; i++) {
                    var country = scope.countryList[i];
                    dropdown.append(
                    '<option' +
                        ' value="' + country.Id + '"' +
                        (scope.customerDetail.BankInfo.AddressInfo.Country.Id === country.Id ? ' selected="selected"' : '') +
                    '>' + country.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedCountryBank(dropdown.find('option:selected').val());
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    }).directive('dropdownLanguage', function () {
        function renderDropdownLanguage(scope, element) {
            var dropdown = element.find("#customerinfodetail-languageoptions");
            dropdown.empty();
            var dropdownLanguage = [];
            var prefixId = 'customerinfo-languageoptions-';
            var isCheckAll = true;

            var languageList = scope.languageList;
            if (languageList && languageList.length > 0) {
                var optionLength = languageList.length;
                for (var i = 0; i < optionLength; i++) {
                    var info = languageList[i];
                    var isChecked = scope.isCustomerKnowLanguage(info.Id);
                    if (!isChecked) {
                        isCheckAll = false;
                    }

                    dropdownLanguage.push({
                        id: prefixId + info.Id,
                        value: info.Id,
                        text: info.Name,
                        checked: isChecked,
                        enable: true,
                        cssClass: "",
                        items: []
                    });
                }
            }

            /* jshint camelcase: false */
            dropdown.fanexMultiSelect(
        {
            minWidth: 177,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: isCheckAll,
            showFooter: false,
            showTooltip: false,
            checkAllText: viewResources.All_Languages,
            isUsedCustomCheckAllText: true,
            customCheckAllText: viewResources.All_Languages,
            noItemText: viewResources.No_languages_found,
            noneSelectedText: viewResources.Symbol_NotApply,
            selectedList: 3,
            listData: dropdownLanguage
        });

            element.find("#customerinfodetail-languageoptions .fanex-multiselect-state-default").attr("tabindex", 7);
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                renderDropdownLanguage(scope, element);
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    renderDropdownLanguage(scope, element);
                    scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                    scope.updateOldSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                });

                element.on('change', function () {
                    scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                    if (!scope.$$phase) {
                        scope.$apply();
                    }
                });
            }
        };
    });
})();