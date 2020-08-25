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

class EditNotificationType extends Component {
    constructor(props) {
        super(props);

        this.state = {
            popup: {
                recipientTypeIds: '',
                chatTypeIds: ''
            },
            recipientTypesOptions: [],
            chatTypesOptions: []
        };
    }

    componentDidMount() {

        // recipient type
        const recipientTypeIds = [];
        this.props.editNotificationTypePopup.data.recipientTypes.map(x => {
            recipientTypeIds.push(x.recipientTypeId);
            return {
                id: x.recipientTypeId
            };
        });

        const recipientTypesOptions = this.props.settings.recipients.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: recipientTypeIds.includes(x.id) ? true : false
            };
        });

        // chat type
        const chatTypeIds = [];
        this.props.editNotificationTypePopup.data.chatTypes.map(x => {
            chatTypeIds.push(x.chatTypeId);
            return {
                id: x.chatTypeId
            };
        });

        const chatTypesOptions = this.props.settings.chatTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: chatTypeIds.includes(x.id) ? true : false
            };
        });

        this.setState({
            recipientTypesOptions,
            chatTypesOptions,
            popup: {
                recipientTypeIds: recipientTypeIds.toString(),
                chatTypeIds: chatTypeIds.toString()
            }
        });
    }

    renderSystemOption = item => <option key={item.id} value={item.id} >{item.name}</option>

    validate = values => {
        const errors = {};
        if (!values.notificationTypeName) {
            errors.notificationTypeName = LangHelper.getResource('NotificationTypeNameRequired');
        }

        if (values.notificationTypeName.length > 500) {
            errors.notificationTypeName = LangHelper.getResource('NotificationTypeNameMaxLengthRequired');
        }

        if (!values.recipientTypeIds) {
            errors.recipientTypeIds = LangHelper.getResource('RecipientTypeRequired');
        }

        if (!values.chatTypeIds) {
            errors.chatTypeIds = LangHelper.getResource('ChatTypeRequired');
        }

        return errors;
    }

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
                        this.props.editNotificationTypeResult &&
                        <ResponseMessage isSuccessful={this.props.editNotificationTypeResult.isSuccessful}
                            message={this.props.editNotificationTypeResult.message || LangHelper.getResource('EditNotificationTypeSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="name" className="required">
                            {LangHelper.getResource('NotificationTypeName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.notificationTypeName} name="notificationTypeName"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.notificationTypeName && errors.notificationTypeName}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="recipientTypeIds" className="required">
                            {LangHelper.getResource('RecipientType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelect
                                isInvalid={touched.recipientTypeIds && errors.recipientTypeIds}
                                initialValue={this.state.popup.recipientTypeIds}
                                dataSource={this.state.recipientTypesOptions}
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
                        <Form.Label column sm="4" md="3" htmlFor="chatType" className="required">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelect
                                isInvalid={touched.chatTypeIds && errors.chatTypeIds}
                                initialValue={this.state.popup.recipientTypeIds.value}
                                dataSource={this.state.chatTypesOptions}
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
                        <Form.Label column sm="4" md="3" htmlFor="systemId" className="required">
                            {LangHelper.getResource('System')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.system && errors.system}
                                name="systemId"
                                onChange={handleChange}
                                value={values.systemId}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
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
            notificationTypeId: values.notificationTypeId,
            notificationTypeName: values.notificationTypeName,
            chatTypeIds: values.chatTypeIds,
            recipientTypeIds: values.recipientTypeIds,
            systemId: values.systemId
        };

        setSubmitting(true);
        this.props.editNotificationType(notificationType).then(() => {
            if (this.props.editNotificationTypeResult.isSuccessful) {
                setTimeout(() => {
                    this.props.onSuccess();
                }, 1000);
            } else {
                setSubmitting(false);
            }
        });
    }

    render() {
        const notificationType = this.props.editNotificationTypePopup.data;
        const initialValues = {
            notificationTypeId: notificationType.notificationTypeId,
            notificationTypeName: notificationType.notificationTypeName,
            recipientTypeIds: notificationType.recipientTypes.map(x => x.recipientTypeId).toString(),
            chatTypeIds: notificationType.chatTypes.map(x => x.chatTypeId).toString(),
            systemId: notificationType.systemId
        };

        return (
            <Modal show className="new-notification-type"
                backdrop="static" keyboard={false} size="lg"
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditNotificationType')}</Modal.Title>
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
        editNotificationType: ActionCreators.editNotificationType
    }, dispatch);
};

const mapStateToProps = state => {
    const {
        editNotificationTypeResult,
        editNotificationTypePopup,
        settings,
        getSettings
    } = state;
    return {
        editNotificationTypeResult,
        editNotificationTypePopup,
        settings,
        getSettings
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditNotificationType);