/*
 *  Document   : formsValidation.js
 *  Author     : pixelcave
 *  Description: Custom javascript code used in Forms Validation page
 */

var FormsValidation = function () {

    return {
        init: function () {
            /*
             *  Jquery Validation, Check out more examples and documentation at https://github.com/jzaefferer/jquery-validation
             */

            /* Initialize Form Validation */
            $('#form-validation').validate({
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
                    // You can use the following if you would like to highlight with green color the input after successful validation!
                    e.closest('.form-group').removeClass('has-success has-error'); // e.closest('.form-group').removeClass('has-success has-error').addClass('has-success');
                    e.closest('.help-block').remove();
                },
                rules: {
                    Image: {
                        required: true
                    },
                    val_email: {
                        required: true,
                        email: true
                    },
                    Password: {
                        required: true,
                        minlength: 5
                    },
                    VeriPassword: {
                        required: true,
                        equalTo: '#val_password'
                    },
                    Name: {
                        required: true
                    },
                    Publisher: {
                        required: true
                    },
                    PublishDate: {
                        required: true
                    },
                    Price: {
                        required: true
                    },
                    Quantity: {
                        required: true
                    },
                    val_terms: {
                        required: true
                    },
                    masked_date3: {
                        required: true,
                        minlength: 13
                    },
                    val_date: {
                        date: true
                    },
                    val_id: {
                        required: true
                    },
                    val_name: {
                        required: true
                    },
                    val_size: {
                        required: true,
                        digits: true
                    },
                    val_location: {
                        required: true
                    },
                    val_description: {
                        required: true
                    },
                    val_img: {
                        required: true
                    },
                    val_address: {
                        required: true
                    },
                    val_rank: {
                        required: true,
                        digits: true
                    },
                    val_quantity: {
                        required: true,
                        digits: true
                    }
                },
                messages: {
                    Image: {
                        required: 'Xin chọn 1 bức ảnh'
                    },
                    val_email: 'Please enter a valid email address',
                    Password: {
                        required: 'Xin hãy nhập mật khẩu',
                        minlength: 'Mật khẩu của bạn phải có độ dài lớn hơn 5 kí tự'
                    },
                    VeriPassword: {
                        required: 'Xin hãy nhập mật khẩu',
                        minlength: 'Mật khẩu của bạn phải có độ dài lớn hơn 5 kí tự',
                        equalTo: 'Xin nhập giống với mật khẩu trên'
                    },
                    val_size: {
                        required: 'Please provid a size',
                        digits: 'Please enter only digits!'
                    },
                    Name: 'Xin hãy nhập tên',
                    Publisher: 'Xin hãy nhập nhà sản xuất',
                    PublishDate: 'Xin hãy nhập ngày sản xuất',
                    Price: 'Xin hãy nhập số!',
                    Quantity: 'Xin hãy nhập số!',
                    val_terms: 'You must agree to the service terms!',
                    masked_date3:
                            {
                                required: 'Please enter a valid date',
                                minlength: 'Your date must be in format yyyy/mm/dd hh:mm:ss'
                            },
                    val_id: 'Please provide a id',
                    val_name: 'Please provide a name',
                    val_location: 'Please provid a location',
                    val_description: 'Please provide a description',
                    val_img: 'Please choose banner',
                    val_address: {
                        required: 'Please provid a address'
                    },
                    val_rank: {
                        required: 'Please provid a rank',
                        digits: 'Please enter only digits!'
                    },
                    val_quantity: {
                        required: 'Please provid a quantity',
                        digits: 'Please enter only digits!'
                    }
                }
            });
            $('#form-user').validate({
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
                    // You can use the following if you would like to highlight with green color the input after successful validation!
                    e.closest('.form-group').removeClass('has-success has-error'); // e.closest('.form-group').removeClass('has-success has-error').addClass('has-success');
                    e.closest('.help-block').remove();
                },
                rules: {
                    Image: {
                        required: true
                    },
                    val_email: {
                        required: true,
                        email: true
                    },
                    Password: {
                        required: true,
                        minlength: 5
                    },
                    VeriPassword: {
                        required: true,
                        equalTo: '#val_password'
                    },
                    Name: {
                        required: true
                    },
                    Publisher: {
                        required: true
                    },
                    PublishDate: {
                        required: true
                    },
                    Price: {
                        required: true
                    },
                    Quantity: {
                        required: true
                    },
                    val_terms: {
                        required: true
                    },
                    masked_date3: {
                        required: true,
                        minlength: 13
                    },
                    val_date: {
                        date: true
                    },
                    val_id: {
                        required: true
                    },
                    val_name: {
                        required: true
                    },
                    val_size: {
                        required: true,
                        digits: true
                    },
                    val_location: {
                        required: true
                    },
                    val_description: {
                        required: true
                    },
                    val_img: {
                        required: true
                    },
                    val_address: {
                        required: true
                    },
                    val_rank: {
                        required: true,
                        digits: true
                    },
                    val_quantity: {
                        required: true,
                        digits: true
                    }
                },
                messages: {
                    Image: {
                        required: 'Xin chọn 1 bức ảnh'
                    },
                    val_email: 'Please enter a valid email address',
                    Password: {
                        required: 'Xin hãy nhập mật khẩu',
                        minlength: 'Mật khẩu của bạn phải có độ dài lớn hơn 5 kí tự'
                    },
                    VeriPassword: {
                        required: 'Xin hãy nhập mật khẩu',
                        minlength: 'Mật khẩu của bạn phải có độ dài lớn hơn 5 kí tự',
                        equalTo: 'Xin nhập giống với mật khẩu trên'
                    },
                    val_size: {
                        required: 'Please provid a size',
                        digits: 'Please enter only digits!'
                    },
                    Name: 'Xin hãy nhập tên',
                    Publisher: 'Xin hãy nhập nhà sản xuất',
                    PublishDate: 'Xin hãy nhập ngày sản xuất',
                    Price: 'Xin hãy nhập số!',
                    Quantity: 'Xin hãy nhập số!',
                    val_terms: 'You must agree to the service terms!',
                    masked_date3:
                    {
                        required: 'Please enter a valid date',
                        minlength: 'Your date must be in format yyyy/mm/dd hh:mm:ss'
                    },
                    val_id: 'Please provide a id',
                    val_name: 'Please provide a name',
                    val_location: 'Please provid a location',
                    val_description: 'Please provide a description',
                    val_img: 'Please choose banner',
                    val_address: {
                        required: 'Please provid a address'
                    },
                    val_rank: {
                        required: 'Please provid a rank',
                        digits: 'Please enter only digits!'
                    },
                    val_quantity: {
                        required: 'Please provid a quantity',
                        digits: 'Please enter only digits!'
                    }
                }
            });
            // Initialize Masked Inputs
            // a - Represents an alpha character (A-Z,a-z)
            // 9 - Represents a numeric character (0-9)
            // * - Represents an alphanumeric character (A-Z,a-z,0-9)
            $('#masked_date').mask('99/99/9999');
            $('#masked_date2').mask('9999-99-99');
            $('#masked_date3').mask('9999-99-99 99:99:99');
            $('#masked_phone').mask('(999) 999-9999');
            $('#masked_phone_ext').mask('(999) 999-9999? x99999');
            $('#masked_taxid').mask('99-9999999');
            $('#masked_ssn').mask('999-99-9999');
            $('#masked_pkey').mask('a*-999-a999');
            
        }
    };
}();