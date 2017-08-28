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
            })
            .state('ingresar', {
                templateUrl: 'views/asistente/ingresar.html',
                controller: 'ingresar'
            });
        //$compileProvider.debugInfoEnabled(false); //Activar en modo produccion
        $logProvider.debugEnabled(true); //Activar en modo produccion
    }])
    .run(["$state", function ($state){
        $state.go("despachar");
    }])
    .factory('notify', [function () {

        return function (mensaje, tipo) {

            var icono;

            if (tipo === "success") {
                icono = "glyphicon glyphicon-saved";
            } else if (tipo === "danger") {
                icono = "glyphicon glyphicon-ban-circle"
            }

            $.notify(
                {
                    icon: icono,
                    //title: titulo,
                    message: mensaje,
                    url: '#',
                    target: '_blank'
                },
                {
                    element: 'body',
                    position: null,
                    showProgressbar: false,
                    type: tipo,
                    allow_dismiss: true,
                    newest_on_top: false,
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
    .factory('date', [function () {
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
        };
    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            recetas: null,
            despachos: null,
            localidad: "",
            personal: 0,
            getStock: getStock,
            getRecetas: getRecetas,
            getDespachos: getDespachos,
            getCompuestos: getCompuestos
        }

        function getCompuestos() {
            $http({
                method: "GET",
                url: "/api/itemnombre"
            }).then(function success(res) {
                dataFac.compuestos = res.data;
                $rootScope.$broadcast('dataFac.compuestos'); //Se informa a los controladores que cambio
            }, function error(err) {
                console.log("Error cargar nombre items farmacia", err);
            })
        }

        function getStock() { //Se obtiene el stock completo de esa localidad
            $http.get("/api/itemfarmacia/").then(function success(res) {
                console.log("Stock de farmacia", res.data);
                dataFac.stock = res.data;
                $rootScope.$broadcast('dataFac.stock'); //Se informa a los controladores que cambio stock
            }, function error(err) {
                console.log("error cargar stock");
            })
        }

        function getRecetas() { //Se obtienen todas las recetas a despachar en esa localidad
            $http({
                method: "GET",
                url: "/api/receta",
                params: { status: 0}
            }).then(function success(res) {
                console.log("Recetas a despachar", res.data);
                dataFac.recetas = res.data;
                $rootScope.$broadcast('dataFac.recetas'); //Se informa a los controladores que cambio recetas a despachar
            }, function error(err) {
                console.log("error cargar recetas", err);
            })
        }

        function getDespachos() { //Se obtienen todos los despachos de la BDD
            $http.get("api/despacho/receta").then(function success(res) {
                console.log("Despachos del personal", res.data);
                dataFac.despachos = res.data;
                $rootScope.$broadcast('dataFac.despachos'); //Se informa a los controladores que cambio despachos
            }, function error(err) {
                console.log("Error al cargar despachos", err);
            })
        }

        return dataFac;
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
                    console.log("item nuevaCantidad erroneo", item.nuevaCantidad);
                    notify("Error, a nueva cantidad a despachar es erronea", "danger");

                } else {
                    data.cantidad = item.cantidad;
                    despacho.items.push(data);
                }
            })

            console.log("Despacho", despacho);
            return $http.post("api/despacho/receta", despacho);

        }
    }])
    .factory('refresh', ["$log", function ($log) { //Sirve para ejecutar una funcion cada cierto tiempo y detenerla cuando se requiera.

        function go(fn) {
            fn();
            console.log("Go refresh");
            return setInterval(fn, 20000);
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
    .controller('despachar', ["$log", "$scope", "$state", "$http", "dataFac", "notify", "crearDespacho", "refresh", function ($log, $scope, $state, $http, dataFac, notify, crearDespacho, refresh) {
        $scope.recetas = dataFac.recetas;
        $scope.receta = null;
        $scope.index = null;
        var actualizar = refresh.go(cargar);

        $scope.$on('dataFac.recetas', function () {
            $scope.recetas = dataFac.recetas;
        })

        function cargar() {
            if ($state.includes('despachar')) {
                dataFac.getRecetas();
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
            console.log("despachar", item);
        }

        $scope.guardarEgreso = function (receta, recetas, index) {
            //actualizar = refresh.go(cargar);
            var total = receta.items.reduce(function (sum, item) {
                return sum + item.count;
            }, 0);

            if (total === receta.items.length) {
                console.log("Se han despachado todos los items", total);

                crearDespacho(receta).then(function success(res) {

                    console.log("Despacho exitoso");
                    notify("Receta despachada exitosamente", "success");
                    $('#myModal').modal('hide'); //Se cierra el modal
                    recetas.splice(index, 1);
                    actualizar = refresh.go(cargar);

                }, function error(e) {
                    console.log("Error despacho", e);
                    $('#myModal').modal('hide'); //Se cierra el modal
                    notify("No se ha podido despachar", "danger");
                    actualizar = refresh.go(cargar);
                })

            } else {
                actualizar = refresh.go(cargar);
                console.log("No se han despachado todos los items", total);
                notify("No se han despachado todos los items", "danger");
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
    .controller('stock', ["$scope", "$state", "$http", "dataFac", "refresh", function ($scope, $state, $http, dataFac, refresh) {
        $scope.stock = dataFac.stock;
        var actualizar = refresh.go(cargar, 30000);

        $scope.$on('dataFac.stock', function () {
            $scope.stock = dataFac.stock;
        })

        function cargar() {
            if ($state.includes('stock')) {
                dataFac.getStock();
            } else {
                refresh.stop(actualizar);
            }
        }
    }])
    .controller('ingresar', ["$scope", "$http", "dataFac", "notify", "date", function ($scope, $http, dataFac, notify, date) {
        $scope.compuestos = dataFac.compuestos;
        $scope.items = null;
        dataFac.getCompuestos();
        //var actualizar = refresh.go(cargar, 30000);

        $scope.$on('dataFac.compuestos', function () {
            $scope.compuestos = dataFac.compuestos;
        })

        $scope.crear = function (compuesto, item, fecha, cantidad) {
            var data = {
                compuesto: compuesto,
                nombre: item,
                fcaducidad: date(fecha),
                cantidad: cantidad                
            }
            console.log("data a enviar", data);

            $http.post("api/itemfarmacia", data).then(function sucess(res) {
                console.log("Ingreso exitoso", res.data);
                notify("Ingreso en farmacia exitoso", "success");
                compuesto = item = fecha = cantidad = {};
            }, function err(err) {
                console.log("No se pudo guardar el ingreso", err)
                notify("No se ha podido guardar el ingreso en farmacia", "danger");
            })
        }

        $scope.cambioCompuesto = function (items) {
            $scope.items = items;
        }


    }])
