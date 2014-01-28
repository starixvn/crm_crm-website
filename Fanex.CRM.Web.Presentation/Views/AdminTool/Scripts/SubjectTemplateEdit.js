"use strict";
/*global gup, showAlertMessage, head */
/* exported udpFeedbackSubjectTemplate */

function onloadSubjectTemplateEdit() {
    var templateId = gup('Id');
    if (templateId.length > 0) {
        $('#txtTemplateID').html(templateId);
        $('#txtTemplateID').attr('readonly', true);
    }
    try { parent.BlockUI(); } catch (e) { }
    var data = '{"templateId":' + templateId + '}';
    //var subject = $('#templateHiddenId' + Id).text();
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetFeedbackSubjectTemplateItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            $('#txtTemplateContent').val(rs.Subject);
        },
        error: function () { },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
    });
}

function udpFeedbackSubjectTemplate() {
    $('#errMsg').html('');
    if ($('#txtTemplateContent').val() === "") {
        showAlertMessage("errMsg", "Please Input Feedback Subject Template Content!", 1);
        $("input[type='button']").removeAttr("disabled");
        return;
    }

    $("input[type='button']").attr("disabled", "true");
    if (JSON.stringify($("#txtTemplateContent").val()).length > 200) {
        showAlertMessage("errMsg", "Max length of subject template is 200 characters!", 1);
        $("input[type='button']").removeAttr("disabled");
        return;
    }
    try { parent.BlockUI(); } catch (e) { }
    var data = '{"templateId":"' + $('#txtTemplateID').text() + '", "newSubject":' + JSON.stringify($('#txtTemplateContent').val()) + '}';

    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/EditSubjectFeedbackTemplate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            if (rs.ErrCode > 0) {
                showAlertMessage("errMsg", rs.ErrMsg, 1);
            }
            else {
                showAlertMessage("errMsg", 'Update Feedback Subject Template successfully!', 0);
                parent.afterUpdate(true);
            }
            try {
                parent.jQuery.unblockUI();
            }
            catch (e) { }
        }
    });
}

head.ready(function () {
    onloadSubjectTemplateEdit();
});