/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, (global.FormValidation = global.FormValidation || {}, global.FormValidation.plugins = global.FormValidation.plugins || {}, global.FormValidation.plugins.Recaptcha3Token = factory()));
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

    var t = /*#__PURE__*/function (_e) {
        _inherits(t, _e);

        var _super = _createSuper(t);

        function t(e) {
            var _this;

            _classCallCheck(this, t);

            _this = _super.call(this, e);
            _this.opts = Object.assign({}, {
                action: "submit",
                hiddenTokenName: "___hidden-token___"
            }, e);
            _this.onValidHandler = _this.onFormValid.bind(_assertThisInitialized(_this));
            return _this;
        }

        _createClass(t, [{
            key: "install",
            value: function install() {
                this.core.on("core.form.valid", this.onValidHandler);
                this.hiddenTokenEle = document.createElement("input");
                this.hiddenTokenEle.setAttribute("type", "hidden");
                this.core.getFormElement().appendChild(this.hiddenTokenEle);
                var e = typeof window[t.LOADED_CALLBACK] === "undefined" ? function () {
                } : window[t.LOADED_CALLBACK];

                window[t.LOADED_CALLBACK] = function () {
                    e();
                };

                var o = this.getScript();

                if (!document.body.querySelector("script[src=\"".concat(o, "\"]"))) {
                    var _e2 = document.createElement("script");

                    _e2.type = "text/javascript";
                    _e2.async = true;
                    _e2.defer = true;
                    _e2.src = o;
                    document.body.appendChild(_e2);
                }
            }
        }, {
            key: "uninstall",
            value: function uninstall() {
                this.core.off("core.form.valid", this.onValidHandler);
                var e = this.getScript();

                var _t = [].slice.call(document.body.querySelectorAll("script[src=\"".concat(e, "\"]")));

                _t.forEach(function (e) {
                    return e.parentNode.removeChild(e);
                });

                this.core.getFormElement().removeChild(this.hiddenTokenEle);
            }
        }, {
            key: "onFormValid",
            value: function onFormValid() {
                var _this2 = this;

                window["grecaptcha"].execute(this.opts.siteKey, {
                    action: this.opts.action
                }).then(function (e) {
                    _this2.hiddenTokenEle.setAttribute("name", _this2.opts.hiddenTokenName);

                    _this2.hiddenTokenEle.value = e;

                    var _t2 = _this2.core.getFormElement();

                    if (_t2 instanceof HTMLFormElement) {
                        _t2.submit();
                    }
                });
            }
        }, {
            key: "getScript",
            value: function getScript() {
                var e = this.opts.language ? "&hl=".concat(this.opts.language) : "";
                return "https://www.google.com/recaptcha/api.js?" + "onload=".concat(t.LOADED_CALLBACK, "&render=").concat(this.opts.siteKey).concat(e);
            }
        }]);

        return t;
    }(e);
    t.LOADED_CALLBACK = "___reCaptcha3Loaded___";

    return t;

}));
