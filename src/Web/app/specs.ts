import './vendor';
import './main';
import 'angular-mocks';

describe('budget-buddy', () => {
    beforeEach(angular.mock.module('budget-buddy'));

    const context = (<any>require).context('.', true, /\.spec\.ts$/);
    context.keys().forEach(context);
})