/*
 *  Document   : ecomCheckout.js
 *  Author     : pixelcave
 *  Description: Custom javascript code used in Checkout page
 */

var EcomCheckout = function () {

    return {
        init: function () {
            /*
             *  Jquery Wizard, Check out more examples and documentation at http://www.thecodemine.org
             */

            /* Initialize Checkout Wizard */
            var checkoutWizard = $('#checkout-wizard');

            checkoutWizard
                .formwizard({
                    disableUIStyles: true,
                    validationEnabled: true,
                    validationOptions: {
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
                            'Phone': {
                                required: true,
                                number: true,
                                minlength: 10,
                                maxlength: 10
                            },
                            'Province': {
                                required: true
                            },
                            'City': {
                                required: true
                            },
                            'District': {
                                required: true
                            },
                            'Address': {
                                required: true
                            },
                            'Payment': {
                                required: true
                            },
                            'Card': {
                                required: true,
                                minlength: 19,
                                maxlength: 19
                            },
                            'CVC': {
                                required: true,
                                number: true,
                                minlength: 3,
                                maxlength: 3
                            },
                            'Month': {
                                required: true

                            },
                            'Year': {
                                required: true
                            },
                            'CardNumber': {
                                required: true,
                                minlength: 14,
                                maxlength: 14
                            },
                            'Serial': {
                                required: true,
                                minlength: 14,
                                maxlength: 14
                            }
                        },
                        messages: {
                            'Phone': {
                                required: 'Xin hãy nhập số điện thoại gồm 10 số như: 0902818547'
                            },
                            'Province': 'Xin hãy chọn tỉnh',
                            'Card': 'Xin hãy nhập mã thẻ chính xác',
                            'CVC': 'Xin hãy nhập CVC chính xác',
                            'Month': 'Xin hãy chọn tháng',
                            'Year': 'Xin hãy chọn năm',
                            'CardNumber': 'Xin hãy nhập 12 số',
                            'Serial': 'Xin hãy nhập 12 số',
                            'City': {
                                required: 'Xin hãy chọn thành phố'
                            },
                            'District': {
                                required: 'Xin hãy chọn huyện'
                            },
                            'Address': {
                                required: 'Xin hãy nhập địa chỉ của bạn'
                            },
                            'Payment': {
                                required: 'Xin hãy chọn phương thức thanh toán'
                            }
                        }
                    },
                    inDuration: 0,
                    outDuration: 0,
                    textBack: 'Previous Step',
                    textNext: 'Next Step',
                    textSubmit: 'Confirm Order'
                });
            $('.checkout-steps a').on('click', function () {
                var gotostep = $(this).data('gotostep');

                checkoutWizard
                    .formwizard('show', gotostep);
            });

        }
    };
}();