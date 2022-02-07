

$(function () {
    "use strict";
    $('.input-file').each(function () {
        var $input = $(this),
            $label = $input.next('.js-labelFile'),
            labelVal = $label.html();

        $input.on('change', function (element) {
            $("#fail").hide();
            $("#success").hide();   

            var fileName = '';
            if (element.target.value) fileName = element.target.value.split('\\').pop();
            fileName ?
                $label.addClass('has-file').find('.js-fileName').html(fileName) :
                $label.removeClass('has-file').html(labelVal);

           
            upload($(this).prop('files')[0]);

            $("#form").hide();
        });
    });

});

function upload(file) {

    var formData = new FormData();
    formData.append('file', file);
    formData.append('password', $("#password").val());// $("#password").Value();

    var h = window.location.origin ? window.location.origin : window.location.protocol + "//" + window.location.host;
    $.ajax({
        type: "POST",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        url: h + "/Upload",
        // beforeSend: function () { $("#loading1").show(); },
        success: function (data) {
            
            if (data == 'True') {
                $("#success").show();           

            } else {
                $("#fail").show();
               // alert('Файл не был загружен!');
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            //$("#img1").hide();
            $("#fail").show();
            alert('Ошибка ' + textStatus);

        }
       


    });
    
}
