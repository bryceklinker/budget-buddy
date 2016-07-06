import { Config, ConfigService } from './';

describe('ConfigService', () => {
    let $httpBackend: angular.IHttpBackendService;
    let configService: ConfigService;

    beforeEach(angular.mock.inject((_$httpBackend_, _ConfigService_) => {
        $httpBackend = _$httpBackend_;
        configService = _ConfigService_;
    }))

    it('should get config from server', (done) => {
        const expected = { budgetApiUrl: 'http://other-server.com/api'};
        $httpBackend.expectGET('/config.json')
            .respond(expected);

        configService.getConfig().then(actual => {
            expect(actual).toEqual(expected);
            done();
        });
        $httpBackend.flush();
    })
})