import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import { object, string } from 'yup';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import LangHelper from '@jsroot/common/langHelper';

class NewNotificationTemplate extends Component {
    constructor(props) {
        super(props);
    }

    renderNotificationTypeOption = item => <option key={item.notificationTypeId} value={item.notificationTypeId} >{item.name}</option>

    validationSchema = object().shape({
        notificationTypeId: string().required(LangHelper.getResource('NotificationTypeRequired')),
        name: string().required(LangHelper.getResource('TemplateNameRequired')).max(100),
        title: string().required(LangHelper.getResource('TitleRequired')).max(300),
        contentEN: string().required(LangHelper.getResource('ContentENRequired')),
        contentSC: string().required(LangHelper.getResource('ContentSCRequired')),
        contentENSC: string().required(LangHelper.getResource('ContentENSCRequired'))
    })

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
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="contentEN" className="required">
                            {LangHelper.getResource('Content')} (EN)
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="textarea" value={values.contentEN} name="contentEN" rows="4"
                                className="shadow-none form-control"
                                isInvalid={touched.contentEN && errors.contentEN}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="contentSC" className="required">
                            {LangHelper.getResource('Content')} (SC)
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="textarea" value={values.contentSC} name="contentSC" rows="4"
                                className="shadow-none form-control"
                                isInvalid={touched.contentEN && errors.contentSC}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" />
                        <Col sm="8" md="9">
                            <Form.Check
                                id="isCombineContentENSC"
                                name="isCombineContentENSC"
                                checked={values.isCombineContentENSC}
                                label={LangHelper.getResource('Combine2ContentENSC')}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="contentSC" className="required">
                            {LangHelper.getResource('Content')} (EN & SC)
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="textarea"
                                value={values.contentENSC} name="contentENSC" rows="4"
                                className="shadow-none form-control"
                                isInvalid={touched.contentENSC && errors.contentENSC}
                                disabled={values.isCombineContentENSC}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>
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
        this.props.createTemplate(values).then(() => {
            if (this.props.createTemplateResult.isSuccessful) {
                setTimeout(() => {
                    this.props.onSuccess();
                }, 1000);
            } else {
                setSubmitting(false);
            }
        });
    }

    render() {
        return (
            <Modal show className="new-notification-template"
                backdrop="static" keyboard={false} size="lg"
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('CreateNewNotificationTemplate')}</Modal.Title>
                </Modal.Header>
                <Formik
                    initialValues={{
                        notificationTypeId: '',
                        name: '',
                        title: '',
                        contentEN: '',
                        contentSC: '',
                        contentENSC: '',
                        isCombineContentENSC: true
                    }}
                    validationSchema={this.validationSchema}
                    render={this.renderForm}
                    onSubmit={this.handleSubmit}
                />
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => {
    return bindActionCreators({
        createTemplate: ActionCreators.createTemplate
    }, dispatch);
};

const mapStateToProps = state => {
    const { createTemplateResult, notificationTypes } = state;
    return {
        createTemplateResult,
        notificationTypes
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(NewNotificationTemplate);