/*
 appAsistente v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/

angular.module('appAsistente', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('despachar', {
                templateUrl: 'views/asistente/despachar.html',
                controller: 'despachar'
            })
            .state('despachar.receta', {
                templateUrl: 'views/asistente/despachar.receta.html',
                controller: 'despachar.receta'
            })
            .state('despachar.transferencias', {
                templateUrl: 'views/asistente/despachar.transferencias.html',
                controller: 'despachar.transferencias'
            })
            .state('despachar.pedidointerno', {
                templateUrl: 'views/asistente/despachar.pedidointerno.html',
                controller: 'despachar.pedidointerno'
            })
            .state('historial', {
                templateUrl: 'views/asistente/historial.html',
                controller: 'historial'
            })
            .state('historial.recetas', {
                templateUrl: 'views/asistente/historial.recetas.html',
                controller: 'historial.recetas'
            })
            .state('historial.transferencias', {
                templateUrl: 'views/asistente/historial.transferencias.html',
                controller: 'historial.transferencias'
            })
            .state('historial.pedidointerno', {
                templateUrl: 'views/asistente/historial.pedidointerno.html',
                controller: 'historial.pedidointerno'
            })
            .state('stock', {
                templateUrl: 'views/asistente/stock.html',
                controller: 'stock'
            })
            .state('ingresar', {
                templateUrl: 'views/asistente/ingresar.html',
                controller: 'ingresar'
            })
            .state('ingresar.items', {
                templateUrl: 'views/asistente/ingresar.items.html',
                controller: 'ingresar.items'
            })
            .state('ingresar.transferencias', {
                templateUrl: 'views/asistente/ingresar.transferencias.html',
                controller: 'ingresar.transferencias'
            });

        //False en modo de produccion
        $compileProvider.debugInfoEnabled(true)
        $compileProvider.commentDirectivesEnabled(true)
        $compileProvider.cssClassDirectivesEnabled(true)
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);

        loadTemplates($state, "despachar", $http, $templateCache);

    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            recetas: null,
            despachos: null,
            transferencias: null,
            transferenciasPorIngresar: null,
            pedidoInterno: null,
            getTransferenciasPorIngresar: getTransferenciasPorIngresar,
            getTransferenciasPendientes: getTransferenciasPendientes,
            getStock: getStock,
            getRecetas: getRecetas,
            getDespachos: getDespachos,
            getCompuestos: getCompuestos,
            getPedidoInterno: getPedidoInterno
        }

        function getPedidoInterno() {
            $http.get("/api/pedidointerno/pendientes").then(function success(res) {
                dataFac.pedidoInterno = res.data;
                $rootScope.$broadcast('dataFac.pedidoInterno');
            }, function(error) { console.log("Error: ", error) })
        }

        function getTransferenciasPorIngresar() {
            $http.get("/api/transferencia/despachada").then(function success(res) {
                dataFac.transferenciasPorIngresar = res.data;
                $rootScope.$broadcast('dataFac.transferenciasPorIngresar');
            }, function error(err) {
                console.log("Error cargar transferencias", err);
            })
        }

        function getTransferenciasPendientes() {
            $http.get("/api/transferencia").then(function success(res) {
                dataFac.transferencias = res.data;
                $rootScope.$broadcast('dataFac.transferencias');
            }, function error(err) {
                console.log("Error cargar transferencias", err);
            })
        }

        function getCompuestos() {
            $http.get("/api/compuesto").then(function success(res) {
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
            $http.get("/api/receta/localidad/pordespachar").then(function success(res) {
                console.log("Recetas a despachar", res.data);
                dataFac.recetas = res.data;
                $rootScope.$broadcast('dataFac.recetas'); //Se informa a los controladores que cambio recetas a despachar
            }, function error(err) {
                console.log("error cargar recetas", err);
            })
        }

        function getDespachos() { //Se obtienen todos los despachos de la BDD
            $http.get("api/receta/asistente").then(function success(res) {
                console.log("Despachos del personal", res.data);
                dataFac.despachos = res.data;
                $rootScope.$broadcast('dataFac.despachos'); //Se informa a los controladores que cambio despachos
            }, function error(err) {
                console.log("Error al cargar despachos", err);
            })
        }

        return dataFac;
    }])
    .factory('crearDespacho', ["$http", function ($http) {
        return function (receta) {

            var recetaId = receta.receta.id;
            items = [];

            receta.items.forEach(function (item) {

                var data = {
                    itemReceta: item.id,
                    cantidad: null,
                    comentario: item.comentario
                }

                if (item.nuevaCantidad) {
                    data.cantidad = item.nuevaCantidad;
                    items.push(data);

                } else {
                    data.cantidad = item.cantidad;
                    items.push(data);
                }
            })

            console.log("Items", items);
            return $http.put("api/receta/" + recetaId, items);

        }
    }])
    .controller('despachar', ["$state", function ($state) {
        $state.go("despachar.receta");
    }])
    .controller('despachar.receta', ["$scope", "$state", "$http", "dataFac", "crearDespacho", function ($scope, $state, $http, dataFac, crearDespacho) {
        $scope.recetas = dataFac.recetas;
        $scope.receta = null;
        $scope.index = null;
        var actualizar = refresh.go(cargar, 30);

        $scope.$on('dataFac.recetas', function () {
            $scope.recetas = dataFac.recetas;
        })

        function cargar() {
            if ($state.includes('despachar.receta')) {
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

            var total = receta.items.reduce(function (sum, item) {
                return sum + item.count;
            }, 0);

            if (total === receta.items.length) {
                crearDespacho(receta).then(function success(res) {

                    console.log("Despacho exitoso");
                    notify("Receta despachada exitosamente", "success");
                    $('#myModal').modal('hide'); //Se cierra el modal
                    //recetas.splice(index, 1);
                    actualizar = refresh.go(cargar, 30);

                }, function error(e) {
                    console.log("Error despacho", e);
                    $('#myModal').modal('hide');
                    notify("No se ha podido despachar", "danger");
                    actualizar = refresh.go(cargar, 30);
                })

            } else {
                actualizar = refresh.go(cargar, 30);
                console.log("No se han despachado todos los items", total);
                notify("No se han despachado todos los items", "danger");
            }
        }
    }])
    .controller('despachar.pedidointerno', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.pedidoInterno = dataFac.pedidoInterno;
        $scope.pedido = null;

        var actualizar = refresh.go(cargar, 30);

        function cargar() {
            if ($state.includes('despachar.pedidointerno')) {
                dataFac.getPedidoInterno();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.$on('dataFac.pedidoInterno', function() { $scope.pedidoInterno = dataFac.pedidoInterno })

        $scope.ver = function (pedido) {
            $scope.pedido = pedido
            $scope.comentario = null
        }

        $scope.guardarEgreso = function (cantidad, comentario) {
            var data = {
                cantidad: cantidad,
                comentario: comentario
            }

            NProgress.start();
            $http.put("api/pedidointerno/" + $scope.pedido.id + "/despachar", data).then(function success() {
                $('#ver_pedido').modal('hide');
                actualizar = refresh.go(cargar, 30);
                notify("Pedido despachado exitosamente", "success");
                NProgress.done();
            }, function error(e) {
                $('#ver_pedido').modal('hide');
                console.log("No se despacho", e)
                notify("No se pudo despachar", "danger")
                NProgress.done();
            })
        }   

    }])
    .controller('despachar.transferencias', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.transferencias = dataFac.transferencias;
        $scope.transferencia = null;

        var actualizar = refresh.go(cargar, 30);

        function cargar() {
            if ($state.includes('despachar.transferencias')) {
                return dataFac.getTransferenciasPendientes()
            } refresh.stop(actualizar)
        }        

        $scope.$on('dataFac.transferencias', function() { $scope.transferencias = dataFac.transferencias })

        $scope.ver = function (transferencia) {
            $scope.transferencia = transferencia
            $scope.comentario = null
        }

        $scope.guardarEgreso = function (nuevaCantidad, comentario) {
            var data = {
                idTransferencia: $scope.transferencia.id,
                cantidad: (function () {
                    if (nuevaCantidad) {
                        return nuevaCantidad
                    } return $scope.transferencia.cantidad
                })(),
                comentario: (function(){
                    if (comentario) {
                        return comentario
                    } return ""
                })()
            }

            NProgress.start();
            $http.put("api/transferencia", data).then(function success() {
                $('#ver_transferencia').modal('hide');
                actualizar = refresh.go(cargar, 30);
                notify("Transferencia despachada exitosamente", "success");
                NProgress.done()
            }, function error(e) {
                $('#ver_transferencia').modal('hide');
                console.log("No se despacho la transferencia", e)
                notify("No se pudo despachar la transferencia", "danger")
                NProgress.done()
            })
        }

    }])
    .controller('historial', ["$state", function ($state) {
        $state.go("historial.recetas")
    }])
    .controller('historial.recetas', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.despachos = dataFac.despachos;
        $scope.receta = null;
        var actualizar = refresh.go(cargar, 30);

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
            actualizar = refresh.go(cargar, 30); //Se reanuda la carga de despachos al cerrar modal
        }
    }])
    .controller('historial.transferencias', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {

    }])
    .controller('historial.pedidointerno', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {

    }])
    .controller('stock', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.stock = dataFac.stock;
        var actualizar = refresh.go(cargar, 30);

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
    .controller('ingresar', ["$state", function ($state) {
        $state.go("ingresar.items")        
    }])
    .controller('ingresar.items', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.compuestos = dataFac.compuestos;
        $scope.items = null;

        if ($scope.compuestos === null) {
            dataFac.getCompuestos();
        }

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
                $state.reload();
            }, function err(err) {
                console.log("No se pudo guardar el ingreso", err)
                notify("No se ha podido guardar el ingreso en farmacia", "danger");
            })
        }
    }])
    .controller('ingresar.transferencias', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.transferencias = dataFac.transferenciasPorIngresar;
        $scope.transferencia = null;

        var actualizar = refresh.go(cargar, 30);

        function cargar() {
            if ($state.includes('ingresar.transferencias')) {
                dataFac.getTransferenciasPorIngresar();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.$on('dataFac.transferenciasPorIngresar', function () {
            $scope.transferencias = dataFac.transferenciasPorIngresar;
        })

        $scope.ver = function (transferencia) {
            $scope.transferencia = transferencia;
        }

        $scope.guardarIngreso = function (comentario) {
            var data = {
                idTransferencia: $scope.transferencia.id,
                comentario: (function () {
                    if (comentario) {
                        return comentario
                    } return ""
                })()
            }

            NProgress.start();
            $http.post("/api/itemfarmacia/transferencia", data).then(function success(res) {
                $('#ver_transferencia').modal('hide');
                actualizar = refresh.go(cargar, 30);
                notify("Transferencia ingresada exitosamente", "success");
                NProgress.done();
            }, function error(err) {
                console.log("Error ingresar transferencia", err);
                notify("No se pudo ingresar la transferencia", "danger");
                NProgress.done();
            })
        }
    }])