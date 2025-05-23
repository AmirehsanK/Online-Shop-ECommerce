/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (o) {
    'use strict';

    function _interopDefaultLegacy(e) {
        return e && typeof e === 'object' && 'default' in e ? e : {'default': e};
    }

    var o__default = /*#__PURE__*/_interopDefaultLegacy(o);

    function _typeof(obj) {
        "@babel/helpers - typeof";

        return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (obj) {
            return typeof obj;
        } : function (obj) {
            return obj && "function" == typeof Symbol && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj;
        }, _typeof(obj);
    }

    var t = FormValidation.formValidation;

    var r = o__default["default"].fn.jquery.split(" ")[0].split(".");

    if (+r[0] < 2 && +r[1] < 9 || +r[0] === 1 && +r[1] === 9 && +r[2] < 1) {
        throw new Error("The J plugin requires jQuery version 1.9.1 or higher");
    }

    o__default["default"].fn["formValidation"] = function (r) {
        var i = arguments;
        return this.each(function () {
            var e = o__default["default"](this);
            var n = e.data("formValidation");
            var a = "object" === _typeof(r) && r;

            if (!n) {
                n = t(this, a);
                e.data("formValidation", n).data("FormValidation", n);
            }

            if ("string" === typeof r) {
                n[r].apply(n, Array.prototype.slice.call(i, 1));
            }
        });
    };

})(jQuery);
