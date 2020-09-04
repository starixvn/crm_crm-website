import React, { Fragment } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Button } from 'react-bootstrap';
import SweetModalDialog from '@jsroot/components/SweetModalDialog';
import LangHelper from '@jsroot/common/langHelper';

export const ContentDetailModalDialog = props => {
    const template = props.template;
    const buttons = [
        <Button variant="primary" size="sm" onClick={props.onClose}>
            {LangHelper.getResource('Close')}
        </Button>
    ];

    return (
        <SweetModalDialog size="lg"
            backdropClassName="notification-template-content-modal-backdrop"
            className="notification-template-content-modal"
            title={template.title}
            contents={
                <Fragment>
                    {
                        template.contents.map(item => {
                            return (
                                <Fragment key={item.languageId}>
                                    <div className="font-weight-bold">{LangHelper.getResource('Content')} ({item.language})</div>
                                    <div className="border p-2 mt-2 mb-3 content">{item.content}</div>
                                </Fragment>
                            );
                        })}
                </Fragment>
            }
            buttons={buttons}
            onClose={props.onClose}
        />
    );
};

ContentDetailModalDialog.propTypes = {
    template: PropTypes.shape({
        notificationTemplateId: PropTypes.number,
        notificationTypeId: PropTypes.number,
        notificationTemplateName: PropTypes.string,
        title: PropTypes.string,
        contents: PropTypes.arrayOf(PropTypes.object)
    }).isRequired
};

ContentDetailModalDialog.defaultProps = {
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

export default connect(mapStateToProps)(ContentDetailModalDialog);