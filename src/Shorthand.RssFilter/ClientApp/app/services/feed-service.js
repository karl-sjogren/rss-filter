import Service from '@ember/service';
import fetch from 'fetch';

export default class FeedService extends Service {
  getFeeds() {
    return fetch('/api/feeds')
      .then(response => response.json());
  }

  getFeed(name) {
    return fetch('/api/fedds/' + encodeURIComponent(name))
      .then(response => response.json());
  }
}