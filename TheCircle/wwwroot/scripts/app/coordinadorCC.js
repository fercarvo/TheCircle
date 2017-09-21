/*
 appCoordinador v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/

//retorna la fecha en un formato especifico
function date(date) {
    var format = new Date(date);
    var day = format.getDate();
    var month = format.getMonth() + 1;
    var year = format.getFullYear();

    return day + '/' + month + '/' + year;
}

//Ejecuta una funcion cada cierto tiempo y detenerla cuando se requiera.
var refresh = {
    go: function (fn, time) {
        fn();
        if (time) {
            console.log("Go refresh in ", fn.name, "by", time, "sec");
            return setInterval(fn, time * 1000);
        }
        console.log("Go refresh in", fn.name, "by", 1000 * 5, "sec");
        return setInterval(fn, 1000 * 5);
    },
    stop: function (repeater) {
        console.log("stop repeater")
        clearInterval(repeater);
    }
}

function notify(mensaje, tipo, progress) {
    return $.notify(
        {
            icon: (() => {
                switch (tipo) {
                    case "success":
                        return "glyphicon glyphicon-saved"
                    case "danger":
                        return "glyphicon glyphicon-ban-circle"
                    default:
                        return ""
                }
            })(),
            message: mensaje,
            url: '#',
            target: '_blank'
        }, {
            element: 'body',
            position: null,
            showProgressbar: (() => {
                if (progress) {
                    return progress
                } return false
            })(),
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
        })
}

angular.module('coordinadorCC', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('recetas', {
                templateUrl: 'views/coordinadorCC/recetas.html',
                controller: 'recetas'
            })
            .state('egresos', {
                templateUrl: 'views/coordinadorCC/egresos.html',
                controller: 'egresos'
            });
        $compileProvider.debugInfoEnabled(true); //Activar en modo producción
    }])
    .run(["$state", "$rootScope", "$cookies", "$http", function ($state, $rootScope, $cookies, $http) {

        refresh.go(function () {
            $http.get("login").then(function () { console.log("Session valida") }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('/login');
                }
            })
        }, 60 * 20) //segundos

        $rootScope.session_name = $cookies.get('session_name')
        $rootScope.session_email = $cookies.get('session_email')
        $rootScope.session_photo = $cookies.get('session_photo')        

        $state.go("recetas");
    }])
    .factory('dataFac', ['$http', function ($http) {
        var dataFactory = {};

        dataFactory.stock = null;
        dataFactory.recetas = null;
        dataFactory.localidad = "CC2";

        return dataFactory;
    }])
    .controller('recetas', ["$scope", "$state", "$http", function ($scope, $state, $http) {

        $scope.recetas = null;

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/receta/localidad/fecha",
                params: data
            }).then(function (res) {
                NProgress.done();
                console.log(res)
                $scope.recetas = res.data;
            }, function (err) {
                console.log("error cargar recetas")
                notify("No se pudo cargar recetas", "danger");
                NProgress.done();
            })
        }

    }])
    .controller('egresos', ["$scope", "$state", "$http", function ($scope, $state, $http) {

        $scope.egresos = null;

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/itemfarmacia/report/egresos",
                params: data
            }).then(function (res) {
                NProgress.done();
                console.log(res.data)
                $scope.egresos = res.data;
            }, function (err) {
                console.log("error cargar recetas")
                notify("No se pudo cargar recetas", "danger");
                NProgress.done();
            })
        }

    }])