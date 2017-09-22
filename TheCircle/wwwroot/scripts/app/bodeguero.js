
angular.module('bodeguero', ['ui.router', 'ngCookies'])
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
    .run(["$state", "$rootScope", "$cookies", "$http", "dataFac", function ($state, $rootScope, $cookies, $http, dataFac) {

        refresh.go(function () {
            $http.get("login").then(function () { console.log("Session valida") }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('/login');
                }
            })
        }, 20) //minutos

        var promises = []
        var states = $state.get()
        NProgress.start()

        for (i = 1; i < states.length; i++) {
            var p = $http.get(states[i].templateUrl, { cache: $templateCache })
            promises.push(p)
            p.then(function () { }, function (error) { console.log("Error template: ", error) })
        }

        Promise.all(promises)
            .then(function () { }).catch(function () { }).then(function () {
                NProgress.done()
                $state.go("despachar") /////////////////////////
            })

        $rootScope.session_name = $cookies.get('session_name')
        $rootScope.session_email = $cookies.get('session_email')
        $rootScope.session_photo = $cookies.get('session_photo')
    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            categorias: null,
            transferencias: null,
            unidades: null,
            getData: data,
            getStock: getStock,
            getCompuestos: getCompuestos,
            getTransferencias: getTransferencias
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

        function getStock() {
            $http.get("/api/itemfarmacia/").then(function success(res) {
                console.log("Stock de bodega", res.data);
                dataFac.stock = res.data;
                $rootScope.$broadcast('dataFac.stock'); //Se informa a los controladores que cambio stock
            }, function error(err) {
                console.log("error cargar stock", err);
            })
        }

        function data() {
            $http.get("/api/compuesto-categoria-unidades").then( (res)=>{
                dataFac.compuestos = res.data.compuestos;
                dataFac.categorias = res.data.categorias;
                dataFac.unidades = res.data.unidades;
                $rootScope.$broadcast('compuesto-categoria-unidades');

            }, (error)=>{
                console.log("Error cargar data", error);
            })
        }

        function getCompuestos() {
            $http.get("/api/compuesto").then((res) => {
                dataFac.compuestos = res.data;
                $rootScope.$broadcast('dataFac.compuestos');
            }, (error) => {
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
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "dasdasdasd"
    }])
    .controller('stock', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.stock = dataFac.stock;
        var actualizar = refresh.go(cargar, 1);

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
    .controller('ingresar', ["$state", "$scope", "$http", "dataFac", function ($state, $scope, $http, dataFac) {
        console.log("En controller ingresar");
        $scope.compuestos = dataFac.getData();

        if ($scope.compuestos === null) { dataFac.getCompuestos() }

        $scope.$on('compuesto-categoria-unidades', ()=>{ 
            $scope.compuestos = dataFac.compuestos;
        })

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

        /*
        if ($scope.categorias === null || $scope.unidades === null) { dataFac.getData() }
        if ($scope.compuestos === null) { dataFac.getCompuestos() }
        */
        dataFac.getData()

        $scope.$on('compuesto-categoria-unidades', ()=>{
            $scope.categorias = dataFac.categorias
            $scope.unidades = dataFac.unidades
            $scope.compuestos = dataFac.compuestos
        })

        //$scope.$on('dataFac.compuestos', ()=>{ $scope.compuestos = dataFac.compuestos })

        $scope.crear =  (form)=>{
            var data = {
                nombre: form.nombre,
                categoria: form.categoria,
                unidad: form.unidad
            }
            console.log(form, data);

            $http.post("api/compuesto", data).then(function sucess(res) {
                console.log("Ingreso exitoso", res);
                notify("Ingreso exitoso de compuesto", "success");
                $state.reload();
            }, function error(e) {
                console.log("No se pudo guardar el compuesto", e)
                notify("No se ha podido guardar el compuesto", "danger");
            })
        }

        $scope.reset = ()=>{ $state.reload() }
    }])