function BlockUI(msg) {
    if (msg == null || msg == "") {
        jQuery.blockUI({
            message: '<table class="blockUI"><tr><td class="tdloading"></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 100) / 2 + 'px',
                left: ($(window).width() - 100) / 2 + 'px',
                border: 'none',
                width: '100px',
                height: '100px'
            }
        });
    } else {
        jQuery.blockUI({
            message: '<table class="blockUI"><tr><td class="tdloading"></td></tr><tr><td><b>' + msg + '</b></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 136) / 2 + 'px',
                left: ($(window).width() - 136) / 2 + 'px',
                border: 'none',
                width: '100px',
                height: '130px'
            }
        });
    }
}

function UnBlockUI() {
    try {
        if (parent.jQuery && parent.jQuery.unblockUI) {
            parent.jQuery.unblockUI();
        }
        //Called inside a frame set (#centerFr)
        if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
            parent.parent.jQuery.unblockUI();
        }
        if (jQuery.unblockUI) {
            jQuery.unblockUI();
        }
    }
    catch (ex) { }
}


function PTUnBlockUI() {

    if (parent.jQuery && parent.jQuery.unblockUI) {
        parent.jQuery.unblockUI();
    }
    //Called inside a frame set (#centerFr)
    if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
        parent.parent.jQuery.unblockUI();
    }
    //Called from popup
    if (window.opener) {
        if (opener.parent.jQuery.unblockUI) {
            opener.parent.jQuery.unblockUI();
        }
        //parent.jQuery.unblockUI();
    }
}