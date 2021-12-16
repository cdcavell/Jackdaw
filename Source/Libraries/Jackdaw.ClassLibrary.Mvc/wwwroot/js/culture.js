/*!
  * Licensed under MIT (https://github.com/cdcavell/Jackdaw/blob/main/LICENSE)
  *
  *  Revisions:
  *  ----------------------------------------------------------------------------------------------------
  * | Contributor           | Build   | Revison Date | Description
  * |-----------------------|---------|--------------|-----------------------------------------------------
  * | Christopher D. Cavell | 0.0.0.1 | 12/16/2021   | Initial Development
  *
  */

$(document).ready(function () {

    $('.culture-link a').click(function (e) {

        e.preventDefault();

        var url = '/Home/SetCulture';
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var model = { Culture: $(this).attr('href') };

        ajaxPost(url, token, model)
            .then(function (data) {
                console.debug(data);
                window.location = new URL(window.location);
            })
            .catch((error) => {
                ajaxError(error)
            });

    });

});


