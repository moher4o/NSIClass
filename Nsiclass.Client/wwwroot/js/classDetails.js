$(() => {
    attachEvents();

    function attachEvents() {
        $('#IdCode').on('click', function () { $("#IdCode").css('background-color', '#ffffff'); });
        $('#name').on('click', function () { $("#name").css('background-color', '#ffffff'); });
        //$('#nameEng').on('click', function () { $("#nameEng").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);

    }

    function CheckFieldsChoose() {
        let result = true;
        if ($('#IdCode').val().length === 0) {
            $("#IdCode").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#name').val().length === 0) {
            $("#name").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        //if ($('#nameEng').val().length === 0) {
        //    $("#nameEng").css('background-color', 'rgb(250, 204, 204)');
        //    result = false;
        //}

        if (!result) {
            // alert('Моля попълнете отбелязаните полета и задайте права');
            notify.showError('Моля попълнете отбелязаните полета');
            //notify.showInfo('Създаване...');
        }
        else {
            notify.showInfo('Запазване...');
            $("#realsend").click();
        }
    }

});