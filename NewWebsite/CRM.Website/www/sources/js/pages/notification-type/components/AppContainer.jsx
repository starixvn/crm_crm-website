import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('IsHost').checked;
    }

    componentDidMount() {
        this.props.getNotificationTypes();
    }

    render() {
        return (
            <Fragment>
                <Filter />
                <DataGrid isHost={this.isHost} />
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const { gridParams } = state;
    return {
        gridParams
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);