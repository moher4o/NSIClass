$(() => {
    attachEvents();

    function attachEvents() {
        $('#IdCode').on('click', function () { $("#validFrom").css('background-color', '#ffffff'); });
        $('#description').on('click', function () { $("#description").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);

        let placeholderElement = $('#modal-placeholder');
        $('button[data-toggle="ajax-modal"]').click(function (event) {
            let url = $(this).data('url') + '?relId=' + $(this).data('relid');
            
            $.get(url).done(function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            });
        });

        placeholderElement.on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();

            let form = $(this).parents('.modal').find('form');
            let actionUrl = form.attr('action');
            let dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {
                let newBody = $('.modal-body', data);
                placeholderElement.find('.modal-body').replaceWith(newBody);
                let isValid = newBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeholderElement.find('.modal').modal('hide');
                }
                location.reload();
            });
        });

    }

    function CheckFieldsChoose() {
        let result = true;
        if ($('#description').val().length === 0) {
            $("#description").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#validFrom').val().length === 0) {
            $("#validFrom").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }

        if (!result) {
            notify.showError('Моля попълнете отбелязаните полета');
        }
        else {
            notify.showInfo('Запазване...');
            $("#realsend").click();
        }
    }


});