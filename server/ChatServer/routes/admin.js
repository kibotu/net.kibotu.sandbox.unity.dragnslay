
/*
 * GET admin console listing.
 */

exports.list = function(req, res){
    res.render('admin', { title: 'Admin Console' });
};