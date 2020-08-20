import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form } from 'react-bootstrap';
import { ActionCreators } from '../actions';
import LangHelper from '@jsroot/common/langHelper';

const Filter = props => {
    const [notificationTypeId, handleNotificationTypeChange] = useState('');
    const [notificationTemplateName, handleTemplateNameChange] = useState('');

    function getParams() {
        return {
            notificationTypeId: parseInt(notificationTypeId, 10),
            notificationTemplateName: notificationTemplateName,
            pageIndex: 1,
            pageSize: 100
        };
    }

    function submitHandler(event) {
        event.preventDefault();
        const params = getParams();
        props.setGridParams(params);
        props.getGridData(params);
    }

    function handleOpenCreatePopup() {
        props.setCreateTemplatePopup({ show: true });
    }

    return (
        <Form className="form-filter" onSubmit={submitHandler}>
            <Form.Row bsPrefix='form-row'>
                <Form.Group>
                    <Form.Label htmlFor="filterNotificationTypeId">{LangHelper.getResource('NotificationType')}</Form.Label>
                    <Form.Control as="select" size="sm" className="shadow-none" id="filterNotificationTypeId"
                        value={notificationTypeId}
                        onChange={event => handleNotificationTypeChange(event.target.value)}
                    >
                        <option value="">{LangHelper.getResource('All')}</option>
                        {
                            props.notificationTypes.map(item => {
                                return (
                                    <option key={item.notificationTypeId} value={item.notificationTypeId}>
                                        {item.notificationTypeName}
                                    </option>
                                );
                            })
                        }
                    </Form.Control>
                </Form.Group>
                <Form.Group>
                    <Form.Label htmlFor="filterNotificationTemplateName">{LangHelper.getResource('TemplateName')}</Form.Label>
                    <Form.Control type="text" size="sm" className="shadow-none"
                        id="filterNotificationTemplateName" autoComplete="off"
                        value={notificationTemplateName}
                        onChange={event => handleTemplateNameChange(event.target.value)}
                    />
                </Form.Group>
                <div>
                    <Button className="mr-2" variant="primary" size="sm"
                        onClick={submitHandler}
                    >
                        {LangHelper.getResource('Submit')}
                    </Button>
                    {
                        props.isHost &&
                        <Button variant="primary" size="sm"
                            onClick={handleOpenCreatePopup}
                        >
                            {LangHelper.getResource('Insert')}
                        </Button>
                    }
                </div>
            </Form.Row>
        </Form>
    );
};

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { notificationTypes } = state;
    return { notificationTypes };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);