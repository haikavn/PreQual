/*! modernizr 3.2.0 (Custom Build) | MIT *
 * http://modernizr.com/download/?-applicationcache-audio-backgroundsize-borderimage-borderradius-boxshadow-canvas-canvastext-cssanimations-csscolumns-cssgradients-cssreflections-csstransforms-csstransforms3d-csstransitions-flexbox-fontface-generatedcontent-geolocation-hashchange-history-hsla-indexeddb-inlinesvg-input-inputtypes-localstorage-multiplebgs-opacity-postmessage-rgba-sessionstorage-smil-svg-svgclippaths-textshadow-video-webgl-websockets-websqldatabase-webworkers-addtest-atrule-domprefixes-hasevent-mq-prefixed-prefixedcss-prefixedcssvalue-prefixes-printshiv-setclasses-testallprops-testprop-teststyles !*/
!function(e, t, n) {
    function r(e, t) { return typeof e === t }

    function a() {
        var e, t, n, a, o, i, s;
        for (var c in x)
            if (x.hasOwnProperty(c)) {
                if (e = [], t =
                    x[c], t.name &&
                    (e.push(t.name.toLowerCase()), t.options && t.options.aliases && t.options.aliases.length))
                    for (n = 0; n < t.options.aliases.length; n++) e.push(t.options.aliases[n].toLowerCase());
                for (a = r(t.fn, "function") ? t.fn() : t.fn, o = 0; o < e.length; o++)
                    i = e[o], s =
                        i.split("."), 1 === s.length
                        ? Modernizr[s[0]] = a
                        : (!Modernizr[s[0]] ||
                            Modernizr[s[0]] instanceof Boolean ||
                            (Modernizr[s[0]] = new Boolean(Modernizr[s[0]])), Modernizr[s[0]][s[1]] = a), b.push(
                        (a ? "" : "no-") + s.join("-"));
            }
    }

    function o(e) {
        var t = S.className, n = Modernizr._config.classPrefix || "";
        if (C && (t = t.baseVal), Modernizr._config.enableJSClass) {
            var r = new RegExp("(^|\\s)" + n + "no-js(\\s|$)");
            t = t.replace(r, "$1" + n + "js$2");
        }
        Modernizr._config.enableClasses &&
            (t += " " + n + e.join(" " + n), C ? S.className.baseVal = t : S.className = t);
    }

    function i(e, t) {
        if ("object" == typeof e) for (var n in e) _(e, n) && i(n, e[n]);
        else {
            e = e.toLowerCase();
            var r = e.split("."), a = Modernizr[r[0]];
            if (2 == r.length && (a = a[r[1]]), "undefined" != typeof a) return Modernizr;
            t = "function" == typeof t ? t() : t, 1 == r.length
                ? Modernizr[r[0]] = t
                : (!Modernizr[r[0]] ||
                    Modernizr[r[0]] instanceof Boolean ||
                    (Modernizr[r[0]] = new Boolean(Modernizr[r[0]])), Modernizr[r[0]][r[1]] = t), o([
                (t && 0 != t ? "" : "no-") + r.join("-")
            ]), Modernizr._trigger(e, t);
        }
        return Modernizr;
    }

    function s() {
        return"function" != typeof t.createElement
            ? t.createElement(arguments[0])
            : C
            ? t.createElementNS.call(t, "http://www.w3.org/2000/svg", arguments[0])
            : t.createElement.apply(t, arguments);
    }

    function c(e) {
        return e.replace(/([a-z])-([a-z])/g, function(e, t, n) { return t + n.toUpperCase() }).replace(/^-/, "");
    }

    function l(e) {
        return e.replace(/([A-Z])/g, function(e, t) { return"-" + t.toLowerCase() }).replace(/^ms-/, "-ms-");
    }

    function d(e, t) { return!!~("" + e).indexOf(t) }

    function u() {
        var e = t.body;
        return e || (e = s(C ? "svg" : "body"), e.fake = !0), e;
    }

    function f(e, n, r, a) {
        var o, i, c, l, d = "modernizr", f = s("div"), p = u();
        if (parseInt(r, 10)) for (; r--;) c = s("div"), c.id = a ? a[r] : d + (r + 1), f.appendChild(c);
        return o = s("style"), o.type = "text/css", o.id =
                "s" + d, (p.fake ? p : f).appendChild(o), p.appendChild(f),
            o.styleSheet ? o.styleSheet.cssText = e : o.appendChild(t.createTextNode(e)), f.id =
                d, p.fake &&
            (p.style.background = "", p.style.overflow = "hidden", l = S.style.overflow, S.style.overflow =
                "hidden", S.appendChild(p)), i =
                n(f, e), p.fake
                ? (p.parentNode.removeChild(p), S.style.overflow = l, S.offsetHeight)
                : f.parentNode.removeChild(f), !!i;
    }

    function p(t, r) {
        var a = t.length;
        if ("CSS" in e && "supports" in e.CSS) {
            for (; a--;) if (e.CSS.supports(l(t[a]), r)) return!0;
            return!1;
        }
        if ("CSSSupportsRule" in e) {
            for (var o = []; a--;) o.push("(" + l(t[a]) + ":" + r + ")");
            return o = o.join(" or "), f("@supports (" + o + ") { #modernizr { position: absolute; } }",
                function(e) { return"absolute" == getComputedStyle(e, null).position });
        }
        return n;
    }

    function m(e, t) { return function() { return e.apply(t, arguments) } }

    function h(e, t, n) {
        var a;
        for (var o in e) if (e[o] in t) return n === !1 ? e[o] : (a = t[e[o]], r(a, "function") ? m(a, n || t) : a);
        return!1;
    }

    function g(e, t, a, o) {
        function i() { u && (delete q.style, delete q.modElem) }

        if (o = r(o, "undefined") ? !1 : o, !r(a, "undefined")) {
            var l = p(e, a);
            if (!r(l, "undefined")) return l;
        }
        for (var u, f, m, h, g, v = ["modernizr", "tspan"]; !q.style;)
            u = !0, q.modElem = s(v.shift()), q.style = q.modElem.style;
        for (m = e.length, f = 0; m > f; f++)
            if (h = e[f], g = q.style[h], d(h, "-") && (h = c(h)), q.style[h] !== n) {
                if (o || r(a, "undefined")) return i(), "pfx" == t ? h : !0;
                try {
                    q.style[h] = a;
                } catch (y) {
                }
                if (q.style[h] != g) return i(), "pfx" == t ? h : !0;
            }
        return i(), !1;
    }

    function v(e, t, n, a, o) {
        var i = e.charAt(0).toUpperCase() + e.slice(1), s = (e + " " + P.join(i + " ") + i).split(" ");
        return r(t, "string") || r(t, "undefined")
            ? g(s, t, a, o)
            : (s = (e + " " + k.join(i + " ") + i).split(" "), h(s, t, n));
    }

    function y(e, t, r) { return v(e, n, n, t, r) }

    var b = [],
        x = [],
        T = {
            _version: "3.2.0",
            _config: { classPrefix: "", enableClasses: !0, enableJSClass: !0, usePrefixes: !0 },
            _q: [],
            on: function(e, t) {
                var n = this;
                setTimeout(function() { t(n[e]) }, 0);
            },
            addTest: function(e, t, n) { x.push({ name: e, fn: t, options: n }) },
            addAsyncTest: function(e) { x.push({ name: null, fn: e }) }
        },
        Modernizr = function() {};
    Modernizr.prototype = T, Modernizr =
            new Modernizr, Modernizr.addTest("applicationcache", "applicationCache" in e), Modernizr.addTest(
            "geolocation",
            "geolocation" in navigator), Modernizr.addTest("history",
            function() {
                var t = navigator.userAgent;
                return-1 === t.indexOf("Android 2.") && -1 === t.indexOf("Android 4.0") ||
                    -1 === t.indexOf("Mobile Safari") ||
                    -1 !== t.indexOf("Chrome") ||
                    -1 !== t.indexOf("Windows Phone")
                    ? e.history && "pushState" in e.history
                    : !1;
            }), Modernizr.addTest("postmessage", "postMessage" in e), Modernizr.addTest("svg",
            !!t.createElementNS && !!t.createElementNS("http://www.w3.org/2000/svg", "svg").createSVGRect),
        Modernizr.addTest("websockets", "WebSocket" in e && 2 === e.WebSocket.CLOSING), Modernizr.addTest(
            "localstorage",
            function() {
                var e = "modernizr";
                try {
                    return localStorage.setItem(e, e), localStorage.removeItem(e), !0;
                } catch (t) {
                    return!1;
                }
            }), Modernizr.addTest("sessionstorage",
            function() {
                var e = "modernizr";
                try {
                    return sessionStorage.setItem(e, e), sessionStorage.removeItem(e), !0;
                } catch (t) {
                    return!1;
                }
            }), Modernizr.addTest("websqldatabase", "openDatabase" in e),
        Modernizr.addTest("webworkers", "Worker" in e);
    var w = T._config.usePrefixes ? " -webkit- -moz- -o- -ms- ".split(" ") : [];
    T._prefixes = w;
    var S = t.documentElement, C = "svg" === S.nodeName.toLowerCase();
    C ||
        !function(e, t) {
            function n(e, t) {
                var n = e.createElement("p"), r = e.getElementsByTagName("head")[0] || e.documentElement;
                return n.innerHTML = "x<style>" + t + "</style>", r.insertBefore(n.lastChild, r.firstChild);
            }

            function r() {
                var e = C.elements;
                return"string" == typeof e ? e.split(" ") : e;
            }

            function a(e, t) {
                var n = C.elements;
                "string" != typeof n && (n = n.join(" ")), "string" != typeof e && (e = e.join(" ")), C.elements =
                    n + " " + e, l(t);
            }

            function o(e) {
                var t = S[e[T]];
                return t || (t = {}, w++, e[T] = w, S[w] = t), t;
            }

            function i(e, n, r) {
                if (n || (n = t), g) return n.createElement(e);
                r || (r = o(n));
                var a;
                return a =
                    r.cache[e]
                    ? r.cache[e].cloneNode()
                    : x.test(e)
                    ? (r.cache[e] = r.createElem(e)).cloneNode()
                    : r.createElem(e), !a.canHaveChildren || b.test(e) || a.tagUrn ? a : r.frag.appendChild(a);
            }

            function s(e, n) {
                if (e || (e = t), g) return e.createDocumentFragment();
                n = n || o(e);
                for (var a = n.frag.cloneNode(), i = 0, s = r(), c = s.length; c > i; i++) a.createElement(s[i]);
                return a;
            }

            function c(e, t) {
                t.cache ||
                (t.cache = {}, t.createElem = e.createElement, t.createFrag = e.createDocumentFragment, t.frag =
                    t.createFrag()), e.createElement =
                    function(n) { return C.shivMethods ? i(n, e, t) : t.createElem(n) }, e.createDocumentFragment =
                    Function("h,f",
                        "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" +
                        r().join().replace(/[\w\-:]+/g,
                            function(e) { return t.createElem(e), t.frag.createElement(e), 'c("' + e + '")' }) +
                        ");return n}")(C, t.frag);
            }

            function l(e) {
                e || (e = t);
                var r = o(e);
                return!C.shivCSS ||
                    h ||
                    r.hasCSS ||
                    (r.hasCSS = !!n(e,
                        "article,aside,dialog,figcaption,figure,footer,header,hgroup,main,nav,section{display:block}mark{background:#FF0;color:#000}template{display:none}")
                    ), g || c(e, r), e;
            }

            function d(e) {
                for (var t,
                    n = e.getElementsByTagName("*"),
                    a = n.length,
                    o = RegExp("^(?:" + r().join("|") + ")$", "i"),
                    i = [];
                    a--;
                ) t = n[a], o.test(t.nodeName) && i.push(t.applyElement(u(t)));
                return i;
            }

            function u(e) {
                for (var t, n = e.attributes, r = n.length, a = e.ownerDocument.createElement(k + ":" + e.nodeName);
                    r--;
                ) t = n[r], t.specified && a.setAttribute(t.nodeName, t.nodeValue);
                return a.style.cssText = e.style.cssText, a;
            }

            function f(e) {
                for (var t,
                    n = e.split("{"),
                    a = n.length,
                    o = RegExp("(^|[\\s,>+~])(" + r().join("|") + ")(?=[[\\s,>+~#.:]|$)", "gi"),
                    i = "$1" + k + "\\:$2";
                    a--;
                ) t = n[a] = n[a].split("}"), t[t.length - 1] = t[t.length - 1].replace(o, i), n[a] = t.join("}");
                return n.join("{");
            }

            function p(e) { for (var t = e.length; t--;) e[t].removeNode() }

            function m(e) {
                function t() { clearTimeout(i._removeSheetTimer), r && r.removeNode(!0), r = null }

                var r, a, i = o(e), s = e.namespaces, c = e.parentWindow;
                return!_ || e.printShived
                    ? e
                    : ("undefined" == typeof s[k] && s.add(k), c.attachEvent("onbeforeprint",
                        function() {
                            t();
                            for (var o, i, s, c = e.styleSheets, l = [], u = c.length, p = Array(u); u--;) p[u] = c[u];
                            for (; s = p.pop();)
                                if (!s.disabled && E.test(s.media)) {
                                    try {
                                        o = s.imports, i = o.length;
                                    } catch (m) {
                                        i = 0;
                                    }
                                    for (u = 0; i > u; u++) p.push(o[u]);
                                    try {
                                        l.push(s.cssText);
                                    } catch (m) {
                                    }
                                }
                            l = f(l.reverse().join("")), a = d(e), r = n(e, l);
                        }), c.attachEvent("onafterprint",
                        function() {
                            p(a), clearTimeout(i._removeSheetTimer), i._removeSheetTimer = setTimeout(t, 500);
                        }), e.printShived = !0, e);
            }

            var h,
                g,
                v = "3.7.3",
                y = e.html5 || {},
                b = /^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i,
                x =
                    /^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i,
                T = "_html5shiv",
                w = 0,
                S = {};
            !function() {
                try {
                    var e = t.createElement("a");
                    e.innerHTML = "<xyz></xyz>", h = "hidden" in e, g = 1 == e.childNodes.length ||
                        function() {
                            t.createElement("a");
                            var e = t.createDocumentFragment();
                            return"undefined" == typeof e.cloneNode ||
                                "undefined" == typeof e.createDocumentFragment ||
                                "undefined" == typeof e.createElement;
                        }();
                } catch (n) {
                    h = !0, g = !0;
                }
            }();
            var C = {
                elements: y.elements ||
                    "abbr article aside audio bdi canvas data datalist details dialog figcaption figure footer header hgroup main mark meter nav output picture progress section summary template time video",
                version: v,
                shivCSS: y.shivCSS !== !1,
                supportsUnknownElements: g,
                shivMethods: y.shivMethods !== !1,
                type: "default",
                shivDocument: l,
                createElement: i,
                createDocumentFragment: s,
                addElements: a
            };
            e.html5 = C, l(t);
            var E = /^$|\b(?:all|print)\b/,
                k = "html5shiv",
                _ = !g &&
                    function() {
                        var n = t.documentElement;
                        return!("undefined" == typeof t.namespaces ||
                            "undefined" == typeof t.parentWindow ||
                            "undefined" == typeof n.applyElement ||
                            "undefined" == typeof n.removeNode ||
                            "undefined" == typeof e.attachEvent);
                    }();
            C.type += " print", C.shivPrint =
                m, m(t), "object" == typeof module && module.exports && (module.exports = C);
        }("undefined" != typeof e ? e : this, t);
    var E = "Moz O ms Webkit", k = T._config.usePrefixes ? E.toLowerCase().split(" ") : [];
    T._domPrefixes = k;
    var _;
    !function() {
        var e = {}.hasOwnProperty;
        _ = r(e, "undefined") || r(e.call, "undefined")
            ? function(e, t) { return t in e && r(e.constructor.prototype[t], "undefined") }
            : function(t, n) { return e.call(t, n) };
    }(), T._l = {}, T.on =
        function(e, t) {
            this._l[e] || (this._l[e] = []), this._l[e].push(t), Modernizr.hasOwnProperty(e) &&
                setTimeout(function() { Modernizr._trigger(e, Modernizr[e]) }, 0);
        }, T._trigger = function(e, t) {
        if (this._l[e]) {
            var n = this._l[e];
            setTimeout(function() {
                    var e, r;
                    for (e = 0; e < n.length; e++) (r = n[e])(t);
                },
                0), delete this._l[e];
        }
    }, Modernizr._q.push(function() { T.addTest = i });
    var P = T._config.usePrefixes ? E.split(" ") : [];
    T._cssomPrefixes = P;
    var N = function(t) {
        var r, a = w.length, o = e.CSSRule;
        if ("undefined" == typeof o) return n;
        if (!t) return!1;
        if (t = t.replace(/^@/, ""), r = t.replace(/-/g, "_").toUpperCase() + "_RULE", r in o) return"@" + t;
        for (var i = 0; a > i; i++) {
            var s = w[i], c = s.toUpperCase() + "_" + r;
            if (c in o) return"@-" + s.toLowerCase() + "-" + t;
        }
        return!1;
    };
    T.atRule = N;
    var $ = function() {
        function e(e, t) {
            var a;
            return e
                ? (t && "string" != typeof t || (t = s(t || "div")), e = "on" + e, a = e in t, !a &&
                    r &&
                    (t.setAttribute || (t = s("div")), t.setAttribute(e, ""), a =
                        "function" == typeof t[e], t[e] !== n && (t[e] = n), t.removeAttribute(e)), a)
                : !1;
        }

        var r = !("onblur" in t.documentElement);
        return e;
    }();
    T.hasEvent = $, Modernizr.addTest("hashchange",
        function() { return $("hashchange", e) === !1 ? !1 : t.documentMode === n || t.documentMode > 7 });
    var R = function(e, t) {
        var n = !1, r = s("div"), a = r.style;
        if (e in a) {
            var o = k.length;
            for (a[e] = t, n = a[e]; o-- && !n;) a[e] = "-" + k[o] + "-" + t, n = a[e];
        }
        return"" === n && (n = !1), n;
    };
    T.prefixedCSSValue = R, Modernizr.addTest("audio",
        function() {
            var e = s("audio"), t = !1;
            try {
                (t = !!e.canPlayType) &&
                (t = new Boolean(t), t.ogg =
                    e.canPlayType('audio/ogg; codecs="vorbis"').replace(/^no$/, ""), t.mp3 =
                    e.canPlayType('audio/mpeg; codecs="mp3"').replace(/^no$/, ""), t.opus =
                    e.canPlayType('audio/ogg; codecs="opus"').replace(/^no$/, ""), t.wav =
                    e.canPlayType('audio/wav; codecs="1"').replace(/^no$/, ""), t.m4a =
                    (e.canPlayType("audio/x-m4a;") || e.canPlayType("audio/aac;")).replace(/^no$/, ""));
            } catch (n) {
            }
            return t;
        }), Modernizr.addTest("canvas",
        function() {
            var e = s("canvas");
            return!(!e.getContext || !e.getContext("2d"));
        }), Modernizr.addTest("canvastext",
        function() {
            return Modernizr.canvas === !1 ? !1 : "function" == typeof s("canvas").getContext("2d").fillText;
        }), Modernizr.addTest("video",
        function() {
            var e = s("video"), t = !1;
            try {
                (t = !!e.canPlayType) &&
                (t = new Boolean(t), t.ogg =
                    e.canPlayType('video/ogg; codecs="theora"').replace(/^no$/, ""), t.h264 =
                    e.canPlayType('video/mp4; codecs="avc1.42E01E"').replace(/^no$/, ""), t.webm =
                    e.canPlayType('video/webm; codecs="vp8, vorbis"').replace(/^no$/, ""), t.vp9 =
                    e.canPlayType('video/webm; codecs="vp9"').replace(/^no$/, ""), t.hls =
                    e.canPlayType('application/x-mpegURL; codecs="avc1.42E01E"').replace(/^no$/, ""));
            } catch (n) {
            }
            return t;
        }), Modernizr.addTest("webgl",
        function() {
            var t = s("canvas"), n = "probablySupportsContext" in t ? "probablySupportsContext" : "supportsContext";
            return n in t ? t[n]("webgl") || t[n]("experimental-webgl") : "WebGLRenderingContext" in e;
        }), Modernizr.addTest("cssgradients",
        function() {
            for (var e,
                t = "background-image:",
                n = "gradient(linear,left top,right bottom,from(#9f9),to(white));",
                r = "",
                a = 0,
                o = w.length - 1;
                o > a;
                a++) e = 0 === a ? "to " : "", r += t + w[a] + "linear-gradient(" + e + "left top, #9f9, white);";
            Modernizr._config.usePrefixes && (r += t + "-webkit-" + n);
            var i = s("a"), c = i.style;
            return c.cssText = r, ("" + c.backgroundImage).indexOf("gradient") > -1;
        }), Modernizr.addTest("multiplebgs",
        function() {
            var e = s("a").style;
            return e.cssText =
                "background:url(https://),url(https://),red url(https://)", /(url\s*\(.*?){3}/.test(e.background);
        }), Modernizr.addTest("opacity",
        function() {
            var e = s("a").style;
            return e.cssText = w.join("opacity:.55;"), /^0.55$/.test(e.opacity);
        }), Modernizr.addTest("rgba",
        function() {
            var e = s("a").style;
            return e.cssText = "background-color:rgba(150,255,150,.5)", ("" + e.backgroundColor).indexOf("rgba") > -1;
        }), Modernizr.addTest("inlinesvg",
        function() {
            var e = s("div");
            return e.innerHTML = "<svg/>", "http://www.w3.org/2000/svg" ==
                ("undefined" != typeof SVGRect && e.firstChild && e.firstChild.namespaceURI);
        });
    var z = s("input"),
        A = "autocomplete autofocus list placeholder max min multiple pattern required step".split(" "),
        j = {};
    Modernizr.input = function(t) {
        for (var n = 0, r = t.length; r > n; n++) j[t[n]] = !!(t[n] in z);
        return j.list && (j.list = !(!s("datalist") || !e.HTMLDataListElement)), j;
    }(A);
    var O = "search tel url email datetime date month week time datetime-local number range color".split(" "), B = {};
    Modernizr.inputtypes = function(e) {
        for (var r, a, o, i = e.length, s = "1)", c = 0; i > c; c++)
            z.setAttribute("type", r = e[c]), o = "text" !== z.type && "style" in z, o &&
            (z.value = s, z.style.cssText =
                "position:absolute;visibility:hidden;", /^range$/.test(r) && z.style.WebkitAppearance !== n
                ? (S.appendChild(z), a = t.defaultView, o =
                    a.getComputedStyle &&
                    "textfield" !== a.getComputedStyle(z, null).WebkitAppearance &&
                    0 !== z.offsetHeight, S.removeChild(z))
                : /^(search|tel)$/.test(r) ||
                (o = /^(url|email)$/.test(r) ? z.checkValidity && z.checkValidity() === !1 : z.value != s)), B[e[c]] =
                !!o;
        return B;
    }(O), Modernizr.addTest("hsla",
        function() {
            var e = s("a").style;
            return e.cssText =
                "background-color:hsla(120,40%,100%,.5)", d(e.backgroundColor, "rgba") || d(e.backgroundColor, "hsla");
        });
    var L = "CSS" in e && "supports" in e.CSS, M = "supportsCSS" in e;
    Modernizr.addTest("supports", L || M);
    var F = {}.toString;
    Modernizr.addTest("svgclippaths",
        function() {
            return!!t.createElementNS &&
                /SVGClipPath/.test(F.call(t.createElementNS("http://www.w3.org/2000/svg", "clipPath")));
        }), Modernizr.addTest("smil",
        function() {
            return!!t.createElementNS &&
                /SVGAnimate/.test(F.call(t.createElementNS("http://www.w3.org/2000/svg", "animate")));
        });
    var W = function() {
        var t = e.matchMedia || e.msMatchMedia;
        return t
            ? function(e) {
                var n = t(e);
                return n && n.matches || !1;
            }
            : function(t) {
                var n = !1;
                return f("@media " + t + " { #modernizr { position: absolute; } }",
                    function(t) {
                        n = "absolute" == (e.getComputedStyle ? e.getComputedStyle(t, null) : t.currentStyle).position;
                    }), n;
            };
    }();
    T.mq = W;
    var D = T.testStyles = f,
        I = function() {
            var e = navigator.userAgent,
                t = e.match(/applewebkit\/([0-9]+)/gi) && parseFloat(RegExp.$1),
                n = e.match(/w(eb)?osbrowser/gi),
                r = e.match(/windows phone/gi) && e.match(/iemobile\/([0-9])+/gi) && parseFloat(RegExp.$1) >= 9,
                a = 533 > t && e.match(/android/gi);
            return n || a || r;
        }();
    I
        ? Modernizr.addTest("fontface", !1)
        : D('@font-face {font-family:"font";src:url("https://")}',
            function(e, n) {
                var r = t.getElementById("smodernizr"),
                    a = r.sheet || r.styleSheet,
                    o = a ? a.cssRules && a.cssRules[0] ? a.cssRules[0].cssText : a.cssText || "" : "",
                    i = /src/i.test(o) && 0 === o.indexOf(n.split(" ")[0]);
                Modernizr.addTest("fontface", i);
            }), D('#modernizr{font:0/0 a}#modernizr:after{content:":)";visibility:hidden;font:7px/1 a}',
        function(e) { Modernizr.addTest("generatedcontent", e.offsetHeight >= 7) });
    var V = { elem: s("modernizr") };
    Modernizr._q.push(function() { delete V.elem });
    var q = { style: V.elem.style };
    Modernizr._q.unshift(function() { delete q.style });
    var H = T.testProp = function(e, t, r) { return g([e], n, t, r) };
    Modernizr.addTest("textshadow", H("textShadow", "1px 1px")), T.testAllProps = v;
    var U = T.prefixed =
            function(e, t, n) {
                return 0 === e.indexOf("@") ? N(e) : (-1 != e.indexOf("-") && (e = c(e)), t ? v(e, t, n) : v(e, "pfx"));
            },
        G = (T.prefixedCSS = function(e) {
            var t = U(e);
            return t && l(t);
        }, U("indexedDB", e));
    Modernizr.addTest("indexeddb", !!G), G && Modernizr.addTest("indexeddb.deletedatabase", "deleteDatabase" in G),
        T.testAllProps =
            y, Modernizr.addTest("cssanimations", y("animationName", "a", !0)),
        Modernizr.addTest("backgroundsize", y("backgroundSize", "100%", !0)),
        Modernizr.addTest("borderimage", y("borderImage", "url() 1", !0)),
        Modernizr.addTest("borderradius", y("borderRadius", "0px", !0)),
        Modernizr.addTest("boxshadow", y("boxShadow", "1px 1px", !0)), function() {
            Modernizr.addTest("csscolumns",
                function() {
                    var e = !1, t = y("columnCount");
                    try {
                        (e = !!t) && (e = new Boolean(e));
                    } catch (n) {
                    }
                    return e;
                });
            for (var e,
                t,
                n = [
                    "Width", "Span", "Fill", "Gap", "Rule", "RuleColor", "RuleStyle", "RuleWidth", "BreakBefore",
                    "BreakAfter", "BreakInside"
                ],
                r = 0;
                r < n.length;
                r++)
                e = n[r].toLowerCase(), t =
                    y("column" + n[r]), ("breakbefore" === e || "breakafter" === e || "breakinside" == e) &&
                    (t = t || y(n[r])), Modernizr.addTest("csscolumns." + e, t);
        }(), Modernizr.addTest("flexbox", y("flexBasis", "1px", !0)),
        Modernizr.addTest("cssreflections", y("boxReflect", "above", !0)), Modernizr.addTest("csstransforms",
            function() { return-1 === navigator.userAgent.indexOf("Android 2.") && y("transform", "scale(1)", !0) }),
        Modernizr.addTest("csstransforms3d",
            function() {
                var e = !!y("perspective", "1px", !0), t = Modernizr._config.usePrefixes;
                if (e && (!t || "webkitPerspective" in S.style)) {
                    var n, r = "#modernizr{width:0;height:0}";
                    Modernizr.supports
                        ? n = "@supports (perspective: 1px)"
                        : (n = "@media (transform-3d)", t && (n += ",(-webkit-transform-3d)")), n +=
                        "{#modernizr{width:7px;height:18px;margin:0;padding:0;border:0}}", D(r + n,
                        function(t) { e = 7 === t.offsetWidth && 18 === t.offsetHeight });
                }
                return e;
            }), Modernizr.addTest("csstransitions", y("transition", "all", !0)), a(), o(b), delete T.addTest, delete T
            .addAsyncTest;
    for (var J = 0; J < Modernizr._q.length; J++) Modernizr._q[J]();
    e.Modernizr = Modernizr;
}(window, document);