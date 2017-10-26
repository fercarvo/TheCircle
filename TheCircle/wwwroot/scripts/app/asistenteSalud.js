/*
    asistenteSalud v1.0 
    Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
    Children International
*/

angular.module('asistenteSalud', ['ui.router'])
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
            /*.state('historial.pedidointerno', { //NOT IMPLEMENTED
                templateUrl: 'views/asistente/historial.pedidointerno.html',
                controller: 'historial.pedidointerno'
            })*/
            .state('historial.ingresoItems', {
                templateUrl: 'views/asistente/historial.ingresoItems.html',
                controller: 'historial.ingresoItems'
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
        $compileProvider.debugInfoEnabled(false)
        $compileProvider.commentDirectivesEnabled(false)
        $compileProvider.cssClassDirectivesEnabled(false)
    }])
    .run(["$state", "$http", "$templateCache", "dataFac", "$rootScope", function ($state, $http, $templateCache, dataFac, $rootScope) {
        $rootScope.notificaciones = {}

        refresh.go(function () {
            dataFac.getRecetas()
            dataFac.getTransferenciasPendientes({})
            dataFac.getPedidoInterno()
        }, 1)

        checkSession($http);

        loadTemplates($state, "despachar", $http, $templateCache);

    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            recetas: null,
            recetasDespachadas: {
                desde: null,
                hasta: null,
                data: null
            },
            transferencias: null,
            transferenciasPorIngresar: null,
            pedidoInterno: null,
            transferenciasDespachadas: {
                desde: null,
                hasta: null,
                data: null
            },
            itemsRegistrados: {
                desde: null,
                hasta: null,
                data: null
            },
            getItemsRegistrados: getItemsRegistrados,
            getTransferenciasDespachadas: getTransferenciasDespachadas,
            getTransferenciasPorIngresar: getTransferenciasPorIngresar,
            getTransferenciasPendientes: getTransferenciasPendientes,
            getStock: getStock,
            getRecetas: getRecetas,
            getDespachos: getDespachos,
            getCompuestos: getCompuestos,
            getPedidoInterno: getPedidoInterno,
            despacharTransferencia: despacharTransferencia
        }

        function getItemsRegistrados($scope, data) {
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/itemfarmacia/registro",
                params: data
            }).then(function (res) {
                console.log("Items registrados", res.data)
                NProgress.done();
                $scope.items.data = res.data;
            }, function (err) {
                console.log("error getItemsRegistrados", err)
                notify("No se pudo cargar los items", "danger");
                NProgress.done();
            })
        }

        function getTransferenciasDespachadas($scope, data) {
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/transferencia/despachada/personal",
                params: data
            }).then(function (res) {
                console.log("Transferencias despachadas", res.data)
                NProgress.done();
                $scope.transferencias.data = res.data;
            }, function (err) {
                console.log("error getTransferenciasDespachadas", err)
                notify("No se pudo cargar las trasnferencias despachadas", "danger");
                NProgress.done();
            })
        }

        function despacharTransferencia(data, $scope) {
            NProgress.start();
            $http.put("/api/transferencia/" + data.idTransferencia + "/despachar", data).then(function (res) {
                $('#ver_transferencia').modal('hide');
                console.log("Transferencia despachada", res.data)
                getTransferenciasPendientes($scope)
                notify("Transferencia despachada exitosamente", "success");
                NProgress.done()
            }, function error(e) {
                $('#ver_transferencia').modal('hide');
                console.log("No se despacho la transferencia", e)
                notify("No se pudo despachar la transferencia", "danger")
                NProgress.done()
            })
        }

        function getPedidoInterno() {
            $http.get("/api/pedidointerno/pendientes").then(function success(res) {
                dataFac.pedidoInterno = res.data;
                $rootScope.notificaciones.pedidos = dataFac.pedidoInterno.length
                $rootScope.$broadcast('dataFac.pedidoInterno');
            }, function(error) { console.log("Error: ", error) })
        }

        function getTransferenciasPorIngresar($scope) {
            $http.get("/api/transferencia/despachada").then(function (res) {
                console.log("getTransferenciasPorIngresar", res.data)
                dataFac.transferenciasPorIngresar = res.data;
                $scope.transferencias = dataFac.transferenciasPorIngresar
            }, function (error) {
                console.log("Error getTransferenciasPorIngresar", error);
            })
        }

        function getTransferenciasPendientes($scope) {
            $http.get("/api/transferencia").then(function (res) {
                console.log("Transferencias pendientes", res.data)
                dataFac.transferencias = res.data;
                $rootScope.notificaciones.transferencias = dataFac.transferencias.length
                $scope.transferencias = dataFac.transferencias
            }, function (error) {
                console.log("Error cargar transferencias", error);
                notify("No se pudieron cargar las transferencias", "danger")
            })
        }

        function getCompuestos($scope) {
            $http.get("/api/compuesto").then(function success(res) {
                dataFac.compuestos = res.data;
                $scope.compuestos = dataFac.compuestos
                //$rootScope.$broadcast('dataFac.compuestos'); //Se informa a los controladores que cambio
            }, function error(err) {
                console.log("Error cargar nombre items farmacia", err);
            })
        }

        function getStock($scope) { //Se obtiene el stock completo de esa localidad
            $http.get("/api/itemfarmacia/").then(function success(res) {
                console.log("Stock de farmacia", res.data);
                dataFac.stock = res.data;
                $scope.stock = dataFac.stock
            }, function (error) {
                console.log("error getStock", error);
            })
        }

        function getRecetas() { //Se obtienen todas las recetas a despachar en esa localidad
            $http.get("/api/receta/localidad/pordespachar").then(function success(res) {
                console.log("Recetas a despachar", res.data);
                dataFac.recetas = res.data;
                $rootScope.notificaciones.recetas = dataFac.recetas.length
                $rootScope.$broadcast('dataFac.recetas'); //Se informa a los controladores que cambio recetas a despachar
            }, function error(err) {
                console.log("error cargar recetas", err);
            })
        }

        function getDespachos($scope, data) { //Se obtienen todos los despachos de la BDD
            NProgress.start();
            $http({
                method: "GET",
                url: "/api/receta/asistente",
                params: data
            }).then(function (res) {
                console.log("getDespachos", res.data);
                $scope.despachos.data = res.data
                NProgress.done();
            }, function (error) {
                console.log("error getDespachos", error)
                notify("Error al cargar la informacion", "danger");
                NProgress.done();
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
        var actualizar = refresh.go(cargar, 1);

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
                    actualizar = refresh.go(cargar, 1);

                }, function error(e) {
                    console.log("Error despacho", e);
                    $('#myModal').modal('hide');
                    notify("No se ha podido despachar", "danger");
                    actualizar = refresh.go(cargar, 1);
                })

            } else {
                actualizar = refresh.go(cargar, 1);
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
            $scope.cantidad = null
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
    .controller('despachar.transferencias', ["$scope", "dataFac", function ($scope, dataFac) {
        $scope.transferencias = dataFac.transferencias;
        $scope.transferencia = null;

        var actualizar = refresh.go(cargarTransferencias, 1);
        $scope.$on("$destroy", function () { refresh.stop(actualizar) });

        function cargarTransferencias() {
            dataFac.getTransferenciasPendientes($scope)
        }        

        $scope.ver = function (transferencia) {
            $scope.transferencia = transferencia
            $scope.comentario = null
            $scope.nuevaCantidad = null
        }

        $scope.guardarEgreso = function (id, nuevaCantidad, comentario) {

            var data = {
                idTransferencia: id,
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

            dataFac.despacharTransferencia(data, $scope)
        }

    }])
    .controller('historial', ["$state", function ($state) {
        $state.go("historial.recetas")
    }])
    .controller('historial.recetas', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.despachos = dataFac.recetasDespachadas;
        $scope.receta = null;

        $scope.$watch("despachos", function () {
            dataFac.recetasDespachadas = $scope.despachos
        })

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }
            dataFac.getDespachos($scope, data); 
        }

        $scope.select = function (receta) {
            $scope.receta = receta;
        }
    }])
    .controller('historial.transferencias', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.transferencias = dataFac.transferenciasDespachadas
        $scope.transferencia = null;

        $scope.$watch("transferencias", function () {
            dataFac.transferenciasDespachadas = $scope.transferencias
        })

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getTransferenciasDespachadas($scope, data)
        }

        $scope.ver = function (transferencia) {
            $scope.transferencia = transferencia
        }

    }])
    /*.controller('historial.pedidointerno', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {

    }])*/
    .controller('historial.ingresoItems', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.items = dataFac.itemsRegistrados
        $scope.item = null;

        $scope.$watch("items", function () {
            dataFac.itemsRegistrados = $scope.items
        })

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getItemsRegistrados($scope, data)
        }

        $scope.ver = function (item) {
            $scope.item = item
        }

    }])
    .controller('stock', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.stock = dataFac.stock;

        var actualizar = refresh.go(cargarStock, 1);
        $scope.$on("$destroy", function () { refresh.stop(actualizar) });

        function cargarStock() {
            dataFac.getStock($scope);
        }
    }])
    .controller('ingresar', ["$state", function ($state) {
        $state.go("ingresar.items")        
    }])
    .controller('ingresar.items', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.compuestos = dataFac.compuestos;
        $scope.items = null;

        if ($scope.compuestos === null) {
            dataFac.getCompuestos($scope);
        }

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

        var actualizar = refresh.go(cargarTransferencias, 1);
        $scope.$on("$destroy", function () { refresh.stop(actualizar) });

        function cargarTransferencias() {
            dataFac.getTransferenciasPorIngresar($scope);
        }

        $scope.ver = function (transferencia) {
            $scope.transferencia = transferencia;
        }

        $scope.guardarIngreso = function (comentario) {
            refresh.stop(actualizar)
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
                actualizar = refresh.go(cargarTransferencias, 1);
                notify("Transferencia ingresada exitosamente", "success");
                NProgress.done();
            }, function error(err) {
                refresh.go(cargarTransferencias, 1);
                console.log("Error ingresar transferencia", err);
                notify("No se pudo ingresar la transferencia", "danger");
                NProgress.done();
            })
        }
    }])