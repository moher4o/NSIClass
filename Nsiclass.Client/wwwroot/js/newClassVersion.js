$(() => {
    attachEvents();

    function attachEvents() {
        $('#IdCode').on('click', function () { $("#IdCode").css('background-color', '#ffffff'); });
        $('#version').on('click', function () { $("#version").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);

    }

    function CheckFieldsChoose() {
        let result = true;

        if ($('#IdCode :selected').text() === 'Моля изберете...') {
            $("#IdCode").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#version').val().length === 0) {
            $("#version").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }

        if (!result) {
             notify.showError('Моля попълнете отбелязаните полета');
        }
        else {
            notify.showInfo('Създаване...');
            $("#realsend").click();
        }
    }

});