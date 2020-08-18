import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import { Button, Col, Form, Modal, Row } from 'react-bootstrap';
import { Formik } from 'formik';
import MultipleSelectField from './MultipleSelectField';
import FormikErrorMessage from '@jsroot/components/FormikErrorMessage';
import ResponseMessage from '@jsroot/components/ResponseMessage';
import LangHelper from '@jsroot/common/langHelper';
import { ChatTypeMapping } from '../../../common/constants';

class EditGroupMappingPopup extends Component {
	constructor(props) {
		super(props);
	}

	renderOptions = (item, key) => <option key={key} value={item.Id} >{item.name}</option>

	validate = values => {
		const errors = {};
		if (!values.botId) {
			errors.botId = LangHelper.getResource('BotRequired');
		}

		if (!values.recipientType) {
			errors.recipientType = LangHelper.getResource('RecipientTypeRequired');
		}

		if (!values.headlineIds || values.headlineIds.trim().length === 0) {
			errors.headlineIds = LangHelper.getResource('GroupHeadlineRequired');
		}

		if (!values.recipientIds || values.recipientIds.trim().length === 0) {
			errors.recipientIds = LangHelper.getResource('RecipientRequired');
		}

		if (!values.languageId || values.languageId.trim().length === 0) {
			errors.languageId = LangHelper.getResource('LanguageRequired');
		}

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
						this.props.personalMappingResult &&
						<ResponseMessage isSuccessful={this.props.personalMappingResult.isSuccessful}
							message={this.props.personalMappingResult.message || LangHelper.getResource('InsertNotificationTemplateSuccessfully')}
						/>
					}
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('BotName')}
						</Form.Label>
						<Col sm="8" md="9">
							<Form.Control as="select" size="sm" className="shadow-none"
								isInvalid={touched.botId && errors.botId}
								name="botName"
								onChange={handleChange}
								value={values.botId}
							>
								{this.props.settings.bots.map(this.renderOptions)}
							</Form.Control>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('Name')}
						</Form.Label>
						<Col sm="8" md="9">
							<div>{values.name}</div>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('ChatType')}
						</Form.Label>
						<Col sm="8" md="9">
							<div>{LangHelper.getResource('Personal')}</div>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('RecipientType')}
						</Form.Label>
						<Col sm="8" md="9">
							<Form.Control as="select" size="sm" className="shadow-none"
								isInvalid={touched.recipientTypeId && errors.recipientTypeId}
								name="recipientTypeId"
								onChange={handleChange}
								value={values.recipientTypeId}
							>
								<option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
								{this.props.settings.recipientTypes.map(this.renderOptions)}
							</Form.Control>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('GroupHeadline')}
						</Form.Label>
						<Col sm="8" md="9">
							<Form.Control as="select" size="sm" className="shadow-none"
								isInvalid={touched.headlineIds && errors.headlineIds}
								name="headlineIds"
								onChange={handleChange}
								value={values.headlineIds}
							>
								<option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
								{this.props.settings.groupHeadline.map(this.renderOptions)}
							</Form.Control>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('Recipient')}
						</Form.Label>
						<Col sm="8" md="9">
							<MultipleSelectField
								width={'100%'}
								initialValue={''}
								data={this.props.recipients || []}
								keyField='id'
								valueField='name'
								statusField='checked'
								isSummited={false}
								isAllTextShown
								onChange={handleChange}
							/>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('LetstalkID')}
						</Form.Label>
						<Col sm="8" md="9">
							<div></div>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('Language')}
						</Form.Label>
						<Col sm="8" md="9">
							<Form.Control as="select" size="sm" className="shadow-none"
								isInvalid={touched.languageId && errors.languageId}
								name="languageId"
								onChange={handleChange}
								value={values.languageId}
							>
								<option value="" disabled hidden>{LangHelper.getResource('PleaseSelect')}</option>
								{this.props.settings.languages.map(this.renderOptions)}
							</Form.Control>
						</Col>
					</Form.Group>
					<Form.Group as={Row}>
						<Form.Label column sm="4" md="3" className="required">
							{LangHelper.getResource('Remark')}
						</Form.Label>
						<Col sm="8" md="9">
							<Form.Control as="textarea" size="sm" className="shadow-none"
								rows={5}
								isInvalid={touched.remark && errors.remark}
								name="remark"
								onChange={handleChange}
								value={values.remark}
							>
							</Form.Control>
						</Col>
					</Form.Group>
				</Modal.Body>

				<Modal.Footer>
					<Button type="submit" variant="primary" size="sm" disabled={isSubmitting}>
						{LangHelper.getResource('Submit')}
					</Button>
					<Button type="button" variant="primary" size="sm">
						{LangHelper.getResource('Cancel')}
					</Button>
				</Modal.Footer>
			</Form>
		);
	}

	handleSubmit = (values, { setSubmitting }) => {
		console.log(values);
		//setSubmitting(true);
		//const template = {
		//	notificationTypeId: values.notificationTypeId,
		//	name: values.name,
		//	title: values.title,
		//	contents: []
		//};
		//this.props.languages.map(item => {
		//	template.contents.push({
		//		languageId: item.settingId,
		//		content: values['language_' + item.settingId]
		//	});
		//});

		//this.props.createTemplate(template).then(() => {
		//	if (this.props.createTemplateResult.isSuccessful) {
		//		setTimeout(() => {
		//			this.props.onSuccess();
		//		}, 3000);
		//	} else {
		//		setSubmitting(false);
		//	}
		//});
	}

	render() {
		const initialValues = {
			botId: '',
			chatTypeId: ChatTypeMapping.Personal,
			recipientTypeId: '',
			headlineIds: '',
			recipients: null,
			languageId: '',
			remark: ''
		};

		return (
			<Modal show className="new-notification-template"
				backdrop="static" keyboard={false} size="md" centered
				onHide={this.props.onClose}
			>
				<Modal.Header closeButton>
					<Modal.Title as="div">{LangHelper.getResource('InsertPersonalMapping')}</Modal.Title>
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

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
	const { settings } = state;
	return {
		settings
	};
};

export default connect(mapStateToProps, mapDispatchToProps)(EditGroupMappingPopup);