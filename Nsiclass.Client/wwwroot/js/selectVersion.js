$(() => {
    attachEvents();

    function attachEvents() {
        $('#selectedVersion').on('click', function () { $("#selectedVersion").css('background-color', '#ffffff'); });
        $('#selectedVersion').on('change', function () { $("#versionName").text('Версия: ' + $(this).val()); });

        $('#send').on('click', CheckFieldsChoose);
    }

    function CheckFieldsChoose() {
        let result = true;

        if ($('#selectedVersion :selected').text() === 'Моля изберете...') {
            $("#selectedVersion").css('background-color', 'rgb(250, 204, 204)');
            notify.showError('Моля изберете версия');
            result = false;
        }

        if ($('#fileName').text() === 'Не е избран файл с елементи') {
            notify.showError('Моля изберете файл');
            result = false;
        }
 
        if (result) {
            $("#realsend").click();
        }
    }

});