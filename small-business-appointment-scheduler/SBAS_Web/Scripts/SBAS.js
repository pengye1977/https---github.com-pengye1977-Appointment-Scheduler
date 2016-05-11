// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-25-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-25-2014
// ***********************************************************************
// <copyright file="MapInfoModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This script file is the main javascript file that is used mostly by the appointment scheudler.
//</summary>
// ***********************************************************************

// Thhis function is used to show custom dialog for infortmation displaying
function LoadPersonDialog(urlroute, heightSize, widthSize) {
    $.ajax({
        type: "GET",
        url: urlroute, //"/Account/RegisterClient",
        cache: false,
        success: function (result) {
            $("#CustomDialog").html(result);
            var accessWindow = $("#CustomDialog").kendoWindow({
                actions: ["Close"],
                draggable: true,
                height: '' + heightSize + '' + "px", //"550px",
                modal: true,
                resizable: false,
                width: '' + widthSize + '' + "px", //"1117px",
                visible: true, /*don't show it yet*/
            }).data("kendoWindow").center().title(" Add Client ").open();

        },
        error: function (result) {

        }
    });
}
// This fucntion loads the dailog needed for marking itmes used by an appointment.
function LoadAppointmentCompletedDialog(urlroute, heightSize, widthSize, id, appointmentCompletedId) {
    $.ajax({
        type: "GET",
        url: urlroute, //"/Account/RegisterClient",
        cache: false,
        data: { id: id, appointmentCompletedId: appointmentCompletedId },
        success: function (result) {
            $("#CustomDialog").html(result);
            var accessWindow = $("#CustomDialog").kendoWindow({
                actions: ["Close"],
                draggable: true,
                height: '' + heightSize + '' + "px", //"550px",
                modal: true,
                resizable: false,
                width: '' + widthSize + '' + "px", //"1117px",
                visible: true, /*don't show it yet*/
            }).data("kendoWindow").center().title(" Edit Completed Appointment Used Inventory ").open();

        },
        error: function (result) {
            //alert("error" + result);
        }
    });
}
// This function is sued to see if an appointment is already completed.
function AppointmentAlreadyCompelted(atitle, contenttoopen) {
    var kendoWindow = $("<div />").kendoWindow({
        title: atitle,
        resizable: false,
        modal: true
    });

    kendoWindow.data("kendoWindow")
        .content($("#" + contenttoopen).html())
        .center().open();

    kendoWindow
        .find(".delete-confirm,.delete-cancel")
        .click(function () {
            kendoWindow.data("kendoWindow").close();
        })
        .end();
}

function item_selected(e) {

    var grid = $("#InventoryItems").data("kendoGrid");
    var price;

    grid.select().each(function () {
        var dataItem = grid.dataItem($(this));
        price = dataItem.ItemPrice;
    });

    if ($("#IsReadyForInvoice").val() == "False") {
        $("#AddSelectionArea").show();
    }
    var pr = $("#itemPrice").data("kendoNumericTextBox");
    pr.value(price);
    var qty = $("#UsedQuanity").data("kendoNumericTextBox");
    qty.value(0);

};
// This javascript is called when marking an apointment completed
function MarkAppointmentComplete(dataItem, uid) {
    $.ajax({
        type: "GET",
        url: "/Appointment/AppointmentCompletedFlags",
        data: { appointmentId: dataItem.AppointmentId, start: kendo.toString(dataItem.start, 'G') },
        cache: false,
        success: function (outterresult) {

            if (outterresult.IsCompleted == false) {
                $.ajax({
                    type: "GET",
                    url: "/Appointment/CreateAppointmentCompleted",
                    data: { id: dataItem.AppointmentId, startDate: kendo.toString(dataItem.start, "G"), endDate: kendo.toString(dataItem.end, "G"), clientId: dataItem.ClientId, customerId: dataItem.CustomerId },
                    cache: false,
                    success: function (innerresult) {
                        dataItem.AppointmentCompletedId = innerresult.Id;
                        dataItem.AppointmentCompleted = innerresult.IsCompleted;
                        dataItem.AppointmentState = "Completed";
                        $('div.k-event[data-uid="' + uid + '"] div.appointment-template').removeClass('DefaultColor').addClass('CompletedColor');
                        $('#AppointmentCompleted').bPopup();

                    },
                    error: function (innerresult) {
                    }
                });
            } else {
                $('#AppointmentAllreadyCompleted').bPopup();
            }
        },
        error: function (outterresult) {
        }
    });

    return dataItem;
};
// THis function is called when marking an appointment ready for invoicing
function MarkAppointmentReadyToInvoice(dataItem, uid) {
    $.ajax({
        type: "GET",
        url: "/Appointment/AppointmentCompletedFlags",
        data: { appointmentId: dataItem.AppointmentId, start: kendo.toString(dataItem.start, 'G') },
        cache: false,
        //sync: true,
        success: function (outterresult) {
            if (outterresult.IsCompleted == true) {
                if (outterresult.IsReadyForInvoicing == false) {
                    $.ajax({
                        type: "GET",
                        url: "/Appointment/MarkAppointmentCompletedReadyForInvoicing",
                        data: { appointmentCompletedId: dataItem.AppointmentCompletedId },
                        cache: false,
                        success: function (innerresult) {
                            if (innerresult.IsReadyForInvoicing == true) {
                                $('#MarkedForInvoicingSuccessful').bPopup();
                                dataItem.AppointmentState = "ReadyForInvoicing";
                                $('div.k-event[data-uid="' + uid + '"] div.appointment-template').removeClass('CompletedColor').addClass('ReadyForInvoicingColor');
                            } else {
                                $('#MarkedForInvoicingUnSuccessful').bPopup();
                            }
                        },
                        error: function (innerresult) {
                            //alert(innerresult);
                        }
                    });
                } else {
                    $('#AppointmentIsSetReadyForInvoicing').bPopup();
                }

            } else {
                $('#AppointmentNotCompleted').bPopup();
            }
        }
        ,
        error: function (outterresult) {
            //alert(outterresult);
        }
    });

    return dataItem;
};
//This function is called when shoinw client information about of a given appointment
function ShowMap(dataItem, uid) {
    $('#map_canvas').html('<div style="top:20px;" class="spinner">&nbsp;</div>');
    $.ajax({
        type: "GET",
        url: "/Client/GetUserInfo",
        data: { userId: dataItem.ClientId },
        cache: false,
        success: function (result) {
            $("#map_canvas").html('');
            $('#MapCanvas').bPopup();
            $("#clientmapfullname").text(result.FullName);
            $("#clientAddressLine1").text(result.AddressLine1);
            $("#clientAddressLine2").text(result.AddressLine2);
            $("#clientPhoneNumber").text(result.PhoneNumber);
            $("#clientMobileNumber").text(result.MobileNumber);
            $("#clientFaxNumber").text(result.FaxNumber);



            var mapOptions = {
                center: new google.maps.LatLng(result.Latitude, result.Longitude),
                zoom: 21,
                mapTypeId: google.maps.MapTypeId.SATELLITE
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            var infoWindow = new google.maps.InfoWindow();
            var lat_lng = new Array();
            var latlngbounds = new google.maps.LatLngBounds();
            var data = result;
            var myLatlng = new google.maps.LatLng(data.Latitude, data.Longitude);
            lat_lng.push(myLatlng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.FullName
            });
            latlngbounds.extend(marker.position);
            (function (marker, data) {
                //google.maps.event.addListener(marker, "click", function (e) {
                //    infoWindow.setContent("Nothing for now"); //data.description
                //    infoWindow.open(map, marker);
                //});
            })(marker, data);



            map.setCenter(latlngbounds.getCenter());
            map.fitBounds(latlngbounds);
        },
        error: function (result) {
            //alert(result);
        }
    });

}
// This function is used to build the map the a given days route.
function ShowTodaysRouteMap(customerid,datetime) {
    $('#routeMapCanvas').html('<div style="top:20px;" class="spinner">&nbsp;</div>');
    $.ajax({
        type: "GET",
        url: "/Appointment/GetTodaysAppointents",
        data: { customerId: customerid, dateTime: datetime },
        cache: false,
        success: function (markers) {
            if (markers.length == 0) {
                $('#routeMapCanvas').html('<div style="text-align:center;"><B>No Appointments Scheduled</B></div>');
            } else {

                var mapOptions = {
                    center: new google.maps.LatLng(markers[0].Latitude, markers[0].Longitude),
                    zoom: 3,
                    mapTypeId: google.maps.MapTypeId.SATELLITE
                };
                var map = new google.maps.Map(document.getElementById("routeMapCanvas"), mapOptions);
                var infoWindow = new google.maps.InfoWindow();
                var lat_lng = new Array();
                var latlngbounds = new google.maps.LatLngBounds();
                for (i = 0; i < markers.length; i++) {
                    var data = markers[i]
                    var myLatlng = new google.maps.LatLng(data.Latitude, data.Longitude);
                    lat_lng.push(myLatlng);
                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: data.FullName
                    });
                    latlngbounds.extend(marker.position);
                    (function (marker, data) {
                        //google.maps.event.addListener(marker, "click", function (e) {
                        //    infoWindow.setContent("Nothing for now"); //data.description
                        //    infoWindow.open(map, marker);
                        //});
                    })(marker, data);
                }
                map.setCenter(latlngbounds.getCenter());
                map.fitBounds(latlngbounds);

                //***********ROUTING****************//

                //Intialize the Path Array
                var path = new google.maps.MVCArray();

                //Intialize the Direction Service
                var service = new google.maps.DirectionsService();

                //Set the Path Stroke Color
                var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7' });

                //Loop and Draw Path Route between the Points on MAP
                for (var i = 0; i < lat_lng.length; i++) {
                    if ((i + 1) < lat_lng.length) {
                        var src = lat_lng[i];
                        var des = lat_lng[i + 1];
                        path.push(src);
                        poly.setPath(path);
                        service.route({
                            origin: src,
                            destination: des,
                            travelMode: google.maps.DirectionsTravelMode.DRIVING
                        }, function (result, status) {
                            if (status == google.maps.DirectionsStatus.OK) {
                                for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                                    path.push(result.routes[0].overview_path[i]);
                                }
                            }
                        });
                    }
                }
            }

        },
        error: function (markers) {
            //
        }
    });

}
// This fucntion is called when marking an appointment compelted
function SetupAppointmentCompletion() {
    if ($("#IsReadyForInvoice").val() == "False") {
        $("#btnUseInvetory").click(function () {

            var itemprice;
            var quantityUsed;
            var selectedRowQuantity;
            var inventoryId;
            var inventoryItemId;
            var appointmentCompletionId;
            var dataItem;
            var grid = $("#InventoryItems").data("kendoGrid");
            grid.select().each(function () {
                dataItem = grid.dataItem($(this));
                selectedRowQuantity = dataItem.QuantityOnHand;
                inventoryId = dataItem.InventoryId;
                inventoryItemId = dataItem.InventoryItemId;
            });

            var itemPriceInput = $("#itemPrice").data("kendoNumericTextBox");
            itemprice = itemPriceInput.value();

            var usedQuanityInput = $("#UsedQuanity").data("kendoNumericTextBox");
            quantityUsed = usedQuanityInput.value();

            if (selectedRowQuantity >= quantityUsed) {

                appointmentCompletionId = $("#AppointmentCompletedId").val();

                $.ajax({
                    type: "GET",
                    url: "/Appointment/AddAppintmentLineItem",
                    data: { id: appointmentCompletionId, inventoryId: inventoryId, itemId: inventoryItemId, itemPrice: itemprice, quantityUsed: quantityUsed },
                    cache: false,
                    success: function (result) {
                        $('#displayResults').html(result);
                        grid.dataSource.read();
                        grid.refresh();
                    },
                    error: function (result) {
                        //alert(result);
                    }
                });
            } else {
                $('#insufficientStock-confirmation').bPopup({
                    zIndex: 100000
                });
            }
        });

        //$("#btnReadyForInvoicing").click(function () {
        //    var id = $("#AppointmentCompletedId").val();
        //    $.ajax({
        //        type: "GET",
        //        url: "/Appointment/MarkAppointmentCompletedReadyForInvoicing",
        //        data: { appointmentCompletedId: id },
        //        cache: false,
        //        success: function (result) {
        //            $('#markedReadyForInvoicing-confirmation').bPopup({
        //                zIndex: 100000
        //            });
        //            //AppointmentAlreadyCompelted("Information", "markedReadyForInvoicing-confirmation");
        //            window.data("kendoWindow").close();
        //        },
        //        error: function (result) {
        //            //alert(result);
        //        }
        //    });
        //});

    } else {
        //$("#btnReadyForInvoicing").attr("disabled", "disabled");
    }
}
// This functoin is called when requesting a google map.
function DrawSBASMap(lat, lng) {
    var result = { Latitude: lat, Longitude: lng };
    var mapOptions = {
        center: new google.maps.LatLng(result.Latitude, result.Longitude),
        zoom: 21,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
    var infoWindow = new google.maps.InfoWindow();
    var lat_lng = new Array();
    var latlngbounds = new google.maps.LatLngBounds();
    var data = result;
    var myLatlng = new google.maps.LatLng(data.Latitude, data.Longitude);
    lat_lng.push(myLatlng);
    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: data.FullName
    });
    latlngbounds.extend(marker.position);
    (function (marker, data) {
        //google.maps.event.addListener(marker, "click", function (e) {
        //    infoWindow.setContent("Nothing for now"); //data.description
        //    infoWindow.open(map, marker);
        //});
    })(marker, data);



    map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);
}