import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';
import ResponseMessage from '@jsroot/components/ResponseMessage';


class NewNotificationType extends Component {
    constructor(props) {
        super(props);

        this.state = {
            popup: {
                recipientTypeIds: {
                    value: ''
                },
                chatTypeIds: {
                    value: ''
                },
                systemId: {
                    value: ''
                }
            },
            recipientTypesOptions: [],
            chatTypesOptions: [],
            systemsOptions: []
        };
    }

    componentDidMount () {
            // recipient type
            const recipientTypesOptions = this.props.settings.recipients.map(x => {
                return {
                    id: x.id,
                    name: x.name,
                    checked: false
                };
            });

            // chat type
            const chatTypesOptions = this.props.settings.chatTypes.map(x => {
                return {
                    id: x.id,
                    name: x.name,
                    checked: false
                };
            });

            // system
            const systemsOptions = this.props.settings.systems.map(x => {
                return {
                    id: x.id,
                    name: x.name
                };
            });

            this.setState(prevState => ({
                    recipientTypesOptions,
                    chatTypesOptions,
                    systemsOptions,
                    popup: {
                        ...prevState.popup
                    },
            }));
    }

    validate = values => {
        const type = this.state.popup;
        const errors = {};
        if (!values.notificationTypeName || values.notificationTypeName.length > 500) {
            errors.notificationTypeName = LangHelper.getResource('NotificationTypeNameRequired');
        }

        if (!type.recipientTypeIds.value) {
            errors.recipientTypeIds = LangHelper.getResource('RecipientTypeRequired');
        }

        if (!type.chatTypeIds.value) {
            errors.chatTypeIds = LangHelper.getResource('ChatTypeRequired');
        }

        if (!values.systemId) {
            errors.systemId = LangHelper.getResource('SystemRequired');
        }

        return errors;
    }

    recipientTypeChange = value => this.handlePopupChange('recipientTypeIds', value)
    chatTypeChange = value => this.handlePopupChange('chatTypeIds', value)

    renderSystemTypeOption = item => <option key={item.id} value={item.id} >{item.name}</option>

    handlePopupChange = (name, value) => {
        this.setState(prevState => ({
            popup: { ...prevState.popup, ...{ [name]: value } }
        }));
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
                        this.props.createTypeResult &&
                        <ResponseMessage isSuccessful={this.props.createTypeResult.isSuccessful}
                            message={this.props.createTypeResult.message || LangHelper.getResource('CreateNewNotificationTypeSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="notificationTypeName" className="required">
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
                            {LangHelper.getResource('RecipientTypeIds')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelectField value={this.state.popup.recipientTypeIds} name="recipientTypeIds" isInvalid={touched.recipientTypeIds && errors.recipientTypeIds}
                            width={570}
                            initialValue={this.state.popup.recipientTypes}
                            data={this.state.recipientTypesOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.recipientTypeChange}
                            />
                         </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="chatTypeIds" className="required">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelectField value={this.state.popup.chatTypeIds} name="chatTypeIds" isInvalid={touched.chatTypeIds && errors.chatTypeIds}
                            width={570}
                            initialValue={this.state.popup.chatTypeIds}
                            data={this.state.chatTypesOptions}
                            keyField='id'
                            valueField='name'
                            statusField='checked'
                            isSummited={false}
                            isAllTextShown
                            onChange={this.chatTypeChange}
                            />
                         </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" value={values.systemId}  htmlFor="systemId" className="required">
                            {LangHelper.getResource('System')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.systemId && errors.systemId}
                                name="systemId"
                                onChange={handleChange}
                                value={values.systemId}
                            >
                                <option value="" hidden>{LangHelper.getResource('PleaseSelect')}</option>
                                {this.state.systemsOptions.map(this.renderSystemTypeOption)}
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
        const type = {
            notificationTypeName: values.notificationTypeName,
            chatTypeIds: this.state.popup.chatTypeIds.value,
            recipientTypeIds: this.state.popup.recipientTypeIds.value,
            systemId: values.systemId
        };

        setSubmitting(true);
        this.props.createType(type).then(() => {
            if (this.props.createTypeResult.isSuccessful) {
                setTimeout(() => {
                    this.props.onSuccess();
                }, 1000);
            } else {
                setSubmitting(false);
            }
        });
    }

    render() {
        const initialValues = {
            notificationTypeName: '',
            recipientTypeIds:'',
            chatTypeIds: '',
            systemId: ''
        }
        return (
            <Modal show className="new-notification-type"
                backdrop="static" keyboard={false} size="lg"
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('CreateNewNotificationType')}</Modal.Title>
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
        createType: ActionCreators.createType
    }, dispatch);
};

const mapStateToProps = state => {
    const { createTypeResult, settings, getSettings } = state;
    return {
        createTypeResult,
        settings,
        getSettings
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(NewNotificationType);