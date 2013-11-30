module.exports = (function() {
    var next_uid = 0;
    var start_uid = 0;

    function isValid(uid) {
        return uid >= start_uid;
    }

    return function() {
        if (!isValid(next_uid)) {
            throw "UID pool depleted."; // will happen at uid >= Number.MAX_VALUE (in js: 1.7976931348623157e+308)
        }
        return next_uid++;
    };
})();