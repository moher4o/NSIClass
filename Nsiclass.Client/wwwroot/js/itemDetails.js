$(() => {
    attachEvents();
    function attachEvents() {
        $('#otherCode').on('click', function () { $("#otherCode").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);

    }

    function CheckFieldsChoose() {
        let result = true;
        if ($('#otherCode').val().length === 0) {
            $("#otherCode").css('background-color', 'rgb(250, 204, 204)');
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