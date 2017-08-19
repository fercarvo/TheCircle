/*
 appAsistente v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appAsistente', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", "$logProvider", function ($stateProvider, $compileProvider, $logProvider) {
        $stateProvider
            .state('despachar', {
                templateUrl: 'views/asistente/despachar.html',
                controller: 'despachar'
            })
            .state('historial', {
                templateUrl: 'views/asistente/historial.html',
                controller: 'historial'
            })
            .state('stock', {
                templateUrl: 'views/asistente/stock.html',
                controller: 'stock'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
        //$logProvider.debugEnabled(false); Activar en modo produccion
    }])
    .run(["$state", function ($state){
        $state.go("despachar");
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
                    offset: { x: 20, y: 70 },
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
        };
    }])
    .factory('dataFac', ['$http', function ($http) {

        function getStock(localidad) {
            return $http.get("/api/itemfarmacia/" + localidad);
        }
        
        function getRecetas(localidad) {
            return $http.get("/api/receta/" + localidad);
        }

        return {
            stock : null,
            recetas : null,
            localidad: "CC2",
            personal: 908362247,
            getStock: getStock,
            getRecetas: getRecetas
        }
    }])
    .factory('crearDespacho', ["$log", "dataFac", "$http", "notify", function ($log, dataFac, $http, notify) {
        return function (receta) {
            var despacho = {}

            despacho.id = receta.receta.id;
            despacho.items = [];

            receta.items.forEach(function (item) {

                var data = {
                    itemReceta: item.id,
                    cantidad: null,
                    personal: dataFac.personal,
                    comentario: item.comentario
                }

                if (item.nuevaCantidad < item.cantidad) {
                    data.cantidad = item.nuevaCantidad;
                    despacho.items.push(data);

                } else if (item.nuevaCantidad > item.cantidad || item.nuevaCantidad <= 0) {
                    $log.error("item nuevaCantidad erroneo", item.nuevaCantidad);
                    notify("Error: ", "La nueva cantidad a despachar es erronea", "danger");

                } else {
                    data.cantidad = item.cantidad;
                    despacho.items.push(data);
                }                
            })

            $log.info("Despacho", despacho);
            return $http.post("api/despacho/receta", despacho);

        }
    }])
    .factory('refresh', ["$log", function ($log) { //Sirve para ejecutar una funcion cada cierto tiempo y detenerla cuando se requiera.

        function go(fn) {
            fn();
            $log.info("Go refresh");
            return setInterval(fn, 10000);
        }

        function stop(repeater) {
            $log.info("Stop refresh");
            clearInterval(repeater);
        }

        return {
            go: go,
            stop: stop
        }
    }])
    .controller('despachar', ["$log", "$scope", "$state", "$http", "dataFac", "notify", "crearDespacho", "refresh", function ($log, $scope, $state, $http, dataFac, notify, crearDespacho, refresh) {
        $scope.recetas = dataFac.recetas;
        $scope.receta = null;
        $scope.index = null;
        var actualizar = refresh.go(cargar)

        function cargar() {
            if ($state.includes('despachar')) {
                $log.info("Ejecutando cargar");
                dataFac.getRecetas(dataFac.localidad).then(function success(res) {
                    $log.info("Recetas a despachar", res.data);
                    dataFac.recetas = res.data;
                    $scope.recetas = dataFac.recetas;
                }, function error(err) {
                    $log.error("error cargar recetas", err);
                })
            } else {
                refresh.stop(actualizar);
            }            
        }

        $scope.select = function (receta, index) {
            refresh.stop(actualizar);
            $scope.receta = angular.copy(receta);

            $scope.receta.items.forEach(function (item) {
                item.cambio = true;
            });

            $scope.index = index;
        }

        $scope.cambiar = function (item) {
            item.cambio = false;
        }

        $scope.despachar = function (item) {
            item.disable = true;
            item.class = "glyphicon glyphicon-ok"
            item.count = 1;
            $log.info("despachar", item);
        }

        $scope.guardarEgreso = function (receta, recetas, index) {
            actualizar = refresh.go(cargar);
            var total = receta.items.reduce(function (sum, item) {
                return sum + item.count;
            }, 0);

            if (total === receta.items.length) {
                $log.info("Se han despachado todos los items", total);

                crearDespacho(receta).then(function success(res) {

                    $log.info("Receta despachada", res.data);
                    notify("Exito: ", "Receta despachada exitosamente", "success");
                    recetas.splice(index, 1);

                }, function error(e) {
                    $log.info("Error despacho", e);
                    notify("Error: ", "No se ha podido despachar", "danger");
                })                

            } else {
                $log.info("No se han despachado todos los items", total);
                notify("Error: ", "No se han despachado todos los items", "danger");
            }
        }
    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};


    }])
    .controller('stock', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.stock = dataFac.stock;

        dataFac.getStock(dataFac.localidad).then(function success(res) {
            dataFac.stock = res.data;
            $scope.stock = dataFac.stock;
        }, function error(err) {
            console.log("error cargar stock");
            alert("error cargar stock");
        })

    }])
