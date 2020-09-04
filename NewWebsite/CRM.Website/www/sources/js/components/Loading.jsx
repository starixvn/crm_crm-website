import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';

class Loading extends PureComponent {
    static propTypes = {
        width: PropTypes.number,
        height: PropTypes.number
    }

    static defaultProps = {
        width: 18,
        height: 18
    }

    renderChildCircle = () => {
        const numOfChildCircle = 12;
        const childs = [];
        for (let i = 1; i <= numOfChildCircle; i++) {
            childs.push(<div key={`loading-${i}`} className={`sk-circle${i} sk-circle`} />);
        }

        return childs;
    }

    render() {
        const { width, height } = this.props;
        return (
            <div className="sk-fading-circle" style={{ width: width, height: height }}>
                {
                    this.renderChildCircle()
                }
            </div>
        );
    }
}

export default Loading;