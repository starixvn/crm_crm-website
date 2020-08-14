import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import { object, string } from 'yup';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import LangHelper from '@jsroot/common/langHelper';
import MultipleSelectField from './MultipleSelectField';

class EditNotificationType extends Component {
    constructor(props) {
        super(props);

        this.state = {
            popup: {
                recipientTypes: {
                    value: ''
                },
                chatTypes: {
                    value: ''
                },
                system: {
                    value: ''
                }
            },
            recipientTypesOptions: [],
            chatTypesOptions: [],
            systemsOptions: []
        };
    }

    validationSchema = object().shape({
        notificationType: string().required(LangHelper.getResource('NotificationTypeRequired')),
        recipientType: string().required(LangHelper.getResource('RecipientTypeRequired')),
        chatType: string().required(LangHelper.getResource('ChatTypeRequired')),
        system: string().required(LangHelper.getResource('SystemRequired')),
    })

    componentDidMount () {
            // recipient type
            const recipientTypeIds = [];
            const recipientTypesOptions = this.props.settings.recipients.map(x => {
                recipientTypeIds.push(x.recipientTypeID);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });

            // chat type
            const chatTypeIds = [];
            const chatTypesOptions = this.props.settings.chatTypes.map(x => {
                chatTypeIds.push(x.id);
                return {
                    id: x.id,
                    name: x.name,
                    checked: true
                };
            });

            // system
            const systemIds = [];
            const systemsOptions = this.props.settings.systems.map(x => {
                systemIds.push(x.id);
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
                        ...prevState.popup,
                        ...{
                            recipientTypes: {
                                value: recipientTypeIds.toString()
                            },
                            chatTypes: {
                                value: chatTypeIds.toString()
                            },
                            systems: {
                                value: systemIds.toString()
                            }
                        }
                    }
            }));
    }
        recipientTypeChange = value => this.handlePopupChange('recipientTypes', value)
        chatTypeChange = value => this.handlePopupChange('chatTypes', value)
        systemChange = value => this.handlePopupChange('system', value)
    
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
        setFieldValue,
        isSubmitting
    }) => {
        return (
            <Form noValidate onSubmit={handleSubmit}>
                <Modal.Body>
                    <FormikErrorMessage touched={touched} errors={errors} fieldNames={Object.keys(values)} />
                    {
                        this.props.editTypeResult &&
                        <ResponseMessage isSuccessful={this.props.editTypeResult.isSuccessful}
                            message={this.props.editTypeResult.message || LangHelper.getResource('EditNotificationTypeSuccessfully')}
                        />
                    }
                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="name" className="required">
                            {LangHelper.getResource('NotificationTypeName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={this.props.editTypePopup.data.notificationTypeName} name="name"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.name && errors.name}
                                onChange={handleChange}
                            />
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="recipientType" className="required">
                            {LangHelper.getResource('RecipientType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelectField
                            width={570}
                            initialValue={this.state.popup.recipientTypes.value}
                            data={this.state.recipientTypesOptions}
                            keyField='id'
                            valueField='name'
                            statusField=''
                            isSummited={false}
                            isAllTextShown
                            onChange={this.recipientTypeChange}
                            />
                         </Col>
                    </Form.Group>

                    <Form.Group as={Row}>
                        <Form.Label column sm="4" md="3" htmlFor="chatType" className="required">
                            {LangHelper.getResource('ChatType')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <MultipleSelectField
                            width={570}
                            initialValue={this.state.popup.chatTypes.value}
                            data={this.state.chatTypesOptions}
                            keyField='id'
                            valueField='name'
                            statusField=''
                            isSummited={false}
                            isAllTextShown
                            onChange={this.chatTypeChange}
                            />
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
                                value={values.system}
                            >
                                <option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
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
        const notificationType = this.props.editTypePopup.data; 
        const initialValues = {
            notificationTypeName:notificationType.notificationTypeName,
            recipientTypeId: notificationType.recipientTypeId,
            chatTypeId: notificationType.chatTypeIds,
            systemId: notificationType.systemId
        };

        return (
            <Modal show className="new-notification-Type"
                backdrop="static" keyboard={false} size="lg"
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{LangHelper.getResource('EditNotificationType')}</Modal.Title>
                </Modal.Header>
                <Formik
                 initialValues={initialValues}
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
    const { editTypeResult, editTypePopup, settings, getSettings} = state;
    return {
        editTypeResult,
        editTypePopup,
        settings,
        getSettings
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EditNotificationType);