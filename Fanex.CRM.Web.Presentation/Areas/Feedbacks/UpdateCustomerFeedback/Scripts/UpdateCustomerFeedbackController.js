(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    SettingManager,
    hotKeyManager,
    updateFeedbackResources,
    feedbackId,
    hasReviewFeedbackPermission,
    isShowDeletedStatus,
    isDeletedFeedbackView,
    orgLengt:true */
    /* jshint camelcase: false */
    angular.module('feedbacks-updatecustomerfeedback-application').controller('updateCustomerFeedbackController',
['$scope', 'UpdateCustomerFeedbackService', '$uiBlockerService', 'alertBoxService',
    function ($scope, feedbackService, $uiBlockerService, alertBoxService) {
        $scope.feedback = null;
        $scope.reviewedBy = '';
        $scope.relatedCustomers = [];
        $scope.hasReviewPermission = hasReviewFeedbackPermission === 'True';
        $scope.bettypes = null;
        $scope.subProducts = null;
        $scope.customersInput = '';

        var oldReview;
        $scope.panelManager = {
            customer: false,
            detail: true,
            description: true,
            solution: true,
            review: false,
            showDescriptionPreview: true,
            showSolutionPreview: true,
            showReviewPreview: true
        };
        $scope.requiredManager = {
            subject: true,
            description: true,
            solution: true,
            review: true
        };

        // Subject with auto-complete.
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;

        // Functions of Related customers.
        $scope.verifyUsernames = function (username) {
            if (username !== '') {
                $uiBlockerService.blockUI();
                feedbackService.verifyUsernames({ Username: username })
                .then(function (data) {
                    var customers = [];
                    angular.forEach(data.Result, function (customer, index) {
                        /* jshint unused: false */
                        customers.push({ Id: customer.Id, Name: customer.Name });
                    });

                    $scope.relatedCustomers = $scope.getRelatedCustomers(customers);
                    $scope.customersInput = '';
                  
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
            }
        };

        $scope.getRelatedCustomers = function (customerList) {
            var customers = $scope.relatedCustomers.concat(customerList);
            // unique customer.
            for (var i = 0; i < customers.length; ++i) {
                for (var j = i + 1; j < customers.length; ++j) {
                    if (customers[i].Id === customers[j].Id) {
                        customers.splice(j--, 1);
                    }
                }
            }

            return customers;
        }
        // ---------- End of Functions of Related customers ----------

        // Subject functions.
        $scope.updateSelectedSubject = function (subject) {
            $scope.feedback.Subject = subject;
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            feedbackService.getNumberOfFeedbackSubjectTemplate()
            .then(function (data) {
                var numberOfFeedbackItem = data.Result;
                $scope.feedbackSubjectTemplateList = [];
                var totalPage = Math.round((numberOfFeedbackItem / sharedScriptManager.cachedData.maxFeedbackSubjectTemplateNumberOnLoad) + 0.5);
                $scope.feedbackSubjectTemplateTotalPage = totalPage;
                $scope.isLoadedFeedbackSubjectTemplate = false;
                for (var i = 1; i <= totalPage; i++) {
                    $scope.getFeedbackSubjectTemplateByPagingIndex(i);
                }
            },
            function () {
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
        };
        $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
            feedbackService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
            .then(function (data) {
                var feedbackSubjectTemplate = data.Result;
                if (feedbackSubjectTemplate != null && feedbackSubjectTemplate.length > 0) {
                    $scope.feedbackSubjectTemplateList = $scope.feedbackSubjectTemplateList.concat(feedbackSubjectTemplate);
                }

                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            },
            function () {
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
        };
        $scope.completedLoadFilterInfo = function () {
            if (!$scope.isLoadedFeedbackSubjectTemplate) {
                return;
            }

            $scope.subjectOptions = {
                NumberOfSuggestItems: 15,
                SubjectList: $scope.feedbackSubjectTemplateList,
                Subject: '',
                IsUpdate: true,
                OnChangeEvent: $scope.updateSelectedSubject
            };
        };
        // ---------- End of Subject functions ----------

        // Functions of jHtml.
        $scope.updateDescription = function (description) {
            $scope.descriptionOptions.SelectedText = description;
            $scope.feedback.Description = description;
        };
        $scope.updateSolution = function (solution) {
            $scope.solutionOptions.SelectedText = solution;
            $scope.feedback.Solution = solution;
        };
        $scope.updateReview = function (review) {
            $scope.reviewOptions.SelectedText = review;
            $scope.feedback.Review = review;
        };
        // ---------- End of Functions of jHtml ----------

        $scope.initController = function () {
            $uiBlockerService.blockUI();
            $scope.isSetFocus = true;
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'Feedback_UpdateCustomerFeedback'
            });

            $scope.getNumberOfFeedbackSubjectTemplate();
            feedbackService.getSettings()
            .then(function (data) {
                $scope.settings = data.Result;

                if (!isShowDeletedStatus) {
                    feedbackService.removeDeletedStatus($scope.settings.StatusList);
                }

                feedbackService.getFeedback(feedbackId)
                    .then(function (dataFeedback) {
                        $scope.feedback = dataFeedback.Result;

                        if ($scope.feedback.CustomerInfo.Title.Id === 0) {
                            $scope.feedback.CustomerInfo.Title = $scope.settings.Titles[0];
                        }

                        $scope.feedback.OverrideCRMInfo = false;
                        oldReview = $scope.feedback.Review;
                        $scope.panelManager.review = $scope.feedback.IsReviewed;
                        if ($scope.feedback.ReviewerId > 0) {
                            $scope.panelManager.showReviewPreview = false;
                            $scope.reviewedBy = updateFeedbackResources.ReviewedBy
                                .replace('{0}', $scope.feedback.ReviewerName)
                                .replace('{1}', $scope.feedback.SolveTimeString);
                        }

                        $scope.descriptionOptions = {
                            OnChangeEvent: $scope.updateDescription,
                            SelectedText: $scope.feedback.Description,
                            EditIconText: updateFeedbackResources.InsertEditBlock
                        };
                        $scope.solutionOptions = {
                            OnChangeEvent: $scope.updateSolution,
                            SelectedText: $scope.feedback.Solution,
                            EditIconText: updateFeedbackResources.InsertEditBlock
                        };
                        $scope.reviewOptions = {
                            OnChangeEvent: $scope.updateReview,
                            SelectedText: $scope.feedback.Review,
                            EditIconText: updateFeedbackResources.InsertEditBlock
                        };
                       
                        $scope.updateAllSettingsForFeedback();

                        $scope.getSubProducts();
                        $scope.getBettypes();

                        $scope.getSubcategories();

                        $scope.initRelatedCustomer();                       

                        $uiBlockerService.unblockUI();
                    },
                    function () {
                        $uiBlockerService.unblockUI();
                    });
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

        $scope.getSubProducts = function () {
            angular.forEach($scope.settings.Products, function (product, index) {
                /* jshint unused: false */
                if (product.Id === $scope.feedback.Product.Id) {
                    $scope.subProducts = product.ChildSettings;
                }
            });

            if (!$scope.subProducts) {
                $scope.subProducts = $scope.settings.Products[0].ChildSettings;
            }
        };
        $scope.getBettypes = function () {
            angular.forEach($scope.subProducts, function (subProduct, index) {
                /* jshint unused: false */
                if (subProduct.Id === $scope.feedback.SubProduct.Id) {
                    $scope.bettypes = subProduct.ChildSettings;
                }
            });

            if (!$scope.bettypes) {
                var notApplySetting = $scope.subProducts[0];
                $scope.bettypes = [{ Id: notApplySetting.Id, Name: notApplySetting.Name }];
            }
        };

        $scope.getSubcategories = function () {
            angular.forEach($scope.settings.Categories, function (category, index) {
                /* jshint unused: false */
                if (category.Id === $scope.feedback.Category.Id) {
                    $scope.subcategories = category.ChildSettings;
                }
            });
        };

        $scope.updateAllSettingsForFeedback = function () {
            SettingManager.updateSettingSelectList($scope.feedback.Extension, $scope.settings.Extensions);
            SettingManager.updateSettingSelectList($scope.feedback.Communication, $scope.settings.Communications);

            SettingManager.updateSettingSelectList($scope.feedback.Category, $scope.settings.Categories);
            SettingManager.updateInActiveChildSettings($scope.feedback.SubCategory, $scope.settings.Categories, $scope.feedback.Category.Id);

            $scope.updateProductSettings();

            SettingManager.updateSettingSelectList($scope.feedback.Language, $scope.settings.Languages);
            SettingManager.updateSettingSelectList($scope.feedback.Status, $scope.settings.StatusList);
            SettingManager.updateSettingSelectList($scope.feedback.Role, $scope.settings.Roles);
            SettingManager.updateSettingSelectList($scope.feedback.Response, $scope.settings.Responses);

            SettingManager.updateSettingSelectList($scope.feedback.CustomerInfo.Title, $scope.settings.Titles);
            SettingManager.updateSettingSelectList($scope.feedback.CustomerInfo.LicenseeType, $scope.settings.LicenseeTypes);
            SettingManager.updateSettingSelectList($scope.feedback.CustomerInfo.SiteSetting, $scope.settings.Sites);
        };

        $scope.updateProductSettings = function () {
            var product = $scope.feedback.Product;
            SettingManager.updateSettingSelectList(product, $scope.settings.Products);
            SettingManager.updateInActiveChildSettings($scope.feedback.SubProduct, $scope.settings.Products, product.Id);
            $scope.subProducts = SettingManager.getChildSettings(product.Id, $scope.settings.Products);
            SettingManager.updateInActiveChildSettings($scope.feedback.BetType, $scope.subProducts, $scope.feedback.SubProduct.Id);
        };
       
        $scope.categoryChanged = function () {
            $scope.subcategories = $scope.feedback.Category.ChildSettings;
            if ($scope.subcategories.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoSubcategoryError.replace('{0}', '[' + $scope.feedback.Category.Name + ']'));
            }
            else {
                $scope.hideMessage();
                $scope.feedback.SubCategory = $scope.subcategories[0];
            }
        };

        $scope.updateSelectedSite = function () {
            $scope.feedback.CustomerInfo.SiteSetting = $scope.feedback.CustomerInfo.SiteSetting || { Id: 0 };
        };

        $scope.productChanged = function () {
            $scope.subProducts = SettingManager.getChildSettings($scope.feedback.Product.Id, $scope.settings.Products);
            $scope.feedback.SubProduct = $scope.subProducts[0];
            $scope.subProductChanged();
        };

        $scope.subProductChanged = function () {
            var childSettings = SettingManager.getChildSettings($scope.feedback.SubProduct.Id, $scope.subProducts);
            if (!childSettings || childSettings.length === 0) {
                var notApplySetting = $scope.subProducts[0];
                childSettings = [{ Id: notApplySetting.Id, Name: notApplySetting.Name }];
            }

            $scope.bettypes = childSettings;

            if ($scope.bettypes.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoBettypeError.replace('{0}', '[' + $scope.feedback.Product.Value + ']'));
            }
            else {
                $scope.hideMessage();
                $scope.feedback.BetType = $scope.bettypes[0];
            }
        };

        $scope.validateFeedback = function () {
            $scope.hideMessage();

            var feedback = $scope.feedback;
            var feedbackTime = new Date(feedback.FeedbackTimeString);
            var resolutionTime = new Date(feedback.ResolutionTimeString);
            if (feedbackTime > resolutionTime) {
                $scope.errorMessage(updateFeedbackResources.ErrorFeedbackResolutionTime);
                return false;
            }

            if (feedback.Communication === $scope.settings.Communications[0]) {
                $scope.errorMessage(updateFeedbackResources.NACommmunicationTool);
                return false;
            }

            var category = feedback.Category;
            if (category.ChildSettings && category.ChildSettings.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoSubcategoryError.replace('{0}', '[' + category.Name + ']'));
                return false;
            }

            var product = feedback.Product;
            if (product.ChildSettings && product.ChildSettings.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoBettypeError.replace('{0}', '[' + product.Value + ']'));
                return false;
            }

            if (feedback.Language === $scope.settings.Languages[0]) {
                $scope.errorMessage(updateFeedbackResources.NALanguage);
                return false;
            }

            $scope.requiredManager.subject = feedback.Subject.trim().length !== 0;
            if (!$scope.requiredManager.subject) {
                $scope.errorMessage(updateFeedbackResources.EmptySubject);
                return false;
            }

            $scope.requiredManager.description = feedback.Description.trim().length !== 0 && feedback.Description !== '<br>';
            if (!$scope.requiredManager.description) {
                $scope.errorMessage(updateFeedbackResources.EmptyRemark);
                return false;
            }

            $scope.requiredManager.solution = feedback.Solution.trim().length !== 0 && feedback.Solution !== '<br>';
            if (!$scope.requiredManager.solution) {
                $scope.errorMessage(updateFeedbackResources.EmptySolution);
                return false;
            }

            $scope.requiredManager.review = !(feedback.IsReviewed && (feedback.Review.trim().length === 0 || feedback.Review === '<br>'));
            if (!$scope.requiredManager.review) {
                $scope.errorMessage(updateFeedbackResources.EmptyReview);
                return false;
            }

            return true;
        };

        $scope.updateFeedback = function () {
            if (!$scope.validateFeedback()) {
                return;
            }

            var feedback = $scope.feedback;
            if (!feedback.OverrideCRMInfo) {
                $scope.updateCustomerFeedback(feedback);
                return;
            }

            parent.homeManager.showConfirmOverrideCRMInfo(updateFeedbackResources.ConfirmOverrideCRMInfo, function (bool) {
                if (bool) { $scope.updateCustomerFeedback(feedback); }
            });
        };

        $scope.updateCustomerFeedback = function (feedback) {
            $uiBlockerService.blockUI();
            if (feedback.HasRelated) {
                if ($scope.relatedCustomers.length > 0) {
                    var relatedCustomerIds = [];
                    angular.forEach($scope.relatedCustomers, function (customer, index) {
                        /* jshint unused: false */
                        relatedCustomerIds.push(customer.Id);
                    });

                    feedback.RelatedCustomer = relatedCustomerIds.join(',');
                }
                else {
                    feedback.RelatedCustomer = null;
                }
            }

            feedback.UpdateReviewMode = !$scope.hasReviewPermission ? 0 : (!feedback.IsReviewed ? 2 : (oldReview === feedback.Review ? 0 : 1));
            $scope.assignIdForParam(feedback);
            feedbackService.updateFeedback(feedback, isDeletedFeedbackView)
                .then(function (data) {
                    var result = data.Result;
                    if (result && result.Code === 0) {
                        alertBoxService.show({
                            type: crmConstants.Enums.MessageType.Success,
                            message: updateFeedbackResources.UpdateFeedbackSuccessfully,
                            callback: function () {
                                parent.ClosePopup();
                                if (feedback.OverrideCRMInfo) {
                                    parent.leftPanelManager.loadLeftPanel(feedback.CustomerInfo.Name, true);
                                }
                            }
                        });
                    }
                    else {
                        $scope.errorMessage(result.Code ? result.Message : 'Oppss... An error occurred while processing your request.');
                    }

                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.assignIdForParam = function (feedback) {
            feedback.Product = { Id: feedback.Product.Id };
            feedback.SubProduct = { Id: feedback.SubProduct.Id };
        };

        $scope.errorMessage = function (error) {
            alertBoxService.show({
                type: crmConstants.Enums.MessageType.Error,
                message: error
            });
        };

        $scope.hideMessage = function () {
            alertBoxService.hide();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.updateFeedback();
        };

        $scope.invokeHotKey_CtrlE = function () {
            if (!$scope.feedback.HasRelated) {
                return;
            }

            $scope.verifyUsernames($scope.customersInput);
        };

        $scope.initRelatedCustomer = function () {
            if ($scope.feedback.HasRelated) {                
                $scope.verifyUsernames($scope.feedback.RelatedCustomer);
            }
        };

        angular.element(document).ready(function () {
            sharedScriptManager.safeApply($scope);
            $scope.initController();
        });
    } ]);
})();