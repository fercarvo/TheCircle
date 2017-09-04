angular.module('appBodeguero', ['ui.router'])
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
            .state('ingresar', {
                templateUrl: 'views/bodeguero/ingresar.html',
                controller: 'ingresar'
            })
            .state('compuesto', {
                templateUrl: 'views/bodeguero/compuesto.html',
                controller: 'compuesto'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
    }])
    .run(["$state", function ($state) {
        $state.go("despachar");
    }])
    .factory('notify', [function () {
        return function (mensaje, tipo) {
            var icono = "";

            if (tipo === "success") {
                icono = "glyphicon glyphicon-saved";
            } else if (tipo === "danger") {
                icono = "glyphicon glyphicon-ban-circle"
            }

            return $.notify(
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
            var format = new Date(date);
            var day = format.getDate();
            var month = format.getMonth() + 1;
            var year = format.getFullYear();

            return day + '/' + month + '/' + year;
        }
    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            getCompuestos: getCompuestos
        }

        function getCompuestos() {
            $http.get("/api/compuesto").then(function success(res) {
                dataFac.compuestos = res.data;
                $rootScope.$broadcast('dataFac.compuestos');
            }, function error(err) {
                console.log("Error cargar compuestos", err);
            })
        }

        return dataFac;
    }])
    .controller('despachar', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "dasdasdasd"


    }])
    .controller('historial', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $scope.casa = "dasdasdasd"
    }])
    .controller('ingresar', ["$state", "$scope", "$http", "dataFac", "notify", "date", function ($state, $scope, $http, dataFac, notify, date) {
        console.log("En controller ingresar");
        $scope.compuestos = dataFac.compuestos;

        if ($scope.compuestos === null) {
            dataFac.getCompuestos();
        }

        $scope.$on('dataFac.compuestos', function () {
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