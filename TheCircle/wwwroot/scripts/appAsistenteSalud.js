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
        //$compileProvider.debugInfoEnabled(false); //Activar en modo produccion
        //$logProvider.debugEnabled(false); //Activar en modo produccion
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
    .factory('dataFac', ['$log', '$http', '$rootScope', function ($log, $http, $rootScope) {

        function getStock(localidad) { //Se obtiene el stock completo de esa localidad
            $http.get("/api/itemfarmacia/" + localidad).then(function success(res) {
                $log.info("Stock de farmacia", res.data);
                this.stock = res.data;
                $rootScope.$broadcast('dataFac.stock'); //Se informa a los controladores que cambio stock
            }, function error(err) {
                $log.error("error cargar stock");
            })
        }

        function getRecetas(localidad) { //Se obtienen todas las recetas a despachar en esa localidad
            $http.get("/api/receta/" + localidad).then(function success(res) {
                $log.info("Recetas a despachar", res.data);
                this.recetas = res.data;
                $rootScope.$broadcast('dataFac.recetas'); //Se informa a los controladores que cambio recetas a despachar
            }, function error(err) {
                $log.error("error cargar recetas", err);
            })
        }

        function getDespachos(asistente) { //Se obtienen todos los despachos de la BDD
            $http.get("api/despacho/receta/" + asistente).then(function success(res) {
                $log.info("Despachos del personal", res.data);
                this.despachos = res.data;
                $rootScope.$broadcast('dataFac.despachos'); //Se informa a los controladores que cambio despachos
            }, function error(err) {
                $log.error("Error al cargar despachos", err);
            })
        }

        return {
            stock : null,
            recetas : null,
            despachos : null,
            localidad: "CC2",
            personal: 908362247,
            getStock: getStock,
            getRecetas: getRecetas,
            getDespachos: getDespachos
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

        function goTime(fn, time) {
            fn();
            $log.info("Go refresh by ", time);
            return setInterval(fn, time);
        }

        function stop(repeater) {
            $log.info("Stop refresh");
            clearInterval(repeater);
        }

        return {
            go: go,
            stop: stop,
            goTime: goTime
        }
    }])
    .controller('despachar', ["$log", "$scope", "$state", "$http", "dataFac", "notify", "crearDespacho", "refresh", function ($log, $scope, $state, $http, dataFac, notify, crearDespacho, refresh) {
        $scope.recetas = dataFac.recetas;
        $scope.receta = null;
        $scope.index = null;
        var actualizar = refresh.go(cargar)

        $scope.$on('dataFac.recetas', function () {
            $scope.recetas = dataFac.recetas;
        })

        function cargar() {
            if ($state.includes('despachar')) {
                dataFac.getRecetas(dataFac.localidad);
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
                    $('#myModal').modal('hide'); //Se cierra el modal
                    recetas.splice(index, 1);

                }, function error(e) {
                    $log.error("Error despacho", e);
                    $('#myModal').modal('hide'); //Se cierra el modal
                    notify("Error: ", "No se ha podido despachar", "danger");
                })

            } else {
                $log.error("No se han despachado todos los items", total);
                notify("Error: ", "No se han despachado todos los items", "danger");
            }
        }
    }])
    .controller('historial', ["$scope", "$state", "$http", "dataFac", "refresh", function ($scope, $state, $http, dataFac, refresh) {
        $scope.despachos = dataFac.despachos;
        $scope.receta = null;
        var actualizar = refresh.go(cargar, 30000);

        $scope.$on('dataFac.despachos', function () {
            $scope.despachos = dataFac.despachos;
        })

        function cargar() {
            if ($state.includes('historial')) {
                dataFac.getDespachos(dataFac.personal);
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.select = function (receta) {
            refresh.stop(actualizar); //Se detiene la carga de despachos cuando se abre el modal
            $scope.receta = receta;
        }

        $scope.close = function () {
            actualizar = refresh.go(cargar, 30000); //Se reanuda la carga de despachos al cerrar modal
        }
    }])
    .controller('stock', ["$log", "$scope", "$state", "$http", "dataFac", "refresh", function ($log, $scope, $state, $http, dataFac, refresh) {
        $scope.stock = dataFac.stock;
        var actualizar = refresh.go(cargar, 30000);

        $scope.$on('dataFac.stock', function () {
            $scope.stock = dataFac.stock;
        })

        function cargar() {
            if ($state.includes('stock')) {
                dataFac.getStock(dataFac.localidad);
            } else {
                refresh.stop(actualizar);
            }
        }
    }])
