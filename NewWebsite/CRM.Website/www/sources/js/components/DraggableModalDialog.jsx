import React, { Component } from 'react';
import { ModalDialog } from 'react-bootstrap';
import Draggable from 'react-draggable';

class DraggableModalDialog extends Component {
    render() {
        return (
            <Draggable handle=".modal-header">
                <ModalDialog {...this.props} />
            </Draggable>
        );
    }
}

export default DraggableModalDialog;