/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, (global.FormValidation = global.FormValidation || {}, global.FormValidation.plugins = global.FormValidation.plugins || {}, global.FormValidation.plugins.PasswordStrength = factory()));
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

    var t = FormValidation.Plugin;

    var a = /*#__PURE__*/function (_t) {
        _inherits(a, _t);

        var _super = _createSuper(a);

        function a(t) {
            var _this;

            _classCallCheck(this, a);

            _this = _super.call(this, t);
            _this.opts = Object.assign({}, {
                minimalScore: 3,
                onValidated: function onValidated() {
                }
            }, t);
            _this.validatePassword = _this.checkPasswordStrength.bind(_assertThisInitialized(_this));
            _this.validatorValidatedHandler = _this.onValidatorValidated.bind(_assertThisInitialized(_this));
            return _this;
        }

        _createClass(a, [{
            key: "install",
            value: function install() {
                this.core.registerValidator(a.PASSWORD_STRENGTH_VALIDATOR, this.validatePassword);
                this.core.on("core.validator.validated", this.validatorValidatedHandler);
                this.core.addField(this.opts.field, {
                    validators: _defineProperty({}, a.PASSWORD_STRENGTH_VALIDATOR, {
                        message: this.opts.message,
                        minimalScore: this.opts.minimalScore
                    })
                });
            }
        }, {
            key: "uninstall",
            value: function uninstall() {
                this.core.off("core.validator.validated", this.validatorValidatedHandler);
                this.core.disableValidator(this.opts.field, a.PASSWORD_STRENGTH_VALIDATOR);
            }
        }, {
            key: "checkPasswordStrength",
            value: function checkPasswordStrength() {
                var _this2 = this;

                return {
                    validate: function validate(t) {
                        var _a = t.value;

                        if (_a === "") {
                            return {
                                valid: true
                            };
                        }

                        var e = zxcvbn(_a);
                        var s = e.score;
                        var i = e.feedback.warning || "The password is weak";

                        if (s < _this2.opts.minimalScore) {
                            return {
                                message: i,
                                meta: {
                                    message: i,
                                    score: s
                                },
                                valid: false
                            };
                        } else {
                            return {
                                meta: {
                                    message: i,
                                    score: s
                                },
                                valid: true
                            };
                        }
                    }
                };
            }
        }, {
            key: "onValidatorValidated",
            value: function onValidatorValidated(t) {
                if (t.field === this.opts.field && t.validator === a.PASSWORD_STRENGTH_VALIDATOR && t.result.meta) {
                    var _a3 = t.result.meta["message"];
                    var e = t.result.meta["score"];
                    this.opts.onValidated(t.result.valid, _a3, e);
                }
            }
        }]);

        return a;
    }(t);
    a.PASSWORD_STRENGTH_VALIDATOR = "___PasswordStrengthValidator";

    return a;

}));
