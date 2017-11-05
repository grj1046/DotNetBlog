(function () {
    window.rsaEncrypt = function (value) {
        var encrypt = new JSEncrypt({ log: true });
        encrypt.setPublicKey('-----BEGIN PUBLIC KEY-----\
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDlOJu6TyygqxfWT7eLtGDwajtN\
FOb9I5XRb6khyfD1Yt3YiCgQWMNW649887VGJiGr/L5i2osbl8C9+WJTeucF+S76\
xFxdU6jE0NQ+Z+zEdhUTooNRaY5nZiu5PgDB0ED/ZKBUSLKL7eibMxZtMlUDHjm4\
gwQco1KRMDSmXSMkDwIDAQAB\
-----END PUBLIC KEY-----');
        //var pwdMD5Twice = $.md5($.md5($("#Input_Password").attr("value")));
        var pass = value;
        var data = encrypt.encrypt(pass);
        return data;
    };
})();
