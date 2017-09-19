/*
    sistema v1.0 
    Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
    Children International
*/

//Ejecuta una funcion cada cierto tiempo y detenerla cuando se requiera.
var refresh = {
    go: (fn, time)=>{
        fn();
        if (time) {
            return setInterval(fn, time)
        } return setInterval(fn, 10000)
    },
    stop: (repeater)=>{ clearInterval(repeater) }
}

/**
    * @name notify
    * @kind function
    * @description
    * Muestra las notificaciones de success o error en los req HTTP
    * de la aplicacion
    *
    * @param {string} mensaje Mensaje que será mostrado en la notificacion
    * @param {string} tipo success/danger, success o error respectivamente
    * @param {boolean} progress true for show progress bar.
    * @returns {promise} Promise de la notificacion generada
    * culmine por ejm
    * var not = notify.close();
    * not();
    */
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

angular.module('sistema', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('activarusuario', {
                templateUrl: 'views/sistema/activarusuario.html',
                controller: 'activarusuario'
            })
            .state('desactivarusuario', {
                templateUrl: 'views/sistema/desactivarusuario.html',
                controller: 'desactivarusuario'
            })
            .state('cambiarclave', {
                templateUrl: 'views/sistema/cambiarclave.html',
                controller: 'cambiarclave'
            });
        $compileProvider.debugInfoEnabled(true); //False en modo de produccion
    }])
    .run(["$state", "$rootScope", "$cookies", "$http", "$templateCache", function ($state, $rootScope, $cookies, $http, $templateCache) {

        refresh.go(function () {
            $http.get("login").then(() => { console.log("Session valida") }, (response) => {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('logout');
                }
            })
        }, 1000 * 6) //cada 20 minutos

        NProgress.start();
        $http.get('/views/sistema/activarusuario.html', { cache: $templateCache }).then(() => {
            $state.go("activarusuario")
            NProgress.done();
        })

        $http.get('/views/sistema/desactivarusuario.html', { cache: $templateCache });
        $http.get('/views/sistema/cambiarclave.html', { cache: $templateCache });


        var name = $cookies.get('session_name')
        var email = $cookies.get('session_email')
        var photo = $cookies.get('session_photo')

        $rootScope.session_photo = "#"

        if (name) { $rootScope.session_name = name }
        if (email) { $rootScope.session_email = email }
        if (photo) { $rootScope.session_photo = photo }
    }])
    .factory('usuarios', ['$http', '$rootScope', function ($http, $rootScope) {

        var usuarios = {
            inactivos: null,
            activos : null,
            all : null,
            getInactivos: getInactivos,
            getActivos: getActivos,
            getAll: getAll
        }

        //Se obtienen todos los usuarios inactivos que existen en el sistema
        function getInactivos() {
            $http.get("api/user/inactivos").then(function success(res) {
                console.log("usuarios.inactivos", res.data);
                usuarios.inactivos = res.data;
                $rootScope.$broadcast('usuarios.inactivos'); //Se informa a los controladores que cambio usuarios.inactivos
            }, function error(err) {
                console.log("error cargar usuarios.inactivos", err);
            })
        }

        //Se obtienen todos los usuarios activos que existen en el sistema
        function getActivos() {
            $http.get("api/user/activos").then(function success(res) {
                console.log("usuarios.activos", res.data);
                usuarios.activos = res.data;
                $rootScope.$broadcast('usuarios.activos'); //Se informa a los controladores que cambio usuarios.activos
            }, function error(err) {
                console.log("error cargar usuarios.activos", err);
            })
        }

        //Se obtienen todos los usuarios activos y no activos del sistema, excepto los que tienen
        //Como rol "sistema"
        function getAll() {
            $http.get("api/user").then(function success(res) {
                console.log("usuarios.all", res.data);
                usuarios.all = res.data;
                $rootScope.$broadcast('usuarios.all'); //Se informa a los controladores que cambio usuarios.all
            }, function error(err) {
                console.log("error cargar usuarios.all", err);
            })
        }

        return usuarios;
    }])
    .controller('activarusuario', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {

        $scope.usuarios = usuarios.inactivos;
        $scope.usuario = null;

        usuarios.getInactivos();

        $scope.$on('usuarios.inactivos', function () {
            $scope.usuarios = usuarios.inactivos;
        })

        $scope.$watch('usuarios', function () {
            usuarios.inactivos = $scope.usuarios;
        })

        $scope.seleccionar = function(usuario){
            $scope.usuario = usuario;
        }

        $scope.aceptar = function () {
            $('#modal_activar').modal('hide');
            NProgress.start();
            $http.put("api/user/" + $scope.usuario.cedula + "/activar").then(function success(res) {
                console.log("Activado con exito");
                $scope.usuarios = res.data;
                notify("Usuario activado exitosamente", "success");
                NProgress.done();
            }, function error(err) {
                console.log("error al activar");
                notify("No se ha podido activar el usuario", "danger");
                NProgress.done();
            })
        }

    }])
    .controller('desactivarusuario', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {
        $scope.usuarios = usuarios.activos;
        $scope.usuario = null;

        usuarios.getActivos();

        $scope.$on('usuarios.activos', ()=>{ $scope.usuarios = usuarios.activos })
        $scope.$watch('usuarios', ()=>{ usuarios.activos = $scope.usuarios })
        $scope.seleccionar = (usuario)=>{ $scope.usuario = usuario }

        $scope.aceptar = function () {
            $('#modal_desactivar').modal('hide');
            NProgress.start();
            $http.put("api/user/" + $scope.usuario.cedula + "/desactivar").then(function success(res) {
                console.log("desActivado con exito");
                $scope.usuarios = res.data;
                notify("Se ha bloqueado al usuario satisfactoriamente", "success");
                NProgress.done();
            }, function error(err) {
                console.log("error al desactivar");
                notify("No se ha podido activar el usuario", "danger");
                NProgress.done();
            })
        }
    }])
    .controller('cambiarclave', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {
        $scope.usuarios = usuarios.all;
        $scope.usuario = null;
        $scope.clave = null;

        usuarios.getAll();

        $scope.$on('usuarios.all', ()=>{ $scope.usuarios = usuarios.all })

        $scope.seleccionar = function (usuario) {
            $scope.usuario = usuario;
        }

        $scope.aceptar = function () {
            $('#cambiar_clave').modal('hide');
            NProgress.start();
            $http.put("api/user/" + $scope.usuario.cedula + "/clave/set").then(function success(res) {
                NProgress.done();
                console.log("Cambio de clave exitoso", res.data);
                $scope.clave = res.data.clave;
                $('#nueva_clave').modal('show');
                notify("Reseteo de clave exitoso", "success");
            }, function error(err) {
                console.log("error cambio de clave");
                NProgress.done();
                notify("No se ha podido cambiar la clave del usuario", "danger");
            })
        }
    }])