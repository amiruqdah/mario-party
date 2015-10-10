/*
 * Copyright 2014, Gregg Tavares.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 *     * Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 * copyright notice, this list of conditions and the following disclaimer
 * in the documentation and/or other materials provided with the
 * distribution.
 *     * Neither the name of Gregg Tavares. nor the names of its
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
"use strict";

// Start the main app logic.
requirejs(
  [ 'hft/commonui',
    'hft/gameclient',
    'hft/misc/input',
    'hft/misc/misc',
    'hft/misc/mobilehacks',
    'hft/misc/touch',
    '../bower_components/hft-utils/dist/audio',
    '../bower_components/hft-utils/dist/imageloader',
    '../bower_components/hft-utils/dist/imageutils',
  ], function(
    CommonUI,
    GameClient,
    Input,
    Misc,
    MobileHacks,
    Touch,
    AudioManager,
    ImageLoader,
    ImageUtils) {
  var g_client;
  var g_audioManager;
  var g_clock;
  var g_grid;
  var g_instrument;
  var g_leftRight = 0;
  var g_oldLeftRight = 0;
  var g_jump = false;

  var globals = {
    debug: false,
    orientation: "landscape-primary",
  };
  Misc.applyUrlSettings(globals);
  MobileHacks.fixHeightHack();
  MobileHacks.disableContextMenu();
  MobileHacks.adjustCSSBasedOnPhone([
    {
      test: MobileHacks.isIOS8OrNewerAndiPhone4OrIPhone5,
      styles: {
        ".button": {
          bottom: "100px",
        },
      },
    },
  ]);

  function $(id) {
    return document.getElementById(id);
  }

  var startClient = function() {

    g_client = new GameClient();

    function handleScore() {
    };

    function handleDeath() {
    };

    function handleSetColor(msg) {
      // Look up the canvas for the avatar
      var canvas = $("avatar");

      // Get the size it's displayed by CSS and make
      // it's internal dimensions match.
      var width = canvas.clientWidth;
      var height = canvas.clientHeight;
      canvas.width = width;
      canvas.height = height;

      // Now get a context so we can draw on it.
      var ctx = canvas.getContext("2d");

      // Adjust the avatar's hue, saturuation, value to match whatever color
      // we chose.
      var coloredImage = ImageUtils.adjustHSV(images.idle.img, msg.h, msg.s, msg.v, [msg.rangeMin, msg.rangeMax]);

      // Scale the image larger. We do this manually because the canvas API
      // will blur the image but we want a pixelated image so it looks "old skool"
      var frame = ImageUtils.scaleImage(coloredImage, 128, 128);

      // Draw it in the canvas and stretch it to fill the canvas.
      ctx.drawImage(frame, 0, 0, ctx.canvas.width, ctx.canvas.height);
    };

    g_client.addEventListener('score', handleScore);
    g_client.addEventListener('die', handleDeath);
    g_client.addEventListener('setColor', handleSetColor);

    var sounds = {};
    g_audioManager = new AudioManager(sounds);

    CommonUI.setupStandardControllerUI(g_client, globals);

    function handleLeftRight(pressed, bit) {
      // Clear the bit for this direction and then set it if pressed is true.
      g_leftRight = (g_leftRight & ~bit) | (pressed ? bit : 0);

      // Only send if anything has changed.
      if (g_leftRight != g_oldLeftRight) {
        g_oldLeftRight = g_leftRight;

        // send a direction (-1, 0, or 1)
        g_client.sendCmd('move', {
            dir: (g_leftRight & 1) ? -1 : ((g_leftRight & 2) ? 1 : 0),
        });
      }
    };

    function handleJump(pressed) {
      // Only send it if it has changed
      if (g_jump != pressed) {
        g_jump = pressed;
        g_client.sendCmd('jump', {
            jump: pressed,
        });
      }
    };

    var keys = { };
    keys[Input.cursorKeys.kLeft]  = function(e) { handleLeftRight(e.pressed, 0x1); }
    keys[Input.cursorKeys.kRight] = function(e) { handleLeftRight(e.pressed, 0x2); }
    keys["Z".charCodeAt(0)]       = function(e) { handleJump(e.pressed);           }
    Input.setupKeys(keys);

    Touch.setupButtons({
      inputElement: $("buttons"),  // element receiving the input
      buttons: [
        { element: $("left"),  callback: function(e) { handleLeftRight(e.pressed, 0x1); }, },
        { element: $("right"), callback: function(e) { handleLeftRight(e.pressed, 0x2); }, },
        { element: $("up"),    callback: function(e) { handleJump(e.pressed);           }, },
      ],
    });
  };

  var images = {
    idle:  { url: "images/bird.png", },
  };

  ImageLoader.loadImages(images, startClient);
});


