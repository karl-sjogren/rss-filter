import Route from '@ember/routing/route';
import { inject } from '@ember/service';

export default Route.extend({
  feedService: inject(),

  model(params) {
    return this.feedService.getFeed(params.name);
  }
});