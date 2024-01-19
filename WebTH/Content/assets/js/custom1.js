jQuery(document).ready(function ($) {
    "use strict";







    initFavorite();






    /* 

    5. Init Favorite

    */

    function initFavorite() {
        if ($('.favorite').length) {
            var favs = $('.favorite');

            favs.each(function () {
                var fav = $(this);
                var active = false;
                if (fav.hasClass('active')) {
                    active = true;
                }

                fav.on('click', function () {
                    var id = $(this).data('id');

                    if (active) {
                        fav.removeClass('active');
                        active = false;
                        DeleteWishlist(id);
                    }
                    else {
                        fav.addClass('active');
                        active = true;
                        AddWishlist(id);
                    }
                });
            });
        }
    }

    function DeleteWishlist(id) {
        $.ajax({
            url: '/wishlist/PostDeleteWishlist',
            type: 'POST',
            data: { ProductId: id },
            success: function (res) {
                if (res.Success == false) {
                    alert(res.Message);
                }
            }
        });
    }

    function AddWishlist(id) {
        $.ajax({
            url: '/wishlist/postwishlist',
            type: 'POST',
            data: { ProductId: id },
            success: function (res) {
                if (res.Success == false) {
                    alert(res.Message);
                }
            }
        });
    }

});