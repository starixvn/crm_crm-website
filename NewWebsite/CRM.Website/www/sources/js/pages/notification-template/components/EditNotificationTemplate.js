import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';

class EditNotificationTemplate extends Component {
    constructor(props) {
        super(props);
    }

    renderNotificationTypeOption = item => <option key={item.notificationTypeId} value={item.notificationTypeId} >{item.name}</option>

    validate = values => {
        const errors = {};
        if (!values.notificationTypeId) {
            errors.notificationTypeId = LangHelper.getResource('NotificationTypeRequired');
        }

        if (!values.name || values.name.trim().length === 0) {
            errors.name = LangHelper.getResource('TemplateNameRequired');
        }

        if (!values.title || values.title.trim().length === 0) {
            errors.title = LangHelper.getResource('TitleRequired');
        }

        this.props.languages.map(item => {
            if (!values['language_' + item.settingId] || values['language_' + item.settingId].trim().length === 0) {
                errors['language_' + item.settingId] = `${item.settingValue} is mandatory!`;
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
                        <Form.Label column sm="4" md="3" htmlFor="notificationTypeId" className="required">
                            {LangHelper.getResource('NotificationType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.notificationTypeId && errors.notificationTypeId}
                                name="notificationTypeId"
                                onChange={handleChange}
                                value={values.notificationTypeId}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="name" className="required">
                            {LangHelper.getResource('TemplateName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.name} name="name"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.name && errors.name}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="title" className="required">
                            {LangHelper.getResource('Title')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.title} name="title"
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
                                    <Form.Label column sm="4" md="3" className="required">
                                        {item.settingValue}
                                    </Form.Label>
                                    <Col sm="8" md="9">
                                        <Form.Control as="textarea" value={values['language_' + item.settingId]} name={'language_' + item.settingId} rows="4"
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
                        {LangHelper.getResource('Submit')}
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
            name: values.name,
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
        const template = this.props.editTemplatePopup.template;
        const initialValues = {
            notificationTemplateId: template.notificationTemplateId,
            notificationTypeId: template.notificationTypeId,
            name: template.name,
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
            <Modal show className="new-notification-template"
                backdrop="static" keyboard={false} size="lg" centered
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditNotificationTemplate')}</Modal.Title>
                </Modal.Header>
                <Formik
                    initialValues={initialValues}
                    validate={this.validate}
                    render={this.renderForm}
                    onSubmit={this.handleSubmit}
                />
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => {
    return bindActionCreators({
        editTemplate: ActionCreators.editTemplate
    }, dispatch);
};

const mapStateToProps = state => {
    const { editTemplateResult, editTemplatePopup, notificationTypes, languages } = state;
    return {
        editTemplateResult,
        editTemplatePopup,
        notificationTypes,
        languages
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditNotificationTemplate);