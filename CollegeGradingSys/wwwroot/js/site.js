$(function () {
    var placeholderElement = $('#modal-placeholder');    
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');        
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');           
            $("#CreateCourseParentId").attr("disabled", "disabled");           
            //============
            if ($("#EditCourseIsSubCourse").val() == 'False') {
                $("#EditCourseParentId").prop('selectedIndex', 0);
                $("#EditCourseParentId").attr("disabled", "disabled");
            }
            else {
                $("#EditCourseParentId").removeAttr("disabled");
            }

            //==============
            if ($("#EditStStatusForCourse").val() == 2 || $("#EditStStatusForCourse").val() == 3) {
                $("#EditCourseGrade").removeAttr("disabled");
            }
            else {
                $("#EditCourseGrade").val("");
                $("#EditCourseGrade").attr("disabled", "disabled");

            }
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        
        event.preventDefault();       
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
            
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            
            if (isValid) {                
                placeholderElement.find('.modal').modal('hide');
            }
        });
    });
    placeholderElement.on('change', '#CreateCourseIsSubCourse', function () {       
       
        if ($("#CreateCourseIsSubCourse").val() == 'False') {            
            $("#CreateCourseParentId").prop('selectedIndex', 0);
            $("#CreateCourseParentId").attr("disabled", "disabled");            
        }
        else {
            $("#CreateCourseParentId").removeAttr("disabled");                     
        }
    });
    placeholderElement.on('change', '#EditCourseIsSubCourse', function () {

        if ($("#EditCourseIsSubCourse").val() == 'False') {
            $("#EditCourseParentId").prop('selectedIndex', 0);
            $("#EditCourseParentId").attr("disabled", "disabled");
        }
        else {
            $("#EditCourseParentId").removeAttr("disabled");
        }
    });
    

    placeholderElement.on('change', '#EditStStatusForCourse', function () {
       
        if ($("#EditStStatusForCourse").val() == 2 || $("#EditStStatusForCourse").val() == 3 ) {
            $("#EditCourseGrade").removeAttr("disabled");
        }
        else {
            $("#EditCourseGrade").val("");
            $("#EditCourseGrade").attr("disabled", "disabled");
           
        }
    });

    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {       
        return new bootstrap.Popover(popoverTriggerEl)
    })

    var popover = new bootstrap.Popover(document.querySelector('.popover-dismiss'), {
        trigger: 'focus'
    })

});