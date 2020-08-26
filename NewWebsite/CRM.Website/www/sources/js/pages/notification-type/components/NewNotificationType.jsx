import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelect from '../../../components/multiple-select/index';
import ResponseMessage from '@jsroot/components/ResponseMessage';


class NewNotificationType extends Component {
    constructor(props) {
        super(props);
    }

    validate = values => {
        const errors = {};

        if (!values.notificationTypeName || values.notificationTypeName.trim().length === 0) {
            errors.notificationTypeName = LangHelper.getResource('NotificationTypeNameRequired');
        } else {
            if (values.notificationTypeName.trim().length > 500) {
                errors.notificationTypeName = LangHelper.formatString(
                    LangHelper.getResource('FieldMaxLength'),
                    LangHelper.getResource('NotificationTypeName'),
                    500);
            }
        }

        if (!values.recipientTypeIds) {
            errors.recipientTypeIds = LangHelper.getResource('RecipientTypeRequired');
        }

        if (!values.chatTypeIds) {
            errors.chatTypeIds = LangHelper.getResource('ChatTypeRequired');
        }

        if (!values.systemId) {
            errors.systemId = LangHelper.getResource('SystemRequired');
        }

        return errors;
    }

    renderSystemOption = item => <option key={item.id} value={item.id} >{item.name}</option>

    renderForm = ({
        values,
        errors,
        touched,
        handleChange,
        handleSubmit,
        isSubmitting

    }) => {

        return (
            <Form noValidate onSubmit={handleSubmit}>
                <Modal.Body>
                    <FormikErrorMessage touched={touched} errors={errors} fieldNames={Object.keys(values)} />
                    {
                        this.props.createNotificationTypeResult &&
                        <ResponseMessage isSuccessful={this.props.createNotificationTypeResult.isSuccessful}
                            message={this.props.createNotificationTypeResult.message || LangHelper.getResource('InsertNotificationTypeSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="name" className="required">
                            {LangHelper.getResource('NotificationTypeName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.notificationTypeName} name="notificationTypeName" id="name"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.notificationTypeName && errors.notificationTypeName}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('RecipientType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelect
                                isInvalid={touched.recipientTypeIds && errors.recipientTypeIds}
                                dataSource={this.props.settings.recipientTypes}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={(currentItem, selectedItemsKey) => {
                                    handleChange('recipientTypeIds')(selectedItemsKey);
                                }}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" className="required">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelect
                                isInvalid={touched.chatTypeIds && errors.chatTypeIds}
                                dataSource={this.props.settings.chatTypes}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={(currentItem, selectedItemsKey) => {
                                    handleChange('chatTypeIds')(selectedItemsKey);
                                }}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" value={values.systemId} className="required">
                            {LangHelper.getResource('System')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.systemId && errors.systemId}
                                name="systemId"
                                onChange={handleChange}
                                value={values.systemId}
                            >
                                <option value="" hidden>{LangHelper.getResource('SelectOptions')}</option>
                                {this.props.settings.systems.map(this.renderSystemOption)}
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
        const notificationType = {
            notificationTypeName: values.notificationTypeName,
            chatTypeIds: values.chatTypeIds,
            recipientTypeIds: values.recipientTypeIds,
            systemId: values.systemId
        };

        setSubmitting(true);
        this.props.createNotificationType(notificationType).then(() => {
            if (this.props.createNotificationTypeResult.isSuccessful) {
                setTimeout(() => {
                    this.props.onSuccess();
                }, 3000);
            } else {
                setSubmitting(false);
            }
        });
    }

    render() {
        const initialValues = {
            notificationTypeName: '',
            recipientTypeIds: '',
            chatTypeIds: '',
            systemId: ''
        };
        return (
            <Modal show className="new-notification-type"
                backdrop="static" keyboard={false} size="lg" centered
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('InsertNotificationType')}</Modal.Title>
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

const mapDispatchToProps = dispatch => {
    return bindActionCreators({
        createNotificationType: ActionCreators.createNotificationType
    }, dispatch);
};

const mapStateToProps = state => {
    const { createNotificationTypeResult, settings, getSettings } = state;
    return {
        createNotificationTypeResult,
        settings,
        getSettings
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(NewNotificationType);