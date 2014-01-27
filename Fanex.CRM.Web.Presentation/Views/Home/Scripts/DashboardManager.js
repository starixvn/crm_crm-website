var DashboardManager = function () {
    var self = this;

    self.Enums = {
        SubMenuId: {
            Main: 'liMain',
            Detail: 'liDetail',
            CommunicationLog: 'liCommnunicationLog',
            SpeciaRemark: 'liSpecialRemark',
            Redemption: 'liRedemption'
        }
    };

    self.CachedData = {
        currentSubMenu: null
    };

    self.dashboardMenuClick = function (subMenuId) {
        var customer = homeManager.CachedData.currentCustomer;
        var subMenuIdEnum = self.Enums.SubMenuId;
        self.CachedData.currentSubMenu = subMenuId;
        switch (subMenuId) {
            case subMenuIdEnum.Main:
                menuManager.goToURL('centerFr', window.rootUrl + 'CustomerSettingDetail/Index?' + $.param({
                    customerId: customer.Id
                }));
                break;
            case subMenuIdEnum.Detail:
                menuManager.goToURL('centerFr', window.rootUrl + 'CustomerSettingDetail/Index?' + $.param({
                    customerId: customer.Id
                }));
                break;
            case subMenuIdEnum.CommunicationLog:
                menuManager.goToURL('centerFr', window.rootUrl + 'CommunicationLog/Index?' + $.param({
                    customerId: customer.Id
                }));
                break;
            case subMenuIdEnum.SpeciaRemark:
                menuManager.goToURL('centerFr', window.rootUrl + 'Remark/Index');
                break;
            case subMenuIdEnum.Redemption:
                menuManager.goToURL('centerFr', window.rootUrl + 'Redemption/Index');
                break;
            default:
                break;
        }

        menuManager.setActiveClassForDasboarSubMenu(subMenuId);
    };
};

var dashboardManager = new DashboardManager();