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
            console.log("Go refresh by ", time)
            return setInterval(fn, time)
        }
        return setInterval(fn, 10000);
    },
    stop: function (repeater) {
        clearInterval(repeater);
    }
}

//Notificaciones bootstrap
function notify(mensaje, tipo) {

    var icono = "";

    if (tipo === "success") {
        icono = "glyphicon glyphicon-saved";
    } else if (tipo === "danger") {
        icono = "glyphicon glyphicon-ban-circle"
    }

    return $.notify(
        {
            icon: icono,
            message: mensaje,
            url: '#',
            target: '_blank'
        }, {
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
        })
}

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

        refresh.go(function (){
            $http.get("login").then( ()=>{
                console.log("Session valida");
            }, (response)=>{
                if (response.status == 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('logout');
                }
            })
        }, 1000*60*20) //cada 20 minutos

        var name = $cookies.get('session_name')
        var email = $cookies.get('session_email')
        var photo = $cookies.get('session_photo')

        $rootScope.session_photo = "#"

        if (name) { $rootScope.session_name = name }
        if (email) { $rootScope.session_email = email }
        if (photo) { $rootScope.session_photo = photo }

        dataFac.getData()

        $state.go("despachar")
    }])
    .factory('dataFac', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFac = {
            stock: null,
            compuestos: null,
            categorias: null,
            unidades: null,
            getData: data,
            getStock: getStock
        }

        function getStock() {
            $http.get("/api/itemfarmacia/").then(function success(res) {
                console.log("Stock de bodega", res.data);
                dataFac.stock = res.data;
                $rootScope.$broadcast('dataFac.stock'); //Se informa a los controladores que cambio stock
            }, function error(err) {
                console.log("error cargar stock");
            })
        }

        function data() {
            $http.get("/api/compuesto-categoria-unidades").then( (res)=>{
                dataFac.compuestos = res.data.compuestos;
                dataFac.categorias = res.data.categorias;
                dataFac.unidades = res.data.unidades;

                $rootScope.$broadcast('compuesto-categoria-unidades');

            }, (error)=>{
                console.log("Error cargar data", err);
            })
        }

        return dataFac;
    }])
    .controller('despachar', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "dasdasdasd"


    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "dasdasdasd"
    }])
    .controller('stock', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
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

        if ($scope.categorias === null) { dataFac.getCategorias() }
        if ($scope.unidades === null) { dataFac.getUnidades() }

        $scope.$on('compuesto-categoria-unidades', ()=>{
            $scope.categorias = dataFac.categorias
            $scope.unidades = dataFac.unidades
        })

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