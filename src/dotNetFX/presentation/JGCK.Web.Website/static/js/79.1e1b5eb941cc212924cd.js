webpackJsonp([79],{1035:function(t,e,o){var s=o(1036);"string"==typeof s&&(s=[[t.i,s,""]]),s.locals&&(t.exports=s.locals);o(241)("ceb9ea8c",s,!0)},1036:function(t,e,o){e=t.exports=o(240)(),e.push([t.i,"\n.ximg-demo {\n  width: 100%;\n  height: auto;\n}\n.ximg-error {\n  background-color: yellow;\n}\n.ximg-error:after {\n  content: '\\52A0\\8F7D\\5931\\8D25';\n  color: red;\n}\n",""])},1037:function(t,e,o){"use strict";var s=o(621);e.a={components:{XImg:s.a},methods:{success:function(t,e){console.log("success load",t);var o=e.parentNode.querySelector("span");e.parentNode.removeChild(o)},error:function(t,e,o){console.log("error load",o,t),e.parentNode.querySelector("span").innerText="load error"}},data:function(){return{list:["https://o5omsejde.qnssl.com/demo/test1.jpg","https://o5omsejde.qnssl.com/demo/test2.jpg","https://o5omsejde.qnssl.com/demo/test0.jpg","https://o5omsejde.qnssl.com/demo/test4.jpg","https://o5omsejde.qnssl.com/demo/test5.jpg","https://o5omsejde.qnssl.com/demo/test6.jpg","https://o5omsejde.qnssl.com/demo/test7.jpg","https://o5omsejde.qnssl.com/demo/test8.jpg"]}}}},1038:function(t,e,o){"use strict";var s=function(){var t=this,e=t.$createElement,o=t._self._c||e;return o("div",t._l(t.list,function(e){return o("div",{staticStyle:{"background-color":"yellow","text-align":"center"}},[o("span",{staticStyle:{"font-size":"20px"}},[t._v("Loading")]),t._v(" "),o("x-img",{staticClass:"ximg-demo",attrs:{src:e,"webp-src":e+"?type=webp","error-class":"ximg-error",offset:-100,container:"#vux_view_box_body"},on:{"on-success":t.success,"on-error":t.error}})],1)}))},n=[],r={render:s,staticRenderFns:n};e.a=r},310:function(t,e,o){"use strict";function s(t){o(1035)}Object.defineProperty(e,"__esModule",{value:!0});var n=o(1037),r=o(1038),i=o(0),c=s,a=i(n.a,r.a,!1,c,null,null);e.default=a.exports},621:function(t,e,o){"use strict";function s(t){o(622)}var n=o(624),r=o(627),i=o(0),c=s,a=i(n.a,r.a,!1,c,null,null);e.a=a.exports},622:function(t,e,o){var s=o(623);"string"==typeof s&&(s=[[t.i,s,""]]),s.locals&&(t.exports=s.locals);o(241)("12a7b4e6",s,!0)},623:function(t,e,o){e=t.exports=o(240)(),e.push([t.i,"\n.vux-x-img, .b-lazy {\n  -webkit-transition: opacity 500ms ease-in-out;\n  transition: opacity 500ms ease-in-out;\n  max-width: 100%;\n}\n.b-lazy {\n  opacity: 0;\n}\n.b-lazy.b-loaded {\n  opacity: 1;\n}\n",""])},624:function(t,e,o){"use strict";var s=o(625),n=o.n(s),r=o(626),i=o.n(r),c=o(73);e.a={name:"x-img",mixins:[c.a],created:function(){this.$vux&&this.$vux.bus&&this.$vux.bus.$on("vux:after-view-enter",this.init)},methods:{init:function(){var t=this;this.blazy&&this.blazy.destroy(),this.$el.src=this.defaultSrc,this.$el.className=this.$el.className.replace("b-loaded",""),this.blazy=new n.a({scroller:this.scroller,container:this.container,selector:"#vux-ximg-"+this.uuid,offset:t.offset,errorClass:t.errorClass,successClass:t.successClass,success:function(e){t.$emit("on-success",t.src,e)},error:function(e,o){t.$emit("on-error",t.src,e,o)}})}},mounted:function(){var t=this;this.$el.setAttribute("id","vux-ximg-"+this.uuid),this.$nextTick(function(){setTimeout(function(){t.init()},t.delay)})},computed:{currentSrc:function(){return i()()&&this.webpSrc?this.webpSrc:this.src}},props:{src:String,webpSrc:String,defaultSrc:{type:String,default:"data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw=="},errorClass:String,successClass:String,offset:{type:Number,defaut:100},scroller:Object,container:String,delay:{type:Number,default:0}},watch:{src:function(t){this.init()}},beforeDestroy:function(){this.blazy&&this.blazy.destroy(),this.blazy=null,this.$vux&&this.$vux.bus&&this.$vux.bus.$off("vux:after-view-enter",this.init)}}},625:function(t,e,o){var s,n;!function(r,i){s=i,void 0!==(n="function"==typeof s?s.call(e,o,e,t):s)&&(t.exports=n)}(0,function(){"use strict";function t(t){var o=t._util;if(o.elements=u(t.options.selector),o.count=o.elements.length,o.destroyed&&(o.destroyed=!1,t.options.container&&m(t.options.container,function(t){f(t,"scroll",o.validateT)}),f(window,"resize",o.saveViewportOffsetT),f(window,"resize",o.validateT),f(window,"scroll",o.validateT),t.options.scroller)){var s=t.options.scroller._xscroll,n=s.userConfig.useOriginScroll?"scroll":"scrollend";s.on("afterrender",o.validateT,t),s.on(n,o.validateT,t)}e(t)}function e(t){for(var e=t._util,s=0;s<e.count;s++){var n=e.elements[s];(o(n)||a(n,t.options.successClass))&&(t.load(n),e.elements.splice(s,1),e.count--,s--)}0===e.count&&t.destroy()}function o(t){var e=t.getBoundingClientRect();return e.right>=h.left&&e.bottom>=h.top&&e.left<=h.right&&e.top<=h.bottom}function s(t,e,o){if(!a(t,o.successClass)&&(e||o.loadInvisible||t.offsetWidth>0&&t.offsetHeight>0)){var s=t.getAttribute(g)||t.getAttribute(o.src);if(s){var u=s.split(o.separator),d=u[A&&u.length>1?1:0],v=c(t,"img");if(v||void 0===t.src){var h=new Image,w=function(){o.error&&o.error(t,"invalid"),l(t,o.errorClass),p(h,"error",w),p(h,"load",x)},x=function(){if(v){r(t,d),i(t,y,o.srcset);var e=t.parentNode;e&&c(e,"picture")&&m(e.getElementsByTagName("source"),function(t){i(t,y,o.srcset)}),o.scroller&&o.scroller.reset()}else t.style.backgroundImage='url("'+d+'")';n(t,o),p(h,"load",x),p(h,"error",w)};f(h,"error",w),f(h,"load",x),r(h,d)}else r(t,d),n(t,o)}else c(t,"video")?(m(t.getElementsByTagName("source"),function(t){i(t,b,o.src)}),t.load(),n(t,o)):(o.error&&o.error(t,"missing"),l(t,o.errorClass))}}function n(t,e){l(t,e.successClass),e.success&&e.success(t),t.removeAttribute(e.src),m(e.breakpoints,function(e){t.removeAttribute(e.src)})}function r(t,e){t[b]=e}function i(t,e,o){var s=t.getAttribute(o);s&&(t[e]=s,t.removeAttribute(o))}function c(t,e){return t.nodeName.toLowerCase()===e}function a(t,e){return-1!==(" "+t.className+" ").indexOf(" "+e+" ")}function l(t,e){a(t,e)||(t.className+=" "+e)}function u(t){for(var e=[],o=document.querySelectorAll(t),s=o.length;s--;e.unshift(o[s]));return e}function d(t){h.bottom=(window.innerHeight||document.documentElement.clientHeight)+t,h.right=(window.innerWidth||document.documentElement.clientWidth)+t}function f(t,e,o){t.attachEvent?t.attachEvent&&t.attachEvent("on"+e,o):t.addEventListener(e,o,!1)}function p(t,e,o){t.detachEvent?t.detachEvent&&t.detachEvent("on"+e,o):t.removeEventListener(e,o,!1)}function m(t,e){if(t&&e)for(var o=t.length,s=0;s<o&&!1!==e(t[s],s);s++);}function v(t,e,o){var s=0;return function(){var n=+new Date;n-s<e||(s=n,t.apply(o,arguments))}}var g,h,A,b="src",y="srcset";return function(o){if(!document.querySelectorAll){var n=document.createStyleSheet();document.querySelectorAll=function(t,e,o,s,r){for(r=document.all,e=[],t=t.replace(/\[for\b/gi,"[htmlFor").split(","),o=t.length;o--;){for(n.addRule(t[o],"k:v"),s=r.length;s--;)r[s].currentStyle.k&&e.push(r[s]);n.removeRule(0)}return e}}var r=this,i=r._util={};i.elements=[],i.destroyed=!0,r.options=o||{},r.options.error=r.options.error||!1,r.options.offset=r.options.offset||100,r.options.success=r.options.success||!1,r.options.selector=r.options.selector||".b-lazy",r.options.separator=r.options.separator||"|",r.options.container=!!r.options.container&&document.querySelectorAll(r.options.container),r.options.errorClass=r.options.errorClass||"b-error",r.options.breakpoints=r.options.breakpoints||!1,r.options.loadInvisible=r.options.loadInvisible||!1,r.options.successClass=r.options.successClass||"b-loaded",r.options.validateDelay=r.options.validateDelay||25,r.options.saveViewportOffsetDelay=r.options.saveViewportOffsetDelay||50,r.options.srcset=r.options.srcset||"data-srcset",r.options.src=g=r.options.src||"data-src",A=window.devicePixelRatio>1,h={},h.top=0-r.options.offset,h.left=0-r.options.offset,r.revalidate=function(){t(this)},r.load=function(t,e){var o=this.options;void 0===t.length?s(t,e,o):m(t,function(t){s(t,e,o)})},r.destroy=function(){var t=this,e=t._util;t.options.container&&m(t.options.container,function(t){p(t,"scroll",e.validateT)}),p(window,"scroll",e.validateT),p(window,"resize",e.validateT),p(window,"resize",e.saveViewportOffsetT),t.scroller&&t.scroller._xscroll&&t.scroller._xscroll.off("scroll scrollend afterrender",e.validateT,t.scroller._xscroll),e.count=0,e.elements.length=0,e.destroyed=!0},i.validateT=v(function(){e(r)},r.options.validateDelay,r),i.saveViewportOffsetT=v(function(){d(r.options.offset)},r.options.saveViewportOffsetDelay,r),d(r.options.offset),m(r.options.breakpoints,function(t){if(t.width>=window.screen.width)return g=t.src,!1}),setTimeout(function(){t(r)})}})},626:function(t,e){var o="can_use_webp";!function(){if(window.localStorage&&"object"==typeof localStorage&&(!localStorage.getItem(o)||"available"!==localStorage.getItem(o)&&"disable"!==localStorage.getItem(o))){var t=document.createElement("img");t.onload=function(){try{localStorage.setItem(o,"available")}catch(t){}},t.onerror=function(){try{localStorage.setItem(o,"disable")}catch(t){}},t.src="data:image/webp;base64,UklGRkoAAABXRUJQVlA4WAoAAAAQAAAAAAAAAAAAQUxQSAsAAAABBxAREYiI/gcAAABWUDggGAAAADABAJ0BKgEAAQABABwlpAADcAD+/gbQAA=="}}(),t.exports=function(){return!!window.localStorage&&"available"===window.localStorage.getItem(o)}},627:function(t,e,o){"use strict";var s=function(){var t=this,e=t.$createElement;return(t._self._c||e)("img",{staticClass:"vux-x-img",attrs:{src:t.defaultSrc,"data-src":t.currentSrc}})},n=[],r={render:s,staticRenderFns:n};e.a=r}});