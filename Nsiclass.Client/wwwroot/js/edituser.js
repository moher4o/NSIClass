$(() => {
    attachEvents();

    function attachEvents() {
        $('#button0').on('click', ButtonZeroAction);
        $('#button1').on('click', ButtonOneAction);
        $('#button2').on('click', ButtonTwoAction);
        $('#button3').on('click', ButtonThreeAction);
        $('#button4').on('click', ButtonFourAction);
        $('#username').on('click', function () { $("#username").css('background-color', '#ffffff'); });
        $('#name').on('click', function () { $("#name").css('background-color', '#ffffff'); });
        $('#email').on('click', function () { $("#email").css('background-color', '#ffffff'); });
        $('#phone').on('click', function () { $("#phone").css('background-color', '#ffffff'); });
        $('#passwordConfirmation').on('click', function () { $("#passwordConfirmation").css('background-color', '#ffffff'); });
        $('#password').on('click', function () { $("#password").css('background-color', '#ffffff'); });
        $('#send').on('click', CheckFieldsChoose);

    }

    function ButtonZeroAction() {
        if ($('#role0').attr('checked')) {
            $('#role0').attr('checked', false);
        }
        else {
            $('#role0').attr('checked', true);
        }
        $('#button0').toggleClass("notselected");
    }
    function ButtonOneAction() {
        if ($('#role1').attr('checked')) {
            $('#role1').attr('checked', false);
            
        }
        else {
            $('#role1').attr('checked', true);
        }
        $('#button1').toggleClass("notselected");
    }
    function ButtonTwoAction() {
        if ($('#role2').attr('checked')) {
            $('#role2').attr('checked', false);

        }
        else {
            $('#role2').attr('checked', true);
        }

        $('#button2').toggleClass("notselected");
    }
    function ButtonThreeAction() {
        if ($('#role3').attr('checked')) {
            $('#role3').attr('checked', false);

        }
        else {
            $('#role3').attr('checked', true);
        }

        $('#button3').toggleClass("notselected");
    }
    function ButtonFourAction() {
        if ($('#role4').attr('checked')) {
            $('#role4').attr('checked', false);

        }
        else {
            $('#role4').attr('checked', true);
        }

        $('#button4').toggleClass("notselected");
    }

    function CheckFieldsChoose() {
        let result = true;
        if ($('#username').val().length === 0) {
            $("#username").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#name').val().length === 0) {
            $("#name").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#email').val().length === 0) {
            $("#email").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#phone').val().length === 0) {
            $("#phone").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#password').val().length === 0) {
            $("#password").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }
        if ($('#passwordConfirmation').val().length === 0) {
            $("#passwordConfirmation").css('background-color', 'rgb(250, 204, 204)');
            result = false;
        }

        if (!($('#role0').attr('checked') || $('#role1').attr('checked') || $('#role2').attr('checked') || $('#role3').attr('checked') || $('#role4').attr('checked'))) {
            result = false;
        }

        if (!result) {
           // alert('Моля попълнете отбелязаните полета и задайте права');
            //notify.showError('Please select value in marked fields');
            notify.showInfo('Създаване...');
        }
        else {
            notify.showInfo('Създаване...');
            $("#realsend").click();
        }
    }

});