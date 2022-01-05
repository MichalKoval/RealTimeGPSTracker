import * as moment from 'moment';

export function formatToDateTimeInterval(datetime_s: string, datetime_e: string) {
    
    let date_s = moment.utc(datetime_s).format('MMMM DD');
    let date_e = moment.utc(datetime_e).format('MMMM DD');
    let time_s = moment.utc(datetime_s).format(' hh:mm A');
    let time_e = moment.utc(datetime_e).format(' hh:mm A');

    if (date_s === date_e) {
        return date_s + time_s + ' -' + time_e;

    } else {
        return date_s + time_s + ' - ' + date_e + time_e;
    }
}

export function formatToDateTimeIntervalLocalClient(datetime_s: string, datetime_e: string) {
    
    let date_s = moment.utc(datetime_s).local().format('MMMM DD');
    let date_e = moment.utc(datetime_e).local().format('MMMM DD');
    let time_s = moment.utc(datetime_s).local().format(' hh:mm A');
    let time_e = moment.utc(datetime_e).local().format(' hh:mm A');

    if (date_s === date_e) {
        return date_s + time_s + ' -' + time_e;

    } else {
        return date_s + time_s + ' - ' + date_e + time_e;
    }
}


/**
 * Converts local client DateTime to global UTC DateTime format
 * @param dateTime Local client DateTime
 * @returns String representing UTC DateTime
 */
export function convertFromLocalClientToUTC(dateTime: string) {
    return moment(dateTime, 'MMM DD YYYY HH:mm:ss').utc().format("YYYY-MM-DDTHH:mm:ss");
}

/**
 * Converts UTC DateTime to shortened UTC DateTime format (YYYYMMDDTHHmmss) used in URL queries.
 * @param dateTime UTC DateTime
 * @returns String representing shortened UTC DateTime
 */
export function convertFromUTCToShortUTC(dateTime: string) {
    return moment(dateTime, 'YYYY-MM-DDTHH:mm:ss').format("YYYYMMDDTHHmmss");
}

/**
 * Converts global UTC DateTime format to local client DateTime
 * @param dateTime UTC DateTime
 * @returns String representing local client DateTime
 */
export function convertFromUTCDatabaseToLocalClient(dateTime: string) {
    return moment.utc(dateTime, 'YYYY-MM-DDTHH:mm:ss').local().format("lll");
}

/**
 * Compares two DateTimes (milliseconds precision) in DateTime representation.
 * @param firstDateTime First DateTime in "YYYY-MM-DDTHH:mm:ss.sss" format.
 * @param secondDateTime Second DateTime in "YYYY-MM-DDTHH:mm:ss.sss" format.
 * @returns True if firstDateTime < secondDateTime. False if firstDateTime > secondDateTime.
 */
export function compareTwoDateTimes(firstDateTime: string, secondDateTime: string): boolean {
    return (moment(secondDateTime, "YYYY-MM-DDTHH:mm:ss").diff(moment(firstDateTime, "YYYY-MM-DDTHH:mm:ss"), 'seconds') > 0) ? true : false; 
}

/**
 * Compares two dates (day precision) in DateTime representation.
 * @param firstDateTime First DateTime in "YYYY-MM-DDTHH:mm:ss.sss" format.
 * @param secondDateTime Second DateTime in "YYYY-MM-DDTHH:mm:ss.sss" format.
 * @returns True if firstDateTime < secondDateTime. False if firstDateTime > secondDateTime.
 */
export function compareTwoDates(firstDateTime: string, secondDateTime: string): boolean {
    return (moment(secondDateTime, "YYYY-MM-DDTHH:mm:ss").diff(moment(firstDateTime, "YYYY-MM-DDTHH:mm:ss"), 'days') > 0) ? true : false; 
}

/**
 * Computes difference between two DateTimes and converts them in days, hours, minutes, seconds format.
 * @param firstDate First DateTime in "YYYY-MM-DDTHH:mm:ss.sss" format.
 * @param secondDate Second DateTime in "YYYY-MM-DDTHH:mm:ss.sss" format.
 * @returns String in days, hours, minutes, seconds format.
 * Example: If difference between two DateTimes is 65 seconds then string with '1 min and 5 s' will be returned.
 * For 3655 seconds difference the string with '1h, 0 min and 55 s' will be returned.
 * For 12073594 seconds difference the string with '139 d, 17 h, 46 min and 34 s' will be returned and so on ...
 *  and so on...
 */
export function durationBetweenDateTimes(firstDate: string, secondDate: string): string {
    var durationInSeconds = moment(secondDate, "YYYY-MM-DDTHH:mm:ss").diff(moment(firstDate, "YYYY-MM-DDTHH:mm:ss"), 'seconds');

    if (durationInSeconds <= 0) return "0 s";

    var seconds = durationInSeconds % 60;
    // Celociselne delenie ~~( vyraz )
    var durationInMinutes = ~~(durationInSeconds / 60);

    if (durationInMinutes == 0) return seconds.toString() + " s";


    var minutes = durationInMinutes % 60;
    var durationInHours = ~~(durationInMinutes / 60);

    if (durationInHours == 0) return minutes.toString() + " min and " + seconds.toString() + " s";
    
    var hours = durationInHours % 24;
    var days = ~~(durationInHours / 24);

    if (days == 0) return hours.toString() + " h, " + minutes.toString() + " min and " + seconds.toString() + " s";
    if (days > 0) return days.toString() + " d, " + hours.toString() + " h, " + minutes.toString() + " min and " + seconds.toString() + " s";
}

/**
 * Checks if DateTime represented in seconds is expired based on current DateTime. 
 * @param dateTimeInSeconds DateTime in seconds to check for expiration.
 * @returns True if DateTime represented in seconds is expired. Otherwise false.
 */
export function isExpiredDateTimeInSeconds(dateTimeInSeconds: number) {
    let dateTime = moment(dateTimeInSeconds*1000).format('YYYY-MM-DDTHH:mm:ss');
    
    return moment().isAfter(dateTime);
}

/**
 * Checks if DateTime is expired based on current DateTime. 
 * @param dateTimeInSeconds DateTime to check for expiration.
 * @returns True if DateTime is expired. Otherwise false.
 */
export function isExpiredDateTime(dateTime: string) {
    return moment().isAfter(dateTime);
}