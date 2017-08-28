angular.module('appBodeguero', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", "$logProvider", function ($stateProvider, $compileProvider, $logProvider) {
        $stateProvider
            .state('despachar', {
                templateUrl: 'views/bodeguero/despachar.html',
                controller: 'despachar'
            })
            .state('historial', {
                templateUrl: 'views/bodeguero/historial.html',
                controller: 'historial'
            })
            .state('ingresar', {
                templateUrl: 'views/bodeguero/ingresar.html',
                controller: 'ingresar'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
        //$logProvider.debugEnabled(false); Activar en modo produccion
    }])
    .run(["$state", function ($state) {
        $state.go("despachar");
    }])
    .controller('despachar', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $scope.casa = "dasdasdasd"


    }])
    .controller('historial', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $scope.casa = "dasdasdasd"
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