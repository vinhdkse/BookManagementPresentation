/*
 *  Document   : contact.js
 *  Author     : pixelcave
 *  Description: Custom javascript code used in Contact page
 */

var Contact = function() {

    return {
        init: function() {
            /*
             * With Gmaps.js, Check out examples and documentation at http://hpneo.github.io/gmaps/examples.html
             */

            // Initialize map

            /*
             *  Jquery Validation, Check out more examples and documentation at https://github.com/jzaefferer/jquery-validation
             */

            /* Initialize Form Validation */
            $('#form-contact').validate({
                errorClass: 'help-block animation-slideDown', // You can change the animation class for a different entrance animation
                errorElement: 'div',
                errorPlacement: function(error, e) {
                    e.parents('.form-group').append(error);
                },
                highlight: function(e) {
                    $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
                    $(e).closest('.help-block').remove();
                },
                success: function(e) {
                    // You can use the following if you would like to highlight with green color the input after successful validation!
                    e.closest('.form-group').removeClass('has-success has-error'); // e.closest('.form-group').removeClass('has-success has-error').addClass('has-success');
                    e.closest('.help-block').remove();
                },
                rules: {
                    'Name': {
                        required: true,
                        minlength: 3
                    },
                    'Email': {
                        required: true,
                        email: true
                    },
                    'Content': {
                        required: true,
                        minlength: 5
                    }
                },
                messages: {
                    'Name': {
                        required: 'Hãy cho chúng tôi biết tên bạn!',
                        minlength: 'Hãy cho chúng tôi biết tên đầy đủ bạn!'
                    },
                    'Email': 'Hãy cho chúng tôi biết Email chính xác của bạn!',
                    'Content': {
                        required: 'Hãy cho chúng tôi biết ý kiến của bạn!',
                        minlength: 'Hãy cho chúng tôi biết ý kiến của bạn!'
                    }
                }
            });
        }
    };
}();