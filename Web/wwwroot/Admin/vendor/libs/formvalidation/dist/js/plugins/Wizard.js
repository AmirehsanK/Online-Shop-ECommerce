/**
 * FormValidation (https://formvalidation.io), v1.10.0 (2236098)
 * The best validation library for JavaScript
 * (c) 2013 - 2021 Nguyen Huu Phuoc <me@phuoc.ng>
 */

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, (global.FormValidation = global.FormValidation || {}, global.FormValidation.plugins = global.FormValidation.plugins || {}, global.FormValidation.plugins.Wizard = factory()));
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

    var e = FormValidation.utils.classSet;

    var s = FormValidation.plugins.Excluded;

    var i = /*#__PURE__*/function (_t) {
        _inherits(i, _t);

        var _super = _createSuper(i);

        function i(t) {
            var _this;

            _classCallCheck(this, i);

            _this = _super.call(this, t);
            _this.currentStep = 0;
            _this.numSteps = 0;
            _this.stepIndexes = [];
            _this.opts = Object.assign({}, {
                activeStepClass: "fv-plugins-wizard--active",
                onStepActive: function onStepActive() {
                },
                onStepInvalid: function onStepInvalid() {
                },
                onStepValid: function onStepValid() {
                },
                onValid: function onValid() {
                },
                stepClass: "fv-plugins-wizard--step"
            }, t);
            _this.prevStepHandler = _this.onClickPrev.bind(_assertThisInitialized(_this));
            _this.nextStepHandler = _this.onClickNext.bind(_assertThisInitialized(_this));
            return _this;
        }

        _createClass(i, [{
            key: "install",
            value: function install() {
                var _this2 = this;

                this.core.registerPlugin(i.EXCLUDED_PLUGIN, this.opts.isFieldExcluded ? new s({
                    excluded: this.opts.isFieldExcluded
                }) : new s());
                var t = this.core.getFormElement();
                this.steps = [].slice.call(t.querySelectorAll(this.opts.stepSelector));
                this.numSteps = this.steps.length;
                this.steps.forEach(function (t) {
                    e(t, _defineProperty({}, _this2.opts.stepClass, true));
                });
                e(this.steps[0], _defineProperty({}, this.opts.activeStepClass, true));
                this.stepIndexes = Array(this.numSteps).fill(0).map(function (t, e) {
                    return e;
                });
                this.prevButton = typeof this.opts.prevButton === "string" ? this.opts.prevButton.substring(0, 1) === "#" ? document.getElementById(this.opts.prevButton.substring(1)) : t.querySelector(this.opts.prevButton) : this.opts.prevButton;
                this.nextButton = typeof this.opts.nextButton === "string" ? this.opts.nextButton.substring(0, 1) === "#" ? document.getElementById(this.opts.nextButton.substring(1)) : t.querySelector(this.opts.nextButton) : this.opts.nextButton;
                this.prevButton.addEventListener("click", this.prevStepHandler);
                this.nextButton.addEventListener("click", this.nextStepHandler);
            }
        }, {
            key: "uninstall",
            value: function uninstall() {
                this.core.deregisterPlugin(i.EXCLUDED_PLUGIN);
                this.prevButton.removeEventListener("click", this.prevStepHandler);
                this.nextButton.removeEventListener("click", this.nextStepHandler);
                this.stepIndexes.length = 0;
            }
        }, {
            key: "getCurrentStep",
            value: function getCurrentStep() {
                return this.currentStep;
            }
        }, {
            key: "goToPrevStep",
            value: function goToPrevStep() {
                var _this3 = this;

                var t = this.currentStep - 1;

                if (t < 0) {
                    return;
                }

                var e = this.opts.isStepSkipped ? this.stepIndexes.slice(0, this.currentStep).reverse().find(function (t, e) {
                    return !_this3.opts.isStepSkipped({
                        currentStep: _this3.currentStep,
                        numSteps: _this3.numSteps,
                        targetStep: t
                    });
                }) : t;
                this.goToStep(e);
                this.onStepActive();
            }
        }, {
            key: "goToNextStep",
            value: function goToNextStep() {
                var _this4 = this;

                this.core.validate().then(function (t) {
                    if (t === "Valid") {
                        var _t2 = _this4.currentStep + 1;

                        if (_t2 >= _this4.numSteps) {
                            _this4.currentStep = _this4.numSteps - 1;
                        } else {
                            var _e3 = _this4.opts.isStepSkipped ? _this4.stepIndexes.slice(_t2, _this4.numSteps).find(function (t, e) {
                                return !_this4.opts.isStepSkipped({
                                    currentStep: _this4.currentStep,
                                    numSteps: _this4.numSteps,
                                    targetStep: t
                                });
                            }) : _t2;

                            _t2 = _e3;

                            _this4.goToStep(_t2);
                        }

                        _this4.onStepActive();

                        _this4.onStepValid();

                        if (_t2 === _this4.numSteps) {
                            _this4.onValid();
                        }
                    } else if (t === "Invalid") {
                        _this4.onStepInvalid();
                    }
                });
            }
        }, {
            key: "goToStep",
            value: function goToStep(t) {
                e(this.steps[this.currentStep], _defineProperty({}, this.opts.activeStepClass, false));
                e(this.steps[t], _defineProperty({}, this.opts.activeStepClass, true));
                this.currentStep = t;
            }
        }, {
            key: "onClickPrev",
            value: function onClickPrev() {
                this.goToPrevStep();
            }
        }, {
            key: "onClickNext",
            value: function onClickNext() {
                this.goToNextStep();
            }
        }, {
            key: "onStepActive",
            value: function onStepActive() {
                var t = {
                    numSteps: this.numSteps,
                    step: this.currentStep
                };
                this.core.emit("plugins.wizard.step.active", t);
                this.opts.onStepActive(t);
            }
        }, {
            key: "onStepValid",
            value: function onStepValid() {
                var t = {
                    numSteps: this.numSteps,
                    step: this.currentStep
                };
                this.core.emit("plugins.wizard.step.valid", t);
                this.opts.onStepValid(t);
            }
        }, {
            key: "onStepInvalid",
            value: function onStepInvalid() {
                var t = {
                    numSteps: this.numSteps,
                    step: this.currentStep
                };
                this.core.emit("plugins.wizard.step.invalid", t);
                this.opts.onStepInvalid(t);
            }
        }, {
            key: "onValid",
            value: function onValid() {
                var t = {
                    numSteps: this.numSteps
                };
                this.core.emit("plugins.wizard.valid", t);
                this.opts.onValid(t);
            }
        }]);

        return i;
    }(t);
    i.EXCLUDED_PLUGIN = "___wizardExcluded";

    return i;

}));
