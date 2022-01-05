import { Component, OnInit, AfterViewInit, OnDestroy, ElementRef, Input } from '@angular/core';
import * as leaflet from 'leaflet';
import * as uuid from 'uuid';
import { IMapData } from 'src/app/core/models/map.model';
import { ICoordinate } from 'src/app/core/models/coordinate.model';
import { randomHexColor, checkHexColor } from 'src/app/core/services/color.service';
//import * as  ResizeObserver from 'resize-observer-polyfill';

@Component({
    selector: 'app-map',
    templateUrl: './map.component.html',
    styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, OnDestroy, AfterViewInit {
    private _resize: boolean;

    @Input()
    set resize(resize: boolean) {
        this._resize = resize;
        
        // Fix map sizing after loading the map component
        setTimeout(() => {
            this._map.invalidateSize();
        }, 200);
    }

    get resize(): boolean {
        return this._resize;
    }
    
    // Map data to be showen
    private _mapData: IMapData = {
        routes: {

        },
        markers: leaflet.featureGroup(),
        markerIcon: leaflet.icon({
            iconRetinaUrl: 'assets/marker-icon-2x.png',
            iconUrl: 'assets/marker-icon.png',
            shadowUrl: 'assets/marker-shadow.png',
            iconSize: [25, 41],
            iconAnchor: [12, 41],
            popupAnchor: [1, -34],
            tooltipAnchor: [16, -28],
            shadowSize: [0, 0]
        })
    }
    
    // Map instance
    private _map: any;

    // Layer with base tiles
    private _baseLayer: any;

    // Every new instance of the map has unique element id to fix issues with "Map container is already instantiated"
    uniqueMapId: string;

    // Component resize event observer
    componentResizeObserver: ResizeObserver;

    private coordinates1 = [[50.10012,14.39526],[50.10012,14.39533],[50.10011,14.39547],[50.10012,14.39555],[50.10014,14.39569],[50.10016,14.39574],[50.10019,14.39584],[50.10024,14.39593],[50.10028,14.396],[50.10026,14.39604],[50.1002,14.39605],[50.10012,14.39618],[50.10005,14.3963],[50.1,14.39638],[50.09971,14.39692],[50.09957,14.39719],[50.09952,14.39728],[50.09948,14.39737],[50.09942,14.39749],[50.09936,14.39761],[50.09921,14.39791],[50.09896,14.3984],[50.09891,14.39849],[50.0987,14.39897],[50.09846,14.39951],[50.0984,14.39965],[50.09835,14.39975],[50.09831,14.39985],[50.09827,14.39993],[50.098,14.40072],[50.09774,14.40147],[50.09771,14.40158],[50.09771,14.40168],[50.09773,14.40191],[50.09778,14.40249],[50.0978,14.40269],[50.09783,14.40315],[50.09789,14.40429],[50.09791,14.40456],[50.09791,14.4046],[50.09789,14.40462],[50.09785,14.40467],[50.09776,14.40471],[50.09774,14.40472],[50.09762,14.40477],[50.09751,14.40483],[50.09761,14.40539],[50.09764,14.40556],[50.09771,14.40606],[50.09774,14.40629],[50.09777,14.40655],[50.0978,14.40692],[50.09781,14.40712],[50.09782,14.40723],[50.09786,14.40745],[50.09788,14.40756],[50.09779,14.40758],[50.09767,14.40761],[50.09758,14.40764],[50.09752,14.40766],[50.09746,14.40768],[50.09738,14.40773],[50.097,14.40798],[50.09689,14.40806],[50.0967,14.40819],[50.09663,14.40823],[50.09656,14.40827],[50.09647,14.40832],[50.09644,14.40834],[50.0964,14.40854],[50.09639,14.40863],[50.09631,14.40889],[50.0963,14.4089],[50.09618,14.40912],[50.09611,14.40926],[50.09607,14.40942],[50.096,14.40982],[50.09595,14.41016],[50.09589,14.41066],[50.09587,14.41096],[50.09587,14.41116],[50.09588,14.41127],[50.09594,14.41147],[50.09598,14.41156],[50.09608,14.41177],[50.0962,14.41202],[50.09625,14.41215],[50.0963,14.4123],[50.09638,14.41256],[50.09641,14.41268],[50.09645,14.4128],[50.09651,14.4132],[50.09657,14.4136],[50.09668,14.41435],[50.09673,14.41469],[50.09679,14.4151],[50.0968,14.41516],[50.09689,14.41571],[50.09692,14.41596],[50.09706,14.41683],[50.09714,14.41745],[50.09718,14.4177],[50.09732,14.41851],[50.09724,14.41853],[50.09723,14.41855],[50.09722,14.41856],[50.09722,14.41857],[50.09714,14.41865],[50.09709,14.41871],[50.09705,14.41876],[50.09703,14.41879],[50.09701,14.41882],[50.09699,14.41885],[50.09693,14.419],[50.0969,14.41912],[50.09697,14.41933],[50.097,14.41948],[50.09703,14.41967],[50.09705,14.41984],[50.09708,14.42005],[50.09711,14.42032],[50.09711,14.42039],[50.0971,14.42047],[50.09688,14.42151],[50.097,14.42146],[50.09703,14.42159],[50.09705,14.42165],[50.09704,14.42177],[50.09705,14.42187],[50.0971,14.42222],[50.09711,14.42227],[50.09713,14.4223],[50.09714,14.42234],[50.09714,14.42236],[50.09714,14.42242],[50.09693,14.42249],[50.0969,14.4225],[50.09696,14.42293],[50.09696,14.42299],[50.09692,14.42311],[50.09706,14.4241],[50.09707,14.42421],[50.09725,14.42549],[50.09726,14.42559],[50.09726,14.42562],[50.09726,14.42565],[50.09726,14.42568],[50.09725,14.4257],[50.09723,14.42574],[50.0972,14.42576],[50.09717,14.42577],[50.09708,14.4258],[50.09701,14.42583],[50.09714,14.42682],[50.09715,14.42693],[50.09719,14.42713],[50.0972,14.42726],[50.09714,14.42731],[50.09725,14.42766],[50.0973,14.42798],[50.09731,14.42808],[50.09734,14.4282],[50.09736,14.42836],[50.09744,14.42909],[50.09762,14.43068],[50.09763,14.43079],[50.09768,14.43206],[50.09772,14.43297],[50.09772,14.43309],[50.09772,14.4332],[50.09772,14.43325],[50.09772,14.43337],[50.09772,14.43344],[50.09768,14.43347],[50.09766,14.43349],[50.09765,14.43352],[50.09764,14.43354],[50.09764,14.43357],[50.09766,14.43368],[50.0978,14.43426],[50.09769,14.43432],[50.09768,14.43433],[50.09768,14.43434],[50.09755,14.43472],[50.09754,14.43476],[50.09754,14.43483],[50.09754,14.43495],[50.09745,14.435],[50.09732,14.43506],[50.09719,14.43513],[50.09706,14.43519],[50.09711,14.43546],[50.0971,14.43549],[50.0971,14.43552],[50.09712,14.43559],[50.09716,14.43575],[50.09735,14.43639],[50.09738,14.4365],[50.09741,14.43661],[50.09742,14.43664],[50.09748,14.4369],[50.09753,14.43713],[50.09754,14.43717],[50.09764,14.43763],[50.09765,14.43766],[50.09767,14.43774],[50.09768,14.43782],[50.09769,14.43784],[50.09777,14.43823],[50.0978,14.43834],[50.09781,14.4384],[50.09784,14.43846],[50.09797,14.43904],[50.09799,14.4391],[50.09801,14.43917],[50.09802,14.43924],[50.09804,14.43937],[50.09806,14.43952],[50.09811,14.43987],[50.09812,14.43998],[50.09812,14.44006],[50.09812,14.44009],[50.09812,14.44013],[50.0981,14.44021]];
    private coordinates2 = [[50.07909,14.50855],[50.07903,14.50851],[50.07899,14.50848],[50.07886,14.50835],[50.07881,14.50831],[50.07879,14.50828],[50.07875,14.50825],[50.07868,14.50819],[50.07866,14.50816],[50.07863,14.50814],[50.0785,14.50802],[50.07844,14.50797],[50.07836,14.50791],[50.07817,14.50774],[50.07802,14.50759],[50.078,14.50757],[50.07796,14.50753],[50.07796,14.50749],[50.07793,14.50743],[50.07791,14.50741],[50.0779,14.5074],[50.07789,14.50739],[50.07787,14.50736],[50.07782,14.50732],[50.07773,14.50725],[50.0777,14.50723],[50.07761,14.50722],[50.0776,14.50703],[50.0776,14.50692],[50.07759,14.50684],[50.07758,14.50673],[50.07755,14.50607],[50.07752,14.50582],[50.07749,14.50539],[50.07746,14.50503],[50.07745,14.50485],[50.07746,14.50468],[50.07744,14.50434],[50.07741,14.50392],[50.07739,14.50356],[50.07736,14.50302],[50.07732,14.50254],[50.07731,14.50232],[50.07729,14.50195],[50.07726,14.50154],[50.07725,14.50141],[50.07724,14.50133],[50.07722,14.50124],[50.0772,14.501],[50.07716,14.50038],[50.07715,14.50014],[50.07714,14.50003],[50.07714,14.49994],[50.07713,14.4998],[50.07712,14.49968],[50.07711,14.4995],[50.07711,14.4994],[50.0771,14.49929],[50.0771,14.49923],[50.0771,14.49911],[50.0771,14.49908],[50.07709,14.49899],[50.07708,14.49878],[50.07706,14.49843],[50.07704,14.49813],[50.07703,14.49787],[50.07701,14.49759],[50.07699,14.49731],[50.07697,14.49703],[50.07695,14.49676],[50.07694,14.49662],[50.07693,14.49652],[50.07693,14.49633],[50.07692,14.4962],[50.0769,14.49593],[50.07688,14.49565],[50.07686,14.49527],[50.07685,14.49514],[50.07684,14.49509],[50.07683,14.49502],[50.07683,14.49501],[50.07682,14.4949],[50.07681,14.49485],[50.0768,14.49476],[50.0768,14.49466],[50.07679,14.49457],[50.07679,14.49448],[50.07677,14.49422],[50.07676,14.4941],[50.07675,14.49393],[50.07674,14.49375],[50.07673,14.4936],[50.07672,14.49328],[50.07672,14.49316],[50.0767,14.49288],[50.07667,14.49238],[50.07667,14.49227],[50.07666,14.49208],[50.07663,14.49163],[50.07661,14.4912],[50.07659,14.49093],[50.07659,14.4908],[50.07659,14.49072],[50.07659,14.49058],[50.07663,14.49009],[50.07665,14.48992],[50.07668,14.48965],[50.07672,14.48936],[50.07673,14.48927],[50.07674,14.48915],[50.07675,14.48902],[50.07676,14.48885],[50.07678,14.48866],[50.07679,14.48857],[50.0768,14.48846],[50.0768,14.48845],[50.07683,14.48821],[50.07685,14.48792],[50.07687,14.48775],[50.0769,14.48754],[50.07691,14.48741],[50.07693,14.48716],[50.07696,14.48687],[50.077,14.48651],[50.07701,14.48637],[50.07702,14.4862],[50.07705,14.48595],[50.07705,14.48584],[50.07706,14.48565],[50.07711,14.48518],[50.07716,14.48474],[50.07717,14.48464],[50.07717,14.4846],[50.07718,14.48451],[50.07721,14.48428],[50.07726,14.4838],[50.07727,14.48371],[50.07728,14.48354],[50.07734,14.48295],[50.07751,14.48125],[50.07755,14.48074],[50.0776,14.48022],[50.07761,14.48017],[50.07762,14.48006],[50.07768,14.47949],[50.07769,14.4794],[50.07773,14.47915],[50.07776,14.47878],[50.07777,14.47867],[50.07784,14.47801],[50.0779,14.47736],[50.07791,14.47725],[50.07799,14.47643],[50.07804,14.47593],[50.07806,14.47582],[50.0781,14.47542],[50.07811,14.47516],[50.07811,14.47506],[50.07813,14.47487],[50.07821,14.47403],[50.07827,14.47355],[50.07829,14.47343],[50.0783,14.4733],[50.07832,14.47312],[50.07832,14.47295],[50.07831,14.4727],[50.07839,14.47268],[50.07842,14.47268],[50.07844,14.47268],[50.07853,14.47266],[50.07851,14.47224],[50.07851,14.47213],[50.07866,14.4721],[50.07878,14.47209],[50.0788,14.47208],[50.0788,14.47205],[50.0788,14.47201],[50.0788,14.47195],[50.07879,14.47172],[50.07878,14.47159],[50.07877,14.47149],[50.0788,14.47138],[50.07877,14.47097],[50.07876,14.47076],[50.07874,14.47036],[50.079,14.46989],[50.07942,14.46912],[50.0795,14.46895],[50.07951,14.46894],[50.07952,14.46893],[50.07954,14.46892],[50.07955,14.46891],[50.07957,14.46885],[50.07959,14.46881],[50.08021,14.46763],[50.08061,14.46691],[50.08068,14.46677],[50.08099,14.4662],[50.08161,14.46505],[50.08198,14.46437],[50.08201,14.4643],[50.08215,14.46406],[50.08212,14.46256],[50.08211,14.46238],[50.08212,14.46182],[50.08212,14.46163],[50.08212,14.46159],[50.08212,14.46148],[50.08211,14.46102],[50.08211,14.46093],[50.0821,14.4608],[50.08206,14.46027],[50.08203,14.45965],[50.08212,14.45963],[50.08215,14.45962],[50.08224,14.4596],[50.08228,14.45958],[50.08233,14.45956],[50.08238,14.45952],[50.0825,14.45955],[50.08252,14.45942],[50.08253,14.4594],[50.08254,14.45939],[50.08255,14.45938],[50.08257,14.45936],[50.08263,14.45929],[50.0827,14.45922],[50.08277,14.45914],[50.08278,14.45909],[50.08285,14.45911],[50.08286,14.45905],[50.08288,14.45897],[50.0829,14.45891],[50.08293,14.45883],[50.08318,14.45859],[50.08323,14.45853],[50.08326,14.4585],[50.08331,14.45847],[50.08338,14.45842],[50.08345,14.45836],[50.0835,14.4583],[50.08354,14.45826],[50.08358,14.45821],[50.08371,14.45805],[50.08377,14.45796],[50.0838,14.45791],[50.08387,14.45781],[50.08397,14.45764],[50.08404,14.45752],[50.08408,14.45745],[50.08411,14.4574],[50.08417,14.45727],[50.08423,14.45715],[50.0843,14.45698],[50.08447,14.45659],[50.08452,14.4565],[50.08458,14.45633],[50.08462,14.45624],[50.08469,14.45608]];

    // s

    // Map orientation based on current device orientation
    //deviceOrientation: DeviceOrientation

    // private addMapRoute() {
    //   /// Defining route on the map --------------------------------------
    //   this.routeStyle = new Style({
    //     stroke: new Stroke({
    //       color: 'rgba(0,0,255,1.0)',
    //       width: 3,
    //       lineCap: 'round'
    //     })
    //   });

    //   // Setting route geometry
    //   this.routeGeometry = new LineString([
    //     [14.43521, 50.1436],
    //     [14.40808, 50.13326],
    //     [14.42491, 50.11852],
    //     [14.4486, 50.1172]
    //   ]);

    //   // Route will be displayed as polygonal chain
    //   this.routeFeature = new Feature({
    //     geometry: this.routeGeometry
    //   });

    //   // Vector layer to render a route
    //   this.routeLayer = new VectorLayer({
    //     source: new VectorSource({
    //       features: [this.routeFeature]
    //     }),
    //     style: this.routeStyle
    //   });
    // }

    private setMapType() {
    //   this.baseLayer = leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    //     maxZoom: 19,
    //   });

      this._baseLayer = leaflet.tileLayer('https://cartodb-basemaps-{s}.global.ssl.fastly.net/light_all/{z}/{x}/{y}.png', {
        maxZoom: 19,
      });

    //   this.baseLayer = leaflet.tileLayer('https://cartodb-basemaps-{s}.global.ssl.fastly.net/dark_all/{z}/{x}/{y}.png', {
    //     maxZoom: 19,
    //   });

      this._baseLayer.addTo(this._map);
    }

    // private setDefaultViewLocation() {
    //   this.view = new View({
    //     center: [50.08804, 14.42076],
    //     zoom: 6
    //   });     
    // }

    private setMapZoomControl() {
      leaflet.control.zoom({
          position: 'bottomleft'
      }).addTo(this._map);
    }

    // private initializeMap() {
    //   this.map = new Map({
    //     target: 'map',
    //     controls: [
    //       this.zoomControl,
    //       this.attributionControl
    //     ],
    //     layers: [
    //       this.baseLayer,
    //       this.routeLayer
    //     ],
    //     view: this.view
    //   });
    // }

    // private setGeolocation() {
    //   this.geolocation = new Geolocation({
    //     tracking: true
    //   });

    //   // Binding view to the projection
    //   this.geolocation.setProjection(this.view.getProjection());

    //   // After getting new coordinate it will be appended to the route and center of the map
    //   // will be updated based on the of the new coordinate
    //   this.geolocation.on('change:position', () => {
    //     var newCoordinate = this.geolocation.getPosition();
        
    //     this.view.setCenter(newCoordinate);
    //     this.routeGeometry.appendCoordinate(newCoordinate);
    //   });
    // }

    // private setMarker() {
    //   this.markerOverlay = new Overlay({
    //     element: document.getElementById('location'),
    //     positioning: OverlayPositioning.CENTER_CENTER
    //   });

    //   // Add marker overlay to the map
    //   this.map.addOverlay(this.markerOverlay);

    //   // Set marker to the current position on the map
    //   this.markerOverlay.setPosition(this.geolocation.getPosition());
    // }

    // private setDeviceOrientationOnMap() {
    //   // this.deviceOrientation = new DeviceOrientation({
    //   //   tracking: true
    //   // });

    //   // // Changing the orientation where device is heading
    //   // this.deviceOrientation.on('change:heading', onChangeHeading);
    //   // function onChangeHeading(event) {
    //   //   var heading = event.target.getHeading();
    //   //   this.view.setRotation(-heading);
    //   // }
    // }

    private initializeMap() {
        this._map = leaflet.map(this.uniqueMapId, {
            zoomControl: false,
            center: [50.08804, 14.42076],
            zoom: 15
        })
    }


    constructor(
        private _elementRef: ElementRef
    ) {
        console.log("Map component instantiated.");
        this.uniqueMapId = 'map' + uuid.v4();
    }
    
    ngOnInit() {
        // this.componentResizeObserver = ResizeObserver(e => {
        //     console.log('Map element resized');
        // });

        // this.componentResizeObserver.observe(this._elementRef.nativeElement);
    }

    ngOnDestroy(): void {
        //this.componentResizeObserver.unobserve(this._elementRef.nativeElement);
    }

    ngAfterViewInit() {
        // Initialize map itself - based on type of the map, default view location and routes
        this.initializeMap();

        // Defining base type of map
        this.setMapType();

        this.setMapZoomControl();

        // // Defining the geolocation api to track our position
        // this.setGeolocation();

        // // Defining marker position on the map
        // this.setMarker();

        // // Defining map view to match current device orientation
        // //this.setDeviceOrientationOnMap();

        // Fix map sizing after loading the map component
        setTimeout(() => {
            this._map.invalidateSize();
        }, 200);

        // Fix map sizing after loading / resizing the map component
        // this.componentResizeObservable$ = fromEvent(this._elementRef.nativeElement, 'resize');
        // this.componentResizeSubscription$ = this.componentResizeObservable$.subscribe(() => {
        //     console.log('map element resized');
        //     setTimeout(() => {
        //         this._map.invalidateSize();
        //     }, 100);
        // });
    }

    // ngDoCheck() {
    //     // Fix map sizing after loading the map component
    //     console.log('ngDoCheck');
    //     setTimeout(() => {
    //         this._map.invalidateSize();
    //     }, 100);
    // }

    private addCoordinatesToPolyline(polyline: leaflet.Polyline, coordinates: ICoordinate[]) {
        var path = polyline.getLatLngs();
        coordinates.forEach(coord => {
            path.push(
                new leaflet.LatLng(
                    coord.lat,
                    coord.lng
                )
            );
        });
    }

    

    routeExists(id: string): boolean {
        return (this._mapData.routes[id]) ? true : false;
    }

    addRoute(
        id: string,
        coordinates: ICoordinate[],
        setMarkerStart: boolean,
        setMarkerEnd: boolean,
        markerNameStart?: string,
        markerNameEnd?: string,
        color?: string
    ) {
        
        this._mapData.routes[id] = {
            polyline: leaflet.polyline(
                [[],[]],
                { color: (color) ? color : randomHexColor()}
            ),
            markerStart: null,
            markerEnd: null
        }

        let route = this._mapData.routes[id];

        this.addCoordinatesToPolyline(route.polyline, coordinates);

        // Setting route to the map
        route.polyline.addTo(this._map);

        if (setMarkerStart) {
            let firstCoordinate = coordinates[0];
            route.markerStart = leaflet.marker(
                new leaflet.LatLng(
                    firstCoordinate.lat,
                    firstCoordinate.lng
                ),
                { icon: this._mapData.markerIcon }
            );

            // Adding last route marker to the map view
            route.markerStart.addTo(this._mapData.markers);
        }
        
        if (setMarkerEnd) {
            let lastCoordinate = coordinates[coordinates.length - 1];
            route.markerEnd = leaflet.marker(
                new leaflet.LatLng(
                    lastCoordinate.lat,
                    lastCoordinate.lng
                ),
                { icon: this._mapData.markerIcon }
            );

            // Adding last route marker to the map view
            route.markerEnd.addTo(this._mapData.markers);
        }

        // Fits all routes to the map view
        this._map.fitBounds(this._mapData.markers.getBounds().pad(0.5))
    }

    updateRoute(id: string, coordinates: ICoordinate[], color?: string) {
        // If there already exists route we will update it;s coordinates
        if (this._mapData.routes[id]) {
            var route = this._mapData.routes[id];

            if (color && checkHexColor(color)) {
                route.polyline.setOptions({strokeColor: color})
            }
            
            // Adding new coordinates to the route
            this.addCoordinatesToPolyline(route.polyline, coordinates);

            // Moving marker to the last added coordinate to indicate actual position of a device
            var lastCoordinate = coordinates[coordinates.length - 1];
            route.markerEnd.setLatLng(
                new leaflet.LatLng(
                    lastCoordinate.lat,
                    lastCoordinate.lng
                )
            );

            // Adding last route marker to the map view
            route.markerEnd.addTo(this._mapData.markers);
            
            // Fits all routes to the map view
            this._map.fitBounds(this._mapData.markers.getBounds().pad(0.5))
        }
    }

    removeRoute(id: string) {
        if (this._mapData.routes[id]) {
            var route = this._mapData.routes[id];
            if (route.polyline) {
                route.polyline.addMap(null);
                route.polyline = null;    
            }
            
            if (route.markerStart) {
                route.markerStart.setMap(null);
                route.markerStart = null;
            }

            if (route.markerEnd) {
                route.markerEnd.setMap(null);
                route.markerEnd = null;
            }
            
            this._mapData.routes[id] = null;

            delete this._mapData.routes[id];

            // Refreshing bounds after removing the route from the map
            this._mapData.markers = leaflet.featureGroup();
            for (let _id in this._mapData.routes) {
                let _route = this._mapData.routes[_id];

                _route.markerStart.addTo(this._mapData.markers);
                _route.markerEnd.addTo(this._mapData.markers);
            }

            // Adding markers to the map view
            this._mapData.markers.addTo(this._map);

            // Fits all routes which left to the map view
            this._map.fitBounds(this._mapData.markers.getBounds().pad(0.5))
        }
    }
}
