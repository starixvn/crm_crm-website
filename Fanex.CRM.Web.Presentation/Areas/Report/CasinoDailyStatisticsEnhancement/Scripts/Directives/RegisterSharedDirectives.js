function registerPickerForCasinoDailyEnhancement() {
    var casinoDailyEnhancementRegister = new RegisterTemplate(casinoDailyEnhancementApplication);

    casinoDailyEnhancementRegister.registerDatePicker();
    casinoDailyEnhancementRegister.registerDownloadFile();
    casinoDailyEnhancementRegister.registerProductMultiSelectPicker();
    casinoDailyEnhancementRegister.registerCommunicationToolPicker(true);
    casinoDailyEnhancementRegister.registerCategoryMultiSelectPicker();
}

registerPickerForCasinoDailyEnhancement();