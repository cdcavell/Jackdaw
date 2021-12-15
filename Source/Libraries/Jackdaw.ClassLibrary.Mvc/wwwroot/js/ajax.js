/*!
  * Licensed under MIT (https://github.com/cdcavell/Jackdaw/blob/main/LICENSE)
  *
  *  Revisions:
  *  ----------------------------------------------------------------------------------------------------
  * | Contributor           | Build   | Revison Date | Description
  * |-----------------------|---------|--------------|-----------------------------------------------------
  * | Christopher D. Cavell | 0.0.0.1 | 12/15/2021   | Initial Development
  *
  */

(function ($) {

    'use strict'

    /* Example Post Call:
    *
    * ajaxPost(url, token, model)
    *     .then(function (data) {
    *         console.log(data);
    *     })
    *     .catch((error) => {
    *         console.log(error)
    *     });
    */

    var ajaxPost = function(url, token, model) {
        return new Promise((resolve, reject) => {
            $('.preloader').delay(500).fadeIn('slow', async function () {
                console.debug('-- AJax POST: ' + url + ' token: ' + token);
                console.debug(model);
                $('.preloader-icon').fadeIn(400);
                $.ajax({
                    url: url,
                    method: "POST",
                    async: true,
                    cache: false,
                    dataType: "json",
                    data: {
                        __RequestVerificationToken: token,
                        model: model
                    },
                    success: function (data) {
                        console.debug(data);
                        resolve(data);
                    },
                    complete: function () {
                        console.debug('-- AJax Complete');
                        $('.preloader-icon').fadeOut(400);
                        $('.preloader').delay(500).fadeOut('slow');
                    },
                    error: function (error) {
                        console.error(error);
                        reject(error)
                        $('.preloader-icon').fadeOut(400);
                        $('.preloader').delay(500).fadeOut('slow');
                    }
                });
            });
        })
    }


    /* Example Get Call:
    *
    * ajaxGet(url)
    *     .then(function (data) {
    *         console.log(data);
    *     })
    *     .catch((error) => {
    *         console.log(error)
    *     });
    */
    var ajaxGet = function(url) {
        return new Promise((resolve, reject) => {
            $('.preloader').delay(500).fadeIn('slow', async function () {
                console.debug('-- AJax GET: ' + url + ' token: ' + token);
                console.debug(model);
                $('.preloader-icon').fadeIn(400);
                $.ajax({
                    url: url,
                    method: "GET",
                    async: true,
                    cache: false,
                    dataType: "json",
                    success: function (data) {
                        console.debug(data);
                        resolve(data);
                    },
                    complete: function () {
                        console.debug('-- AJax Complete');
                        $('.preloader-icon').fadeOut(400);
                        $('.preloader').delay(500).fadeOut('slow');
                    },
                    error: function (error) {
                        console.error(error);
                        reject(error)
                        $('.preloader-icon').fadeOut(400);
                        $('.preloader').delay(500).fadeOut('slow');
                    }
                });
            });
        })
    }

})(jQuery);