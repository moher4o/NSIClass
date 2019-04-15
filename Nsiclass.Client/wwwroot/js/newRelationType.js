$(() => {
     attachEvents();

    function attachEvents() {
        $('#SourceCode').on('click', function () { $("#SourceCode").css('background-color', '#ffffff'); });
        $('#SourceVersionCode').on('click', function () { $("#SourceVersionCode").css('background-color', '#ffffff'); });
        $('#description').on('click', function () { $("#description").css('background-color', '#ffffff'); });
        $('#DestCode').on('click', function () { $("#DestCode").css('background-color', '#ffffff'); });
        $('#DestVersionCode').on('click', function () { $("#DestVersionCode").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);
    }


    function CheckFieldsChoose() {
        let result = true;
        if ($('#validFrom').val().length === 0) {
            $("#validFrom").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }

        if ($('#SourceCode :selected').text() === 'Моля изберете...') {
            $("#SourceCode").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#SourceVersionCode :selected').text() === 'Моля изберете...') {
            $("#SourceVersionCode").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }

        if ($('#DestCode :selected').text() === 'Моля изберете...') {
            $("#DestCode").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#DestVersionCode :selected').text() === 'Моля изберете...') {
            $("#DestVersionCode").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }


        if ($('#description').val().length === 0) {
            $("#description").css('background-color', 'rgb(250, 204, 204)');
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