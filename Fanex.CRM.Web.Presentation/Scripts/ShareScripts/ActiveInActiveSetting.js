var activeInActiveSetting;
var ActiveInActiveSetting = function () {
    var self = this;

    self.getActiveOrInactiveSettings = function (feedback, allSettings, settingName, name) {
        var feedbackSettingId = [];
        var inactive = [];
        angular.forEach(feedback, function (setting) {
            switch (name) {
                case "Communication":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv1);
                    }
                    break;
                case "Station":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv1);
                    }
                    break;
                case "Category":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv1);
                    }
                    break;
                case "SubCategory":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv2);
                    }
                    break;
                case "Product":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv1);
                    }
                    break;
                case "SubProduct":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv2);
                    }
                    break;
                case "Bettype":
                    if (settingName === setting.SettingType) {
                        feedbackSettingId.push(setting.SettingIdLv3);
                    }
                    break;
                default:
                    feedbackSettingId;
            }
        });
        var distinctFeedbackIds = feedbackSettingId.filter(function (value, index) { return feedbackSettingId.indexOf(value) === index });
        allSettings.map(function (x) {
            if (!x.IsActive) {
                inactive.push(x.Id);
            }
        });
        var activeSettings = allSettings.filter(function (item) {
            return item.IsActive;
        });

        var isActive = true;
        for (var i = 0; i < inactive.length; i++) {
            if (distinctFeedbackIds.indexOf(inactive[i]) !== -1) {
                isActive = false;
                break;
            }
        }

        if (isActive) {
            return activeSettings;
        }
        return allSettings;
    };

    self.getActiveOrInactiveSettingsDetail = function (settingId, allSettings) {
        var inactive = [];
        allSettings.map(function (x) {
            if (!x.IsActive) {
                inactive.push(x.Id);
            }
        });
        var activeSettingsDetails = allSettings.filter(function (item) {
            return item.IsActive;
        });

        var isActive = inactive.indexOf(settingId) !== -1;

        if (isActive) {
            return activeSettingsDetails;
        }
        return allSettings;
    };

    self.getActiveSettingRelation = function (feedback, functionName, settingRelations, settingName) {
        var settingRelationId = [];
        var inactiveRelation = [];
        var setting = feedback;
        var role = functionName === "editLog" ? "RoleInfo" : "Role";
        switch (settingName) {
            case "Response":
                settingRelationId.push(setting["Response"].Id);
                break;
            case role:
                settingRelationId.push(setting["Role"].Id);
                break;
            case "Country":
                settingRelationId.push(setting["Country"].Id);
                break;
            case "Language":
                settingRelationId.push(setting["Language"].Id);
                break;
            default:
                settingRelationId;
        }
        var distinctRelationIds = settingRelationId.filter(function (value, index) { return settingRelationId.indexOf(value) === index });
        settingRelations.map(function (x) {
            if (!x.IsActive) {
                inactiveRelation.push(x.Id);
            }
        });
        var activeRelationSettings = settingRelations.filter(function (item) {
            return item.IsActive;
        });

        var isActive = true;
        for (var i = 0; i < inactiveRelation.length; i++) {
            if (distinctRelationIds.indexOf(inactiveRelation[i]) !== -1) {
                isActive = false;
                break;
            }
        }

        if (isActive) {
            return activeRelationSettings;
        }
        return settingRelations;
    };
};

activeInActiveSetting = new ActiveInActiveSetting();