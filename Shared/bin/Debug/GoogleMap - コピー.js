<html>
    <head>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=quarterly&sensor=true&language=ja&key=AIzaSyBRWlXbVhdiftGJz9cwQlD43PfXRxv1Mqw"></script>
    <script type="text/javascript" >
/**
 * @name InfoBox
 * @version 1.1.13 [March 19, 2014]
 * @author Gary Little (inspired by proof-of-concept code from Pamela Fox of Google)
 * @copyright Copyright 2010 Gary Little [gary at luxcentral.com]
 * @fileoverview InfoBox extends the Google Maps JavaScript API V3 <tt>OverlayView</tt> class.
 *  <p>
 *  An InfoBox behaves like a <tt>google.maps.InfoWindow</tt>, but it supports several
 *  additional properties for advanced styling. An InfoBox can also be used as a map label.
 *  <p>
 *  An InfoBox also fires the same events as a <tt>google.maps.InfoWindow</tt>.
 */

/*!
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*jslint browser:true */
/*global google */

/**
 * @name InfoBoxOptions
 * @class This class represents the optional parameter passed to the {@link InfoBox} constructor.
 * @property {string|Node} content The content of the InfoBox (plain text or an HTML DOM node).
 * @property {boolean} [disableAutoPan=false] Disable auto-pan on <tt>open</tt>.
 * @property {number} maxWidth The maximum width (in pixels) of the InfoBox. Set to 0 if no maximum.
 * @property {Size} pixelOffset The offset (in pixels) from the top left corner of the InfoBox
 *  (or the bottom left corner if the <code>alignBottom</code> property is <code>true</code>)
 *  to the map pixel corresponding to <tt>position</tt>.
 * @property {LatLng} position The geographic location at which to display the InfoBox.
 * @property {number} zIndex The CSS z-index style value for the InfoBox.
 *  Note: This value overrides a zIndex setting specified in the <tt>boxStyle</tt> property.
 * @property {string} [boxClass="infoBox"] The name of the CSS class defining the styles for the InfoBox container.
 * @property {Object} [boxStyle] An object literal whose properties define specific CSS
 *  style values to be applied to the InfoBox. Style values defined here override those that may
 *  be defined in the <code>boxClass</code> style sheet. If this property is changed after the
 *  InfoBox has been created, all previously set styles (except those defined in the style sheet)
 *  are removed from the InfoBox before the new style values are applied.
 * @property {string} closeBoxMargin The CSS margin style value for the close box.
 *  The default is "2px" (a 2-pixel margin on all sides).
 * @property {string} closeBoxURL The URL of the image representing the close box.
 *  Note: The default is the URL for Google's standard close box.
 *  Set this property to "" if no close box is required.
 * @property {Size} infoBoxClearance Minimum offset (in pixels) from the InfoBox to the
 *  map edge after an auto-pan.
 * @property {boolean} [isHidden=false] Hide the InfoBox on <tt>open</tt>.
 *  [Deprecated in favor of the <tt>visible</tt> property.]
 * @property {boolean} [visible=true] Show the InfoBox on <tt>open</tt>.
 * @property {boolean} alignBottom Align the bottom left corner of the InfoBox to the <code>position</code>
 *  location (default is <tt>false</tt> which means that the top left corner of the InfoBox is aligned).
 * @property {string} pane The pane where the InfoBox is to appear (default is "floatPane").
 *  Set the pane to "mapPane" if the InfoBox is being used as a map label.
 *  Valid pane names are the property names for the <tt>google.maps.MapPanes</tt> object.
 * @property {boolean} enableEventPropagation Propagate mousedown, mousemove, mouseover, mouseout,
 *  mouseup, click, dblclick, touchstart, touchend, touchmove, and contextmenu events in the InfoBox
 *  (default is <tt>false</tt> to mimic the behavior of a <tt>google.maps.InfoWindow</tt>). Set
 *  this property to <tt>true</tt> if the InfoBox is being used as a map label.
 */

/**
 * Creates an InfoBox with the options specified in {@link InfoBoxOptions}.
 *  Call <tt>InfoBox.open</tt> to add the box to the map.
 * @constructor
 * @param {InfoBoxOptions} [opt_opts]
 */
function InfoBox(opt_opts) {

    opt_opts = opt_opts || {};

    google.maps.OverlayView.apply(this, arguments);

    // Standard options (in common with google.maps.InfoWindow):
    //
    this.content_ = opt_opts.content || "";
    this.disableAutoPan_ = opt_opts.disableAutoPan || false;
    this.maxWidth_ = opt_opts.maxWidth || 0;
    this.pixelOffset_ = opt_opts.pixelOffset || new google.maps.Size(0, 0);
    this.position_ = opt_opts.position || new google.maps.LatLng(0, 0);
    this.zIndex_ = opt_opts.zIndex || null;

    // Additional options (unique to InfoBox):
    //
    this.boxClass_ = opt_opts.boxClass || "infoBox";
    this.boxStyle_ = opt_opts.boxStyle || {};
    this.closeBoxMargin_ = opt_opts.closeBoxMargin || "2px";
    this.closeBoxURL_ = opt_opts.closeBoxURL || "http://www.google.com/intl/en_us/mapfiles/close.gif";
    if (opt_opts.closeBoxURL === "") {
        this.closeBoxURL_ = "";
    }
    this.infoBoxClearance_ = opt_opts.infoBoxClearance || new google.maps.Size(1, 1);

    if (typeof opt_opts.visible === "undefined") {
        if (typeof opt_opts.isHidden === "undefined") {
            opt_opts.visible = true;
        } else {
            opt_opts.visible = !opt_opts.isHidden;
        }
    }
    this.isHidden_ = !opt_opts.visible;

    this.alignBottom_ = opt_opts.alignBottom || false;
    this.pane_ = opt_opts.pane || "floatPane";
    this.enableEventPropagation_ = opt_opts.enableEventPropagation || false;

    this.div_ = null;
    this.closeListener_ = null;
    this.moveListener_ = null;
    this.mapListener_ = null;
    this.contextListener_ = null;
    this.eventListeners_ = null;
    this.fixedWidthSet_ = null;
}

    /* InfoBox extends OverlayView in the Google Maps API v3.
     */
    InfoBox.prototype = new google.maps.OverlayView();

    /**
     * Creates the DIV representing the InfoBox.
     * @private
     */
    InfoBox.prototype.createInfoBoxDiv_ = function () {

        var i;
        var events;
        var bw;
        var me = this;

        // This handler prevents an event in the InfoBox from being passed on to the map.
        //
        var cancelHandler = function (e) {
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        };

        // This handler ignores the current event in the InfoBox and conditionally prevents
        // the event from being passed on to the map. It is used for the contextmenu event.
        //
        var ignoreHandler = function (e) {

            e.returnValue = false;

            if (e.preventDefault) {

                e.preventDefault();
            }

            if (!me.enableEventPropagation_) {

                cancelHandler(e);
            }
        };

        if (!this.div_) {

            this.div_ = document.createElement("div");

            this.setBoxStyle_();

            if (typeof this.content_.nodeType === "undefined") {
                this.div_.innerHTML = this.getCloseBoxImg_() + this.content_;
            } else {
                this.div_.innerHTML = this.getCloseBoxImg_();
                this.div_.appendChild(this.content_);
            }

            // Add the InfoBox DIV to the DOM
            this.getPanes()[this.pane_].appendChild(this.div_);

            this.addClickHandler_();

            if (this.div_.style.width) {

                this.fixedWidthSet_ = true;

            } else {

                if (this.maxWidth_ !== 0 && this.div_.offsetWidth > this.maxWidth_) {

                    this.div_.style.width = this.maxWidth_;
                    this.div_.style.overflow = "auto";
                    this.fixedWidthSet_ = true;

                } else { // The following code is needed to overcome problems with MSIE

                    bw = this.getBoxWidths_();

                    this.div_.style.width = (this.div_.offsetWidth - bw.left - bw.right) + "px";
                    this.fixedWidthSet_ = false;
                }
            }

            this.panBox_(this.disableAutoPan_);

            if (!this.enableEventPropagation_) {

                this.eventListeners_ = [];

                // Cancel event propagation.
                //
                // Note: mousemove not included (to resolve Issue 152)
                events = ["mousedown", "mouseover", "mouseout", "mouseup",
                    "click", "dblclick", "touchstart", "touchend", "touchmove"];

                for (i = 0; i < events.length; i++) {

                    this.eventListeners_.push(google.maps.event.addDomListener(this.div_, events[i], cancelHandler));
                }

                // Workaround for Google bug that causes the cursor to change to a pointer
                // when the mouse moves over a marker underneath InfoBox.
                this.eventListeners_.push(google.maps.event.addDomListener(this.div_, "mouseover", function (e) {
                    this.style.cursor = "default";
                }));
            }

            this.contextListener_ = google.maps.event.addDomListener(this.div_, "contextmenu", ignoreHandler);

            /**
             * This event is fired when the DIV containing the InfoBox's content is attached to the DOM.
             * @name InfoBox#domready
             * @event
             */
            google.maps.event.trigger(this, "domready");
        }
    };

    /**
     * Returns the HTML <IMG> tag for the close box.
     * @private
     */
    InfoBox.prototype.getCloseBoxImg_ = function () {

        var img = "";

        if (this.closeBoxURL_ !== "") {

            img = "<img";
            img += " src='" + this.closeBoxURL_ + "'";
            img += " align=right"; // Do this because Opera chokes on style='float: right;'
            img += " style='";
            img += " position: relative;"; // Required by MSIE
            img += " cursor: pointer;";
            img += " margin: " + this.closeBoxMargin_ + ";";
            img += "'>";
        }

        return img;
    };

    /**
     * Adds the click handler to the InfoBox close box.
     * @private
     */
    InfoBox.prototype.addClickHandler_ = function () {

        var closeBox;

        if (this.closeBoxURL_ !== "") {

            closeBox = this.div_.firstChild;
            this.closeListener_ = google.maps.event.addDomListener(closeBox, "click", this.getCloseClickHandler_());

        } else {

            this.closeListener_ = null;
        }
    };

    /**
     * Returns the function to call when the user clicks the close box of an InfoBox.
     * @private
     */
    InfoBox.prototype.getCloseClickHandler_ = function () {

        var me = this;

        return function (e) {

            // 1.0.3 fix: Always prevent propagation of a close box click to the map:
            e.cancelBubble = true;

            if (e.stopPropagation) {

                e.stopPropagation();
            }

            /**
             * This event is fired when the InfoBox's close box is clicked.
             * @name InfoBox#closeclick
             * @event
             */
            google.maps.event.trigger(me, "closeclick");

            me.close();
        };
    };

    /**
     * Pans the map so that the InfoBox appears entirely within the map's visible area.
     * @private
     */
    InfoBox.prototype.panBox_ = function (disablePan) {

        var map;
        var bounds;
        var xOffset = 0, yOffset = 0;

        if (!disablePan) {

            map = this.getMap();

            if (map instanceof google.maps.Map) { // Only pan if attached to map, not panorama

                if (!map.getBounds().contains(this.position_)) {
                    // Marker not in visible area of map, so set center
                    // of map to the marker position first.
                    map.setCenter(this.position_);
                }

                bounds = map.getBounds();

                var mapDiv = map.getDiv();
                var mapWidth = mapDiv.offsetWidth;
                var mapHeight = mapDiv.offsetHeight;
                var iwOffsetX = this.pixelOffset_.width;
                var iwOffsetY = this.pixelOffset_.height;
                var iwWidth = this.div_.offsetWidth;
                var iwHeight = this.div_.offsetHeight;
                var padX = this.infoBoxClearance_.width;
                var padY = this.infoBoxClearance_.height;
                var pixPosition = this.getProjection().fromLatLngToContainerPixel(this.position_);

                if (pixPosition.x < (-iwOffsetX + padX)) {
                    xOffset = pixPosition.x + iwOffsetX - padX;
                } else if ((pixPosition.x + iwWidth + iwOffsetX + padX) > mapWidth) {
                    xOffset = pixPosition.x + iwWidth + iwOffsetX + padX - mapWidth;
                }
                if (this.alignBottom_) {
                    if (pixPosition.y < (-iwOffsetY + padY + iwHeight)) {
                        yOffset = pixPosition.y + iwOffsetY - padY - iwHeight;
                    } else if ((pixPosition.y + iwOffsetY + padY) > mapHeight) {
                        yOffset = pixPosition.y + iwOffsetY + padY - mapHeight;
                    }
                } else {
                    if (pixPosition.y < (-iwOffsetY + padY)) {
                        yOffset = pixPosition.y + iwOffsetY - padY;
                    } else if ((pixPosition.y + iwHeight + iwOffsetY + padY) > mapHeight) {
                        yOffset = pixPosition.y + iwHeight + iwOffsetY + padY - mapHeight;
                    }
                }

                if (!(xOffset === 0 && yOffset === 0)) {

                    // Move the map to the shifted center.
                    //
                    var c = map.getCenter();
                    map.panBy(xOffset, yOffset);
                }
            }
        }
    };

    /**
     * Sets the style of the InfoBox by setting the style sheet and applying
     * other specific styles requested.
     * @private
     */
    InfoBox.prototype.setBoxStyle_ = function () {

        var i, boxStyle;

        if (this.div_) {

            // Apply style values from the style sheet defined in the boxClass parameter:
            this.div_.className = this.boxClass_;

            // Clear existing inline style values:
            this.div_.style.cssText = "";

            // Apply style values defined in the boxStyle parameter:
            boxStyle = this.boxStyle_;
            for (i in boxStyle) {

                if (boxStyle.hasOwnProperty(i)) {

                    this.div_.style[i] = boxStyle[i];
                }
            }

            // Fix for iOS disappearing InfoBox problem.
            // See http://stackoverflow.com/questions/9229535/google-maps-markers-disappear-at-certain-zoom-level-only-on-iphone-ipad
            this.div_.style.WebkitTransform = "translateZ(0)";

            // Fix up opacity style for benefit of MSIE:
            //
            if (typeof this.div_.style.opacity !== "undefined" && this.div_.style.opacity !== "") {
                // See http://www.quirksmode.org/css/opacity.html
                this.div_.style.MsFilter = "\"progid:DXImageTransform.Microsoft.Alpha(Opacity=" + (this.div_.style.opacity * 100) + ")\"";
                this.div_.style.filter = "alpha(opacity=" + (this.div_.style.opacity * 100) + ")";
            }

            // Apply required styles:
            //
            this.div_.style.position = "absolute";
            this.div_.style.visibility = 'hidden';
            if (this.zIndex_ !== null) {

                this.div_.style.zIndex = this.zIndex_;
            }
        }
    };

    /**
     * Get the widths of the borders of the InfoBox.
     * @private
     * @return {Object} widths object (top, bottom left, right)
     */
    InfoBox.prototype.getBoxWidths_ = function () {

        var computedStyle;
        var bw = { top: 0, bottom: 0, left: 0, right: 0 };
        var box = this.div_;

        if (document.defaultView && document.defaultView.getComputedStyle) {

            computedStyle = box.ownerDocument.defaultView.getComputedStyle(box, "");

            if (computedStyle) {

                // The computed styles are always in pixel units (good!)
                bw.top = parseInt(computedStyle.borderTopWidth, 10) || 0;
                bw.bottom = parseInt(computedStyle.borderBottomWidth, 10) || 0;
                bw.left = parseInt(computedStyle.borderLeftWidth, 10) || 0;
                bw.right = parseInt(computedStyle.borderRightWidth, 10) || 0;
            }

        } else if (document.documentElement.currentStyle) { // MSIE

            if (box.currentStyle) {

                // The current styles may not be in pixel units, but assume they are (bad!)
                bw.top = parseInt(box.currentStyle.borderTopWidth, 10) || 0;
                bw.bottom = parseInt(box.currentStyle.borderBottomWidth, 10) || 0;
                bw.left = parseInt(box.currentStyle.borderLeftWidth, 10) || 0;
                bw.right = parseInt(box.currentStyle.borderRightWidth, 10) || 0;
            }
        }

        return bw;
    };

    /**
     * Invoked when <tt>close</tt> is called. Do not call it directly.
     */
    InfoBox.prototype.onRemove = function () {

        if (this.div_) {

            this.div_.parentNode.removeChild(this.div_);
            this.div_ = null;
        }
    };

    /**
     * Draws the InfoBox based on the current map projection and zoom level.
     */
    InfoBox.prototype.draw = function () {

        this.createInfoBoxDiv_();

        var pixPosition = this.getProjection().fromLatLngToDivPixel(this.position_);

        this.div_.style.left = (pixPosition.x + this.pixelOffset_.width) + "px";

        if (this.alignBottom_) {
            this.div_.style.bottom = -(pixPosition.y + this.pixelOffset_.height) + "px";
        } else {
            this.div_.style.top = (pixPosition.y + this.pixelOffset_.height) + "px";
        }

        if (this.isHidden_) {

            this.div_.style.visibility = "hidden";

        } else {

            this.div_.style.visibility = "visible";
        }
    };

    /**
     * Sets the options for the InfoBox. Note that changes to the <tt>maxWidth</tt>,
     *  <tt>closeBoxMargin</tt>, <tt>closeBoxURL</tt>, and <tt>enableEventPropagation</tt>
     *  properties have no affect until the current InfoBox is <tt>close</tt>d and a new one
     *  is <tt>open</tt>ed.
     * @param {InfoBoxOptions} opt_opts
     */
    InfoBox.prototype.setOptions = function (opt_opts) {
        if (typeof opt_opts.boxClass !== "undefined") { // Must be first

            this.boxClass_ = opt_opts.boxClass;
            this.setBoxStyle_();
        }
        if (typeof opt_opts.boxStyle !== "undefined") { // Must be second

            this.boxStyle_ = opt_opts.boxStyle;
            this.setBoxStyle_();
        }
        if (typeof opt_opts.content !== "undefined") {

            this.setContent(opt_opts.content);
        }
        if (typeof opt_opts.disableAutoPan !== "undefined") {

            this.disableAutoPan_ = opt_opts.disableAutoPan;
        }
        if (typeof opt_opts.maxWidth !== "undefined") {

            this.maxWidth_ = opt_opts.maxWidth;
        }
        if (typeof opt_opts.pixelOffset !== "undefined") {

            this.pixelOffset_ = opt_opts.pixelOffset;
        }
        if (typeof opt_opts.alignBottom !== "undefined") {

            this.alignBottom_ = opt_opts.alignBottom;
        }
        if (typeof opt_opts.position !== "undefined") {

            this.setPosition(opt_opts.position);
        }
        if (typeof opt_opts.zIndex !== "undefined") {

            this.setZIndex(opt_opts.zIndex);
        }
        if (typeof opt_opts.closeBoxMargin !== "undefined") {

            this.closeBoxMargin_ = opt_opts.closeBoxMargin;
        }
        if (typeof opt_opts.closeBoxURL !== "undefined") {

            this.closeBoxURL_ = opt_opts.closeBoxURL;
        }
        if (typeof opt_opts.infoBoxClearance !== "undefined") {

            this.infoBoxClearance_ = opt_opts.infoBoxClearance;
        }
        if (typeof opt_opts.isHidden !== "undefined") {

            this.isHidden_ = opt_opts.isHidden;
        }
        if (typeof opt_opts.visible !== "undefined") {

            this.isHidden_ = !opt_opts.visible;
        }
        if (typeof opt_opts.enableEventPropagation !== "undefined") {

            this.enableEventPropagation_ = opt_opts.enableEventPropagation;
        }

        if (this.div_) {

            this.draw();
        }
    };

    /**
     * Sets the content of the InfoBox.
     *  The content can be plain text or an HTML DOM node.
     * @param {string|Node} content
     */
    InfoBox.prototype.setContent = function (content) {
        this.content_ = content;

        if (this.div_) {

            if (this.closeListener_) {

                google.maps.event.removeListener(this.closeListener_);
                this.closeListener_ = null;
            }

            // Odd code required to make things work with MSIE.
            //
            if (!this.fixedWidthSet_) {

                this.div_.style.width = "";
            }

            if (typeof content.nodeType === "undefined") {
                this.div_.innerHTML = this.getCloseBoxImg_() + content;
            } else {
                this.div_.innerHTML = this.getCloseBoxImg_();
                this.div_.appendChild(content);
            }

            // Perverse code required to make things work with MSIE.
            // (Ensures the close box does, in fact, float to the right.)
            //
            if (!this.fixedWidthSet_) {
                this.div_.style.width = this.div_.offsetWidth + "px";
                if (typeof content.nodeType === "undefined") {
                    this.div_.innerHTML = this.getCloseBoxImg_() + content;
                } else {
                    this.div_.innerHTML = this.getCloseBoxImg_();
                    this.div_.appendChild(content);
                }
            }

            this.addClickHandler_();
        }

        /**
         * This event is fired when the content of the InfoBox changes.
         * @name InfoBox#content_changed
         * @event
         */
        google.maps.event.trigger(this, "content_changed");
    };

    /**
     * Sets the geographic location of the InfoBox.
     * @param {LatLng} latlng
     */
    InfoBox.prototype.setPosition = function (latlng) {

        this.position_ = latlng;

        if (this.div_) {

            this.draw();
        }

        /**
         * This event is fired when the position of the InfoBox changes.
         * @name InfoBox#position_changed
         * @event
         */
        google.maps.event.trigger(this, "position_changed");
    };

    /**
     * Sets the zIndex style for the InfoBox.
     * @param {number} index
     */
    InfoBox.prototype.setZIndex = function (index) {

        this.zIndex_ = index;

        if (this.div_) {

            this.div_.style.zIndex = index;
        }

        /**
         * This event is fired when the zIndex of the InfoBox changes.
         * @name InfoBox#zindex_changed
         * @event
         */
        google.maps.event.trigger(this, "zindex_changed");
    };

    /**
     * Sets the visibility of the InfoBox.
     * @param {boolean} isVisible
     */
    InfoBox.prototype.setVisible = function (isVisible) {

        this.isHidden_ = !isVisible;
        if (this.div_) {
            this.div_.style.visibility = (this.isHidden_ ? "hidden" : "visible");
        }
    };

    /**
     * Returns the content of the InfoBox.
     * @returns {string}
     */
    InfoBox.prototype.getContent = function () {

        return this.content_;
    };

    /**
     * Returns the geographic location of the InfoBox.
     * @returns {LatLng}
     */
    InfoBox.prototype.getPosition = function () {

        return this.position_;
    };

    /**
     * Returns the zIndex for the InfoBox.
     * @returns {number}
     */
    InfoBox.prototype.getZIndex = function () {

        return this.zIndex_;
    };

    /**
     * Returns a flag indicating whether the InfoBox is visible.
     * @returns {boolean}
     */
    InfoBox.prototype.getVisible = function () {

        var isVisible;

        if ((typeof this.getMap() === "undefined") || (this.getMap() === null)) {
            isVisible = false;
        } else {
            isVisible = !this.isHidden_;
        }
        return isVisible;
    };

    /**
     * Shows the InfoBox. [Deprecated; use <tt>setVisible</tt> instead.]
     */
    InfoBox.prototype.show = function () {

        this.isHidden_ = false;
        if (this.div_) {
            this.div_.style.visibility = "visible";
        }
    };

    /**
     * Hides the InfoBox. [Deprecated; use <tt>setVisible</tt> instead.]
     */
    InfoBox.prototype.hide = function () {

        this.isHidden_ = true;
        if (this.div_) {
            this.div_.style.visibility = "hidden";
        }
    };

    /**
     * Adds the InfoBox to the specified map or Street View panorama. If <tt>anchor</tt>
     *  (usually a <tt>google.maps.Marker</tt>) is specified, the position
     *  of the InfoBox is set to the position of the <tt>anchor</tt>. If the
     *  anchor is dragged to a new location, the InfoBox moves as well.
     * @param {Map|StreetViewPanorama} map
     * @param {MVCObject} [anchor]
     */
    InfoBox.prototype.open = function (map, anchor) {

        var me = this;

        if (anchor) {
            console.log(anchor);
            this.position_ = anchor.getPosition();
            this.moveListener_ = google.maps.event.addListener(anchor, "position_changed", function () {
                me.setPosition(this.getPosition());
            });

            this.mapListener_ = google.maps.event.addListener(anchor, "map_changed", function () {
                me.setMap(this.map);
            });
        }

        this.setMap(map);

        if (this.div_) {

            this.panBox_();
        }
    };

    /**
     * Removes the InfoBox from the map.
     */
    InfoBox.prototype.close = function () {

        var i;

        if (this.closeListener_) {

            google.maps.event.removeListener(this.closeListener_);
            this.closeListener_ = null;
        }

        if (this.eventListeners_) {

            for (i = 0; i < this.eventListeners_.length; i++) {

                google.maps.event.removeListener(this.eventListeners_[i]);
            }
            this.eventListeners_ = null;
        }

        if (this.moveListener_) {

            google.maps.event.removeListener(this.moveListener_);
            this.moveListener_ = null;
        }

        if (this.mapListener_) {

            google.maps.event.removeListener(this.mapListener_);
            this.mapListener_ = null;
        }

        if (this.contextListener_) {

            google.maps.event.removeListener(this.contextListener_);
            this.contextListener_ = null;
        }

        this.setMap(null);
    };
//****************************************************************************************************************************************************//
//****************************************************************************************************************************************************//
//****************************************************************************************************************************************************//
    </script>
    <script type="text/javascript">
        var map;
        var polylinePoints_editwork;
        var polygonPoints_editwork;
        var polyline_editwork;
        var polygon_list;
        var polyline_list;
        var maker_list;
        var circle_list;
        var icon_marker_list;
        var infowindow_label_list;
        //
        //
        //初期化関数
        function init() {
         
        //
            //*****************************
            //配列の作成
            //*****************************
            polylinePoints_editwork = new google.maps.MVCArray();
            polygonPoints_editwork  = new google.maps.MVCArray();
            polygon_list       = new google.maps.MVCArray();
            polyline_list      = new google.maps.MVCArray();
            marker_list       = new google.maps.MVCArray();
            circle_list       = new google.maps.MVCArray();
            icon_marker_list  = new google.maps.MVCArray();
            infowindow_label_list = new google.maps.MVCArray();
            //var latlng  　    = ;
            //*****************************
            //地図オプション
            //*****************************
            var myOptions = {
                zoom: 15 ,
                center: new google.maps.LatLng(35.168896, 136.906906) ,
                mapTypeId: google.maps.MapTypeId.ROADMAP ,
                overviewMapControl:true ,
                scaleControl: true , 
                disableDoubleClickZoom: true
            };
            //*****************************
            //地図の作成
            //*****************************
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            //*****************************
            //新規表示用ポリライン
            //*****************************
            polyline_editwork = new google.maps.Polyline({
                path: polylinePoints_editwork,
                fillColor:  '#00ff00',   // 塗りつぶし色
                fillOpacity: 0.25,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
                map: map,             // 表示させる地図（google.maps.Map）
                strokeColor:  '#00ff00', // 外周色
                strokeOpacity: 1,       // 外周透過度（0: 透明 ⇔ 1:不透明）
                strokeWeight: 5 ,        // 外周太さ（ピクセル）
                clickable : false 
            });
            //*****************************
            //新規表示用ポリゴン
            //*****************************
            polygon_editwork = new google.maps.Polygon({
                path: polygonPoints_editwork,
                fillColor:  '#00ff00',   // 塗りつぶし色
                fillOpacity: 0.25,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
                map: map,             // 表示させる地図（google.maps.Map）
                strokeColor:  '#00ff00', // 外周色
                strokeOpacity: 1,       // 外周透過度（0: 透明 ⇔ 1:不透明）
                strokeWeight: 5 ,        // 外周太さ（ピクセル）
                clickable : true
            });
            //*****************************
            //イベントハンドラ
            //*****************************
            
            
            google.maps.event.addListener(map, 'click', function(event) {
                Event_MouseClick('left',event.latLng);
            });
            google.maps.event.addListener(map, 'rightclick', function(event) {
                Event_MouseClick('right',event.latLng);
            });
            google.maps.event.addListener(map, 'mousemove', function (event) {
                Event_Mousemove(event.latLng);
            });
            google.maps.event.addListener( map, "idle", function() {
                Event_Idle();
            });
           




//            google.maps.event.addListener( map, 'keydown', function() {
//            alert("aaaaa");
//                //Event_KeyDown(event.keyCode);
//            });
            //setCenter (35.168896, 136.906906,100.0  , '#00ff00')
            //setRadius (35.168896, 136.906906,1000.0 , '#00ff00')
            //alert("aaaaa");
        }
        //*****************************
        //イベントコールバック
        //*****************************
        function RaiseEvent(ev,data){
            var event = new MessageEvent(ev, { 'view': window, 'bubbles': false, 'cancelable': false, 'data': data });
            document.dispatchEvent (event);
        }
        function Event_ObjectClick(button,id,index,location) {
            var data = button + '\t' + id + '\t' + index + '\t' + location.lat() + '\t'+ location.lng();
            RaiseEvent('GoogleMap_ObjectClick',data)
  //           window.external.Googlemap_ObjectClick(button,id,index,location.lat(), location.lng());
        }
         function Event_MouseClick(button,location) {
            var data = button + '\t' + location.lat() + '\t'+ location.lng();
            RaiseEvent('GoogleMap_MouseClick',data)
            //window.external.Googlemap_MouseClick(button,location.lat(), location.lng());
        }
        function Event_Mousemove(location) {
            var data = location.lat() + '\t'+ location.lng();
            RaiseEvent('GoogleMap_MouseMove',data)
            //window.external.Googlemap_MouseMove(location.lat(), location.lng());
        }
        function Event_Idle() {
        //alert("aaaaa");
            RaiseEvent('GoogleMap_Idle',"")
            //window.external.Googlemap_Idle();
        }
        function Event_KeyDown(key) {
        alert("aaaaa");
            window.external.Googlemap_KeyDown(key);
        }
        //***********************************************************
        //****                     ポリゴン
        //***********************************************************
        //***********************
        //■ポリゴンをリストに追加
        //***********************
        function addPolygon(INobjectID,INLatLonsStr,INcolor,INname) {
        
            var objID = parseInt(INobjectID);
            LatLonsStr = String(INLatLonsStr);
            color = String(INcolor);
            name = String(INname);
        
            
//        alert(objectID);
//        alert(LatLonsStr);
//        alert(color);
//        alert(name);
            
            
            var points = new google.maps.MVCArray();
            arr = LatLonsStr.split(",");
            for(var i =0;i<arr.length;i+=2) {
                if(arr[i]!=0 && arr[i+1]!=0){
                    points.push(new google.maps.LatLng(arr[i],arr[i+1]));
                }
            }
            polygon = new google.maps.Polygon({
                path: points,
                fillColor:  color,   // 塗りつぶし色
                fillOpacity: 0.10,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
                map: map,             // 表示させる地図（google.maps.Map）
                strokeColor:  color, // 外周色
                strokeOpacity: 1,       // 外周透過度（0: 透明 ⇔ 1:不透明）
                strokeWeight: 5 ,        // 外周太さ（ピクセル）
                clickable : true , //false ,
                objectID : objID 
            });
            var marker = new google.maps.Marker({
                position: points.getAt(0),
                map: map,
                title: 'objectID=' + objID + '\n名称=' + name
            });
            //alert(polygonPoints.getAt(0));
            var index = polygon_list.push(polygon);
            google.maps.event.addListener(polygon, 'click', function(event) {
                Event_ObjectClick('right',objID,index,event.latLng);
            });
            
            marker_list.push(marker);
            
        }
        function addPolyline(INobjectID,INLatLonsStr,INcolor,InLineType) {
            
            var linetype = parseInt(InLineType);
            var objID = parseInt(INobjectID);
            LatLonsStr = String(INLatLonsStr);
            color = String(INcolor);
            //name = String(INname);

            //alert(INobjectID);                    
            
            var points = new google.maps.MVCArray();
            arr = LatLonsStr.split(",");
            for(var i =0;i<arr.length;i+=2) {
                if(arr[i]!=0 && arr[i+1]!=0){
                    points.push(new google.maps.LatLng(arr[i],arr[i+1]));
                }
            }
            
            var polyline;

            if(linetype == 0 ){
                //実線
                polyline = new google.maps.Polyline({
                    path: points,
                    //fillColor:  color,   // 塗りつぶし色
                    //fillOpacity: 0.10,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
                    map: map,             // 表示させる地図（google.maps.Map）
                    strokeColor:  color, // 外周色
                    strokeOpacity: 1,       // 外周透過度（0: 透明 ⇔ 1:不透明）
                    strokeWeight: 5 ,        // 外周太さ（ピクセル）
                    clickable : false ,
                    objectID : objID 
                });
            } else if(linetype == 1){
                //破線
                var lineSymbol = {
                    path: 'M 0,-1 0,1',
                    strokeOpacity: 1,
                    strokeWeight: 5,
                    scale: 4
                };

                polyline = new google.maps.Polyline({
                    path: points,
                    //fillColor:  color,   // 塗りつぶし色
                    //fillOpacity: 0.10,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
                    map: map,             // 表示させる地図（google.maps.Map）
                    strokeColor:  color, // 外周色
                    strokeOpacity: 0,
                    icons: [{
                        icon: lineSymbol,
                        offset: '0',
                        repeat: '20px'
                    }],
                    strokeWeight: 5 ,        // 外周太さ（ピクセル）
                    clickable : false ,
                    objectID : objID 
                });
            }
       
/*
            var marker = new google.maps.Marker({
                position: points.getAt(0),
                map: map,
                title: 'objectID=' + objID + '\n名称=' + name
            });
            */
            //alert(polylinePoints.getAt(0));
            var index = polyline_list.push(polyline);
            google.maps.event.addListener(polyline, 'click', function(event) {
                Event_ObjectClick('right',objID,index,event.latLng);
            });
            
            marker_list.push(marker);
            
        }
        function addCircle(objectID,rud,Lat,Lng,Color) {
            circle = new google.maps.Circle({
                center: new google.maps.LatLng( Lat, Lng )  ,       // 中心点(google.maps.LatLng)
                fillColor:  Color,   // 塗りつぶし色
                fillOpacity: 1,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
                map: map,             // 表示させる地図（google.maps.Map）
                radius: rud,          // 半径（ｍ）
                strokeColor:  Color, // 外周色
                strokeOpacity: 1,       // 外周透過度（0: 透明 ⇔ 1:不透明）
                strokeWeight: 5 ,        // 外周太さ（ピクセル）
                objectID: objectID , 
                clickable : false 
            });
            circle_list.push(circle);
        }
        
        function addIconMarker(Lat,Lng,icon) {
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng( Lat, Lng )  ,      
                map: map ,
                icon : icon
            });
            icon_marker_list.push(marker);
        }
        
        function addInfoWindowLabel(IN_icon,IN_Lat,IN_Lng,IN_text,IN_cssText,IN_offsetX,IN_offsetY) {
        
            icon    = String(IN_icon);
            Lat     = parseFloat(IN_Lat);
            Lng     = parseFloat(IN_Lng);
            text    = String(IN_text);
            cssText = String(IN_cssText);
            offsetX = parseInt(IN_offsetX);
            offsetY = parseInt(IN_offsetY);
        
         
        
            if ( icon != undefined )  {
                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng( Lat, Lng )  ,      
                    map: map ,
                    icon : icon
                });
                infowindow_label_list.push(marker);
            }
            
            var labelText = document.createElement("div");
            labelText.style.cssText = cssText
            labelText.innerHTML = text//"City Hall, Sechelt<br>British Columbia<br>Canada";
        
	        var myOptions = {
		        content: labelText
//		        ,boxStyle: {
//		            border: "4px solid blue"
//		            ,background: "white"
//		            ,textAlign: "center"
//		            fontSize: "10pt"
//		            ,width: "50px"
//		        }
		        ,disableAutoPan: true
		        ,pixelOffset: new google.maps.Size(offsetX,offsetY)
		        ,position: new google.maps.LatLng(Lat, Lng)
		        ,closeBoxURL: ""
		        ,isHidden: false
		        ,pane: "floatPane"
		        ,enableEventPropagation: true
		        ,draggable: true
	        };
	        var ibLabel = new InfoBox(myOptions);
	        ibLabel.open(map);
            infowindow_label_list.push(ibLabel);
        }

        function removeInfoWindowLabel() {
            infowindow_label_list.forEach(function(InfoBox, idx) {
                InfoBox.setMap(null);
            });
        }  
	         
////            var marker = new google.maps.Marker({
////                map: map,
////                draggable: true,
////	            position: new google.maps.LatLng(35.17115, 136.884343),
////	            visible: true
////	        });
////            var boxText = document.createElement("div");
////            boxText.style.cssText = "border: 1px solid black; margin-top: 8px; background: yellow; padding: 5px;";
////            boxText.innerHTML = "City Hall, Sechelt<br>British Columbia<br>Canada";

////            var myOptions = {
////		        content: boxText
////		        ,setPosition: new google.maps.LatLng(35.17115, 136.884343)
////		        ,disableAutoPan: false
////		        ,maxWidth: 0
////		        ,pixelOffset: new google.maps.Size(-140, 0)
////		        ,zIndex: null
////		        ,boxStyle: { 
////		            background: "url('tipbox.gif') no-repeat"
////		            ,opacity: 0.75
////		            ,width: "280px"
////		        }
////		        ,closeBoxMargin: "10px 2px 2px 2px"
////		        ,closeBoxURL: null//"http://www.google.com/intl/en_us/mapfiles/close.gif"
////		        ,infoBoxClearance: new google.maps.Size(1, 1)
////		        ,isHidden: false
////		        ,pane: "floatPane"
////		        ,enableEventPropagation: false
////	        };
////            
////            var ib = new InfoBox(myOptions);
////	        ib.open(map,this);
        
        
//          var marker = new google.maps.Marker({
//                position: new google.maps.LatLng( Lat, Lng )  ,      
//                map: map ,
//                icon: 'images/beachflag.png'
//            });
//          icon_marker_list.push(marker);
            
//         var image=new google.maps.MarkerImage(icon,
//            new google.maps.Size(100.0, 200.0),
//            new google.maps.Point(0, 0),
//            new google.maps.Point(22.0, 100.0)
//         );
//       icon: 'images/beachflag.png'
//            var contentStr = '123';

    
        function removeIconMarker() {
            icon_marker_list.forEach(function(marker, idx) {
                marker.setMap(null);
            });
        }  
        
        function addCircle2(objectID,rud,Lat,Lng,Color,str) {
//            circle = new google.maps.Circle({
//                center: new google.maps.LatLng( Lat, Lng )  ,       // 中心点(google.maps.LatLng)
//                fillColor:  Color,   // 塗りつぶし色
//                fillOpacity: 1,       // 塗りつぶし透過度（0: 透明 ⇔ 1:不透明）
//                map: map,             // 表示させる地図（google.maps.Map）
//                radius: rud,          // 半径（ｍ）
//                strokeColor:  Color, // 外周色
//                strokeOpacity: 1,       // 外周透過度（0: 透明 ⇔ 1:不透明）
//                strokeWeight: 5 ,        // 外周太さ（ピクセル）
//                objectID: objectID , 
//                
//                clickable : false 
//            });
//            circle_list.push(circle);
            
            
             var marker = new google.maps.Marker({
                position: new google.maps.LatLng( Lat, Lng )  ,      
                map: map,
              	label: {
		        	color: '#550000' ,	// ラベルの色
			        text: 'A' 		// 文字
		        } ,
                title: str
            });
          
            
            marker_list.push(marker);
            
        }
        function removeCircle() {
            circle_list.forEach(function(circle, idx) {
                circle.setMap(null);
            });
        }  
        //***********************
        //■indexのポリゴンを変更
        //***********************
        function changePolygonIndex(index,element,value) {
            polygon_list.getAt(index).set(strokeColor, value);
            //alert(obj.fillColor);
            //obj.fillColor=color;
            //        obj.fillColor='#000000';
            //     polygon_list.setAt(index,obj);
        }
        //***********************
        //■ObjectIDのポリゴンを変更
        //***********************
        function changePolygonObjectID(objectID,element,value) {
            polygon_list.forEach(function(obj, i){
                if (obj.objectID == objectID){
                    obj.set(element,value);
                    return;
                }
            });
            //getAt(index).set(strokeColor, value);
            //alert(obj.fillColor);
            //obj.fillColor=color;
            //        obj.fillColor='#000000';
            //     polygon_list.setAt(index,obj);
        }
        //***********************
        //■編集作業用ポリゴンのポイントをクリアする
        //***********************
        function clearPolygon_editwork() {
            polygonPoints_editwork.clear();
        }
        //***********************
        //■編集作業用ポリゴンに座標を再設定する
        //***********************
        function drawPolygon_editwork(objectID,LatLonsStr,color) {
            polygon_editwork.set("fillColor",color);
            polygonPoints_editwork.clear();
            var arr = LatLonsStr.split(",");
            for(var i =0;i<arr.length;i+=2) {
                if(arr[i]!=0 && arr[i+1]!=0){
                    polygonPoints_editwork.push(new google.maps.LatLng(arr[i],arr[i+1]));
                }
            }
            //alert(polylinePoints.length);
        }
        //***********************
        //■編集作業用ポリゴンリストをクリアする
        //***********************
        function clearPolygon_editwork() {
            polygonPoints_editwork.clear();
        }
        //***********************
        //■全ての確定ポリゴンを地図から消去しリストをクリアする
        //***********************
        function removePolygonListALL() {
            polygon_list.forEach(function(polygon, idx) {
                polygon.setMap(null);
            });
            marker_list.forEach(function(marker, idx) {
                marker.setMap(null);
            });
            polygon_list.clear();
            marker_list.clear();
        }
           function removePolyline() {
            polyline_list.forEach(function(polyline, idx) {
                polyline.setMap(null);
            });
            polyline_list.clear();
        }
        //***********************
        //■全ての確定ポリゴンを地図から消去しリストをクリアする
        //***********************
        function removePolygonIndex(index) {
            polygon_list.forEach(function(polygon, idx) {
                polygon.setMap(null);
            });
            polygon_list.clear();
        }
        //***********************************************************
        //****                     ポリライン
        //***********************************************************
        //***********************
        //■編集作業用ポリラインに座標を再設定する
        //***********************
        function drawPolyline_editwork(objectID,LatLonsStr,color) {
            polyline_editwork.set("fillColor",color);
            polyline_editwork.set("strokeColor",color);
            polylinePoints_editwork.clear();
            //alert(color);
            var arr = LatLonsStr.split(",");
            for(var i =0;i<arr.length;i+=2) {
                if(arr[i]!=0 && arr[i+1]!=0){
                    polylinePoints_editwork.push(new google.maps.LatLng(arr[i],arr[i+1]));
                }
            }
            //alert(polylinePoints.length);
            //alert(polyline_editwork.fillColor);
        }
        //***********************
        //■編集作業用ポリラインをクリアする
        //***********************
        function clearPolyline_editwork() {
            polylinePoints_editwork.clear();
        }
        //***********************************************************
        //****                     その他
        //***********************************************************
        function toCenter(Lat,Lng) {
            map.panTo(new google.maps.LatLng(Lat,Lng)); 
        }
        function zoomIn() {
            var level = map.getZoom();
            level ++;
            map.setZoom(level);
        }
        function zoomOut() {
            var level = map.getZoom();
            if (level != 0){
                level --;
            }
            map.setZoom(level);
        }
    </script>
    
    </head>
    <body onload="init();">
        <div id="map_canvas" style="width:100%; height:100%"></div>
    </body>
</html>
