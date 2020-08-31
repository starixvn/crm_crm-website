export const convertDateToString = date => {
    var month = date.getMonth() + 1;
    var dd = date.getDate();

    return [
        month,
        dd,
        date.getFullYear()
    ].join('/');
};

export const debounce = (func, wait) => {
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