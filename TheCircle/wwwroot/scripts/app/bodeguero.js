
angular.module('bodeguero', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('despachar', {
                templateUrl: 'views/bodeguero/despachar.html',
                controller: 'despachar'
            })
            .state('historial', {
                templateUrl: 'views/bodeguero/historial.html',
                controller: 'historial'
            })
            .state('historial.transferencias', {
                templateUrl: 'views/bodeguero/historial.transferencias.html',
                controller: 'historial.transferencias'
            })
            .state('historial.ingresoItems', {
                templateUrl: 'views/bodeguero/historial.ingresoItems.html',
                controller: 'historial.ingresoItems'
            })
            .state('stock', {
                templateUrl: 'views/bodeguero/stock.html',
                controller: 'stock'
            })
            .state('ingresar', {
                templateUrl: 'views/bodeguero/ingresar.html',
                controller: 'ingresar'
            })
            .state('compuesto', {
                templateUrl: 'views/bodeguero/compuesto.html',
                controller: 'compuesto'
            });
        $compileProvider.debugInfoEnabled(true); //false en modo de produccion
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);

        loadTemplates($state, "despachar", $http, $templateCache);
    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            categorias: null,
            transferencias: null,
            unidades: null,
            nombres: null,
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
            getData: getData,
            getStock: getStock,
            getCompuestos: getCompuestos,
            getTransferencias: getTransferencias
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
                NProgress.done();
                $scope.transferencias.data = res.data;
            }, function (err) {
                console.log("error getTransferenciasDespachadas", err)
                notify("No se pudo cargar las trasnferencias despachadas", "danger");
                NProgress.done();
            })
        }

        function getTransferencias() {
            $http.get("/api/transferencia").then(function (res) {
                console.log("Transferencias a despachar", res.data);
                dataFac.transferencias = res.data;
                $rootScope.$broadcast('dataFac.transferencias');
            }, function (err) {
                console.log("error cargar stock", err);
            })
        }

        function getStock($scope) {
            $http.get("/api/itemfarmacia/").then(function (res) {
                console.log("Stock de bodega", res.data);
                dataFac.stock = res.data;
                $scope.stock = dataFac.stock;
            }, function (error) {
                console.log("error getStock", error)
            })
        }

        function getData() {
            $http.get("/api/compuesto-categoria-unidades").then( (res)=>{
                dataFac.compuestos = res.data.compuestos;
                dataFac.categorias = res.data.categorias;
                dataFac.unidades = res.data.unidades;
                $rootScope.$broadcast('compuesto-categoria-unidades');

            }, function(error){
                console.log("Error cargar data", error);
            })
        }

        function getCompuestos() {
            $http.get("/api/compuesto").then((res) => {
                dataFac.compuestos = res.data;
                $rootScope.$broadcast('dataFac.compuestos');
            }, function(error) {
                console.log("Error cargar compuestos", error);
            })
        }

        return dataFac;
    }])
    .controller('despachar', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.transferencias = dataFac.transferencias;
        $scope.transferencia = null;

        var actualizar = refresh.go(cargar, 1);

        function cargar() {
            if ($state.includes('despachar')) {
                dataFac.getTransferencias();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.$on('dataFac.transferencias', function () {
            $scope.transferencias = dataFac.transferencias
        })

        $scope.ver = function (transferencia) {
            $scope.transferencia = transferencia
            $scope.comentario = null;
        }

        $scope.guardarEgreso = function (id, cantidad, comentario) {
            var data = {
                idTransferencia: id,
                cantidad: cantidad,
                comentario: (function () {
                    if (comentario) {
                        return comentario
                    } return ""
                })()
            }

            refresh.stop(actualizar)
            NProgress.start()

            $http.put("/api/transferencia/" + id + "/despachar", data).then(function (res) {
                console.log("Se despacho la transferencia", res.data);
                notify("Transferencia ingresada exitosamente", "success");

            }, function (err) {
                console.log("Error ingresar transferencia", err);
                notify("No se pudo ingresar la transferencia", "danger");

            }).finally(function () {
                NProgress.done();
                $('#ver_transferencia').modal('hide');
                actualizar = refresh.go(cargar, 1)
            })
        }
    }])
    .controller('historial', ["$state", function ($state) {
        $state.go("historial.transferencias")
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
            dataFac.getStock($scope)
        }
    }])
    .controller('ingresar', ["$state", "$scope", "$http", "dataFac", function ($state, $scope, $http, dataFac) {
        dataFac.getData();
        $scope.compuestos = dataFac.compuestos;
        $scope.nombres = dataFac.nombres;

        if ($scope.compuestos === null) { dataFac.getCompuestos() }

        $scope.$on('compuesto-categoria-unidades', function() { 
            $scope.compuestos = dataFac.compuestos;
        })

        $scope.seleccionar = function (obj) {
            $scope.form.nombre = obj
        }

        $http.get("/api/itemfarmacia/nombre").then(function (res) {
            dataFac.nombres = res.data
            $scope.nombres = dataFac.nombres
        }, function () { })

        $scope.crear = function (form) {
            var data = {
                nombre: form.nombre,
                compuesto: form.compuesto,
                fcaducidad: date(form.fecha),
                cantidad: form.cantidad
            }
            console.log("data a enviar", data);

            $http.post("api/itemfarmacia", data).then(function sucess(res) {
                console.log("Ingreso exitoso", res.data);
                notify("Ingreso exitoso", "success");
                $state.reload();
            }, function err(err) {
                console.log("No se pudo guardar", err)
                notify("No se ha podido guardar el ingreso", "danger");
            })
        }
    }])
    .controller('compuesto', ["$state", "$scope", "$http", "dataFac", function ($state, $scope, $http, dataFac) {

        $scope.categorias = dataFac.categorias;
        $scope.unidades = dataFac.unidades;
        $scope.compuestos = dataFac.compuestos;

        dataFac.getData()

        $scope.$on('compuesto-categoria-unidades', function(){
            $scope.categorias = dataFac.categorias
            $scope.unidades = dataFac.unidades
            $scope.compuestos = dataFac.compuestos
        })

        $scope.crear =  function(form){
            var data = {
                nombre: form.nombre,
                categoria: form.categoria,
                unidad: form.unidad
            }
            console.log(form, data);

            $http.post("/api/compuesto", data).then(function (res) {
                console.log("Ingreso exitoso", res);
                notify("Ingreso exitoso de compuesto", "success");
                $state.reload();
            }, function (error) {
                console.log("No se pudo guardar el compuesto", error)
                notify("No se ha podido guardar el compuesto", "danger");
            })
        }

        $scope.reset = function(){ $state.reload() }
    }])