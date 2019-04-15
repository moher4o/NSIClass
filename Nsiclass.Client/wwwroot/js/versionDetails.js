$(() => {
    attachEvents();
    
    function attachEvents() {
        $('#IdCode').on('click', function () { $("#validFrom").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);
        
    }

    function CheckFieldsChoose() {
        let result = true;
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