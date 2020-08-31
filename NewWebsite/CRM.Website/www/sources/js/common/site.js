import { MappingStatus } from './constants';
const _rootUrl = document.getElementById('RootUrl') ? document.getElementById('RootUrl').value : '/';
let _xsrfToken = null;

export default class Site {
    static blockUI() {
        document.getElementById('loadingProcess').style.display = 'block';
    }

    static downloadFile(data, fileName) {
        const url = window.URL.createObjectURL(new Blob([data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', fileName);
        document.body.appendChild(link);
        link.click();
        link.parentNode.removeChild(link);
        window.URL.revokeObjectURL(url);
    }

    static getXsrfToken() {
        if (!_xsrfToken && document.querySelector('input[name=__RequestVerificationToken]')) {
            _xsrfToken = document.querySelector('input[name=__RequestVerificationToken]').value;
        }

        return _xsrfToken;
    }

    static handleAxiosError(error) {
        if (error.response) {
            let url;
            switch (error.response.status) {
                case 401:
                    url = 'auth/login';
                    break;

                case 404:
                    url = 'error/PageNotFound';
                    break;

                case 405:
                    url = 'error/AccessDenied';
                    break;

                case 500:
                    url = 'error/InternalError';
                    break;
            }

            window.location = _rootUrl + url;
        }
    }

    static resolveClientUrl(relativeUrl) {
        return _rootUrl + relativeUrl;
    }

    static unblockUI() {
        document.getElementById('loadingProcess').style.display = 'none';
    }

    static mappingStatusClass(mappingStatusId) {
        switch (mappingStatusId) {
            case MappingStatus.BotRemoved:
                return 'badge-grey';
            case MappingStatus.NotMapped:
                return 'badge-red';
            case MappingStatus.Mapped:
                return 'badge-green';
            default:
                return '';
        }
    }

    static displayCopiedToClipboard() {
        const div = document.createElement('div');
        div.className = "copied-confirm";
        div.innerHTML = `Coppied to clipboard!`;
        document.body.appendChild(div);
        setTimeout(() => {
            div.remove();
        }, 1000);
    }

    static debounce(func, wait) {
        let timeout;
        return function (...theArgs) {
            const context = this,
                args = theArgs;
    
            const later = () => {
                func.apply(context, args);
            };
    
            clearTimeout(timeout);
            timeout = setTimeout(later, wait || 500);
        };
    };
}