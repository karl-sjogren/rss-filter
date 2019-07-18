import Route from '@ember/routing/route';
import { inject } from '@ember/service';

export default Route.extend({
  intl: inject(),

  beforeModel() {
    this._super(...arguments)
    return this.intl.setLocale(['en-us']);
  }
});