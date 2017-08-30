angular.module('appSistema', ['ui.router'])
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
            .state('modificarusuario', {
                templateUrl: 'views/sistema/modificarusuario.html',
                controller: 'modificarusuario'
            });
        //$compileProvider.debugInfoEnabled(false); //Activar en modo producción
    }])
    .run(["$state", function ($state) {
        $state.go("activarusuario");
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

        function getInactivos() {
            $http.get("api/user/inactivos/").then(function success(res) {
                console.log("usuarios.inactivos", res.data);
                usuarios.inactivos = res.data;
                $rootScope.$broadcast('usuarios.inactivos'); //Se informa a los controladores que cambio usuarios.inactivos
            }, function error(err) {
                console.log("error cargar usuarios.inactivos", err);
            })
        }

        function getActivos() {
            $http.get("api/user/activos/").then(function success(res) {
                console.log("usuarios.activos", res.data);
                usuarios.activos = res.data;
                $rootScope.$broadcast('usuarios.activos'); //Se informa a los controladores que cambio usuarios.activos
            }, function error(err) {
                console.log("error cargar usuarios.activos", err);
            })
        }

        function getAll() {
            $http.get("api/user/").then(function success(res) {
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

        $scope.seleccionar = function(usuario){
            $scope.usuario = usuario;
        }

        $scope.aceptar = function() {
            $http.put("api/user/" + usuario.cedula + "/activar").then(function success(res) {
                console.log("Activado con exito");
                usuarios.getInactivos();
            }, function error(err) {
                console.log("error al activar");
            })
        }

    }])
    .controller('desactivarusuario', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {
        $scope.usuarios = usuarios.activos;
        $scope.usuario = null;

        usuarios.getActivos();

        $scope.$on('usuarios.activos', function () {
            $scope.usuarios = usuarios.activos;
        })

        $scope.seleccionar = function(usuario){
            $scope.usuario = usuario;
        }

        $scope.aceptar = function() {
            $http.put("api/user/" + usuario.cedula + "/desactivar").then(function success(res) {
                console.log("desActivado con exito");
                usuarios.getActivos();
            }, function error(err) {
                console.log("error al desactivar");
            })
        }
    }])
    .controller('modificarusuario', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "casacasacasa?";
    }])