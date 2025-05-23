/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, (global.FormValidation = global.FormValidation || {}, global.FormValidation.plugins = global.FormValidation.plugins || {}, global.FormValidation.plugins.TypingAnimation = factory()));
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
                autoPlay: true
            }, e);
            return _this;
        }

        _createClass(t, [{
            key: "install",
            value: function install() {
                this.fields = Object.keys(this.core.getFields());

                if (this.opts.autoPlay) {
                    this.play();
                }
            }
        }, {
            key: "play",
            value: function play() {
                return this.animate(0);
            }
        }, {
            key: "animate",
            value: function animate(e) {
                var _this2 = this;

                if (e >= this.fields.length) {
                    return Promise.resolve(e);
                }

                var _t = this.fields[e];
                var s = this.core.getElements(_t)[0];
                var i = s.getAttribute("type");
                var r = this.opts.data[_t];

                if ("checkbox" === i || "radio" === i) {
                    s.checked = true;
                    s.setAttribute("checked", "true");
                    return this.core.revalidateField(_t).then(function (_t2) {
                        return _this2.animate(e + 1);
                    });
                } else if (!r) {
                    return this.animate(e + 1);
                } else {
                    return new Promise(function (i) {
                        return new Typed(s, {
                            attr: "value",
                            autoInsertCss: true,
                            bindInputFocusEvents: true,
                            onComplete: function onComplete() {
                                i(e + 1);
                            },
                            onStringTyped: function onStringTyped(e, i) {
                                s.value = r[e];

                                _this2.core.revalidateField(_t);
                            },
                            strings: r,
                            typeSpeed: 100
                        });
                    }).then(function (e) {
                        return _this2.animate(e);
                    });
                }
            }
        }]);

        return t;
    }(e);

    return t;

}));
