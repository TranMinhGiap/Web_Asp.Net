$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var quantityTmp = $('#quantity_value').text();
        if (quantityTmp != '') {
            quantity = parseInt(quantityTmp);
        }
        $.ajax({
            url: "/ShoppingCart/AddToCart",
            type: "POST",
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            },
            error: function (xhr) {
                if (xhr.status === 401) {
                    const currentUrl = window.location.pathname + window.location.search;
                    window.location.href = "/Account/Login?returnUrl=" + encodeURIComponent(currentUrl);
                } else {
                    alert("Đã xảy ra lỗi khi thêm sản phẩm.");
                }
            }
        });
    });

    //
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        if (confirm("Bạn có muốn xóa sản phẩm không?")) {
            var id = $(this).data('id');
            $.ajax({
                url: "/ShoppingCart/Delete",
                type: "POST",
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                        alert(rs.msg);
                        LoadCart();
                    }
                }
            });
        }
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        if (confirm("Bạn có muốn xóa hết sản phẩm trong giỏ hàng không ?")) {
            DeleteAll();
        }
    });

    //$('body').on('click', '.btnUpdate', function (e) {
    //    e.preventDefault();
    //    var id = $(this).data("id");
    //    var quantity = $('#Quantity_' + id).val();
    //    Update(id, quantity);
    //});

    $('body').on('input', '.input_product_quantity', function () {
        var $input = $(this);
        var id = $input.data("id");
        var quantity = parseInt($input.val());

        if (isNaN(quantity) || quantity < 1) {
            alert('Số lượng phải lớn hơn 0');
            $input.val(1);
            return;
        }

        clearTimeout($input.data('timeout'));

        $input.data('timeout', setTimeout(function () {
            Update(id, quantity);
        }, 500));
    });
    //
});

function ShowCount() {
    $.ajax({
        url: "/ShoppingCart/ShowCount",
        type: "GET",
        success: function (rs) {
            $('#checkout_items').html(rs.Count);                                            
        }
    });
}
function DeleteAll() {
    $.ajax({
        url: "/ShoppingCart/DeleteAll",
        type: "POST",
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
            ShowCount();
        }
    });
}
function LoadCart() {
    $.ajax({
        url: "/ShoppingCart/PartialItemCart",
        type: "GET",
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}

function Update(id, quantity) {
    $.ajax({
        url: "/ShoppingCart/Update",
        type: "POST",
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}