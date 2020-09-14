import React from 'react';
import { toast } from 'react-toastify';
import { shallow } from 'enzyme';
import { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import NewNotificationTemplate from '../NewNotificationTemplate';
import EditNotificationTemplate from '../EditNotificationTemplate';
import ContentDetailModalDialog from '../ContentDetailModalDialog';
import SweetModalDialog from '@jsroot/components/SweetModalDialog';

jest.mock('react-toastify');

describe('AppContainer', () => {
    afterAll(() => {
        jest.clearAllMocks();
    });

    it('should render correctly', () => {
        // Act
        const container = shallow(<AppContainer />);

        // Assert
        expect(container).toMatchSnapshot();
        expect(container.find(Filter).length).toBe(1);
        expect(container.find(DataGrid).length).toBe(1);
    });

    describe('create notification template', () => {
        it('should render correctly', () => {
            // Arrange
            const props = {
                createTemplatePopup: { show: true }
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(NewNotificationTemplate).length).toBe(1);
        });

        it('should call exactly functions when close popup', () => {
            // Arrange
            const props = {
                setCreateTemplatePopup: jest.fn(),
                setCreateTemplateResult: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseCreatePopup();

            // Assert
            expect(props.setCreateTemplatePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setCreateTemplateResult).toHaveBeenCalledWith(null);
        });

        it('should call exactly functions when create template successful', () => {
            // Arrange
            const props = {
                gridParams: {},
                getGridData: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseCreatePopup = jest.fn();

            // Act
            container.handleCreateSuccess();

            // Assert
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
            expect(container.handleCloseCreatePopup).toHaveBeenCalled();
        });
    });

    describe('edit notification template', () => {
        it('should render correctly', () => {
            // Arrange
            const props = {
                editTemplatePopup: { show: true }
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(EditNotificationTemplate).length).toBe(1);
        });

        it('should call exactly functions when close popup', () => {
            // Arrange
            const props = {
                setEditTemplatePopup: jest.fn(),
                setEditTemplateResult: jest.fn(),
                setSelectedTemplate: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseEditPopup();

            // Assert
            expect(props.setEditTemplatePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setEditTemplateResult).toHaveBeenCalledWith(null);
            expect(props.setSelectedTemplate).toHaveBeenCalledWith(null);
        });

        it('should call exactly functions when edit template successful', () => {
            // Arrange
            const props = {
                gridParams: {},
                getGridData: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseEditPopup = jest.fn();

            // Act
            container.handleEditSuccess();

            // Assert
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
            expect(container.handleCloseEditPopup).toHaveBeenCalled();
        });
    });

    describe('delete notification template', () => {
        it('should render correctly', () => {
            // Arrange
            const props = {
                deleteTemplatePopup: { show: true, name: 'voiding abnormal bet' }
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(SweetModalDialog).length).toBe(1);
            expect(container.find(SweetModalDialog).props().contents)
                .toEqual(<div>Are you sure you want to delete <strong>voiding abnormal bet</strong>?</div>);
        });

        it('should call exactly functions when close popup', () => {
            // Arrange
            const props = {
                setDeleteTemplatePopup: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseDeletePopup();

            // Assert
            expect(props.setDeleteTemplatePopup).toHaveBeenCalledWith({ show: false });
        });

        it('should call exactly functions when delete successful', async () => {
            // Arrange
            const response = { data: { isSuccessful: true, message: '' } };
            const promise = Promise.resolve(response);
            const props = {
                deleteTemplatePopup: { templateId: 1, name: 'void abnormal bet' },
                deleteTemplate: jest.fn(() => promise),
                getGridData: jest.fn(),
                gridParams: {}
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseDeletePopup = jest.fn();

            // Act
            container.handleDelete();
            await promise;

            // Assert
            expect(props.deleteTemplate).toHaveBeenCalledWith({ notificationTemplateId: 1, notificationTemplateName: 'void abnormal bet' });
            expect(toast.success).toHaveBeenCalledWith('DeleteNotificationTemplateSuccessfully');
            expect(container.handleCloseDeletePopup).toHaveBeenCalled();
            expect(props.getGridData).toHaveBeenCalledWith(props.gridParams);
        });

        it('should call exactly functions when delete unsuccessful', async () => {
            // Arrange
            const response = { data: { isSuccessful: false, message: 'delete unsuccessful' } };
            const promise = Promise.resolve(response);
            const props = {
                deleteTemplatePopup: { templateId: 1, name: 'void abnormal bet' },
                deleteTemplate: jest.fn(() => promise),
                getGridData: jest.fn(),
                gridParams: {}
            };
            const container = shallow(<AppContainer {...props} />).instance();
            container.handleCloseDeletePopup = jest.fn();

            // Act
            container.handleDelete();
            await promise;

            // Assert
            expect(props.deleteTemplate).toHaveBeenCalledWith({ notificationTemplateId: 1, notificationTemplateName: 'void abnormal bet' });
            expect(toast.error).toHaveBeenCalledWith('delete unsuccessful');
            expect(container.handleCloseDeletePopup).toHaveBeenCalled();
        });
    });

    describe('view content', () => {
        it('render component correctly', () => {
            // Arrange
            const props = {
                contentDetailPopup: { show: true }
            };

            // Act
            const container = shallow(<AppContainer {...props} />);

            // Assert
            expect(container).toMatchSnapshot();
            expect(container.find(ContentDetailModalDialog).length).toBe(1);
        });

        it('should calls exactly functions when close popup', () => {
            // Arrange
            const props = {
                setContentDetailPopup: jest.fn(),
                setSelectedTemplate: jest.fn()
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseViewContentPopup();

            // Assert
            expect(props.setContentDetailPopup).toHaveBeenCalledWith({ show: false });
            expect(props.setSelectedTemplate).toHaveBeenCalledWith(null);
        });
    });
});