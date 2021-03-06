layui.define(["view", "table", "upload"], function (e) {
    var a = layui.jquery
        , t = layui.laytpl
        , i = layui.element
        , n = layui.table
        , l = layui.upload
        , s = layui.setter
        , r = layui.view
        , o = layui.device()
        , u = a(window)
        , c = a("body")
        , d = a("#" + s.container)
        , y = "layui-show"
        , m = "layui-this"
        , f = "layui-disabled"
        , h = "#LAY_app_body"
        , p = "LAY_app_flexible"
        , v = "layadmin-side-spread-sm"
        , b = "layadmin-tabsbody-item"
        , g = "layui-icon-shrink-right"
        , x = "layui-icon-spread-left"
        , C = "layadmin-side-shrink"
        , k = "LAY-system-side-menu"
        , F = {
            v: "1.7.0 pro",
            req: r.req,
            exit: r.exit,
            escape: function (e) {
                return String(e || "").replace(/&(?!#?[a-zA-Z0-9]+;)/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/'/g, "&#39;").replace(/"/g, "&quot;")
            },
            on: function (e, a) {
                return layui.onevent.call(this, s.MOD_NAME, e, a)
            },
            popup: r.popup,
            popupRight: function (e) {
                return F.popup.index = layer.open(a.extend({
                    type: 1,
                    id: "LAY_adminPopupR",
                    anim: -1,
                    title: !1,
                    closeBtn: !1,
                    offset: "r",
                    shade: .1,
                    shadeClose: !0,
                    skin: "layui-anim layui-anim-rl layui-layer-adminRight",
                    area: "300px"
                }, e))
            },
            sendAuthCode: function (e) {
                e = a.extend({
                    seconds: 60,
                    elemPhone: "#LAY_phone",
                    elemVercode: "#LAY_vercode"
                }, e);
                var t, i = e.seconds, n = function (l) {
                    var s = a(e.elem);
                    i--,
                        i < 0 ? (s.removeClass(f).html("???????????????"),
                            i = e.seconds,
                            clearInterval(t)) : s.addClass(f).html(i + "????????????"),
                        l || (t = setInterval(function () {
                            n(!0)
                        }, 1e3))
                };
                c.off("click", e.elem).on("click", e.elem, function () {
                    e.elemPhone = a(e.elemPhone),
                        e.elemVercode = a(e.elemVercode);
                    var t = e.elemPhone
                        , l = t.val();
                    if (i === e.seconds && !a(this).hasClass(f)) {
                        if (!/^1\d{10}$/.test(l))
                            return t.focus(),
                                layer.msg("???????????????????????????");
                        if ("object" == typeof e.ajax) {
                            var s = e.ajax.success;
                            delete e.ajax.success
                        }
                        F.req(a.extend(!0, {
                            url: "/auth/code",
                            type: "get",
                            data: {
                                phone: l
                            },
                            success: function (a) {
                                layer.msg("???????????????????????????????????????????????????", {
                                    icon: 1,
                                    shade: 0
                                }),
                                    e.elemVercode.focus(),
                                    n(),
                                    s && s(a)
                            }
                        }, e.ajax))
                    }
                })
            },
            screen: function () {
                var e = u.width();
                return e > 1200 ? 3 : e > 992 ? 2 : e > 768 ? 1 : 0
            },
            sideFlexible: function (e) {
                var t = d
                    , i = a("#" + p)
                    , n = F.screen();
                "spread" === e ? (i.removeClass(x).addClass(g),
                    n < 2 ? t.addClass(v) : t.removeClass(v),
                    t.removeClass(C)) : (i.removeClass(g).addClass(x),
                        n < 2 ? t.removeClass(C) : t.addClass(C),
                        t.removeClass(v)),
                    layui.event.call(this, s.MOD_NAME, "side({*})", {
                        status: e
                    })
            },
            resizeTable: function (e) {
                var t = this
                    , i = function () {
                        t.tabsBody(F.tabsPage.index).find(".layui-table-view").each(function () {
                            var e = a(this).attr("lay-id");
                            layui.table.resize(e)
                        })
                    };
                layui.table && (e ? setTimeout(i, e) : i())
            },
            theme: function (e) {
                var i = (s.theme,
                    layui.data(s.tableName))
                    , n = "LAY_layadmin_theme"
                    , l = document.createElement("style")
                    , r = t([".layui-side-menu,", ".layadmin-pagetabs .layui-tab-title li:after,", ".layadmin-pagetabs .layui-tab-title li.layui-this:after,", ".layui-layer-admin .layui-layer-title,", ".layadmin-side-shrink .layui-side-menu .layui-nav>.layui-nav-item>.layui-nav-child", "{background-color:{{d.color.main}} !important;}", ".layui-nav-tree .layui-this,", ".layui-nav-tree .layui-this>a,", ".layui-nav-tree .layui-nav-child dd.layui-this,", ".layui-nav-tree .layui-nav-child dd.layui-this a", "{background-color:{{d.color.selected}} !important;}", ".layui-layout-admin .layui-logo{background-color:{{d.color.logo || d.color.main}} !important;}", "{{# if(d.color.header){ }}", ".layui-layout-admin .layui-header{background-color:{{ d.color.header }};}", ".layui-layout-admin .layui-header a,", ".layui-layout-admin .layui-header a cite{color: #f8f8f8;}", ".layui-layout-admin .layui-header a:hover{color: #fff;}", ".layui-layout-admin .layui-header .layui-nav .layui-nav-more{border-top-color: #fbfbfb;}", ".layui-layout-admin .layui-header .layui-nav .layui-nav-mored{border-color: transparent; border-bottom-color: #fbfbfb;}", ".layui-layout-admin .layui-header .layui-nav .layui-this:after, .layui-layout-admin .layui-header .layui-nav-bar{background-color: #fff; background-color: rgba(255,255,255,.5);}", ".layadmin-pagetabs .layui-tab-title li:after{display: none;}", "{{# } }}"].join("")).render(e = a.extend({}, i.theme, e))
                    , o = document.getElementById(n);
                "styleSheet" in l ? (l.setAttribute("type", "text/css"),
                    l.styleSheet.cssText = r) : l.innerHTML = r,
                    l.id = n,
                    o && c[0].removeChild(o),
                    c[0].appendChild(l),
                    c.attr("layadmin-themealias", e.color.alias),
                    i.theme = i.theme || {},
                    layui.each(e, function (e, a) {
                        i.theme[e] = a
                    }),
                    layui.data(s.tableName, {
                        key: "theme",
                        value: i.theme
                    })
            },
            initTheme: function (e) {
                var a = s.theme;
                e = e || 0,
                    a.color[e] && (a.color[e].index = e,
                        F.theme({
                            color: a.color[e]
                        }))
            },
            tabsPage: {},
            tabsHeader: function (e) {
                return a("#LAY_app_tabsheader").children("li").eq(e || 0)
            },
            tabsBody: function (e) {
                return a(h).find("." + b).eq(e || 0)
            },
            tabsBodyChange: function (e) {
                F.tabsHeader(e).attr("lay-attr", layui.router().href),
                    F.tabsBody(e).addClass(y).siblings().removeClass(y),
                    z.rollPage("auto", e)
            },
            resize: function (e) {
                var a = layui.router()
                    , t = a.path.join("-");
                F.resizeFn[t] && (u.off("resize", F.resizeFn[t]),
                    delete F.resizeFn[t]),
                    "off" !== e && (e(),
                        F.resizeFn[t] = e,
                        u.on("resize", F.resizeFn[t]))
            },
            resizeFn: {},
            runResize: function () {
                var e = layui.router()
                    , a = e.path.join("-");
                F.resizeFn[a] && F.resizeFn[a]()
            },
            delResize: function () {
                this.resize("off")
            },
            closeThisTabs: function () {
                F.tabsPage.index && a(A).eq(F.tabsPage.index).find(".layui-tab-close").trigger("click")
            },
            fullScreen: function () {
                var e = document.documentElement
                    , a = e.requestFullScreen || e.webkitRequestFullScreen || e.mozRequestFullScreen || e.msRequestFullscreen;
                "undefined" != typeof a && a && a.call(e)
            },
            exitScreen: function () {
                document.documentElement;
                document.exitFullscreen ? document.exitFullscreen() : document.mozCancelFullScreen ? document.mozCancelFullScreen() : document.webkitCancelFullScreen ? document.webkitCancelFullScreen() : document.msExitFullscreen && document.msExitFullscreen()
            },
            correctRouter: function (e) {
                return /^\//.test(e) || (e = "/" + e),
                    e.replace(/^(\/+)/, "/").replace(new RegExp("/" + s.entry + "$"), "/")
            }
        }
        , z = F.events = {
            flexible: function (e) {
                var a = e.find("#" + p)
                    , t = a.hasClass(x);
                F.sideFlexible(t ? "spread" : null),
                    F.resizeTable(350)
            },
            refresh: function () {
                layui.index.render()
            },
            serach: function (e) {
                e.off("keypress").on("keypress", function (a) {
                    if (this.value.replace(/\s/g, "") && 13 === a.keyCode) {
                        var t = e.attr("lay-action")
                            , i = e.attr("lay-text") || "??????";
                        t += this.value,
                            i = i + ' <span style="color: #FF5722;">' + F.escape(this.value) + "</span>",
                            location.hash = F.correctRouter(t),
                            z.serach.keys || (z.serach.keys = {}),
                            z.serach.keys[F.tabsPage.index] = this.value,
                            this.value === z.serach.keys[F.tabsPage.index] && z.refresh(e),
                            this.value = ""
                    }
                })
            },
            message: function (e) {
                e.find(".layui-badge-dot").remove()
            },
            theme: function () {
                F.popupRight({
                    id: "LAY_adminPopupTheme",
                    success: function () {
                        r(this.id).render("system/theme")
                    }
                })
            },
            note: function (e) {
                var a = F.screen() < 2
                    , t = layui.data(s.tableName).note;
                z.note.index = F.popup({
                    title: "??????",
                    shade: 0,
                    offset: ["41px", a ? null : e.offset().left - 250 + "px"],
                    anim: -1,
                    id: "LAY_adminNote",
                    skin: "layadmin-note layui-anim layui-anim-upbit",
                    content: '<textarea placeholder="??????"></textarea>',
                    resize: !1,
                    success: function (e, a) {
                        var i = e.find("textarea")
                            , n = void 0 === t ? "?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????" : t;
                        i.val(n).focus().on("keyup", function () {
                            layui.data(s.tableName, {
                                key: "note",
                                value: this.value
                            })
                        })
                    }
                })
            },
            fullscreen: function (e) {
                var a = "layui-icon-screen-full"
                    , t = "layui-icon-screen-restore"
                    , i = e.children("i");
                i.hasClass(a) ? (F.fullScreen(),
                    i.addClass(t).removeClass(a)) : (F.exitScreen(),
                        i.addClass(a).removeClass(t))
            },
            about: function () {
                F.popupRight({
                    id: "LAY_adminPopupAbout",
                    success: function () {
                        r(this.id).render("system/about")
                    }
                })
            },
            more: function () {
                F.popupRight({
                    id: "LAY_adminPopupMore",
                    success: function () {
                        r(this.id).render("system/more")
                    }
                })
            },
            back: function () {
                history.back()
            },
            setTheme: function (e) {
                var a = e.data("index");
                e.siblings(".layui-this").data("index");
                e.hasClass(m) || (e.addClass(m).siblings(".layui-this").removeClass(m),
                    F.initTheme(a))
            },
            rollPage: function (e, t) {
                var i = a("#LAY_app_tabsheader")
                    , n = i.children("li")
                    , l = (i.prop("scrollWidth"),
                        i.outerWidth())
                    , s = parseFloat(i.css("left"));
                if ("left" === e) {
                    if (!s && s <= 0)
                        return;
                    var r = -s - l;
                    n.each(function (e, t) {
                        var n = a(t)
                            , l = n.position().left;
                        if (l >= r)
                            return i.css("left", -l),
                                !1
                    })
                } else
                    "auto" === e ? !function () {
                        var e, r = n.eq(t);
                        if (r[0]) {
                            if (e = r.position().left,
                                e < -s)
                                return i.css("left", -e);
                            if (e + r.outerWidth() >= l - s) {
                                var o = e + r.outerWidth() - (l - s);
                                n.each(function (e, t) {
                                    var n = a(t)
                                        , l = n.position().left;
                                    if (l + s > 0 && l - s > o)
                                        return i.css("left", -l),
                                            !1
                                })
                            }
                        }
                    }() : n.each(function (e, t) {
                        var n = a(t)
                            , r = n.position().left;
                        if (r + n.outerWidth() >= l - s)
                            return i.css("left", -r),
                                !1
                    })
            },
            leftPage: function () {
                z.rollPage("left")
            },
            rightPage: function () {
                z.rollPage()
            },
            closeThisTabs: function () {
                F.closeThisTabs()
            },
            closeOtherTabs: function (e) {
                var t = "LAY-system-pagetabs-remove";
                "all" === e ? (a(A + ":gt(0)").remove(),
                    a(h).find("." + b + ":gt(0)").remove()) : (a(A).each(function (e, i) {
                        e && e != F.tabsPage.index && (a(i).addClass(t),
                            F.tabsBody(e).addClass(t))
                    }),
                        a("." + t).remove())
            },
            closeAllTabs: function () {
                z.closeOtherTabs("all"),
                    location.hash = ""
            },
            shade: function () {
                F.sideFlexible()
            }
        };
    !function () {
        var e = layui.data(s.tableName);
        e.theme ? F.theme(e.theme) : s.theme && F.initTheme(s.theme.initColorIndex),
            c.addClass("layui-layout-body"),
            F.screen() < 1 && delete s.pageTabs,
            s.pageTabs || d.addClass("layadmin-tabspage-none"),
            o.ie && o.ie < 10 && r.error("IE" + o.ie + "???????????????????????????????????????Chrome / Firefox / Edge ??????????????????", {
                offset: "auto",
                id: "LAY_errorIE"
            })
    }(),
        F.on("hash(side)", function (e) {
            var t = e.path
                , i = function (e) {
                    return {
                        list: e.children(".layui-nav-child"),
                        name: e.data("name"),
                        jump: e.data("jump")
                    }
                }
                , n = a("#" + k)
                , l = "layui-nav-itemed"
                , s = function (e) {
                    var n = F.correctRouter(t.join("/"));
                    e.each(function (e, s) {
                        var r = a(s)
                            , o = i(r)
                            , u = o.list.children("dd")
                            , c = t[0] == o.name || 0 === e && !t[0] || o.jump && n == F.correctRouter(o.jump);
                        if (u.each(function (e, s) {
                            var r = a(s)
                                , u = i(r)
                                , c = u.list.children("dd")
                                , d = t[0] == o.name && t[1] == u.name || u.jump && n == F.correctRouter(u.jump);
                            if (c.each(function (e, s) {
                                var r = a(s)
                                    , c = i(r)
                                    , d = t[0] == o.name && t[1] == u.name && t[2] == c.name || c.jump && n == F.correctRouter(c.jump);
                                if (d) {
                                    var y = c.list[0] ? l : m;
                                    return r.addClass(y).siblings().removeClass(y),
                                        !1
                                }
                            }),
                                d) {
                                var y = u.list[0] ? l : m;
                                return r.addClass(y).siblings().removeClass(y),
                                    !1
                            }
                        }),
                            c) {
                            var d = o.list[0] ? l : m;
                            return r.addClass(d).siblings().removeClass(d),
                                !1
                        }
                    })
                };
            n.find("." + m).removeClass(m),
                F.screen() < 2 && F.sideFlexible(),
                s(n.children("li"))
        }),
        i.on("nav(layadmin-system-side-menu)", function (e) {
            e.siblings(".layui-nav-child")[0] && d.hasClass(C) && (F.sideFlexible("spread"),
                layer.close(e.data("index"))),
                F.tabsPage.type = "nav"
        }),
        i.on("nav(layadmin-pagetabs-nav)", function (e) {
            var a = e.parent();
            a.removeClass(m),
                a.parent().removeClass(y)
        });
    var P = function (e) {
        var a = e.attr("lay-id")
            , t = e.attr("lay-attr")
            , i = e.index();
        location.hash = a === s.entry ? "/" : t || "/",
            F.tabsBodyChange(i)
    }
        , A = "#LAY_app_tabsheader>li";
    c.on("click", A, function () {
        var e = a(this)
            , t = e.index();
        return F.tabsPage.type = "tab",
            F.tabsPage.index = t,
            "iframe" === e.attr("lay-attr") ? F.tabsBodyChange(t) : (P(e),
                F.runResize(),
                void F.resizeTable())
    }),
        i.on("tabDelete(layadmin-layout-tabs)", function (e) {
            var t = a(A + ".layui-this");
            e.index && F.tabsBody(e.index).remove(),
                P(t),
                F.delResize()
        }),
        c.on("click", "*[lay-href]", function () {
            var e = a(this)
                , t = e.attr("lay-href")
                , i = layui.router();
            F.tabsPage.elem = e,
                location.hash = F.correctRouter(t),
                F.correctRouter(t) === i.href && F.events.refresh()
        }),
        c.on("click", "*[layadmin-event]", function () {
            var e = a(this)
                , t = e.attr("layadmin-event");
            z[t] && z[t].call(this, e)
        }),
        c.on("mouseenter", "*[lay-tips]", function () {
            var e = a(this);
            if (!e.parent().hasClass("layui-nav-item") || d.hasClass(C)) {
                var t = e.attr("lay-tips")
                    , i = e.attr("lay-offset")
                    , n = e.attr("lay-direction")
                    , l = layer.tips(t, this, {
                        tips: n || 1,
                        time: -1,
                        success: function (e, a) {
                            i && e.css("margin-left", i + "px")
                        }
                    });
                e.data("index", l)
            }
        }).on("mouseleave", "*[lay-tips]", function () {
            layer.close(a(this).data("index"))
        });
    var T = layui.data.resizeSystem = function () {
        layer.closeAll("tips"),
            T.lock || setTimeout(function () {
                F.sideFlexible(F.screen() < 2 ? "" : "spread"),
                    delete T.lock
            }, 100),
            T.lock = !0
    }
        ;
    u.on("resize", layui.data.resizeSystem),
        !function () {
            var e = s.request;
            if (e.tokenName) {
                var a = {};
                a[e.tokenName] = layui.data(s.tableName)[e.tokenName] || "",
                    n.set({
                        headers: a,
                        where: a
                    }),
                    l.set({
                        headers: a,
                        data: a
                    })
            }
        }(),
        e("admin", F)
});
