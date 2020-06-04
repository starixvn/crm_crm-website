(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    template */

    angular.module('pic-management-application')
        .controller('changePicStateController', ['$uiBlockerService', 'changePicStateService', function ($uiBlockerService, changePicStateService) {
            let vm = this;
            vm.messageResultOptions = null;
            vm.initController = function () {
                sharedScriptManager.safeApply(vm);
            };

            vm.changeState = function () {
                $uiBlockerService.blockUI();
                changePicStateService.changePicState({ PicId: item.PicId, IsActive: item.IsActive })
                    .then(function (data) {
                        vm.showResult(data.Result);
                        $uiBlockerService.unblockUI();
                    },
                        function () {
                            $uiBlockerService.unblockUI();
                        });
            };

            vm.showResult = function (result) {
                if (result.Code !== 0) {
                    vm.messageResultOptions = new MessageResult(result.Message, true, false);
                    return;
                }

                vm.messageResultOptions = new MessageResult(result.Message, true, true);
                parent.homeManager.CachedData.shouldReloadView = true;
                setTimeout(function () {
                    vm.messageResultOptions = new MessageResult('', false, false);
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            };
            vm.cancelChangeState = function () {
                parent.ClosePopup();
            };
            angular.element(document).ready(function () {
                vm.initController();
            });
        }]);
})();