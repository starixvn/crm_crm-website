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
import ResponseMessage from '@jsroot/components/ResponseMessage';


class NewNotificationType extends Component {
    constructor(props) {
        super(props);

        this.state = {
            popup: {
                recipientType: {
                    value: ''
                },
                chatType: {
                    value: ''
                },
                SystemId: {
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
            const recipientTypeIds = [];
            const recipientTypesOptions = this.props.settings.recipients.map(x => {
                recipientTypeIds.push(x.id);
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
                            recipientType: {
                                value: recipientTypeIds.toString()
                            },
                            chatType: {
                                value: chatTypeIds.toString()
                            },
                            SystemId: {
                                value: systemIds.toString()
                            }
                        }
                    }
            }));
    }

    validate = values => {
        const errors = {};
        if (!values.NotificationTypeName) {
            errors.NotificationTypeName = LangHelper.getResource('NotificationTypeRequired');
        }

        // if (!values.recipientType) {
        //     errors.recipientType = LangHelper.getResource('RecipientTypeRequired');
        // }

        // if (!values.chatType) {
        //     errors.chatType = LangHelper.getResource('ChatTypeRequired');
        // }

        // if (!values.systemId) {
        //     errors.systemId = LangHelper.getResource('SystemRequired');
        // }


        return errors;
    }

    recipientTypeChange = value => this.handlePopupChange('recipientTypes', value)
    chatTypeChange = value => this.handlePopupChange('chatTypes', value)
    systemChange = value => this.handlePopupChange('systemId', value)

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
                        <Form.Label column sm="4" md="3" htmlFor="NotificationTypeName" className="required">
                            {LangHelper.getResource('NotificationTypeName')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control type="text" size="sm" value={values.NotificationTypeName} name="NotificationTypeName"
                                className="shadow-none" autoComplete="off"
                                isInvalid={touched.NotificationTypeName && errors.NotificationTypeName}
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
                            initialValue={this.state.popup.recipientTypes}
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
                            initialValue={this.state.popup.chatTypes}
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
                        <Form.Label column sm="4" md="3" htmlFor="systemId" className="required">
                            {LangHelper.getResource('System')}
                        </Form.Label>
                        <Col sm="8" md="9">
                            <Form.Control as="select" size="sm" className="shadow-none"
                                isInvalid={touched.SystemId && errors.SystemId}
                                name="SystemId"
                                onChange={handleChange}
                                value={values.SystemId}
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
        setSubmitting(true);
        this.props.createType(values).then(() => {
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
            NotificationTypeName: '',
            recipientType: '',
            chatType: '',
            SystemId: ''
        }
        return (
            <Modal show className="new-notification-Type"
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