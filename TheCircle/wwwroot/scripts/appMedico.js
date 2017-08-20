/*
 appMedico v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appMedico', ['ui.router', 'nvd3'])
    .config(["$stateProvider", "$compileProvider", "$logProvider", function ($stateProvider, $compileProvider, $logProvider) {
        $stateProvider
            .state('atencion', {
                templateUrl: 'views/medico/atencion.html',
                controller: 'atencion'
            })
            .state('atencion.registro', {
                templateUrl: 'views/medico/atencion.registro.html',
                controller: 'atencion.registro'
            })
            .state('atencion.receta', {
                templateUrl: 'views/medico/atencion.receta.html',
                controller: 'atencion.receta'
            })
            .state('atencion.remision', {
                templateUrl: 'views/medico/atencion.remision.html',
                controller: 'atencion.remision'
            })
            .state('anulaciones', {
                templateUrl: 'views/medico/anulaciones.html',
                controller: 'anulaciones'
            })
            .state('estadisticas', {
                templateUrl: 'views/medico/estadistica.html',
                controller: 'estadisticas'
            })
            .state('estadisticas.atenciones', {
                templateUrl: 'views/medico/estadistica.atenciones.html',
                controller: 'estadisticas.atenciones'
            })
            .state('estadisticas.remisiones', {
                templateUrl: 'views/medico/estadistica.remisiones.html',
                controller: 'estadisticas.remisiones'
            })
            .state('estadisticas.recetas', {
                templateUrl: 'views/medico/estadistica.recetas.html',
                controller: 'estadisticas.recetas'
            })
            .state('estadisticas.enfermedades', {
                templateUrl: 'views/medico/estadistica.enfermedades.html',
                controller: 'estadisticas.enfermedades'
            });
        //$compileProvider.debugInfoEnabled(false); //Activar en modo producción
        $logProvider.debugEnabled(true); //Activar en modo produccion
    }])
    .run(["$state", function ($state){
        $state.go("atencion");
    }])
    .directive('mdTable', function(){
        return {
            restrict: 'EA', //E = element, A = attribute, C = class, M = comment
            scope: { data: '='},
            templateUrl: 'views/directive/table.html'
        }
    })
    .factory('date', ["$log", function ($log) {
        return function (date) {
            try {
                var format = new Date(date);
                var day = format.getDate();
                var month = format.getMonth() + 1;
                var year = format.getFullYear();
                return day + '/' + month + '/' + year;
            } catch (e) {
                console.log(e);
                return null;
            }
        };;
    }])
    .factory('dataFactory', ['$http', function ($http) {

        function getInstituciones() {
            return $http.get("/api/institucion");
        }

        function getEnfermedades() {
            return $http.get("/api/enfermedad");
        }

        function getStock(localidad) {
            return $http.get("/api/itemfarmacia/" + localidad);
        }

        function getRecetas(doctor) {
            return $http.get("/api/reporte/receta/" + doctor);
        }

        return {
            enfermedades: null,
            instituciones: null,
            stock: null,
            recetas: null,
            //estadisticas : {}, //Se guardaba la data, pero ya no por pedido gerencia
            tipos: ["curativo", "seguimiento", "control"],
            getInstituciones: getInstituciones,
            getEnfermedades: getEnfermedades,
            getStock: getStock,
            getRecetas: getRecetas
        }
    }])
    .factory('disable', [function () {
        return {
            atencion : false,
            remision : false,
            receta : false
        }
    }])
    .factory('notify', [function () {
        return function (titulo, mensaje, tipo) {

            var icono;

            if (tipo === "success") {
                icono = "glyphicon glyphicon-saved";
            } else if (tipo == "danger") {
                icono = "glyphicon glyphicon-ban-circle"
            }

            $.notify(
                {
                    icon: icono,
                    title: titulo,
                    message: mensaje,
                    url: '#',
                    target: '_blank'
                },
                {
                    element: 'body',
                    position: null,
                    showProgressbar: true,
                    type: tipo,
                    allow_dismiss: true,
                    newest_on_top: false,
                    showProgressbar: false,
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    offset: {x:20,y:70},
                    spacing: 10,
                    z_index: 1031,
                    delay: 1000,
                    timer: 1000,
                    url_target: '_blank',
                    mouse_over: "pause",
                    animate: {
                        enter: 'animated bounceIn',
                        exit: 'animated bounceOut'
                    },
                    onShow: null,
                    onShown: null,
                    onClose: null,
                    onClosed: null,
                    icon_type: 'class'
                });
        }
    }])
    .factory('atencionFactory', [function () { //factory donde se guarda toda la data ingresada
        return {
            doctor : 908362247,
            localidad : "CC2",
            apadrinado : {},
            foto : "/images/ci.png",
            codigo : null,
            atencion : null,
            diagnosticos : null,
            remision : null,
            receta: {
                items: [],
                id: null
            },
            status : true
        }
    }])
    .factory('refresh', ["$log", function ($log) { //Sirve para ejecutar una funcion cada cierto tiempo y detenerla cuando se requiera.

        function go(fn) {
            fn();
            console.log("Go refresh");
            return setInterval(fn, 10000);
        }

        function goTime(fn, time) {
            fn();
            console.log("Go refresh by ", time);
            return setInterval(fn, time);
        }

        function stop(repeater) {
            console.log("Stop refresh");
            clearInterval(repeater);
        }

        return {
            go: go,
            stop: stop,
            goTime: goTime
        }
    }])
    .controller('atencion', ["$log", "$scope", "$state", "$http", "atencionFactory", "disable", function ($log, $scope, $state, $http, atencionFactory, disable) {

        $state.go('atencion.registro');
        $scope.disable = disable.atencion;
        $scope.apadrinado = atencionFactory.apadrinado;
        $scope.foto = atencionFactory.foto;
        $scope.status = atencionFactory.status;

        $scope.$on('disable', function (event, data) {
            $scope.disable = disable.atencion;
            console.log("Se desactivo atencion.html");
        });

        $scope.buscarApadrinado = function (codigo) {
            $http.get("/api/apadrinado/" + codigo).then(function success(res) {

                if (res.data.status === "D" || res.data.status === "E") {
                    $scope.status = false;
                    atencionFactory.status = false;
                } else {
                    $scope.status = true;
                }
                $scope.foto = "/api/apadrinado/foto/" + codigo;
                atencionFactory.apadrinado = res.data;
                $scope.apadrinado = atencionFactory.apadrinado;
                atencionFactory.codigo = codigo;

            }, function error(err) {

                console.log("No existe apadrinado", err);
                atencionFactory.apadrinado = {};
                $scope.apadrinado = atencionFactory.apadrinado;
                atencionFactory.foto = "/images/ci.png";
                $scope.foto = atencionFactory.foto;
                atencionFactory.status = true;
                $scope.status = atencionFactory.status;
                atencionFactory.codigo = null;
                $scope.codigo = atencionFactory.codigo;
            });
        };

    }])
    .controller('atencion.registro', ["$log", "$scope", "$state", "$http", "dataFactory", "atencionFactory", "disable", "notify", function ($log, $scope, $state, $http, dataFactory, atencionFactory, disable, notify) {

        $scope.disable = disable.atencion;
        $scope.enfermedades = dataFactory.enfermedades;
        $scope.tipos = dataFactory.tipos;
        $scope.atencion = atencionFactory.atencion;


        $scope.activar = function () {
            $(".myselect").select2();
        }


        if (dataFactory.enfermedades === null) {
            dataFactory.getEnfermedades().then(function success(res) {
                dataFactory.enfermedades = res.data;
                $scope.enfermedades = dataFactory.enfermedades;
            }, function error(err) {
                console.log("Error cargar enfermedades", err);
            })
        }


        $scope.reset = function () {
            $scope.atencion = {};
        };

        $scope.$watch('atencion', function() {
            atencionFactory.atencion = $scope.atencion;
        });

        $scope.send = function () {
            var AtencionNueva = {
                doctor: atencionFactory.doctor,
                apadrinado: atencionFactory.codigo,
                tipo: atencionFactory.atencion.tipo,
                diagnosticos: [atencionFactory.atencion.diagp,
                  atencionFactory.atencion.diag1,
                  atencionFactory.atencion.diag2],
                localidad: atencionFactory.localidad,
                peso: atencionFactory.atencion.peso,
                talla: atencionFactory.atencion.talla
            }

            $http.post("/api/atencion", AtencionNueva).then(function success(res){

                console.log("Se creo atencion", res.data);
                disable.atencion = true;
                atencionFactory.atencion = res.data.atencion; //Se guarda la data ingresada en la factory
                atencionFactory.diagnosticos = res.data.diagnosticos; //Se guarda la data ingresada en la factory
                $scope.disable = disable.atencion; //Se desactiva atencion.registro.html
                $scope.$emit('disable', {}); //evento para desactivar atencion.html
                notify("Exito", "Apadrinado creado satisfactoriamente", "success");
                $state.go('atencion.receta');

            }, function error(err) {
                notify("Error", "Intento fallido de atencion medica", "danger");
                console.log("error atencion", err);
            });
        }

    }])
    .controller('atencion.remision', ["$log", "$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory","notify", function ($log, $scope, $state, $http, disable, dataFactory, atencionFactory,notify) {

        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = atencionFactory.diagnosticos

        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (dataFactory.instituciones === null) {
            dataFactory.getInstituciones().then(function success(res) {
                dataFactory.instituciones = res.data;
                $scope.instituciones = dataFactory.instituciones;
            }, function error(err) {
                console.log("error cargar instituciones", err);
            })
        }

        $scope.send = function (remision) {
            var RemisionRequest = {
                atencionM: atencionFactory.atencion.id,
                institucion: remision.institucion,
                monto: remision.monto,
                sintomas: remision.sintomas
            }

            $http.post("/api/remision", RemisionRequest).then(function success(res) {

                console.log("se creo remision", res.data);
                disable.remision = true;
                atencionFactory.remision = res.data; //Se guarda la remision en la factory
                $scope.disable = disable.remision; //Se desactiva atencion.remision.html
                notify("Exito", "Se creo la remision exitosamente", "success");

            }, function (err) {
                console.log("error crear remision", err);
                notify("Error", "No se pudo generar la remision", "danger");
            });
        }

    }])
    .controller('atencion.receta', ["$log", "$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory", "notify", function ($log, $scope, $state, $http, disable, dataFactory, atencionFactory,notify) {

        $scope.disable = disable.receta;
        $scope.stock = dataFactory.stock;
        $scope.receta = atencionFactory.receta;
        $scope.ItemRecetaNuevo = {};
        $scope.editarItem = true;
        $scope.diagnosticos = atencionFactory.diagnosticos;
        getStock();


        $scope.activar = function () {
            $(".myselect").select2();
        }

        function getStock() {
            if (atencionFactory.codigo !== null) {
                dataFactory.getStock(atencionFactory.localidad).then(function success(res) {
                    dataFactory.stock = res.data;
                    $scope.stock = dataFactory.stock;
                }, function error(err) {
                    console.log("Error cargar Stock de farmacia", err);
                })
            };
        }

        if (atencionFactory.receta.id === null) {
            var RecetaRequest = {
              doctor: atencionFactory.doctor,
              apadrinado: atencionFactory.codigo
            }

            $http.post("/api/receta", RecetaRequest).then(function success(res) {
                console.log("Se creo receta", res.data);
                atencionFactory.receta.id = res.data.id;
                $scope.receta.id = atencionFactory.receta.id;
            }, function err(err) {
                console.log("No se creo receta", err);
            });

        }

        $scope.addItenReceta = function (item) {
            $('.modal').modal('hide');
            var obj = angular.copy(item);
            console.log("Item copiado ", obj);

            atencionFactory.receta.items.push(obj);
            $scope.receta.items = atencionFactory.receta.items;
            console.log("Receta despues de agregar item", $scope.receta.items);
        }

        $scope.eliminarItem = function (itemsReceta, index) {
            console.log("receta", itemsReceta);
            itemsReceta.splice(index, 1);
            atencionFactory.receta.items = itemsReceta;
        }

        $scope.select = function (item) {
            $scope.ItemRecetaNuevo.itemFarmacia = angular.copy(item);
            $scope.ItemRecetaNuevo.diagnostico = {};
            $scope.ItemRecetaNuevo.cantidad = 1;
            $scope.ItemRecetaNuevo.posologia = "";
        }

        $scope.guardarReceta = function () {
            var data = { idReceta: atencionFactory.receta.id, items: atencionFactory.receta.items };

            $http.post("/api/itemsreceta", JSON.parse(angular.toJson(data))).then(function success(res) {
                console.log("Se crearon los items", res.data);
                notify("Exito", "Se creo la receta exitosamente", "success");
                disable.receta = true;
                $scope.disable = disable.receta; //Se desactiva atencion.receta.html
                getStock();
            }, function err(err){
                console.log("No se pudieron crear los items", err);
                notify("Error", "No se pudo crear la receta", "danger");
            });
        }


    }])
    .controller('anulaciones', ["$log", "$scope", "$state", "$http", "atencionFactory", "notify", "dataFactory", function ($log, $scope, $state, $http, atencionFactory, notify, dataFactory) {
        console.log("en anulaciones");

        $scope.recetas = dataFactory.recetas;
        $scope.receta = null;

        function cargar() {
            dataFactory.getRecetas(atencionFactory.doctor).then(function success(res) {
                console.log("recetas by status", res.data);
                dataFactory.recetas = res.data;
                $scope.recetas = dataFactory.recetas;
            }, function error(err) {
                console.log("error cargar recetas", err);
            })
        }

        $scope.select = function (receta) {
            $scope.receta = receta;
        }

        $scope.eliminarReceta = function (receta) {

            $http.delete("/api/receta/" + receta.receta.id).then(function success(res) {
                console.log("Receta eliminada con exito", receta, res);
                notify("Exito", "Receta eliminada con exito", "success");
                $scope.init();

            }, function error(err) {
                console.log("No se pudo eliminar receta", err);
                notify("Error", "Receta no se pudo eliminar", "danger");
            });
        }



    }])
    .controller('estadisticas', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $state.go('estadisticas.enfermedades');

    }])
    .controller('estadisticas.atenciones', ["$log", "$scope", "$state", "$http", "dataFactory", "atencionFactory", "date", function ($log, $scope, $state, $http, dataFactory, atencionFactory, date) {
        //$scope.atenciones = dataFactory.estadisticas.atenciones;

        //Se guardaba la data, pero ya no por pedido de gerencia sistemas
        /*$scope.$watch('atenciones', function () {
            dataFactory.estadisticas.atenciones = $scope.atenciones;
        });*/

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: date(desde),
                hasta: date(hasta),
                doctor: atencionFactory.doctor
            }

            $http.post("/api/reporte/atencion", data).then(function success(res) {
                $scope.atenciones.all = res.data;
            }, function error(err) {
                console.log("error cargar atenciones", err);
                alert("error cargar atenciones")
            });
        }

    }])
    .controller('estadisticas.remisiones', ["$log", "$scope", "$state", "$http", "dataFactory", "atencionFactory", "date", function ($log, $scope, $state, $http, dataFactory, atencionFactory, date) {
        //$scope.remisiones = dataFactory.estadisticas.remisiones;

        //Se guardaba la data, pero ya no por pedido de gerencia sistemas
        /*
        $scope.$watch('remisiones', function () {
            dataFactory.estadisticas.remisiones = $scope.remisiones;
        });
        */

        $scope.generar = function (desde, hasta) {

            var data = {
                desde: date(desde),
                hasta: date(hasta),
                doctor: atencionFactory.doctor
            }

            $http.post("/api/reporte/remision", data).then(function success(res) {
                $scope.remisiones.all = res.data;
            }, function error(err) {
                console.log("error cargar remisiones", err);
                alert("error cargar remisiones")
            });
        }

    }])
    .controller('estadisticas.recetas', ["$log", "$scope", "$state", "$http", "dataFactory", "atencionFactory", "notify", "date", function ($log, $scope, $state, $http, dataFactory, atencionFactory, notify, date) {
        //$scope.recetas = dataFactory.estadisticas.recetas;

        //Se guardaba la data, pero ya no por pedido de gerencia sistemas
        /*
        $scope.$watch('recetas', function () {
            dataFactory.estadisticas.recetas = $scope.recetas;
        });
        */

        $scope.select = function (receta) {
            $scope.receta = receta;
        }

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: date(desde),
                hasta: date(hasta),
                doctor: atencionFactory.doctor
            }

            $http.post("/api/reporte/receta", data).then(function success(res) {
                $scope.recetas.all = res.data;
            }, function error(err) {
                console.log("error cargar recetas")
                notify("Error", "No se pudo cargar recetas", "danger");
            });
        }

    }])
    .controller('estadisticas.enfermedades', ["$log", "$scope", "$state", "$http", "dataFactory", "atencionFactory", "date", function ($log, $scope, $state, $http, dataFactory, atencionFactory, date) {

        //$scope.enfermedades = dataFactory.estadisticas.enfermedades;
        $scope.data = [];

        //Se guardaba la data, pero ya no por pedido de gerencia sistemas
        /*$scope.$watch('enfermedades', function () {
            dataFactory.estadisticas.enfermedades = $scope.enfermedades;
        });
        */

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: date(desde),
                hasta: date(hasta),
                localidad: atencionFactory.localidad
            }
            $http.post("/api/reporte/enfermedad", data).then(function success(res) {
                $scope.data = [];

                for (i = 0; i < res.data.length; i++) {
                    $scope.data.push({ key: res.data[i].codigo + ' ' + res.data[i].nombre, y: res.data[i].veces, color: color[i] });
                }

                /*$scope.data = res.data.map(function (obj) {
                    return { key: obj.codigo + ' ' + obj.nombre, y: obj.veces, color: "red" };
                });*/
                //$scope.data = arr;
            }, function error(err) {
                console.log("Error cargar estadisticas", err);
                alert("Error cargar estadisticas");
            });

        }

        var color = ["#901F61", "#009877", "#D64227", "#FED115", "#ADBF2B"];

        $scope.options = {
            chart: {
                type: 'pieChart',
                height: 340,
                x: function (d) { return d.key; },
                y: function (d) { return d.y; },
                showLabels: false,
                duration: 500,
                labelThreshold: 0.01,
                labelSunbeamLayout: true,
                legendPosition: "right",
                legend: {
                    margin: {
                        top: 5,
                        right: 35,
                        bottom: 5,
                        left: 0
                    }
                }
            }
        };
    }])
