import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';

export class EditNotificationTemplate extends Component {
    static propTypes = {
        languages: PropTypes.arrayOf(PropTypes.object),
        notificationTypes: PropTypes.arrayOf(PropTypes.object),
        template: PropTypes.shape({
            notificationTemplateId: PropTypes.number,
            notificationTypeId: PropTypes.number,
            notificationTemplateName: PropTypes.string,
            title: PropTypes.string,
            contents: PropTypes.arrayOf(PropTypes.object)
        }).isRequired
    }

    static defaultProps = {
        languages: [],
        notificationTypes: [],
        template: {
            notificationTemplateId: 0,
            notificationTypeId: 0,
            notificationTemplateName: '',
            title: '',
            contents: []
        }
    }

    constructor(props) {
        super(props);
    }

    renderNotificationTypeOption = item => {
        return (
            <option key={item.notificationTypeId} value={item.notificationTypeId}>
                {item.notificationTypeName}
            </option>
        );
    }

    validate = values => {
        const errors = {};
        if (!values.notificationTypeId) {
            errors.notificationTypeId = LangHelper.getResource('NotificationTypeRequired');
        }

        if (!values.notificationTemplateName || values.notificationTemplateName.trim().length === 0) {
            errors.notificationTemplateName = LangHelper.getResource('TemplateNameRequired');
        } else {
            if (values.notificationTemplateName.trim().length > 1000) {
                errors.notificationTemplateName = LangHelper.formatString(
                    LangHelper.getResource('FieldMaxLength'),
                    LangHelper.getResource('TemplateName'),
                    1000);
            }
        }

        if (!values.title || values.title.trim().length === 0) {
            errors.title = LangHelper.getResource('TitleRequired');
        } else {
            if (values.title.trim().length > 2000) {
                errors.title = LangHelper.formatString(
                    LangHelper.getResource('FieldMaxLength'),
                    LangHelper.getResource('Title'),
                    2000);
            }
        }

        this.props.languages.map(item => {
            if (!values['language_' + item.settingId] || values['language_' + item.settingId].trim().length === 0) {
                errors['language_' + item.settingId] = `${LangHelper.getResource('Content')} (${item.settingValue}) is mandatory!`;
            }
        });

        return errors;
    }

    renderForm = ({
        values,
        errors,
        touched,
        handleChange,
        handleSubmit,
        setFieldValue,
        isSubmitting
    }) => {
        return (
            <Form noValidate onSubmit={handleSubmit}>
                <Modal.Body>
                    <FormikErrorMessage touched={touched} errors={errors} fieldNames={Object.keys(values)} />
                    {
                        this.props.editTemplateResult &&
                        <ResponseMessage isSuccessful={this.props.editTemplateResult.isSuccessful}
                            message={this.props.editTemplateResult.message || LangHelper.getResource('EditNotificationTemplateSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required" htmlFor="notificationTypeId">
                            {LangHelper.getResource('NotificationType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.notificationTypeId && errors.notificationTypeId}
                                name="notificationTypeId" id="notificationTypeId"
                                onChange={handleChange}
                                value={values.notificationTypeId}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required" htmlFor="notificationTemplateName">
                            {LangHelper.getResource('TemplateName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.notificationTemplateName}
                                name="notificationTemplateName" id="notificationTemplateName"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.notificationTemplateName && errors.notificationTemplateName}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required" htmlFor="title">
                            {LangHelper.getResource('Title')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.title}
                                name="title" id="title"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.title && errors.title}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
                    {
                        this.props.languages.map(item => {
                            return (
                                <Form.Group key={`language${item.settingId}`} as={Row}>
                                    <Form.Label column sm="4" md="3" className="required" htmlFor={'language_' + item.settingId}>
                                        {LangHelper.getResource('Content')} ({item.settingValue})
                                    </Form.Label>
                                    <Col sm="8" md="9">
                                        <Form.Control as="textarea" value={values['language_' + item.settingId]}
                                            name={'language_' + item.settingId} id={'language_' + item.settingId} rows="4"
                                            className="shadow-none form-control"
                                            isInvalid={touched['language_' + item.settingId] && errors['language_' + item.settingId]}
                                            onChange={handleChange}
                                        />
                                    </Col>
                                </Form.Group>
                            );
                        })
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button type="submit" variant="primary" size="sm" disabled={isSubmitting}>
                        {LangHelper.getResource('Save')}
                    </Button>
                    <Button type="submit" variant="primary" size="sm" onClick={this.props.onClose}>
                        {LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Form>
        );
    }

    handleSubmit = (values, { setSubmitting }) => {
        setSubmitting(true);
        const template = {
            notificationTemplateId: values.notificationTemplateId,
            notificationTypeId: values.notificationTypeId,
            notificationTemplateName: values.notificationTemplateName,
            title: values.title,
            contents: []
        };
        this.props.languages.map(item => {
            template.contents.push({
                languageId: item.settingId,
                content: values['language_' + item.settingId]
            });
        });
        this.props.editTemplate(template).then(() => {
            if (this.props.editTemplateResult.isSuccessful) {
                setTimeout(() => {
                    this.props.onSuccess();
                }, 3000);
            } else {
                setSubmitting(false);
            }
        });
    }

    render() {
        const template = this.props.template;
        const initialValues = {
            notificationTemplateId: template.notificationTemplateId,
            notificationTypeId: template.notificationTypeId,
            notificationTemplateName: template.notificationTemplateName,
            title: template.title
        };
        this.props.languages.map(item => {
            initialValues['language_' + item.settingId] = '';
            const index = template.contents.findIndex(x => x.languageId === item.settingId);
            if (index >= 0) {
                initialValues['language_' + item.settingId] = template.contents[index].content;
            }
        });

        return (
            <Modal show className="notification-template-modal" dialogAs={DraggableModalDialog}
                backdrop="static" keyboard={false} size="lg" centered
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditNotificationTemplate')}</Modal.Title>
                </Modal.Header>
                <Formik
                    initialValues={initialValues}
                    validate={this.validate}
                    onSubmit={this.handleSubmit}
                >
                    {this.renderForm}
                </Formik>
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { editTemplateResult, selectedTemplate, notificationTypes, languages } = state;
    return {
        editTemplateResult,
        template: selectedTemplate,
        notificationTypes,
        languages
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditNotificationTemplate);