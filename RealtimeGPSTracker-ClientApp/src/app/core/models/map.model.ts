import * as leaflet from 'leaflet';

export interface IMapData {
    routes: { [id: string]: IMapRoute };
    markers: leaflet.FeatureGroup;
    markerIcon: leaflet.Icon;
}

export interface IMapRoute {
    polyline: leaflet.Polyline;
    markerStart: leaflet.Marker;
    markerEnd: leaflet.Marker;
}