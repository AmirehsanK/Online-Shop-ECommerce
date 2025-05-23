/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, (global.FormValidation = global.FormValidation || {}, global.FormValidation.plugins = global.FormValidation.plugins || {}, global.FormValidation.plugins.Recaptcha = factory()));
})(this, (function () {
    'use strict';

    function _classCallCheck(instance, Constructor) {
        if (!(instance instanceof Constructor)) {
            throw new TypeError("Cannot call a class as a function");
        }
    }

    function _defineProperties(target, props) {
        for (var i = 0; i < props.length; i++) {
            var descriptor = props[i];
            descriptor.enumerable = descriptor.enumerable || false;
            descriptor.configurable = true;
            if ("value" in descriptor) descriptor.writable = true;
            Object.defineProperty(target, descriptor.key, descriptor);
        }
    }

    function _createClass(Constructor, protoProps, staticProps) {
        if (protoProps) _defineProperties(Constructor.prototype, protoProps);
        if (staticProps) _defineProperties(Constructor, staticProps);
        Object.defineProperty(Constructor, "prototype", {
            writable: false
        });
        return Constructor;
    }

    function _defineProperty(obj, key, value) {
        if (key in obj) {
            Object.defineProperty(obj, key, {
                value: value,
                enumerable: true,
                configurable: true,
                writable: true
            });
        } else {
            obj[key] = value;
        }

        return obj;
    }

    function _inherits(subClass, superClass) {
        if (typeof superClass !== "function" && superClass !== null) {
            throw new TypeError("Super expression must either be null or a function");
        }

        subClass.prototype = Object.create(superClass && superClass.prototype, {
            constructor: {
                value: subClass,
                writable: true,
                configurable: true
            }
        });
        Object.defineProperty(subClass, "prototype", {
            writable: false
        });
        if (superClass) _setPrototypeOf(subClass, superClass);
    }

    function _getPrototypeOf(o) {
        _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf.bind() : function _getPrototypeOf(o) {
            return o.__proto__ || Object.getPrototypeOf(o);
        };
        return _getPrototypeOf(o);
    }

    function _setPrototypeOf(o, p) {
        _setPrototypeOf = Object.setPrototypeOf ? Object.setPrototypeOf.bind() : function _setPrototypeOf(o, p) {
            o.__proto__ = p;
            return o;
        };
        return _setPrototypeOf(o, p);
    }

    function _isNativeReflectConstruct() {
        if (typeof Reflect === "undefined" || !Reflect.construct) return false;
        if (Reflect.construct.sham) return false;
        if (typeof Proxy === "function") return true;

        try {
            Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {
            }));
            return true;
        } catch (e) {
            return false;
        }
    }

    function _assertThisInitialized(self) {
        if (self === void 0) {
            throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
        }

        return self;
    }

    function _possibleConstructorReturn(self, call) {
        if (call && (typeof call === "object" || typeof call === "function")) {
            return call;
        } else if (call !== void 0) {
            throw new TypeError("Derived constructors may only return object or undefined");
        }

        return _assertThisInitialized(self);
    }

    function _createSuper(Derived) {
        var hasNativeReflectConstruct = _isNativeReflectConstruct();

        return function _createSuperInternal() {
            var Super = _getPrototypeOf(Derived),
                result;

            if (hasNativeReflectConstruct) {
                var NewTarget = _getPrototypeOf(this).constructor;

                result = Reflect.construct(Super, arguments, NewTarget);
            } else {
                result = Super.apply(this, arguments);
            }

            return _possibleConstructorReturn(this, result);
        };
    }

    var e = FormValidation.Plugin;

    var t = FormValidation.utils.fetch;

    var i = /*#__PURE__*/function (_e) {
        _inherits(i, _e);

        var _super = _createSuper(i);

        function i(e) {
            var _this;

            _classCallCheck(this, i);

            _this = _super.call(this, e);
            _this.widgetIds = new Map();
            _this.captchaStatus = "NotValidated";
            _this.opts = Object.assign({}, i.DEFAULT_OPTIONS, e);
            _this.fieldResetHandler = _this.onResetField.bind(_assertThisInitialized(_this));
            _this.preValidateFilter = _this.preValidate.bind(_assertThisInitialized(_this));
            _this.iconPlacedHandler = _this.onIconPlaced.bind(_assertThisInitialized(_this));
            return _this;
        }

        _createClass(i, [{
            key: "install",
            value: function install() {
                var _this2 = this;

                this.core.on("core.field.reset", this.fieldResetHandler).on("plugins.icon.placed", this.iconPlacedHandler).registerFilter("validate-pre", this.preValidateFilter);
                var e = typeof window[i.LOADED_CALLBACK] === "undefined" ? function () {
                } : window[i.LOADED_CALLBACK];

                window[i.LOADED_CALLBACK] = function () {
                    e();
                    var s = {
                        badge: _this2.opts.badge,
                        callback: function callback() {
                            if (_this2.opts.backendVerificationUrl === "") {
                                _this2.captchaStatus = "Valid";

                                _this2.core.updateFieldStatus(i.CAPTCHA_FIELD, "Valid");
                            }
                        },
                        "error-callback": function errorCallback() {
                            _this2.captchaStatus = "Invalid";

                            _this2.core.updateFieldStatus(i.CAPTCHA_FIELD, "Invalid");
                        },
                        "expired-callback": function expiredCallback() {
                            _this2.captchaStatus = "NotValidated";

                            _this2.core.updateFieldStatus(i.CAPTCHA_FIELD, "NotValidated");
                        },
                        sitekey: _this2.opts.siteKey,
                        size: _this2.opts.size
                    };
                    var a = window["grecaptcha"].render(_this2.opts.element, s);

                    _this2.widgetIds.set(_this2.opts.element, a);

                    _this2.core.addField(i.CAPTCHA_FIELD, {
                        validators: {
                            promise: {
                                message: _this2.opts.message,
                                promise: function promise(e) {
                                    var s = _this2.widgetIds.has(_this2.opts.element) ? window["grecaptcha"].getResponse(_this2.widgetIds.get(_this2.opts.element)) : e.value;

                                    if (s === "") {
                                        _this2.captchaStatus = "Invalid";
                                        return Promise.resolve({
                                            valid: false
                                        });
                                    } else if (_this2.opts.backendVerificationUrl === "") {
                                        _this2.captchaStatus = "Valid";
                                        return Promise.resolve({
                                            valid: true
                                        });
                                    } else if (_this2.captchaStatus === "Valid") {
                                        return Promise.resolve({
                                            valid: true
                                        });
                                    } else {
                                        return t(_this2.opts.backendVerificationUrl, {
                                            method: "POST",
                                            params: _defineProperty({}, i.CAPTCHA_FIELD, s)
                                        }).then(function (e) {
                                            var t = "".concat(e["success"]) === "true";
                                            _this2.captchaStatus = t ? "Valid" : "Invalid";
                                            return Promise.resolve({
                                                meta: e,
                                                valid: t
                                            });
                                        })["catch"](function (e) {
                                            _this2.captchaStatus = "NotValidated";
                                            return Promise.reject({
                                                valid: false
                                            });
                                        });
                                    }
                                }
                            }
                        }
                    });
                };

                var s = this.getScript();

                if (!document.body.querySelector("script[src=\"".concat(s, "\"]"))) {
                    var _e2 = document.createElement("script");

                    _e2.type = "text/javascript";
                    _e2.async = true;
                    _e2.defer = true;
                    _e2.src = s;
                    document.body.appendChild(_e2);
                }
            }
        }, {
            key: "uninstall",
            value: function uninstall() {
                if (this.timer) {
                    clearTimeout(this.timer);
                }

                this.core.off("core.field.reset", this.fieldResetHandler).off("plugins.icon.placed", this.iconPlacedHandler).deregisterFilter("validate-pre", this.preValidateFilter);
                this.widgetIds.clear();
                var e = this.getScript();
                var t = [].slice.call(document.body.querySelectorAll("script[src=\"".concat(e, "\"]")));
                t.forEach(function (e) {
                    return e.parentNode.removeChild(e);
                });
                this.core.removeField(i.CAPTCHA_FIELD);
            }
        }, {
            key: "getScript",
            value: function getScript() {
                var e = this.opts.language ? "&hl=".concat(this.opts.language) : "";
                return "https://www.google.com/recaptcha/api.js?onload=".concat(i.LOADED_CALLBACK, "&render=explicit").concat(e);
            }
        }, {
            key: "preValidate",
            value: function preValidate() {
                var _this3 = this;

                if (this.opts.size === "invisible" && this.widgetIds.has(this.opts.element)) {
                    var _e3 = this.widgetIds.get(this.opts.element);

                    return this.captchaStatus === "Valid" ? Promise.resolve() : new Promise(function (t, _i) {
                        window["grecaptcha"].execute(_e3).then(function () {
                            if (_this3.timer) {
                                clearTimeout(_this3.timer);
                            }

                            _this3.timer = window.setTimeout(t, 1 * 1e3);
                        });
                    });
                } else {
                    return Promise.resolve();
                }
            }
        }, {
            key: "onResetField",
            value: function onResetField(e) {
                if (e.field === i.CAPTCHA_FIELD && this.widgetIds.has(this.opts.element)) {
                    var _e4 = this.widgetIds.get(this.opts.element);

                    window["grecaptcha"].reset(_e4);
                }
            }
        }, {
            key: "onIconPlaced",
            value: function onIconPlaced(e) {
                if (e.field === i.CAPTCHA_FIELD) {
                    if (this.opts.size === "invisible") {
                        e.iconElement.style.display = "none";
                    } else {
                        var _t = document.getElementById(this.opts.element);

                        if (_t) {
                            _t.parentNode.insertBefore(e.iconElement, _t.nextSibling);
                        }
                    }
                }
            }
        }]);

        return i;
    }(e);
    i.CAPTCHA_FIELD = "g-recaptcha-response";
    i.DEFAULT_OPTIONS = {
        backendVerificationUrl: "",
        badge: "bottomright",
        size: "normal",
        theme: "light"
    };
    i.LOADED_CALLBACK = "___reCaptchaLoaded___";

    return i;

}));
