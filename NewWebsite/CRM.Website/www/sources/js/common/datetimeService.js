import moment from 'moment';

const ShortDateFormat = 'MMM DD, YYYY';
const ShortDateTimeFormat = 'MMM DD, YYYY hh:mm a';
const LongDateTimeFormat = 'MMM DD, YYYY hh:mm:ss a';
const ClientTimezone = parseInt(document.getElementById('ClientTimezone') ? document.getElementById('ClientTimezone').value : '8', 10);
const ServerTimezone = parseInt(document.getElementById('ServerTimezone') ? document.getElementById('ServerTimezone').value : '-4', 10);

export default class DateTimeService {
    static startOfDay(date) {
        return moment(date).startOf('day');
    }

    static endOfDay(date) {
        return moment(date).endOf('day');
    }

    static firstDayOfWeek(date) {
        return moment(date).startOf('week');
    }

    static lastDayOfWeek(date) {
        return moment(date).endOf('week');
    }

    static firstDayOfMonth(date) {
        return moment(date).startOf('month');
    }

    static lastDayOfMonth(date) {
        return moment(date).endOf('month');
    }

    static toClientDateTime(date) {
        date = moment(date, LongDateTimeFormat);
        return moment.utc(date).utcOffset(ClientTimezone);
    }

    static toServerDateTime(date) {
        date = moment(date, LongDateTimeFormat);
        return moment(date).utc().utcOffset(ServerTimezone);
    }

    static toShortDateFormat(date) {
        return moment(date).format(ShortDateFormat);
    }

    static toShortDateTimeFormat(date) {
        return moment(date).format(ShortDateTimeFormat);
    }

    static toLongDateTimeFormat(date) {
        return moment(date).format(LongDateTimeFormat);
    }
}