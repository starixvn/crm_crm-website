import notificationTemplate, {
    setNotificationTypes,
    setLanguages,
    setGridParams,
    setGridData,
    setSelectedTemplate,
    setCreateTemplatePopup,
    setCreateTemplateResult,
    setEditTemplatePopup,
    setEditTemplateResult,
    setDeleteTemplatePopup,
    setDeleteTemplateResult,
    setContentDetailPopup
} from '../notificationTemplateSlice';

describe('notificationTemplate reducer', () => {
    it('should handle initial state', () => {
        expect(notificationTemplate(undefined, {})).toEqual({
            notificationTypes: [],
            languages: [],
            gridParams: {
                notificationTypeId: '',
                notificationTemplateName: '',
                pageIndex: 1,
                pageSize: 100
            },
            gridData: null,
            selectedTemplate: null,
            createTemplatePopup: { show: false },
            createTemplateResult: null,
            editTemplatePopup: { show: false },
            editTemplateResult: null,
            deleteTemplatePopup: { show: false },
            deleteTemplateResult: null,
            contentDetailPopup: { show: false }
        });
    });

    it('should handle setNotificationTypes', () => {
        expect(
            notificationTemplate({}, {
                type: setNotificationTypes.type,
                payload: [{ id: 1, type: 'tests' }]
            })
        ).toEqual({
            notificationTypes: [{ id: 1, type: 'tests' }]
        });
    });

    it('should handle setLanguages', () => {
        expect(
            notificationTemplate({}, {
                type: setLanguages.type,
                payload: [{ id: 1, language: 'english' }]
            })
        ).toEqual({
            languages: [{ id: 1, language: 'english' }]
        });
    });

    it('should handle setGridParams', () => {
        expect(
            notificationTemplate({}, {
                type: setGridParams.type,
                payload: {
                    notificationTypeId: 1,
                    notificationTemplateName: '',
                    pageIndex: 1,
                    pageSize: 100
                }
            })
        ).toEqual({
            gridParams: {
                notificationTypeId: 1,
                notificationTemplateName: '',
                pageIndex: 1,
                pageSize: 100
            }
        });
    });

    it('should handle setGridData', () => {
        expect(
            notificationTemplate({}, {
                type: setGridData.type,
                payload: { totalItems: 0, data: [] }
            })
        ).toEqual({
            gridData: { totalItems: 0, data: [] }
        });
    });

    it('should handle setSelectedTemplate', () => {
        expect(
            notificationTemplate({}, {
                type: setSelectedTemplate.type,
                payload: { id: 1, name: 'template' }
            })
        ).toEqual({
            selectedTemplate: { id: 1, name: 'template' }
        });
    });

    it('should handle setCreateTemplatePopup', () => {
        expect(
            notificationTemplate({}, {
                type: setCreateTemplatePopup.type,
                payload: { show: false }
            })
        ).toEqual({
            createTemplatePopup: { show: false }
        });
    });

    it('should handle setCreateTemplateResult', () => {
        expect(
            notificationTemplate({}, {
                type: setCreateTemplateResult.type,
                payload: { isSuccessful: false, message: '' }
            })
        ).toEqual({
            createTemplateResult: { isSuccessful: false, message: '' }
        });
    });

    it('should handle setEditTemplatePopup', () => {
        expect(
            notificationTemplate({}, {
                type: setEditTemplatePopup.type,
                payload: { show: false }
            })
        ).toEqual({
            editTemplatePopup: { show: false }
        });
    });

    it('should handle setEditTemplateResult', () => {
        expect(
            notificationTemplate({}, {
                type: setEditTemplateResult.type,
                payload: { isSuccessful: false, message: '' }
            })
        ).toEqual({
            editTemplateResult: { isSuccessful: false, message: '' }
        });
    });

    it('should handle setDeleteTemplatePopup', () => {
        expect(
            notificationTemplate({}, {
                type: setDeleteTemplatePopup.type,
                payload: { show: false }
            })
        ).toEqual({
            deleteTemplatePopup: { show: false }
        });
    });

    it('should handle setDeleteTemplateResult', () => {
        expect(
            notificationTemplate({}, {
                type: setDeleteTemplateResult.type,
                payload: { isSuccessful: false, message: '' }
            })
        ).toEqual({
            deleteTemplateResult: { isSuccessful: false, message: '' }
        });
    });

    it('should handle setContentDetailPopup', () => {
        expect(
            notificationTemplate({}, {
                type: setContentDetailPopup.type,
                payload: { show: false }
            })
        ).toEqual({
            contentDetailPopup: { show: false }
        });
    });
});