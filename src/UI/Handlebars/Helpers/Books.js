var Handlebars = require('handlebars');
var StatusModel = require('../../System/StatusModel');
var _ = require('underscore');

Handlebars.registerHelper('poster', function() {
    var placeholder = StatusModel.get('urlBase') + '/Content/Images/poster-dark.png';

    var img = this.remoteImageSmall;
    if(img)
    {
        return new Handlebars.SafeString('<img class="series-poster x-series-poster" src="{0}">'.format(img));
    }

    return new Handlebars.SafeString('<img class="series-poster placeholder-image" src="{0}">'.format(placeholder));
});

Handlebars.registerHelper('authors', function() {
    var authors = "";
    this.authors.forEach(function(v){
        authors += ", " + v;
    });
    return authors.substring(2);
});

Handlebars.registerHelper('googleUrl', function() {
    return "";
});