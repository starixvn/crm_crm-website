"use strict";
/*global showAlertMessage */
/* exported insertFeedbackSubjectTemplate */

function insertFeedbackSubjectTemplate() {
    $('#errMsg').html('');

    $("input[type='button']").attr("disabled", "true");
    if ($("#txtFeedbackTemplateName").val() === "") {
        showAlertMessage("errMsg", "Please Input Feedback Subject Template Content !", 1);
        $("input[type='button']").removeAttr("disabled");
        return;
    }
    if (JSON.stringify($("#txtFeedbackTemplateName").val()).length > 200) {
        showAlertMessage("errMsg", "Max length of subject template is 200 characters", 1);
        $("input[type='button']").removeAttr("disabled");
        return;
    }
    var data = '{"subject":' + JSON.stringify($("#txtFeedbackTemplateName").val()) + '}';


    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/AddSubjectFeedbackTemplate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            if (rs.ErrCode > 0) {
                showAlertMessage("errMsg", rs.ErrMsg, 1);
            }
            else {
                showAlertMessage("errMsg", 'Insert FeedBack Subject Template Successfully', 0);
                parent.afterUpdate(true);
            }
        },
        error: function () { },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
    });
}