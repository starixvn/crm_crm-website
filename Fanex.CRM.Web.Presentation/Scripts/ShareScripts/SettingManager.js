"use strict";
var SettingManager = (function ($) {
    return {
        updateSettingSelectList: function (setting, dataList) {
            if (!SettingManager.hasSetting(setting.Id, dataList) && setting.Id !== 0) {
                dataList.push({
                    Id: setting.Id,
                    Name: setting.Name,
                    Value: setting.Name,
                    AliasName: setting.AliasName ? setting.AliasName : ""
                });
            }
        },

        hasSetting: function (settingId, dataList) {
            var length = dataList ? dataList.length : 0;
            for (var i = 0; i < length; i++) {
                if (settingId == dataList[i].Id) {
                    return true;
                }
            }

            return false;
        },

        updateInActiveChildSettings: function (setting, parentList, parentId) {
            var childSettings = SettingManager.getChildSettings(parentId, parentList);

            if (!SettingManager.hasSetting(setting.Id, childSettings)) {
                childSettings.push({
                    Id: setting.Id,
                    Name: setting.Name,
                    Value: setting.Name
                });
            }
        },

        getChildSettings: function (parentId, parentDataList) {
            var length = parentDataList ? parentDataList.length : 0;
            for (var i = 0; i < length; i++) {
                var parent = parentDataList[i];

                if (parentId === parent.Id) {
                    if (!parent.ChildSettings) {
                        parent.ChildSettings = [];
                    }

                    return parent.ChildSettings;
                }
            }
        },

        updateSettingSelectListWithFullData: function (setting, dataList) {
            var length = dataList ? dataList.length : 0;
            var result = [];

            for (var i = 0; i < length; i++) {
                var item = dataList[i];
                if (item.IsActive === true || item.Id === setting.Id) {
                    result.push({
                        Id: item.Id,
                        Name: item.Name,
                        Value: item.Name,
                        AliasName: item.AliasName ? item.AliasName : ""
                    });
                }
            }

            return result;
        }
    }
})();