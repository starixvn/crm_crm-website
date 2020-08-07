import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import { object, string } from 'yup';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import LangHelper from '@jsroot/common/langHelper';

class EditNotificationType extends Component {
    constructor(props) {
        super(props);
    }

    renderNotificationTypeOption = item => <option key={item.notificationTypeId} value={item.notificationTypeId} >{item.name}</option>

    validationSchema = object().shape({
        notificationType: string().required(LangHelper.getResource('NotificationTypeRequired')),
        recipientType: string().required(LangHelper.getResource('RecipientTypeRequired')),
        chatType: string().required(LangHelper.getResource('ChatTypeRequired')),
        system: string().required(LangHelper.getResource('SystemRequired')),
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
                    {/* <FormikErrorMessage touched={touched} errors={errors} fieldNames={Object.keys(values)} /> */}
                    {
                        this.props.editTypeResult &&
                        <ResponseMessage isSuccessful={this.props.editTypeResult.isSuccessful}
                            message={this.props.editTypeResult.message || LangHelper.getResource('EditNotificationTypeSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="notificationType" className="required">
                            {LangHelper.getResource('NotificationTypeName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm"  name="notificationType"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.notificationType && errors.notificationType}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="recipientType" className="required">
                            {LangHelper.getResource('RecipientType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.recipientType && errors.recipientType}
                                name="recipientType"
                                onChange={handleChange}
                                // value={values.recipientType}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                            </Form.Control>
                        </Col>
                    </Form.Group>
                    
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="chatType" className="required">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.chatType && errors.chatType}
                                name="chatType"
                                onChange={handleChange}
                                // value={values.chatType}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                            </Form.Control>
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="system" className="required">
                            {LangHelper.getResource('System')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.system && errors.system}
                                name="system"
                                onChange={handleChange}
                                // value={values.system}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.notificationTypes.map(this.renderNotificationTypeOption)}
                            </Form.Control>
                        </Col>
                    </Form.Group>

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
        this.props.editType(values).then(() => {
            if (this.props.editTypeResult.isSuccessful) {
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
            <Modal show className="new-notification-Type"
                backdrop="static" keyboard={false} size="lg"
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditNotificationType')}</Modal.Title>
                </Modal.Header>
                <Formik
                    validationSchema={this.props.editTypePopup.type}
                    render={this.renderForm}
                    onSubmit={this.handleSubmit}
                />
            </Modal>
        );
    }
}

const mapDispatchToProps = dispatch => {
    return bindActionCreators({
        editType: ActionCreators.editType
    }, dispatch);
};

const mapStateToProps = state => {
    const { editTypeResult, editTypePopup, notificationTypes } = state;
    return {
        editTypeResult,
        editTypePopup,
        notificationTypes
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditNotificationType);