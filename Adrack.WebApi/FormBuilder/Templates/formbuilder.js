/*! jQuery v2.2.5-pre | (c) jQuery Foundation | jquery.org/license */
!function (a, b) { "object" == typeof module && "object" == typeof module.exports ? module.exports = a.document ? b(a, !0) : function (a) { if (!a.document) throw new Error("jQuery requires a window with a document"); return b(a) } : b(a) }("undefined" != typeof window ? window : this, function (a, b) {
    var c = [], d = a.document, e = c.slice, f = c.concat, g = c.push, h = c.indexOf, i = {}, j = i.toString, k = i.hasOwnProperty, l = {}, m = "2.2.5-pre b14ce54334a568eaaa107be4c441660a57c3db24", n = function (a, b) { return new n.fn.init(a, b) }, o = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, p = /^-ms-/, q = /-([\da-z])/gi, r = function (a, b) { return b.toUpperCase() }; n.fn = n.prototype = { jquery: m, constructor: n, selector: "", length: 0, toArray: function () { return e.call(this) }, get: function (a) { return null != a ? a < 0 ? this[a + this.length] : this[a] : e.call(this) }, pushStack: function (a) { var b = n.merge(this.constructor(), a); return b.prevObject = this, b.context = this.context, b }, each: function (a) { return n.each(this, a) }, map: function (a) { return this.pushStack(n.map(this, function (b, c) { return a.call(b, c, b) })) }, slice: function () { return this.pushStack(e.apply(this, arguments)) }, first: function () { return this.eq(0) }, last: function () { return this.eq(-1) }, eq: function (a) { var b = this.length, c = +a + (a < 0 ? b : 0); return this.pushStack(c >= 0 && c < b ? [this[c]] : []) }, end: function () { return this.prevObject || this.constructor() }, push: g, sort: c.sort, splice: c.splice }, n.extend = n.fn.extend = function () { var a, b, c, d, e, f, g = arguments[0] || {}, h = 1, i = arguments.length, j = !1; for ("boolean" == typeof g && (j = g, g = arguments[h] || {}, h++), "object" == typeof g || n.isFunction(g) || (g = {}), h === i && (g = this, h--); h < i; h++)if (null != (a = arguments[h])) for (b in a) c = g[b], d = a[b], g !== d && (j && d && (n.isPlainObject(d) || (e = n.isArray(d))) ? (e ? (e = !1, f = c && n.isArray(c) ? c : []) : f = c && n.isPlainObject(c) ? c : {}, g[b] = n.extend(j, f, d)) : void 0 !== d && (g[b] = d)); return g }, n.extend({ expando: "jQuery" + (m + Math.random()).replace(/\D/g, ""), isReady: !0, error: function (a) { throw new Error(a) }, noop: function () { }, isFunction: function (a) { return "function" === n.type(a) }, isArray: Array.isArray, isWindow: function (a) { return null != a && a === a.window }, isNumeric: function (a) { var b = a && a.toString(); return !n.isArray(a) && b - parseFloat(b) + 1 >= 0 }, isPlainObject: function (a) { var b; if ("object" !== n.type(a) || a.nodeType || n.isWindow(a)) return !1; if (a.constructor && !k.call(a, "constructor") && !k.call(a.constructor.prototype || {}, "isPrototypeOf")) return !1; for (b in a); return void 0 === b || k.call(a, b) }, isEmptyObject: function (a) { var b; for (b in a) return !1; return !0 }, type: function (a) { return null == a ? a + "" : "object" == typeof a || "function" == typeof a ? i[j.call(a)] || "object" : typeof a }, globalEval: function (a) { var b, c = eval; a = n.trim(a), a && (1 === a.indexOf("use strict") ? (b = d.createElement("script"), b.text = a, d.head.appendChild(b).parentNode.removeChild(b)) : c(a)) }, camelCase: function (a) { return a.replace(p, "ms-").replace(q, r) }, nodeName: function (a, b) { return a.nodeName && a.nodeName.toLowerCase() === b.toLowerCase() }, each: function (a, b) { var c, d = 0; if (s(a)) { for (c = a.length; d < c; d++)if (b.call(a[d], d, a[d]) === !1) break } else for (d in a) if (b.call(a[d], d, a[d]) === !1) break; return a }, trim: function (a) { return null == a ? "" : (a + "").replace(o, "") }, makeArray: function (a, b) { var c = b || []; return null != a && (s(Object(a)) ? n.merge(c, "string" == typeof a ? [a] : a) : g.call(c, a)), c }, inArray: function (a, b, c) { return null == b ? -1 : h.call(b, a, c) }, merge: function (a, b) { for (var c = +b.length, d = 0, e = a.length; d < c; d++)a[e++] = b[d]; return a.length = e, a }, grep: function (a, b, c) { for (var d, e = [], f = 0, g = a.length, h = !c; f < g; f++)d = !b(a[f], f), d !== h && e.push(a[f]); return e }, map: function (a, b, c) { var d, e, g = 0, h = []; if (s(a)) for (d = a.length; g < d; g++)e = b(a[g], g, c), null != e && h.push(e); else for (g in a) e = b(a[g], g, c), null != e && h.push(e); return f.apply([], h) }, guid: 1, proxy: function (a, b) { var c, d, f; if ("string" == typeof b && (c = a[b], b = a, a = c), n.isFunction(a)) return d = e.call(arguments, 2), f = function () { return a.apply(b || this, d.concat(e.call(arguments))) }, f.guid = a.guid = a.guid || n.guid++ , f }, now: Date.now, support: l }), "function" == typeof Symbol && (n.fn[Symbol.iterator] = c[Symbol.iterator]), n.each("Boolean Number String Function Array Date RegExp Object Error Symbol".split(" "), function (a, b) { i["[object " + b + "]"] = b.toLowerCase() }); function s(a) { var b = !!a && "length" in a && a.length, c = n.type(a); return "function" !== c && !n.isWindow(a) && ("array" === c || 0 === b || "number" == typeof b && b > 0 && b - 1 in a) } var t = function (a) { var b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u = "sizzle" + 1 * new Date, v = a.document, w = 0, x = 0, y = ga(), z = ga(), A = ga(), B = function (a, b) { return a === b && (l = !0), 0 }, C = 1 << 31, D = {}.hasOwnProperty, E = [], F = E.pop, G = E.push, H = E.push, I = E.slice, J = function (a, b) { for (var c = 0, d = a.length; c < d; c++)if (a[c] === b) return c; return -1 }, K = "checked|selected|async|autofocus|autoplay|controls|defer|disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped", L = "[\\x20\\t\\r\\n\\f]", M = "(?:\\\\.|[\\w-]|[^\\x00-\\xa0])+", N = "\\[" + L + "*(" + M + ")(?:" + L + "*([*^$|!~]?=)" + L + "*(?:'((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\"|(" + M + "))|)" + L + "*\\]", O = ":(" + M + ")(?:\\((('((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\")|((?:\\\\.|[^\\\\()[\\]]|" + N + ")*)|.*)\\)|)", P = new RegExp(L + "+", "g"), Q = new RegExp("^" + L + "+|((?:^|[^\\\\])(?:\\\\.)*)" + L + "+$", "g"), R = new RegExp("^" + L + "*," + L + "*"), S = new RegExp("^" + L + "*([>+~]|" + L + ")" + L + "*"), T = new RegExp("=" + L + "*([^\\]'\"]*?)" + L + "*\\]", "g"), U = new RegExp(O), V = new RegExp("^" + M + "$"), W = { ID: new RegExp("^#(" + M + ")"), CLASS: new RegExp("^\\.(" + M + ")"), TAG: new RegExp("^(" + M + "|[*])"), ATTR: new RegExp("^" + N), PSEUDO: new RegExp("^" + O), CHILD: new RegExp("^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(" + L + "*(even|odd|(([+-]|)(\\d*)n|)" + L + "*(?:([+-]|)" + L + "*(\\d+)|))" + L + "*\\)|)", "i"), bool: new RegExp("^(?:" + K + ")$", "i"), needsContext: new RegExp("^" + L + "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(" + L + "*((?:-\\d)?\\d*)" + L + "*\\)|)(?=[^-]|$)", "i") }, X = /^(?:input|select|textarea|button)$/i, Y = /^h\d$/i, Z = /^[^{]+\{\s*\[native \w/, $ = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/, _ = /[+~]/, aa = /'|\\/g, ba = new RegExp("\\\\([\\da-f]{1,6}" + L + "?|(" + L + ")|.)", "ig"), ca = function (a, b, c) { var d = "0x" + b - 65536; return d !== d || c ? b : d < 0 ? String.fromCharCode(d + 65536) : String.fromCharCode(d >> 10 | 55296, 1023 & d | 56320) }, da = function () { m() }; try { H.apply(E = I.call(v.childNodes), v.childNodes), E[v.childNodes.length].nodeType } catch (ea) { H = { apply: E.length ? function (a, b) { G.apply(a, I.call(b)) } : function (a, b) { var c = a.length, d = 0; while (a[c++] = b[d++]); a.length = c - 1 } } } function fa(a, b, d, e) { var f, h, j, k, l, o, r, s, w = b && b.ownerDocument, x = b ? b.nodeType : 9; if (d = d || [], "string" != typeof a || !a || 1 !== x && 9 !== x && 11 !== x) return d; if (!e && ((b ? b.ownerDocument || b : v) !== n && m(b), b = b || n, p)) { if (11 !== x && (o = $.exec(a))) if (f = o[1]) { if (9 === x) { if (!(j = b.getElementById(f))) return d; if (j.id === f) return d.push(j), d } else if (w && (j = w.getElementById(f)) && t(b, j) && j.id === f) return d.push(j), d } else { if (o[2]) return H.apply(d, b.getElementsByTagName(a)), d; if ((f = o[3]) && c.getElementsByClassName && b.getElementsByClassName) return H.apply(d, b.getElementsByClassName(f)), d } if (c.qsa && !A[a + " "] && (!q || !q.test(a))) { if (1 !== x) w = b, s = a; else if ("object" !== b.nodeName.toLowerCase()) { (k = b.getAttribute("id")) ? k = k.replace(aa, "\\$&") : b.setAttribute("id", k = u), r = g(a), h = r.length, l = V.test(k) ? "#" + k : "[id='" + k + "']"; while (h--) r[h] = l + " " + qa(r[h]); s = r.join(","), w = _.test(a) && oa(b.parentNode) || b } if (s) try { return H.apply(d, w.querySelectorAll(s)), d } catch (y) { } finally { k === u && b.removeAttribute("id") } } } return i(a.replace(Q, "$1"), b, d, e) } function ga() { var a = []; function b(c, e) { return a.push(c + " ") > d.cacheLength && delete b[a.shift()], b[c + " "] = e } return b } function ha(a) { return a[u] = !0, a } function ia(a) { var b = n.createElement("div"); try { return !!a(b) } catch (c) { return !1 } finally { b.parentNode && b.parentNode.removeChild(b), b = null } } function ja(a, b) { var c = a.split("|"), e = c.length; while (e--) d.attrHandle[c[e]] = b } function ka(a, b) { var c = b && a, d = c && 1 === a.nodeType && 1 === b.nodeType && (~b.sourceIndex || C) - (~a.sourceIndex || C); if (d) return d; if (c) while (c = c.nextSibling) if (c === b) return -1; return a ? 1 : -1 } function la(a) { return function (b) { var c = b.nodeName.toLowerCase(); return "input" === c && b.type === a } } function ma(a) { return function (b) { var c = b.nodeName.toLowerCase(); return ("input" === c || "button" === c) && b.type === a } } function na(a) { return ha(function (b) { return b = +b, ha(function (c, d) { var e, f = a([], c.length, b), g = f.length; while (g--) c[e = f[g]] && (c[e] = !(d[e] = c[e])) }) }) } function oa(a) { return a && "undefined" != typeof a.getElementsByTagName && a } c = fa.support = {}, f = fa.isXML = function (a) { var b = a && (a.ownerDocument || a).documentElement; return !!b && "HTML" !== b.nodeName }, m = fa.setDocument = function (a) { var b, e, g = a ? a.ownerDocument || a : v; return g !== n && 9 === g.nodeType && g.documentElement ? (n = g, o = n.documentElement, p = !f(n), (e = n.defaultView) && e.top !== e && (e.addEventListener ? e.addEventListener("unload", da, !1) : e.attachEvent && e.attachEvent("onunload", da)), c.attributes = ia(function (a) { return a.className = "i", !a.getAttribute("className") }), c.getElementsByTagName = ia(function (a) { return a.appendChild(n.createComment("")), !a.getElementsByTagName("*").length }), c.getElementsByClassName = Z.test(n.getElementsByClassName), c.getById = ia(function (a) { return o.appendChild(a).id = u, !n.getElementsByName || !n.getElementsByName(u).length }), c.getById ? (d.find.ID = function (a, b) { if ("undefined" != typeof b.getElementById && p) { var c = b.getElementById(a); return c ? [c] : [] } }, d.filter.ID = function (a) { var b = a.replace(ba, ca); return function (a) { return a.getAttribute("id") === b } }) : (delete d.find.ID, d.filter.ID = function (a) { var b = a.replace(ba, ca); return function (a) { var c = "undefined" != typeof a.getAttributeNode && a.getAttributeNode("id"); return c && c.value === b } }), d.find.TAG = c.getElementsByTagName ? function (a, b) { return "undefined" != typeof b.getElementsByTagName ? b.getElementsByTagName(a) : c.qsa ? b.querySelectorAll(a) : void 0 } : function (a, b) { var c, d = [], e = 0, f = b.getElementsByTagName(a); if ("*" === a) { while (c = f[e++]) 1 === c.nodeType && d.push(c); return d } return f }, d.find.CLASS = c.getElementsByClassName && function (a, b) { if ("undefined" != typeof b.getElementsByClassName && p) return b.getElementsByClassName(a) }, r = [], q = [], (c.qsa = Z.test(n.querySelectorAll)) && (ia(function (a) { o.appendChild(a).innerHTML = "<a id='" + u + "'></a><select id='" + u + "-\r\\' msallowcapture=''><option selected=''></option></select>", a.querySelectorAll("[msallowcapture^='']").length && q.push("[*^$]=" + L + "*(?:''|\"\")"), a.querySelectorAll("[selected]").length || q.push("\\[" + L + "*(?:value|" + K + ")"), a.querySelectorAll("[id~=" + u + "-]").length || q.push("~="), a.querySelectorAll(":checked").length || q.push(":checked"), a.querySelectorAll("a#" + u + "+*").length || q.push(".#.+[+~]") }), ia(function (a) { var b = n.createElement("input"); b.setAttribute("type", "hidden"), a.appendChild(b).setAttribute("name", "D"), a.querySelectorAll("[name=d]").length && q.push("name" + L + "*[*^$|!~]?="), a.querySelectorAll(":enabled").length || q.push(":enabled", ":disabled"), a.querySelectorAll("*,:x"), q.push(",.*:") })), (c.matchesSelector = Z.test(s = o.matches || o.webkitMatchesSelector || o.mozMatchesSelector || o.oMatchesSelector || o.msMatchesSelector)) && ia(function (a) { c.disconnectedMatch = s.call(a, "div"), s.call(a, "[s!='']:x"), r.push("!=", O) }), q = q.length && new RegExp(q.join("|")), r = r.length && new RegExp(r.join("|")), b = Z.test(o.compareDocumentPosition), t = b || Z.test(o.contains) ? function (a, b) { var c = 9 === a.nodeType ? a.documentElement : a, d = b && b.parentNode; return a === d || !(!d || 1 !== d.nodeType || !(c.contains ? c.contains(d) : a.compareDocumentPosition && 16 & a.compareDocumentPosition(d))) } : function (a, b) { if (b) while (b = b.parentNode) if (b === a) return !0; return !1 }, B = b ? function (a, b) { if (a === b) return l = !0, 0; var d = !a.compareDocumentPosition - !b.compareDocumentPosition; return d ? d : (d = (a.ownerDocument || a) === (b.ownerDocument || b) ? a.compareDocumentPosition(b) : 1, 1 & d || !c.sortDetached && b.compareDocumentPosition(a) === d ? a === n || a.ownerDocument === v && t(v, a) ? -1 : b === n || b.ownerDocument === v && t(v, b) ? 1 : k ? J(k, a) - J(k, b) : 0 : 4 & d ? -1 : 1) } : function (a, b) { if (a === b) return l = !0, 0; var c, d = 0, e = a.parentNode, f = b.parentNode, g = [a], h = [b]; if (!e || !f) return a === n ? -1 : b === n ? 1 : e ? -1 : f ? 1 : k ? J(k, a) - J(k, b) : 0; if (e === f) return ka(a, b); c = a; while (c = c.parentNode) g.unshift(c); c = b; while (c = c.parentNode) h.unshift(c); while (g[d] === h[d]) d++; return d ? ka(g[d], h[d]) : g[d] === v ? -1 : h[d] === v ? 1 : 0 }, n) : n }, fa.matches = function (a, b) { return fa(a, null, null, b) }, fa.matchesSelector = function (a, b) { if ((a.ownerDocument || a) !== n && m(a), b = b.replace(T, "='$1']"), c.matchesSelector && p && !A[b + " "] && (!r || !r.test(b)) && (!q || !q.test(b))) try { var d = s.call(a, b); if (d || c.disconnectedMatch || a.document && 11 !== a.document.nodeType) return d } catch (e) { } return fa(b, n, null, [a]).length > 0 }, fa.contains = function (a, b) { return (a.ownerDocument || a) !== n && m(a), t(a, b) }, fa.attr = function (a, b) { (a.ownerDocument || a) !== n && m(a); var e = d.attrHandle[b.toLowerCase()], f = e && D.call(d.attrHandle, b.toLowerCase()) ? e(a, b, !p) : void 0; return void 0 !== f ? f : c.attributes || !p ? a.getAttribute(b) : (f = a.getAttributeNode(b)) && f.specified ? f.value : null }, fa.error = function (a) { throw new Error("Syntax error, unrecognized expression: " + a) }, fa.uniqueSort = function (a) { var b, d = [], e = 0, f = 0; if (l = !c.detectDuplicates, k = !c.sortStable && a.slice(0), a.sort(B), l) { while (b = a[f++]) b === a[f] && (e = d.push(f)); while (e--) a.splice(d[e], 1) } return k = null, a }, e = fa.getText = function (a) { var b, c = "", d = 0, f = a.nodeType; if (f) { if (1 === f || 9 === f || 11 === f) { if ("string" == typeof a.textContent) return a.textContent; for (a = a.firstChild; a; a = a.nextSibling)c += e(a) } else if (3 === f || 4 === f) return a.nodeValue } else while (b = a[d++]) c += e(b); return c }, d = fa.selectors = { cacheLength: 50, createPseudo: ha, match: W, attrHandle: {}, find: {}, relative: { ">": { dir: "parentNode", first: !0 }, " ": { dir: "parentNode" }, "+": { dir: "previousSibling", first: !0 }, "~": { dir: "previousSibling" } }, preFilter: { ATTR: function (a) { return a[1] = a[1].replace(ba, ca), a[3] = (a[3] || a[4] || a[5] || "").replace(ba, ca), "~=" === a[2] && (a[3] = " " + a[3] + " "), a.slice(0, 4) }, CHILD: function (a) { return a[1] = a[1].toLowerCase(), "nth" === a[1].slice(0, 3) ? (a[3] || fa.error(a[0]), a[4] = +(a[4] ? a[5] + (a[6] || 1) : 2 * ("even" === a[3] || "odd" === a[3])), a[5] = +(a[7] + a[8] || "odd" === a[3])) : a[3] && fa.error(a[0]), a }, PSEUDO: function (a) { var b, c = !a[6] && a[2]; return W.CHILD.test(a[0]) ? null : (a[3] ? a[2] = a[4] || a[5] || "" : c && U.test(c) && (b = g(c, !0)) && (b = c.indexOf(")", c.length - b) - c.length) && (a[0] = a[0].slice(0, b), a[2] = c.slice(0, b)), a.slice(0, 3)) } }, filter: { TAG: function (a) { var b = a.replace(ba, ca).toLowerCase(); return "*" === a ? function () { return !0 } : function (a) { return a.nodeName && a.nodeName.toLowerCase() === b } }, CLASS: function (a) { var b = y[a + " "]; return b || (b = new RegExp("(^|" + L + ")" + a + "(" + L + "|$)")) && y(a, function (a) { return b.test("string" == typeof a.className && a.className || "undefined" != typeof a.getAttribute && a.getAttribute("class") || "") }) }, ATTR: function (a, b, c) { return function (d) { var e = fa.attr(d, a); return null == e ? "!=" === b : !b || (e += "", "=" === b ? e === c : "!=" === b ? e !== c : "^=" === b ? c && 0 === e.indexOf(c) : "*=" === b ? c && e.indexOf(c) > -1 : "$=" === b ? c && e.slice(-c.length) === c : "~=" === b ? (" " + e.replace(P, " ") + " ").indexOf(c) > -1 : "|=" === b && (e === c || e.slice(0, c.length + 1) === c + "-")) } }, CHILD: function (a, b, c, d, e) { var f = "nth" !== a.slice(0, 3), g = "last" !== a.slice(-4), h = "of-type" === b; return 1 === d && 0 === e ? function (a) { return !!a.parentNode } : function (b, c, i) { var j, k, l, m, n, o, p = f !== g ? "nextSibling" : "previousSibling", q = b.parentNode, r = h && b.nodeName.toLowerCase(), s = !i && !h, t = !1; if (q) { if (f) { while (p) { m = b; while (m = m[p]) if (h ? m.nodeName.toLowerCase() === r : 1 === m.nodeType) return !1; o = p = "only" === a && !o && "nextSibling" } return !0 } if (o = [g ? q.firstChild : q.lastChild], g && s) { m = q, l = m[u] || (m[u] = {}), k = l[m.uniqueID] || (l[m.uniqueID] = {}), j = k[a] || [], n = j[0] === w && j[1], t = n && j[2], m = n && q.childNodes[n]; while (m = ++n && m && m[p] || (t = n = 0) || o.pop()) if (1 === m.nodeType && ++t && m === b) { k[a] = [w, n, t]; break } } else if (s && (m = b, l = m[u] || (m[u] = {}), k = l[m.uniqueID] || (l[m.uniqueID] = {}), j = k[a] || [], n = j[0] === w && j[1], t = n), t === !1) while (m = ++n && m && m[p] || (t = n = 0) || o.pop()) if ((h ? m.nodeName.toLowerCase() === r : 1 === m.nodeType) && ++t && (s && (l = m[u] || (m[u] = {}), k = l[m.uniqueID] || (l[m.uniqueID] = {}), k[a] = [w, t]), m === b)) break; return t -= e, t === d || t % d === 0 && t / d >= 0 } } }, PSEUDO: function (a, b) { var c, e = d.pseudos[a] || d.setFilters[a.toLowerCase()] || fa.error("unsupported pseudo: " + a); return e[u] ? e(b) : e.length > 1 ? (c = [a, a, "", b], d.setFilters.hasOwnProperty(a.toLowerCase()) ? ha(function (a, c) { var d, f = e(a, b), g = f.length; while (g--) d = J(a, f[g]), a[d] = !(c[d] = f[g]) }) : function (a) { return e(a, 0, c) }) : e } }, pseudos: { not: ha(function (a) { var b = [], c = [], d = h(a.replace(Q, "$1")); return d[u] ? ha(function (a, b, c, e) { var f, g = d(a, null, e, []), h = a.length; while (h--) (f = g[h]) && (a[h] = !(b[h] = f)) }) : function (a, e, f) { return b[0] = a, d(b, null, f, c), b[0] = null, !c.pop() } }), has: ha(function (a) { return function (b) { return fa(a, b).length > 0 } }), contains: ha(function (a) { return a = a.replace(ba, ca), function (b) { return (b.textContent || b.innerText || e(b)).indexOf(a) > -1 } }), lang: ha(function (a) { return V.test(a || "") || fa.error("unsupported lang: " + a), a = a.replace(ba, ca).toLowerCase(), function (b) { var c; do if (c = p ? b.lang : b.getAttribute("xml:lang") || b.getAttribute("lang")) return c = c.toLowerCase(), c === a || 0 === c.indexOf(a + "-"); while ((b = b.parentNode) && 1 === b.nodeType); return !1 } }), target: function (b) { var c = a.location && a.location.hash; return c && c.slice(1) === b.id }, root: function (a) { return a === o }, focus: function (a) { return a === n.activeElement && (!n.hasFocus || n.hasFocus()) && !!(a.type || a.href || ~a.tabIndex) }, enabled: function (a) { return a.disabled === !1 }, disabled: function (a) { return a.disabled === !0 }, checked: function (a) { var b = a.nodeName.toLowerCase(); return "input" === b && !!a.checked || "option" === b && !!a.selected }, selected: function (a) { return a.parentNode && a.parentNode.selectedIndex, a.selected === !0 }, empty: function (a) { for (a = a.firstChild; a; a = a.nextSibling)if (a.nodeType < 6) return !1; return !0 }, parent: function (a) { return !d.pseudos.empty(a) }, header: function (a) { return Y.test(a.nodeName) }, input: function (a) { return X.test(a.nodeName) }, button: function (a) { var b = a.nodeName.toLowerCase(); return "input" === b && "button" === a.type || "button" === b }, text: function (a) { var b; return "input" === a.nodeName.toLowerCase() && "text" === a.type && (null == (b = a.getAttribute("type")) || "text" === b.toLowerCase()) }, first: na(function () { return [0] }), last: na(function (a, b) { return [b - 1] }), eq: na(function (a, b, c) { return [c < 0 ? c + b : c] }), even: na(function (a, b) { for (var c = 0; c < b; c += 2)a.push(c); return a }), odd: na(function (a, b) { for (var c = 1; c < b; c += 2)a.push(c); return a }), lt: na(function (a, b, c) { for (var d = c < 0 ? c + b : c; --d >= 0;)a.push(d); return a }), gt: na(function (a, b, c) { for (var d = c < 0 ? c + b : c; ++d < b;)a.push(d); return a }) } }, d.pseudos.nth = d.pseudos.eq; for (b in { radio: !0, checkbox: !0, file: !0, password: !0, image: !0 }) d.pseudos[b] = la(b); for (b in { submit: !0, reset: !0 }) d.pseudos[b] = ma(b); function pa() { } pa.prototype = d.filters = d.pseudos, d.setFilters = new pa, g = fa.tokenize = function (a, b) { var c, e, f, g, h, i, j, k = z[a + " "]; if (k) return b ? 0 : k.slice(0); h = a, i = [], j = d.preFilter; while (h) { c && !(e = R.exec(h)) || (e && (h = h.slice(e[0].length) || h), i.push(f = [])), c = !1, (e = S.exec(h)) && (c = e.shift(), f.push({ value: c, type: e[0].replace(Q, " ") }), h = h.slice(c.length)); for (g in d.filter) !(e = W[g].exec(h)) || j[g] && !(e = j[g](e)) || (c = e.shift(), f.push({ value: c, type: g, matches: e }), h = h.slice(c.length)); if (!c) break } return b ? h.length : h ? fa.error(a) : z(a, i).slice(0) }; function qa(a) { for (var b = 0, c = a.length, d = ""; b < c; b++)d += a[b].value; return d } function ra(a, b, c) { var d = b.dir, e = c && "parentNode" === d, f = x++; return b.first ? function (b, c, f) { while (b = b[d]) if (1 === b.nodeType || e) return a(b, c, f) } : function (b, c, g) { var h, i, j, k = [w, f]; if (g) { while (b = b[d]) if ((1 === b.nodeType || e) && a(b, c, g)) return !0 } else while (b = b[d]) if (1 === b.nodeType || e) { if (j = b[u] || (b[u] = {}), i = j[b.uniqueID] || (j[b.uniqueID] = {}), (h = i[d]) && h[0] === w && h[1] === f) return k[2] = h[2]; if (i[d] = k, k[2] = a(b, c, g)) return !0 } } } function sa(a) { return a.length > 1 ? function (b, c, d) { var e = a.length; while (e--) if (!a[e](b, c, d)) return !1; return !0 } : a[0] } function ta(a, b, c) { for (var d = 0, e = b.length; d < e; d++)fa(a, b[d], c); return c } function ua(a, b, c, d, e) { for (var f, g = [], h = 0, i = a.length, j = null != b; h < i; h++)(f = a[h]) && (c && !c(f, d, e) || (g.push(f), j && b.push(h))); return g } function va(a, b, c, d, e, f) { return d && !d[u] && (d = va(d)), e && !e[u] && (e = va(e, f)), ha(function (f, g, h, i) { var j, k, l, m = [], n = [], o = g.length, p = f || ta(b || "*", h.nodeType ? [h] : h, []), q = !a || !f && b ? p : ua(p, m, a, h, i), r = c ? e || (f ? a : o || d) ? [] : g : q; if (c && c(q, r, h, i), d) { j = ua(r, n), d(j, [], h, i), k = j.length; while (k--) (l = j[k]) && (r[n[k]] = !(q[n[k]] = l)) } if (f) { if (e || a) { if (e) { j = [], k = r.length; while (k--) (l = r[k]) && j.push(q[k] = l); e(null, r = [], j, i) } k = r.length; while (k--) (l = r[k]) && (j = e ? J(f, l) : m[k]) > -1 && (f[j] = !(g[j] = l)) } } else r = ua(r === g ? r.splice(o, r.length) : r), e ? e(null, g, r, i) : H.apply(g, r) }) } function wa(a) { for (var b, c, e, f = a.length, g = d.relative[a[0].type], h = g || d.relative[" "], i = g ? 1 : 0, k = ra(function (a) { return a === b }, h, !0), l = ra(function (a) { return J(b, a) > -1 }, h, !0), m = [function (a, c, d) { var e = !g && (d || c !== j) || ((b = c).nodeType ? k(a, c, d) : l(a, c, d)); return b = null, e }]; i < f; i++)if (c = d.relative[a[i].type]) m = [ra(sa(m), c)]; else { if (c = d.filter[a[i].type].apply(null, a[i].matches), c[u]) { for (e = ++i; e < f; e++)if (d.relative[a[e].type]) break; return va(i > 1 && sa(m), i > 1 && qa(a.slice(0, i - 1).concat({ value: " " === a[i - 2].type ? "*" : "" })).replace(Q, "$1"), c, i < e && wa(a.slice(i, e)), e < f && wa(a = a.slice(e)), e < f && qa(a)) } m.push(c) } return sa(m) } function xa(a, b) { var c = b.length > 0, e = a.length > 0, f = function (f, g, h, i, k) { var l, o, q, r = 0, s = "0", t = f && [], u = [], v = j, x = f || e && d.find.TAG("*", k), y = w += null == v ? 1 : Math.random() || .1, z = x.length; for (k && (j = g === n || g || k); s !== z && null != (l = x[s]); s++) { if (e && l) { o = 0, g || l.ownerDocument === n || (m(l), h = !p); while (q = a[o++]) if (q(l, g || n, h)) { i.push(l); break } k && (w = y) } c && ((l = !q && l) && r-- , f && t.push(l)) } if (r += s, c && s !== r) { o = 0; while (q = b[o++]) q(t, u, g, h); if (f) { if (r > 0) while (s--) t[s] || u[s] || (u[s] = F.call(i)); u = ua(u) } H.apply(i, u), k && !f && u.length > 0 && r + b.length > 1 && fa.uniqueSort(i) } return k && (w = y, j = v), t }; return c ? ha(f) : f } return h = fa.compile = function (a, b) { var c, d = [], e = [], f = A[a + " "]; if (!f) { b || (b = g(a)), c = b.length; while (c--) f = wa(b[c]), f[u] ? d.push(f) : e.push(f); f = A(a, xa(e, d)), f.selector = a } return f }, i = fa.select = function (a, b, e, f) { var i, j, k, l, m, n = "function" == typeof a && a, o = !f && g(a = n.selector || a); if (e = e || [], 1 === o.length) { if (j = o[0] = o[0].slice(0), j.length > 2 && "ID" === (k = j[0]).type && c.getById && 9 === b.nodeType && p && d.relative[j[1].type]) { if (b = (d.find.ID(k.matches[0].replace(ba, ca), b) || [])[0], !b) return e; n && (b = b.parentNode), a = a.slice(j.shift().value.length) } i = W.needsContext.test(a) ? 0 : j.length; while (i--) { if (k = j[i], d.relative[l = k.type]) break; if ((m = d.find[l]) && (f = m(k.matches[0].replace(ba, ca), _.test(j[0].type) && oa(b.parentNode) || b))) { if (j.splice(i, 1), a = f.length && qa(j), !a) return H.apply(e, f), e; break } } } return (n || h(a, o))(f, b, !p, e, !b || _.test(a) && oa(b.parentNode) || b), e }, c.sortStable = u.split("").sort(B).join("") === u, c.detectDuplicates = !!l, m(), c.sortDetached = ia(function (a) { return 1 & a.compareDocumentPosition(n.createElement("div")) }), ia(function (a) { return a.innerHTML = "<a href='#'></a>", "#" === a.firstChild.getAttribute("href") }) || ja("type|href|height|width", function (a, b, c) { if (!c) return a.getAttribute(b, "type" === b.toLowerCase() ? 1 : 2) }), c.attributes && ia(function (a) { return a.innerHTML = "<input/>", a.firstChild.setAttribute("value", ""), "" === a.firstChild.getAttribute("value") }) || ja("value", function (a, b, c) { if (!c && "input" === a.nodeName.toLowerCase()) return a.defaultValue }), ia(function (a) { return null == a.getAttribute("disabled") }) || ja(K, function (a, b, c) { var d; if (!c) return a[b] === !0 ? b.toLowerCase() : (d = a.getAttributeNode(b)) && d.specified ? d.value : null }), fa }(a); n.find = t, n.expr = t.selectors, n.expr[":"] = n.expr.pseudos, n.uniqueSort = n.unique = t.uniqueSort, n.text = t.getText, n.isXMLDoc = t.isXML, n.contains = t.contains; var u = function (a, b, c) { var d = [], e = void 0 !== c; while ((a = a[b]) && 9 !== a.nodeType) if (1 === a.nodeType) { if (e && n(a).is(c)) break; d.push(a) } return d }, v = function (a, b) { for (var c = []; a; a = a.nextSibling)1 === a.nodeType && a !== b && c.push(a); return c }, w = n.expr.match.needsContext, x = /^<([\w-]+)\s*\/?>(?:<\/\1>|)$/, y = /^.[^:#\[\.,]*$/; function z(a, b, c) { if (n.isFunction(b)) return n.grep(a, function (a, d) { return !!b.call(a, d, a) !== c }); if (b.nodeType) return n.grep(a, function (a) { return a === b !== c }); if ("string" == typeof b) { if (y.test(b)) return n.filter(b, a, c); b = n.filter(b, a) } return n.grep(a, function (a) { return h.call(b, a) > -1 !== c }) } n.filter = function (a, b, c) { var d = b[0]; return c && (a = ":not(" + a + ")"), 1 === b.length && 1 === d.nodeType ? n.find.matchesSelector(d, a) ? [d] : [] : n.find.matches(a, n.grep(b, function (a) { return 1 === a.nodeType })) }, n.fn.extend({ find: function (a) { var b, c = this.length, d = [], e = this; if ("string" != typeof a) return this.pushStack(n(a).filter(function () { for (b = 0; b < c; b++)if (n.contains(e[b], this)) return !0 })); for (b = 0; b < c; b++)n.find(a, e[b], d); return d = this.pushStack(c > 1 ? n.unique(d) : d), d.selector = this.selector ? this.selector + " " + a : a, d }, filter: function (a) { return this.pushStack(z(this, a || [], !1)) }, not: function (a) { return this.pushStack(z(this, a || [], !0)) }, is: function (a) { return !!z(this, "string" == typeof a && w.test(a) ? n(a) : a || [], !1).length } }); var A, B = /^(?:\s*(<[\w\W]+>)[^>]*|#([\w-]*))$/, C = n.fn.init = function (a, b, c) { var e, f; if (!a) return this; if (c = c || A, "string" == typeof a) { if (e = "<" === a[0] && ">" === a[a.length - 1] && a.length >= 3 ? [null, a, null] : B.exec(a), !e || !e[1] && b) return !b || b.jquery ? (b || c).find(a) : this.constructor(b).find(a); if (e[1]) { if (b = b instanceof n ? b[0] : b, n.merge(this, n.parseHTML(e[1], b && b.nodeType ? b.ownerDocument || b : d, !0)), x.test(e[1]) && n.isPlainObject(b)) for (e in b) n.isFunction(this[e]) ? this[e](b[e]) : this.attr(e, b[e]); return this } return f = d.getElementById(e[2]), f && f.parentNode && (this.length = 1, this[0] = f), this.context = d, this.selector = a, this } return a.nodeType ? (this.context = this[0] = a, this.length = 1, this) : n.isFunction(a) ? void 0 !== c.ready ? c.ready(a) : a(n) : (void 0 !== a.selector && (this.selector = a.selector, this.context = a.context), n.makeArray(a, this)) }; C.prototype = n.fn, A = n(d); var D = /^(?:parents|prev(?:Until|All))/, E = { children: !0, contents: !0, next: !0, prev: !0 }; n.fn.extend({ has: function (a) { var b = n(a, this), c = b.length; return this.filter(function () { for (var a = 0; a < c; a++)if (n.contains(this, b[a])) return !0 }) }, closest: function (a, b) { for (var c, d = 0, e = this.length, f = [], g = w.test(a) || "string" != typeof a ? n(a, b || this.context) : 0; d < e; d++)for (c = this[d]; c && c !== b; c = c.parentNode)if (c.nodeType < 11 && (g ? g.index(c) > -1 : 1 === c.nodeType && n.find.matchesSelector(c, a))) { f.push(c); break } return this.pushStack(f.length > 1 ? n.uniqueSort(f) : f) }, index: function (a) { return a ? "string" == typeof a ? h.call(n(a), this[0]) : h.call(this, a.jquery ? a[0] : a) : this[0] && this[0].parentNode ? this.first().prevAll().length : -1 }, add: function (a, b) { return this.pushStack(n.uniqueSort(n.merge(this.get(), n(a, b)))) }, addBack: function (a) { return this.add(null == a ? this.prevObject : this.prevObject.filter(a)) } }); function F(a, b) { while ((a = a[b]) && 1 !== a.nodeType); return a } n.each({ parent: function (a) { var b = a.parentNode; return b && 11 !== b.nodeType ? b : null }, parents: function (a) { return u(a, "parentNode") }, parentsUntil: function (a, b, c) { return u(a, "parentNode", c) }, next: function (a) { return F(a, "nextSibling") }, prev: function (a) { return F(a, "previousSibling") }, nextAll: function (a) { return u(a, "nextSibling") }, prevAll: function (a) { return u(a, "previousSibling") }, nextUntil: function (a, b, c) { return u(a, "nextSibling", c) }, prevUntil: function (a, b, c) { return u(a, "previousSibling", c) }, siblings: function (a) { return v((a.parentNode || {}).firstChild, a) }, children: function (a) { return v(a.firstChild) }, contents: function (a) { return a.contentDocument || n.merge([], a.childNodes) } }, function (a, b) { n.fn[a] = function (c, d) { var e = n.map(this, b, c); return "Until" !== a.slice(-5) && (d = c), d && "string" == typeof d && (e = n.filter(d, e)), this.length > 1 && (E[a] || n.uniqueSort(e), D.test(a) && e.reverse()), this.pushStack(e) } }); var G = /\S+/g; function H(a) { var b = {}; return n.each(a.match(G) || [], function (a, c) { b[c] = !0 }), b } n.Callbacks = function (a) { a = "string" == typeof a ? H(a) : n.extend({}, a); var b, c, d, e, f = [], g = [], h = -1, i = function () { for (e = a.once, d = b = !0; g.length; h = -1) { c = g.shift(); while (++h < f.length) f[h].apply(c[0], c[1]) === !1 && a.stopOnFalse && (h = f.length, c = !1) } a.memory || (c = !1), b = !1, e && (f = c ? [] : "") }, j = { add: function () { return f && (c && !b && (h = f.length - 1, g.push(c)), function d(b) { n.each(b, function (b, c) { n.isFunction(c) ? a.unique && j.has(c) || f.push(c) : c && c.length && "string" !== n.type(c) && d(c) }) }(arguments), c && !b && i()), this }, remove: function () { return n.each(arguments, function (a, b) { var c; while ((c = n.inArray(b, f, c)) > -1) f.splice(c, 1), c <= h && h-- }), this }, has: function (a) { return a ? n.inArray(a, f) > -1 : f.length > 0 }, empty: function () { return f && (f = []), this }, disable: function () { return e = g = [], f = c = "", this }, disabled: function () { return !f }, lock: function () { return e = g = [], c || (f = c = ""), this }, locked: function () { return !!e }, fireWith: function (a, c) { return e || (c = c || [], c = [a, c.slice ? c.slice() : c], g.push(c), b || i()), this }, fire: function () { return j.fireWith(this, arguments), this }, fired: function () { return !!d } }; return j }, n.extend({ Deferred: function (a) { var b = [["resolve", "done", n.Callbacks("once memory"), "resolved"], ["reject", "fail", n.Callbacks("once memory"), "rejected"], ["notify", "progress", n.Callbacks("memory")]], c = "pending", d = { state: function () { return c }, always: function () { return e.done(arguments).fail(arguments), this }, then: function () { var a = arguments; return n.Deferred(function (c) { n.each(b, function (b, f) { var g = n.isFunction(a[b]) && a[b]; e[f[1]](function () { var a = g && g.apply(this, arguments); a && n.isFunction(a.promise) ? a.promise().progress(c.notify).done(c.resolve).fail(c.reject) : c[f[0] + "With"](this === d ? c.promise() : this, g ? [a] : arguments) }) }), a = null }).promise() }, promise: function (a) { return null != a ? n.extend(a, d) : d } }, e = {}; return d.pipe = d.then, n.each(b, function (a, f) { var g = f[2], h = f[3]; d[f[1]] = g.add, h && g.add(function () { c = h }, b[1 ^ a][2].disable, b[2][2].lock), e[f[0]] = function () { return e[f[0] + "With"](this === e ? d : this, arguments), this }, e[f[0] + "With"] = g.fireWith }), d.promise(e), a && a.call(e, e), e }, when: function (a) { var b = 0, c = e.call(arguments), d = c.length, f = 1 !== d || a && n.isFunction(a.promise) ? d : 0, g = 1 === f ? a : n.Deferred(), h = function (a, b, c) { return function (d) { b[a] = this, c[a] = arguments.length > 1 ? e.call(arguments) : d, c === i ? g.notifyWith(b, c) : --f || g.resolveWith(b, c) } }, i, j, k; if (d > 1) for (i = new Array(d), j = new Array(d), k = new Array(d); b < d; b++)c[b] && n.isFunction(c[b].promise) ? c[b].promise().progress(h(b, j, i)).done(h(b, k, c)).fail(g.reject) : --f; return f || g.resolveWith(k, c), g.promise() } }); var I; n.fn.ready = function (a) { return n.ready.promise().done(a), this }, n.extend({ isReady: !1, readyWait: 1, holdReady: function (a) { a ? n.readyWait++ : n.ready(!0) }, ready: function (a) { (a === !0 ? --n.readyWait : n.isReady) || (n.isReady = !0, a !== !0 && --n.readyWait > 0 || (I.resolveWith(d, [n]), n.fn.triggerHandler && (n(d).triggerHandler("ready"), n(d).off("ready")))) } }); function J() { d.removeEventListener("DOMContentLoaded", J), a.removeEventListener("load", J), n.ready() } n.ready.promise = function (b) { return I || (I = n.Deferred(), "complete" === d.readyState || "loading" !== d.readyState && !d.documentElement.doScroll ? a.setTimeout(n.ready) : (d.addEventListener("DOMContentLoaded", J), a.addEventListener("load", J))), I.promise(b) }, n.ready.promise(); var K = function (a, b, c, d, e, f, g) { var h = 0, i = a.length, j = null == c; if ("object" === n.type(c)) { e = !0; for (h in c) K(a, b, h, c[h], !0, f, g) } else if (void 0 !== d && (e = !0, n.isFunction(d) || (g = !0), j && (g ? (b.call(a, d), b = null) : (j = b, b = function (a, b, c) { return j.call(n(a), c) })), b)) for (; h < i; h++)b(a[h], c, g ? d : d.call(a[h], h, b(a[h], c))); return e ? a : j ? b.call(a) : i ? b(a[0], c) : f }, L = function (a) { return 1 === a.nodeType || 9 === a.nodeType || !+a.nodeType }; function M() { this.expando = n.expando + M.uid++ } M.uid = 1, M.prototype = { register: function (a, b) { var c = b || {}; return a.nodeType ? a[this.expando] = c : Object.defineProperty(a, this.expando, { value: c, writable: !0, configurable: !0 }), a[this.expando] }, cache: function (a) { if (!L(a)) return {}; var b = a[this.expando]; return b || (b = {}, L(a) && (a.nodeType ? a[this.expando] = b : Object.defineProperty(a, this.expando, { value: b, configurable: !0 }))), b }, set: function (a, b, c) { var d, e = this.cache(a); if ("string" == typeof b) e[b] = c; else for (d in b) e[d] = b[d]; return e }, get: function (a, b) { return void 0 === b ? this.cache(a) : a[this.expando] && a[this.expando][b] }, access: function (a, b, c) { var d; return void 0 === b || b && "string" == typeof b && void 0 === c ? (d = this.get(a, b), void 0 !== d ? d : this.get(a, n.camelCase(b))) : (this.set(a, b, c), void 0 !== c ? c : b) }, remove: function (a, b) { var c, d, e, f = a[this.expando]; if (void 0 !== f) { if (void 0 === b) this.register(a); else { n.isArray(b) ? d = b.concat(b.map(n.camelCase)) : (e = n.camelCase(b), b in f ? d = [b, e] : (d = e, d = d in f ? [d] : d.match(G) || [])), c = d.length; while (c--) delete f[d[c]] } (void 0 === b || n.isEmptyObject(f)) && (a.nodeType ? a[this.expando] = void 0 : delete a[this.expando]) } }, hasData: function (a) { var b = a[this.expando]; return void 0 !== b && !n.isEmptyObject(b) } }; var N = new M, O = new M, P = /^(?:\{[\w\W]*\}|\[[\w\W]*\])$/, Q = /[A-Z]/g; function R(a, b, c) {
        var d; if (void 0 === c && 1 === a.nodeType) if (d = "data-" + b.replace(Q, "-$&").toLowerCase(), c = a.getAttribute(d), "string" == typeof c) {
            try { c = "true" === c || "false" !== c && ("null" === c ? null : +c + "" === c ? +c : P.test(c) ? n.parseJSON(c) : c) } catch (e) { } O.set(a, b, c)
        } else c = void 0; return c
    } n.extend({ hasData: function (a) { return O.hasData(a) || N.hasData(a) }, data: function (a, b, c) { return O.access(a, b, c) }, removeData: function (a, b) { O.remove(a, b) }, _data: function (a, b, c) { return N.access(a, b, c) }, _removeData: function (a, b) { N.remove(a, b) } }), n.fn.extend({ data: function (a, b) { var c, d, e, f = this[0], g = f && f.attributes; if (void 0 === a) { if (this.length && (e = O.get(f), 1 === f.nodeType && !N.get(f, "hasDataAttrs"))) { c = g.length; while (c--) g[c] && (d = g[c].name, 0 === d.indexOf("data-") && (d = n.camelCase(d.slice(5)), R(f, d, e[d]))); N.set(f, "hasDataAttrs", !0) } return e } return "object" == typeof a ? this.each(function () { O.set(this, a) }) : K(this, function (b) { var c, d; if (f && void 0 === b) { if (c = O.get(f, a) || O.get(f, a.replace(Q, "-$&").toLowerCase()), void 0 !== c) return c; if (d = n.camelCase(a), c = O.get(f, d), void 0 !== c) return c; if (c = R(f, d, void 0), void 0 !== c) return c } else d = n.camelCase(a), this.each(function () { var c = O.get(this, d); O.set(this, d, b), a.indexOf("-") > -1 && void 0 !== c && O.set(this, a, b) }) }, null, b, arguments.length > 1, null, !0) }, removeData: function (a) { return this.each(function () { O.remove(this, a) }) } }), n.extend({ queue: function (a, b, c) { var d; if (a) return b = (b || "fx") + "queue", d = N.get(a, b), c && (!d || n.isArray(c) ? d = N.access(a, b, n.makeArray(c)) : d.push(c)), d || [] }, dequeue: function (a, b) { b = b || "fx"; var c = n.queue(a, b), d = c.length, e = c.shift(), f = n._queueHooks(a, b), g = function () { n.dequeue(a, b) }; "inprogress" === e && (e = c.shift(), d--), e && ("fx" === b && c.unshift("inprogress"), delete f.stop, e.call(a, g, f)), !d && f && f.empty.fire() }, _queueHooks: function (a, b) { var c = b + "queueHooks"; return N.get(a, c) || N.access(a, c, { empty: n.Callbacks("once memory").add(function () { N.remove(a, [b + "queue", c]) }) }) } }), n.fn.extend({ queue: function (a, b) { var c = 2; return "string" != typeof a && (b = a, a = "fx", c--), arguments.length < c ? n.queue(this[0], a) : void 0 === b ? this : this.each(function () { var c = n.queue(this, a, b); n._queueHooks(this, a), "fx" === a && "inprogress" !== c[0] && n.dequeue(this, a) }) }, dequeue: function (a) { return this.each(function () { n.dequeue(this, a) }) }, clearQueue: function (a) { return this.queue(a || "fx", []) }, promise: function (a, b) { var c, d = 1, e = n.Deferred(), f = this, g = this.length, h = function () { --d || e.resolveWith(f, [f]) }; "string" != typeof a && (b = a, a = void 0), a = a || "fx"; while (g--) c = N.get(f[g], a + "queueHooks"), c && c.empty && (d++ , c.empty.add(h)); return h(), e.promise(b) } }); var S = /[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source, T = new RegExp("^(?:([+-])=|)(" + S + ")([a-z%]*)$", "i"), U = ["Top", "Right", "Bottom", "Left"], V = function (a, b) { return a = b || a, "none" === n.css(a, "display") || !n.contains(a.ownerDocument, a) }; function W(a, b, c, d) { var e, f = 1, g = 20, h = d ? function () { return d.cur() } : function () { return n.css(a, b, "") }, i = h(), j = c && c[3] || (n.cssNumber[b] ? "" : "px"), k = (n.cssNumber[b] || "px" !== j && +i) && T.exec(n.css(a, b)); if (k && k[3] !== j) { j = j || k[3], c = c || [], k = +i || 1; do f = f || ".5", k /= f, n.style(a, b, k + j); while (f !== (f = h() / i) && 1 !== f && --g) } return c && (k = +k || +i || 0, e = c[1] ? k + (c[1] + 1) * c[2] : +c[2], d && (d.unit = j, d.start = k, d.end = e)), e } var X = /^(?:checkbox|radio)$/i, Y = /<([\w:-]+)/, Z = /^$|\/(?:java|ecma)script/i, $ = { option: [1, "<select multiple='multiple'>", "</select>"], thead: [1, "<table>", "</table>"], col: [2, "<table><colgroup>", "</colgroup></table>"], tr: [2, "<table><tbody>", "</tbody></table>"], td: [3, "<table><tbody><tr>", "</tr></tbody></table>"], _default: [0, "", ""] }; $.optgroup = $.option, $.tbody = $.tfoot = $.colgroup = $.caption = $.thead, $.th = $.td; function _(a, b) { var c = "undefined" != typeof a.getElementsByTagName ? a.getElementsByTagName(b || "*") : "undefined" != typeof a.querySelectorAll ? a.querySelectorAll(b || "*") : []; return void 0 === b || b && n.nodeName(a, b) ? n.merge([a], c) : c } function aa(a, b) { for (var c = 0, d = a.length; c < d; c++)N.set(a[c], "globalEval", !b || N.get(b[c], "globalEval")) } var ba = /<|&#?\w+;/; function ca(a, b, c, d, e) { for (var f, g, h, i, j, k, l = b.createDocumentFragment(), m = [], o = 0, p = a.length; o < p; o++)if (f = a[o], f || 0 === f) if ("object" === n.type(f)) n.merge(m, f.nodeType ? [f] : f); else if (ba.test(f)) { g = g || l.appendChild(b.createElement("div")), h = (Y.exec(f) || ["", ""])[1].toLowerCase(), i = $[h] || $._default, g.innerHTML = i[1] + n.htmlPrefilter(f) + i[2], k = i[0]; while (k--) g = g.lastChild; n.merge(m, g.childNodes), g = l.firstChild, g.textContent = "" } else m.push(b.createTextNode(f)); l.textContent = "", o = 0; while (f = m[o++]) if (d && n.inArray(f, d) > -1) e && e.push(f); else if (j = n.contains(f.ownerDocument, f), g = _(l.appendChild(f), "script"), j && aa(g), c) { k = 0; while (f = g[k++]) Z.test(f.type || "") && c.push(f) } return l } !function () { var a = d.createDocumentFragment(), b = a.appendChild(d.createElement("div")), c = d.createElement("input"); c.setAttribute("type", "radio"), c.setAttribute("checked", "checked"), c.setAttribute("name", "t"), b.appendChild(c), l.checkClone = b.cloneNode(!0).cloneNode(!0).lastChild.checked, b.innerHTML = "<textarea>x</textarea>", l.noCloneChecked = !!b.cloneNode(!0).lastChild.defaultValue }(); var da = /^key/, ea = /^(?:mouse|pointer|contextmenu|drag|drop)|click/, fa = /^([^.]*)(?:\.(.+)|)/; function ga() { return !0 } function ha() { return !1 } function ia() { try { return d.activeElement } catch (a) { } } function ja(a, b, c, d, e, f) { var g, h; if ("object" == typeof b) { "string" != typeof c && (d = d || c, c = void 0); for (h in b) ja(a, h, c, d, b[h], f); return a } if (null == d && null == e ? (e = c, d = c = void 0) : null == e && ("string" == typeof c ? (e = d, d = void 0) : (e = d, d = c, c = void 0)), e === !1) e = ha; else if (!e) return a; return 1 === f && (g = e, e = function (a) { return n().off(a), g.apply(this, arguments) }, e.guid = g.guid || (g.guid = n.guid++)), a.each(function () { n.event.add(this, b, e, d, c) }) } n.event = { global: {}, add: function (a, b, c, d, e) { var f, g, h, i, j, k, l, m, o, p, q, r = N.get(a); if (r) { c.handler && (f = c, c = f.handler, e = f.selector), c.guid || (c.guid = n.guid++), (i = r.events) || (i = r.events = {}), (g = r.handle) || (g = r.handle = function (b) { return "undefined" != typeof n && n.event.triggered !== b.type ? n.event.dispatch.apply(a, arguments) : void 0 }), b = (b || "").match(G) || [""], j = b.length; while (j--) h = fa.exec(b[j]) || [], o = q = h[1], p = (h[2] || "").split(".").sort(), o && (l = n.event.special[o] || {}, o = (e ? l.delegateType : l.bindType) || o, l = n.event.special[o] || {}, k = n.extend({ type: o, origType: q, data: d, handler: c, guid: c.guid, selector: e, needsContext: e && n.expr.match.needsContext.test(e), namespace: p.join(".") }, f), (m = i[o]) || (m = i[o] = [], m.delegateCount = 0, l.setup && l.setup.call(a, d, p, g) !== !1 || a.addEventListener && a.addEventListener(o, g)), l.add && (l.add.call(a, k), k.handler.guid || (k.handler.guid = c.guid)), e ? m.splice(m.delegateCount++, 0, k) : m.push(k), n.event.global[o] = !0) } }, remove: function (a, b, c, d, e) { var f, g, h, i, j, k, l, m, o, p, q, r = N.hasData(a) && N.get(a); if (r && (i = r.events)) { b = (b || "").match(G) || [""], j = b.length; while (j--) if (h = fa.exec(b[j]) || [], o = q = h[1], p = (h[2] || "").split(".").sort(), o) { l = n.event.special[o] || {}, o = (d ? l.delegateType : l.bindType) || o, m = i[o] || [], h = h[2] && new RegExp("(^|\\.)" + p.join("\\.(?:.*\\.|)") + "(\\.|$)"), g = f = m.length; while (f--) k = m[f], !e && q !== k.origType || c && c.guid !== k.guid || h && !h.test(k.namespace) || d && d !== k.selector && ("**" !== d || !k.selector) || (m.splice(f, 1), k.selector && m.delegateCount-- , l.remove && l.remove.call(a, k)); g && !m.length && (l.teardown && l.teardown.call(a, p, r.handle) !== !1 || n.removeEvent(a, o, r.handle), delete i[o]) } else for (o in i) n.event.remove(a, o + b[j], c, d, !0); n.isEmptyObject(i) && N.remove(a, "handle events") } }, dispatch: function (a) { a = n.event.fix(a); var b, c, d, f, g, h = [], i = e.call(arguments), j = (N.get(this, "events") || {})[a.type] || [], k = n.event.special[a.type] || {}; if (i[0] = a, a.delegateTarget = this, !k.preDispatch || k.preDispatch.call(this, a) !== !1) { h = n.event.handlers.call(this, a, j), b = 0; while ((f = h[b++]) && !a.isPropagationStopped()) { a.currentTarget = f.elem, c = 0; while ((g = f.handlers[c++]) && !a.isImmediatePropagationStopped()) a.rnamespace && !a.rnamespace.test(g.namespace) || (a.handleObj = g, a.data = g.data, d = ((n.event.special[g.origType] || {}).handle || g.handler).apply(f.elem, i), void 0 !== d && (a.result = d) === !1 && (a.preventDefault(), a.stopPropagation())) } return k.postDispatch && k.postDispatch.call(this, a), a.result } }, handlers: function (a, b) { var c, d, e, f, g = [], h = b.delegateCount, i = a.target; if (h && i.nodeType && ("click" !== a.type || isNaN(a.button) || a.button < 1)) for (; i !== this; i = i.parentNode || this)if (1 === i.nodeType && (i.disabled !== !0 || "click" !== a.type)) { for (d = [], c = 0; c < h; c++)f = b[c], e = f.selector + " ", void 0 === d[e] && (d[e] = f.needsContext ? n(e, this).index(i) > -1 : n.find(e, this, null, [i]).length), d[e] && d.push(f); d.length && g.push({ elem: i, handlers: d }) } return h < b.length && g.push({ elem: this, handlers: b.slice(h) }), g }, props: "altKey bubbles cancelable ctrlKey currentTarget detail eventPhase metaKey relatedTarget shiftKey target timeStamp view which".split(" "), fixHooks: {}, keyHooks: { props: "char charCode key keyCode".split(" "), filter: function (a, b) { return null == a.which && (a.which = null != b.charCode ? b.charCode : b.keyCode), a } }, mouseHooks: { props: "button buttons clientX clientY offsetX offsetY pageX pageY screenX screenY toElement".split(" "), filter: function (a, b) { var c, e, f, g = b.button; return null == a.pageX && null != b.clientX && (c = a.target.ownerDocument || d, e = c.documentElement, f = c.body, a.pageX = b.clientX + (e && e.scrollLeft || f && f.scrollLeft || 0) - (e && e.clientLeft || f && f.clientLeft || 0), a.pageY = b.clientY + (e && e.scrollTop || f && f.scrollTop || 0) - (e && e.clientTop || f && f.clientTop || 0)), a.which || void 0 === g || (a.which = 1 & g ? 1 : 2 & g ? 3 : 4 & g ? 2 : 0), a } }, fix: function (a) { if (a[n.expando]) return a; var b, c, e, f = a.type, g = a, h = this.fixHooks[f]; h || (this.fixHooks[f] = h = ea.test(f) ? this.mouseHooks : da.test(f) ? this.keyHooks : {}), e = h.props ? this.props.concat(h.props) : this.props, a = new n.Event(g), b = e.length; while (b--) c = e[b], a[c] = g[c]; return a.target || (a.target = d), 3 === a.target.nodeType && (a.target = a.target.parentNode), h.filter ? h.filter(a, g) : a }, special: { load: { noBubble: !0 }, focus: { trigger: function () { if (this !== ia() && this.focus) return this.focus(), !1 }, delegateType: "focusin" }, blur: { trigger: function () { if (this === ia() && this.blur) return this.blur(), !1 }, delegateType: "focusout" }, click: { trigger: function () { if ("checkbox" === this.type && this.click && n.nodeName(this, "input")) return this.click(), !1 }, _default: function (a) { return n.nodeName(a.target, "a") } }, beforeunload: { postDispatch: function (a) { void 0 !== a.result && a.originalEvent && (a.originalEvent.returnValue = a.result) } } } }, n.removeEvent = function (a, b, c) { a.removeEventListener && a.removeEventListener(b, c) }, n.Event = function (a, b) { return this instanceof n.Event ? (a && a.type ? (this.originalEvent = a, this.type = a.type, this.isDefaultPrevented = a.defaultPrevented || void 0 === a.defaultPrevented && a.returnValue === !1 ? ga : ha) : this.type = a, b && n.extend(this, b), this.timeStamp = a && a.timeStamp || n.now(), void (this[n.expando] = !0)) : new n.Event(a, b) }, n.Event.prototype = { constructor: n.Event, isDefaultPrevented: ha, isPropagationStopped: ha, isImmediatePropagationStopped: ha, isSimulated: !1, preventDefault: function () { var a = this.originalEvent; this.isDefaultPrevented = ga, a && !this.isSimulated && a.preventDefault() }, stopPropagation: function () { var a = this.originalEvent; this.isPropagationStopped = ga, a && !this.isSimulated && a.stopPropagation() }, stopImmediatePropagation: function () { var a = this.originalEvent; this.isImmediatePropagationStopped = ga, a && !this.isSimulated && a.stopImmediatePropagation(), this.stopPropagation() } }, n.each({ mouseenter: "mouseover", mouseleave: "mouseout", pointerenter: "pointerover", pointerleave: "pointerout" }, function (a, b) { n.event.special[a] = { delegateType: b, bindType: b, handle: function (a) { var c, d = this, e = a.relatedTarget, f = a.handleObj; return e && (e === d || n.contains(d, e)) || (a.type = f.origType, c = f.handler.apply(this, arguments), a.type = b), c } } }), n.fn.extend({ on: function (a, b, c, d) { return ja(this, a, b, c, d) }, one: function (a, b, c, d) { return ja(this, a, b, c, d, 1) }, off: function (a, b, c) { var d, e; if (a && a.preventDefault && a.handleObj) return d = a.handleObj, n(a.delegateTarget).off(d.namespace ? d.origType + "." + d.namespace : d.origType, d.selector, d.handler), this; if ("object" == typeof a) { for (e in a) this.off(e, b, a[e]); return this } return b !== !1 && "function" != typeof b || (c = b, b = void 0), c === !1 && (c = ha), this.each(function () { n.event.remove(this, a, c, b) }) } }); var ka = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:-]+)[^>]*)\/>/gi, la = /<script|<style|<link/i, ma = /checked\s*(?:[^=]|=\s*.checked.)/i, na = /^true\/(.*)/, oa = /^\s*<!(?:\[CDATA\[|--)|(?:\]\]|--)>\s*$/g; function pa(a, b) { return n.nodeName(a, "table") && n.nodeName(11 !== b.nodeType ? b : b.firstChild, "tr") ? a.getElementsByTagName("tbody")[0] || a.appendChild(a.ownerDocument.createElement("tbody")) : a } function qa(a) { return a.type = (null !== a.getAttribute("type")) + "/" + a.type, a } function ra(a) { var b = na.exec(a.type); return b ? a.type = b[1] : a.removeAttribute("type"), a } function sa(a, b) { var c, d, e, f, g, h, i, j; if (1 === b.nodeType) { if (N.hasData(a) && (f = N.access(a), g = N.set(b, f), j = f.events)) { delete g.handle, g.events = {}; for (e in j) for (c = 0, d = j[e].length; c < d; c++)n.event.add(b, e, j[e][c]) } O.hasData(a) && (h = O.access(a), i = n.extend({}, h), O.set(b, i)) } } function ta(a, b) { var c = b.nodeName.toLowerCase(); "input" === c && X.test(a.type) ? b.checked = a.checked : "input" !== c && "textarea" !== c || (b.defaultValue = a.defaultValue) } function ua(a, b, c, d) { b = f.apply([], b); var e, g, h, i, j, k, m = 0, o = a.length, p = o - 1, q = b[0], r = n.isFunction(q); if (r || o > 1 && "string" == typeof q && !l.checkClone && ma.test(q)) return a.each(function (e) { var f = a.eq(e); r && (b[0] = q.call(this, e, f.html())), ua(f, b, c, d) }); if (o && (e = ca(b, a[0].ownerDocument, !1, a, d), g = e.firstChild, 1 === e.childNodes.length && (e = g), g || d)) { for (h = n.map(_(e, "script"), qa), i = h.length; m < o; m++)j = e, m !== p && (j = n.clone(j, !0, !0), i && n.merge(h, _(j, "script"))), c.call(a[m], j, m); if (i) for (k = h[h.length - 1].ownerDocument, n.map(h, ra), m = 0; m < i; m++)j = h[m], Z.test(j.type || "") && !N.access(j, "globalEval") && n.contains(k, j) && (j.src ? n._evalUrl && n._evalUrl(j.src) : n.globalEval(j.textContent.replace(oa, ""))) } return a } function va(a, b, c) { for (var d, e = b ? n.filter(b, a) : a, f = 0; null != (d = e[f]); f++)c || 1 !== d.nodeType || n.cleanData(_(d)), d.parentNode && (c && n.contains(d.ownerDocument, d) && aa(_(d, "script")), d.parentNode.removeChild(d)); return a } n.extend({ htmlPrefilter: function (a) { return a.replace(ka, "<$1></$2>") }, clone: function (a, b, c) { var d, e, f, g, h = a.cloneNode(!0), i = n.contains(a.ownerDocument, a); if (!(l.noCloneChecked || 1 !== a.nodeType && 11 !== a.nodeType || n.isXMLDoc(a))) for (g = _(h), f = _(a), d = 0, e = f.length; d < e; d++)ta(f[d], g[d]); if (b) if (c) for (f = f || _(a), g = g || _(h), d = 0, e = f.length; d < e; d++)sa(f[d], g[d]); else sa(a, h); return g = _(h, "script"), g.length > 0 && aa(g, !i && _(a, "script")), h }, cleanData: function (a) { for (var b, c, d, e = n.event.special, f = 0; void 0 !== (c = a[f]); f++)if (L(c)) { if (b = c[N.expando]) { if (b.events) for (d in b.events) e[d] ? n.event.remove(c, d) : n.removeEvent(c, d, b.handle); c[N.expando] = void 0 } c[O.expando] && (c[O.expando] = void 0) } } }), n.fn.extend({ domManip: ua, detach: function (a) { return va(this, a, !0) }, remove: function (a) { return va(this, a) }, text: function (a) { return K(this, function (a) { return void 0 === a ? n.text(this) : this.empty().each(function () { 1 !== this.nodeType && 11 !== this.nodeType && 9 !== this.nodeType || (this.textContent = a) }) }, null, a, arguments.length) }, append: function () { return ua(this, arguments, function (a) { if (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) { var b = pa(this, a); b.appendChild(a) } }) }, prepend: function () { return ua(this, arguments, function (a) { if (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) { var b = pa(this, a); b.insertBefore(a, b.firstChild) } }) }, before: function () { return ua(this, arguments, function (a) { this.parentNode && this.parentNode.insertBefore(a, this) }) }, after: function () { return ua(this, arguments, function (a) { this.parentNode && this.parentNode.insertBefore(a, this.nextSibling) }) }, empty: function () { for (var a, b = 0; null != (a = this[b]); b++)1 === a.nodeType && (n.cleanData(_(a, !1)), a.textContent = ""); return this }, clone: function (a, b) { return a = null != a && a, b = null == b ? a : b, this.map(function () { return n.clone(this, a, b) }) }, html: function (a) { return K(this, function (a) { var b = this[0] || {}, c = 0, d = this.length; if (void 0 === a && 1 === b.nodeType) return b.innerHTML; if ("string" == typeof a && !la.test(a) && !$[(Y.exec(a) || ["", ""])[1].toLowerCase()]) { a = n.htmlPrefilter(a); try { for (; c < d; c++)b = this[c] || {}, 1 === b.nodeType && (n.cleanData(_(b, !1)), b.innerHTML = a); b = 0 } catch (e) { } } b && this.empty().append(a) }, null, a, arguments.length) }, replaceWith: function () { var a = []; return ua(this, arguments, function (b) { var c = this.parentNode; n.inArray(this, a) < 0 && (n.cleanData(_(this)), c && c.replaceChild(b, this)) }, a) } }), n.each({ appendTo: "append", prependTo: "prepend", insertBefore: "before", insertAfter: "after", replaceAll: "replaceWith" }, function (a, b) { n.fn[a] = function (a) { for (var c, d = [], e = n(a), f = e.length - 1, h = 0; h <= f; h++)c = h === f ? this : this.clone(!0), n(e[h])[b](c), g.apply(d, c.get()); return this.pushStack(d) } }); var wa, xa = { HTML: "block", BODY: "block" }; function ya(a, b) { var c = n(b.createElement(a)).appendTo(b.body), d = n.css(c[0], "display"); return c.detach(), d } function za(a) { var b = d, c = xa[a]; return c || (c = ya(a, b), "none" !== c && c || (wa = (wa || n("<iframe frameborder='0' width='0' height='0'/>")).appendTo(b.documentElement), b = wa[0].contentDocument, b.write(), b.close(), c = ya(a, b), wa.detach()), xa[a] = c), c } var Aa = /^margin/, Ba = new RegExp("^(" + S + ")(?!px)[a-z%]+$", "i"), Ca = function (b) { var c = b.ownerDocument.defaultView; return c && c.opener || (c = a), c.getComputedStyle(b) }, Da = function (a, b, c, d) { var e, f, g = {}; for (f in b) g[f] = a.style[f], a.style[f] = b[f]; e = c.apply(a, d || []); for (f in b) a.style[f] = g[f]; return e }, Ea = d.documentElement; !function () { var b, c, e, f, g = d.createElement("div"), h = d.createElement("div"); function i() { h.style.cssText = "-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;position:relative;display:block;margin:auto;border:1px;padding:1px;top:1%;width:50%", h.innerHTML = "", Ea.appendChild(g); var d = a.getComputedStyle(h); b = "1%" !== d.top, f = "2px" === d.marginLeft, c = "4px" === d.width, h.style.marginRight = "50%", e = "4px" === d.marginRight, Ea.removeChild(g) } h.style && (h.style.backgroundClip = "content-box", h.cloneNode(!0).style.backgroundClip = "", l.clearCloneStyle = "content-box" === h.style.backgroundClip, g.style.cssText = "border:0;width:8px;height:0;top:0;left:-9999px;padding:0;margin-top:1px;position:absolute", g.appendChild(h), n.extend(l, { pixelPosition: function () { return i(), b }, boxSizingReliable: function () { return null == c && i(), c }, pixelMarginRight: function () { return null == c && i(), e }, reliableMarginLeft: function () { return null == c && i(), f }, reliableMarginRight: function () { var b, c = h.appendChild(d.createElement("div")); return c.style.cssText = h.style.cssText = "-webkit-box-sizing:content-box;box-sizing:content-box;display:block;margin:0;border:0;padding:0", c.style.marginRight = c.style.width = "0", h.style.width = "1px", Ea.appendChild(g), b = !parseFloat(a.getComputedStyle(c).marginRight), Ea.removeChild(g), h.removeChild(c), b } })) }(); function Fa(a, b, c) { var d, e, f, g, h = a.style; return c = c || Ca(a), g = c ? c.getPropertyValue(b) || c[b] : void 0, "" !== g && void 0 !== g || n.contains(a.ownerDocument, a) || (g = n.style(a, b)), c && !l.pixelMarginRight() && Ba.test(g) && Aa.test(b) && (d = h.width, e = h.minWidth, f = h.maxWidth, h.minWidth = h.maxWidth = h.width = g, g = c.width, h.width = d, h.minWidth = e, h.maxWidth = f), void 0 !== g ? g + "" : g } function Ga(a, b) { return { get: function () { return a() ? void delete this.get : (this.get = b).apply(this, arguments) } } } var Ha = /^(none|table(?!-c[ea]).+)/, Ia = { position: "absolute", visibility: "hidden", display: "block" }, Ja = { letterSpacing: "0", fontWeight: "400" }, Ka = ["Webkit", "O", "Moz", "ms"], La = d.createElement("div").style; function Ma(a) { if (a in La) return a; var b = a[0].toUpperCase() + a.slice(1), c = Ka.length; while (c--) if (a = Ka[c] + b, a in La) return a } function Na(a, b, c) { var d = T.exec(b); return d ? Math.max(0, d[2] - (c || 0)) + (d[3] || "px") : b } function Oa(a, b, c, d, e) { for (var f = c === (d ? "border" : "content") ? 4 : "width" === b ? 1 : 0, g = 0; f < 4; f += 2)"margin" === c && (g += n.css(a, c + U[f], !0, e)), d ? ("content" === c && (g -= n.css(a, "padding" + U[f], !0, e)), "margin" !== c && (g -= n.css(a, "border" + U[f] + "Width", !0, e))) : (g += n.css(a, "padding" + U[f], !0, e), "padding" !== c && (g += n.css(a, "border" + U[f] + "Width", !0, e))); return g } function Pa(a, b, c) { var d = !0, e = "width" === b ? a.offsetWidth : a.offsetHeight, f = Ca(a), g = "border-box" === n.css(a, "boxSizing", !1, f); if (e <= 0 || null == e) { if (e = Fa(a, b, f), (e < 0 || null == e) && (e = a.style[b]), Ba.test(e)) return e; d = g && (l.boxSizingReliable() || e === a.style[b]), e = parseFloat(e) || 0 } return e + Oa(a, b, c || (g ? "border" : "content"), d, f) + "px" } function Qa(a, b) { for (var c, d, e, f = [], g = 0, h = a.length; g < h; g++)d = a[g], d.style && (f[g] = N.get(d, "olddisplay"), c = d.style.display, b ? (f[g] || "none" !== c || (d.style.display = ""), "" === d.style.display && V(d) && (f[g] = N.access(d, "olddisplay", za(d.nodeName)))) : (e = V(d), "none" === c && e || N.set(d, "olddisplay", e ? c : n.css(d, "display")))); for (g = 0; g < h; g++)d = a[g], d.style && (b && "none" !== d.style.display && "" !== d.style.display || (d.style.display = b ? f[g] || "" : "none")); return a } n.extend({ cssHooks: { opacity: { get: function (a, b) { if (b) { var c = Fa(a, "opacity"); return "" === c ? "1" : c } } } }, cssNumber: { animationIterationCount: !0, columnCount: !0, fillOpacity: !0, flexGrow: !0, flexShrink: !0, fontWeight: !0, lineHeight: !0, opacity: !0, order: !0, orphans: !0, widows: !0, zIndex: !0, zoom: !0 }, cssProps: { "float": "cssFloat" }, style: function (a, b, c, d) { if (a && 3 !== a.nodeType && 8 !== a.nodeType && a.style) { var e, f, g, h = n.camelCase(b), i = a.style; return b = n.cssProps[h] || (n.cssProps[h] = Ma(h) || h), g = n.cssHooks[b] || n.cssHooks[h], void 0 === c ? g && "get" in g && void 0 !== (e = g.get(a, !1, d)) ? e : i[b] : (f = typeof c, "string" === f && (e = T.exec(c)) && e[1] && (c = W(a, b, e), f = "number"), null != c && c === c && ("number" === f && (c += e && e[3] || (n.cssNumber[h] ? "" : "px")), l.clearCloneStyle || "" !== c || 0 !== b.indexOf("background") || (i[b] = "inherit"), g && "set" in g && void 0 === (c = g.set(a, c, d)) || (i[b] = c)), void 0) } }, css: function (a, b, c, d) { var e, f, g, h = n.camelCase(b); return b = n.cssProps[h] || (n.cssProps[h] = Ma(h) || h), g = n.cssHooks[b] || n.cssHooks[h], g && "get" in g && (e = g.get(a, !0, c)), void 0 === e && (e = Fa(a, b, d)), "normal" === e && b in Ja && (e = Ja[b]), "" === c || c ? (f = parseFloat(e), c === !0 || isFinite(f) ? f || 0 : e) : e } }), n.each(["height", "width"], function (a, b) { n.cssHooks[b] = { get: function (a, c, d) { if (c) return Ha.test(n.css(a, "display")) && 0 === a.offsetWidth ? Da(a, Ia, function () { return Pa(a, b, d) }) : Pa(a, b, d) }, set: function (a, c, d) { var e, f = d && Ca(a), g = d && Oa(a, b, d, "border-box" === n.css(a, "boxSizing", !1, f), f); return g && (e = T.exec(c)) && "px" !== (e[3] || "px") && (a.style[b] = c, c = n.css(a, b)), Na(a, c, g) } } }), n.cssHooks.marginLeft = Ga(l.reliableMarginLeft, function (a, b) { if (b) return (parseFloat(Fa(a, "marginLeft")) || a.getBoundingClientRect().left - Da(a, { marginLeft: 0 }, function () { return a.getBoundingClientRect().left })) + "px" }), n.cssHooks.marginRight = Ga(l.reliableMarginRight, function (a, b) { if (b) return Da(a, { display: "inline-block" }, Fa, [a, "marginRight"]) }), n.each({ margin: "", padding: "", border: "Width" }, function (a, b) { n.cssHooks[a + b] = { expand: function (c) { for (var d = 0, e = {}, f = "string" == typeof c ? c.split(" ") : [c]; d < 4; d++)e[a + U[d] + b] = f[d] || f[d - 2] || f[0]; return e } }, Aa.test(a) || (n.cssHooks[a + b].set = Na) }), n.fn.extend({ css: function (a, b) { return K(this, function (a, b, c) { var d, e, f = {}, g = 0; if (n.isArray(b)) { for (d = Ca(a), e = b.length; g < e; g++)f[b[g]] = n.css(a, b[g], !1, d); return f } return void 0 !== c ? n.style(a, b, c) : n.css(a, b) }, a, b, arguments.length > 1) }, show: function () { return Qa(this, !0) }, hide: function () { return Qa(this) }, toggle: function (a) { return "boolean" == typeof a ? a ? this.show() : this.hide() : this.each(function () { V(this) ? n(this).show() : n(this).hide() }) } }); function Ra(a, b, c, d, e) { return new Ra.prototype.init(a, b, c, d, e) } n.Tween = Ra, Ra.prototype = { constructor: Ra, init: function (a, b, c, d, e, f) { this.elem = a, this.prop = c, this.easing = e || n.easing._default, this.options = b, this.start = this.now = this.cur(), this.end = d, this.unit = f || (n.cssNumber[c] ? "" : "px") }, cur: function () { var a = Ra.propHooks[this.prop]; return a && a.get ? a.get(this) : Ra.propHooks._default.get(this) }, run: function (a) { var b, c = Ra.propHooks[this.prop]; return this.options.duration ? this.pos = b = n.easing[this.easing](a, this.options.duration * a, 0, 1, this.options.duration) : this.pos = b = a, this.now = (this.end - this.start) * b + this.start, this.options.step && this.options.step.call(this.elem, this.now, this), c && c.set ? c.set(this) : Ra.propHooks._default.set(this), this } }, Ra.prototype.init.prototype = Ra.prototype, Ra.propHooks = { _default: { get: function (a) { var b; return 1 !== a.elem.nodeType || null != a.elem[a.prop] && null == a.elem.style[a.prop] ? a.elem[a.prop] : (b = n.css(a.elem, a.prop, ""), b && "auto" !== b ? b : 0) }, set: function (a) { n.fx.step[a.prop] ? n.fx.step[a.prop](a) : 1 !== a.elem.nodeType || null == a.elem.style[n.cssProps[a.prop]] && !n.cssHooks[a.prop] ? a.elem[a.prop] = a.now : n.style(a.elem, a.prop, a.now + a.unit) } } }, Ra.propHooks.scrollTop = Ra.propHooks.scrollLeft = { set: function (a) { a.elem.nodeType && a.elem.parentNode && (a.elem[a.prop] = a.now) } }, n.easing = { linear: function (a) { return a }, swing: function (a) { return .5 - Math.cos(a * Math.PI) / 2 }, _default: "swing" }, n.fx = Ra.prototype.init, n.fx.step = {}; var Sa, Ta, Ua = /^(?:toggle|show|hide)$/, Va = /queueHooks$/; function Wa() { return a.setTimeout(function () { Sa = void 0 }), Sa = n.now() } function Xa(a, b) { var c, d = 0, e = { height: a }; for (b = b ? 1 : 0; d < 4; d += 2 - b)c = U[d], e["margin" + c] = e["padding" + c] = a; return b && (e.opacity = e.width = a), e } function Ya(a, b, c) { for (var d, e = (_a.tweeners[b] || []).concat(_a.tweeners["*"]), f = 0, g = e.length; f < g; f++)if (d = e[f].call(c, b, a)) return d } function Za(a, b, c) { var d, e, f, g, h, i, j, k, l = this, m = {}, o = a.style, p = a.nodeType && V(a), q = N.get(a, "fxshow"); c.queue || (h = n._queueHooks(a, "fx"), null == h.unqueued && (h.unqueued = 0, i = h.empty.fire, h.empty.fire = function () { h.unqueued || i() }), h.unqueued++ , l.always(function () { l.always(function () { h.unqueued-- , n.queue(a, "fx").length || h.empty.fire() }) })), 1 === a.nodeType && ("height" in b || "width" in b) && (c.overflow = [o.overflow, o.overflowX, o.overflowY], j = n.css(a, "display"), k = "none" === j ? N.get(a, "olddisplay") || za(a.nodeName) : j, "inline" === k && "none" === n.css(a, "float") && (o.display = "inline-block")), c.overflow && (o.overflow = "hidden", l.always(function () { o.overflow = c.overflow[0], o.overflowX = c.overflow[1], o.overflowY = c.overflow[2] })); for (d in b) if (e = b[d], Ua.exec(e)) { if (delete b[d], f = f || "toggle" === e, e === (p ? "hide" : "show")) { if ("show" !== e || !q || void 0 === q[d]) continue; p = !0 } m[d] = q && q[d] || n.style(a, d) } else j = void 0; if (n.isEmptyObject(m)) "inline" === ("none" === j ? za(a.nodeName) : j) && (o.display = j); else { q ? "hidden" in q && (p = q.hidden) : q = N.access(a, "fxshow", {}), f && (q.hidden = !p), p ? n(a).show() : l.done(function () { n(a).hide() }), l.done(function () { var b; N.remove(a, "fxshow"); for (b in m) n.style(a, b, m[b]) }); for (d in m) g = Ya(p ? q[d] : 0, d, l), d in q || (q[d] = g.start, p && (g.end = g.start, g.start = "width" === d || "height" === d ? 1 : 0)) } } function $a(a, b) { var c, d, e, f, g; for (c in a) if (d = n.camelCase(c), e = b[d], f = a[c], n.isArray(f) && (e = f[1], f = a[c] = f[0]), c !== d && (a[d] = f, delete a[c]), g = n.cssHooks[d], g && "expand" in g) { f = g.expand(f), delete a[d]; for (c in f) c in a || (a[c] = f[c], b[c] = e) } else b[d] = e } function _a(a, b, c) { var d, e, f = 0, g = _a.prefilters.length, h = n.Deferred().always(function () { delete i.elem }), i = function () { if (e) return !1; for (var b = Sa || Wa(), c = Math.max(0, j.startTime + j.duration - b), d = c / j.duration || 0, f = 1 - d, g = 0, i = j.tweens.length; g < i; g++)j.tweens[g].run(f); return h.notifyWith(a, [j, f, c]), f < 1 && i ? c : (h.resolveWith(a, [j]), !1) }, j = h.promise({ elem: a, props: n.extend({}, b), opts: n.extend(!0, { specialEasing: {}, easing: n.easing._default }, c), originalProperties: b, originalOptions: c, startTime: Sa || Wa(), duration: c.duration, tweens: [], createTween: function (b, c) { var d = n.Tween(a, j.opts, b, c, j.opts.specialEasing[b] || j.opts.easing); return j.tweens.push(d), d }, stop: function (b) { var c = 0, d = b ? j.tweens.length : 0; if (e) return this; for (e = !0; c < d; c++)j.tweens[c].run(1); return b ? (h.notifyWith(a, [j, 1, 0]), h.resolveWith(a, [j, b])) : h.rejectWith(a, [j, b]), this } }), k = j.props; for ($a(k, j.opts.specialEasing); f < g; f++)if (d = _a.prefilters[f].call(j, a, k, j.opts)) return n.isFunction(d.stop) && (n._queueHooks(j.elem, j.opts.queue).stop = n.proxy(d.stop, d)), d; return n.map(k, Ya, j), n.isFunction(j.opts.start) && j.opts.start.call(a, j), n.fx.timer(n.extend(i, { elem: a, anim: j, queue: j.opts.queue })), j.progress(j.opts.progress).done(j.opts.done, j.opts.complete).fail(j.opts.fail).always(j.opts.always) } n.Animation = n.extend(_a, { tweeners: { "*": [function (a, b) { var c = this.createTween(a, b); return W(c.elem, a, T.exec(b), c), c }] }, tweener: function (a, b) { n.isFunction(a) ? (b = a, a = ["*"]) : a = a.match(G); for (var c, d = 0, e = a.length; d < e; d++)c = a[d], _a.tweeners[c] = _a.tweeners[c] || [], _a.tweeners[c].unshift(b) }, prefilters: [Za], prefilter: function (a, b) { b ? _a.prefilters.unshift(a) : _a.prefilters.push(a) } }), n.speed = function (a, b, c) { var d = a && "object" == typeof a ? n.extend({}, a) : { complete: c || !c && b || n.isFunction(a) && a, duration: a, easing: c && b || b && !n.isFunction(b) && b }; return d.duration = n.fx.off ? 0 : "number" == typeof d.duration ? d.duration : d.duration in n.fx.speeds ? n.fx.speeds[d.duration] : n.fx.speeds._default, null != d.queue && d.queue !== !0 || (d.queue = "fx"), d.old = d.complete, d.complete = function () { n.isFunction(d.old) && d.old.call(this), d.queue && n.dequeue(this, d.queue) }, d }, n.fn.extend({ fadeTo: function (a, b, c, d) { return this.filter(V).css("opacity", 0).show().end().animate({ opacity: b }, a, c, d) }, animate: function (a, b, c, d) { var e = n.isEmptyObject(a), f = n.speed(b, c, d), g = function () { var b = _a(this, n.extend({}, a), f); (e || N.get(this, "finish")) && b.stop(!0) }; return g.finish = g, e || f.queue === !1 ? this.each(g) : this.queue(f.queue, g) }, stop: function (a, b, c) { var d = function (a) { var b = a.stop; delete a.stop, b(c) }; return "string" != typeof a && (c = b, b = a, a = void 0), b && a !== !1 && this.queue(a || "fx", []), this.each(function () { var b = !0, e = null != a && a + "queueHooks", f = n.timers, g = N.get(this); if (e) g[e] && g[e].stop && d(g[e]); else for (e in g) g[e] && g[e].stop && Va.test(e) && d(g[e]); for (e = f.length; e--;)f[e].elem !== this || null != a && f[e].queue !== a || (f[e].anim.stop(c), b = !1, f.splice(e, 1)); !b && c || n.dequeue(this, a) }) }, finish: function (a) { return a !== !1 && (a = a || "fx"), this.each(function () { var b, c = N.get(this), d = c[a + "queue"], e = c[a + "queueHooks"], f = n.timers, g = d ? d.length : 0; for (c.finish = !0, n.queue(this, a, []), e && e.stop && e.stop.call(this, !0), b = f.length; b--;)f[b].elem === this && f[b].queue === a && (f[b].anim.stop(!0), f.splice(b, 1)); for (b = 0; b < g; b++)d[b] && d[b].finish && d[b].finish.call(this); delete c.finish }) } }), n.each(["toggle", "show", "hide"], function (a, b) { var c = n.fn[b]; n.fn[b] = function (a, d, e) { return null == a || "boolean" == typeof a ? c.apply(this, arguments) : this.animate(Xa(b, !0), a, d, e) } }), n.each({ slideDown: Xa("show"), slideUp: Xa("hide"), slideToggle: Xa("toggle"), fadeIn: { opacity: "show" }, fadeOut: { opacity: "hide" }, fadeToggle: { opacity: "toggle" } }, function (a, b) { n.fn[a] = function (a, c, d) { return this.animate(b, a, c, d) } }), n.timers = [], n.fx.tick = function () { var a, b = 0, c = n.timers; for (Sa = n.now(); b < c.length; b++)a = c[b], a() || c[b] !== a || c.splice(b--, 1); c.length || n.fx.stop(), Sa = void 0 }, n.fx.timer = function (a) { n.timers.push(a), a() ? n.fx.start() : n.timers.pop() }, n.fx.interval = 13, n.fx.start = function () { Ta || (Ta = a.setInterval(n.fx.tick, n.fx.interval)) }, n.fx.stop = function () { a.clearInterval(Ta), Ta = null }, n.fx.speeds = { slow: 600, fast: 200, _default: 400 }, n.fn.delay = function (b, c) { return b = n.fx ? n.fx.speeds[b] || b : b, c = c || "fx", this.queue(c, function (c, d) { var e = a.setTimeout(c, b); d.stop = function () { a.clearTimeout(e) } }) }, function () { var a = d.createElement("input"), b = d.createElement("select"), c = b.appendChild(d.createElement("option")); a.type = "checkbox", l.checkOn = "" !== a.value, l.optSelected = c.selected, b.disabled = !0, l.optDisabled = !c.disabled, a = d.createElement("input"), a.value = "t", a.type = "radio", l.radioValue = "t" === a.value }(); var ab, bb = n.expr.attrHandle; n.fn.extend({ attr: function (a, b) { return K(this, n.attr, a, b, arguments.length > 1) }, removeAttr: function (a) { return this.each(function () { n.removeAttr(this, a) }) } }), n.extend({ attr: function (a, b, c) { var d, e, f = a.nodeType; if (3 !== f && 8 !== f && 2 !== f) return "undefined" == typeof a.getAttribute ? n.prop(a, b, c) : (1 === f && n.isXMLDoc(a) || (b = b.toLowerCase(), e = n.attrHooks[b] || (n.expr.match.bool.test(b) ? ab : void 0)), void 0 !== c ? null === c ? void n.removeAttr(a, b) : e && "set" in e && void 0 !== (d = e.set(a, c, b)) ? d : (a.setAttribute(b, c + ""), c) : e && "get" in e && null !== (d = e.get(a, b)) ? d : (d = n.find.attr(a, b), null == d ? void 0 : d)) }, attrHooks: { type: { set: function (a, b) { if (!l.radioValue && "radio" === b && n.nodeName(a, "input")) { var c = a.value; return a.setAttribute("type", b), c && (a.value = c), b } } } }, removeAttr: function (a, b) { var c, d, e = 0, f = b && b.match(G); if (f && 1 === a.nodeType) while (c = f[e++]) d = n.propFix[c] || c, n.expr.match.bool.test(c) && (a[d] = !1), a.removeAttribute(c) } }), ab = { set: function (a, b, c) { return b === !1 ? n.removeAttr(a, c) : a.setAttribute(c, c), c } }, n.each(n.expr.match.bool.source.match(/\w+/g), function (a, b) { var c = bb[b] || n.find.attr; bb[b] = function (a, b, d) { var e, f; return d || (f = bb[b], bb[b] = e, e = null != c(a, b, d) ? b.toLowerCase() : null, bb[b] = f), e } }); var cb = /^(?:input|select|textarea|button)$/i, db = /^(?:a|area)$/i; n.fn.extend({ prop: function (a, b) { return K(this, n.prop, a, b, arguments.length > 1) }, removeProp: function (a) { return this.each(function () { delete this[n.propFix[a] || a] }) } }), n.extend({
        prop: function (a, b, c) {
            var d, e, f = a.nodeType; if (3 !== f && 8 !== f && 2 !== f) return 1 === f && n.isXMLDoc(a) || (b = n.propFix[b] || b,
                e = n.propHooks[b]), void 0 !== c ? e && "set" in e && void 0 !== (d = e.set(a, c, b)) ? d : a[b] = c : e && "get" in e && null !== (d = e.get(a, b)) ? d : a[b]
        }, propHooks: { tabIndex: { get: function (a) { var b = n.find.attr(a, "tabindex"); return b ? parseInt(b, 10) : cb.test(a.nodeName) || db.test(a.nodeName) && a.href ? 0 : -1 } } }, propFix: { "for": "htmlFor", "class": "className" }
    }), l.optSelected || (n.propHooks.selected = { get: function (a) { var b = a.parentNode; return b && b.parentNode && b.parentNode.selectedIndex, null }, set: function (a) { var b = a.parentNode; b && (b.selectedIndex, b.parentNode && b.parentNode.selectedIndex) } }), n.each(["tabIndex", "readOnly", "maxLength", "cellSpacing", "cellPadding", "rowSpan", "colSpan", "useMap", "frameBorder", "contentEditable"], function () { n.propFix[this.toLowerCase()] = this }); var eb = /[\t\r\n\f]/g; function fb(a) { return a.getAttribute && a.getAttribute("class") || "" } n.fn.extend({ addClass: function (a) { var b, c, d, e, f, g, h, i = 0; if (n.isFunction(a)) return this.each(function (b) { n(this).addClass(a.call(this, b, fb(this))) }); if ("string" == typeof a && a) { b = a.match(G) || []; while (c = this[i++]) if (e = fb(c), d = 1 === c.nodeType && (" " + e + " ").replace(eb, " ")) { g = 0; while (f = b[g++]) d.indexOf(" " + f + " ") < 0 && (d += f + " "); h = n.trim(d), e !== h && c.setAttribute("class", h) } } return this }, removeClass: function (a) { var b, c, d, e, f, g, h, i = 0; if (n.isFunction(a)) return this.each(function (b) { n(this).removeClass(a.call(this, b, fb(this))) }); if (!arguments.length) return this.attr("class", ""); if ("string" == typeof a && a) { b = a.match(G) || []; while (c = this[i++]) if (e = fb(c), d = 1 === c.nodeType && (" " + e + " ").replace(eb, " ")) { g = 0; while (f = b[g++]) while (d.indexOf(" " + f + " ") > -1) d = d.replace(" " + f + " ", " "); h = n.trim(d), e !== h && c.setAttribute("class", h) } } return this }, toggleClass: function (a, b) { var c = typeof a; return "boolean" == typeof b && "string" === c ? b ? this.addClass(a) : this.removeClass(a) : n.isFunction(a) ? this.each(function (c) { n(this).toggleClass(a.call(this, c, fb(this), b), b) }) : this.each(function () { var b, d, e, f; if ("string" === c) { d = 0, e = n(this), f = a.match(G) || []; while (b = f[d++]) e.hasClass(b) ? e.removeClass(b) : e.addClass(b) } else void 0 !== a && "boolean" !== c || (b = fb(this), b && N.set(this, "__className__", b), this.setAttribute && this.setAttribute("class", b || a === !1 ? "" : N.get(this, "__className__") || "")) }) }, hasClass: function (a) { var b, c, d = 0; b = " " + a + " "; while (c = this[d++]) if (1 === c.nodeType && (" " + fb(c) + " ").replace(eb, " ").indexOf(b) > -1) return !0; return !1 } }); var gb = /\r/g, hb = /[\x20\t\r\n\f]+/g; n.fn.extend({ val: function (a) { var b, c, d, e = this[0]; { if (arguments.length) return d = n.isFunction(a), this.each(function (c) { var e; 1 === this.nodeType && (e = d ? a.call(this, c, n(this).val()) : a, null == e ? e = "" : "number" == typeof e ? e += "" : n.isArray(e) && (e = n.map(e, function (a) { return null == a ? "" : a + "" })), b = n.valHooks[this.type] || n.valHooks[this.nodeName.toLowerCase()], b && "set" in b && void 0 !== b.set(this, e, "value") || (this.value = e)) }); if (e) return b = n.valHooks[e.type] || n.valHooks[e.nodeName.toLowerCase()], b && "get" in b && void 0 !== (c = b.get(e, "value")) ? c : (c = e.value, "string" == typeof c ? c.replace(gb, "") : null == c ? "" : c) } } }), n.extend({ valHooks: { option: { get: function (a) { var b = n.find.attr(a, "value"); return null != b ? b : n.trim(n.text(a)).replace(hb, " ") } }, select: { get: function (a) { for (var b, c, d = a.options, e = a.selectedIndex, f = "select-one" === a.type || e < 0, g = f ? null : [], h = f ? e + 1 : d.length, i = e < 0 ? h : f ? e : 0; i < h; i++)if (c = d[i], (c.selected || i === e) && (l.optDisabled ? !c.disabled : null === c.getAttribute("disabled")) && (!c.parentNode.disabled || !n.nodeName(c.parentNode, "optgroup"))) { if (b = n(c).val(), f) return b; g.push(b) } return g }, set: function (a, b) { var c, d, e = a.options, f = n.makeArray(b), g = e.length; while (g--) d = e[g], (d.selected = n.inArray(n.valHooks.option.get(d), f) > -1) && (c = !0); return c || (a.selectedIndex = -1), f } } } }), n.each(["radio", "checkbox"], function () { n.valHooks[this] = { set: function (a, b) { if (n.isArray(b)) return a.checked = n.inArray(n(a).val(), b) > -1 } }, l.checkOn || (n.valHooks[this].get = function (a) { return null === a.getAttribute("value") ? "on" : a.value }) }); var ib = /^(?:focusinfocus|focusoutblur)$/; n.extend(n.event, { trigger: function (b, c, e, f) { var g, h, i, j, l, m, o, p = [e || d], q = k.call(b, "type") ? b.type : b, r = k.call(b, "namespace") ? b.namespace.split(".") : []; if (h = i = e = e || d, 3 !== e.nodeType && 8 !== e.nodeType && !ib.test(q + n.event.triggered) && (q.indexOf(".") > -1 && (r = q.split("."), q = r.shift(), r.sort()), l = q.indexOf(":") < 0 && "on" + q, b = b[n.expando] ? b : new n.Event(q, "object" == typeof b && b), b.isTrigger = f ? 2 : 3, b.namespace = r.join("."), b.rnamespace = b.namespace ? new RegExp("(^|\\.)" + r.join("\\.(?:.*\\.|)") + "(\\.|$)") : null, b.result = void 0, b.target || (b.target = e), c = null == c ? [b] : n.makeArray(c, [b]), o = n.event.special[q] || {}, f || !o.trigger || o.trigger.apply(e, c) !== !1)) { if (!f && !o.noBubble && !n.isWindow(e)) { for (j = o.delegateType || q, ib.test(j + q) || (h = h.parentNode); h; h = h.parentNode)p.push(h), i = h; i === (e.ownerDocument || d) && p.push(i.defaultView || i.parentWindow || a) } g = 0; while ((h = p[g++]) && !b.isPropagationStopped()) b.type = g > 1 ? j : o.bindType || q, m = (N.get(h, "events") || {})[b.type] && N.get(h, "handle"), m && m.apply(h, c), m = l && h[l], m && m.apply && L(h) && (b.result = m.apply(h, c), b.result === !1 && b.preventDefault()); return b.type = q, f || b.isDefaultPrevented() || o._default && o._default.apply(p.pop(), c) !== !1 || !L(e) || l && n.isFunction(e[q]) && !n.isWindow(e) && (i = e[l], i && (e[l] = null), n.event.triggered = q, e[q](), n.event.triggered = void 0, i && (e[l] = i)), b.result } }, simulate: function (a, b, c) { var d = n.extend(new n.Event, c, { type: a, isSimulated: !0 }); n.event.trigger(d, null, b) } }), n.fn.extend({ trigger: function (a, b) { return this.each(function () { n.event.trigger(a, b, this) }) }, triggerHandler: function (a, b) { var c = this[0]; if (c) return n.event.trigger(a, b, c, !0) } }), n.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error contextmenu".split(" "), function (a, b) { n.fn[b] = function (a, c) { return arguments.length > 0 ? this.on(b, null, a, c) : this.trigger(b) } }), n.fn.extend({ hover: function (a, b) { return this.mouseenter(a).mouseleave(b || a) } }), l.focusin = "onfocusin" in a, l.focusin || n.each({ focus: "focusin", blur: "focusout" }, function (a, b) { var c = function (a) { n.event.simulate(b, a.target, n.event.fix(a)) }; n.event.special[b] = { setup: function () { var d = this.ownerDocument || this, e = N.access(d, b); e || d.addEventListener(a, c, !0), N.access(d, b, (e || 0) + 1) }, teardown: function () { var d = this.ownerDocument || this, e = N.access(d, b) - 1; e ? N.access(d, b, e) : (d.removeEventListener(a, c, !0), N.remove(d, b)) } } }); var jb = a.location, kb = n.now(), lb = /\?/; n.parseJSON = function (a) { return JSON.parse(a + "") }, n.parseXML = function (b) { var c; if (!b || "string" != typeof b) return null; try { c = (new a.DOMParser).parseFromString(b, "text/xml") } catch (d) { c = void 0 } return c && !c.getElementsByTagName("parsererror").length || n.error("Invalid XML: " + b), c }; var mb = /#.*$/, nb = /([?&])_=[^&]*/, ob = /^(.*?):[ \t]*([^\r\n]*)$/gm, pb = /^(?:about|app|app-storage|.+-extension|file|res|widget):$/, qb = /^(?:GET|HEAD)$/, rb = /^\/\//, sb = {}, tb = {}, ub = "*/".concat("*"), vb = d.createElement("a"); vb.href = jb.href; function wb(a) { return function (b, c) { "string" != typeof b && (c = b, b = "*"); var d, e = 0, f = b.toLowerCase().match(G) || []; if (n.isFunction(c)) while (d = f[e++]) "+" === d[0] ? (d = d.slice(1) || "*", (a[d] = a[d] || []).unshift(c)) : (a[d] = a[d] || []).push(c) } } function xb(a, b, c, d) { var e = {}, f = a === tb; function g(h) { var i; return e[h] = !0, n.each(a[h] || [], function (a, h) { var j = h(b, c, d); return "string" != typeof j || f || e[j] ? f ? !(i = j) : void 0 : (b.dataTypes.unshift(j), g(j), !1) }), i } return g(b.dataTypes[0]) || !e["*"] && g("*") } function yb(a, b) { var c, d, e = n.ajaxSettings.flatOptions || {}; for (c in b) void 0 !== b[c] && ((e[c] ? a : d || (d = {}))[c] = b[c]); return d && n.extend(!0, a, d), a } function zb(a, b, c) { var d, e, f, g, h = a.contents, i = a.dataTypes; while ("*" === i[0]) i.shift(), void 0 === d && (d = a.mimeType || b.getResponseHeader("Content-Type")); if (d) for (e in h) if (h[e] && h[e].test(d)) { i.unshift(e); break } if (i[0] in c) f = i[0]; else { for (e in c) { if (!i[0] || a.converters[e + " " + i[0]]) { f = e; break } g || (g = e) } f = f || g } if (f) return f !== i[0] && i.unshift(f), c[f] } function Ab(a, b, c, d) { var e, f, g, h, i, j = {}, k = a.dataTypes.slice(); if (k[1]) for (g in a.converters) j[g.toLowerCase()] = a.converters[g]; f = k.shift(); while (f) if (a.responseFields[f] && (c[a.responseFields[f]] = b), !i && d && a.dataFilter && (b = a.dataFilter(b, a.dataType)), i = f, f = k.shift()) if ("*" === f) f = i; else if ("*" !== i && i !== f) { if (g = j[i + " " + f] || j["* " + f], !g) for (e in j) if (h = e.split(" "), h[1] === f && (g = j[i + " " + h[0]] || j["* " + h[0]])) { g === !0 ? g = j[e] : j[e] !== !0 && (f = h[0], k.unshift(h[1])); break } if (g !== !0) if (g && a["throws"]) b = g(b); else try { b = g(b) } catch (l) { return { state: "parsererror", error: g ? l : "No conversion from " + i + " to " + f } } } return { state: "success", data: b } } n.extend({ active: 0, lastModified: {}, etag: {}, ajaxSettings: { url: jb.href, type: "GET", isLocal: pb.test(jb.protocol), global: !0, processData: !0, async: !0, contentType: "application/x-www-form-urlencoded; charset=UTF-8", accepts: { "*": ub, text: "text/plain", html: "text/html", xml: "application/xml, text/xml", json: "application/json, text/javascript" }, contents: { xml: /\bxml\b/, html: /\bhtml/, json: /\bjson\b/ }, responseFields: { xml: "responseXML", text: "responseText", json: "responseJSON" }, converters: { "* text": String, "text html": !0, "text json": n.parseJSON, "text xml": n.parseXML }, flatOptions: { url: !0, context: !0 } }, ajaxSetup: function (a, b) { return b ? yb(yb(a, n.ajaxSettings), b) : yb(n.ajaxSettings, a) }, ajaxPrefilter: wb(sb), ajaxTransport: wb(tb), ajax: function (b, c) { "object" == typeof b && (c = b, b = void 0), c = c || {}; var e, f, g, h, i, j, k, l, m = n.ajaxSetup({}, c), o = m.context || m, p = m.context && (o.nodeType || o.jquery) ? n(o) : n.event, q = n.Deferred(), r = n.Callbacks("once memory"), s = m.statusCode || {}, t = {}, u = {}, v = 0, w = "canceled", x = { readyState: 0, getResponseHeader: function (a) { var b; if (2 === v) { if (!h) { h = {}; while (b = ob.exec(g)) h[b[1].toLowerCase()] = b[2] } b = h[a.toLowerCase()] } return null == b ? null : b }, getAllResponseHeaders: function () { return 2 === v ? g : null }, setRequestHeader: function (a, b) { var c = a.toLowerCase(); return v || (a = u[c] = u[c] || a, t[a] = b), this }, overrideMimeType: function (a) { return v || (m.mimeType = a), this }, statusCode: function (a) { var b; if (a) if (v < 2) for (b in a) s[b] = [s[b], a[b]]; else x.always(a[x.status]); return this }, abort: function (a) { var b = a || w; return e && e.abort(b), z(0, b), this } }; if (q.promise(x).complete = r.add, x.success = x.done, x.error = x.fail, m.url = ((b || m.url || jb.href) + "").replace(mb, "").replace(rb, jb.protocol + "//"), m.type = c.method || c.type || m.method || m.type, m.dataTypes = n.trim(m.dataType || "*").toLowerCase().match(G) || [""], null == m.crossDomain) { j = d.createElement("a"); try { j.href = m.url, j.href = j.href, m.crossDomain = vb.protocol + "//" + vb.host != j.protocol + "//" + j.host } catch (y) { m.crossDomain = !0 } } if (m.data && m.processData && "string" != typeof m.data && (m.data = n.param(m.data, m.traditional)), xb(sb, m, c, x), 2 === v) return x; k = n.event && m.global, k && 0 === n.active++ && n.event.trigger("ajaxStart"), m.type = m.type.toUpperCase(), m.hasContent = !qb.test(m.type), f = m.url, m.hasContent || (m.data && (f = m.url += (lb.test(f) ? "&" : "?") + m.data, delete m.data), m.cache === !1 && (m.url = nb.test(f) ? f.replace(nb, "$1_=" + kb++) : f + (lb.test(f) ? "&" : "?") + "_=" + kb++)), m.ifModified && (n.lastModified[f] && x.setRequestHeader("If-Modified-Since", n.lastModified[f]), n.etag[f] && x.setRequestHeader("If-None-Match", n.etag[f])), (m.data && m.hasContent && m.contentType !== !1 || c.contentType) && x.setRequestHeader("Content-Type", m.contentType), x.setRequestHeader("Accept", m.dataTypes[0] && m.accepts[m.dataTypes[0]] ? m.accepts[m.dataTypes[0]] + ("*" !== m.dataTypes[0] ? ", " + ub + "; q=0.01" : "") : m.accepts["*"]); for (l in m.headers) x.setRequestHeader(l, m.headers[l]); if (m.beforeSend && (m.beforeSend.call(o, x, m) === !1 || 2 === v)) return x.abort(); w = "abort"; for (l in { success: 1, error: 1, complete: 1 }) x[l](m[l]); if (e = xb(tb, m, c, x)) { if (x.readyState = 1, k && p.trigger("ajaxSend", [x, m]), 2 === v) return x; m.async && m.timeout > 0 && (i = a.setTimeout(function () { x.abort("timeout") }, m.timeout)); try { v = 1, e.send(t, z) } catch (y) { if (!(v < 2)) throw y; z(-1, y) } } else z(-1, "No Transport"); function z(b, c, d, h) { var j, l, t, u, w, y = c; 2 !== v && (v = 2, i && a.clearTimeout(i), e = void 0, g = h || "", x.readyState = b > 0 ? 4 : 0, j = b >= 200 && b < 300 || 304 === b, d && (u = zb(m, x, d)), u = Ab(m, u, x, j), j ? (m.ifModified && (w = x.getResponseHeader("Last-Modified"), w && (n.lastModified[f] = w), w = x.getResponseHeader("etag"), w && (n.etag[f] = w)), 204 === b || "HEAD" === m.type ? y = "nocontent" : 304 === b ? y = "notmodified" : (y = u.state, l = u.data, t = u.error, j = !t)) : (t = y, !b && y || (y = "error", b < 0 && (b = 0))), x.status = b, x.statusText = (c || y) + "", j ? q.resolveWith(o, [l, y, x]) : q.rejectWith(o, [x, y, t]), x.statusCode(s), s = void 0, k && p.trigger(j ? "ajaxSuccess" : "ajaxError", [x, m, j ? l : t]), r.fireWith(o, [x, y]), k && (p.trigger("ajaxComplete", [x, m]), --n.active || n.event.trigger("ajaxStop"))) } return x }, getJSON: function (a, b, c) { return n.get(a, b, c, "json") }, getScript: function (a, b) { return n.get(a, void 0, b, "script") } }), n.each(["get", "post"], function (a, b) { n[b] = function (a, c, d, e) { return n.isFunction(c) && (e = e || d, d = c, c = void 0), n.ajax(n.extend({ url: a, type: b, dataType: e, data: c, success: d }, n.isPlainObject(a) && a)) } }), n._evalUrl = function (a) { return n.ajax({ url: a, type: "GET", dataType: "script", async: !1, global: !1, "throws": !0 }) }, n.fn.extend({ wrapAll: function (a) { var b; return n.isFunction(a) ? this.each(function (b) { n(this).wrapAll(a.call(this, b)) }) : (this[0] && (b = n(a, this[0].ownerDocument).eq(0).clone(!0), this[0].parentNode && b.insertBefore(this[0]), b.map(function () { var a = this; while (a.firstElementChild) a = a.firstElementChild; return a }).append(this)), this) }, wrapInner: function (a) { return n.isFunction(a) ? this.each(function (b) { n(this).wrapInner(a.call(this, b)) }) : this.each(function () { var b = n(this), c = b.contents(); c.length ? c.wrapAll(a) : b.append(a) }) }, wrap: function (a) { var b = n.isFunction(a); return this.each(function (c) { n(this).wrapAll(b ? a.call(this, c) : a) }) }, unwrap: function () { return this.parent().each(function () { n.nodeName(this, "body") || n(this).replaceWith(this.childNodes) }).end() } }), n.expr.filters.hidden = function (a) { return !n.expr.filters.visible(a) }, n.expr.filters.visible = function (a) { return a.offsetWidth > 0 || a.offsetHeight > 0 || a.getClientRects().length > 0 }; var Bb = /%20/g, Cb = /\[\]$/, Db = /\r?\n/g, Eb = /^(?:submit|button|image|reset|file)$/i, Fb = /^(?:input|select|textarea|keygen)/i; function Gb(a, b, c, d) { var e; if (n.isArray(b)) n.each(b, function (b, e) { c || Cb.test(a) ? d(a, e) : Gb(a + "[" + ("object" == typeof e && null != e ? b : "") + "]", e, c, d) }); else if (c || "object" !== n.type(b)) d(a, b); else for (e in b) Gb(a + "[" + e + "]", b[e], c, d) } n.param = function (a, b) { var c, d = [], e = function (a, b) { b = n.isFunction(b) ? b() : null == b ? "" : b, d[d.length] = encodeURIComponent(a) + "=" + encodeURIComponent(b) }; if (void 0 === b && (b = n.ajaxSettings && n.ajaxSettings.traditional), n.isArray(a) || a.jquery && !n.isPlainObject(a)) n.each(a, function () { e(this.name, this.value) }); else for (c in a) Gb(c, a[c], b, e); return d.join("&").replace(Bb, "+") }, n.fn.extend({ serialize: function () { return n.param(this.serializeArray()) }, serializeArray: function () { return this.map(function () { var a = n.prop(this, "elements"); return a ? n.makeArray(a) : this }).filter(function () { var a = this.type; return this.name && !n(this).is(":disabled") && Fb.test(this.nodeName) && !Eb.test(a) && (this.checked || !X.test(a)) }).map(function (a, b) { var c = n(this).val(); return null == c ? null : n.isArray(c) ? n.map(c, function (a) { return { name: b.name, value: a.replace(Db, "\r\n") } }) : { name: b.name, value: c.replace(Db, "\r\n") } }).get() } }), n.ajaxSettings.xhr = function () { try { return new a.XMLHttpRequest } catch (b) { } }; var Hb = { 0: 200, 1223: 204 }, Ib = n.ajaxSettings.xhr(); l.cors = !!Ib && "withCredentials" in Ib, l.ajax = Ib = !!Ib, n.ajaxTransport(function (b) { var c, d; if (l.cors || Ib && !b.crossDomain) return { send: function (e, f) { var g, h = b.xhr(); if (h.open(b.type, b.url, b.async, b.username, b.password), b.xhrFields) for (g in b.xhrFields) h[g] = b.xhrFields[g]; b.mimeType && h.overrideMimeType && h.overrideMimeType(b.mimeType), b.crossDomain || e["X-Requested-With"] || (e["X-Requested-With"] = "XMLHttpRequest"); for (g in e) h.setRequestHeader(g, e[g]); c = function (a) { return function () { c && (c = d = h.onload = h.onerror = h.onabort = h.onreadystatechange = null, "abort" === a ? h.abort() : "error" === a ? "number" != typeof h.status ? f(0, "error") : f(h.status, h.statusText) : f(Hb[h.status] || h.status, h.statusText, "text" !== (h.responseType || "text") || "string" != typeof h.responseText ? { binary: h.response } : { text: h.responseText }, h.getAllResponseHeaders())) } }, h.onload = c(), d = h.onerror = c("error"), void 0 !== h.onabort ? h.onabort = d : h.onreadystatechange = function () { 4 === h.readyState && a.setTimeout(function () { c && d() }) }, c = c("abort"); try { h.send(b.hasContent && b.data || null) } catch (i) { if (c) throw i } }, abort: function () { c && c() } } }), n.ajaxSetup({ accepts: { script: "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript" }, contents: { script: /\b(?:java|ecma)script\b/ }, converters: { "text script": function (a) { return n.globalEval(a), a } } }), n.ajaxPrefilter("script", function (a) { void 0 === a.cache && (a.cache = !1), a.crossDomain && (a.type = "GET") }), n.ajaxTransport("script", function (a) { if (a.crossDomain) { var b, c; return { send: function (e, f) { b = n("<script>").prop({ charset: a.scriptCharset, src: a.url }).on("load error", c = function (a) { b.remove(), c = null, a && f("error" === a.type ? 404 : 200, a.type) }), d.head.appendChild(b[0]) }, abort: function () { c && c() } } } }); var Jb = [], Kb = /(=)\?(?=&|$)|\?\?/; n.ajaxSetup({ jsonp: "callback", jsonpCallback: function () { var a = Jb.pop() || n.expando + "_" + kb++; return this[a] = !0, a } }), n.ajaxPrefilter("json jsonp", function (b, c, d) { var e, f, g, h = b.jsonp !== !1 && (Kb.test(b.url) ? "url" : "string" == typeof b.data && 0 === (b.contentType || "").indexOf("application/x-www-form-urlencoded") && Kb.test(b.data) && "data"); if (h || "jsonp" === b.dataTypes[0]) return e = b.jsonpCallback = n.isFunction(b.jsonpCallback) ? b.jsonpCallback() : b.jsonpCallback, h ? b[h] = b[h].replace(Kb, "$1" + e) : b.jsonp !== !1 && (b.url += (lb.test(b.url) ? "&" : "?") + b.jsonp + "=" + e), b.converters["script json"] = function () { return g || n.error(e + " was not called"), g[0] }, b.dataTypes[0] = "json", f = a[e], a[e] = function () { g = arguments }, d.always(function () { void 0 === f ? n(a).removeProp(e) : a[e] = f, b[e] && (b.jsonpCallback = c.jsonpCallback, Jb.push(e)), g && n.isFunction(f) && f(g[0]), g = f = void 0 }), "script" }), n.parseHTML = function (a, b, c) { if (!a || "string" != typeof a) return null; "boolean" == typeof b && (c = b, b = !1), b = b || d; var e = x.exec(a), f = !c && []; return e ? [b.createElement(e[1])] : (e = ca([a], b, f), f && f.length && n(f).remove(), n.merge([], e.childNodes)) }; var Lb = n.fn.load; n.fn.load = function (a, b, c) { if ("string" != typeof a && Lb) return Lb.apply(this, arguments); var d, e, f, g = this, h = a.indexOf(" "); return h > -1 && (d = n.trim(a.slice(h)), a = a.slice(0, h)), n.isFunction(b) ? (c = b, b = void 0) : b && "object" == typeof b && (e = "POST"), g.length > 0 && n.ajax({ url: a, type: e || "GET", dataType: "html", data: b }).done(function (a) { f = arguments, g.html(d ? n("<div>").append(n.parseHTML(a)).find(d) : a) }).always(c && function (a, b) { g.each(function () { c.apply(this, f || [a.responseText, b, a]) }) }), this }, n.each(["ajaxStart", "ajaxStop", "ajaxComplete", "ajaxError", "ajaxSuccess", "ajaxSend"], function (a, b) { n.fn[b] = function (a) { return this.on(b, a) } }), n.expr.filters.animated = function (a) { return n.grep(n.timers, function (b) { return a === b.elem }).length }; function Mb(a) { return n.isWindow(a) ? a : 9 === a.nodeType && a.defaultView } n.offset = { setOffset: function (a, b, c) { var d, e, f, g, h, i, j, k = n.css(a, "position"), l = n(a), m = {}; "static" === k && (a.style.position = "relative"), h = l.offset(), f = n.css(a, "top"), i = n.css(a, "left"), j = ("absolute" === k || "fixed" === k) && (f + i).indexOf("auto") > -1, j ? (d = l.position(), g = d.top, e = d.left) : (g = parseFloat(f) || 0, e = parseFloat(i) || 0), n.isFunction(b) && (b = b.call(a, c, n.extend({}, h))), null != b.top && (m.top = b.top - h.top + g), null != b.left && (m.left = b.left - h.left + e), "using" in b ? b.using.call(a, m) : l.css(m) } }, n.fn.extend({ offset: function (a) { if (arguments.length) return void 0 === a ? this : this.each(function (b) { n.offset.setOffset(this, a, b) }); var b, c, d = this[0], e = { top: 0, left: 0 }, f = d && d.ownerDocument; if (f) return b = f.documentElement, n.contains(b, d) ? (e = d.getBoundingClientRect(), c = Mb(f), { top: e.top + c.pageYOffset - b.clientTop, left: e.left + c.pageXOffset - b.clientLeft }) : e }, position: function () { if (this[0]) { var a, b, c = this[0], d = { top: 0, left: 0 }; return "fixed" === n.css(c, "position") ? b = c.getBoundingClientRect() : (a = this.offsetParent(), b = this.offset(), n.nodeName(a[0], "html") || (d = a.offset()), d.top += n.css(a[0], "borderTopWidth", !0), d.left += n.css(a[0], "borderLeftWidth", !0)), { top: b.top - d.top - n.css(c, "marginTop", !0), left: b.left - d.left - n.css(c, "marginLeft", !0) } } }, offsetParent: function () { return this.map(function () { var a = this.offsetParent; while (a && "static" === n.css(a, "position")) a = a.offsetParent; return a || Ea }) } }), n.each({ scrollLeft: "pageXOffset", scrollTop: "pageYOffset" }, function (a, b) { var c = "pageYOffset" === b; n.fn[a] = function (d) { return K(this, function (a, d, e) { var f = Mb(a); return void 0 === e ? f ? f[b] : a[d] : void (f ? f.scrollTo(c ? f.pageXOffset : e, c ? e : f.pageYOffset) : a[d] = e) }, a, d, arguments.length) } }), n.each(["top", "left"], function (a, b) { n.cssHooks[b] = Ga(l.pixelPosition, function (a, c) { if (c) return c = Fa(a, b), Ba.test(c) ? n(a).position()[b] + "px" : c }) }), n.each({ Height: "height", Width: "width" }, function (a, b) { n.each({ padding: "inner" + a, content: b, "": "outer" + a }, function (c, d) { n.fn[d] = function (d, e) { var f = arguments.length && (c || "boolean" != typeof d), g = c || (d === !0 || e === !0 ? "margin" : "border"); return K(this, function (b, c, d) { var e; return n.isWindow(b) ? b.document.documentElement["client" + a] : 9 === b.nodeType ? (e = b.documentElement, Math.max(b.body["scroll" + a], e["scroll" + a], b.body["offset" + a], e["offset" + a], e["client" + a])) : void 0 === d ? n.css(b, c, g) : n.style(b, c, d, g) }, b, f ? d : void 0, f, null) } }) }), n.fn.extend({ bind: function (a, b, c) { return this.on(a, null, b, c) }, unbind: function (a, b) { return this.off(a, null, b) }, delegate: function (a, b, c, d) { return this.on(b, a, c, d) }, undelegate: function (a, b, c) { return 1 === arguments.length ? this.off(a, "**") : this.off(b, a || "**", c) }, size: function () { return this.length } }), n.fn.andSelf = n.fn.addBack, "function" == typeof define && define.amd && define("jquery", [], function () { return n }); var Nb = a.jQuery, Ob = a.$; return n.noConflict = function (b) { return a.$ === n && (a.$ = Ob), b && a.jQuery === n && (a.jQuery = Nb), n }, b || (a.jQuery = a.$ = n), n
});

/*! jQuery Validation Plugin - v1.19.1 - 6/15/2019
 * https://jqueryvalidation.org/
 * Copyright (c) 2019 Jörn Zaefferer; Licensed MIT */
!function (a) { "function" == typeof define && define.amd ? define(["jquery"], a) : "object" == typeof module && module.exports ? module.exports = a(require("jquery")) : a(jQuery) }(function (a) { a.extend(a.fn, { validate: function (b) { if (!this.length) return void (b && b.debug && window.console && console.warn("Nothing selected, can't validate, returning nothing.")); var c = a.data(this[0], "validator"); return c ? c : (this.attr("novalidate", "novalidate"), c = new a.validator(b, this[0]), a.data(this[0], "validator", c), c.settings.onsubmit && (this.on("click.validate", ":submit", function (b) { c.submitButton = b.currentTarget, a(this).hasClass("cancel") && (c.cancelSubmit = !0), void 0 !== a(this).attr("formnovalidate") && (c.cancelSubmit = !0) }), this.on("submit.validate", function (b) { function d() { var d, e; return c.submitButton && (c.settings.submitHandler || c.formSubmitted) && (d = a("<input type='hidden'/>").attr("name", c.submitButton.name).val(a(c.submitButton).val()).appendTo(c.currentForm)), !(c.settings.submitHandler && !c.settings.debug) || (e = c.settings.submitHandler.call(c, c.currentForm, b), d && d.remove(), void 0 !== e && e) } return c.settings.debug && b.preventDefault(), c.cancelSubmit ? (c.cancelSubmit = !1, d()) : c.form() ? c.pendingRequest ? (c.formSubmitted = !0, !1) : d() : (c.focusInvalid(), !1) })), c) }, valid: function () { var b, c, d; return a(this[0]).is("form") ? b = this.validate().form() : (d = [], b = !0, c = a(this[0].form).validate(), this.each(function () { b = c.element(this) && b, b || (d = d.concat(c.errorList)) }), c.errorList = d), b }, rules: function (b, c) { var d, e, f, g, h, i, j = this[0], k = "undefined" != typeof this.attr("contenteditable") && "false" !== this.attr("contenteditable"); if (null != j && (!j.form && k && (j.form = this.closest("form")[0], j.name = this.attr("name")), null != j.form)) { if (b) switch (d = a.data(j.form, "validator").settings, e = d.rules, f = a.validator.staticRules(j), b) { case "add": a.extend(f, a.validator.normalizeRule(c)), delete f.messages, e[j.name] = f, c.messages && (d.messages[j.name] = a.extend(d.messages[j.name], c.messages)); break; case "remove": return c ? (i = {}, a.each(c.split(/\s/), function (a, b) { i[b] = f[b], delete f[b] }), i) : (delete e[j.name], f) }return g = a.validator.normalizeRules(a.extend({}, a.validator.classRules(j), a.validator.attributeRules(j), a.validator.dataRules(j), a.validator.staticRules(j)), j), g.required && (h = g.required, delete g.required, g = a.extend({ required: h }, g)), g.remote && (h = g.remote, delete g.remote, g = a.extend(g, { remote: h })), g } } }), a.extend(a.expr.pseudos || a.expr[":"], { blank: function (b) { return !a.trim("" + a(b).val()) }, filled: function (b) { var c = a(b).val(); return null !== c && !!a.trim("" + c) }, unchecked: function (b) { return !a(b).prop("checked") } }), a.validator = function (b, c) { this.settings = a.extend(!0, {}, a.validator.defaults, b), this.currentForm = c, this.init() }, a.validator.format = function (b, c) { return 1 === arguments.length ? function () { var c = a.makeArray(arguments); return c.unshift(b), a.validator.format.apply(this, c) } : void 0 === c ? b : (arguments.length > 2 && c.constructor !== Array && (c = a.makeArray(arguments).slice(1)), c.constructor !== Array && (c = [c]), a.each(c, function (a, c) { b = b.replace(new RegExp("\\{" + a + "\\}", "g"), function () { return c }) }), b) }, a.extend(a.validator, { defaults: { messages: {}, groups: {}, rules: {}, errorClass: "error", pendingClass: "pending", validClass: "valid", errorElement: "label", focusCleanup: !1, focusInvalid: !0, errorContainer: a([]), errorLabelContainer: a([]), onsubmit: !0, ignore: ":hidden", ignoreTitle: !1, onfocusin: function (a) { this.lastActive = a, this.settings.focusCleanup && (this.settings.unhighlight && this.settings.unhighlight.call(this, a, this.settings.errorClass, this.settings.validClass), this.hideThese(this.errorsFor(a))) }, onfocusout: function (a) { this.checkable(a) || !(a.name in this.submitted) && this.optional(a) || this.element(a) }, onkeyup: function (b, c) { var d = [16, 17, 18, 20, 35, 36, 37, 38, 39, 40, 45, 144, 225]; 9 === c.which && "" === this.elementValue(b) || a.inArray(c.keyCode, d) !== -1 || (b.name in this.submitted || b.name in this.invalid) && this.element(b) }, onclick: function (a) { a.name in this.submitted ? this.element(a) : a.parentNode.name in this.submitted && this.element(a.parentNode) }, highlight: function (b, c, d) { "radio" === b.type ? this.findByName(b.name).addClass(c).removeClass(d) : a(b).addClass(c).removeClass(d) }, unhighlight: function (b, c, d) { "radio" === b.type ? this.findByName(b.name).removeClass(c).addClass(d) : a(b).removeClass(c).addClass(d) } }, setDefaults: function (b) { a.extend(a.validator.defaults, b) }, messages: { required: "This field is required.", remote: "Please fix this field.", email: "Please enter a valid email address.", url: "Please enter a valid URL.", date: "Please enter a valid date.", dateISO: "Please enter a valid date (ISO).", number: "Please enter a valid number.", digits: "Please enter only digits.", equalTo: "Please enter the same value again.", maxlength: a.validator.format("Please enter no more than {0} characters."), minlength: a.validator.format("Please enter at least {0} characters."), rangelength: a.validator.format("Please enter a value between {0} and {1} characters long."), range: a.validator.format("Please enter a value between {0} and {1}."), max: a.validator.format("Please enter a value less than or equal to {0}."), min: a.validator.format("Please enter a value greater than or equal to {0}."), step: a.validator.format("Please enter a multiple of {0}.") }, autoCreateRanges: !1, prototype: { init: function () { function b(b) { var c = "undefined" != typeof a(this).attr("contenteditable") && "false" !== a(this).attr("contenteditable"); if (!this.form && c && (this.form = a(this).closest("form")[0], this.name = a(this).attr("name")), d === this.form) { var e = a.data(this.form, "validator"), f = "on" + b.type.replace(/^validate/, ""), g = e.settings; g[f] && !a(this).is(g.ignore) && g[f].call(e, this, b) } } this.labelContainer = a(this.settings.errorLabelContainer), this.errorContext = this.labelContainer.length && this.labelContainer || a(this.currentForm), this.containers = a(this.settings.errorContainer).add(this.settings.errorLabelContainer), this.submitted = {}, this.valueCache = {}, this.pendingRequest = 0, this.pending = {}, this.invalid = {}, this.reset(); var c, d = this.currentForm, e = this.groups = {}; a.each(this.settings.groups, function (b, c) { "string" == typeof c && (c = c.split(/\s/)), a.each(c, function (a, c) { e[c] = b }) }), c = this.settings.rules, a.each(c, function (b, d) { c[b] = a.validator.normalizeRule(d) }), a(this.currentForm).on("focusin.validate focusout.validate keyup.validate", ":text, [type='password'], [type='file'], select, textarea, [type='number'], [type='search'], [type='tel'], [type='url'], [type='email'], [type='datetime'], [type='date'], [type='month'], [type='week'], [type='time'], [type='datetime-local'], [type='range'], [type='color'], [type='radio'], [type='checkbox'], [contenteditable], [type='button']", b).on("click.validate", "select, option, [type='radio'], [type='checkbox']", b), this.settings.invalidHandler && a(this.currentForm).on("invalid-form.validate", this.settings.invalidHandler) }, form: function () { return this.checkForm(), a.extend(this.submitted, this.errorMap), this.invalid = a.extend({}, this.errorMap), this.valid() || a(this.currentForm).triggerHandler("invalid-form", [this]), this.showErrors(), this.valid() }, checkForm: function () { this.prepareForm(); for (var a = 0, b = this.currentElements = this.elements(); b[a]; a++)this.check(b[a]); return this.valid() }, element: function (b) { var c, d, e = this.clean(b), f = this.validationTargetFor(e), g = this, h = !0; return void 0 === f ? delete this.invalid[e.name] : (this.prepareElement(f), this.currentElements = a(f), d = this.groups[f.name], d && a.each(this.groups, function (a, b) { b === d && a !== f.name && (e = g.validationTargetFor(g.clean(g.findByName(a))), e && e.name in g.invalid && (g.currentElements.push(e), h = g.check(e) && h)) }), c = this.check(f) !== !1, h = h && c, c ? this.invalid[f.name] = !1 : this.invalid[f.name] = !0, this.numberOfInvalids() || (this.toHide = this.toHide.add(this.containers)), this.showErrors(), a(b).attr("aria-invalid", !c)), h }, showErrors: function (b) { if (b) { var c = this; a.extend(this.errorMap, b), this.errorList = a.map(this.errorMap, function (a, b) { return { message: a, element: c.findByName(b)[0] } }), this.successList = a.grep(this.successList, function (a) { return !(a.name in b) }) } this.settings.showErrors ? this.settings.showErrors.call(this, this.errorMap, this.errorList) : this.defaultShowErrors() }, resetForm: function () { a.fn.resetForm && a(this.currentForm).resetForm(), this.invalid = {}, this.submitted = {}, this.prepareForm(), this.hideErrors(); var b = this.elements().removeData("previousValue").removeAttr("aria-invalid"); this.resetElements(b) }, resetElements: function (a) { var b; if (this.settings.unhighlight) for (b = 0; a[b]; b++)this.settings.unhighlight.call(this, a[b], this.settings.errorClass, ""), this.findByName(a[b].name).removeClass(this.settings.validClass); else a.removeClass(this.settings.errorClass).removeClass(this.settings.validClass) }, numberOfInvalids: function () { return this.objectLength(this.invalid) }, objectLength: function (a) { var b, c = 0; for (b in a) void 0 !== a[b] && null !== a[b] && a[b] !== !1 && c++; return c }, hideErrors: function () { this.hideThese(this.toHide) }, hideThese: function (a) { a.not(this.containers).text(""), this.addWrapper(a).hide() }, valid: function () { return 0 === this.size() }, size: function () { return this.errorList.length }, focusInvalid: function () { if (this.settings.focusInvalid) try { a(this.findLastActive() || this.errorList.length && this.errorList[0].element || []).filter(":visible").trigger("focus").trigger("focusin") } catch (b) { } }, findLastActive: function () { var b = this.lastActive; return b && 1 === a.grep(this.errorList, function (a) { return a.element.name === b.name }).length && b }, elements: function () { var b = this, c = {}; return a(this.currentForm).find("input, select, textarea, [contenteditable]").not(":submit, :reset, :image, :disabled").not(this.settings.ignore).filter(function () { var d = this.name || a(this).attr("name"), e = "undefined" != typeof a(this).attr("contenteditable") && "false" !== a(this).attr("contenteditable"); return !d && b.settings.debug && window.console && console.error("%o has no name assigned", this), e && (this.form = a(this).closest("form")[0], this.name = d), this.form === b.currentForm && (!(d in c || !b.objectLength(a(this).rules())) && (c[d] = !0, !0)) }) }, clean: function (b) { return a(b)[0] }, errors: function () { var b = this.settings.errorClass.split(" ").join("."); return a(this.settings.errorElement + "." + b, this.errorContext) }, resetInternals: function () { this.successList = [], this.errorList = [], this.errorMap = {}, this.toShow = a([]), this.toHide = a([]) }, reset: function () { this.resetInternals(), this.currentElements = a([]) }, prepareForm: function () { this.reset(), this.toHide = this.errors().add(this.containers) }, prepareElement: function (a) { this.reset(), this.toHide = this.errorsFor(a) }, elementValue: function (b) { var c, d, e = a(b), f = b.type, g = "undefined" != typeof e.attr("contenteditable") && "false" !== e.attr("contenteditable"); return "radio" === f || "checkbox" === f ? this.findByName(b.name).filter(":checked").val() : "number" === f && "undefined" != typeof b.validity ? b.validity.badInput ? "NaN" : e.val() : (c = g ? e.text() : e.val(), "file" === f ? "C:\\fakepath\\" === c.substr(0, 12) ? c.substr(12) : (d = c.lastIndexOf("/"), d >= 0 ? c.substr(d + 1) : (d = c.lastIndexOf("\\"), d >= 0 ? c.substr(d + 1) : c)) : "string" == typeof c ? c.replace(/\r/g, "") : c) }, check: function (b) { b = this.validationTargetFor(this.clean(b)); var c, d, e, f, g = a(b).rules(), h = a.map(g, function (a, b) { return b }).length, i = !1, j = this.elementValue(b); "function" == typeof g.normalizer ? f = g.normalizer : "function" == typeof this.settings.normalizer && (f = this.settings.normalizer), f && (j = f.call(b, j), delete g.normalizer); for (d in g) { e = { method: d, parameters: g[d] }; try { if (c = a.validator.methods[d].call(this, j, b, e.parameters), "dependency-mismatch" === c && 1 === h) { i = !0; continue } if (i = !1, "pending" === c) return void (this.toHide = this.toHide.not(this.errorsFor(b))); if (!c) return this.formatAndAdd(b, e), !1 } catch (k) { throw this.settings.debug && window.console && console.log("Exception occurred when checking element " + b.id + ", check the '" + e.method + "' method.", k), k instanceof TypeError && (k.message += ".  Exception occurred when checking element " + b.id + ", check the '" + e.method + "' method."), k } } if (!i) return this.objectLength(g) && this.successList.push(b), !0 }, customDataMessage: function (b, c) { return a(b).data("msg" + c.charAt(0).toUpperCase() + c.substring(1).toLowerCase()) || a(b).data("msg") }, customMessage: function (a, b) { var c = this.settings.messages[a]; return c && (c.constructor === String ? c : c[b]) }, findDefined: function () { for (var a = 0; a < arguments.length; a++)if (void 0 !== arguments[a]) return arguments[a] }, defaultMessage: function (b, c) { "string" == typeof c && (c = { method: c }); var d = this.findDefined(this.customMessage(b.name, c.method), this.customDataMessage(b, c.method), !this.settings.ignoreTitle && b.title || void 0, a.validator.messages[c.method], "<strong>Warning: No message defined for " + b.name + "</strong>"), e = /\$?\{(\d+)\}/g; return "function" == typeof d ? d = d.call(this, c.parameters, b) : e.test(d) && (d = a.validator.format(d.replace(e, "{$1}"), c.parameters)), d }, formatAndAdd: function (a, b) { var c = this.defaultMessage(a, b); this.errorList.push({ message: c, element: a, method: b.method }), this.errorMap[a.name] = c, this.submitted[a.name] = c }, addWrapper: function (a) { return this.settings.wrapper && (a = a.add(a.parent(this.settings.wrapper))), a }, defaultShowErrors: function () { var a, b, c; for (a = 0; this.errorList[a]; a++)c = this.errorList[a], this.settings.highlight && this.settings.highlight.call(this, c.element, this.settings.errorClass, this.settings.validClass), this.showLabel(c.element, c.message); if (this.errorList.length && (this.toShow = this.toShow.add(this.containers)), this.settings.success) for (a = 0; this.successList[a]; a++)this.showLabel(this.successList[a]); if (this.settings.unhighlight) for (a = 0, b = this.validElements(); b[a]; a++)this.settings.unhighlight.call(this, b[a], this.settings.errorClass, this.settings.validClass); this.toHide = this.toHide.not(this.toShow), this.hideErrors(), this.addWrapper(this.toShow).show() }, validElements: function () { return this.currentElements.not(this.invalidElements()) }, invalidElements: function () { return a(this.errorList).map(function () { return this.element }) }, showLabel: function (b, c) { var d, e, f, g, h = this.errorsFor(b), i = this.idOrName(b), j = a(b).attr("aria-describedby"); h.length ? (h.removeClass(this.settings.validClass).addClass(this.settings.errorClass), h.html(c)) : (h = a("<" + this.settings.errorElement + ">").attr("id", i + "-error").addClass(this.settings.errorClass).html(c || ""), d = h, this.settings.wrapper && (d = h.hide().show().wrap("<" + this.settings.wrapper + "/>").parent()), this.labelContainer.length ? this.labelContainer.append(d) : this.settings.errorPlacement ? this.settings.errorPlacement.call(this, d, a(b)) : d.insertAfter(b), h.is("label") ? h.attr("for", i) : 0 === h.parents("label[for='" + this.escapeCssMeta(i) + "']").length && (f = h.attr("id"), j ? j.match(new RegExp("\\b" + this.escapeCssMeta(f) + "\\b")) || (j += " " + f) : j = f, a(b).attr("aria-describedby", j), e = this.groups[b.name], e && (g = this, a.each(g.groups, function (b, c) { c === e && a("[name='" + g.escapeCssMeta(b) + "']", g.currentForm).attr("aria-describedby", h.attr("id")) })))), !c && this.settings.success && (h.text(""), "string" == typeof this.settings.success ? h.addClass(this.settings.success) : this.settings.success(h, b)), this.toShow = this.toShow.add(h) }, errorsFor: function (b) { var c = this.escapeCssMeta(this.idOrName(b)), d = a(b).attr("aria-describedby"), e = "label[for='" + c + "'], label[for='" + c + "'] *"; return d && (e = e + ", #" + this.escapeCssMeta(d).replace(/\s+/g, ", #")), this.errors().filter(e) }, escapeCssMeta: function (a) { return a.replace(/([\\!"#$%&'()*+,.\/:;<=>?@\[\]^`{|}~])/g, "\\$1") }, idOrName: function (a) { return this.groups[a.name] || (this.checkable(a) ? a.name : a.id || a.name) }, validationTargetFor: function (b) { return this.checkable(b) && (b = this.findByName(b.name)), a(b).not(this.settings.ignore)[0] }, checkable: function (a) { return /radio|checkbox/i.test(a.type) }, findByName: function (b) { return a(this.currentForm).find("[name='" + this.escapeCssMeta(b) + "']") }, getLength: function (b, c) { switch (c.nodeName.toLowerCase()) { case "select": return a("option:selected", c).length; case "input": if (this.checkable(c)) return this.findByName(c.name).filter(":checked").length }return b.length }, depend: function (a, b) { return !this.dependTypes[typeof a] || this.dependTypes[typeof a](a, b) }, dependTypes: { "boolean": function (a) { return a }, string: function (b, c) { return !!a(b, c.form).length }, "function": function (a, b) { return a(b) } }, optional: function (b) { var c = this.elementValue(b); return !a.validator.methods.required.call(this, c, b) && "dependency-mismatch" }, startRequest: function (b) { this.pending[b.name] || (this.pendingRequest++ , a(b).addClass(this.settings.pendingClass), this.pending[b.name] = !0) }, stopRequest: function (b, c) { this.pendingRequest-- , this.pendingRequest < 0 && (this.pendingRequest = 0), delete this.pending[b.name], a(b).removeClass(this.settings.pendingClass), c && 0 === this.pendingRequest && this.formSubmitted && this.form() ? (a(this.currentForm).submit(), this.submitButton && a("input:hidden[name='" + this.submitButton.name + "']", this.currentForm).remove(), this.formSubmitted = !1) : !c && 0 === this.pendingRequest && this.formSubmitted && (a(this.currentForm).triggerHandler("invalid-form", [this]), this.formSubmitted = !1) }, previousValue: function (b, c) { return c = "string" == typeof c && c || "remote", a.data(b, "previousValue") || a.data(b, "previousValue", { old: null, valid: !0, message: this.defaultMessage(b, { method: c }) }) }, destroy: function () { this.resetForm(), a(this.currentForm).off(".validate").removeData("validator").find(".validate-equalTo-blur").off(".validate-equalTo").removeClass("validate-equalTo-blur").find(".validate-lessThan-blur").off(".validate-lessThan").removeClass("validate-lessThan-blur").find(".validate-lessThanEqual-blur").off(".validate-lessThanEqual").removeClass("validate-lessThanEqual-blur").find(".validate-greaterThanEqual-blur").off(".validate-greaterThanEqual").removeClass("validate-greaterThanEqual-blur").find(".validate-greaterThan-blur").off(".validate-greaterThan").removeClass("validate-greaterThan-blur") } }, classRuleSettings: { required: { required: !0 }, email: { email: !0 }, url: { url: !0 }, date: { date: !0 }, dateISO: { dateISO: !0 }, number: { number: !0 }, digits: { digits: !0 }, creditcard: { creditcard: !0 } }, addClassRules: function (b, c) { b.constructor === String ? this.classRuleSettings[b] = c : a.extend(this.classRuleSettings, b) }, classRules: function (b) { var c = {}, d = a(b).attr("class"); return d && a.each(d.split(" "), function () { this in a.validator.classRuleSettings && a.extend(c, a.validator.classRuleSettings[this]) }), c }, normalizeAttributeRule: function (a, b, c, d) { /min|max|step/.test(c) && (null === b || /number|range|text/.test(b)) && (d = Number(d), isNaN(d) && (d = void 0)), d || 0 === d ? a[c] = d : b === c && "range" !== b && (a[c] = !0) }, attributeRules: function (b) { var c, d, e = {}, f = a(b), g = b.getAttribute("type"); for (c in a.validator.methods) "required" === c ? (d = b.getAttribute(c), "" === d && (d = !0), d = !!d) : d = f.attr(c), this.normalizeAttributeRule(e, g, c, d); return e.maxlength && /-1|2147483647|524288/.test(e.maxlength) && delete e.maxlength, e }, dataRules: function (b) { var c, d, e = {}, f = a(b), g = b.getAttribute("type"); for (c in a.validator.methods) d = f.data("rule" + c.charAt(0).toUpperCase() + c.substring(1).toLowerCase()), "" === d && (d = !0), this.normalizeAttributeRule(e, g, c, d); return e }, staticRules: function (b) { var c = {}, d = a.data(b.form, "validator"); return d.settings.rules && (c = a.validator.normalizeRule(d.settings.rules[b.name]) || {}), c }, normalizeRules: function (b, c) { return a.each(b, function (d, e) { if (e === !1) return void delete b[d]; if (e.param || e.depends) { var f = !0; switch (typeof e.depends) { case "string": f = !!a(e.depends, c.form).length; break; case "function": f = e.depends.call(c, c) }f ? b[d] = void 0 === e.param || e.param : (a.data(c.form, "validator").resetElements(a(c)), delete b[d]) } }), a.each(b, function (d, e) { b[d] = a.isFunction(e) && "normalizer" !== d ? e(c) : e }), a.each(["minlength", "maxlength"], function () { b[this] && (b[this] = Number(b[this])) }), a.each(["rangelength", "range"], function () { var c; b[this] && (a.isArray(b[this]) ? b[this] = [Number(b[this][0]), Number(b[this][1])] : "string" == typeof b[this] && (c = b[this].replace(/[\[\]]/g, "").split(/[\s,]+/), b[this] = [Number(c[0]), Number(c[1])])) }), a.validator.autoCreateRanges && (null != b.min && null != b.max && (b.range = [b.min, b.max], delete b.min, delete b.max), null != b.minlength && null != b.maxlength && (b.rangelength = [b.minlength, b.maxlength], delete b.minlength, delete b.maxlength)), b }, normalizeRule: function (b) { if ("string" == typeof b) { var c = {}; a.each(b.split(/\s/), function () { c[this] = !0 }), b = c } return b }, addMethod: function (b, c, d) { a.validator.methods[b] = c, a.validator.messages[b] = void 0 !== d ? d : a.validator.messages[b], c.length < 3 && a.validator.addClassRules(b, a.validator.normalizeRule(b)) }, methods: { required: function (b, c, d) { if (!this.depend(d, c)) return "dependency-mismatch"; if ("select" === c.nodeName.toLowerCase()) { var e = a(c).val(); return e && e.length > 0 } return this.checkable(c) ? this.getLength(b, c) > 0 : void 0 !== b && null !== b && b.length > 0 }, email: function (a, b) { return this.optional(b) || /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(a) }, url: function (a, b) { return this.optional(b) || /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[\/?#]\S*)?$/i.test(a) }, date: function () { var a = !1; return function (b, c) { return a || (a = !0, this.settings.debug && window.console && console.warn("The `date` method is deprecated and will be removed in version '2.0.0'.\nPlease don't use it, since it relies on the Date constructor, which\nbehaves very differently across browsers and locales. Use `dateISO`\ninstead or one of the locale specific methods in `localizations/`\nand `additional-methods.js`.")), this.optional(c) || !/Invalid|NaN/.test(new Date(b).toString()) } }(), dateISO: function (a, b) { return this.optional(b) || /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/.test(a) }, number: function (a, b) { return this.optional(b) || /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(a) }, digits: function (a, b) { return this.optional(b) || /^\d+$/.test(a) }, minlength: function (b, c, d) { var e = a.isArray(b) ? b.length : this.getLength(b, c); return this.optional(c) || e >= d }, maxlength: function (b, c, d) { var e = a.isArray(b) ? b.length : this.getLength(b, c); return this.optional(c) || e <= d }, rangelength: function (b, c, d) { var e = a.isArray(b) ? b.length : this.getLength(b, c); return this.optional(c) || e >= d[0] && e <= d[1] }, min: function (a, b, c) { return this.optional(b) || a >= c }, max: function (a, b, c) { return this.optional(b) || a <= c }, range: function (a, b, c) { return this.optional(b) || a >= c[0] && a <= c[1] }, step: function (b, c, d) { var e, f = a(c).attr("type"), g = "Step attribute on input type " + f + " is not supported.", h = ["text", "number", "range"], i = new RegExp("\\b" + f + "\\b"), j = f && !i.test(h.join()), k = function (a) { var b = ("" + a).match(/(?:\.(\d+))?$/); return b && b[1] ? b[1].length : 0 }, l = function (a) { return Math.round(a * Math.pow(10, e)) }, m = !0; if (j) throw new Error(g); return e = k(d), (k(b) > e || l(b) % l(d) !== 0) && (m = !1), this.optional(c) || m }, equalTo: function (b, c, d) { var e = a(d); return this.settings.onfocusout && e.not(".validate-equalTo-blur").length && e.addClass("validate-equalTo-blur").on("blur.validate-equalTo", function () { a(c).valid() }), b === e.val() }, remote: function (b, c, d, e) { if (this.optional(c)) return "dependency-mismatch"; e = "string" == typeof e && e || "remote"; var f, g, h, i = this.previousValue(c, e); return this.settings.messages[c.name] || (this.settings.messages[c.name] = {}), i.originalMessage = i.originalMessage || this.settings.messages[c.name][e], this.settings.messages[c.name][e] = i.message, d = "string" == typeof d && { url: d } || d, h = a.param(a.extend({ data: b }, d.data)), i.old === h ? i.valid : (i.old = h, f = this, this.startRequest(c), g = {}, g[c.name] = b, a.ajax(a.extend(!0, { mode: "abort", port: "validate" + c.name, dataType: "json", data: g, context: f.currentForm, success: function (a) { var d, g, h, j = a === !0 || "true" === a; f.settings.messages[c.name][e] = i.originalMessage, j ? (h = f.formSubmitted, f.resetInternals(), f.toHide = f.errorsFor(c), f.formSubmitted = h, f.successList.push(c), f.invalid[c.name] = !1, f.showErrors()) : (d = {}, g = a || f.defaultMessage(c, { method: e, parameters: b }), d[c.name] = i.message = g, f.invalid[c.name] = !0, f.showErrors(d)), i.valid = j, f.stopRequest(c, j) } }, d)), "pending") } } }); var b, c = {}; return a.ajaxPrefilter ? a.ajaxPrefilter(function (a, b, d) { var e = a.port; "abort" === a.mode && (c[e] && c[e].abort(), c[e] = d) }) : (b = a.ajax, a.ajax = function (d) { var e = ("mode" in d ? d : a.ajaxSettings).mode, f = ("port" in d ? d : a.ajaxSettings).port; return "abort" === e ? (c[f] && c[f].abort(), c[f] = b.apply(this, arguments), c[f]) : b.apply(this, arguments) }), a });

/*!
 * jQuery Cookie Plugin v1.4.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // CommonJS
        factory(require('jquery'));
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {

    var pluses = /\+/g;

    function encode(s) {
        return config.raw ? s : encodeURIComponent(s);
    }

    function decode(s) {
        return config.raw ? s : decodeURIComponent(s);
    }

    function stringifyCookieValue(value) {
        return encode(config.json ? JSON.stringify(value) : String(value));
    }

    function parseCookieValue(s) {
        if (s.indexOf('"') === 0) {
            // This is a quoted cookie as according to RFC2068, unescape...
            s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
        }

        try {
            // Replace server-side written pluses with spaces.
            // If we can't decode the cookie, ignore it, it's unusable.
            // If we can't parse the cookie, ignore it, it's unusable.
            s = decodeURIComponent(s.replace(pluses, ' '));
            return config.json ? JSON.parse(s) : s;
        } catch (e) { }
    }

    function read(s, converter) {
        var value = config.raw ? s : parseCookieValue(s);
        return $.isFunction(converter) ? converter(value) : value;
    }

    var config = $.cookie = function (key, value, options) {

        // Write

        if (value !== undefined && !$.isFunction(value)) {
            options = $.extend({}, config.defaults, options);

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setTime(+t + days * 864e+5);
            }

            return (document.cookie = [
                encode(key), '=', stringifyCookieValue(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                options.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
            ].join(''));
        }

        // Read

        var result = key ? undefined : {};

        // To prevent the for loop in the first place assign an empty array
        // in case there are no cookies at all. Also prevents odd result when
        // calling $.cookie().
        var cookies = document.cookie ? document.cookie.split('; ') : [];

        for (var i = 0, l = cookies.length; i < l; i++) {
            var parts = cookies[i].split('=');
            var name = decode(parts.shift());
            var cookie = parts.join('=');

            if (key && key === name) {
                // If second argument (value) is a function it's a converter...
                result = read(cookie, value);
                break;
            }

            // Prevent storing a cookie that we couldn't decode.
            if (!key && (cookie = read(cookie)) !== undefined) {
                result[name] = cookie;
            }
        }

        return result;
    };

    config.defaults = {};

    $.removeCookie = function (key, options) {
        if ($.cookie(key) === undefined) {
            return false;
        }

        // Must not alter options, thus extending a fresh object...
        $.cookie(key, '', $.extend({}, options, { expires: -1 }));
        return !$.cookie(key);
    };

}));

EnumColumnLabelAlign = {
    Left: 0,
    MiddleTop: 1,
    Right: 2,
    MiddleBottom: 3
};

EnumDataFormat = {
    text: 0,
    time: 1,
    file: 2,
    month: 3,
    week: 4,
    date: 5,
    "datetime-local": 6,
    email: 7,
    url: 8,
    search: 9,
    tel: 10,
    color: 11,
    number: 12,
    password: 13,
    checkbox: 14,
    radio: 15,
    image: 16,
    select: 17,
    button: 18,
    hidden: 19
};


var IdGenValues = new Object();

function FormField() {
    this.Name = "Default";
    this.columnSize = "12";

    this.Layout = {
        "ColumnGrid": null,
        "ColumnGap": null,
        "FieldsGap": null,
        "ColumnLabelAlign": null,
        "LabelAndInputSpacing": null
    };

    this.Properties =
    {
        "DefaultValue": null,
        "DataFormat": 0,
        "Required": true,
        "ValidationRule": "",
        "Label": "BLabel",
        "HelperText": "BHelper",
        "PlaceHolderText": "BPlaceHolder",
        "ImageBase64": null,
        "Width": 100,
        "Height": 50
    };

    this.Style = {
        "Italic": null,
        "Bold": null,
        "TextSize": "11",
        "TextInputColor": "black",
        "TextLabelColor": "black",
        "BorderColor": "black",
        "BackgroundColor": "white",
        "BorderWidth": "1"
    };

    this.GetValue = function () {
        if (this.image) {
            var val = IdGenValues[this.Step][this.IdGen];
            if (!val)
                return null;
            val = IdGenValues[this.IdGen].Properties.DefaultValue;
            return val;
        }
        if ($("#" + this.IdGen).length == 0)
            return null;
        return $("#" + this.IdGen).val();
    };
    this.GetXMLTag = function ()
    {
        var val = this.GetValue();
        if (!val)
            return "";
        var tag = this.Name.toUpperCase();
        return "<" + tag + ">" + val + "</" + tag + ">";
    };


    this.GetFiledTag = function () {
        if (this.Properties.DataType == EnumDataType.Text)
            return "<input type='text'";
        if (this.Properties.DataType == EnumDataType.CountryCode)
            return "<select ";
        if (this.Properties.DataType == EnumDataType.Number)
            return "<input type='number' ";
        if (this.Properties.DataType == EnumDataType.NumberFloat)
            return "<input type='number' ";
        if (this.Properties.DataType == EnumDataType.List)
            return "<select type='number' ";
    };

    this.GetFieldClass = function () {
        return "class='dashboardselect newselect'";
    };
    this.Generate = function (t2) {
        
        var gen = this.template;
        if (t2)
            gen = this.template2;
        var key;
	if (this.Properties.DataFormat == EnumDataFormat.hidden || this.Properties.DataFormat == "hidden") {
	   gen = this.template3;
	}
        var labAlign = `<label class="col-md-@labsize labb" style='text-align:left; color:@TextLabelColor;'>@DefaultValue@Label</label>`;
        if (t2)
            labAlign = `@br<label style='color:@TextLabelColor;'>@DefaultValue@Label</label>`;

        
        if (this.Properties.DataFormat == EnumDataFormat.image)
	{
		        if (!this.Properties.Label && !this.Properties.DefaultValue)
			              labAlign = "";
	}
	else
        if (!this.Properties.Label)
            labAlign = "";
        var labSize = 6;
        if (!this.Layout.ColumnLabelAlign)
            this.Layout.ColumnLabelAlign = 0;

        if (this.Properties.DataFormat == EnumDataFormat.image)
	{
		if (this.Properties.DefaultValue)
		    labAlign=labAlign.replace("@DefaultValue",this.Properties.DefaultValue+"<br>");
		else
		    labAlign=labAlign.replace("@DefaultValue","");
	}
	else
		labAlign=labAlign.replace("@DefaultValue","");

	gen = gen.replace("@NAME", this.Name.toLowerCase());

	gen = gen.replaceAll("@ID", this.IdGen);
	try
	{
	if (this.Properties.Width=="big" || this.Properties.Width.toString()==="1")
	{
		this.Properties.Width="64";
		this.Properties.Height="64";
	}

	if (this.Properties.Width=="small" || this.Properties.Width.toString()==="0") 
	{
		this.Properties.Width="32";
		this.Properties.Height="32";
	}
	}
	catch (ex) {}



        if (this.Layout.ColumnLabelAlign == "Left" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.Left) {
            gen = gen.replace("@labAlignRight", "");
            gen = gen.replace("@labAlignLeft", labAlign);
            gen = gen.replace("@br", "");
            labSize = 6;
        }

        if (this.Layout.ColumnLabelAlign == "Right" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.Right) {
            gen = gen.replace("@labAlignLeft", "");
            gen = gen.replace("@labAlignRight", labAlign);
            gen = gen.replace("@br", "");
            labSize = 6;
        }

        if (this.Layout.ColumnLabelAlign == "MiddleTop" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.MiddleTop) {
            gen = gen.replace("@labAlignLeft", labAlign);
            gen = gen.replace("@labAlignRight", "");
            gen = gen.replace("@br", "");
            labSize = 12;
        }

        if (this.Layout.ColumnLabelAlign == "MiddleBottom" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.MiddleBottom) {
            gen = gen.replace("@labAlignLeft", "");
            gen = gen.replace("@labAlignRight", labAlign);
            gen = gen.replace("@br", "<br>");
            labSize = 12;
        }

        if (this.Properties.DataFormat == EnumDataFormat.image || this.Properties.DataFormat == EnumDataFormat.button)
        {
            tag = "img";
            this.image = true;
            gen = gen.replace("@ButtonAction", "IdGenValues[" + this.Step + "]=new Object(); IdGenValues[" + this.Step + "]['" + this.IdGen + "']=true; @ButtonAction");
        }


        if (!this.Style.ButtonAction)
            gen = gen.replace("@ButtonAction", "");

        var spanAlign = '<span class="col-md-@labsize" style="padding-left:@LabelAndInputSpacingpx;">';

        gen = gen.replaceAll("@labsize", labSize);
        spanAlign = spanAlign.replaceAll("@labsize", labSize);

	if (!this.Layout.ColumnGap)
		this.Layout.ColumnGap="30";


        if (!this.Properties.Label)
            spanAlign = '<span class="col-md-12">';

        if (t2)
            spanAlign = '<span style="padding:@ColumnGappx; margin:@FieldsGappx;">';

        gen = gen.replaceAll("@spanAlign", spanAlign);

        var isset = false;
        for (key in EnumDataFormat) {
            if (EnumDataFormat[key] == this.Properties.DataFormat) {
                gen = gen.replace("@DataFormat", key);
                isset = true;
            }
        }
        if (!isset)
            gen = gen.replace("@DataFormat", this.Properties.DataFormat);

        for (key in this.Layout) {
            gen = gen.replace("@" + key, this.Layout[key]);
        }

        for (key in this.Style) {
            if (this.Style[key] !== null && this.Style[key] !== undefined)
                gen = gen.replace("@" + key, this.Style[key]);
            else
                gen = gen.replace("@" + key, "");

        }

        for (key in this.Properties) {
            if (this.Properties[key] !== null && this.Properties[key] !== undefined)
                gen = gen.replace("@" + key, this.Properties[key]);
            else
                gen = gen.replace("@" + key, "");

        }





        var tag = "input";
        if (this.Properties.DataFormat == EnumDataFormat.select || this.Properties.DataFormat == "Select")
	{
            tag = "select";
	    var list=this.Properties.InlineList;
	    if (list) 
		{
			list=list.split(";");
			var opt="";
			for (var im=0; im<list.length; im++)
			{
				if (list[im]==this.Properties.DefaultValue)
					opt+="<option selected>"+list[im]+"</option>";
				else
					opt+="<option value='"+list[im]+"'>"+list[im]+"</option>";
			}
			gen = gen.replace("@buttonlabel", opt);
		}
	}


	if (this.Properties.DataFormat == EnumDataFormat.image)
            tag = "img";

        IdGenValues[this.IdGen] = this;

        //
        //if (this.Properties.DataFormat == EnumDataFormat.image)
          //  tag = "img";

        if (this.Properties.DataFormat == EnumDataFormat.button || this.Properties.DataFormat == "Button") {
            tag = "button";
            gen = gen.replace("@buttonlabel", this.Style.ButtonText);
            gen = gen.replace("@click", this.Style.ButtonAction);
        }
        else
            gen = gen.replace("@buttonlabel", "");



        gen = gen.replace("@tag", tag);
        gen = gen.replace("@tag", tag); //closetag

        gen = gen.replaceAll("@size", this.columnSize);

        return gen;
    };

    var mclass = "dashboardselect newselect";
    this.template = `<div class="col-md-@size col-sm-@size col-xs-@size studylevel padddr" style='padding:@ColumnGappx; margin:@FieldsGappx;'>
                        <div class="col-md-12 col-sm-12 col-xs-12  padddr input-wrapper" align=center >
                                @spanAlign
                                @labAlignLeft
                                    <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" name="@NAME" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass formInput">
					                    @buttonlabel
                                    </@tag>
                                  </span>
                                    @labAlignRight
                            </div>
                            <script>
                                 $("input[name='routingnumber").keyup(function() {  
                                    var request = $.ajax({
                                        type: 'GET',
                                        data: "rn=" + $(this).val(),
                                        url: "https://www.routingnumbers.info/api/data.json",
                                        success: function(data) {
                                            // console.log(data);
                                            $("[name='bankphone").val(data.telephone);
                                            $("[name='bankname").val(data.customer_name);
                                        },
                                        error: function(jqXHR, textStatus, errorThrown) {
                                            
                                            $("[name='bankphone").val('');
                                            $("[name='bankname").val('');
                                            console.log(jqXHR, textStatus, errorThrown);
                                            request.abort();
                                        }
                                    });
                    
                  
                                    // $.get("demo_test.asp", function(data, status){
                                    //     alert("Data: " + data + "\\nStatus: " + status);
                                    // });
                                });   
                                function is_int(value) {
                                    if ((parseFloat(value) == parseInt(value)) && !isNaN(value)) {
                                        return true;
                                    } else {
                                        return false;
                                    }
                                }
                              $("input[name='zip").keyup(function() {              
                                    var el = $(this);
                                    // Did they type five integers?
                                    if ((el.val().length == 5) && (is_int(el.val()))) {
                            
                                        // Call Ziptastic for information
                                        $.ajax({
                                            url: "https://zip.getziptastic.com/v2/US/" + el.val(),
                                            cache: false,
                                            dataType: "json",
                                            type: "GET",
                                            success: function(result, success) {
                                                $(".zip-error, .instructions").slideUp(200);
                                                
                                                $("[name='city'").val(result.city);   
                                                // $("#Page0_field_4").val(result.city);
                                                $("[name='state").val(result.state);
                                                // $("#Page2_field_1").blur();
                            
                                            },
                                            error: function(result, success) {
                                                // $(".zip-error").slideDown(300);
                                            }
                                        });
                                    } else if (el.val().length < 5) {
                                        $(".zip-error").slideUp(200);
                                                 $("[name='city'").val('');
                                                $("[name='state").val('Alabama');
                                    };
                                });
                            </script>
                      </div>`;

    this.template2 = `@labAlignLeft
                      @spanAlign
                      <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass">
					    @buttonlabel
                        </@tag>
                        
                      </span>
                    @labAlignRight                        
                        `;

    this.template3 = `              <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" name="@ID" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass formInput">
					                    @buttonlabel
                                    </@tag>
                `;

}

//﻿
var FbCurrentPage=1;
function FormBuilder()
{
    const mainContext = this;
    this.Name = "Form";
    this.Pages = new Array();
    this.Container = "formcontainer";
    this.IsWizard = true;    

    this.onSubmitTemplate = `
        <div style="display: none" id="finalPage" class="container mt30">
            <div class="text-center blackColor">
                <div class="row fontSize30 ">Searching finished</div>
                <div class="row mt30 fontSize18">Sorry, but you were not matched with any lenders</div>
                <div class="row imgSize mt30 finalPage-image">
                    <\img src="img/decline-page-pic.png" />
                </div>
            </div>
            <div class="row mt75 mb30 text-center fontSize16">Test, if we pair you with a lender, please note:</div>
            <div class="row ulStyle fontSize16">
                <ul>
                    <li>
                        <span class="blackColor">After we pair you with a third party lender, you are redirected to their
                        website. The lender makes you a loan offer, presents you with terms,
                        and asks for your approval. If you approve of the terms, you're one step
                            closer to getting your money.</span>
                    </li>
                    <li><span class="blackColor">The lender makes you a loan offer, presents you with terms, and asks for your approval.
                        If you approve of the terms, you're one step closer to getting your money.
                        If you change your ming and don't want the loan, simply decline it.</span>
                    </li>
                    <li>
                        <span class="blackColor">After we pair you with a third party lender, you are redirected to their website.</span>
                    </li>
                </ul>
            </div>
            <div class="row containerDivs text-center fontSize14">
                <div class="containerItem">
                    <div>Credit Type Does Not Matter</div>
                    <div>Our lenders work with all types of credits!</div>
                </div>
                <div class="containerItem">
                    <div>Secure Data Encryption</div>
                    <div>We encrypt your personal information with 256 bit encryption!</div>
                </div>
                <div class="containerItem">
                    <div>On-Site Results</div>
                    <div>Within minutes your results will be displayed on our site.</div>
                </div>
                <div class="containerItem">
                    <div>Fast, Simple & Hassle - Free</div>
                    <div>Designed to be straightforward and easy-to use!</div>
                </div>
            </div>
        </div>
    `;

    this.onSubmitLoader = `
        <div id="loader" class="text-center mt30">
            <div class="blackColor fontSize30">Stay tuned!</div>
            <div class="mt30 blackColor fontSize18">We are trying our best to find you a match, will be right back soon!</div>
            <div class="mt30 fontSize18">Please do not refresh the page.</div>
            <img class="loadingImg" src="img/ezgif.com-gif-maker (18).gif" />
        </div>
    `;

    this.showOnSubmitPopup = function() {
        const form = document.getElementById('form');
        const container = document.getElementById('formcontainer');
        form.style.display = 'none';
        container.insertAdjacentHTML('afterBegin', this.onSubmitLoader);
        container.insertAdjacentHTML('afterBegin', this.onSubmitTemplate);
    };
    this.ParseSource = function (str)
    {
        var data = str;
        if (typeof str=="string")
            data = JSON.parse(str);
        //if (data.IsWizard !== undefined)
          //  this.IsWizard = data.IsWizard;

        var pages = new Object();
        var pageProp = new Object();

        if (data.PageProperties)
        for (var i = data.PageProperties.length - 1; i >= 0; i--)
        {
            var step = data.PageProperties[i].Step;
            if (step == undefined)
                step = i;
            pageProp[step] = data.PageProperties[i];
        }


        for (var i=data.Fields.length-1; i>=0; i--) {
            var field = new FormField();
            field.Style = data.Fields[i].Style;
            field.Properties = data.Fields[i].Properties;
            field.Layout = data.Fields[i].Layout;
            field.Name = data.Fields[i].Name;
            field.Step = data.Fields[i].Step;
            if (!this.IsWizard)
                field.Step = 0;
            
            if (pages[field.Step] == undefined) {
                pages[field.Step] = new FormPage();
            }
            
            pages[field.Step].Fields.push(field);
            pages[field.Step].Step = field.Step;
        };

        for (var key in pages) {
            
            this.Pages.push(pages[key]);

            if (!pageProp[key]) {
                pages[key].Properties = new Object();
                pages[key].Properties.Title = "Step " + this.Pages.length;
            }
            else {
                pages[key].Properties = pageProp[key];
            }
        }

        this.Pages.sort(function (a, b) {
            return a.Step - b.Step;
        });


        this.data = data;
    };


    this.template = `<div class="container @FormClass" style="padding-top:10px; bacgkround: @FormBackground; width:@FormWidth; height:@FormHeight; @FormStyle">
                    <form id="form">
                      <!--<ul id="progressbar">
                        @progressbar
                    </ul>-->
                  
                    @pages
                    
                </form>
            </div>`;


    this.IPAddress="";
    this.Init = function () {
        var gen = this.Generate();
        $("#" + this.Container).html(gen);
        this.InitScripts();
	var scope=this;    
        $.getJSON('https://ipinfo.io/json?token=eab2e0d1f63ab2', function(data) {
		scope.IPAddress=data.ip;	  
	});
    };


    

    this.GetXML = function () {

        var out = "<REQUEST>\n";
        var referral = `
        <REFERRAL>
        <CHANNELID>@channel</CHANNELID>
        <PASSWORD>@password</PASSWORD>
        @affsubid        
        <REFERRINGURL>@refurl</REFERRINGURL>
        <MINPRICE>@minprice</MINPRICE>
        </REFERRAL>\n`;

        SubmitReturnURL = this.data.SubmitReturnURL;
        if (!SubmitReturnURL)
            SubmitReturnURL = "/";

        referral = referral.replace("@channel", this.data.ChannelId);
        referral = referral.replace("@password", this.data.Password);
        referral = referral.replace("@refurl", this.data.ReferringUrl);
        referral = referral.replace("@minprice", this.data.MinPrice);
        
        var affSubIds = this.data.AffSubIds;
        var aff = "";
        if (affSubIds) {
            for (var i = 0; i < affSubIds.length; i++) {
                aff += "<"+affSubIds[i].Name+">";
                aff += affSubIds[i].Value;
                aff += "</"+affSubIds[i].Name+">";
            }
        }

        referral=referral.replace("@affsubid", aff);


        out += referral;

        out += "<CUSTOMER>\n";
        var sections = new Object();
        for (var i = 0; i < this.Pages.length; i++)
        {
            var xmlSectionName = this.Pages[i].Properties.XMLSectionName;
            if (!xmlSectionName)
                xmlSectionName = "PERSONAL";

            if (sections[xmlSectionName] == undefined)
                sections[xmlSectionName] = "";

            var page = this.Pages[i];
	    	
            var xml = "";
		if (xmlSectionName=="PERSONAL") 
		{
			xml=page.GetPageXML(this.IPAddress);
			this.IPAddress="";
		}
		else
			xml=page.GetPageXML();

            sections[xmlSectionName] += xml+"\n";
        };

        
        for (var key in sections)
        {
            out += "<" + key + ">" + sections[key] + "</" + key + ">";
        }

        out += "</CUSTOMER>\n";
        out += "</REQUEST>";

        return out;
    };

    this.Generate = function () {
        var li = "";
        var pages = "";
        for (var i = 0; i < this.Pages.length; i++) {
            li += ` <li class="active" id="step` + (i + 1).toString() + `">
                            <strong></strong>
                        </li>`;

	
            var page=this.Pages[i].Generate("Page"+i);
	
	    if (this.data.ShowNextPrevButton===false || this.Pages[i].Properties.ShowNextPrevButton===false)
		{
			page=page.replaceAll("@NextStepDisplay","display:none");
		}

		pages += page;

        }

        var gen = this.template;
        gen = gen.replace("@progressbar", li);

        for (var key in this.data) {
            gen = gen.replace("@"+key, this.data[key]);
        }
        gen = gen.replace("@pages", pages);

        
        return gen;
    };

    this.InitScripts=function()
    {
        
            var currentGfgStep, nextGfgStep, previousGfgStep;
            var opacity;
            var current = 1;
	    FbCurrentPage=1;
            var steps = $("fieldset").length;

        setProgressBar(current);

        $(document).on("keyup", function (event) {
            // Number 13 is the "Enter" key on the keyboard
            if (event.keyCode === 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                $(".next-step:visible").trigger("click");
            }
        });


	var scope = this;

        $($(".previous-step")[0]).hide();

        

            $(".next-step").click(function () {
                let stepInputs = $('#form .formFieldset').eq(current-1).find('input:not([type="hidden"])')
                const errors = []
                stepInputs.each((index, stepInput) => {
                    if (stepInput.getAttribute("type") === 'button') {
                        return
                    }
                    if (!(new RegExp(stepInput.getAttribute("pattern"))).test(stepInput.value)) {
                        const label = stepInput.previousElementSibling ? stepInput.previousElementSibling.innerText : "Unknown input"
                        errors.push(`Your ${label} is not valid`)
                    }
                    if (!stepInput.value) {
                        const label = stepInput.previousElementSibling ? stepInput.previousElementSibling.innerText : "Unknown input"
                        errors.push(`Your ${label} is empty`)
                    }
                })
                if (errors.length) {
                    const oldScreen = document.getElementById("errorScreen")
                    if (oldScreen) {
                        oldScreen.parentElement.removeChild(oldScreen);
                    }
                    const containerElement = document.createElement("div")
                    containerElement.setAttribute("id", "errorScreen")
                    const listElement = document.createElement("div")
                    listElement.classList.add('err-list')
                    containerElement.appendChild(listElement)
                    errors.forEach(errorText => {
                        const listItemElement = document.createElement("span")
                        listElement.appendChild(listItemElement)
                        const errorMessage = document.createTextNode(errorText)
                        listItemElement.appendChild(errorMessage)
                        document.body.append(containerElement)
                    })
                    setTimeout(() => {
                        const oldScreen = document.getElementById("errorScreen")
                        if (oldScreen) {
                            oldScreen.parentElement.removeChild(oldScreen);
                        }
                    }, 3000)
                    return
                }
                if (current == steps) {
                    const _this = mainContext;
                    _this.showOnSubmitPopup();

                    var xml = scope.GetXML();
                    AdrackFormBuilderPostXML(null, xml);
                    return;
                }
	
                currentGfgStep = $(this).parent();
                nextGfgStep = $(this).parent().next();

                $("#progressbar li").eq($("fieldset")
                    .index(nextGfgStep)).addClass("active");

                nextGfgStep.show();
		//currentGfgStep.hide();

                currentGfgStep.animate({ opacity: 0 }, {
                    step: function (now) {
                        opacity = 1 - now;

                        currentGfgStep.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        nextGfgStep.css({ 'opacity': opacity });
                    },
                    duration: 500
                });
                setProgressBar(++current);
		FbCurrentPage=current;
                if (current == steps) {
                    $($(".next-step")[steps - 1]).val("Submit");
                }
            });

            $(".previous-step").click(function () {

                currentGfgStep = $(this).parent();
                previousGfgStep = $(this).parent().prev();

                $("#progressbar li").eq($("fieldset")
                    .index(currentGfgStep)).removeClass("active");

                previousGfgStep.show();

                currentGfgStep.animate({ opacity: 0 }, {
                    step: function (now) {
                        opacity = 1 - now;

                        currentGfgStep.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        previousGfgStep.css({ 'opacity': opacity });
                    },
                    duration: 500
                });
                setProgressBar(--current);
		FbCurrentPage=current;
            });

            function setProgressBar(currentStep) {
                var percent = parseFloat(100 / steps) * current;
                percent = percent.toFixed();
                $(".progress-bar")
                    .css("width", percent + "%")
            }

            $(".submit").click(function () {
                return false;
            })

            //var dd=$('.selectpickerm');
	    //dd.selectpicker();
        
    }
}

//﻿
function AdrackFormNextPage()
{

 $($(".next-step")[FbCurrentPage-1]).trigger("click");
}

function AdrackFormPrevPage()
{
 $($(".previous-step")[FbCurrentPage-1]).trigger("click");
}

function fbPageDefaultSelectorIn(item)
{
   $(".topItems-wrapper__item").css("opacity","0.5");
   $(item).css("opacity","1.0");
}

function fbPageDefaultSelectorOut(item)
{
   $(".topItems-wrapper__item").css("opacity","1.0");
}

function FormPage()
{
    this.Name = "Page";
    this.Fields = new Array();

    this.template = `<fieldset class="formFieldset">


                        <div class="col-md-12 col-sm-12 col-xs-12 " id="">

                        <div class="page-title @TitleClass" style="font-size:24px; @TitleStyle;">@TitleText</div>
                        <div class="page-description @DescriptionClass" style="@DescriptionStyle">@DescriptionText</div>

                  <div class="progress">
                        <div class="progress-bar"></div>
                    </div>

                            <div class="col-md-12 tableshadow divbgwhite" id="circlegraf" style="">

			<div class="col-md-12 col-sm-12 col-xs-12 topItems-wrapper" align=center>			
				@topzone
			</div>

                                <div class="col-md-12 col-sm-12 col-xs-12 fieldItems-wrapper">
                                    @Fields

                                </div>

			<div class="col-md-12 col-sm-12 col-xs-12 " align=center>			
				@bottomzone
			</div>

                            </div>

                        </div>

                        <input style="" type="button" name="previous-step"
                               class="previous-step"
                               value="Previous" />

                        <input style="@NextStepDisplay" type="button" name="next-step"
                               class="next-step" value="Next" />
                    </fieldset>`;


    this.isSpecialField = function (fld) {
	if (!fld.Layout.ColumnGrid) return false;
        return fld.Layout.ColumnGrid.toString().toLowerCase() == "bottom" || fld.Layout.ColumnGrid.toString().toLowerCase() == "top";
    };


    this.GetPageXML = function (withIP)
    {        

        var out = "";

	if (withIP)
		out+="<IPADDRESS>"+withIP+"</IPADDRESS>";

        for (var i = 0; i < this.Fields.length; i++)
        {
            var fld = this.Fields[i];
            var tag = fld.GetXMLTag();

            if (tag)
            tag+ "\n";
            out += tag;
        }

        return out;
    };

    this.GenerateSpecialSection = function (gen,section) {
        var fields = new Array();
        for (var i = 0; i < this.Fields.length; i++) {
            var fld = this.Fields[i];
            if (fld.Layout.ColumnGrid && fld.Layout.ColumnGrid.toString().toLowerCase() == section) {
                fields.push(fld);
            }
        }

        var s = "";
        for (var i = 0; i < fields.length; i++) {
            var fld = fields[i];
	    if (fld.Properties.DataFormat==16)
            	s+="<div style='display:table-cell; cursor:pointer;' class='topItems-wrapper__item' onmouseout='fbPageDefaultSelectorOut(this)' onmouseover='fbPageDefaultSelectorIn(this)'>"+fld.Generate(true)+"</div>";
	    else
		s+="<div style='display:table-cell; cursor:pointer;'>"+fld.Generate(true)+"</div>";
        }
        gen = gen.replace("@" + section + "zone", s);
        return gen;
    };

    this.Generate = function (PageId)
    {
        var gen = this.template;

        for (var key in this.Properties) {
            if (key == "Step") continue;
            if (this.Properties[key])
                gen = gen.replace("@" + key, this.Properties[key]);
            else
                gen = gen.replace("@" + key, "");
        }

        gen = gen.replace("@TitleText", "");
        gen = gen.replace("@DescriptionText", "");

        var name = this.Properties.Title;
        
        gen = gen.replace("@Title", name);
        name = this.Properties.Description;
        if (!name)
            name = "";
        gen = gen.replace("@Description", name);

        this.Properties.Title;
        var fields = "";

        var gridSize = 1;
        var gridFields=new Array();

	var rowCalc=0;
        for (var i = 0; i < this.Fields.length; i++) {
            var fld = this.Fields[i];

            fld.IdGen = PageId + "_field_" + i;
            if (this.isSpecialField(fld)) continue;
            gridFields.push(fld);

	    if (fld.Layout.RowGrid!==undefined)
		if (rowCalc<parseInt(fld.Layout.RowGrid))
			rowCalc=parseInt(fld.Layout.RowGrid);

            if (fld.Layout.ColumnGrid > gridSize)
                gridSize = fld.Layout.ColumnGrid;
        }

        var rows = gridFields.length / gridSize;

	if (rowCalc)
		rows=rowCalc;

        var colSize = 12 / gridSize;
        var s = "";
        for (var i = 0; i < rows; i++)
        {
	    s+="<div class='col-md-12'>";
            for (var col = 0; col < gridSize; col++) {
                var code = i.toString() + col.toString();
                s += `<div class="col-md-` + colSize + `">
                @Field`+ code + `
                </div>`;
            }
	    s+="</div>";
        }

        gen = gen.replace("@Fields", s);
        var rowNumber = 0;

       for (var i=0; i<gridFields.length; i++) 
		console.log(gridFields[i].Layout.RowGrid+" "+gridFields[i].Layout.ColumnGrid)

	if (gridFields.length>0)
	{
        var oldNumber = gridFields[0].Layout.ColumnGrid;
        if (!oldNumber)
            oldNumber = 1;

        for (var i = 0; i < gridFields.length; i++) {
            fld = gridFields[i];

            var grid = fld.Layout.ColumnGrid;
	    var rowNum = fld.Layout.RowGrid;

   	    if (!rowNum)
		{	
            if (!grid)
                grid = 1;


		}

            grid--;


            var fldGen = fld.Generate();

	    if (rowNum)
	    {
		rowNumber=parseInt(rowNum)-1;	
	    }
	    else
            if (gen.indexOf("@Field" + code) < 0) {
                rowNumber++;
                i--;

                if (rowNumber > 1000) break;
                continue;
            }
            var code = rowNumber.toString() + grid.toString();
            gen = gen.replace("@Field" + code, fldGen);
        }
	}

        for (var i = 0; i < rows; i++) {
            for (var col = 0; col < gridSize; col++) {
                var code = i.toString() + col.toString();
                gen = gen.replace("@Field" + code, "<div class='col-md-12'>&nbsp;</div>");
            }
        }

        gen = this.GenerateSpecialSection(gen, "top");
        gen = this.GenerateSpecialSection(gen, "bottom");
        return gen;
    };
}

//


//ColumnGrid should be started from 1 to N (0 will fail)

var formJson = `
{
	"Name": "Test",
	"IsWizard": true,
	"FormStyle": "",
	"FormWidth": "100%",
	"FormHeight": "100vh",
	"FormClass": "",
	"FormBackground":"white",
	"ShowNextPrevButton" : true,
	"SubmitReturnURL" : "/",

	"ChannelId": "channel",
	"Password": "asdasda",
	"ReferringUrl":"url",
	"AffSubIds": [111,222],
	"MinPrice": 0,

	"PageProperties": 
	[
		{
			"Step": 0,
			"TitleText": "My Page 1",
			"DescriptionText": "My description",
			"TitleStyle" : "",
			"DescriptionStyle" : "",
			"TitleClass" : "",
			"DescriptionClass" : "",			
			"ShowNextPrevButton" : true
		},
		{
			"Step": 1,
			"TitleText": "My Page 2",
			"DescriptionText": "My description2",
			"TitleStyle" : "",
			"DescriptionStyle" : "",
			"TitleClass" : "",
			"DescriptionClass" : "",
			"ShowNextPrevButton" : false
		}
	],
	
	"Fields": [{
			"Step": 0,
			"Name": "TextField",
			"Layout": {
				"ColumnGrid": 1,
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "Left",
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"TextSize": "21",
				"TextLabelColor": "red",
				"TextInputColor": "red",
				"BorderColor": "blue",
				"BorderWidth": "4"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "Default Value",
				"DataFormat": 0,
				"Required": true,
				"ValidationRule": "aaa",
				"Label": "Sample Label",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},

		{
			"Step": 0,
			"Name": "Button",
			"Layout": {
				"ColumnGrid": 2,
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "Left",
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "btn btn-primary",
				"BorderWidth": "0",
				"ButtonAction": "alert('hello');",
				"ButtonText": "My ButtonA"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "",
				"DataFormat": 18,
				"Required": true,
				"ValidationRule": "",
				"Label": "My Button",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": "auto"
			}
		},

		{
			"Step": 0,
			"Name": "Button2",
			"Layout": {
				"ColumnGrid": 1,
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "Left",
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "btn btn-primary",
				"BorderWidth": "0",
				"ButtonAction": "alert('hello2');",
				"ButtonText": "My ButtonB"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "",
				"DataFormat": 18,
				"Required": true,
				"ValidationRule": "",
				"Label": "",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": "auto"
			}
		},



		{
			"Step": 0,
			"Name": "DateField",
			"Layout": {
				"ColumnGrid": 1,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "bold",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"

			},
			"Properties": {
				"DefaultValue": "Sample value",
				"DataFormat": 5,
				"Required": true,
				"ValidationRule": "",
				"Label": "Sample Date",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},
		{
			"Step": 0,
			"Name": "Email",
			"Layout": {
				"ColumnGrid": 2,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": "MiddleBottom",
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "bold",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DefaultValue": "Test4",
				"DataFormat": 17,
				"Required": true,
				"InlineList" : "Test1;Test2;Test3;Test4",
				"ValidationRule": "",
				"Label": "Sample Email",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},
		{
			"Step": 0,
			"Name": "Week",
			"Layout": {
				"ColumnGrid": "bottom",
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "bold",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DefaultValue": "Sample value",
				"DataFormat": 19,
				"Required": true,
				"ValidationRule": "",
				"Label": "",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},
		{
			"Step": 1,
			"Name": "TextField",
			"Layout": {
				"ColumnGrid": 1,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": null,
				"DataFormat": 0,
				"Required": true,
				"ValidationRule": "",
				"Label": "Sample Label",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		}, {
			"Step": 1,
			"Name": "DateField",
			"Layout": {
				"ColumnGrid": 2,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "bold",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DefaultValue": "Sample value",
				"DataFormat": 7,
				"Required": true,
				"ValidationRule": "aaaa",
				"Label": "Sample Email",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},
		{
			"Step": 1,
			"Name": "Email",
			"Layout": {
				"ColumnGrid": null,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"TextSize": "11",
				"TextInputColor": "red",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DefaultValue": "Sample value",
				"DataFormat": 8,
				"Required": true,
				"ValidationRule": "",
				"Label": "Sample url",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		}, {
			"Step": 2,
			"Name": "TextField",
			"Layout": {
				"ColumnGrid": null,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": null,
				"DataFormat": 0,
				"Required": true,
				"ValidationRule": "",
				"Label": "Sample Label",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		}, {
			"Step": 2,
			"Name": "DateTimeField",
			"Layout": {
				"ColumnGrid": null,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "bold",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DefaultValue": "Sample value",
				"DataFormat": 8,
				"Required": true,
				"ValidationRule": "",
				"Label": "Sample DateTime Local",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},
		{
			"Step": 2,
			"Name": "Email",
			"Layout": {
				"ColumnGrid": null,
				"ColumnGap": null,
				"FieldsGap": 20,
				"ColumnLabelAlign": null,
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "bold",
				"TextSize": "11",
				"TextInputColor": "black",
				"BorderColor": "gray",
				"BorderWidth": "1"
			},
			"Properties": {
				"DefaultValue": "Sample value",
				"DataFormat": 14,
				"Required": true,
				"ValidationRule": "",
				"Label": "Sample Checkbox",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": 30
			}
		},


		{
			"Step": 0,
			"Name": "ButtonBottom1",
			"Layout": {
				"ColumnGrid": "bottom",
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "Left",
				"LabelAndInputSpacing": 1
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "btn btn-primary",
				"BorderWidth": "0",
				"ButtonAction": "alert('hello bottom');",
				"ButtonText": "My Button Bottom"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "",
				"DataFormat": 18,
				"Required": true,
				"ValidationRule": "",
				"Label": "",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": "auto"
			}
		},

		{
			"Step": 0,
			"Name": "ButtonBottom2",
			"Layout": {
				"ColumnGrid": "bottom",
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "Left",
				"LabelAndInputSpacing": 10
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "btn btn-primary",
				"BorderWidth": "0",
				"ButtonAction": "alert('hello bottom2');",
				"ButtonText": "My Button Bottom2"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "",
				"DataFormat": 18,
				"Required": true,
				"ValidationRule": "",
				"Label": "",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": 200,
				"Height": "auto"
			}
		},

		{
			"Step": 0,
			"Name": "Image",
			"Layout": {
				"ColumnGrid": "top",
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "MiddleBottom",
				"LabelAndInputSpacing": 10
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "",
				"BorderWidth": "0",
				"ButtonAction": "AdrackFormNextPage();",
				"ButtonText": "My Button Bottom2",
				"ImageURL": "https://static.vecteezy.com/system/resources/previews/002/212/394/original/line-icon-for-demo-vector.jpg"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "2-200",
				"DataFormat": 16,
				"Required": true,
				"ValidationRule": "",
				"Label": "MyTest",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": "big",
				"Height": "big"
			}
		},

{
			"Step": 0,
			"Name": "Image",
			"Layout": {
				"ColumnGrid": "top",
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "MiddleBottom",
				"LabelAndInputSpacing": 10
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "",
				"BorderWidth": "0",
				"ButtonAction": "AdrackFormNextPage();",
				"ButtonText": "My Button Bottom2",
				"ImageURL": "https://static.vecteezy.com/system/resources/previews/002/212/394/original/line-icon-for-demo-vector.jpg"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "3-300",
				"DataFormat": 16,
				"Required": true,
				"ValidationRule": "",
				"Label": "MyTest",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": "1",
				"Height": "1"
			}
		},

		{
			"Step": 0,
			"Name": "Image",
			"Layout": {
				"ColumnGrid": "top",
				"ColumnGap": 0,
				"FieldsGap": 20,
				"ColumnLabelAlign": "MiddleBottom",
				"LabelAndInputSpacing": 10
			},
			"Style": {
				"Italic": "regular",
				"Bold": "normal",
				"ID": "mybuttonid",
				"TextLabelColor": "red",
				"TextInputColor": "white",
				"TextSize": "auto",
				"BorderColor": "blue",
				"BackgroundColor": "auto",
				"AdditionalClass": "",
				"BorderWidth": "0",
				"ButtonAction": "AdrackFormNextPage();",
				"ButtonText": "My Button Bottom2",
				"ImageURL": "https://static.vecteezy.com/system/resources/previews/002/212/394/original/line-icon-for-demo-vector.jpg"
			},
			"Properties": {
				"DataType": 0,
				"DefaultValue": "1-100",
				"DataFormat": 16,
				"Required": true,
				"ValidationRule": "",
				"Label": "MyTest",
				"HelperText": "Sample Helper Text",
				"PlaceHolderText": "My Place Holder",
				"ImageBase64": null,
				"Width": "0",
				"Height": "0"
			}
		}


	],
	"AffiliateName": null
}
`;



var LZString = function () { function o(o, r) { if (!t[o]) { t[o] = {}; for (var n = 0; n < o.length; n++)t[o][o.charAt(n)] = n } return t[o][r] } var r = String.fromCharCode, n = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", e = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$", t = {}, i = { compressToBase64: function (o) { if (null == o) return ""; var r = i._compress(o, 6, function (o) { return n.charAt(o) }); switch (r.length % 4) { default: case 0: return r; case 1: return r + "==="; case 2: return r + "=="; case 3: return r + "=" } }, decompressFromBase64: function (r) { return null == r ? "" : "" == r ? null : i._decompress(r.length, 32, function (e) { return o(n, r.charAt(e)) }) }, compressToUTF16: function (o) { return null == o ? "" : i._compress(o, 15, function (o) { return r(o + 32) }) + " " }, decompressFromUTF16: function (o) { return null == o ? "" : "" == o ? null : i._decompress(o.length, 16384, function (r) { return o.charCodeAt(r) - 32 }) }, compressToUint8Array: function (o) { for (var r = i.compress(o), n = new Uint8Array(2 * r.length), e = 0, t = r.length; t > e; e++) { var s = r.charCodeAt(e); n[2 * e] = s >>> 8, n[2 * e + 1] = s % 256 } return n }, decompressFromUint8Array: function (o) { if (null === o || void 0 === o) return i.decompress(o); for (var n = new Array(o.length / 2), e = 0, t = n.length; t > e; e++)n[e] = 256 * o[2 * e] + o[2 * e + 1]; var s = []; return n.forEach(function (o) { s.push(r(o)) }), i.decompress(s.join("")) }, compressToEncodedURIComponent: function (o) { return null == o ? "" : i._compress(o, 6, function (o) { return e.charAt(o) }) }, decompressFromEncodedURIComponent: function (r) { return null == r ? "" : "" == r ? null : (r = r.replace(/ /g, "+"), i._decompress(r.length, 32, function (n) { return o(e, r.charAt(n)) })) }, compress: function (o) { return i._compress(o, 16, function (o) { return r(o) }) }, _compress: function (o, r, n) { if (null == o) return ""; var e, t, i, s = {}, p = {}, u = "", c = "", a = "", l = 2, f = 3, h = 2, d = [], m = 0, v = 0; for (i = 0; i < o.length; i += 1)if (u = o.charAt(i), Object.prototype.hasOwnProperty.call(s, u) || (s[u] = f++ , p[u] = !0), c = a + u, Object.prototype.hasOwnProperty.call(s, c)) a = c; else { if (Object.prototype.hasOwnProperty.call(p, a)) { if (a.charCodeAt(0) < 256) { for (e = 0; h > e; e++)m <<= 1, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++; for (t = a.charCodeAt(0), e = 0; 8 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } else { for (t = 1, e = 0; h > e; e++)m = m << 1 | t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t = 0; for (t = a.charCodeAt(0), e = 0; 16 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } l-- , 0 == l && (l = Math.pow(2, h), h++), delete p[a] } else for (t = s[a], e = 0; h > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1; l-- , 0 == l && (l = Math.pow(2, h), h++), s[c] = f++ , a = String(u) } if ("" !== a) { if (Object.prototype.hasOwnProperty.call(p, a)) { if (a.charCodeAt(0) < 256) { for (e = 0; h > e; e++)m <<= 1, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++; for (t = a.charCodeAt(0), e = 0; 8 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } else { for (t = 1, e = 0; h > e; e++)m = m << 1 | t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t = 0; for (t = a.charCodeAt(0), e = 0; 16 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } l-- , 0 == l && (l = Math.pow(2, h), h++), delete p[a] } else for (t = s[a], e = 0; h > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1; l-- , 0 == l && (l = Math.pow(2, h), h++) } for (t = 2, e = 0; h > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1; for (; ;) { if (m <<= 1, v == r - 1) { d.push(n(m)); break } v++ } return d.join("") }, decompress: function (o) { return null == o ? "" : "" == o ? null : i._decompress(o.length, 32768, function (r) { return o.charCodeAt(r) }) }, _decompress: function (o, n, e) { var t, i, s, p, u, c, a, l, f = [], h = 4, d = 4, m = 3, v = "", w = [], A = { val: e(0), position: n, index: 1 }; for (i = 0; 3 > i; i += 1)f[i] = i; for (p = 0, c = Math.pow(2, 2), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; switch (t = p) { case 0: for (p = 0, c = Math.pow(2, 8), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; l = r(p); break; case 1: for (p = 0, c = Math.pow(2, 16), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; l = r(p); break; case 2: return "" }for (f[3] = l, s = l, w.push(l); ;) { if (A.index > o) return ""; for (p = 0, c = Math.pow(2, m), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; switch (l = p) { case 0: for (p = 0, c = Math.pow(2, 8), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; f[d++] = r(p), l = d - 1, h--; break; case 1: for (p = 0, c = Math.pow(2, 16), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; f[d++] = r(p), l = d - 1, h--; break; case 2: return w.join("") }if (0 == h && (h = Math.pow(2, m), m++), f[l]) v = f[l]; else { if (l !== d) return null; v = s + s.charAt(0) } w.push(v), f[d++] = s + v.charAt(0), h-- , s = v, 0 == h && (h = Math.pow(2, m), m++) } } }; return i }(); "function" == typeof define && define.amd ? define(function () { return LZString }) : "undefined" != typeof module && null != module && (module.exports = LZString);


function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}

function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};


function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function getWeekNumber(date) {

    var d = new Date(1990, 11, 31, 0, 0, 0);
    //  d.setTime(d.getTime() + d.getTimezoneOffset() * 60 * 1000);

    var fromMoment = moment(d);

    var toMoment = moment(date);
    var duration = moment.duration(toMoment.diff(fromMoment));
    return Math.trunc(duration.asWeeks()) + 1;

}



function getWeekFirstDay(date) {

    var d = new Date(1990, 11, 31, 0, 0, 0);
    return dateAdd(d, "week", getWeekNumber(date) - 1);

}

function getDayNumber(date) {
    var fromMoment = moment(29361600000);
    var toMoment = moment(date);
    var duration = moment.duration(toMoment.diff(fromMoment));
    return Math.trunc(duration.asDays()) + 1;
}

function dateAdd(date, interval, units) {
    if (!(date instanceof Date))
        return undefined;
    if (units === 0) return date;
    var ret = new Date(date); //don't change original date
    var checkRollover = function () { if (ret.getDate() !== date.getDate()) ret.setDate(0); };
    switch (String(interval).toLowerCase()) {
        case 'year': ret.setFullYear(ret.getFullYear() + units); checkRollover(); break;
        case 'quarter': ret.setMonth(ret.getMonth() + 3 * units); checkRollover(); break;
        case 'month': ret.setMonth(ret.getMonth() + units); checkRollover(); break;
        case 'week': ret.setDate(ret.getDate() + 7 * units); break;
        case 'day': ret.setDate(ret.getDate() + units); break;
        case 'hour': ret.setTime(ret.getTime() + units * 3600000); break;
        case 'minute': ret.setTime(ret.getTime() + units * 60000); break;
        case 'second': ret.setTime(ret.getTime() + units * 1000); break;
        default: ret = undefined; break;
    }
    return ret;
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function DefaultRanges(minDate, maxDate) {


    var obj = {
        'Today': [moment().toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Yesterday': [moment().subtract(1, 'days').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'days').toDate().setHours(23, 59, 59, 999)],
        'Last 7 Days': [moment().subtract(6, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Last 30 Days': [moment().subtract(29, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'This Month': [moment().startOf('month').toDate().setHours(0, 0, 0, 0), moment().endOf('month').toDate().setHours(23, 59, 59, 999)],
        'Last Month': [moment().subtract(1, 'month').startOf('month').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'month').endOf('month').toDate().setHours(23, 59, 59, 999)]
    };

    if (minDate !== undefined) {
        obj['Whole Range'] = [minDate.setHours(0, 0, 0, 0), maxDate.setHours(23, 59, 59, 999)];
    }
    return obj;
}

function isNumeric(num) {
    if (num.match(/^-{0,1}\d+$/)) {
        return true;
    } else if (num.match(/^\d+\.\d+$/)) {
        return true;
    }
    return false;
}


function getParameterByName(name) {
    var regexS = "[\\?&]" + name + "=([^&#]*)",
        regex = new RegExp(regexS),
        results = regex.exec(window.location.search);
    if (results === null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

function DateTimeInfo() {

}


function DateString(date, format) {

    if (!format) format = "MM/DD/YYYY HH:mm:ss (UTC)";

    if (date === null || date === undefined || date === "")
        return moment(new Date(Date.now())).format(format);

    if (date < new Date(2000, 1, 1))
        return "N/A";


    var res = moment(date).format(format);
    return res;
}


function DateStringOnly(date, format) {
    if (!format) format = "YYYY-MM-DD"; //YYYY-MM-DD
    if (date === null || date === undefined || date === "")
        return moment(new Date(Date.now())).format(format);
    if (date < new Date(2000, 1, 1))
        return "N/A";
    var res = moment(date).format(format);
    return res;
}

function ParseDate(date, ignoreutc) {
    if (date === null || date === undefined) {
        return null;
    }
    ignoreutc = true;

    if (date instanceof Date)
        return date;

    date = date.replace("(UTC)", "");

    var minus = date.indexOf("-");
    if (minus > 0 && minus < 4) {
        var parsed = Date.parse(date);
        return new Date(parsed);
    }
    var plus = date.indexOf("+");
    if (plus > 0)
        date = date.substring(0, plus);
    //if (date.indexOf("T") < 0)
    //  return new Date(Date.parse(date));
    var split = date.split("T");

    if (date.indexOf("T") < 0) {
        split = date.split(" ");
    }

    var dateString = split[0].split("-");
    if (dateString[0].indexOf("/") >= 0) {
        dateString = split[0].split("/");
        var strs = new Array();
        strs[0] = dateString[2];
        strs[1] = dateString[0];
        strs[2] = dateString[1];
        dateString = strs;
    }


    if (split.length > 1) {
        var timeString = split[1].split(":");
        if (!timeString[2])
            timeString[2] = 0;
        if (ignoreutc)
            dt = new Date(dateString[0], parseInt(dateString[1]) - 1, dateString[2], timeString[0], timeString[1], timeString[2]);
        else
            dt = new Date(Date.UTC(dateString[0], parseInt(dateString[1]) - 1, dateString[2], timeString[0], timeString[1], timeString[2]));
    }
    else {
        if (ignoreutc)
            dt = new Date(dateString[0], parseInt(dateString[1]) - 1, dateString[2]);
        else
            dt = new Date(Date.UTC(dateString[0], parseInt(dateString[1]) - 1, dateString[2]));
    }
    return dt;
}

var SubmitReturnURL = "";
function AdrackFormBuilderPostXML(_url,xml)
{
    console.log(2131231);
    const loader = document.getElementById('loader');
    const finalPage = document.getElementById('finalPage');
    if (!_url)
        _url = "https://dev-lead-distribution-api.adrack.com/import/";

    $.ajax({
            type: "POST",
            headers: {
                'Access-Control-Allow-Origin': '*',
            },
            data: xml,
            dataType: "json",
            contentType: "application/xml",
            crossDomain: true,
            url: _url
        }
    ).done(function (result, textstatus, request)
    {
        console.log(5555);

        if (SubmitReturnURL!="/")
            window.location.href = SubmitReturnURL;

    }).fail(function (error) {
        console.log(error);
        console.log(error?.response?.data);

        // alert(error.responseText);
    });
    loader.style.display = 'none';
    finalPage.style.display = 'block';
}


function AdrackFormBuilderAPI() {

    this.userName = null;
    this.password = null;

    //this.serverURL = "https://localhost:44331/api/formBuilder/";
//    this.serverURL = "https://qa-lead-distribution.adrack.xyz/api/formBuilder/";
    this.serverURL = "https://dev-lead-distribution-api.adrack.com/api/formBuilder/";

    this.donorId = null;

    this.writeCache = false;
    this.ignoreCacheNextTime = false;
    this.cacheList = new Object();

    this.cacheTimeoutSeconds = 3600 * 5; //5 hours timeout

    this.saveLoginInfo = function () {
        $.cookie('user', this.userName);
        $.cookie('pass', this.password);
    };

    this.readLoginInfo = function () {
        this.userName = "-";
        this.password = "-";
    };

    this.getCommand = function (action) {
        var obj = new Object();
        obj.action = action;
        obj.params = new Object();
        return obj;
    };


    this.clearCache = function () {
        for (var key in this.cacheList) {
            window.localStorage[key] = "null";
        }
        this.cacheList = new Object();
    };



    this.runCommand = function (command, successHandler,
        errorHandler, datatype) {
        var scope = this;

        return this.runCommandCache(command, successHandler, errorHandler, datatype);
    };

    this.runCommandCache = function (command, successHandler,
        errorHandler, datatype) {
        if (!datatype)
            datatype = "json";




        //this.writeCache = false;

        if (this.writeCache && !this.ignoreCacheNextTime) {
            this.ignoreCacheNextTime = false;
            var storageKey = JSON.stringify(command);

            this.cacheList[storageKey] = 1;
            if (window.localStorage[storageKey] !== undefined && window.localStorage[storageKey] !== "null") {
                var decompressed = LZString.decompress(window.localStorage[storageKey].toString());

                var result = JSON.parse(decompressed);
                console.log("DECOMPRESS " + decompressed.length);
                if (Date.now() - result.dateCached < this.cacheTimeoutSeconds * 1000) {
                    successHandler(result);
                    return;
                }
            }
        }

        this.ignoreCacheNextTime = false;

        /*$.ajaxSetup({
                xhrFields: {
                    withCredentials: true
                }
            });*/

        var serverURL = this.serverURL;


        var sendCommand = JSON.stringify(command);
        var cloneCommand = JSON.parse(sendCommand);


        $.ajax({
            type: "POST",
 headers: {
                        'Access-Control-Allow-Origin': '*',
                    },
            data: JSON.stringify(cloneCommand.params),
            dataType: datatype,
            crossDomain: true,
            url: serverURL + command.action
        }
        ).done(function (result, textstatus, request) {
            if (datatype === 'json') {
                result.dateCached = Date.now();
                try {
                    //if (request.responseText.length > 1024 * 1024 * 8)
                    //  window.localStorage[storageKey] = null;
                    //else 
                    {
                        var storageKey = JSON.stringify(command);
                        var resStr = JSON.stringify(result);
                        totalSize += resStr.length;
                        var compressed = LZString.compress(resStr);



                        try {
                            window.localStorage[storageKey] = compressed;
                        }
                        catch (e) {
                            window.localStorage[storageKey] = null;
                        }
                    }
                }
                catch (ex) {

                }
                successHandler(result);

            }
            else
                successHandler(result);
        }).fail(function (error) {
            errorHandler(error.responseText);
        });
    };



    this.runCustomAction = function (actionName, params, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand(actionName);
        if (!params)
            adrackAPI.getWrappedParameters(command);
        else
            command.params = params;
        this.runCommand(command, successHandler, errorHandler);
    };

    this.getForm = function (id, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand("getJSForm/"+id);
        this.runCommand(command, successHandler, errorHandler);
    };

}

var adrackFormBuilderAPI = new AdrackFormBuilderAPI();

//

﻿
//
function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}

function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

function getCookie(cname) {
    //return cookies[cname];
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function getWeekNumber(date) {

    var d = new Date(1990, 11, 31, 0, 0, 0);
    //  d.setTime(d.getTime() + d.getTimezoneOffset() * 60 * 1000);

    var fromMoment = moment(d);

    var toMoment = moment(date);
    var duration = moment.duration(toMoment.diff(fromMoment));
    return Math.trunc(duration.asWeeks()) + 1;

}

function ClearCacheAndReload() {
    window.localStorage.clear();
    window.location.reload();
}

function getWeekFirstDay(date) {

    var d = new Date(1990, 11, 31, 0, 0, 0);
    return dateAdd(d, "week", getWeekNumber(date) - 1);

}

function getDayNumber(date) {
    var fromMoment = moment(29361600000);
    var toMoment = moment(date);
    var duration = moment.duration(toMoment.diff(fromMoment));
    return Math.trunc(duration.asDays()) + 1;
}

function dateAdd(date, interval, units) {
    if (!(date instanceof Date))
        return undefined;
    if (units === 0) return date;
    var ret = new Date(date); //don't change original date
    var checkRollover = function () { if (ret.getDate() !== date.getDate()) ret.setDate(0); };
    switch (String(interval).toLowerCase()) {
        case 'year': ret.setFullYear(ret.getFullYear() + units); checkRollover(); break;
        case 'quarter': ret.setMonth(ret.getMonth() + 3 * units); checkRollover(); break;
        case 'month': ret.setMonth(ret.getMonth() + units); checkRollover(); break;
        case 'week': ret.setDate(ret.getDate() + 7 * units); break;
        case 'day': ret.setDate(ret.getDate() + units); break;
        case 'hour': ret.setTime(ret.getTime() + units * 3600000); break;
        case 'minute': ret.setTime(ret.getTime() + units * 60000); break;
        case 'second': ret.setTime(ret.getTime() + units * 1000); break;
        default: ret = undefined; break;
    }
    return ret;
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function DefaultRanges(minDate, maxDate) {


    var obj = {
        'Today': [moment().toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Yesterday': [moment().subtract(1, 'days').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'days').toDate().setHours(23, 59, 59, 999)],
        'Last 7 Days': [moment().subtract(6, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Last 30 Days': [moment().subtract(29, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'This Month': [moment().startOf('month').toDate().setHours(0, 0, 0, 0), moment().endOf('month').toDate().setHours(23, 59, 59, 999)],
        'Last Month': [moment().subtract(1, 'month').startOf('month').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'month').endOf('month').toDate().setHours(23, 59, 59, 999)]
    };

    if (minDate !== undefined) {
        obj['Whole Range'] = [minDate.setHours(0, 0, 0, 0), maxDate.setHours(23, 59, 59, 999)];
    }
    return obj;
}
