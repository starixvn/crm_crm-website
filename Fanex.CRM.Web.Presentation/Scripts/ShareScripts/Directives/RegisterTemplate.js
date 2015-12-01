"use strict";
/* global directiveManager */
/* exported RegisterTemplate */
function RegisterTemplate(application) {
    var self = this;
    self.application = application;
    self.registerReportTypePicker = function () {
        var reportTypeRegisterParams = {
            PlugInName: 'reportTypeContainer',
            OptionName: 'reportTypeOptions',
            PickerId: 'reporttype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, reportTypeRegisterParams);
    };

    self.registerReportByPicker = function () {
        var reportByRegisterParams = {
            PlugInName: 'reportByPickerContainer',
            OptionName: 'reportByOptions',
            PickerId: 'reportby',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, reportByRegisterParams);
    };

    self.registerCurrencyPicker = function () {
        var currencyRegisterParams = {
            PlugInName: 'currencyPickerContainer',
            OptionName: 'currencyOptions',
            PickerId: 'currency',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, currencyRegisterParams);
    };

    self.registerDatePicker = function () {
        var fromDateRegisterParams = {
            PlugInName: 'datePickerFromDateContainer',
            OptionName: 'fromDateOptions',
            PickerId: 'fromDate',
            RestrictType: 'C'
        };
        var toDateRegisterParams = {
            PlugInName: 'datePickerToDateContainer',
            OptionName: 'toDateOptions',
            PickerId: 'toDate',
            RestrictType: 'C'
        };
        directiveManager.registerDatePickerContainer(self.application, fromDateRegisterParams);
        directiveManager.registerDatePickerContainer(self.application, toDateRegisterParams);
    };
    self.registerWeekPicker = function () {
        var fromWeekRegisterParams = {
            PlugInName: 'weekPickerFromWeekContainer',
            OptionName: 'fromWeekOptions',
            PickerId: 'fromweek',
            RestrictType: 'C'
        };
        var toWeekRegisterParams = {
            PlugInName: 'weekPickerToWeekContainer',
            OptionName: 'toWeekOptions',
            PickerId: 'toweek',
            RestrictType: 'C'
        };

        directiveManager.registerWeekPickerContainer(self.application, fromWeekRegisterParams);
        directiveManager.registerWeekPickerContainer(self.application, toWeekRegisterParams);
    };
    self.registerMonthPicker = function () {
        var fromMonthRegisterParams = {
            PlugInName: 'monthPickerFromMonthContainer',
            OptionName: 'fromMonthOptions',
            PickerId: {
                Month: 'frommonth_month',
                Year: 'frommonth_year'
            },
            RestrictType: 'C'
        };

        var toMonthRegisterParams = {
            PlugInName: 'monthPickerToMonthContainer',
            OptionName: 'toMonthOptions',
            PickerId: {
                Month: 'tomonth_month',
                Year: 'tomonth_year'
            },
            RestrictType: 'C'
        };

        directiveManager.registerMonthPickerContainer(self.application, fromMonthRegisterParams);
        directiveManager.registerMonthPickerContainer(self.application, toMonthRegisterParams);
    };
    self.registerYearPicker = function () {
        var fromYearRegisterParams = {
            PlugInName: 'yearPickerFromDateContainer',
            OptionName: 'fromYearOptions',
            PickerId: 'fromdate_year',
            RestrictType: 'C'
        };

        var toYearRegisterParams = {
            PlugInName: 'yearPickerToDateContainer',
            OptionName: 'toYearOptions',
            PickerId: 'todate_year',
            RestrictType: 'C'
        };

        directiveManager.registerYearPickerContainer(self.application, fromYearRegisterParams);
        directiveManager.registerYearPickerContainer(self.application, toYearRegisterParams);
    };
    self.registerProductPicker = function () {
        var productRegisterParams = {
            PlugInName: 'productPickerContainer',
            OptionName: 'productOptions',
            PickerId: 'product',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, productRegisterParams);
    };
    self.registerSitePicker = function () {
        var siteRegisterParams = {
            PlugInName: 'sitePickerContainer',
            OptionName: 'siteOptions',
            PickerId: 'site',
            RestrictType: 'C'
        };

        directiveManager.registerDropdownSiteContainer(self.application, siteRegisterParams);
    };
    self.registerStatusPicker = function () {
        var statusRegisterParams = {
            PlugInName: 'statusPickerContainer',
            OptionName: 'statusOptions',
            PickerId: 'status',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, statusRegisterParams);
    };
    self.registerResponsePicker = function () {
        var responseRegisterParams = {
            PlugInName: 'responsePickerContainer',
            OptionName: 'responseOptions',
            PickerId: 'response',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, responseRegisterParams);
    };
    self.registerRolePicker = function () {
        var roleRegisterParams = {
            PlugInName: 'rolePickerContainer',
            OptionName: 'roleOptions',
            PickerId: 'role',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, roleRegisterParams);
    };
    self.registerCommunicationToolPicker = function () {
        var communicationToolRegisterParams = {
            PlugInName: 'communicationToolPickerContainer',
            OptionName: 'communicationToolOptions',
            PickerId: 'communication-tool',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, communicationToolRegisterParams);
    };
    self.registerMainCategoryPicker = function () {
        var mainCategoryRegisterParams = {
            PlugInName: 'mainCategoryPickerContainer',
            OptionName: 'mainCategoryOptions',
            PickerId: 'main-category',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, mainCategoryRegisterParams);
    };
    self.registerSubCategoryPicker = function () {
        var subCategoryRegisterParams = {
            PlugInName: 'subCategoryPickerContainer',
            OptionName: 'subCategoryOptions',
            PickerId: 'sub-category',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, subCategoryRegisterParams);
    };
    self.registerExtensionPicker = function () {
        var extensionRegisterParams = {
            PlugInName: 'extensionPickerContainer',
            OptionName: 'extensionOptions',
            PickerId: 'extension',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, extensionRegisterParams);
    };
    self.registerDefaultFocus = function () {
        var defaultFocusRegisterParams = {
            PlugInName: 'defaultFocus',
            OptionName: 'defaultFocusOptions',
            PickerId: null,
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDefaultFocus(self.application, defaultFocusRegisterParams);
    };
    self.registerDownloadFile = function () {
        var downloadFileRegisterParams = {
            PlugInName: 'downloadFileContainer',
            OptionName: 'downloadFileOptions',
            RestrictType: 'C'
        };

        directiveManager.registerDownloadFile(self.application, downloadFileRegisterParams);
    };

    self.registerTitlePicker = function () {
        var titleRegisterParams = {
            PlugInName: 'titlePickerContainer',
            OptionName: 'titleOptions',
            PickerId: 'title',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, titleRegisterParams);
    };
    self.registerCustomerTypePicker = function () {
        var customerTypeRegisterParams = {
            PlugInName: 'customerTypePickerContainer',
            OptionName: 'customerTypeOptions',
            PickerId: 'customertype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, customerTypeRegisterParams);
    };
    self.registerVIPTypePicker = function () {
        var vipTypeRegisterParams = {
            PlugInName: 'vipTypePickerContainer',
            OptionName: 'vipTypeOptions',
            PickerId: 'viptype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, vipTypeRegisterParams);
    };

    self.registerHeadlineTypePicker = function () {
        var headlineTypeRegisterParams = {
            PlugInName: 'headlineTypePickerContainer',
            OptionName: 'headlineTypeOptions',
            PickerId: 'headlineType',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, headlineTypeRegisterParams);
    };

    self.registerLanguageMultiSelectPicker = function () {
        var languageRegisterParams = {
            PlugInName: 'languagePickerContainer',
            OptionName: 'languageOptions',
            PickerId: 'language',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, languageRegisterParams);
    };
    self.registerSiteMultiSelectPicker = function () {
        var siteRegisterParams = {
            PlugInName: 'sitePickerContainer',
            OptionName: 'siteOptions',
            PickerId: 'site',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, siteRegisterParams);
    };
    self.registerCountryMultiSelectPicker = function () {
        var countryRegisterParams = {
            PlugInName: 'countryPickerContainer',
            OptionName: 'countryOptions',
            PickerId: 'country',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, countryRegisterParams);
    };
    self.registerProductMultiSelectPicker = function () {
        var productRegisterParams = {
            PlugInName: 'productPickerContainer',
            OptionName: 'productOptions',
            PickerId: 'product',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, productRegisterParams);
    };

    self.registerToggleElement = function () {
        var countryRegisterParams = {
            PlugInName: 'toggleElement',
            OptionName: 'toggleElementOptions',
            PickerId: null,
            RestrictType: 'C'
        };

        directiveManager.registerToggleElement(self.application, countryRegisterParams);
    };

    self.registerSubjectAutoComplete = function () {
        var subjectAutoCompleteRegisterParams = {
            PlugInName: 'subjectAutoCompleteContainer',
            OptionName: 'subjectOptions',
            PickerId: "subject",
            RestrictType: 'C'
        };

        directiveManager.registerSubjectAutoComplete(self.application, subjectAutoCompleteRegisterParams);
    };

    self.registerCategoryMultiSelectPicker = function () {
        var categoryRegisterParams = {
            PlugInName: 'categoryPickerContainer',
            OptionName: 'categoryOptions',
            PickerId: 'category',
            RestrictType: 'A',
            DataPropertyName: {
                Id: 'Id',
                Name: 'Name'
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, categoryRegisterParams);
    };

    self.registerStatusMultiSelectPicker = function () {
        var statusRegisterParams = {
            PlugInName: 'statusMultiContainer',
            OptionName: 'statusOptions',
            PickerId: 'status',
            RestrictType: 'A',
            DataPropertyName: {
                Id: 'Id',
                Name: 'Name'
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, statusRegisterParams);
    };

    self.registerPositionMultiSelectPicker = function () {
        var positionRegisterParams = {
            PlugInName: 'positionMultiContainer',
            OptionName: 'positionOptions',
            PickerId: 'position',
            RestrictType: 'A',
            DataPropertyName: {
                Id: 'Id',
                Name: 'Name'
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, positionRegisterParams);
    };

    self.registerCurrencyMultiSelectPicker = function () {
        var currencyRegisterParams = {
            PlugInName: 'currencyMultiContainer',
            OptionName: 'currencyOptions',
            PickerId: 'currency',
            RestrictType: 'A',
            DataPropertyName: {
                Id: 'Id',
                Name: 'Name'
            }
        };

        directiveManager.registerMultiSelectContainer(self.application, currencyRegisterParams);
    };

    self.registerLanguagePicker = function () {
        var languageRegisterParams = {
            PlugInName: 'languagePickerContainer',
            OptionName: 'languageOptions',
            PickerId: 'language',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, languageRegisterParams);
    };
    self.registerBetTypePicker = function () {
        var betTypeRegisterParams = {
            PlugInName: 'betTypePickerContainer',
            OptionName: 'betTypeOptions',
            PickerId: 'bettype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, betTypeRegisterParams);
    };
    self.registerLicenseeTypePicker = function () {
        var licenseeTypeRegisterParams = {
            PlugInName: 'licenseeTypePickerContainer',
            OptionName: 'licenseeTypeOptions',
            PickerId: 'licensee-type',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, licenseeTypeRegisterParams);
    };
    self.registerCountryPicker = function () {
        var betTypeRegisterParams = {
            PlugInName: 'countryPickerContainer',
            OptionName: 'countryOptions',
            PickerId: 'country',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.application, betTypeRegisterParams);
    };
    self.registerDescriptionHtml = function () {
        var descriptionRegisterParams = {
            PlugInName: 'descriptionEditorContainer',
            OptionName: 'descriptionOptions',
            PickerId: 'descriptioneditorbox',
            RestrictType: 'C'
        };

        directiveManager.registerHtmlArea(self.application, descriptionRegisterParams);
    };
    self.registerSolutionHtml = function () {
        var descriptionRegisterParams = {
            PlugInName: 'solutionEditorContainer',
            OptionName: 'solutionOptions',
            PickerId: 'solutioneditorbox',
            RestrictType: 'C'
        };

        directiveManager.registerHtmlArea(self.application, descriptionRegisterParams);
    };
    self.registerReviewHtml = function () {
        var reviewRegisterParams = {
            PlugInName: 'reviewEditorContainer',
            OptionName: 'reviewOptions',
            PickerId: 'revieweditorbox',
            RestrictType: 'C'
        };

        directiveManager.registerHtmlArea(self.application, reviewRegisterParams);
    };
    self.registerRawHtmlDirective = function () {
        var rawHtmlDirectiveRegisterParams = {
            RestrictType: 'C'
        };

        directiveManager.registerRawHtmlDirective(self.application, rawHtmlDirectiveRegisterParams);
    };
    self.registerTipsyDirective = function () {
        var tipsyDirectiveRegisterParams = {
            OptionName: 'tipsyOptions',
            RestrictType: 'C'
        };

        directiveManager.registerTipsyDirective(self.application, tipsyDirectiveRegisterParams);
    };
    self.registerRelatedCustomerDirective = function () {
        var relatedCustomerDirectiveRegisterParams = {
            PlugInName: 'relatedCustomerDirectiveContainer',
            PickerId: 'txtrelatedcustomer',
            OptionName: 'relatedCustomerOptions',
            RestrictType: 'C'
        };

        directiveManager.registerRelatedCustomerDirective(self.application, relatedCustomerDirectiveRegisterParams);
    };
}
