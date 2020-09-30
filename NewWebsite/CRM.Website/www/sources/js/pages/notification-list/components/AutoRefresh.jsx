import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import LangHelper from '@jsroot/common/langHelper';
import functions from '../functions';
import { AUTO_REFRESH_TIMES } from '@jsroot/common/constants';

export class AutoRefresh extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isOpenDropdown: false
        }

        this.autoRefresh = React.createRef();
        this.handleClickOutside = this.handleClickOutside.bind(this);
    }

    componentDidMount = () => {
        this.countDownFunc();
        document.addEventListener('mousedown', this.handleClickOutside);
    }

    componentWillUnmount = () => {
        clearInterval(this.countDownFunc);
        document.removeEventListener('mousedown', this.handleClickOutside)
    }

    handleClickOutside(event) {
        if (this.autoRefresh && !this.autoRefresh.current.contains(event.target) && this.state.isOpenDropdown) {
            this.setState({ isOpenDropdown: false });
        }
    }

    componentDidUpdate = (prevProps) => {
        if (prevProps.autoRefresh.value !== this.props.autoRefresh.value) {
            this.props.setAutoRefresh({
                ...this.props.autoRefresh,
                countDown: this.props.autoRefresh.value
            });
        }

        if (this.props.autoRefresh.countDown === 0 && this.props.isRefresh && this.props.autoRefresh.value > 0) {
            this.props.setAutoRefresh({
                ...this.props.autoRefresh,
                countDown: this.props.autoRefresh.value
            });
            
            this.props.getGridData(this.props.gridParams, data => functions.handleNewResendOptions(data, this.props.setResendOptions));
        }
    }

    countDownFunc = () => setInterval(() => {
        if (this.props.isRefresh) {
            this.props.setAutoRefresh({
                ...this.props.autoRefresh,
                countDown: this.props.autoRefresh.countDown - 1
            });
        }
    }, 1000)

    handleAutoRefreshChange = value => {
        this.setState({ isOpenDropdown: false });
        this.props.setIsRefresh(value !== 0);
        this.props.setAutoRefresh({
            ...this.props.autoRefresh,
            value: value
        });
    }

    handleRefresh = () => {
        this.props.getGridData(this.props.gridParams, data => functions.handleNewResendOptions(data, this.props.setResendOptions));
        this.props.setAutoRefresh({
            ...this.props.autoRefresh,
            countDown: this.props.autoRefresh.value
        });
    }

    toggleAutoFreshDropdown = () =>
        this.setState({ isOpenDropdown: !this.state.isOpenDropdown })

    render() {
        return <div className='d-flex align-items-center' ref={this.autoRefresh}>
            <div className='auto-refresh'>
                <div className='d-flex justify-content-between align-items-center auto-refresh-value'
                    onClick={() => this.toggleAutoFreshDropdown()}
                >
                    <div>
                        {this.props.autoRefresh.value !== 0 ?
                            this.props.autoRefresh.countDown :
                            LangHelper.getResource('NotRefresh')}
                    </div>
                    <span className={`fa fa-caret-${this.state.isOpenDropdown ? 'up' : 'down'}`}/>
                </div>
                {
                    this.state.isOpenDropdown &&
                        <ul className='auto-refresh-dropdown'>
                            {
                                AUTO_REFRESH_TIMES.map(time =>
                                    <li key={time} value={time} onClick={() => this.handleAutoRefreshChange(time)}>{time}</li>)
                            }
                            <li onClick={() => this.handleAutoRefreshChange(0)} value={0}>{LangHelper.getResource('NotRefresh')}</li>
                        </ul>
                }
                
            </div>
            <span className='fa fa-refresh link-click' onClick={() => this.handleRefresh()}/>
        </div>;
    }
}

const mapDispatchToProps = dispatch => bindActionCreators(ActionCreators, dispatch);

const mapStateToProps = state => {
    const {
        gridParams,
        autoRefresh,
        isRefresh
    } = state;

    return {
        gridParams,
        autoRefresh,
        isRefresh
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AutoRefresh);