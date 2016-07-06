import { Config } from './';

export class ConfigService {

    constructor(private $http: angular.IHttpService) {

    }

    getConfig(): angular.IPromise<Config> {
        return this.$http.get<Config>('/config.json')
            .then(res => res.data)
    }
}
ConfigService.$inject = ['$http'];
angular.module('budget-buddy')
    .service('ConfigService', ConfigService);