var Handlebars = require('handlebars');
var StatusModel = require('../../System/StatusModel');
var _ = require('underscore');

Handlebars.registerHelper('route', function() {
    return StatusModel.get('urlBase') + '/series/' + this.titleSlug;
});

Handlebars.registerHelper('percentOfEpisodes', function() {
    var episodeCount = this.episodeCount;
    var episodeFileCount = this.episodeFileCount;

    var percent = 100;

    if (episodeCount > 0) {
        percent = episodeFileCount / episodeCount * 100;
    }

    return percent;
});

Handlebars.registerHelper('seasonCountHelper', function() {
    var seasonCount = this.seasonCount;
    var continuing = this.status === 'continuing';

    if (continuing) {
        return new Handlebars.SafeString('<span class="label label-info">Season {0}</span>'.format(seasonCount));
    }

    if (seasonCount === 1) {
        return new Handlebars.SafeString('<span class="label label-info">{0} Season</span>'.format(seasonCount));
    }

    return new Handlebars.SafeString('<span class="label label-info">{0} Seasons</span>'.format(seasonCount));
});