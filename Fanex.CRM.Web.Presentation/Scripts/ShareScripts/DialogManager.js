"use strict";
var DialogManager = function () {
	var self = this;

	self.CachedData = {
		resultPopup: null
	};

	self.initPage = function () {
		self.CachedData.resultPopup = $('#div-resultpopup');
	};

	self.alert = function (title, message, okButton, closeCallback) {
		self.CachedData.resultPopup.removeClass('display-none');
		self.CachedData.resultPopup.html(message);
		self.CachedData.resultPopup.dialog({
			zIndex: 500,
			resizable: false,
			modal: true,
			// position: 'center',
			title: title,
			close: function (event, ui) {
				/* jshint unused: false */
				self.CachedData.resultPopup.dialog('destroy');
				$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
				self.CachedData.resultPopup = $('#div-resultpopup');
				if (closeCallback && typeof (closeCallback) === 'function') {
					closeCallback();
				}
			},
			buttons: [{
				text: okButton,
				click: function () {
					self.CachedData.resultPopup.dialog('close');
				}
			}]
		});
	};

	self.confirm = function (title, message, okCallback, cancelCallback, closeCallback) {
		self.CachedData.resultPopup.removeClass('display-none');
		self.CachedData.resultPopup.html(message);
		self.CachedData.resultPopup.dialog({
			zIndex: 500,
			resizable: false,
			modal: true,
			title: title,
			close: function (event, ui) {
				self.CachedData.resultPopup.dialog('destroy');
				$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
				self.CachedData.resultPopup = $('#div-resultpopup');
				if (closeCallback && typeof (closeCallback) === 'function') {
					closeCallback();
				}
			},
			buttons: [
				{
					text: "OK",
					click: function () {
						self.CachedData.resultPopup.dialog('close');
						if (okCallback && typeof (okCallback) === 'function') {
							okCallback();
						}
					}
				},
				{
					text: "Cancel",
					click: function () {
						self.CachedData.resultPopup.dialog('close');
						if (cancelCallback && typeof (cancelCallback) === 'function') {
							cancelCallback();
						}
					}
				}
			]
		});
	};
};

var dialogManager = new DialogManager();
$(document).ready(function () {
	dialogManager.initPage();
});