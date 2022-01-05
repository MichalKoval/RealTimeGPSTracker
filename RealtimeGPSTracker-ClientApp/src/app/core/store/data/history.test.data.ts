import { of } from 'rxjs';
import { DistanceUnit, IHistoryItem } from '../../models/history.model';
import { DeviceStatus } from '../../models/device.model';

export const historyItems: IHistoryItem[] = [
    {
        id: '1',
        datetimeStart: '03-06-2020 10:07:18',
        datetimeEnd: '03-06-2020 10:10:20',
        title: 'Today trip',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: of({
            id: "00000000-0000-0000-0000-000000000004",
            name: "Device4",
            createTime: "5/1/2020 12:04:00 AM",
            color: "#9fd21f",
            interval: 5000,
            status: DeviceStatus.OFFLINE
        }),
        coordinates$: of([{id:"",time:"",lat:50.10012,lng:14.39526,speed:0},{id:"",time:"",lat:50.10012,lng:14.39533,speed:0},{id:"",time:"",lat:50.10011,lng:14.39547,speed:0},{id:"",time:"",lat:50.10012,lng:14.39555,speed:0},{id:"",time:"",lat:50.10014,lng:14.39569,speed:0},{id:"",time:"",lat:50.10016,lng:14.39574,speed:0},{id:"",time:"",lat:50.10019,lng:14.39584,speed:0},{id:"",time:"",lat:50.10024,lng:14.39593,speed:0},{id:"",time:"",lat:50.10028,lng:14.396,speed:0},{id:"",time:"",lat:50.10026,lng:14.39604,speed:0},{id:"",time:"",lat:50.1002,lng:14.39605,speed:0},{id:"",time:"",lat:50.10012,lng:14.39618,speed:0},{id:"",time:"",lat:50.10005,lng:14.3963,speed:0},{id:"",time:"",lat:50.1,lng:14.39638,speed:0},{id:"",time:"",lat:50.09971,lng:14.39692,speed:0},{id:"",time:"",lat:50.09957,lng:14.39719,speed:0},{id:"",time:"",lat:50.09952,lng:14.39728,speed:0},{id:"",time:"",lat:50.09948,lng:14.39737,speed:0},{id:"",time:"",lat:50.09942,lng:14.39749,speed:0},{id:"",time:"",lat:50.09936,lng:14.39761,speed:0},{id:"",time:"",lat:50.09921,lng:14.39791,speed:0},{id:"",time:"",lat:50.09896,lng:14.3984,speed:0},{id:"",time:"",lat:50.09891,lng:14.39849,speed:0},{id:"",time:"",lat:50.0987,lng:14.39897,speed:0},{id:"",time:"",lat:50.09846,lng:14.39951,speed:0},{id:"",time:"",lat:50.0984,lng:14.39965,speed:0},{id:"",time:"",lat:50.09835,lng:14.39975,speed:0},{id:"",time:"",lat:50.09831,lng:14.39985,speed:0},{id:"",time:"",lat:50.09827,lng:14.39993,speed:0},{id:"",time:"",lat:50.098,lng:14.40072,speed:0},{id:"",time:"",lat:50.09774,lng:14.40147,speed:0},{id:"",time:"",lat:50.09771,lng:14.40158,speed:0},{id:"",time:"",lat:50.09771,lng:14.40168,speed:0},{id:"",time:"",lat:50.09773,lng:14.40191,speed:0},{id:"",time:"",lat:50.09778,lng:14.40249,speed:0},{id:"",time:"",lat:50.0978,lng:14.40269,speed:0},{id:"",time:"",lat:50.09783,lng:14.40315,speed:0},{id:"",time:"",lat:50.09789,lng:14.40429,speed:0},{id:"",time:"",lat:50.09791,lng:14.40456,speed:0},{id:"",time:"",lat:50.09791,lng:14.4046,speed:0},{id:"",time:"",lat:50.09789,lng:14.40462,speed:0},{id:"",time:"",lat:50.09785,lng:14.40467,speed:0},{id:"",time:"",lat:50.09776,lng:14.40471,speed:0},{id:"",time:"",lat:50.09774,lng:14.40472,speed:0},{id:"",time:"",lat:50.09762,lng:14.40477,speed:0},{id:"",time:"",lat:50.09751,lng:14.40483,speed:0},{id:"",time:"",lat:50.09761,lng:14.40539,speed:0},{id:"",time:"",lat:50.09764,lng:14.40556,speed:0},{id:"",time:"",lat:50.09771,lng:14.40606,speed:0},{id:"",time:"",lat:50.09774,lng:14.40629,speed:0},{id:"",time:"",lat:50.09777,lng:14.40655,speed:0},{id:"",time:"",lat:50.0978,lng:14.40692,speed:0},{id:"",time:"",lat:50.09781,lng:14.40712,speed:0},{id:"",time:"",lat:50.09782,lng:14.40723,speed:0},{id:"",time:"",lat:50.09786,lng:14.40745,speed:0},{id:"",time:"",lat:50.09788,lng:14.40756,speed:0},{id:"",time:"",lat:50.09779,lng:14.40758,speed:0},{id:"",time:"",lat:50.09767,lng:14.40761,speed:0},{id:"",time:"",lat:50.09758,lng:14.40764,speed:0},{id:"",time:"",lat:50.09752,lng:14.40766,speed:0},{id:"",time:"",lat:50.09746,lng:14.40768,speed:0},{id:"",time:"",lat:50.09738,lng:14.40773,speed:0},{id:"",time:"",lat:50.097,lng:14.40798,speed:0},{id:"",time:"",lat:50.09689,lng:14.40806,speed:0},{id:"",time:"",lat:50.0967,lng:14.40819,speed:0},{id:"",time:"",lat:50.09663,lng:14.40823,speed:0},{id:"",time:"",lat:50.09656,lng:14.40827,speed:0},{id:"",time:"",lat:50.09647,lng:14.40832,speed:0},{id:"",time:"",lat:50.09644,lng:14.40834,speed:0},{id:"",time:"",lat:50.0964,lng:14.40854,speed:0},{id:"",time:"",lat:50.09639,lng:14.40863,speed:0},{id:"",time:"",lat:50.09631,lng:14.40889,speed:0},{id:"",time:"",lat:50.0963,lng:14.4089,speed:0},{id:"",time:"",lat:50.09618,lng:14.40912,speed:0},{id:"",time:"",lat:50.09611,lng:14.40926,speed:0},{id:"",time:"",lat:50.09607,lng:14.40942,speed:0}]),
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '2',
        datetimeStart: '03-06-2020 11:15:00',
        datetimeEnd: '03-06-2020 11:20:00',
        title: 'Yesterday trip',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: of({
            id: "00000000-0000-0000-0000-000000000006",
            name: "Device6",
            createTime: "7/1/2020 12:06:00 AM",
            color: "#872fc6",
            interval: 4000,
            status: DeviceStatus.OFFLINE
        }),
        coordinates$: of([{id:"",time:"",lat:50.76681,lng:15.05292,speed:0},{id:"",time:"",lat:50.76667,lng:15.05271,speed:0},{id:"",time:"",lat:50.76665,lng:15.05255,speed:0},{id:"",time:"",lat:50.76658,lng:15.05232,speed:0},{id:"",time:"",lat:50.76638,lng:15.05196,speed:0},{id:"",time:"",lat:50.76582,lng:15.05269,speed:0},{id:"",time:"",lat:50.76576,lng:15.05275,speed:0},{id:"",time:"",lat:50.76574,lng:15.05272,speed:0},{id:"",time:"",lat:50.76573,lng:15.05269,speed:0},{id:"",time:"",lat:50.76504,lng:15.05161,speed:0},{id:"",time:"",lat:50.76472,lng:15.05106,speed:0},{id:"",time:"",lat:50.76405,lng:15.05008,speed:0},{id:"",time:"",lat:50.76282,lng:15.04843,speed:0},{id:"",time:"",lat:50.76279,lng:15.04838,speed:0},{id:"",time:"",lat:50.76267,lng:15.04823,speed:0},{id:"",time:"",lat:50.76265,lng:15.04819,speed:0},{id:"",time:"",lat:50.76216,lng:15.04753,speed:0},{id:"",time:"",lat:50.76195,lng:15.04713,speed:0},{id:"",time:"",lat:50.76187,lng:15.04695,speed:0},{id:"",time:"",lat:50.76135,lng:15.04734,speed:0},{id:"",time:"",lat:50.7591,lng:15.0492,speed:0},{id:"",time:"",lat:50.75808,lng:15.05013,speed:0},{id:"",time:"",lat:50.75653,lng:15.05138,speed:0},{id:"",time:"",lat:50.7561,lng:15.05169,speed:0},{id:"",time:"",lat:50.75587,lng:15.0519,speed:0},{id:"",time:"",lat:50.75559,lng:15.05212,speed:0},{id:"",time:"",lat:50.75556,lng:15.05215,speed:0},{id:"",time:"",lat:50.75526,lng:15.05228,speed:0},{id:"",time:"",lat:50.75522,lng:15.05229,speed:0},{id:"",time:"",lat:50.75461,lng:15.05229,speed:0},{id:"",time:"",lat:50.75405,lng:15.05224,speed:0},{id:"",time:"",lat:50.75366,lng:15.05223,speed:0},{id:"",time:"",lat:50.75342,lng:15.05219,speed:0},{id:"",time:"",lat:50.7532,lng:15.05219,speed:0},{id:"",time:"",lat:50.75293,lng:15.05223,speed:0},{id:"",time:"",lat:50.75281,lng:15.05227,speed:0},{id:"",time:"",lat:50.7527,lng:15.05232,speed:0},{id:"",time:"",lat:50.75268,lng:15.05234,speed:0},{id:"",time:"",lat:50.75262,lng:15.05234,speed:0},{id:"",time:"",lat:50.75228,lng:15.05252,speed:0},{id:"",time:"",lat:50.75227,lng:15.0525,speed:0},{id:"",time:"",lat:50.75226,lng:15.05249,speed:0},{id:"",time:"",lat:50.75226,lng:15.05248,speed:0},{id:"",time:"",lat:50.75225,lng:15.05248,speed:0},{id:"",time:"",lat:50.75224,lng:15.05247,speed:0},{id:"",time:"",lat:50.75223,lng:15.05247,speed:0},{id:"",time:"",lat:50.75222,lng:15.05246,speed:0},{id:"",time:"",lat:50.75216,lng:15.05246,speed:0},{id:"",time:"",lat:50.75216,lng:15.05247,speed:0},{id:"",time:"",lat:50.75215,lng:15.05247,speed:0},{id:"",time:"",lat:50.75214,lng:15.05248,speed:0},{id:"",time:"",lat:50.75212,lng:15.05249,speed:0},{id:"",time:"",lat:50.75211,lng:15.05251,speed:0},{id:"",time:"",lat:50.75211,lng:15.05252,speed:0},{id:"",time:"",lat:50.7521,lng:15.05253,speed:0},{id:"",time:"",lat:50.75209,lng:15.05257,speed:0},{id:"",time:"",lat:50.75194,lng:15.0526,speed:0},{id:"",time:"",lat:50.75188,lng:15.05263,speed:0},{id:"",time:"",lat:50.75151,lng:15.05261,speed:0},{id:"",time:"",lat:50.75128,lng:15.05253,speed:0},{id:"",time:"",lat:50.75121,lng:15.05252,speed:0},{id:"",time:"",lat:50.75101,lng:15.05253,speed:0},{id:"",time:"",lat:50.75096,lng:15.05254,speed:0},{id:"",time:"",lat:50.7508,lng:15.05262,speed:0},{id:"",time:"",lat:50.75068,lng:15.05274,speed:0},{id:"",time:"",lat:50.75062,lng:15.05284,speed:0},{id:"",time:"",lat:50.75043,lng:15.05328,speed:0},{id:"",time:"",lat:50.75039,lng:15.05334,speed:0},{id:"",time:"",lat:50.75036,lng:15.05337,speed:0},{id:"",time:"",lat:50.74971,lng:15.05372,speed:0},{id:"",time:"",lat:50.74966,lng:15.05373,speed:0},{id:"",time:"",lat:50.74847,lng:15.05383,speed:0},{id:"",time:"",lat:50.74837,lng:15.0539,speed:0},{id:"",time:"",lat:50.74833,lng:15.05394,speed:0},{id:"",time:"",lat:50.7483,lng:15.054,speed:0},{id:"",time:"",lat:50.74823,lng:15.05426,speed:0},{id:"",time:"",lat:50.74822,lng:15.05426,speed:0},{id:"",time:"",lat:50.74822,lng:15.05427,speed:0},{id:"",time:"",lat:50.74814,lng:15.05445,speed:0},{id:"",time:"",lat:50.74803,lng:15.05457,speed:0},{id:"",time:"",lat:50.74798,lng:15.0546,speed:0},{id:"",time:"",lat:50.74791,lng:15.05462,speed:0},{id:"",time:"",lat:50.74752,lng:15.0546,speed:0},{id:"",time:"",lat:50.74694,lng:15.05453,speed:0},{id:"",time:"",lat:50.7454,lng:15.05449,speed:0},{id:"",time:"",lat:50.74526,lng:15.05451,speed:0},{id:"",time:"",lat:50.74508,lng:15.05456,speed:0},{id:"",time:"",lat:50.74489,lng:15.05458,speed:0},{id:"",time:"",lat:50.74477,lng:15.05452,speed:0},{id:"",time:"",lat:50.7447,lng:15.05471,speed:0},{id:"",time:"",lat:50.74463,lng:15.05484,speed:0},{id:"",time:"",lat:50.74455,lng:15.05496,speed:0},{id:"",time:"",lat:50.74446,lng:15.05507,speed:0},{id:"",time:"",lat:50.74432,lng:15.05518,speed:0},{id:"",time:"",lat:50.74431,lng:15.05514,speed:0},{id:"",time:"",lat:50.7443,lng:15.05512,speed:0},{id:"",time:"",lat:50.74428,lng:15.05511,speed:0},{id:"",time:"",lat:50.74427,lng:15.05509,speed:0},{id:"",time:"",lat:50.74425,lng:15.05507,speed:0},{id:"",time:"",lat:50.74422,lng:15.05505,speed:0},{id:"",time:"",lat:50.74417,lng:15.05504,speed:0},{id:"",time:"",lat:50.74411,lng:15.05504,speed:0},{id:"",time:"",lat:50.74406,lng:15.05507,speed:0},{id:"",time:"",lat:50.74401,lng:15.05512,speed:0},{id:"",time:"",lat:50.74401,lng:15.05513,speed:0},{id:"",time:"",lat:50.74399,lng:15.05515,speed:0},{id:"",time:"",lat:50.74381,lng:15.05506,speed:0},{id:"",time:"",lat:50.7437,lng:15.05503,speed:0},{id:"",time:"",lat:50.74367,lng:15.05501,speed:0},{id:"",time:"",lat:50.74362,lng:15.05499,speed:0},{id:"",time:"",lat:50.74354,lng:15.05497,speed:0},{id:"",time:"",lat:50.74344,lng:15.05497,speed:0},{id:"",time:"",lat:50.74322,lng:15.055,speed:0},{id:"",time:"",lat:50.74275,lng:15.05513,speed:0}]),
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '3',
        datetimeStart: '07-06-2020 09:11:15',
        datetimeEnd: '07-06-2020 10:10:10',
        title: 'title3',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '4',
        datetimeStart: '01-06-2020 10:11:15',
        datetimeEnd: '01-06-2020 14:11:15',
        title: 'title4',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '5',
        datetimeStart: '07-06-2020 23:00:00',
        datetimeEnd: '08-06-2020 01:30:00',
        title: 'title5',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '6',
        datetimeStart: '01-01-2019 11:11:11',
        datetimeEnd: '01-01-2019 12:12:12',
        title: 'title6',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '7',
        datetimeStart: '01-03-2019 13:13:13',
        datetimeEnd: '01-03-2019 15:15:15',
        title: 'title7',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    },
    {
        id: '8',
        datetimeStart: '01-12-2019 12:12:12',
        datetimeEnd: '01-12-2019 20:20:20',
        title: 'title8',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false                
    },
    {
        id: '9',
        datetimeStart: '01-08-2019 18:17:16',
        datetimeEnd: '01-08-2019 19:18:17',
        title: 'title9',
        duration: '0 s',
        distance: '0 m',
        distanceUnit: DistanceUnit.KM,
        device$: null,
        coordinates$: null,
        
        //flags
        isSelected: false,
        isShownOnMap: false,
        toDelete: false
    }
]