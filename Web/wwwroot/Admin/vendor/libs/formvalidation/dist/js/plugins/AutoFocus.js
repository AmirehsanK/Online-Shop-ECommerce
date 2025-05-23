/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, (global.FormValidation = global.FormValidation || {}, global.FormValidation.plugins = global.FormValidation.plugins || {}, global.FormValidation.plugins.AutoFocus = factory()));
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

    var t$1 = FormValidation.Plugin;

    var t = /*#__PURE__*/function (_e) {
        _inherits(t, _e);

        var _super = _createSuper(t);

        function t(e) {
            var _this;

            _classCallCheck(this, t);

            _this = _super.call(this, e);
            _this.statuses = new Map();
            _this.opts = Object.assign({}, {
                onStatusChanged: function onStatusChanged() {
                }
            }, e);
            _this.elementValidatingHandler = _this.onElementValidating.bind(_assertThisInitialized(_this));
            _this.elementValidatedHandler = _this.onElementValidated.bind(_assertThisInitialized(_this));
            _this.elementNotValidatedHandler = _this.onElementNotValidated.bind(_assertThisInitialized(_this));
            _this.elementIgnoredHandler = _this.onElementIgnored.bind(_assertThisInitialized(_this));
            _this.fieldAddedHandler = _this.onFieldAdded.bind(_assertThisInitialized(_this));
            _this.fieldRemovedHandler = _this.onFieldRemoved.bind(_assertThisInitialized(_this));
            return _this;
        }

        _createClass(t, [{
            key: "install",
            value: function install() {
                this.core.on("core.element.validating", this.elementValidatingHandler).on("core.element.validated", this.elementValidatedHandler).on("core.element.notvalidated", this.elementNotValidatedHandler).on("core.element.ignored", this.elementIgnoredHandler).on("core.field.added", this.fieldAddedHandler).on("core.field.removed", this.fieldRemovedHandler);
            }
        }, {
            key: "uninstall",
            value: function uninstall() {
                this.statuses.clear();
                this.core.off("core.element.validating", this.elementValidatingHandler).off("core.element.validated", this.elementValidatedHandler).off("core.element.notvalidated", this.elementNotValidatedHandler).off("core.element.ignored", this.elementIgnoredHandler).off("core.field.added", this.fieldAddedHandler).off("core.field.removed", this.fieldRemovedHandler);
            }
        }, {
            key: "areFieldsValid",
            value: function areFieldsValid() {
                return Array.from(this.statuses.values()).every(function (e) {
                    return e === "Valid" || e === "NotValidated" || e === "Ignored";
                });
            }
        }, {
            key: "getStatuses",
            value: function getStatuses() {
                return this.statuses;
            }
        }, {
            key: "onFieldAdded",
            value: function onFieldAdded(e) {
                this.statuses.set(e.field, "NotValidated");
            }
        }, {
            key: "onFieldRemoved",
            value: function onFieldRemoved(e) {
                if (this.statuses.has(e.field)) {
                    this.statuses["delete"](e.field);
                }

                this.opts.onStatusChanged(this.areFieldsValid());
            }
        }, {
            key: "onElementValidating",
            value: function onElementValidating(e) {
                this.statuses.set(e.field, "Validating");
                this.opts.onStatusChanged(false);
            }
        }, {
            key: "onElementValidated",
            value: function onElementValidated(e) {
                this.statuses.set(e.field, e.valid ? "Valid" : "Invalid");

                if (e.valid) {
                    this.opts.onStatusChanged(this.areFieldsValid());
                } else {
                    this.opts.onStatusChanged(false);
                }
            }
        }, {
            key: "onElementNotValidated",
            value: function onElementNotValidated(e) {
                this.statuses.set(e.field, "NotValidated");
                this.opts.onStatusChanged(false);
            }
        }, {
            key: "onElementIgnored",
            value: function onElementIgnored(e) {
                this.statuses.set(e.field, "Ignored");
                this.opts.onStatusChanged(this.areFieldsValid());
            }
        }]);

        return t;
    }(t$1);

    var s = /*#__PURE__*/function (_t) {
        _inherits(s, _t);

        var _super = _createSuper(s);

        function s(t) {
            var _this;

            _classCallCheck(this, s);

            _this = _super.call(this, t);
            _this.fieldStatusPluginName = "___autoFocusFieldStatus";
            _this.opts = Object.assign({}, {
                onPrefocus: function onPrefocus() {
                }
            }, t);
            _this.invalidFormHandler = _this.onFormInvalid.bind(_assertThisInitialized(_this));
            return _this;
        }

        _createClass(s, [{
            key: "install",
            value: function install() {
                this.core.on("core.form.invalid", this.invalidFormHandler).registerPlugin(this.fieldStatusPluginName, new t());
            }
        }, {
            key: "uninstall",
            value: function uninstall() {
                this.core.off("core.form.invalid", this.invalidFormHandler).deregisterPlugin(this.fieldStatusPluginName);
            }
        }, {
            key: "onFormInvalid",
            value: function onFormInvalid() {
                var t = this.core.getPlugin(this.fieldStatusPluginName);
                var i = t.getStatuses();

                var _s = Object.keys(this.core.getFields()).filter(function (t) {
                    return i.get(t) === "Invalid";
                });

                if (_s.length > 0) {
                    var _t2 = _s[0];

                    var _i = this.core.getElements(_t2);

                    if (_i.length > 0) {
                        var _s3 = _i[0];
                        var e = {
                            firstElement: _s3,
                            field: _t2
                        };
                        this.core.emit("plugins.autofocus.prefocus", e);
                        this.opts.onPrefocus(e);

                        _s3.focus();
                    }
                }
            }
        }]);

        return s;
    }(t$1);

    return s;

}));
