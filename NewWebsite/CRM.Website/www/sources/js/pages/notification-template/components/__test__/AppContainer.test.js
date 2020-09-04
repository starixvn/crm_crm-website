import React from 'react';
import { shallow } from 'enzyme';
import { AppContainer } from '../AppContainer';
import Filter from '../Filter';
import DataGrid from '../DataGrid';
import NewNotificationTemplate from '../NewNotificationTemplate';
import EditNotificationTemplate from '../EditNotificationTemplate';
import ContentDetailModalDialog from '../ContentDetailModalDialog';

describe('AppContainer', () => {
    afterAll(() => {
        jest.clearAllMocks();
    });

    it('renders component correctly', () => {
        // Act
        const container = shallow(<AppContainer />);

        // Assert
        expect(container).toMatchSnapshot();
    });

    it('renders default component correctly', () => {
        // Arrange
        const props = { isHost: false };

        // Act
        const container = shallow(<AppContainer {...props} />);

        // Assert
        expect(container.find(Filter).length).toBe(1);
        expect(container.find(Filter).props().isHost).toBe(props.isHost);

        expect(container.find(DataGrid).length).toBe(1);
        expect(container.find(DataGrid).props().isHost).toBe(props.isHost);
    });

    describe('create notification template', () => {
        it('render component correctly', () => {
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

        it('should calls exactly functions when close popup', () => {
            // Arrange
            const props = {
                setCreateTemplatePopup: jest.fn(),
                setCreateTemplateResult: jest.fn(),
                createTemplatePopup: { show: true }
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseCreatePopup();

            // Assert
            expect(props.setCreateTemplatePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setCreateTemplateResult).toHaveBeenCalledWith(null);
        });

        it('should calls exactly functions when create template successful', () => {
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
        it('render component correctly', () => {
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

        it('should calls exactly functions when close popup', () => {
            // Arrange
            const props = {
                setEditTemplatePopup: jest.fn(),
                setEditTemplateResult: jest.fn(),
                setSelectedTemplate: jest.fn(),
                editTemplatePopup: { show: true }
            };
            const container = shallow(<AppContainer {...props} />).instance();

            // Act
            container.handleCloseEditPopup();

            // Assert
            expect(props.setEditTemplatePopup).toHaveBeenCalledWith({ show: false });
            expect(props.setEditTemplateResult).toHaveBeenCalledWith(null);
            expect(props.setSelectedTemplate).toHaveBeenCalledWith(null);
        });

        it('should calls exactly functions when edit template successful', () => {
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
                setSelectedTemplate: jest.fn(),
                contentDetailPopup: { show: true }
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