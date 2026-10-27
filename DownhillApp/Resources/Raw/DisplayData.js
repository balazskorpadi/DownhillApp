let map;
let pendingRoute = null;
let routeLine = null;
let startMarker = null;
let endMarker = null;

function InitMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: {lat: 47.4979, lng: 19.0402},
        zoom: 14,
        mapTypeControl: true,
        streetViewControl: false
    });

    if (pendingRoute) {
        DrawRoute(pendingRoute);
    };
}

function DrawRoute(coordinates) {
    coordinates = coordinates.map(x => ({
        lat: x[0],
        lng: x[1]
        }));

    if (routeLine) routeLine.setMap(null);
    if (startMarker) startMarker.setMap(null);
    if (endMarker) endMarker.setMap(null);
    map.setCenter(coordinates[0]);
    map.setZoom(14);

    routeLine = new google.maps.Polyline({
        path: coordinates,
        geodesic: true,
        strokeColor: "#FFA500",
        strokeOpacity: 0.9,
        strokeWeight: 5,
        map: map
    });

    startMarker = new google.maps.Marker({
        position: coordinates[0],
        map: map,
        title: "Startpoint",
        label: { text: "A", color: "#FFFFFF" },
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            scale: 10,
            fillColor: "#01796F",
            fillOpacity: 1,
            strokeColor: "#FFFFFF",
            strokeWeight: 2,
        }
    });

    endMarker = new google.maps.Marker({
        position: coordinates[coordinates.length - 1],
        map: map,
        title: "Endpoint",
        label: { text: "B", color: "#FFFFFF" },
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            scale: 10,
            fillColor: "#FF0000",
            fillOpacity: 1,
            strokeColor: "#FFFFFF",
            strokeWeight: 2,
        }
    });

}