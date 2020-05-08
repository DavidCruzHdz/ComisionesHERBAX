/*
    Autor : Hector Hernandez Martinez
    Descripcion : 
    Fecha : 05 07 2019
*/
var comisiones = 
{
    vAjaxData: null,
    objParameter: null,
    objListParameter: null,
    objwebMethodRequest: null,
    urlMain: null,

    init:function(){
    },

    load: function () {
    },

    /*Carga parametros y funciones globales*/
    getCurrentUrl: function () {
        var pathname = '';
        if (window.location.pathname != undefined)
        {
            pathname = window.location.pathname;
        }
        return pathname;
    },

    getData: function (webMethodRequest, pdata, urlRequest) { var iData = null; if (pdata == undefined) { vAjaxData = ''; } else { vAjaxData = pdata; } if (webMethodRequest != null) { objwebMethodRequest = this.urlMain + "/" + webMethodRequest; } else if (urlRequest != null) { objwebMethodRequest = this.urlMain + urlRequest } $.ajax({ type: "POST", url: objwebMethodRequest, data: vAjaxData, dataType: 'json', contentType: "application/json; charset=utf-8", cache: false, timeout: 600000, async: false, success: function (retorno) { iData = retorno; }, error: function (jqXHR, data, e) { if (jqXHR.status == 200) { } else if (jqXHR.status === 0) { /*alert('No es posible la conexion, verifique la red');*/ } else if (jqXHR.status == 404) { /*alert('Pagina no existente [404], verifique la url');*/ } else if (jqXHR.status == 500) { /*alert('Error interno [500].');*/ } else { /*alert('Error : ' + jqXHR.responseText);*/ } } }); return iData; },

    /**/
    validaBusquedaSocio: function () {
        var _valido = true;

        if ($("#Partner_id").val() == '' && $("#FirstName").val() == '' && $("#LastName").val() == '')
        {
            $("#Partner_id").focus();
            _valido = false;
        }

        return _valido;
    },

    buscarSocio: function () {
        if (this.validaBusquedaSocio()) {
            mostrarLoader();
            objParameter = "{ 'Partner_id': '" + $("#Partner_id").val() + "','FirstName': '" + $("#FirstName").val() + "','LastName': '" + $("#LastName").val() + "' }";
            var socios = this.getData("PrestamoSocio/buscarSocio", objParameter);
            $("#tbodySocios").empty();

            if (socios != null) {
                if (socios != undefined) {
                    //alert("num REG: " + socios.length);
                    if (socios.length > 0) {
                        //console.log(JSON.stringify(socios));
                        ocultarLoader();
                        var tdResult = "";
                        $.each(socios, function (key, value) {
                            tdResult += "<tr> <td>" + value.Partner_id + "</td> <td>" + value.Nombre + "</td> <td> <a href='../Prestamo/nuevo?Partner_id=" + value.Partner_id + "&nombre=" + value.Nombre + "&Id_Periodo=" + value.Id_Periodo + "' class=''btn btn-outline-secondary''>Crear Pr&eacute;stamo</a> </td> </tr>";
                        });

                        $("#tbodySocios").append(tdResult);
                    }
                }
            }
        }//Valida socio
    }
    ,
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    loadPrestamo: function () {
        var values = this.getUrlVars();
        if (values.Partner_id != undefined) {
            $("#Partner_id").val(values.Partner_id)
            $("#Partner_id").attr('readonly', true);
        }
        if (values.nombre != undefined) {
            $("#lblFullName").text(values.nombre);
        }
        $("#txtImporteSolicitar").focus();
        
    },
    soloNumeros: function (e) {
        
        var key = window.Event ? e.which : e.keyCode
        return (key >= 48 && key <= 57 && key != 46);

    },
    crearPrestamo: function () {
        //alert("Trabajando la Creacion del prestamo:   metodo controller:  crearPrestamo");
        if (this.validaDatosSolicitudPrestamo()) {
            objParameter = "{ 'Partner_id': '" + $("#Partner_id").val() + "','Importe': '" + $("#txtImporteSolicitar").val() + "','idFormaPago': '" + $("#slFormaPago").children("option:selected").val() + "' ,'NumeroPagos': '" + $("#txtNumeroPagos").val() + "','motivo': '" + $("#txtMotivoPrestamo").val() + "','id_Periodo': '" + $("#id_Periodo").val() + "'}";
            var response = this.getData(null, objParameter, "/Prestamo/crearPrestamo");
            if (response == 'ok') {
                //alertSystem("success", "Se creo registro de Pr&eacute;stamo.");
                //window.location.href = comisiones.urlMain + "/PrestamoSocio/";

                Swal.fire({
                    title: "Se creo registro de Pr&eacute;stamo.",
                    text: "Presione el boton para cerrar!",
                    type: "success",
                    showConfirmButton: true,
                    confirmButtonText: "Cerrar",
                    animation: false,
                    customClass: {
                        popup: 'animated tada'
                    }
                }).then(function (result) {
                    window.location.href = comisiones.urlMain + "/PrestamoSocio/";
                })
            }
        } else {
            alertSystem("success", "Todos los campos son obligatorios.");
        }
    },
    enviarSolicitudPrestamo: function () {

        if (this.validaDatosSolicitudPrestamo()) {
            objParameter = "{ 'Partner_id': '" + $("#Partner_id").val() + "','Importe': '" + $("#txtImporteSolicitar").val() + "','idFormaPago': '" + $("#slFormaPago").children("option:selected").val() + "' ,'NumeroPagos': '" + $("#txtNumeroPagos").val() + "','motivo': '" + $("#txtMotivoPrestamo").val() + "'}";
            var solicEnviada = this.getData(null, objParameter, "/Prestamo/enviarSolicitudPrestamo");
            if (solicEnviada == 'ok')
            {

                //alertSystem("success", "La solicitud se ha enviado exitosamente.");
                //window.location.href = comisiones.urlMain + "/PrestamoSocio/";


                Swal.fire({
                    title: "La solicitud se ha enviado exitosamente.",
                    text: "Presione el boton para cerrar!",
                    type: "success",
                    showConfirmButton: true,
                    confirmButtonText: "Cerrar",
                    animation: false,
                    customClass: {
                        popup: 'animated tada'
                    }
                }).then(function (result) {
                    window.location.href = comisiones.urlMain + "/PrestamoSocio/";
                })
            }
           /* else
            {
                alert("Lo sentimos"+ " " + solicEnviada);
                //window.location.href = comisiones.urlMain + "/PrestamoSocio/";
            } */
        }

    },

    validaDatosSolicitudPrestamo: function () {
        var solicitudCompletada = true;
        if ($("#txtImporteSolicitar").val() == '') {
            solicitudCompletada = false;
            $("#txtImporteSolicitar").focus();
        } else if ($("#slFormaPago").children("option:selected").val() == 0) {
            solicitudCompletada = false;
            $("#slFormaPago").focus();
        } else if ($("#txtNumeroPagos").val() == '') {
            solicitudCompletada = false;
            $("#txtNumeroPagos").focus();
        } else if ($("#txtMotivoPrestamo").val() == '') {
            solicitudCompletada = false;
            $("#txtMotivoPrestamo").focus();
        }

        return solicitudCompletada;
    }

}