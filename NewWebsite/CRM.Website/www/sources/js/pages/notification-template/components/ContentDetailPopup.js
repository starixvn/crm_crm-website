import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Accordion, Button, Card, Modal } from 'react-bootstrap';
import LangHelper from '@jsroot/common/langHelper';

class ContentDetailPopup extends Component {
    render() {
        const template = this.props.selectedTemplate;
        return (
            <Modal show className="new-notification-template"
                backdrop="static" keyboard={false} size="lg" centered
                onHide={this.props.onClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="div">{template.title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        template.contents.map(item => {
                            return (
                                <Accordion key={item.languageId} defaultActiveKey="0">
                                    <Card>
                                        <Card.Header>
                                            <Accordion.Toggle as="div" eventKey="0">
                                                <a href="#">{item.language}</a>
                                            </Accordion.Toggle>
                                        </Card.Header>
                                        <Accordion.Collapse eventKey="0">
                                            <Card.Body>{item.content}</Card.Body>
                                        </Accordion.Collapse>
                                    </Card>
                                </Accordion>
                            );
                        })
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" size="sm" onClick={this.props.onClose}>
                        {LangHelper.getResource('Close')}
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

const mapStateToProps = state => {
    const { selectedTemplate } = state;
    return { selectedTemplate };
};

export default connect(mapStateToProps)(ContentDetailPopup);