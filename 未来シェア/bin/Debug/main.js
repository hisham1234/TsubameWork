var map;
var polylinePoints_editwork;
var polygonPoints_editwork;
var polyline_editwork;
var polygon_list;
var polyline_list;
var maker_list;
var circle_list;
var icon_marker_list;
var infowindow_label_list;
//
//
//�������֐�
function init() {

    //
    //*****************************
    //�z��̍쐬
    //*****************************
    polylinePoints_editwork = new google.maps.MVCArray();
    polygonPoints_editwork = new google.maps.MVCArray();
    polygon_list = new google.maps.MVCArray();
    polyline_list = new google.maps.MVCArray();
    marker_list = new google.maps.MVCArray();
    circle_list = new google.maps.MVCArray();
    icon_marker_list = new google.maps.MVCArray();
    infowindow_label_list = new google.maps.MVCArray();
    //var latlng  �@    = ;
    //*****************************
    //�n�}�I�v�V����
    //*****************************
    var myOptions = {
        zoom: 15,
        center: new google.maps.LatLng(35.168896, 136.906906),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        overviewMapControl: true,
        scaleControl: true,
        disableDoubleClickZoom: true
    };
    //*****************************
    //�n�}�̍쐬
    //*****************************
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    //*****************************
    //�V�K�\���p�|�����C��
    //*****************************
    polyline_editwork = new google.maps.Polyline({
        path: polylinePoints_editwork,
        fillColor: '#00ff00',   // �h��Ԃ��F
        fillOpacity: 0.25,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
        map: map,             // �\��������n�}�igoogle.maps.Map�j
        strokeColor: '#00ff00', // �O���F
        strokeOpacity: 1,       // �O�����ߓx�i0: ���� �� 1:�s�����j
        strokeWeight: 5,        // �O�������i�s�N�Z���j
        clickable: false
    });
    //*****************************
    //�V�K�\���p�|���S��
    //*****************************
    polygon_editwork = new google.maps.Polygon({
        path: polygonPoints_editwork,
        fillColor: '#00ff00',   // �h��Ԃ��F
        fillOpacity: 0.25,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
        map: map,             // �\��������n�}�igoogle.maps.Map�j
        strokeColor: '#00ff00', // �O���F
        strokeOpacity: 1,       // �O�����ߓx�i0: ���� �� 1:�s�����j
        strokeWeight: 5,        // �O�������i�s�N�Z���j
        clickable: true
    });
    //*****************************
    //�C�x���g�n���h��
    //*****************************


    google.maps.event.addListener(map, 'click', function (event) {
        Event_MouseClick('left', event.latLng);
    });
    google.maps.event.addListener(map, 'rightclick', function (event) {
        Event_MouseClick('right', event.latLng);
    });
    google.maps.event.addListener(map, 'mousemove', function (event) {
        Event_Mousemove(event.latLng);
    });
    google.maps.event.addListener(map, "idle", function () {
        Event_Idle();
    });





    //            google.maps.event.addListener( map, 'keydown', function() {
    //            alert("aaaaa");
    //                //Event_KeyDown(event.keyCode);
    //            });
    //setCenter (35.168896, 136.906906,100.0  , '#00ff00')
    //setRadius (35.168896, 136.906906,1000.0 , '#00ff00')
    //alert("aaaaa");
}
//*****************************
//�C�x���g�R�[���o�b�N
//*****************************
function RaiseEvent(ev, data) {
    var event = new MessageEvent(ev, { 'view': window, 'bubbles': false, 'cancelable': false, 'data': data });
    document.dispatchEvent(event);
}
function Event_ObjectClick(button, id, index, location) {
    var data = button + '\t' + id + '\t' + index + '\t' + location.lat() + '\t' + location.lng();
    RaiseEvent('GoogleMap_ObjectClick', data)
    //           window.external.Googlemap_ObjectClick(button,id,index,location.lat(), location.lng());
}
function Event_MouseClick(button, location) {
    var data = button + '\t' + location.lat() + '\t' + location.lng();
    RaiseEvent('GoogleMap_MouseClick', data)
    //window.external.Googlemap_MouseClick(button,location.lat(), location.lng());
}
function Event_Mousemove(location) {
    var data = location.lat() + '\t' + location.lng();
    RaiseEvent('GoogleMap_MouseMove', data)
    //window.external.Googlemap_MouseMove(location.lat(), location.lng());
}
function Event_Idle() {
    //alert("aaaaa");
    RaiseEvent('GoogleMap_Idle', "")
    //window.external.Googlemap_Idle();
}
function Event_KeyDown(key) {
    alert("aaaaa");
    window.external.Googlemap_KeyDown(key);
}
//***********************************************************
//****                     �|���S��
//***********************************************************
//***********************
//���|���S�������X�g�ɒǉ�
//***********************
function addPolygon(INobjectID, INLatLonsStr, INcolor, INname) {

    var objID = parseInt(INobjectID);
    LatLonsStr = String(INLatLonsStr);
    color = String(INcolor);
    name = String(INname);


    //        alert(objectID);
    //        alert(LatLonsStr);
    //        alert(color);
    //        alert(name);


    var points = new google.maps.MVCArray();
    arr = LatLonsStr.split(",");
    for (var i = 0; i < arr.length; i += 2) {
        if (arr[i] != 0 && arr[i + 1] != 0) {
            points.push(new google.maps.LatLng(arr[i], arr[i + 1]));
        }
    }
    polygon = new google.maps.Polygon({
        path: points,
        fillColor: color,   // �h��Ԃ��F
        fillOpacity: 0.10,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
        map: map,             // �\��������n�}�igoogle.maps.Map�j
        strokeColor: color, // �O���F
        strokeOpacity: 1,       // �O�����ߓx�i0: ���� �� 1:�s�����j
        strokeWeight: 5,        // �O�������i�s�N�Z���j
        clickable: true, //false ,
        objectID: objID
    });
    var marker = new google.maps.Marker({
        position: points.getAt(0),
        map: map,
        title: 'objectID=' + objID + '\n����=' + name
    });
    //alert(polygonPoints.getAt(0));
    var index = polygon_list.push(polygon);
    google.maps.event.addListener(polygon, 'click', function (event) {
        Event_ObjectClick('right', objID, index, event.latLng);
    });

    marker_list.push(marker);

}
function addPolyline(INobjectID, INLatLonsStr, INcolor, InLineType) {

    var linetype = parseInt(InLineType);
    var objID = parseInt(INobjectID);
    LatLonsStr = String(INLatLonsStr);
    color = String(INcolor);
    //name = String(INname);

    //alert(INobjectID);                    

    var points = new google.maps.MVCArray();
    arr = LatLonsStr.split(",");
    for (var i = 0; i < arr.length; i += 2) {
        if (arr[i] != 0 && arr[i + 1] != 0) {
            points.push(new google.maps.LatLng(arr[i], arr[i + 1]));
        }
    }

    var polyline;

    if (linetype == 0) {
        //����
        polyline = new google.maps.Polyline({
            path: points,
            //fillColor:  color,   // �h��Ԃ��F
            //fillOpacity: 0.10,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
            map: map,             // �\��������n�}�igoogle.maps.Map�j
            strokeColor: color, // �O���F
            strokeOpacity: 1,       // �O�����ߓx�i0: ���� �� 1:�s�����j
            strokeWeight: 5,        // �O�������i�s�N�Z���j
            clickable: false,
            objectID: objID
        });
    } else if (linetype == 1) {
        //�j��
        var lineSymbol = {
            path: 'M 0,-1 0,1',
            strokeOpacity: 1,
            strokeWeight: 5,
            scale: 4
        };

        polyline = new google.maps.Polyline({
            path: points,
            //fillColor:  color,   // �h��Ԃ��F
            //fillOpacity: 0.10,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
            map: map,             // �\��������n�}�igoogle.maps.Map�j
            strokeColor: color, // �O���F
            strokeOpacity: 0,
            icons: [{
                icon: lineSymbol,
                offset: '0',
                repeat: '20px'
            }],
            strokeWeight: 5,        // �O�������i�s�N�Z���j
            clickable: false,
            objectID: objID
        });
    }

    /*
                var marker = new google.maps.Marker({
                    position: points.getAt(0),
                    map: map,
                    title: 'objectID=' + objID + '\n����=' + name
                });
                */
    //alert(polylinePoints.getAt(0));
    var index = polyline_list.push(polyline);
    google.maps.event.addListener(polyline, 'click', function (event) {
        Event_ObjectClick('right', objID, index, event.latLng);
    });

    marker_list.push(marker);

}
function addCircle(objectID, rud, Lat, Lng, Color) {
    circle = new google.maps.Circle({
        center: new google.maps.LatLng(Lat, Lng),       // ���S�_(google.maps.LatLng)
        fillColor: Color,   // �h��Ԃ��F
        fillOpacity: 1,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
        map: map,             // �\��������n�}�igoogle.maps.Map�j
        radius: rud,          // ���a�i���j
        strokeColor: Color, // �O���F
        strokeOpacity: 1,       // �O�����ߓx�i0: ���� �� 1:�s�����j
        strokeWeight: 5,        // �O�������i�s�N�Z���j
        objectID: objectID,
        clickable: false
    });
    circle_list.push(circle);
}

function addIconMarker(Lat, Lng, icon) {
    var marker = new google.maps.Marker({
        position: new google.maps.LatLng(Lat, Lng),
        map: map,
        icon: icon
    });
    icon_marker_list.push(marker);
}

function addInfoWindowLabel(IN_icon, IN_Lat, IN_Lng, IN_text, IN_cssText, IN_offsetX, IN_offsetY) {

    icon = String(IN_icon);
    Lat = parseFloat(IN_Lat);
    Lng = parseFloat(IN_Lng);
    text = String(IN_text);
    cssText = String(IN_cssText);
    offsetX = parseInt(IN_offsetX);
    offsetY = parseInt(IN_offsetY);



    if (icon != undefined) {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(Lat, Lng),
            map: map,
            icon: icon
        });
        infowindow_label_list.push(marker);
    }

    var labelText = document.createElement("div");
    labelText.style.cssText = cssText
    labelText.innerHTML = text//"City Hall, Sechelt<br>British Columbia<br>Canada";

    var myOptions = {
        content: labelText
        //		        ,boxStyle: {
        //		            border: "4px solid blue"
        //		            ,background: "white"
        //		            ,textAlign: "center"
        //		            fontSize: "10pt"
        //		            ,width: "50px"
        //		        }
        , disableAutoPan: true
        , pixelOffset: new google.maps.Size(offsetX, offsetY)
        , position: new google.maps.LatLng(Lat, Lng)
        , closeBoxURL: ""
        , isHidden: false
        , pane: "floatPane"
        , enableEventPropagation: true
        , draggable: true
    };
    var ibLabel = new InfoBox(myOptions);
    ibLabel.open(map);
    infowindow_label_list.push(ibLabel);
}

function removeInfoWindowLabel() {
    infowindow_label_list.forEach(function (InfoBox, idx) {
        InfoBox.setMap(null);
    });
}

////            var marker = new google.maps.Marker({
////                map: map,
////                draggable: true,
////	            position: new google.maps.LatLng(35.17115, 136.884343),
////	            visible: true
////	        });
////            var boxText = document.createElement("div");
////            boxText.style.cssText = "border: 1px solid black; margin-top: 8px; background: yellow; padding: 5px;";
////            boxText.innerHTML = "City Hall, Sechelt<br>British Columbia<br>Canada";

////            var myOptions = {
////		        content: boxText
////		        ,setPosition: new google.maps.LatLng(35.17115, 136.884343)
////		        ,disableAutoPan: false
////		        ,maxWidth: 0
////		        ,pixelOffset: new google.maps.Size(-140, 0)
////		        ,zIndex: null
////		        ,boxStyle: { 
////		            background: "url('tipbox.gif') no-repeat"
////		            ,opacity: 0.75
////		            ,width: "280px"
////		        }
////		        ,closeBoxMargin: "10px 2px 2px 2px"
////		        ,closeBoxURL: null//"http://www.google.com/intl/en_us/mapfiles/close.gif"
////		        ,infoBoxClearance: new google.maps.Size(1, 1)
////		        ,isHidden: false
////		        ,pane: "floatPane"
////		        ,enableEventPropagation: false
////	        };
////            
////            var ib = new InfoBox(myOptions);
////	        ib.open(map,this);


//          var marker = new google.maps.Marker({
//                position: new google.maps.LatLng( Lat, Lng )  ,      
//                map: map ,
//                icon: 'images/beachflag.png'
//            });
//          icon_marker_list.push(marker);

//         var image=new google.maps.MarkerImage(icon,
//            new google.maps.Size(100.0, 200.0),
//            new google.maps.Point(0, 0),
//            new google.maps.Point(22.0, 100.0)
//         );
//       icon: 'images/beachflag.png'
//            var contentStr = '123';


function removeIconMarker() {
    icon_marker_list.forEach(function (marker, idx) {
        marker.setMap(null);
    });
}

function addCircle2(objectID, rud, Lat, Lng, Color, str) {
    //            circle = new google.maps.Circle({
    //                center: new google.maps.LatLng( Lat, Lng )  ,       // ���S�_(google.maps.LatLng)
    //                fillColor:  Color,   // �h��Ԃ��F
    //                fillOpacity: 1,       // �h��Ԃ����ߓx�i0: ���� �� 1:�s�����j
    //                map: map,             // �\��������n�}�igoogle.maps.Map�j
    //                radius: rud,          // ���a�i���j
    //                strokeColor:  Color, // �O���F
    //                strokeOpacity: 1,       // �O�����ߓx�i0: ���� �� 1:�s�����j
    //                strokeWeight: 5 ,        // �O�������i�s�N�Z���j
    //                objectID: objectID , 
    //                
    //                clickable : false 
    //            });
    //            circle_list.push(circle);


    var marker = new google.maps.Marker({
        position: new google.maps.LatLng(Lat, Lng),
        map: map,
        label: {
            color: '#550000',	// ���x���̐F
            text: 'A' 		// ����
        },
        title: str
    });


    marker_list.push(marker);

}
function removeCircle() {
    circle_list.forEach(function (circle, idx) {
        circle.setMap(null);
    });
}
//***********************
//��index�̃|���S����ύX
//***********************
function changePolygonIndex(index, element, value) {
    polygon_list.getAt(index).set(strokeColor, value);
    //alert(obj.fillColor);
    //obj.fillColor=color;
    //        obj.fillColor='#000000';
    //     polygon_list.setAt(index,obj);
}
//***********************
//��ObjectID�̃|���S����ύX
//***********************
function changePolygonObjectID(objectID, element, value) {
    polygon_list.forEach(function (obj, i) {
        if (obj.objectID == objectID) {
            obj.set(element, value);
            return;
        }
    });
    //getAt(index).set(strokeColor, value);
    //alert(obj.fillColor);
    //obj.fillColor=color;
    //        obj.fillColor='#000000';
    //     polygon_list.setAt(index,obj);
}
//***********************
//���ҏW��Ɨp�|���S���̃|�C���g���N���A����
//***********************
function clearPolygon_editwork() {
    polygonPoints_editwork.clear();
}
//***********************
//���ҏW��Ɨp�|���S���ɍ��W���Đݒ肷��
//***********************
function drawPolygon_editwork(objectID, LatLonsStr, color) {
    polygon_editwork.set("fillColor", color);
    polygonPoints_editwork.clear();
    var arr = LatLonsStr.split(",");
    for (var i = 0; i < arr.length; i += 2) {
        if (arr[i] != 0 && arr[i + 1] != 0) {
            polygonPoints_editwork.push(new google.maps.LatLng(arr[i], arr[i + 1]));
        }
    }
    //alert(polylinePoints.length);
}
//***********************
//���ҏW��Ɨp�|���S�����X�g���N���A����
//***********************
function clearPolygon_editwork() {
    polygonPoints_editwork.clear();
}
//***********************
//���S�Ă̊m��|���S����n�}������������X�g���N���A����
//***********************
function removePolygonListALL() {
    polygon_list.forEach(function (polygon, idx) {
        polygon.setMap(null);
    });
    marker_list.forEach(function (marker, idx) {
        marker.setMap(null);
    });
    polygon_list.clear();
    marker_list.clear();
}
function removePolyline() {
    polyline_list.forEach(function (polyline, idx) {
        polyline.setMap(null);
    });
    polyline_list.clear();
}
//***********************
//���S�Ă̊m��|���S����n�}������������X�g���N���A����
//***********************
function removePolygonIndex(index) {
    polygon_list.forEach(function (polygon, idx) {
        polygon.setMap(null);
    });
    polygon_list.clear();
}
//***********************************************************
//****                     �|�����C��
//***********************************************************
//***********************
//���ҏW��Ɨp�|�����C���ɍ��W���Đݒ肷��
//***********************
function drawPolyline_editwork(objectID, LatLonsStr, color) {
    polyline_editwork.set("fillColor", color);
    polyline_editwork.set("strokeColor", color);
    polylinePoints_editwork.clear();
    //alert(color);
    var arr = LatLonsStr.split(",");
    for (var i = 0; i < arr.length; i += 2) {
        if (arr[i] != 0 && arr[i + 1] != 0) {
            polylinePoints_editwork.push(new google.maps.LatLng(arr[i], arr[i + 1]));
        }
    }
    //alert(polylinePoints.length);
    //alert(polyline_editwork.fillColor);
}
//***********************
//���ҏW��Ɨp�|�����C�����N���A����
//***********************
function clearPolyline_editwork() {
    polylinePoints_editwork.clear();
}
//***********************************************************
//****                     ���̑�
//***********************************************************
function toCenter(Lat, Lng) {
    map.panTo(new google.maps.LatLng(Lat, Lng));
}
function zoomIn() {
    var level = map.getZoom();
    level++;
    map.setZoom(level);
}
function zoomOut() {
    var level = map.getZoom();
    if (level != 0) {
        level--;
    }
    map.setZoom(level);
}