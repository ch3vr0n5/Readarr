var Backbone = require('backbone');
var BookGroupModel = require('../Book/BookGroupModel');
var _ = require('underscore');

module.exports = Backbone.Collection.extend({
    url   : window.NzbDrone.ApiRoot + '/author/lookup',
    model : BookGroupModel,

    parse : function(response) {
        var self = this;

        _.each(response, function(model) {
            model.id = undefined;

            if (self.unmappedFolderModel) {
                model.path = self.unmappedFolderModel.get('folder').path;
            }
        });

        return response;
    }
});