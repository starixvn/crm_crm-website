import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import Filter from './Filter';
import DataGrid from './DataGrid';
import Content from './Content';
import ErrorResolvedContent from './ErrorResolvedContent';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.isHost = document.getElementById('is-host').checked;
    }

    render() {
        return (
            <Fragment>
                <Filter />
                <DataGrid isHost={this.isHost} />
                {
                    this.props.contentPopup.show &&
                    <Content
                        onClose={() => this.props.setContentPopup({ show: false })}
                        data={this.props.contentPopup.data}
                    />
                }
                {
                    this.props.errorOrResolvedContentPopup.show &&
                    <ErrorResolvedContent
                        onClose={() => this.props.setErrorOrResolvedContentPopup({ show: false })}
                        data={this.props.errorOrResolvedContentPopup.data}
                    />
                }
            </Fragment>
        );
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        gridParams,
        contentPopup,
        setContentPopup,
        errorOrResolvedContentPopup,
        setErrorOrResolvedContentPopup
    } = state;

    return {
        gridParams,
        contentPopup,
        setContentPopup,
        errorOrResolvedContentPopup,
        setErrorOrResolvedContentPopup
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);