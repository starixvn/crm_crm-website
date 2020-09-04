import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import Layout from '../../../components/Layout';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('is-host').checked;
    }

    render() {
        return (
            <Layout>
                <Filter isHost={this.isHost} />
                <DataGrid isHost={this.isHost} />

            </Layout>
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