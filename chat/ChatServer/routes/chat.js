
/*
 * GET chat listing.
 */

exports.list = function(req, res){
    res.render('chat', { title: 'Websockets chat' });
};