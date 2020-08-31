import React, { Fragment } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Button, Modal } from 'react-bootstrap';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';
import LangHelper from '@jsroot/common/langHelper';

export const ContentDetailPopup = props => {
    const template = props.template;

    return (
        <Modal show className="notification-template-content-modal" dialogAs={DraggableModalDialog}
            backdrop="static" keyboard={false} size="lg" centered
            onHide={props.onClose}
        >
            <Modal.Header closeButton>
                <Modal.Title as="div">{template.title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {
                    template.contents.map(item => {
                        return (
                            <Fragment key={item.languageId}>
                                <div className="font-weight-bold">{LangHelper.getResource('Content')} ({item.language})</div>
                                <div className="border p-2 mt-2 mb-3 content">{item.content}</div>
                            </Fragment>
                        );
                    })
                }
            </Modal.Body>
            <Modal.Footer>
                <Button variant="primary" size="sm" onClick={props.onClose}>
                    {LangHelper.getResource('Close')}
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

ContentDetailPopup.propTypes = {
    template: PropTypes.shape({
        notificationTemplateId: PropTypes.number,
        notificationTypeId: PropTypes.number,
        notificationTemplateName: PropTypes.string,
        title: PropTypes.string,
        contents: PropTypes.arrayOf(PropTypes.object)
    }).isRequired
};

ContentDetailPopup.defaultProps = {
    template: {
        notificationTemplateId: 0,
        notificationTypeId: 0,
        notificationTemplateName: '',
        title: '',
        contents: []
    }
};

const mapStateToProps = state => {
    const { selectedTemplate } = state;
    return { template: selectedTemplate };
};

export default connect(mapStateToProps)(ContentDetailPopup);