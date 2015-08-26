'use strict';
var varables = require('./Varables.js');

module.exports = function (hoistEvent) {
    var slack = global.Hoist.connector(varables.SlackConnectorKey);
    return slack.get('/chat.postMessage', {
        channel: varables.SlackChannel,
        text: JSON.stringify(hoistEvent.payload),
        username: 'hoist-bot'
    })
   .then(function (result) {
       return Hoist.event.raise('slack:message:sent', result);
   });
};