import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Alert, Button, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';

class ConfirmPopup extends PureComponent {
    static propTypes = {
        size: PropTypes.oneOf(['sm', 'lg', 'xl']),
        title: PropTypes.string,
        content: PropTypes.oneOfType([
            PropTypes.string,
            PropTypes.element
        ]),
        requestResponse: PropTypes.shape({
            isSuccessful: PropTypes.bool,
            message: PropTypes.string
        }),
        onSubmit: PropTypes.func,
        onClose: PropTypes.func
    }

    static defaultProps = {
        size: 'sm',
        title: '',
        content: '',
        requestResponse: null
    }

    render() {
        const { size, title, content, requestResponse } = this.props;
        return (
            <Modal show backdrop="static"
                keyboard={false} size={size} centered
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        requestResponse &&
                        <Alert variant={requestResponse.isSuccessful ? 'success' : 'danger'} className="d-flex">
                            <div><i className={`fa ${requestResponse.isSuccessful ? 'fa-check' : 'fa-exclamation-triangle'}`} /></div>
                            <div className="ml-2">{requestResponse.message}</div>
                        </Alert>
                    }
                    {content}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" size="sm" onClick={this.props.onSubmit}>
                        {LangHelper.getResource('OK')}
                    </Button>
                    <Button variant="primary" size="sm" onClick={this.props.onClose}>
                        {LangHelper.getResource('Cancel')}
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

export default ConfirmPopup;