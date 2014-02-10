"use strict";
/* global customerDetailApplication,
crmConstants,
viewResources */
customerDetailApplication.directive('dropdownSite', function () {
    return {
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Site.NotApply + '"' +
                     ' data-ng-click="updateSelectedSite();"' +
                     ' ng-selected="customerDetail.SiteSetting.Id == crmConstants.Enums.Site.NotApply">' + viewResources.Select + '</option>\
                   <optgroup data-ng-repeat="siteGroup in siteList track by $index" label="{{ siteGroup.Name }}" value="{{ siteGroup.Id }}">\
                        <option data-ng-repeat="site in siteGroup.ChildSites track by $index"' +
                            ' data-ng-click="updateSelectedSite(site);"' +
                            ' value="{{ site.Id }}"' +
                            ' ng-selected="customerDetail.SiteSetting.Id == site.Id">{{ site.Name }}</option>\
                   </optgroup>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
            /* jshint unused: vars */
        },
        link: function (scope, elem, attrs) {
            /* jshint unused: vars */
        }
    };
}).directive('dropdownCountry', function () {
    return {
        /* jshint camelcase: false */
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Country.NotApply +
                        ' data-ng-click="updateSelectedCountry(country);"' +
                        ' ng-selected="customerDetail.Country.Id == crmConstants.Enums.Country.NotApply">' +
                        viewResources.Symbol_NotApply +
                  '</option>\
                   <option data-ng-repeat="country in countryList track by $index"' +
                        ' data-ng-click="updateSelectedCountry(country);"' +
                        ' value="{{ country.Id }}"' +
                        ' ng-selected="customerDetail.Country.Id == country.Id"' +
                        '>{{ country.Name }}</option>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
            /* jshint unused: vars */
        },
        link: function (scope, elem, attrs) {
            /* jshint unused: vars */
        }
    };
}).directive('dropdownCountryBankContainer', function () {
    function renderDropdown(scope, element, attrs) {
        if (scope.countryList) {
            var dropdown = element.find("#customerinfodetail-countryoptions-bank");
            dropdown.empty();
            dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                    (scope.customerDetail.BankInfo.AddressInfo.Country.Id == crmConstants.Enums.Country.NotApply ? ' selected="selected"' : '') +
                    '">' + viewResources.Symbol_NotApply +
                '</option>');
            var countryListLength = scope.countryList.length;
            for (var i = 0; i < countryListLength; i++) {
                var country = scope.countryList[i];
                dropdown.append(
                    '<option' +
                        ' value="' + country.Id + '"' +
                        (scope.customerDetail.BankInfo.AddressInfo.Country.Id == country.Id ? ' selected="selected"' : '') +
                    '>' + country.Name + '</option>');
            }

            dropdown.find('option').click(function () {
                scope.updateSelectedCountryBank($(this).val());
            });
        }
    };

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
}).directive('dropdownLanguage', function () {
    function renderDropdownLanguage(scope, element, attrs) {
        /* jshint unused: vars */
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
                    text: info.Value,
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
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            renderDropdownLanguage(scope, element, attrs);
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                /* jshint unused: vars */
                renderDropdownLanguage(scope, element, attrs);
                scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
            });

            element.on('change', function () {
                scope.$apply(function () {
                    scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                });
            });
        }
    };
});