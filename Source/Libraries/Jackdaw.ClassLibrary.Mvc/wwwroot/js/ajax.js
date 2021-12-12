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

function ajaxPost(url, token, model) {
    return new Promise((resolve, reject) => {
        $('#Processing').fadeIn(async function () {
            console.debug('-- AJax POST: ' + url + ' token: ' + token);
            console.debug(model);
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
                    $('#Processing').fadeOut();
                },
                error: function (error) {
                    console.error(error);
                    reject(error)
                    $('#Processing').fadeOut();
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
function ajaxGet(url) {
    return new Promise((resolve, reject) => {
        $('#Processing').fadeIn(async function () {
            console.debug('-- AJax GET: ' + url + ' token: ' + token);
            console.debug(model);
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
                    $('#Processing').fadeOut();
                },
                error: function (error) {
                    console.error(error);
                    reject(error)
                    $('#Processing').fadeOut();
                }
            });
        });
    })
}