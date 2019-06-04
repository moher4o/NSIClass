$(() => {
    attachEvents();

    function attachEvents() {
        $('#relName').on('click', function () { $("#relName").css('background-color', '#ffffff'); });
        $('#relName').on('change', function () { $("#relNameSelected").text('Релация: ' + $(this).val()); });
        $('#btnHelp').on('click', ShowHelp);
        $('#send').on('click', CheckFieldsChoose);
    }

    function ShowHelp() {
        $('#helpImage').show();
    }


    function CheckFieldsChoose() {
        let result = true;

        if ($('#relName :selected').text() === 'Моля изберете...') {
            $("#relName").css('background-color', 'rgb(250, 204, 204)');
            notify.showError('Моля изберете релация');
            result = false;
        }

        if ($('#fileName').text() === 'Не е избран файл с елементи') {
            notify.showError('Моля изберете файл');
            result = false;
        }

        if (result) {
            //LoadProgressBar();
            $('#warning').css("color", "green");
            $('#warning').text("Зареждането на файла приключи. Моля, изчакайте обработката!");
            $("#realsend").click();
            
        }
    }

    function LoadProgressBar() {
        var progressbar = $("#progressbar-5");
        var progressLabel = $(".progress-label");
        progressbar.show();
        $("#progressbar-5").progressbar({
            //value: false,
            change: function () {
                progressLabel.text(
                    progressbar.progressbar("value") + "%");  // Showing the progress increment value in progress bar
            },
            complete: function () {
                //progressLabel.text("Зареждането на файла приключи. Моля, изчакайте обработката!");
                $('#warning').css("color", "green");
                $('#warning').text("Зареждането на файла приключи. Моля, изчакайте обработката!");
                //progressbar.progressbar("value", 0);  //Reinitialize the progress bar value 0
                //progressLabel.text("");
                //progressbar.hide(); //Hiding the progress bar
                $("#realsend").click();
            }
        });
        
        
        function progress() {
            var val = progressbar.progressbar("value") || 0;
            progressbar.progressbar("value", val + 1);
            if (val < 99) {
                setTimeout(progress, 15);
            }
        }
        setTimeout(progress, 100);
    }

});