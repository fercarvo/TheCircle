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
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
        //$logProvider.debugEnabled(false); Activar en modo produccion
    }])
    .controller('despachar', ["$log", "$scope", "$state", "$http", "atencionFactory", "disable", function ($log, $scope, $state, $http, atencionFactory, disable) {
        $scope.casa = "dasdasdasd"


    }])
    .controller('historial', ["$log", "$scope", "$state", "$http", "atencionFactory", "disable", function ($log, $scope, $state, $http, atencionFactory, disable) {
        $scope.casa = "dasdasdasd"
    }])

    .run(["$state", function ($state) {
        $state.go("despachar");
    }])