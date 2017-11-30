var Marionette = require('marionette');
var Controller = require('./Controller');

module.exports = Marionette.AppRouter.extend({
    controller : new Controller(),
    appRoutes  : {
        'addbooks'                  : 'addBooks',
        'addbooks/:action(/:query)' : 'addBooks',
        'calendar'                   : 'calendar',
        'settings'                   : 'settings',
        'settings/:action(/:query)'  : 'settings',
        'wanted'                     : 'wanted',
        'wanted/:action'             : 'wanted',
        'history'                    : 'activity',
        'history/:action'            : 'activity',
        'activity'                   : 'activity',
        'activity/:action'           : 'activity',
        'rss'                        : 'rss',
        'system'                     : 'system',
        'system/:action'             : 'system',
        'seasonpass'                 : 'seasonPass',
        'serieseditor'               : 'seriesEditor',
        ':whatever'                  : 'showNotFound'
    }
});