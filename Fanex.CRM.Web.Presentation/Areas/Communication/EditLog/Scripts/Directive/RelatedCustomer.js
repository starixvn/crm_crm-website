editLogApplication.directive('relatedCustomerContainer', function () {
    function bindingRelatedCustomerEvent(scope, element, attrs) {
        var relatedTo = element.find("#relatedTo");
        relatedTo.fcbkcomplete({
            /* jshint camelcase: false */
            json_url: window.rootUrl + 'Customer/RelatedCustomerAutoSuggestion',
            addontab: true,
            maxitems: 30,
            input_min_size: 1,
            height: 10,
            cache: false,
            newel: true
        });

        relatedTo.attr("disabled", null);
        var typeCust = element.find("#typeCust");
        typeCust.val(scope.communicationLog.RelatedCustomers);
        typeCust.removeAttr("disabled");

        element.find(".holder").attr("data-ng-class", "holder");
        element.find(".bit-box").attr("data-ng-class", "bit-box");
        element.find(".closebutton").attr("data-ng-class", "closebutton");
        element.find(".bit-input").attr("data-ng-class", "bit-input");
        element.find(".maininput").attr("data-ng-class", "maininput");
        element.find(".facebook-auto").attr("data-ng-class", "facebook-auto");
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            bindingRelatedCustomerEvent(scope, element, attrs);
        }
    };
});