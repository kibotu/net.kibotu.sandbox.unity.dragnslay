
/*
 * GET home page.
 */

exports.index = function(req, res){
    // res.render('index', { title: 'Server' });
    res.writeHead(302, {
        'Location': 'http://www.kibotu.net'
    });
    res.end();
};