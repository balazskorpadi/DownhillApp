/*let map;
let pendingRoute = null;
let routeLine = null;
let startMarker = null;
let endMarker = null;

function InitMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: {lat: 47.4979, lng: 19.0402},
        zoom: 50,
        mapTypeId: google.maps.MapTypeId.TERRAIN,
        tilt: 45,
        heading:0,
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

}*/


let map;
let pendingRoute = null;
let routeLine = null;
let startMarker = null;
let endMarker = null;


// =====================================
// TÉRKÉP INICIALIZÁLÁSA
// =====================================

async function InitMap() {

    const {
        Map3DElement,
        Marker3DElement,
        Polyline3DElement
    } = await google.maps.importLibrary("maps3d");

    // Osztályok eltárolása a késõbbi használathoz
    window.Map3DElement = Map3DElement;
    window.Marker3DElement = Marker3DElement;
    window.Polyline3DElement = Polyline3DElement;

    // 3D térkép létrehozása
    map = new Map3DElement({
        center: {
            lat: 47.4979,
            lng: 19.0402,
            altitude: 0
        },

        range: 2000,

        tilt: 65,

        heading: 0,

        mode: "HYBRID"
    });

    // Térkép hozzáadása a HTML-hez
    document.getElementById("map").appendChild(map);

    // Korábban megadott útvonal kirajzolása
    if (pendingRoute) {
        DrawRoute(pendingRoute);
    }
}


// =====================================
// ÚTVONAL KIRAJZOLÁSA
// =====================================

function DrawRoute(coordinates) {

    if (!map) {
        pendingRoute = coordinates;
        return;
    }

    // Koordináták átalakítása
    coordinates = coordinates.map(x => ({
        lat: x[0],
        lng: x[1]
    }));

    if (coordinates.length < 2) return;

    // Korábbi elemek törlése
    if (routeLine) {
        routeLine.remove();
    }

    if (startMarker) {
        startMarker.remove();
    }

    if (endMarker) {
        endMarker.remove();
    }

    // Kamera középpontjának beállítása
    map.center = {
        lat: coordinates[0].lat,
        lng: coordinates[0].lng,
        altitude: 0
    };

    map.range = 1500;
    map.tilt = 65;

    // =================================
    // 3D ÚTVONAL
    // =================================

    routeLine = new window.Polyline3DElement({

        path: coordinates,

        altitudeMode: "CLAMP_TO_GROUND",

        strokeColor: "#FFA500",

        strokeWidth: 8,

        outerColor: "#FFFFFF",

        outerWidth: 0.15,

        geodesic: true

    });

    map.appendChild(routeLine);


    // =================================
    // START MARKER
    // =================================

    startMarker = new window.Marker3DElement({

        position: {
            lat: coordinates[0].lat,
            lng: coordinates[0].lng
        },

        label: "A",

        altitudeMode: "CLAMP_TO_GROUND",

        drawsWhenOccluded: true

    });

    map.appendChild(startMarker);


    // =================================
    // END MARKER
    // =================================

    const last = coordinates[coordinates.length - 1];

    endMarker = new window.Marker3DElement({

        position: {
            lat: last.lat,
            lng: last.lng
        },

        label: "B",

        altitudeMode: "CLAMP_TO_GROUND",

        drawsWhenOccluded: true

    });

    map.appendChild(endMarker);

}
