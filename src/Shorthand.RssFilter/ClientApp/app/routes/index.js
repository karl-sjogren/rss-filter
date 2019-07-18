import Route from '@ember/routing/route';
import { inject } from '@ember/service';

export default Route.extend({
  feedService: inject(),

  model() {
    return this.feedService.getFeeds();
  },

  setupController(controller, model) {
    controller.set('feeds', model);
  }
});