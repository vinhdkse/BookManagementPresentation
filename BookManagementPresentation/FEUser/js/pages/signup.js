/*
 *  Document   : signup.js
 *  Author     : pixelcave
 *  Description: Custom javascript code used in Sign Up page
 */

var Signup = function() {

    return {
        init: function() {
            /* Jquery Validation, Check out more examples and documentation at https://github.com/jzaefferer/jquery-validation */
            /* Sign Up form - Initialize Validation */
       
            $('#form-sign-up').validate({
                errorClass: 'help-block animation-slideDown', // You can change the animation class for a different entrance animation - check animations page
                errorElement: 'div',
                errorPlacement: function (error, e) {
                    e.parents('.form-group > div').append(error);
                },
                highlight: function (e) {
                    $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
                    $(e).closest('.help-block').remove();
                },
                success: function (e) {
                    e.closest('.form-group').removeClass('has-success has-error');
                    e.closest('.help-block').remove();
                },
                rules: {
                    'Name': {
                        required: true
                    },
                    'Email': {
                        required: true,
                        email: true
                    },
                    'Password': {
                        required: true,
                        minlength: 5
                    },
                    'Serect': {
                        required: true
                    },
                    'VerifyPassword': {
                        required: true,
                        equalTo: '#Password'
                    },
                    'OldPassword': {
                        required: true
                    }, 'Choosen': {
                        required: true
                    }
                },
                messages: {
                    'Name': {
                        required: 'Xin hãy nhập tên của bạn'
                    },
                    'Email': 'Xin nhập địa chỉ Email',
                    'Choosen': 'Xin chọn cách lấy mật khẩu',
                    'Serect': 'Xin hãy nhập mã bí mật',
                    'Password': {
                        required: 'Xin hãy nhập mật khẩu',
                        minlength: 'Xin hãy nhập mật khẩu từ 5 kí tự'
                    },
                    'VerifyPassword': {
                        required: 'Xin hãy nhập mật khẩu',
                        minlength: 'Xin hãy nhập mật khẩu từ 5 kí tự',
                        equalTo: 'Xin nhập giống password ở trên'
                    },
                    'OldPassword': {
                        required: 'Xin hãy nhập mật khẩu trước đó!'
                    }
                }
            });
           
        }
    };
}();