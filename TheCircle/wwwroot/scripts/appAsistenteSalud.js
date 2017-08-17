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
            getStock: getStock,
            getRecetas: getRecetas
        }
    }])
    .controller('despachar', ["$log", "$scope", "$state", "$http", "dataFac", "notify", function ($log, $scope, $state, $http, dataFac, notify) {
        $scope.recetas = dataFac.recetas;
        $scope.receta = null;
        $scope.index = null;

        dataFac.getRecetas(dataFac.localidad).then(function success(res) {
            $log.info("Cargando recetas", res.data);
            dataFac.recetas = res.data;
            $scope.recetas = dataFac.recetas;
        }, function error(err) {
            $log.error("error cargar recetas", err);
        })


        $scope.select = function (receta, index) {
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

        $scope.guardarDespacho = function (receta, recetas, index) {
            $log.info("Receta a despachar", receta.items);

            try {
                var total = receta.items.reduce(function (sum, item) {
                    return sum + item.count;
                }, 0);

                if (total === receta.items.length) {
                    $log.info("Se puede despachar", total, receta.items);
                    notify("Exito: ", "Receta despachada exitosamente", "success");
                    recetas.splice(index, 1);

                } else {
                    $log.info("No se puede despachar", total, receta.items);
                    notify("Error: ", "No se han despachado todos los items", "danger");
                }
            } catch (e) {
                $log.error("Error try", e);
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
