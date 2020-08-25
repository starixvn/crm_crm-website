export const getNowGMTAdd8 = () => {
    const now = new Date();
    now.setHours(now.getHours() - 8 - (now.getTimezoneOffset() / 60));
    return now
};

export const convertDateToString = date => {
    var month = date.getMonth() + 1;
    var dd = date.getDate();
  
    return [
        (month > 9 ? '' : '0') + month,
        (dd > 9 ? '' : '0') + dd,
        date.getFullYear()
    ].join('/');
}

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
}

const functions = {
    getNowGMTAdd8,
    convertDateToString,
    debounce
}

export default functions;