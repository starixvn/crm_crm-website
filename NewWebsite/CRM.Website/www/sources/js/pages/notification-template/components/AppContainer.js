import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import NewNotificationTemplate from './NewNotificationTemplate';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('IsHost').checked;
    }

    componentDidMount() {
        this.props.getNotificationTypes();
    }

    handleCloseCreatePopup = () => this.props.setCreateTemplatePopup({ show: false })

    handleCreateSuccess = () => {
        this.handleCloseCreatePopup();
        this.props.setCreateTemplateResult(null);
        this.props.getGridData(this.props.gridParams);
    }

    render() {
        return (
            <Fragment>
                <Filter isHost={this.isHost} />
                <DataGrid isHost={this.isHost} />
                {
                    this.props.createTemplatePopup.show &&
                    <NewNotificationTemplate
                        onClose={this.handleCloseCreatePopup}
                        onSuccess={this.handleCreateSuccess}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { gridParams, createTemplatePopup } = state;
    return {
        gridParams,
        createTemplatePopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);