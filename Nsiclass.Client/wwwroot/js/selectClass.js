$(() => {
    attachEvents();

    function attachEvents() {
        $('#selectedClass').on('click', function () { $("#selectedClass").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);

    }

    function CheckFieldsChoose() {
        let result = true;

        if ($('#selectedClass :selected').text() === 'Моля изберете...') {
            $("#selectedClass").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }

        if (!result) {
            notify.showError('Моля изберете класификация');
        }
        else {
            $("#realsend").click();
        }
    }

});