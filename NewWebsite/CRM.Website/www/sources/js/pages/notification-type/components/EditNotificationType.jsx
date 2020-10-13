import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import { renderSystemOption, validateFormik } from '../common.functions';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelect from '../../../components/multiple-select/index';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import DraggableModalDialog from '@jsroot/components/DraggableModalDialog';

export class EditNotificationType extends Component {
    static defaultProps = {
        editNotificationTypePopup: {
            data: {
                recipientTypes: [],
                chatTypes: []
            }
        },
        settings: {
            recipientTypes: [],
            chatTypes: [],
            systems: []
        },
        editNotificationTypeResult: {},
        getSettings: {}
    };

    constructor(props) {
        super(props);

        this.state = {
            recipientTypesOptions: [],
            chatTypesOptions: []
        };
    }

    componentDidMount() {
        const data = this.props.editNotificationTypePopup.data;
        const recipientTypesOptions = this.props.settings.recipientTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: data.recipientTypes.some(y => y.recipientTypeId === x.id) ? true : false
            };
        });

        const chatTypesOptions = this.props.settings.chatTypes.map(x => {
            return {
                id: x.id,
                name: x.name,
                checked: data.chatTypes.some(y => y.chatTypeId === x.id) ? true : false
            };
        });

        this.setState({
            recipientTypesOptions,
            chatTypesOptions
        });
    }

    renderForm = ({
        values,
        errors,
        touched,
        handleChange,
        handleSubmit,
        isSubmitting,
        setFieldValue
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
                                dataSource={this.state.recipientTypesOptions}
                                keyField='id'
                                valueField='name'
                                statusField='checked'
                                isAllTextShown
                                onChange={(currentItem, selectedItemsKey) => {
                                    setFieldValue('recipientTypeIds', selectedItemsKey);
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
                                dataSource={this.state.chatTypesOptions}
                                valueField='name'
                                statusField='checked'
                                keyField='id'
                                isAllTextShown
                                onChange={(currentItem, selectedItemsKey) => {
                                    setFieldValue('chatTypeIds', selectedItemsKey);
                                }}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="systemId" className="required">
                            {LangHelper.getResource('System')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none" id="systemId"
                                isInvalid={touched.system && errors.system}
                                name="systemId"
                                onChange={handleChange}
                                value={values.systemId}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.props.settings.systems.map(renderSystemOption)}
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
                }, 3000);
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
            <Modal show className="new-notification-type" dialogAs={DraggableModalDialog}
                backdrop="static" keyboard={false} size="lg" centered
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditNotificationType')}</Modal.Title>
                </Modal.Header>
                <Formik
                    initialValues={initialValues}
                    validate={values => validateFormik(values)}
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