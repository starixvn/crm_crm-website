import notificationType, {
    setNotificationTypeGridParams,
    setNotificationTypeGridData,
    setSettings,
    setCreateNotificationTypePopup,
    setCreateNotificationTypeResult,
    setEditNotificationTypePopup,
    setEditNotificationTypeResult,
    setDeleteNotificationTypePopup
} from '../notificationTypeSlice';

describe('notificationType reducer', () => {
    it('should handle initial state', () => {
        expect(notificationType(undefined, {})).toEqual({
            notificationTypeGridParams: {
                pageIndex: 1,
                pageSize: 100
            },
            createNotificationTypePopup: { show: false },
            createNotificationTypeResult: null,
            editNotificationTypePopup: { show: false },
            editNotificationTypeResult: null,
            deleteNotificationTypePopup: { show: false },
            notificationTypeGridData: null,
            settings: {
                ChatTypes: [],
                RecipientTypes: [],
                Systems: []
            }
        });
    });

    it('should handle setNotificationTypeGridParams', () => {
        expect(
            notificationType({}, {
                type: setNotificationTypeGridParams.type,
                payload: {
                    notificationTypeId: 1,
                    notificationTypeName: '',
                    pageIndex: 1,
                    pageSize: 100
                }
            })
        ).toEqual({
            notificationTypeGridParams: {
                notificationTypeId: 1,
                notificationTypeName: '',
                pageIndex: 1,
                pageSize: 100
            }
        });
    });

    it('should handle setNotificationTypeGridData', () => {
        expect(
            notificationType({}, {
                type: setNotificationTypeGridData.type,
                payload: { totalItems: 0, data: [] }
            })
        ).toEqual({
            notificationTypeGridData: { totalItems: 0, data: [] }
        });
    });

    it('should handle setSettings', () => {
        expect(
            notificationType({}, {
                type: setSettings.type,
                payload: [{ ChatTypes: [], RecipientTypes: [], Systems: [] }]
            })
        ).toEqual({
            settings: [{ ChatTypes: [], RecipientTypes: [], Systems: [] }]
        });
    });

    it('should handle setCreateNotificationTypePopup', () => {
        expect(
            notificationType({}, {
                type: setCreateNotificationTypePopup.type,
                payload: { show: false }
            })
        ).toEqual({
            createNotificationTypePopup: { show: false }
        });
    });

    it('should handle setCreateNotificationTypeResult', () => {
        expect(
            notificationType({}, {
                type: setCreateNotificationTypeResult.type,
                payload: { isSuccessful: false, message: '' }
            })
        ).toEqual({
            createNotificationTypeResult: { isSuccessful: false, message: '' }
        });
    });

    it('should handle setEditNotificationTypePopup', () => {
        expect(
            notificationType({}, {
                type: setEditNotificationTypePopup.type,
                payload: { show: false }
            })
        ).toEqual({
            editNotificationTypePopup: { show: false }
        });
    });

    it('should handle setEditNotificationTypeResult', () => {
        expect(
            notificationType({}, {
                type: setEditNotificationTypeResult.type,
                payload: { isSuccessful: false, message: '' }
            })
        ).toEqual({
            editNotificationTypeResult: { isSuccessful: false, message: '' }
        });
    });

    it('should handle setDeleteNotificationTypePopup', () => {
        expect(
            notificationType({}, {
                type: setDeleteNotificationTypePopup.type,
                payload: { show: false }
            })
        ).toEqual({
            deleteNotificationTypePopup: { show: false }
        });
    });
});